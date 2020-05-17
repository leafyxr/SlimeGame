using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Switches between Camera Objects stored in an array
public class CameraSwitch : MonoBehaviour
{

    [SerializeField]
    //Array of camera's
    Camera[] Cameras;
    //CurrentCamera
    int CurrentCamera = 0;
    // Sets starting camera
    void Start()
    {
        camSwitch();
    }

    // Switches camera on key press
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        {
            //Iterates current camera
            CurrentCamera++;
            //resets camera to 0 when end of array is reached
            if (CurrentCamera >= Cameras.Length) CurrentCamera = 0;
            //Switch Camera
            camSwitch();
        }
    }

    void camSwitch()
    {
        //Loop through array
        for (int i = 0; i < Cameras.Length; i++)
        {
            //Activate current camera, deactivate the rest
            if (i == CurrentCamera) Cameras[i].gameObject.SetActive(true);
            else Cameras[i].gameObject.SetActive(false);
        }
    }
}
