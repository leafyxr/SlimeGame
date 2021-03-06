﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string[] LoadScene;

    [SerializeField]
    private GameObject Settings;

    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private GameObject MenuObject;

    [SerializeField]
    private SceneLoader sceneLoader;


    void Start () {
        MenuObject.SetActive(true);
        Settings.SetActive(false);
        dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
	}

    private void Update()
    {
    }
    public void StartButton1()
    {
        MenuObject.SetActive(false);
        sceneLoader.LoadScene(LoadScene[0]);
    }

    public void StartButton2()
    {
        MenuObject.SetActive(false);
        sceneLoader.LoadScene(LoadScene[1]);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void settings()
    {
        MenuObject.SetActive(false);
        Settings.SetActive(true);
    }

    public void menu()
    {
        MenuObject.SetActive(true);
        Settings.SetActive(false);
    }

}
