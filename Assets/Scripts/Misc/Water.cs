using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    //Upwards force
    [SerializeField]
    float floatForce = 5;

    //Damage over Time settings (DOT)
    [SerializeField]
    int playerDamage = 1;
    [SerializeField]
    float damageDelay = 0.5f;
    float clock = 0;

    //When an object is inside the area
    private void OnTriggerStay(Collider other)
    {
        //Damage Player (DOT)
        if (other.gameObject.CompareTag("Player"))
        {
            clock += Time.deltaTime;
            if (clock >= damageDelay)
            {
                clock = 0;
                other.GetComponent<PlayerController>().damagePlayer(playerDamage);
            }
        }
        //If object has a rigidbody component
        if (other.GetComponent<Rigidbody>())
        {
            //If below water level
            if(other.transform.position.y < transform.position.y)
            {
                //Calculate Distance
                float distance = (Mathf.Abs(transform.position.y - other.transform.position.y));
                //Calculate Force
                float baseForce = floatForce;
                //Cause force to fluctuate over time using Cos
                baseForce += 0.1f * floatForce * Mathf.Pow(Mathf.Cos(Time.timeSinceLevelLoad), 2);
                Debug.Log("Distance = " + distance + " Force = " + baseForce);

                //Reduce speed of rigidbody
                other.GetComponent<Rigidbody>().velocity = other.GetComponent<Rigidbody>().velocity * 0.8f;
                //Add upwards force
                other.GetComponent<Rigidbody>().AddForce(Vector3.up * distance * baseForce);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Reset DOT clock
        if (other.gameObject.CompareTag("Player"))
        {
            clock = 0;
        }
    }


}
