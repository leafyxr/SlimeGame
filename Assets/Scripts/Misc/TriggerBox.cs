using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trigger Box, Activates when player is in area, can be used for multiple purposes.
public class TriggerBox : MonoBehaviour {

    public bool Trigger;
	// Use this for initialization
	void Start () {
        Trigger = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Trigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Trigger = false;
        }
    }
}
