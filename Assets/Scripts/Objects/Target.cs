using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interactable Target
public class Target : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        //Sets bridge open/closed
        if (LinkedObjectType == ObjectType.Bridge)
        {
            Activated = LinkedObject.GetComponent<Bridge>().isOpen();
        }
    }
    
    //Action
    public override void Action()
    {
        Enabled = false;
        Activated = !Activated;
        this.Activate();
    }

    //Triggers on collision with a projectile
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && Enabled)
        {
            Debug.Log("Hit");
            Action();
        }
    }
}
