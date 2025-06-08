using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

using System.Collections;
using UnityEngine.UI;
using YG;


public class Hero : MonoBehaviour
{
    public float flyUpForce = 5f; // Сила полета вверх
    public float flyRightForce = 3f; // Сила полета вправо
    public float rotationSpeed = 100f; // Скорость поворота
    [SerializeField] private GameObject heroPrefab; // Префаб героя
    [SerializeField] private float pushForceMagnitude = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private CameraController cameraController;
    private CameraController cameraControllerComponent;
    private CheckpointManagerScript checkpointManager;

    private bool flag = true;
    public bool isFrozen = false;
    private bool isTouchingFirstGear = false;
    private bool isTouchingSecondGear = false;
    public int numberOfClonesAlive;

    public GameObject effectClone;
    public GameObject effectDie;
    public GameObject bonusBigPrefab;
    public GameObject bonusSmallPrefab;
    public GameObject bonusClonePrefab;
    public GameObject bonusClonePrefab4;
    public GameObject bombPrefab;
    public GameObject eggPrefab;
    public Animator animator;
    public RuntimeAnimatorController[] skinControllers; // Массив с контроллерами
    public Image canvasImage;
    public float obstacleCheckDistance = 1f;
    public float magnetRadius = 1160f;
    public float magnetForce = 1160f;
    public float explosionForce = 200f;
    public float explosionRadius = 200f;
    public float fadeDuration = 1.0f;
    public float pushForce = 10f;  // Сила, с которой персонаж будет отталкиваться от бомбы
    public bool canPerformActions = false;
    public bool isPlayerInside = false;
    [Header("Player Animation Settings")]

    List<GameObject> objectsToReset = new List<GameObject>();
    List<BonusInfo> bonusInfos = new List<BonusInfo>();

    public float followDistance = 10f; // Максимальное расстояние, на котором персонаж будет "догонять" другого
    public float followAcceleration = 5f; // Ускорение при догонке

    public GameObject birdPrefab; // Префаб птицы
    public float birdSpeed = 5f; // Скорость полёта птицы
    public float birdFlyDuration = 2f; // Продолжительность полета птицы
    public float birdSpawnOffsetX = -5f; // Смещение по X для точки спавна птицы
    public float birdSpawnOffsetY = -5f; // Смещение по Y для точки спавна птицы
    public float birdFlightAngle = 45f; // Угол полета птицы в градусах
    public AudioClip touchSound; // Звук при касании триггера
    private AudioSource audioSource; // Источник звука
    public bool canMoveAfterDeath = true;
    private float moveBlockDuration = 1f; // Длительность блокировки движения после смерти
    private float moveBlockTimer = 0f;  // Таймер для отсчета времени

    private CloneManager cloneManager;
    private bool isDead = false;

    public AudioClip deathSound; // Звук смерти
    public AudioSource audioSourceDeath; // Источник звука

    public Image fadeImage;
    public Image[] levelProgressImages;  // Ссылка на 11 изображений прогресса
    public Image[] levelCircles;          // Ссылка на кружки уровня
    public Color completedColor = new Color(1, 1, 1, 1);  // Цвет для пройденных уровней (белый)
    public Color currentColor = new Color(0, 0, 1, 1);    // Цвет для текущего уровня (синий)
    public Color uncompletedColor = new Color(0.02f, 0.02f, 0.02f, 1); // Цвет для непройденных уровней (серый)
    private static bool isAnimationPlaying = false;

    public AudioSource audioSourceFly;
    public AudioClip[] soundClipsFly;
    private bool isFlying = false;
    private int selectedSkinIndex;
    private int highestLevelReached;
    private bool levelTransitionRequested = false; // Флаг для проверки, была ли нажата кнопка
    public Button nextLevelButton;
    public CanvasGroup levelEndPanel;
    private static bool isSucking = false;

