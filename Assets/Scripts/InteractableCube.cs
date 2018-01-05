using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCube : MonoBehaviour {

    public static List<InteractableCube> interactablesInScene;
    public List<InteractableCube> interactables;

	// Use this for initialization
	void Start () {
		if(interactablesInScene == null)
        {
            interactablesInScene = new List<InteractableCube>();
        }

        interactables = interactablesInScene;
        interactablesInScene.Add(this);
	}
}
