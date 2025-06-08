using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

using System.Collections;
using UnityEngine.UI;
using YG;


public class Hero : MonoBehaviour
{
    public float flyUpForce = 5f; // ���� ������ �����
    public float flyRightForce = 3f; // ���� ������ ������
    public float rotationSpeed = 100f; // �������� ��������
    [SerializeField] private GameObject heroPrefab; // ������ �����
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
    public RuntimeAnimatorController[] skinControllers; // ������ � �������������
    public Image canvasImage;
    public float obstacleCheckDistance = 1f;
    public float magnetRadius = 1160f;
    public float magnetForce = 1160f;
    public float explosionForce = 200f;
    public float explosionRadius = 200f;
    public float fadeDuration = 1.0f;
    public float pushForce = 10f;  // ����, � ������� �������� ����� ������������� �� �����
    public bool canPerformActions = false;
    public bool isPlayerInside = false;
    [Header("Player Animation Settings")]

    List<GameObject> objectsToReset = new List<GameObject>();
    List<BonusInfo> bonusInfos = new List<BonusInfo>();

    public float followDistance = 10f; // ������������ ����������, �� ������� �������� ����� "��������" �������
    public float followAcceleration = 5f; // ��������� ��� �������

    public GameObject birdPrefab; // ������ �����
    public float birdSpeed = 5f; // �������� ����� �����
    public float birdFlyDuration = 2f; // ����������������� ������ �����
    public float birdSpawnOffsetX = -5f; // �������� �� X ��� ����� ������ �����
    public float birdSpawnOffsetY = -5f; // �������� �� Y ��� ����� ������ �����
    public float birdFlightAngle = 45f; // ���� ������ ����� � ��������
    public AudioClip touchSound; // ���� ��� ������� ��������
    private AudioSource audioSource; // �������� �����
    public bool canMoveAfterDeath = true;
    private float moveBlockDuration = 1f; // ������������ ���������� �������� ����� ������
    private float moveBlockTimer = 0f;  // ������ ��� ������� �������

    private CloneManager cloneManager;
    private bool isDead = false;

    public AudioClip deathSound; // ���� ������
    public AudioSource audioSourceDeath; // �������� �����

    public Image fadeImage;
    public Image[] levelProgressImages;  // ������ �� 11 ����������� ���������
    public Image[] levelCircles;          // ������ �� ������ ������
    public Color completedColor = new Color(1, 1, 1, 1);  // ���� ��� ���������� ������� (�����)
    public Color currentColor = new Color(0, 0, 1, 1);    // ���� ��� �������� ������ (�����)
    public Color uncompletedColor = new Color(0.02f, 0.02f, 0.02f, 1); // ���� ��� ������������ ������� (�����)
    private static bool isAnimationPlaying = false;

    public AudioSource audioSourceFly;
    public AudioClip[] soundClipsFly;
    private bool isFlying = false;
    private int selectedSkinIndex;
    private int highestLevelReached;
    private bool levelTransitionRequested = false; // ���� ��� ��������, ���� �� ������ ������
    public Button nextLevelButton;
    public CanvasGroup levelEndPanel;
    private static bool isSucking = false;

    public Button buttonPause;
    public struct BonusInfo
    {
        public Vector3 position;
        public string type; // ��� ������ ("bigBonus" ��� "smallBonus")

    }

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        checkpointManager = GameObject.FindObjectOfType<CheckpointManagerScript>();
        audioSource = gameObject.AddComponent<AudioSource>();

        cloneManager = FindObjectOfType<CloneManager>();
        cloneManager.AddClone(); // ����������� ������� ��� �������� ������ �����


