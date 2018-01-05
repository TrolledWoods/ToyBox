using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTriggeringObject : MonoBehaviour {

    public struct eventWrapper
    {
        public Func<bool, bool> EventFunction;
        public IsTriggeringObject trigger;
        public eventWrapper(Func<bool, bool> eventFunc, IsTriggeringObject trigger) {
            EventFunction = eventFunc;
            this.trigger = trigger; }
    }

    bool IsTriggering = true;
    public bool IsCurrentlyTriggering { get { return IsTriggering; } }

    List<eventWrapper> changedStatusFuncs;

    void Awake()
    {
        changedStatusFuncs = new List<eventWrapper>();
    }

    public void SetIsTriggering(bool Value)
    {
        if(Value != IsTriggering)
        {
            // Give all the things that want the status an update
            for(int i = 0; i < changedStatusFuncs.Count; i++)
            {
                changedStatusFuncs[i].EventFunction(Value);
            }
        }

        IsTriggering = Value;
    }

    /// <summary>
    /// Adds a function that should be called on an event
    /// </summary>
    /// <param name="EventFunction">Gives the new status, return whatever you want</param>
    public void AddChangedStatusFunction(eventWrapper eventFunc)
    {
        changedStatusFuncs.Add(eventFunc);
    }

    public void RemoveChangedStatusFunction(eventWrapper eventFunc)
    {
        changedStatusFuncs.Remove(eventFunc);
    }

}
