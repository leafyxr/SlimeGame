using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    [SerializeField]
    private bool isActive = false;

    [SerializeField]
    GameObject MenuObject;
    [SerializeField]
    Text HeaderText;
    [SerializeField]
    Text InfoTextBox;
    [SerializeField]
    Text ConfirmText;

    private float timescale;
    void Start()
    {
        if (!isActive)
        {
            MenuObject.SetActive(isActive);
        }
    }

    //Displays Information UI Box
    public void DisplayInfo(InfoText info)
    {
        if (!isActive)
        {
            timescale = Time.timeScale;
            Time.timeScale = 0;
            isActive = !isActive;
            MenuObject.SetActive(isActive);
            HeaderText.text = info.Header;
            InfoTextBox.text = info._InfoText;
            ConfirmText.text = info.ConfirmText;
        }
    }

    public void Confirm()
    {
        if (isActive)
        {
            Time.timeScale = timescale;
            isActive = !isActive;
            MenuObject.SetActive(isActive);
        }
    }
}
