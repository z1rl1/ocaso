using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 3.0f; // Время жизни объекта в секундах

    void Start()
    {
        Invoke("DestroyObject", lifetime); // Запускаем функцию DestroyObject через lifetime секунд
    }

    void DestroyObject()
    {
        // Уничтожаем объект
        Destroy(gameObject);
    }
}