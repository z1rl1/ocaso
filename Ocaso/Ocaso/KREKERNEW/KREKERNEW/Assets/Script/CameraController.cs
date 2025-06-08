using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    public float baseCameraSpeed = 5f; // ������� �������� ������
    public float accelerationFactor = 2f; // ������ ���������
    private float maxPlayerOffsetPercent = 0.5f; // ����������� ���������� �������� ������ �� ������ ������ (30% �� ������ ������)
    private bool isPlayerFocused = false;

    // ���������� ��� ������ ������
    private bool isShaking = false;
    public float shakeMagnitude = 0.9f;
    public float shakeDuration = 0.35f;
    private float shakeTime = 0f;
    private float originalY; // ���������� ��������� Y-��������� ������

    private Hero hero;

    private void Start()
    {
        hero = FindObjectOfType<Hero>();
        // ��������� �������� ��������� ������ ��� �������
        originalY = transform.position.y;
    }

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    private void Update()
    {
        // ���������� ��������� ��������� ������
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
            // ���������� �������� ������ ������
            currentPosition.x += baseCameraSpeed * Time.deltaTime;

            if (isPlayerFocused)
            {
                // ������ �������� ������ ������������ ������ ������
                float playerOffset = player.position.x - (currentPosition.x - GetCameraWidth() / 2);

                // ���� ����� ��������� ����� � ������� ����, �������� ������
                if (playerOffset > maxPlayerOffsetPercent * GetCameraWidth())
                {
                    float excessDistance = playerOffset - maxPlayerOffsetPercent * GetCameraWidth();
                    currentPosition.x += accelerationFactor * excessDistance * Time.deltaTime;
                }
            }
        }

        // ������ ������ ������
        if (isShaking)
        {
            if (shakeTime < shakeDuration)
            {
                // ��������� ��������� �������� ������ �� Y
                currentPosition.y = originalY + Random.Range(-shakeMagnitude, shakeMagnitude);
                shakeTime += Time.deltaTime;
            }
            else
            {
                // ����������� ������ � ���������� ������ � �������� ���������
                isShaking = false;
                shakeTime = 0f;
                currentPosition.y = originalY;
            }
        }

        // ��������� ������� ������
        transform.position = currentPosition;
        pos.z = -10f;

    }
    public void SetCameraSpeed(float newSpeed)
    {
        baseCameraSpeed = newSpeed;
    }



    // ��������� ������ ������� ������� ������
    private float GetCameraWidth()
    {
        return Camera.main.orthographicSize * Camera.main.aspect * 2;
    }

    // ����� ��� ������ ������ ������
    public void ShakeCamera()
    {
        isShaking = true;
        shakeTime = 0f; // ��������� ����� ������
    }
}
