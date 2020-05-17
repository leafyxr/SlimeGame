using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventBox : MonoBehaviour {

    [SerializeField]
    Text sceneButton;//Restart/Next Scene button
    [SerializeField]
    Text quitButton;
    [SerializeField]
    Text eventText;
    [SerializeField]
    SceneLoader LoadingScreen;
    [SerializeField]
    GameObject UI;
    [SerializeField]
    private string currentScene;//Current Level Scene
    private string loadScene;//Scene to be loaded when button clicked
    private DataManager dataManager;

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        
    }

    public void pauseGame(bool paused)
    {
        dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        dataManager.dataSave();
        loadScene = currentScene;
        UI.SetActive(!paused);
        if (paused) 
        {
            Time.timeScale = 0;
            eventText.text = "Paused\nEsc to Resume";
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void gameOver()
    {
        Time.timeScale = 0;
        UI.SetActive(false);
        eventText.text = "Game Over!\nPlease try again.";
        loadScene = currentScene;
    }

    public void winGame()
    {
        dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        dataManager.dataSave();
        Time.timeScale = 0;
        UI.SetActive(false);
        eventText.text = "Level Complete!";
    }

    public void ChangeScene()
    {
        Time.timeScale = 1;
        LoadingScreen.LoadScene(loadScene);
    }
    public void exitGame()
    {
        LoadingScreen.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
