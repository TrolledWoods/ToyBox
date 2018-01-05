using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    Vector3 targetPosition;
    public float cameraSpeed = 0.1f;
    public float minimumCameraZoom = 3f;
    public float cameraZoomIncrease = 1.5f;

    public float angleChangeSpeed = 0.1f;
    public float angleIncrease = 20f;
    public float maxAngleX = 180;
    public int maxAngleSteps = 4;
    Quaternion targetAngle;
    Quaternion originalRotation;

    public Transform following;
    
	// Use this for initialization
	void Start () {
        transform.position = following.position;
        originalRotation = transform.rotation;
        targetAngle = originalRotation;
	}
	
	// Update is called once per frame
	void Update () {

        // Move the camera towards the target
        targetPosition = following.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed);

        // Reset the rotation of the transform
        transform.rotation = Quaternion.Lerp(transform.rotation, targetAngle, angleChangeSpeed);

        // Make the camera change the angle so that stuff doesn't block the target
        // Make a raycast to see if you can see the player
        RaycastHit hitInfo;
        bool hitSomething = true;
        int steps = 0;
        targetAngle = originalRotation;
        while(hitSomething && steps < maxAngleSteps)
        {
            steps++;
            hitSomething = Physics.Raycast(transform.position, targetAngle * Vector3.up, out hitInfo, transform.lossyScale.y);

            if(hitSomething)
            {
                Vector3 eulerAngles = targetAngle.eulerAngles;
                targetAngle = Quaternion.Euler(eulerAngles.x + angleIncrease, eulerAngles.y, eulerAngles.z);
            }
        }

	}
}
