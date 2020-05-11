using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimit : MonoBehaviour {
    public int target = 30;
	// Use this for initialization
	void Start () {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.targetFrameRate != target)
        {
                Application.targetFrameRate = target;
        }
}
}
