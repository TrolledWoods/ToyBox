using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ButtonScript : MonoBehaviour {

    public Signal signal;
    public int BlocksOnButton;

    public MeshRenderer[] indicators;
    public Material inactive;
    public Material active;
    public Material couldBeActive;

    List<IsTriggeringObject.eventWrapper> objectsOnButton;
    
    void Awake()
    {
        objectsOnButton = new List<IsTriggeringObject.eventWrapper>();
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
                // If there was no block here before
                if (BlocksOnButton == 0)
                {
                    // Activate the button
                    signal.Trigger();
                    ChangeIndicatorMaterial(active);
                }

                BlocksOnButton++;
            }
            else
            {
                if (BlocksOnButton == 0)
                {
                    ChangeIndicatorMaterial(couldBeActive);
                }
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

            if (trigger.IsCurrentlyTriggering)
            {
                BlocksOnButton--;

                // If there isn't a block here anymore
                if (BlocksOnButton == 0)
                {
                    // Inactivate the button
                    signal.Trigger();
                }
            }

            if (objectsOnButton.Count == 0)
            {
                // Make the material inactive too
                ChangeIndicatorMaterial(inactive);
            }else if(objectsOnButton.Count > 0 && BlocksOnButton == 0)
            {
                ChangeIndicatorMaterial(couldBeActive);
            }
        }
    }

    bool stateChanged(bool newState)
    {
        if (BlocksOnButton == 0 && newState)
        {
            // Activate the button
            signal.Trigger();
            ChangeIndicatorMaterial(active);
        }

        BlocksOnButton += newState ? 1 : -1;

        if (BlocksOnButton == 0 && !newState)
        {
            // Inactivate the button
            signal.Trigger();
            ChangeIndicatorMaterial(couldBeActive);
        }

        return false;
    }

    void ChangeIndicatorMaterial(Material m)
    {
        for(int i = 0; i < indicators.Length; i++)
        {
            indicators[i].material = m;
        }
    }
}
