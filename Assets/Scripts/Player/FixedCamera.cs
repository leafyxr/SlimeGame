using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    Camera camera;
    [SerializeField]
    Transform focusObject;
    [SerializeField]
    Vector3 offset;

    Vector3 velocity = Vector3.zero;
    float smooothTime = 3.0f;

    [SerializeField]
    float maxDistance = 7;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 pos = camera.WorldToViewportPoint(focusObject.position);
        float distance = Vector3.Distance(transform.position, focusObject.position);
        if (pos.x < 0.1 || pos.x > 0.9 || pos.y < 0.1 || pos.y > 0.9 || distance > 7)
        {
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            pos = camera.ViewportToWorldPoint(pos);
            pos.x += offset.x;
            pos.y += offset.y;
            pos.z += offset.z;

            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooothTime);
        }
    }
}
