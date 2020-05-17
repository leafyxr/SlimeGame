using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad :  Interactable
{
    //Mass required to activate
    [SerializeField]
    float MassRequirement = 100;
    //Size of area
    Vector3 boxExtents;
    private void Start()
    {
        //Set size of area
        boxExtents = GetComponent<BoxCollider>().size / 2f;
        boxExtents -= boxExtents * 0.1f;
        boxExtents.y = 100;
    }

    //action
    public override void Action() 
    {
        this.Activate();
    }

    //Physics Update
    private void FixedUpdate()
    {
        //Enabled?
        if (this.Enabled)
        {
            //Detect opjects in area
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, boxExtents, transform.TransformDirection(Vector3.up));
            float mass = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                //Sum together masses of rigidbodies
                if (hit.rigidbody)
                {
                    mass += hit.rigidbody.mass;
                }
            }

            //Activate if requirement met
            if (mass >= MassRequirement && !this.Activated)
            {
                this.Activated = true;
                Action();
            }
            //Deativate if requirement no longer met
            else if (mass < MassRequirement && this.Activated)
            {
                this.Activated = false;
                Action();
            }
        }
    }
}
