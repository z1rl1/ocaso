using UnityEngine;

public class CheckpointManagerScript : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    private Vector3 lastCameraPosition;
    private Vector3 lastPlayerScale; // Добавляем переменную для хранения размера персонажа
    private bool checkpointSet; // Переменная для проверки, был ли установлен чекпоинт

    public void SetCheckpoint(Vector3 position, Vector3 cameraPosition, Vector3 playerScale) // Модифицируем метод SetCheckpoint, чтобы он сохранял и размер персонажа
    {
        lastCheckpointPosition = position;
        lastCameraPosition = cameraPosition;
        lastPlayerScale = playerScale;
        checkpointSet = true;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    public Vector3 GetLastCameraPosition()
    {
        return lastCameraPosition;
    }

    public Vector3 GetLastPlayerScale() // Добавляем метод для получения размера персонажа
    {
        return lastPlayerScale;
    }

    public bool IsCheckpointSet()
    {
        return checkpointSet;
    }

    public void ResetCheckpoint() 
    {
        checkpointSet = false;
    }
}
