using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;
    public AudioClip bombSound; // Звуковой файл взрыва

    private CameraController cameraController;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            Explode();

            // Начинаем тряску камеры
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();

            // Создаем временный объект для проигрывания звука
            PlaySoundAndDestroy();
        }
    }

    private void Explode()
    {
        // Генерация взрывной волны
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 explosionDirection = (rb.transform.position - transform.position).normalized;
                rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    private void PlaySoundAndDestroy()
    {
        // Создаем временный объект для звука
        GameObject tempObj = new GameObject("TempAudio");
        tempObj.transform.position = transform.position;

        // Добавляем компонент AudioSource и проигрываем звук
        AudioSource audioSource = tempObj.AddComponent<AudioSource>();
        audioSource.clip = bombSound;
        audioSource.volume = 0.3f;
        audioSource.Play();

        // Уничтожаем временный объект после окончания звука
        Destroy(tempObj, bombSound.length);

        // Уничтожаем объект бомбы
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}