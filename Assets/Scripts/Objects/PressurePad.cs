using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad :  Interactable
{
    [SerializeField]
    float MassRequirement = 100;

    Vector3 boxExtents;
    private void Start()
    {
        boxExtents = GetComponent<BoxCollider>().size / 2f;
        boxExtents -= boxExtents * 0.1f;
        boxExtents.y = 100;
    }

    public override void Action() 
    {
        this.Activate();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (this.Enabled)
        {
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, boxExtents, transform.TransformDirection(Vector3.up));
            float mass = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.rigidbody)
                {
                    mass += hit.rigidbody.mass;
                }
            }

            if (mass >= MassRequirement && !this.Activated)
            {
                this.Activated = true;
                Action();
            }
            else if (mass < MassRequirement && this.Activated)
            {
                this.Activated = false;
                Action();
            }
        }
    }
}