        if (YandexGame.SDKEnabled)
        {
            // ��������� ������ � ������
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

        // ���� ����� ��������� ������ �������� � �������� ������
        if (isPlayerInside && Input.GetKeyDown(KeyCode.Space))
            canPerformActions = true;



        if (canPerformActions)
        {
            // ���� �������� �� ��������� � ������ ���������� �������� �� ����
            if (!canMoveAfterDeath)
            {
                moveBlockTimer += Time.deltaTime;
                if (moveBlockTimer >= moveBlockDuration)
                {
                    canMoveAfterDeath = true;  // ��������� �������� ����� 5 ������
                    moveBlockTimer = 0f;  // ���������� ������
                }
                else
                {
                    rb.velocity = Vector2.zero; // ������������� ��������
                    return;  // ����� �� Update, ���� �������� �����������
                }
            }


            if (levelTransitionRequested)
            {
                YandexGame.FullscreenShow();
                levelTransitionRequested = false;
                // ������������� �� ������� �������� �������
                YandexGame.CloseFullAdEvent += OnAdClosed;
            }


            // �������� ������� ��� ��������� ������� �������
            if (Input.GetKey(KeyCode.Space))
            {
                Fly();
                animator.SetBool("Fly", true);
                // ���� ����� ������ ����� ���������� ������ � ���� �� ���������������
                if (!isFlying)
                {
                    PlayRandomSound();
                    isFlying = true; // ������������� ����, ����� �� �������������� ���� ��������
                }


            }
            else
            {
                animator.SetBool("Fly", false);
                isFlying = false; // ���������� ����, ����� ������ �������
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }


        }
        if (isFrozen)
        {
            rb.velocity = Vector2.zero; // ���������� �������� ���������
            cameraController.transform.position = checkpointManager.GetLastCameraPosition(); // ����������� ������ � ���������
            transform.position = checkpointManager.GetLastCheckpointPosition(); // ����������� ��������� � ���������
            transform.localScale = checkpointManager.GetLastPlayerScale(); // ������������ ������ ���������
            isFrozen = false;
        }

    }

    private void OnAdClosed()
    {
        // ������������ �� �������, ����� �������� ��������� �������
        YandexGame.CloseFullAdEvent -= OnAdClosed;
        StartCoroutine(TransitionToNextLevel());
        levelTransitionRequested = false;
    }


    public void OnNextLevelClicked()
    {
        // ������������� ����, ��� ������ ������
        levelTransitionRequested = true;

        levelEndPanel.alpha = 0;           // ������������� ���������
        levelEndPanel.interactable = false; // �������� ��������������
        levelEndPanel.blocksRaycasts = false; // �������� ���������� ������

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

            // ��������� ��������
            int highestLevelReached = YandexGame.savesData.HighestLevelReached;
            if (currentLevelNumber >= highestLevelReached)
            {
                YandexGame.savesData.HighestLevelReached = highestLevelReached + 1;
                YandexGame.SaveProgress();
            }

            // ������������ ��������� �������
            int nextSceneIndex = currentScene.buildIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
                nextSceneName = "lvl0"; // ������������ � ������� ������
            }
            else
            {
                nextSceneName = $"lvl{nextSceneIndex}";
            }

