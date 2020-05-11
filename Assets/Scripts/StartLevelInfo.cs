using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelInfo : MonoBehaviour
{
    public InfoText Info;
    void Start()
    {
        FindObjectOfType<InfoBox>().GetComponent<InfoBox>().DisplayInfo(Info);
    }
}
