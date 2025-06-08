using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    public float baseCameraSpeed = 5f; // Ѕазова€ скорость камеры
    public float accelerationFactor = 2f; // ‘актор ускорени€
    private float maxPlayerOffsetPercent = 0.5f; // ћаксимально допустимое смещение игрока от центра камеры (30% от ширины камеры)
    private bool isPlayerFocused = false;

    // ѕеременные дл€ тр€ски камеры
    private bool isShaking = false;
    public float shakeMagnitude = 0.9f;
    public float shakeDuration = 0.35f;
    private float shakeTime = 0f;
    private float originalY; // —охранение исходного Y-положени€ камеры

    private Hero hero;

    private void Start()
    {
        hero = FindObjectOfType<Hero>();
        // —охран€ем исходное положение камеры при запуске
        originalY = transform.position.y;
    }

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    private void Update()
    {
        // ќпредел€ем начальное положение камеры
        Vector3 currentPosition = transform.position;

        Hero[] clones = FindObjectsOfType<Hero>();
        if (clones.Length > 0)
        {
            Hero rightmostPlayer = clones[0];
            for (int i = 1; i < clones.Length; i++)
            {
                if (clones[i].transform.position.x > rightmostPlayer.transform.position.x)
                {
                    rightmostPlayer = clones[i];
                }
            }

            player = rightmostPlayer.transform;
            isPlayerFocused = true;
        }
        else
        {
            isPlayerFocused = false;
        }

        if (hero.canPerformActions)
        {
            // ѕосто€нное движение камеры вправо
            currentPosition.x += baseCameraSpeed * Time.deltaTime;

            if (isPlayerFocused)
            {
                // –асчет смещени€ игрока относительно центра камеры
                float playerOffset = player.position.x - (currentPosition.x - GetCameraWidth() / 2);

                // ≈сли игрок находитс€ ближе к правому краю, ускор€ем камеру
                if (playerOffset > maxPlayerOffsetPercent * GetCameraWidth())
                {
                    float excessDistance = playerOffset - maxPlayerOffsetPercent * GetCameraWidth();
                    currentPosition.x += accelerationFactor * excessDistance * Time.deltaTime;
                }
            }
        }

        // Ћогика тр€ски камеры
        if (isShaking)
        {
            if (shakeTime < shakeDuration)
            {
                // ƒобавл€ем случайное смещение только по Y
                currentPosition.y = originalY + Random.Range(-shakeMagnitude, shakeMagnitude);
                shakeTime += Time.deltaTime;
            }
            else
            {
                // «аканчиваем тр€ску и возвращаем камеру в исходное положение
                isShaking = false;
                shakeTime = 0f;
                currentPosition.y = originalY;
            }
        }

        // ќбновл€ем позицию камеры
        transform.position = currentPosition;
        pos.z = -10f;

    }
    public void SetCameraSpeed(float newSpeed)
    {
        baseCameraSpeed = newSpeed;
    }



    // ѕолучение ширины видимой области камеры
    private float GetCameraWidth()
    {
        return Camera.main.orthographicSize * Camera.main.aspect * 2;
    }

    // ћетод дл€ начала тр€ски камеры
    public void ShakeCamera()
    {
        isShaking = true;
        shakeTime = 0f; // ќбновл€ем врем€ тр€ски
    }
}