    public Button buttonPause;
    public struct BonusInfo
    {
        public Vector3 position;
        public string type; // Тип бонуса ("bigBonus" или "smallBonus")

    }

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        checkpointManager = GameObject.FindObjectOfType<CheckpointManagerScript>();
        audioSource = gameObject.AddComponent<AudioSource>();

        cloneManager = FindObjectOfType<CloneManager>();
        cloneManager.AddClone(); // Увеличиваем счетчик при создании нового клона


        if (YandexGame.SDKEnabled)
        {
            // Загружаем данные с облака
            selectedSkinIndex = YandexGame.savesData.pickSkin;
            highestLevelReached = YandexGame.savesData.HighestLevelReached;
            animator.runtimeAnimatorController = skinControllers[selectedSkinIndex];
        }
        nextLevelButton.onClick.AddListener(OnNextLevelClicked);
    }



    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        cameraController = Camera.main.GetComponent<CameraController>();
        cameraControllerComponent = FindObjectOfType<CameraController>();
    }

    public void Update()
    {
        numberOfClonesAlive = GameObject.FindGameObjectsWithTag("Character").Length;

        // Если игрок находится внутри триггера и нажимает пробел
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Space))
            canPerformActions = true;



        if (canPerformActions)
        {
            // Если персонаж не заморожен и таймер блокировки движения не истёк
            if (!canMoveAfterDeath)
            {
                moveBlockTimer += Time.deltaTime;
                if (moveBlockTimer >= moveBlockDuration)
                {
                    canMoveAfterDeath = true;  // Разрешаем движение после 5 секунд
                    moveBlockTimer = 0f;  // Сбрасываем таймер
                }
                else
                {
                    rb.velocity = Vector2.zero; // Останавливаем движение
                    return;  // Выход из Update, если движение блокировано
                }
            }


            if (levelTransitionRequested)
            {
                YandexGame.FullscreenShow();
                levelTransitionRequested = false;
                // Подписываемся на событие закрытия рекламы
                YandexGame.CloseFullAdEvent += OnAdClosed;
            }


            // Проверка нажатия или удержания клавиши пробела
            if (Input.GetKey(KeyCode.Space))
            {
                Fly();
                animator.SetBool("Fly", true);
                // Если игрок только начал удерживать пробел и звук не воспроизводится
                if (!isFlying)
                {
                    PlayRandomSound();
                    isFlying = true; // Устанавливаем флаг, чтобы не воспроизводить звук повторно
                }


            }
            else
            {
                animator.SetBool("Fly", false);
                isFlying = false; // Сбрасываем флаг, когда пробел отпущен
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }


        }
        if (isFrozen)
        {
            rb.velocity = Vector2.zero; // Остановить движение персонажа
            cameraController.transform.position = checkpointManager.GetLastCameraPosition(); // Переместить камеру к чекпоинту
            transform.position = checkpointManager.GetLastCheckpointPosition(); // Переместить персонажа к чекпоинту
            transform.localScale = checkpointManager.GetLastPlayerScale(); // Восстановить размер персонажа
            isFrozen = false;
        }

    }

    private void OnAdClosed()
    {
        // Отписываемся от события, чтобы избежать повторных вызовов
        YandexGame.CloseFullAdEvent -= OnAdClosed;
        StartCoroutine(TransitionToNextLevel());
        levelTransitionRequested = false;
    }


    public void OnNextLevelClicked()
    {
        // Устанавливаем флаг, что кнопка нажата
        levelTransitionRequested = true;

        levelEndPanel.alpha = 0;           // Устанавливаем видимость
        levelEndPanel.interactable = false; // Включаем взаимодействие
        levelEndPanel.blocksRaycasts = false; // Включаем блокировку кликов

        Time.timeScale = 1f;
    }

    private IEnumerator TransitionToNextLevel()
    {
        if (!isAnimationPlaying)
        {
            isAnimationPlaying = true;
            Scene currentScene = SceneManager.GetActiveScene();
            string nextSceneName;

            int currentLevelNumber = int.Parse(currentScene.name.Replace("lvl", ""));

            // Сохраняем прогресс
            int highestLevelReached = YandexGame.savesData.HighestLevelReached;
            if (currentLevelNumber >= highestLevelReached)
            {
                YandexGame.savesData.HighestLevelReached = highestLevelReached + 1;
                YandexGame.SaveProgress();
            }

            // Рассчитываем следующий уровень
            int nextSceneIndex = currentScene.buildIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
                nextSceneName = "lvl0"; // Возвращаемся к первому уровню
            }
            else
            {
                nextSceneName = $"lvl{nextSceneIndex}";
            }

            // Переход на следующий уровень
            yield return StartCoroutine(FadeAndLoadScene(nextSceneName, nextSceneIndex));
        }
    }

    void PlayRandomSound()
    {
        if (soundClipsFly.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, soundClipsFly.Length);
            audioSourceFly.clip = soundClipsFly[randomIndex];

            // Устанавливаем случайный питч в диапазоне от 0.8 до 1.2
            audioSourceFly.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            audioSourceFly.volume = UnityEngine.Random.Range(0.25f, 0.65f);

            //// Остановить текущий звук, если он воспроизводится
            //if (audioSourceFly.isPlaying)
            //{
            //    audioSourceFly.Stop();
            //}
            audioSourceFly.Play();
        }
    }


    // Функция для возвращения персонажа к чекпоинту
    void KillAllClonesExceptOne()
    {
        // Находим все объекты с компонентом Hero (это могут быть все персонажи, включая клонов)
        Hero[] heroes = FindObjectsOfType<Hero>();

        // Получаем последний живой объект (клона или персонажа), которого мы не будем уничтожать
        Hero lastHero = heroes[heroes.Length - 1];

        foreach (Hero hero in heroes)
        {
            // Если это не тот объект, которого мы оставляем живым, уничтожаем его
            if (hero != lastHero)
            {
                hero.Die(); // Вызываем метод смерти для каждого клона
            }
        }
    }

    // Метод для уничтожения персонажа (включая клонов)
    public void Die()
    {
        if (isDead) return; // Если уже мёртв, не выполняем код снова
        Destroy(gameObject); // Уничтожаем объект
        isDead = true; // Помечаем клона как мёртвого
        Instantiate(effectDie, transform.position, Quaternion.identity); // Эффект смерти
        cloneManager.RemoveClone(); // Уменьшаем счетчик клонов
        audioSourceDeath.PlayOneShot(deathSound); // Проигрываем звук смерти
        
    }

    // рестарт для ui кнопки
    public void Restart()
    {
        if (checkpointManager.IsCheckpointSet())
        {
            KillAllClonesExceptOne();
            StartCoroutine(FadeAndDelay());
        }
        else
        {
            StartCoroutine(FadeAndDelay());
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }


    void Fly()
    {
        // Устанавливаем постоянное значение скорости
        Vector2 flyVelocity = new Vector2(flyRightForce, flyUpForce);
        rb.velocity = flyVelocity;
    }


    private void FixedUpdate()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "bigBonus" || collider.tag == "smallBonus" || collider.tag == "YskBonus" || collider.tag == "SlowBonus")
            {
                Vector2 direction = transform.position - collider.transform.position;
                collider.GetComponent<Rigidbody2D>().AddForce(direction.normalized * magnetForce);
            }
        }



    }


    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("DieZone") || collision.gameObject.CompareTag("DieZoneStatic") || collision.gameObject.CompareTag("DieZoneDynamic"))
        {
            Instantiate(effectDie, transform.position, Quaternion.identity);
            cloneManager.RemoveClone(); // Уменьшаем счетчик при смерти
            if (isDead) return; // Если уже мёртв, не выполняем код снова
            isDead = true; // Помечаем клона как мёртвого


            if (cloneManager.numberOfClonesAlive == 0)
            {
                cloneManager.AddClone();
                StartCoroutine(FadeAndDelay());
                audioSourceDeath.PlayOneShot(deathSound);
                isDead = false;
            }
            else
            {
                StartCoroutine(HandleDeath());
            }
        }



        if (collision.gameObject.CompareTag("Gear"))
        {
            if (!isTouchingFirstGear)
            {
                isTouchingFirstGear = true;
                Debug.Log("isTouchingFirstGear");

            }
            else if (!isTouchingSecondGear)
            {
                isTouchingSecondGear = true;
                Debug.Log("isTouchingSectGear");
            }

            // Если персонаж касается двух шестеренок, он умирает
            if (isTouchingFirstGear && isTouchingSecondGear)
            {
                cloneManager.RemoveClone(); // Уменьшаем счетчик при смерти

                if (cloneManager.numberOfClonesAlive == 0)
                {
                    cloneManager.AddClone();
                    StartCoroutine(FadeAndDelay());
                    audioSourceDeath.PlayOneShot(deathSound);
                }
                else
                {
                    StartCoroutine(HandleDeath());
                }
            }
        }

    }

    // Корутин для обработки смерти с задержкой
    private IEnumerator HandleDeath()
    {
        // Воспроизводим звук смерти
        audioSourceDeath.PlayOneShot(deathSound);

        // Выключаем коллайдер
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Выключаем спрайт (или объект с компонентом SpriteRenderer)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        // Ждем, пока не закончится звук
        yield return new WaitForSeconds(deathSound.length);
        // Уничтожаем объект
        Destroy(gameObject);
    }
    void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Gear"))
        {
            if (isTouchingFirstGear)
            {
                isTouchingFirstGear = false;
            }
            else if (isTouchingSecondGear)
            {
                isTouchingSecondGear = false;
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            Debug.Log("Player save!");
            checkpointManager.SetCheckpoint(collision.transform.position, cameraController.transform.position, transform.localScale); // Сохраняем и позицию персонажа, и позицию камеры
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }



        if (collision.gameObject.tag == "Bomb")
        {

            BonusPosition bonus = collision.GetComponent<BonusPosition>();
            if (bonus != null)
            {
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);
            }
            Instantiate(effectClone, transform.position, Quaternion.identity);

            Transform[] heroTransforms = FindObjectsOfType<Hero>().Select(hero => hero.transform).ToArray();
            foreach (Transform heroTransform in heroTransforms)
            {
                // Передаем информацию о подобранном бонусе каждому клону
                Hero cloneCharacter = heroTransform.GetComponent<Hero>();
                if (cloneCharacter != null)
                {
                    cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);
                }
            }

            // Отталкиваем персонажа
            PushBackFromExplosion(collision.transform.position);

        }



        else if (collision.tag == "DieBird")
        {
            BonusPosition bonus = collision.GetComponent<BonusPosition>();
            if (bonus != null)
            {
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);
            }
            Instantiate(effectClone, transform.position, Quaternion.identity);


            Transform[] heroTransforms = FindObjectsOfType<Hero>().Select(hero => hero.transform).ToArray();
            foreach (Transform heroTransform in heroTransforms)
            {
                // Передаем информацию о подобранном бонусе каждому клону
                Hero cloneCharacter = heroTransform.GetComponent<Hero>();
                if (cloneCharacter != null)
                {
                    cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);
                }
            }

            if (touchSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(touchSound);
            }
            else
            {
                Debug.LogWarning("AudioSource или AudioClip не назначены.");
            }

            Destroy(collision.gameObject);
            // Появление и движение птицы
            SpawnAndMoveBird();
        }



        if (collision.tag == "bigBonus" || collision.tag == "smallBonus")
        {
            BonusPosition bonus = collision.GetComponent<BonusPosition>();
            if (bonus != null && !bonus.isPicked) // Проверяем, был ли бонус подобран
            {
                bonus.isPicked = true; // Блокируем бонус
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);
                Instantiate(effectClone, transform.position, Quaternion.identity);

                Transform[] heroTransforms = FindObjectsOfType<Hero>().Select(hero => hero.transform).ToArray();
                foreach (Transform heroTransform in heroTransforms)
                {
                    // Увеличиваем или уменьшаем масштаб в зависимости от типа бонуса
                    float scaleMultiplier = collision.tag == "bigBonus" ? 1.5f : 0.7f;
                    heroTransform.localScale *= scaleMultiplier;

                    // Передаем информацию о подобранном бонусе каждому клону
                    Hero cloneCharacter = heroTransform.GetComponent<Hero>();
                    if (cloneCharacter != null)
                    {
                        cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);
                    }
                }

                Destroy(collision.gameObject);
            }
        }

        if (collision.tag == "cloneBonus" || collision.tag == "cloneBonus4")
        {
            BonusPosition bonus = collision.GetComponent<BonusPosition>();
            if (bonus != null && !bonus.isPicked) // Проверяем, был ли бонус подобран
            {
                bonus.isPicked = true; // Блокируем бонус
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);

                Instantiate(effectClone, transform.position, Quaternion.identity);

                int numberOfHeroes = (collision.tag == "cloneBonus") ? 2 : 4;

                // Получаем все существующие персонажи
                Hero[] cloneCharacters = FindObjectsOfType<Hero>();

                for (int i = 0; i < numberOfHeroes; i++)
                {
                    GameObject hero = Instantiate(heroPrefab, transform.position + (i % 2 == 0 ? Vector3.right : Vector3.left) * 2f, Quaternion.identity);
                    hero.transform.localScale = transform.localScale;
                    numberOfClonesAlive++;

                    // Получаем скрипт клонированного персонажа
                    Hero cloneCharacter = hero.GetComponent<Hero>();
                    if (cloneCharacter != null)
                    {
                        // Передаем информацию о подобранных бонусах каждому клону
                        cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);

                        // Обновляем информацию о подобранных бонусах для всех уже существующих клонов
                        foreach (Hero existingClone in cloneCharacters)
                        {
                            if (existingClone != cloneCharacter)
                            {
                                existingClone.bonusInfos = new List<BonusInfo>(bonusInfos);
                            }
                        }
                    }
                }

                Destroy(collision.gameObject);
            }
        }

        if (collision.CompareTag("trigger1"))
        {
            GameObject dropstone = GameObject.FindGameObjectWithTag("dropstone");
            GameObject dropstonedop = GameObject.FindGameObjectWithTag("dropstone_dop_object");
            objectsToReset.Add(dropstone);
            objectsToReset.Add(dropstonedop);


            if (dropstone != null)
            {
                Rigidbody2D dropstoneRigidbody = dropstone.GetComponent<Rigidbody2D>();
                Rigidbody2D dropstoneDopRigidbody = dropstonedop.GetComponent<Rigidbody2D>();
                if (dropstoneRigidbody != null)
                {
                    dropstoneRigidbody.simulated = true;
                    dropstoneDopRigidbody.simulated = true;
                }
            }

        }

        if (collision.CompareTag("trigger2"))
        {
            // Включаем симуляцию Rigidbody для всех объектов с тегом "dropstone2"
            GameObject[] dropstones = GameObject.FindGameObjectsWithTag("dropstone2");
            foreach (GameObject dropstone in dropstones)
            {
                Rigidbody2D dropstoneRigidbody = dropstone.GetComponent<Rigidbody2D>();
                if (dropstoneRigidbody != null)
                {
                    dropstoneRigidbody.simulated = true;
                }
            }
            if (flag)
            {
                flag = false;
                cameraController.ShakeCamera();
            }
            flag = true;
        }


        if (collision.CompareTag("trigger4"))
        {
            StartCoroutine(MoveToTriggerCenter(collision));
        }

        if (collision.CompareTag("trigger5"))
        {
            cameraController.shakeMagnitude = 0.4f;
            cameraController.shakeDuration = 0.25f;
            cameraController.ShakeCamera();
        }


        if (collision.CompareTag("trigger6")) // триггер для самого начала
        {
            isPlayerInside = true;
        }

        if (collision.CompareTag("trigger7")) // триггер для самого начала и автоматического нажатия на пробел
        {
            isPlayerInside = true;
            canPerformActions = true;
        }
    }



    private void PushBackFromExplosion(Vector3 explosionPosition)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 pushDirection = (transform.position - explosionPosition).normalized;
            rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }

    private void SpawnAndMoveBird()
    {
        // Рассчитываем точку спавна относительно персонажа с заданным смещением
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += birdSpawnOffsetX; // Смещаем по X влево
        spawnPosition.y += birdSpawnOffsetY; // Смещаем по Y вниз
        spawnPosition.z = 0; // Устанавливаем z на 0, чтобы птица была в том же слое, что и персонаж

        // Спавним птицу в рассчитанной позиции
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);

        // Начинаем корутину для движения птицы
        StartCoroutine(MoveBird(bird));
    }

    private IEnumerator MoveBird(GameObject bird)
    {
        // Устанавливаем начальное время
        float startTime = Time.time;

        // Переводим угол в радианы
        float angleRad = birdFlightAngle * Mathf.Deg2Rad;

        // Рассчитываем направление движения на основе угла
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0).normalized;

        while (Time.time - startTime < birdFlyDuration)
        {
            // Двигаем птицу по направлению
            bird.transform.Translate(direction * birdSpeed * Time.deltaTime);

            yield return null;
        }

        // Уничтожаем птицу после завершения полета
        Destroy(bird);
    }





    private IEnumerator MoveToTriggerCenter(Collider2D collision)
    {


        // Получаем центр триггера
        Vector3 triggerCenter = collision.bounds.center;

        // Определяем начальные позиции персонажей
        Transform[] clones = GameObject.FindGameObjectsWithTag("Character").Select(obj => obj.transform).ToArray();
        Vector3[] startPositions = clones.Select(clone => clone.position).ToArray();

        // Определяем целевые позиции для полета к центру триггера
        Vector3[] targetPositions = Enumerable.Repeat(triggerCenter, clones.Length).ToArray();

        // Плавное перемещение к целевым позициям с использованием синусоидальной функции
        float duration = 1f; // Уменьшаем продолжительность полета до 1 секунды
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Вычисляем прогресс перемещения
            float t = elapsedTime / duration;

            // Изменяем позиции по оси Y, используя синусоидальную функцию, умноженную на 4 для еще большего ускорения
            float yOffset = Mathf.Sin(t * Mathf.PI * 4f); // Значение синуса изменяется быстрее

            for (int i = 0; i < clones.Length; i++)
            {
                Vector3 newPosition = Vector3.Lerp(startPositions[i], targetPositions[i], t) + Vector3.up * yOffset;
                clones[i].position = newPosition;
            }

            // Увеличиваем прошедшее время
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // После завершения перемещения устанавливаем точные позиции центра триггера для всех персонажей
        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].position = targetPositions[i];
        }

        yield return StartCoroutine(FadeCanvasSprite(sprite, 1f, 0f, 0)); // Запускаем анимацию исчезновения

        // Отключаем коллайдеры персонажей, если нужно
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Отключаем коллайдер персонажа
        }


        // Проверяем, выполняется ли уже засасывание
        if (isSucking)
        {
            yield break; // Если уже выполняется засасывание, просто выходим
        }

        // Устанавливаем флаг, что засасывание началось
        isSucking = true;



        if (levelEndPanel != null)
        {
            levelEndPanel.alpha = 0;           // Устанавливаем видимость
            levelEndPanel.interactable = false; // Включаем взаимодействие
            levelEndPanel.blocksRaycasts = false; // Включаем блокировку кликов
        }

        yield return StartCoroutine(FadeInPanel());
        AudioListener.volume = 0;

        // После завершения засасывания сбрасываем флаг
        isSucking = false;
    }

    private IEnumerator FadeInPanel()
    {
        // Сначала устанавливаем альфа-канал панели в 0
        CanvasGroup canvasGroup = levelEndPanel.GetComponent<CanvasGroup>(); // Предполагается, что панель использует CanvasGroup для управления альфа-каналом

        buttonPause.GetComponent<Image>().enabled = false;

        // Отключаем кнопку, чтобы она не реагировала на события
        buttonPause.interactable = false;


        canvasGroup.alpha = 0f;           // Устанавливаем изначально невидимой
        levelEndPanel.interactable = false; // Панель неактивна
        levelEndPanel.blocksRaycasts = false; // Блокируем клики

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f); // Плавно изменяем альфа
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Устанавливаем финальный альфа
        levelEndPanel.interactable = true;  // Включаем взаимодействие
        levelEndPanel.blocksRaycasts = true; // Разблокируем клики

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;


        Time.timeScale = 0f;

    }


    private IEnumerator FadeAndLoadScene(string sceneName, int currentLevel)
    {
        isAnimationPlaying = true; // Устанавливаем флаг, что анимация идет

        // Запускаем анимацию затемнения
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        // Показываем прогресс
        yield return StartCoroutine(ShowLevelProgress(currentLevel));

        // Синхронная загрузка сцены
        SceneManager.LoadScene(sceneName); // Загрузить сцену синхронно

        // После загрузки сцены, скрываем анимацию затемнения
        //fadeImage.gameObject.SetActive(false);
        isAnimationPlaying = false; // Завершаем анимацию
    }




    private IEnumerator AnimateColorTransition(Image image, Color targetColor, float duration)
    {
        Color startColor = image.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;  // Убедитесь, что цвет установлен в конечное значение
    }

    private IEnumerator UpdateLevelProgress(int currentLevel)
    {
        float transitionDuration = 0.5f; // Время анимации изменения цвета

        // Обновление цветов кружков
        for (int i = 0; i < levelCircles.Length; i++)
        {
            Color targetColor;

            if (i < currentLevel - 1)
            {
                // Уровни, которые пройдены
                targetColor = completedColor;
            }
            else if (i == currentLevel - 1)
            {
                // Текущий уровень
                targetColor = currentColor;
            }
            else
            {
                // Не пройденные уровни
                targetColor = uncompletedColor;
            }

            // Плавное изменение цвета
            yield return StartCoroutine(AnimateColorTransition(levelCircles[i], targetColor, transitionDuration));
        }
    }


    private IEnumerator ShowLevelProgress(int currentLevel)
    {
        // Показ изображений прогресса
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 0;
            img.color = color;
        }

        // Появление изображений прогресса
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Время для появления изображений

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            foreach (Image img in levelProgressImages)
            {
                Color color = img.color;
                color.a = alpha;
                img.color = color;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедитесь, что альфа-канал установлен в 1
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 1;
            img.color = color;
        }

        // Обновите прогресс уровней с плавным изменением цвета
        yield return StartCoroutine(UpdateLevelProgress(currentLevel));
    }








    // ВСЕ что связано с чекпоинтами
    public IEnumerator FadeAndDelay()
    {
        Debug.Log("ВСЕ что связано с чекпоинтами ");
        cameraController.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Отключаем коллайдер персонажа
        }

        yield return StartCoroutine(FadeCanvasSprite(sprite, 1f, 0f, 0)); // Запускаем анимацию исчезновения
        yield return StartCoroutine(FadeCanvasImage(canvasImage, 0f, 1f, fadeDuration)); // Запускаем анимацию исчезновения
        yield return new WaitForSeconds(1f);
        isFrozen = true;
        canMoveAfterDeath = false; // Блокируем движение после смерти
        yield return new WaitForSeconds(1f);

        if (collider != null)
        {
            collider.enabled = true; // Включаем коллайдер персонажа

        }


        foreach (GameObject obj in objectsToReset)
        {
            if (obj.CompareTag("dropstone"))
            {
                ResetObjectProperties(obj, new Vector3(142.97f, 84.4f, 0f), false, Vector3.zero);
            }
            else if (obj.CompareTag("dropstone_dop_object"))
            {
                ResetObjectProperties(obj, obj.transform.position, true, new Vector3(0f, 0f, -0.02184f));
            }

        }
        RestoreBonuses();


        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "lvl5")
        {
            yield return new WaitForSeconds(1f);
            // Перезагружаем сцену
            SceneManager.LoadScene(currentSceneName);
        }

        if (currentSceneName == "lvl7" && !checkpointManager.IsCheckpointSet())
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(currentSceneName);
        }

        if (currentSceneName == "lvl8" && !checkpointManager.IsCheckpointSet())
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(currentSceneName);
        }

        else
        {
            yield return StartCoroutine(FadeCanvasSprite(sprite, 0f, 1f, 0)); // Запускаем анимацию появления
            yield return StartCoroutine(FadeCanvasImage(canvasImage, 1f, 0f, fadeDuration)); // Запускаем анимацию исчезновения
            cameraControllerComponent.enabled = true;

        }

    }


    void ResetObjectProperties(GameObject obj, Vector3 position, bool resetRotation, Vector3 rotationEulerAngles)
    {
        Rigidbody2D objRigidbody = obj.GetComponent<Rigidbody2D>();
        if (objRigidbody != null)
        {
            objRigidbody.simulated = false; // Отключить физическую симуляцию
        }

        obj.transform.position = position; // Сбросить положение объекта

        if (resetRotation)
        {
            obj.transform.rotation = Quaternion.Euler(rotationEulerAngles); // Сбросить поворот объекта
        }
    }


    IEnumerator FadeCanvasImage(Image image, float startAlpha, float targetAlpha, float duration)
    {
        Color color = image.color;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            color.a = alpha;
            image.color = color;
            yield return null;
        }
    }

    IEnumerator FadeCanvasSprite(SpriteRenderer spriteRenderer, float startAlpha, float targetAlpha, float duration)
    {
        Color color = spriteRenderer.color;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }
        // Устанавливаем окончательное значение альфа-канала
        color.a = targetAlpha;
        spriteRenderer.color = color;
    }


    // подбор и рестарт бонусов при возвращении
    void SavePickedUpBonusPosition(string bonusType, Vector3 initialPosition)
    {
        bool bonusPickedUp = bonusInfos.Any(info => info.type == bonusType && info.position == initialPosition);
        if (!bonusPickedUp)
        {
            BonusInfo info;
            info.position = initialPosition; // Сохраняем первоначальную позицию
            info.type = bonusType;
            bonusInfos.Add(info);
        }
    }

    // Метод для восстановления подобранных бонусов после смерти персонажа
    void RestoreBonuses()
    {
        foreach (BonusInfo info in bonusInfos.ToList()) // Проходим по копии списка для безопасного удаления
        {
            // Проверяем, был ли бонус подобран ранее
            if (bonusInfos.Any(i => i.position == info.position && i.type == info.type))
            {
                // Создаем бонус на сохраненной позиции в зависимости от его типа
                if (info.type == "bigBonus")
                {
                    Instantiate(bonusBigPrefab, info.position, Quaternion.identity);
                }
                else if (info.type == "smallBonus")
                {
                    Instantiate(bonusSmallPrefab, info.position, Quaternion.identity);
                }
                else if (info.type == "cloneBonus")
                {
                    Instantiate(bonusClonePrefab, info.position, Quaternion.identity);
                }
                else if (info.type == "cloneBonus4")
                {
                    Instantiate(bonusClonePrefab4, info.position, Quaternion.identity);
                }
                else if (info.type == "Bomb")
                {
                    Instantiate(bombPrefab, info.position, Quaternion.identity);
                }
                else if (info.type == "DieBird")
                {
                    Instantiate(eggPrefab, info.position, Quaternion.identity);
                }
            }
        }

        // Очистка списка информации о бонусах после восстановления
        bonusInfos.Clear();
    }
}