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
        //set start position
        transform.position = focusObject.position + offset;
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        //Get Position of player in the viewport 
        Vector3 pos = camera.WorldToViewportPoint(focusObject.position);
        //Get distance from camera to player
        float distance = Vector3.Distance(transform.position, focusObject.position);
        //If Player out of bounds
        if (pos.x < 0.1 || pos.x > 0.9 || pos.y < 0.1 || pos.y > 0.9 || distance > 7)
        {
            //Set X and Y value within bounds of 0 and 1
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            //Convert to world coords
            pos = camera.ViewportToWorldPoint(pos);
            //Apply Camera Offset
            pos.x += offset.x;
            pos.y += offset.y;
            pos.z += offset.z;

            //Move the camera smoothly towards target position.
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smooothTime);
        }
    }
}