            // ������� �� ��������� �������
            yield return StartCoroutine(FadeAndLoadScene(nextSceneName, nextSceneIndex));
        }
    }

    void PlayRandomSound()
    {
        if (soundClipsFly.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, soundClipsFly.Length);
            audioSourceFly.clip = soundClipsFly[randomIndex];

            // ������������� ��������� ���� � ��������� �� 0.8 �� 1.2
            audioSourceFly.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            audioSourceFly.volume = UnityEngine.Random.Range(0.25f, 0.65f);

            //// ���������� ������� ����, ���� �� ���������������
            //if (audioSourceFly.isPlaying)
            //{
            //    audioSourceFly.Stop();
            //}
            audioSourceFly.Play();
        }
    }


    // ������� ��� ����������� ��������� � ���������
    void KillAllClonesExceptOne()
    {
        // ������� ��� ������� � ����������� Hero (��� ����� ���� ��� ���������, ������� ������)
        Hero[] heroes = FindObjectsOfType<Hero>();

        // �������� ��������� ����� ������ (����� ��� ���������), �������� �� �� ����� ����������
        Hero lastHero = heroes[heroes.Length - 1];

        foreach (Hero hero in heroes)
        {
            // ���� ��� �� ��� ������, �������� �� ��������� �����, ���������� ���
            if (hero != lastHero)
            {
                hero.Die(); // �������� ����� ������ ��� ������� �����
            }
        }
    }

    // ����� ��� ����������� ��������� (������� ������)
    public void Die()
    {
        if (isDead) return; // ���� ��� ����, �� ��������� ��� �����
        Destroy(gameObject); // ���������� ������
        isDead = true; // �������� ����� ��� �������
        Instantiate(effectDie, transform.position, Quaternion.identity); // ������ ������
        cloneManager.RemoveClone(); // ��������� ������� ������
        audioSourceDeath.PlayOneShot(deathSound); // ����������� ���� ������
        
    }

    // ������� ��� ui ������
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
        // ������������� ���������� �������� ��������
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
            cloneManager.RemoveClone(); // ��������� ������� ��� ������
            if (isDead) return; // ���� ��� ����, �� ��������� ��� �����
            isDead = true; // �������� ����� ��� �������


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

            // ���� �������� �������� ���� ����������, �� �������
            if (isTouchingFirstGear && isTouchingSecondGear)
            {
                cloneManager.RemoveClone(); // ��������� ������� ��� ������

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

    // ������� ��� ��������� ������ � ���������
    private IEnumerator HandleDeath()
    {
        // ������������� ���� ������
        audioSourceDeath.PlayOneShot(deathSound);

        // ��������� ���������
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // ��������� ������ (��� ������ � ����������� SpriteRenderer)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        // ����, ���� �� ���������� ����
        yield return new WaitForSeconds(deathSound.length);
        // ���������� ������
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
            checkpointManager.SetCheckpoint(collision.transform.position, cameraController.transform.position, transform.localScale); // ��������� � ������� ���������, � ������� ������
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
                // �������� ���������� � ����������� ������ ������� �����
                Hero cloneCharacter = heroTransform.GetComponent<Hero>();
                if (cloneCharacter != null)
                {
                    cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);
                }
            }

            // ����������� ���������
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
                // �������� ���������� � ����������� ������ ������� �����
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
                Debug.LogWarning("AudioSource ��� AudioClip �� ���������.");
            }

            Destroy(collision.gameObject);
            // ��������� � �������� �����
            SpawnAndMoveBird();
        }



        if (collision.tag == "bigBonus" || collision.tag == "smallBonus")
        {
            BonusPosition bonus = collision.GetComponent<BonusPosition>();
            if (bonus != null && !bonus.isPicked) // ���������, ��� �� ����� ��������
            {
                bonus.isPicked = true; // ��������� �����
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);
                Instantiate(effectClone, transform.position, Quaternion.identity);

                Transform[] heroTransforms = FindObjectsOfType<Hero>().Select(hero => hero.transform).ToArray();
                foreach (Transform heroTransform in heroTransforms)
                {
                    // ����������� ��� ��������� ������� � ����������� �� ���� ������
                    float scaleMultiplier = collision.tag == "bigBonus" ? 1.5f : 0.7f;
                    heroTransform.localScale *= scaleMultiplier;

                    // �������� ���������� � ����������� ������ ������� �����
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
            if (bonus != null && !bonus.isPicked) // ���������, ��� �� ����� ��������
            {
                bonus.isPicked = true; // ��������� �����
                SavePickedUpBonusPosition(collision.tag, bonus.initialPosition);

                Instantiate(effectClone, transform.position, Quaternion.identity);

                int numberOfHeroes = (collision.tag == "cloneBonus") ? 2 : 4;

                // �������� ��� ������������ ���������
                Hero[] cloneCharacters = FindObjectsOfType<Hero>();

                for (int i = 0; i < numberOfHeroes; i++)
                {
                    GameObject hero = Instantiate(heroPrefab, transform.position + (i % 2 == 0 ? Vector3.right : Vector3.left) * 2f, Quaternion.identity);
                    hero.transform.localScale = transform.localScale;
                    numberOfClonesAlive++;

                    // �������� ������ �������������� ���������
                    Hero cloneCharacter = hero.GetComponent<Hero>();
                    if (cloneCharacter != null)
                    {
                        // �������� ���������� � ����������� ������� ������� �����
                        cloneCharacter.bonusInfos = new List<BonusInfo>(bonusInfos);

                        // ��������� ���������� � ����������� ������� ��� ���� ��� ������������ ������
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
            // �������� ��������� Rigidbody ��� ���� �������� � ����� "dropstone2"
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


        if (collision.CompareTag("trigger6")) // ������� ��� ������ ������
        {
            isPlayerInside = true;
        }

        if (collision.CompareTag("trigger7")) // ������� ��� ������ ������ � ��������������� ������� �� ������
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
        // ������������ ����� ������ ������������ ��������� � �������� ���������
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += birdSpawnOffsetX; // ������� �� X �����
        spawnPosition.y += birdSpawnOffsetY; // ������� �� Y ����
        spawnPosition.z = 0; // ������������� z �� 0, ����� ����� ���� � ��� �� ����, ��� � ��������

        // ������� ����� � ������������ �������
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);

        // �������� �������� ��� �������� �����
        StartCoroutine(MoveBird(bird));
    }

    private IEnumerator MoveBird(GameObject bird)
    {
        // ������������� ��������� �����
        float startTime = Time.time;

        // ��������� ���� � �������
        float angleRad = birdFlightAngle * Mathf.Deg2Rad;

        // ������������ ����������� �������� �� ������ ����
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0).normalized;

        while (Time.time - startTime < birdFlyDuration)
        {
            // ������� ����� �� �����������
            bird.transform.Translate(direction * birdSpeed * Time.deltaTime);

            yield return null;
        }

        // ���������� ����� ����� ���������� ������
        Destroy(bird);
    }





    private IEnumerator MoveToTriggerCenter(Collider2D collision)
    {


        // �������� ����� ��������
        Vector3 triggerCenter = collision.bounds.center;

        // ���������� ��������� ������� ����������
        Transform[] clones = GameObject.FindGameObjectsWithTag("Character").Select(obj => obj.transform).ToArray();
        Vector3[] startPositions = clones.Select(clone => clone.position).ToArray();

        // ���������� ������� ������� ��� ������ � ������ ��������
        Vector3[] targetPositions = Enumerable.Repeat(triggerCenter, clones.Length).ToArray();

        // ������� ����������� � ������� �������� � �������������� �������������� �������
        float duration = 1f; // ��������� ����������������� ������ �� 1 �������
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ��������� �������� �����������
            float t = elapsedTime / duration;

            // �������� ������� �� ��� Y, ��������� �������������� �������, ���������� �� 4 ��� ��� �������� ���������
            float yOffset = Mathf.Sin(t * Mathf.PI * 4f); // �������� ������ ���������� �������

            for (int i = 0; i < clones.Length; i++)
            {
                Vector3 newPosition = Vector3.Lerp(startPositions[i], targetPositions[i], t) + Vector3.up * yOffset;
                clones[i].position = newPosition;
            }

            // ����������� ��������� �����
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ����� ���������� ����������� ������������� ������ ������� ������ �������� ��� ���� ����������
        for (int i = 0; i < clones.Length; i++)
        {
            clones[i].position = targetPositions[i];
        }

        yield return StartCoroutine(FadeCanvasSprite(sprite, 1f, 0f, 0)); // ��������� �������� ������������

        // ��������� ���������� ����������, ���� �����
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // ��������� ��������� ���������
        }


        // ���������, ����������� �� ��� �����������
        if (isSucking)
        {
            yield break; // ���� ��� ����������� �����������, ������ �������
        }

        // ������������� ����, ��� ����������� ��������
        isSucking = true;



        if (levelEndPanel != null)
        {
            levelEndPanel.alpha = 0;           // ������������� ���������
            levelEndPanel.interactable = false; // �������� ��������������
            levelEndPanel.blocksRaycasts = false; // �������� ���������� ������
        }

        yield return StartCoroutine(FadeInPanel());
        AudioListener.volume = 0;

        // ����� ���������� ����������� ���������� ����
        isSucking = false;
    }

    private IEnumerator FadeInPanel()
    {
        // ������� ������������� �����-����� ������ � 0
        CanvasGroup canvasGroup = levelEndPanel.GetComponent<CanvasGroup>(); // ��������������, ��� ������ ���������� CanvasGroup ��� ���������� �����-�������

        buttonPause.GetComponent<Image>().enabled = false;

        // ��������� ������, ����� ��� �� ����������� �� �������
        buttonPause.interactable = false;


        canvasGroup.alpha = 0f;           // ������������� ���������� ���������
        levelEndPanel.interactable = false; // ������ ���������
        levelEndPanel.blocksRaycasts = false; // ��������� �����

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f); // ������ �������� �����
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // ������������� ��������� �����
        levelEndPanel.interactable = true;  // �������� ��������������
        levelEndPanel.blocksRaycasts = true; // ������������ �����

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;


        Time.timeScale = 0f;

    }


    private IEnumerator FadeAndLoadScene(string sceneName, int currentLevel)
    {
        isAnimationPlaying = true; // ������������� ����, ��� �������� ����

        // ��������� �������� ����������
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        // ���������� ��������
        yield return StartCoroutine(ShowLevelProgress(currentLevel));

        // ���������� �������� �����
        SceneManager.LoadScene(sceneName); // ��������� ����� ���������

        // ����� �������� �����, �������� �������� ����������
        //fadeImage.gameObject.SetActive(false);
        isAnimationPlaying = false; // ��������� ��������
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

        image.color = targetColor;  // ���������, ��� ���� ���������� � �������� ��������
    }

    private IEnumerator UpdateLevelProgress(int currentLevel)
    {
        float transitionDuration = 0.5f; // ����� �������� ��������� �����

        // ���������� ������ �������
        for (int i = 0; i < levelCircles.Length; i++)
        {
            Color targetColor;

            if (i < currentLevel - 1)
            {
                // ������, ������� ��������
                targetColor = completedColor;
            }
            else if (i == currentLevel - 1)
            {
                // ������� �������
                targetColor = currentColor;
            }
            else
            {
                // �� ���������� ������
                targetColor = uncompletedColor;
            }

            // ������� ��������� �����
            yield return StartCoroutine(AnimateColorTransition(levelCircles[i], targetColor, transitionDuration));
        }
    }


    private IEnumerator ShowLevelProgress(int currentLevel)
    {
        // ����� ����������� ���������
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 0;
            img.color = color;
        }

        // ��������� ����������� ���������
        float elapsedTime = 0f;
        float fadeDuration = 1f; // ����� ��� ��������� �����������

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

        // ���������, ��� �����-����� ���������� � 1
        foreach (Image img in levelProgressImages)
        {
            Color color = img.color;
            color.a = 1;
            img.color = color;
        }

        // �������� �������� ������� � ������� ���������� �����
        yield return StartCoroutine(UpdateLevelProgress(currentLevel));
    }








    // ��� ��� ������� � �����������
    public IEnumerator FadeAndDelay()
    {
        Debug.Log("��� ��� ������� � ����������� ");
        cameraController.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // ��������� ��������� ���������
        }

        yield return StartCoroutine(FadeCanvasSprite(sprite, 1f, 0f, 0)); // ��������� �������� ������������
        yield return StartCoroutine(FadeCanvasImage(canvasImage, 0f, 1f, fadeDuration)); // ��������� �������� ������������
        yield return new WaitForSeconds(1f);
        isFrozen = true;
        canMoveAfterDeath = false; // ��������� �������� ����� ������
        yield return new WaitForSeconds(1f);

        if (collider != null)
        {
            collider.enabled = true; // �������� ��������� ���������

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
            // ������������� �����
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
            yield return StartCoroutine(FadeCanvasSprite(sprite, 0f, 1f, 0)); // ��������� �������� ���������
            yield return StartCoroutine(FadeCanvasImage(canvasImage, 1f, 0f, fadeDuration)); // ��������� �������� ������������
            cameraControllerComponent.enabled = true;

        }

    }


    void ResetObjectProperties(GameObject obj, Vector3 position, bool resetRotation, Vector3 rotationEulerAngles)
    {
        Rigidbody2D objRigidbody = obj.GetComponent<Rigidbody2D>();
        if (objRigidbody != null)
        {
            objRigidbody.simulated = false; // ��������� ���������� ���������
        }

        obj.transform.position = position; // �������� ��������� �������

        if (resetRotation)
        {
            obj.transform.rotation = Quaternion.Euler(rotationEulerAngles); // �������� ������� �������
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
        // ������������� ������������� �������� �����-������
        color.a = targetAlpha;
        spriteRenderer.color = color;
    }


    // ������ � ������� ������� ��� �����������
    void SavePickedUpBonusPosition(string bonusType, Vector3 initialPosition)
    {
        bool bonusPickedUp = bonusInfos.Any(info => info.type == bonusType && info.position == initialPosition);
        if (!bonusPickedUp)
        {
            BonusInfo info;
            info.position = initialPosition; // ��������� �������������� �������
            info.type = bonusType;
            bonusInfos.Add(info);
        }
    }

    // ����� ��� �������������� ����������� ������� ����� ������ ���������
    void RestoreBonuses()
    {
        foreach (BonusInfo info in bonusInfos.ToList()) // �������� �� ����� ������ ��� ����������� ��������
        {
            // ���������, ��� �� ����� �������� �����
            if (bonusInfos.Any(i => i.position == info.position && i.type == info.type))
            {
                // ������� ����� �� ����������� ������� � ����������� �� ��� ����
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

        // ������� ������ ���������� � ������� ����� ��������������
        bonusInfos.Clear();
    }
}