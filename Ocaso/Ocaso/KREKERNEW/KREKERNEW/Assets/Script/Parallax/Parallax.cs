using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour


{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(0f, 1f)] float parallaxStrenght = 0.1f;
    [SerializeField] bool disableVerrticalParallax;
    Vector3 targetPreviooousPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (!followingTarget)
            followingTarget = Camera.main.transform;

        targetPreviooousPosition = followingTarget.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var delta = followingTarget.position - targetPreviooousPosition;

        if (disableVerrticalParallax)
            delta.y = 0;

        targetPreviooousPosition = followingTarget.position;

        transform.position += delta * parallaxStrenght;
    }
}