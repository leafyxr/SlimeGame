using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [SerializeField]
    int RestoreValue;//Value to be Added
    public int consume()
    {
        //Destrot game object in 0.5 seconds
        Destroy(gameObject, 0.1f);
        return RestoreValue;//Return Value
    }
}
