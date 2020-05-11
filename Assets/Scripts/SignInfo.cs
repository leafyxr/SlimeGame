using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInfo : MonoBehaviour
{
    [SerializeField]
    InfoText text;

    public void displayInfo()
    {
        FindObjectOfType<InfoBox>().GetComponent<InfoBox>().DisplayInfo(text);
    }
}
