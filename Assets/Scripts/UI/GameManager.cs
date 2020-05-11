using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    EventBox eventBox;
    DataManager dataManager;
    SceneLoader sceneLoader;
    float clock = 0.0f;
    [SerializeField]
    float AutoSaveDelay = 200;
    bool paused = false;

    [SerializeField]
    GameObject[] Milestones;
    
    void Start()
    {
        eventBox = FindObjectOfType<EventBox>().GetComponent<EventBox>();
        dataManager = FindObjectOfType<DataManager>().GetComponent<DataManager>();
        sceneLoader = FindObjectOfType<SceneLoader>().GetComponent<SceneLoader>();
        eventBox.gameObject.SetActive(paused);
        //FindObjectOfType<PlayerController>().GetComponent<PlayerController>().loadData(dataManager.data);
        if (Milestones.Length == dataManager.data.Milestones.Length)
        {
            for (int i = 0; i < Milestones.Length; i++)
            {
            }
        }
        else
        {
            dataManager.data.Milestones = new bool[Milestones.Length];
            Debug.LogError("Milestone Error");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") || paused && Input.GetButtonDown("UnPause"))
        {
            paused = !paused;
            eventBox.gameObject.SetActive(paused);
            eventBox.pauseGame(paused);
        }

        clock += Time.deltaTime;

        if (clock > AutoSaveDelay)
        {
            dataManager.dataSave();
        }
    }

    public bool setMilestone(GameObject gameObject, bool activated)
    {
        int i = 0;
        foreach(GameObject milestone in Milestones)
        {
            if (gameObject == milestone)
            {
                dataManager.data.Milestones[i] = activated;
                dataManager.dataSave();
                return true;
            }
            i++;
        }
        return false;
    }
}
