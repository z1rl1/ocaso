using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 5f;
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 20); // 20 - это ширина вашего фона
        transform.position = startPosition + Vector2.right * newPosition;
    }
}