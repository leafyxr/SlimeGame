using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {

    [SerializeField]
    int SlimeLevel = 100;//Current Level of Slime
    [SerializeField]
    int MaxSlime;//Maximum ammount of Slime
    [SerializeField]
    Text SlimeLabel;//Text Label for displaying current slime
    [SerializeField]
    float SizeModifier;//Size modifier
    [SerializeField]
    float MinSize;//Minimum Size
    [SerializeField]
    float MassModifier;//Mass Modifier
    float Mass;//Current Mass



    [SerializeField]
    float speed = 6.0f; //Move Speed
    float distToGround; //Distance to Ground

    private Vector3 moveDir = Vector3.zero; // Move Direction
    private bool grounded = true; // Is on the ground
    private Animator animator; //Animator Componenet

    GameObject currentSelection;
    [SerializeField]
    float ScanRange = 0.5f;

    [SerializeField]
    Transform reticle; // Aiming Reticle
    [SerializeField]
    GameObject projectile; // Projectile Prefab
    [SerializeField]
    int fireCost; // Slime cost of launching projectile
    [SerializeField]
    float firingAngle; // projectile launch angle
    [SerializeField]
    int maxDistance = 5; //maximum distance projectile can travel;

    [SerializeField]
    Material Outline;

    //Pick up and Carry properties
    [SerializeField]
    GameObject carryLocation;

    [SerializeField]
    float throwForce = 1000;

    GameObject currentCarryObject;
    float carryMass;
    bool Carrying = false;

    private void Start() //Runs on Initilisation
    {
        //Assigns Animator Component
        animator = GetComponentInChildren<Animator>();
        //gets players collider bounds and sets how far the players centre is from the ground
        Collider collider = GetComponent<Collider>();
        distToGround = collider.bounds.extents.y;
        //Set Start Slime Level
        modSlime(0);
    }
    private void Update() //Runs every Frame
    {
        if (Time.timeScale != 0)
        {
            //If fire button pressed
            if (Input.GetButtonDown("Fire"))
            {
                if (Carrying)
                {
                    fireCarryable();
                }
                else
                {
                    //Calculate if can fire based on slime level
                    int slime = SlimeLevel - fireCost;
                    if (slime <= 0) return;
                    else
                    {
                        fireProjectile();
                    }
                }
            }
            //If Interact button pressed
            if (Input.GetButtonDown("Interact"))
            {
                if (currentSelection != null)
                {

                    //Carry Selected object
                    if (currentSelection.CompareTag("Carryable") && !Carrying)
                    {
                        Carrying = true;
                        currentCarryObject = currentSelection;
                        removeOutline(currentSelection.GetComponent<MeshRenderer>());
                        currentSelection = null;
                        carryMass = currentCarryObject.GetComponent<Rigidbody>().mass;
                        currentCarryObject.GetComponent<Rigidbody>().mass = 1;
                    }
                    else if (currentSelection.CompareTag("Consumable"))
                    {
                        modSlime(currentSelection.GetComponent<Consumable>().consume());
                        removeOutline(currentSelection.GetComponent<MeshRenderer>());
                        currentSelection = null;
                    }
                    else if (currentSelection.CompareTag("Sign"))
                    {
                        currentSelection.GetComponent<SignInfo>().displayInfo();
                        removeOutline(currentSelection.GetComponent<MeshRenderer>());
                        currentSelection = null;
                    }
                    //Add to slime value

                }
            }
        }
        //Update UI
        SlimeLabel.text = SlimeLevel.ToString();
    }

    //Modify Slime Value
    private bool modSlime(int Change)
    {
        //Get Current Level
        int startLevel = SlimeLevel;
        //Find new Level
        int newLevel = SlimeLevel + Change;
        //Set newLevel to max if it is over
        if (newLevel > MaxSlime) newLevel = MaxSlime;
        if (newLevel <= 0) return false;

        //Set Slime Level
        SlimeLevel = newLevel;

        //Change Mass and Size based on new value
        transform.localScale = Vector3.one * (MinSize + (((float)newLevel / (float)MaxSlime) * (float)SizeModifier));
        Mass = ((float)newLevel / (float)MaxSlime) * MassModifier;

        //Set Mass
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = Mass;
        return true;
    }

    //Damages the player
    public bool damagePlayer(int damage)
    {
        if (!modSlime(-damage))
        {
            FindObjectOfType<GameManager>().GetComponent<GameManager>().gameOver();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FixedUpdate() //Runs every Physics pass
    {
        //Checks if player is grounded
        grounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        //Gets movement direction from Input
        moveDir = Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical");
        moveDir = moveDir.normalized;
        //If there is input set walking animation and rotate player model in the direction of movement
        if (moveDir != Vector3.zero)
        {
            animator.SetBool("Walking", true);
            transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            //if grounded move player at full speed
            if (grounded)
            {
                moveDir *= speed * Time.deltaTime;
            }
            //else move at a slower speed
            else
            {
                moveDir *= speed * 0.4f * Time.deltaTime;
            }
        }
        //Else disable walk animation
        else
        {
            moveDir = Vector3.zero;
            animator.SetBool("Walking", false);
        }
        
        //Add movement to rigidbody component
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity += moveDir;
        rigidbody.AddForce(Vector3.up * 0.8f);

        if (Carrying)
        {
            Carry();
        }


        //Scan for all physics objects in range 

        Collider[] Hits = Physics.OverlapSphere(transform.position, ScanRange);
        //Iterate through scan results and Interact with the first Hit
        bool selection = false;
        foreach (Collider hit in Hits)
        {
            if (hit.CompareTag("Carryable") || hit.CompareTag("Consumable") || hit.CompareTag("Sign") || hit.CompareTag("Interactable"))
            {
                if (currentCarryObject != hit.gameObject)
                {
                    selection = true;
                    if (currentSelection == null || currentSelection != hit.gameObject)
                    {

                        if (currentSelection != null)
                        {
                            removeOutline(currentSelection.GetComponent<MeshRenderer>());
                            currentSelection = null;
                        }

                        currentSelection = hit.gameObject;
                        addOutline(currentSelection.GetComponent<MeshRenderer>());
                    }
                }
            }
        }

        if (!selection && currentSelection != null)
        { 
            removeOutline(currentSelection.GetComponent<MeshRenderer>());
            currentSelection = null;
        }
    }

    void addOutline(MeshRenderer mr)
    {
        //Check for empty material slot
        if (ReferenceEquals(mr.materials[mr.materials.Length - 1],null))
        {
            mr.materials[mr.materials.Length - 1] = Outline;
            return;
        }

        //Create array of materials
        Material[] m = new Material[mr.materials.Length + 1];
        int i = 0;

        //add all existing materials to array
        foreach (Material mat in mr.materials)
        {
            
            m[i] = mat;
            i++;
        }

        //add outline to the end
        m[i] = Outline;

        //set new material array
        mr.materials = m;
    }

    void removeOutline(MeshRenderer mr)
    {
        //New Material Array but smaller than the original
        Material[] m = new Material[mr.materials.Length - 1];

        //Add all old materials to new array
        for (int i = 0; i < m.Length; i++)
        {

            m[i] = mr.materials[i];
        }
        //set new array
        mr.materials = m;
    }

    void fireProjectile()//Fires a projectile
    {
        //Take away Slime 
        modSlime(-fireCost);
        //Calculates Direction
        Vector3 target = reticle.GetComponent<Reticule>().getPoint();
        Vector3 curPos = carryLocation.transform.position;
        Vector3 direction = target - curPos;
        direction.Normalize();
        //Calculates rotation
        Quaternion rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        //Makes a projectile object
        GameObject projectileC = Instantiate(projectile as GameObject) as GameObject;
        projectileC.GetComponent<Projectile>().GetGameObject(projectileC);
        //Starts projectile motion coroutine
        StartCoroutine(projectileFire(projectileC, target));
    }

    void fireCarryable()
    {
        //Calulate Direction
        Vector3 target = reticle.GetComponent<Reticule>().getPoint();
        Vector3 curPos = carryLocation.transform.position;
        Vector3 direction = target - curPos;
        direction.Normalize();
        Carrying = false;
        Rigidbody body = currentCarryObject.GetComponent<Rigidbody>();

        //Calculate Distance
        float distance = Vector3.Distance(body.transform.position, target);
        //Keep distance capped at a maximum
        if (distance > maxDistance) distance = maxDistance;

        //Calculate force
        float force = throwForce * SlimeLevel / MaxSlime * distance / maxDistance;
        //Add Upwards force
        direction.y += 0.5f;

        //Apply Force
        body.AddForce(direction * force);
        body.mass = carryMass;

        currentCarryObject = null;
    }

    IEnumerator projectileFire(GameObject proj, Vector3 target)
    {
        //Sets start position
        proj.transform.position = carryLocation.transform.position;
        //Finds Distance, If distance is over the maximum, set distance to the max value
        float distance = Vector3.Distance(proj.transform.position, target);
        if (distance > maxDistance)
        {
            target.y = 0;
            distance = maxDistance;
        }
           //Calculate overall velocity 
        float velocity = Mathf.Abs(distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / -Physics.gravity.y));

        //Calulate X and Y components of Velocity
        float velocityX = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float velocityY = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        //Calculate Duration of Projectile Flight
        float duration = distance / velocityX;

        //Sets Rotation of projectile to move direction.
        proj.transform.rotation = Quaternion.LookRotation(target - proj.transform.position);

        //Clock
        float time = 0;
        //Runs until duration reached
        while (time < 2 * duration && proj != null)
        {
            //Moves projectile, Velocity Y is affected by Gravity, Velocity X remains Constant
            proj.transform.Translate(0, (velocityY - (-Physics.gravity.y * time)) * Time.deltaTime, velocityX * Time.deltaTime);
            //Update Clock
            time += Time.deltaTime;
            //Repeat Loop until finished
            yield return null;
        }
        Destroy(proj, 0.1f);
    }

    void Carry()
    {
        currentCarryObject.GetComponent<Transform>().position = carryLocation.GetComponent<Transform>().position;
        currentCarryObject.GetComponent<Transform>().rotation = carryLocation.GetComponent<Transform>().rotation;
    }
}
