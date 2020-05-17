using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base class for interactable objects
public class Interactable : MonoBehaviour
{
    //Can be used
    protected bool Enabled = true;

    //is activated
    protected bool Activated = false;

    //Linked Object settings
    protected enum ObjectType { Door, Light, Platform, Bridge };
    [SerializeField]
    protected GameObject LinkedObject;
    [SerializeField]
    protected ObjectType LinkedObjectType;


    public virtual void Action() { }

    //Activate Object Based on Type
    protected void Activate()
    {
        
        switch (LinkedObjectType)
        {
            case ObjectType.Door:
                if (LinkedObject.GetComponent<Animator>())
                {
                    //Trigger Door Animation
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
            case ObjectType.Bridge:
                if (LinkedObject.GetComponent<Animator>())
                {
                    //Trigger Bridge Animation
                    LinkedObject.GetComponent<Animator>().SetBool("Open", Activated);
                }
                break;
            default:
                Debug.LogError("Object Cannot be activated");
                break;

        } 
    }


}
