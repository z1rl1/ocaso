using UnityEngine;

public class CheckpointManagerScript : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    private Vector3 lastCameraPosition;
    private Vector3 lastPlayerScale; // ��������� ���������� ��� �������� ������� ���������
    private bool checkpointSet; // ���������� ��� ��������, ��� �� ���������� ��������

    public void SetCheckpoint(Vector3 position, Vector3 cameraPosition, Vector3 playerScale) // ������������ ����� SetCheckpoint, ����� �� �������� � ������ ���������
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

    public Vector3 GetLastPlayerScale() // ��������� ����� ��� ��������� ������� ���������
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
