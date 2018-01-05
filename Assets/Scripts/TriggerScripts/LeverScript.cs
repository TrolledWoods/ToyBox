using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LeverScript : MonoBehaviour {

    public Signal signal;
    bool Active = false;

    public MeshRenderer[] indicators;
    public Material inactive;
    public Material active;

    public Transform valve;
    public float valveRotDist = 0.9f;
    public float valveSpeed = 1f;
    float valveT = 0f;
    float originalValveRotY;

    List<IsTriggeringObject.eventWrapper> objectsOnButton;
    
    void Awake()
    {
        originalValveRotY = valve.rotation.eulerAngles.y;
        objectsOnButton = new List<IsTriggeringObject.eventWrapper>();
    }

    void Update()
    {
        if(Active)
        {
            valveT += Time.deltaTime * valveSpeed;
        }
        else
        {
            valveT -= Time.deltaTime * valveSpeed;
        }

        valveT = Mathf.Clamp(valveT, 0, 1);

        valve.rotation = Quaternion.Euler(0, valveT * valveRotDist + originalValveRotY, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        // Only do this if the thing entering has the triggering object component 
        //and that it is currently triggering stuff
        IsTriggeringObject trigger = other.gameObject.GetComponent<IsTriggeringObject>();

        if (trigger != null)
        {
            IsTriggeringObject.eventWrapper eventObj = new IsTriggeringObject.eventWrapper(stateChanged, trigger);
            trigger.AddChangedStatusFunction(eventObj);
            objectsOnButton.Add(eventObj);

            if (trigger.IsCurrentlyTriggering)
            {
                Entered();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Only do this if the thing entering has the triggering object component 
        //and that it is currently triggering stuff
        IsTriggeringObject trigger = other.gameObject.GetComponent<IsTriggeringObject>();

        if (trigger != null)
        {
            // Remove the trigger from the list of eventObjs
            for(int i = 0; i < objectsOnButton.Count; i++)
            {
                if(objectsOnButton[i].trigger == trigger)
                {
                    trigger.RemoveChangedStatusFunction(objectsOnButton[i]);
                    objectsOnButton.RemoveAt(i);
                    break;
                }
            }
        }
    }

    bool stateChanged(bool newState)
    {
        if (newState)
        {
            Entered();
        }

        return false;
    }

    void Entered()
    {
        Active = !Active;

        signal.Trigger();

        if(Active)
        {
            ChangeIndicatorMaterial(active);
        }
        else
        {
            ChangeIndicatorMaterial(inactive);
        }
    }

    void ChangeIndicatorMaterial(Material m)
    {
        for(int i = 0; i < indicators.Length; i++)
        {
            indicators[i].material = m;
        }
    }
}
