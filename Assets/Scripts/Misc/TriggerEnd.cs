using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triggers level end
public class TriggerEnd : MonoBehaviour
{
    bool trigger;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Triggers end of level when player comes into contact
        if (collision.gameObject.CompareTag("Player"))
        {
            trigger = true;
            gameManager.win();
        }
    }
}
