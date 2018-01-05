using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class InvisibleWall : MonoBehaviour {

    public Signal signal;

    MeshRenderer _renderer;
    Collider _collider;

    Material activeMaterial;
    public Material inactiveMaterial;

    int thingsInWall = 0;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        activeMaterial = _renderer.material;
    }
	
	// Update is called once per frame
	void Update () {
        if (signal.IsHappy())
        {
            _renderer.material = inactiveMaterial;
            _renderer.gameObject.layer = 2;
            _collider.isTrigger = true;
        }
        else
        {
            _renderer.material = activeMaterial;
            _renderer.gameObject.layer = 0;
            _collider.isTrigger = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        thingsInWall++;
    }

    void OnTriggerExit(Collider other)
    {
        thingsInWall--;
    }
}
