using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    bool Open = true;

    //Set Start animation
    void Start()
    {
        GetComponent<Animator>().SetBool("Open", Open);
    }

    //Is Bridge Open
    public bool isOpen() { return Open; }

}
