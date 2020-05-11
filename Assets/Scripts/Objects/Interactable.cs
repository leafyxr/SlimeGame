using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool Enabled = true;
    protected bool Activated = false;

    protected enum ObjectType { Door, Light, Platform };
    [SerializeField]
    protected GameObject LinkedObject;
    [SerializeField]
    protected ObjectType LinkedObjectType;
    public virtual void Action() { }

    protected void Activate()
    {
        switch (LinkedObjectType)
        {
            case ObjectType.Door:
                if (LinkedObject.GetComponent<Animator>())
                {
                    LinkedObject.GetComponent<Animator>().SetBool("Open", Activated);
                }
                else Debug.LogError("Door Object Cannot be activated");
                break;
            case ObjectType.Light:
                Debug.Log("Light Object not Implemented");
                break;
            case ObjectType.Platform:
                Debug.Log("Platform Object not Implemented");
                break;
            default:
                Debug.LogError("Object Cannot be activated");
                break;

        } 
    }


}
