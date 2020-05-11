using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    //Reticle transform
    Transform reticule;
    //Speed of reticle spin
    public int spinSpeed = 30;
    private Vector3 Point; 

    // Start is called before the first frame update
    void Start()
    {
        //get transform
        reticule = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //cast a ray down in the world from our current position
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit))
        {
            //sets the rotation so the x-axis aligns to the normal of the surface beneath us
            //this aligns the x to the surface's outward direction not the surface's x-axis
            transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }
        
        //draw a ray from camera to mouse
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit2))
        {
            Point = hit2.point;
            //set reticule at point
            reticule.position = Point;
        }
        else
        {
            reticule.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //Spin Reticule
        reticule.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }

    public Vector3 getPoint() { return Point; }
}
