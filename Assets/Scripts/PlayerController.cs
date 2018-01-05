using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    
    public float speed = 1000f;
    public float pickupRadius = 2f;
    Rigidbody rb;

    Rigidbody holdingRb;
    public Transform holding;
    public float preferedDist;
    public float holdingPosLerpSpeed = 0.1f;
    Vector3 holdingTargetPos;
    IsTriggeringObject holdingTrigger;

    public RectTransform grabInfo;
    public Camera cam;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        
         // Move the player
         if (Input.GetAxisRaw("Vertical") > 0.5f)
         {
             rb.AddForce(Vector3.forward * speed * Time.deltaTime);
         }
         if (Input.GetAxisRaw("Vertical") < -0.5f)
         {
             rb.AddForce(Vector3.back * speed * Time.deltaTime);
         }
         if (Input.GetAxisRaw("Horizontal") > 0.5f)
         {
             rb.AddForce(Vector3.right * speed * Time.deltaTime);
         }
         if (Input.GetAxisRaw("Horizontal") < -0.5f)
         {
             rb.AddForce(Vector3.left * speed * Time.deltaTime);
         }

        if (holding == null && InteractableCube.interactablesInScene != null)
        {
            // See if there are any interactable cubes inside the pickupradius and if there is, take the closest one
            // and make the "e" icon appear above it
            float smallestDistanceYet = pickupRadius * pickupRadius;
            Transform thingToPickUp = null;

            for(int i = 0; i < InteractableCube.interactablesInScene.Count; i++)
            {
                float distSqr = (transform.position - InteractableCube.interactablesInScene[i].gameObject.transform.position).sqrMagnitude;
                
                if(distSqr < smallestDistanceYet)
                {
                    smallestDistanceYet = distSqr;
                    thingToPickUp = InteractableCube.interactablesInScene[i].gameObject.transform;
                }
            }

            if (thingToPickUp != null)
            {
                grabInfo.gameObject.SetActive(true);
                grabInfo.position = cam.WorldToScreenPoint(thingToPickUp.position + Vector3.up * thingToPickUp.localScale.y * 1.5f);

                if (Input.GetButtonDown("Grab"))
                {
                    holding = thingToPickUp;
                    holdingRb = holding.GetComponent<Rigidbody>();
                    holdingTrigger = holding.GetComponent<IsTriggeringObject>();
                    holdingTargetPos = holding.position;

                    if (holdingTrigger != null) holdingTrigger.SetIsTriggering(false);

                    // Set the layer of what you're holding to ignore raycasts
                    holding.gameObject.layer = 2;
                }
            }
            else
            {
                grabInfo.gameObject.SetActive(false);
            }
        }
        else if(holding != null)
        {
            grabInfo.gameObject.SetActive(false);

            // Make the gameobjects y coordinate move towards your y coordinate
            holdingTargetPos = new Vector3(holdingTargetPos.x, transform.position.y, holdingTargetPos.z);

            // Get the distance between the object you're holding and yourself
            float dist = Vector3.Distance(holding.position, transform.position);

            // Then get the unit vector between the two
            Vector3 unitV = (holding.position - transform.position) / dist;

            // Move what you're holding so that it's the prefered distance away from you
            holdingTargetPos = transform.position + unitV * preferedDist;

            // Move towards the target position
            holding.position = Vector3.Lerp(holding.position, holdingTargetPos, holdingPosLerpSpeed);

            // If the target position is too far away from the actual position, drop the thing
            float distFromTarget = Vector3.Distance(transform.position, holding.position);

            if (distFromTarget > pickupRadius)
            {
                holding = null;
            }
            else {

                // If what you're holding has a rigidbody, make sure that the velocity is zero
                if (holdingRb != null)
                {
                    holdingRb.velocity = Vector3.zero;
                }

                if (Input.GetButtonDown("Grab"))
                {
                    // Set the layer of what you're holding to not ignore raycasts
                    holding.gameObject.layer = 0;
                    if (holdingTrigger != null) holdingTrigger.SetIsTriggering(true);

                    holding = null;
                }
            }
        }
         
	}
}
