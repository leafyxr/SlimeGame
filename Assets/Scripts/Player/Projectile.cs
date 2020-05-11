using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    GameObject staticCopy;//Static version of projectile
    GameObject thisObject;//Reference to this gameObject

    private void OnCollisionEnter(Collision collision)//On Collision Start
    {
        if (!collision.gameObject.CompareTag("Player"))//If not Colliding with Player
        {
            //Create a Static copy
            GameObject copy = Instantiate(staticCopy as GameObject) as GameObject;
            //Set copy position and rotation
            copy.transform.SetPositionAndRotation(thisObject.transform.position, thisObject.transform.rotation);
            //Destroy projectile
            Destroy(thisObject, 0.1f);
        }
    }

    public void GetGameObject(GameObject newObject)
    {
        //Set reference to game object
        thisObject = newObject;
    }
}
