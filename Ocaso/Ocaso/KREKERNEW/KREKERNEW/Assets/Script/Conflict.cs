using UnityEngine;

public class Conflict : MonoBehaviour
{
    private CameraController cameraController;

    private bool flag = true; 

    private void Start()
    {
        // Находим компонент CameraController на камере
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (flag)
            {
                flag = false;
                cameraController.ShakeCamera();
            }
        }
    }
}
