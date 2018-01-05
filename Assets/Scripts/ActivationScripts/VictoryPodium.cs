using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPodium : MonoBehaviour {

    public Signal activationSignal;
    public string nextLevel; 

	// Use this for initialization
	void Start () {
		
	}
	
    void OnTriggerEnter()
    {
        if(activationSignal.IsHappy())
        {
            // Remove some static resources that shouldn't be loaded in the next level
            Signal.SignalStates = null;
            InteractableCube.interactablesInScene = null;

            // Load the new scene
            SceneManager.LoadScene(nextLevel);
        }
    }
}
