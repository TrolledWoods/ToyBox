using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Signal : MonoBehaviour {

    public static bool[] SignalStates;
    static int Signals = 20;

    public bool[] signals;
    public int[] controlling;

    public MeshRenderer[] isActiveIndicators;
    public Material activeMaterial;
    public Material inactiveMaterial;

    public Text[] signalIndicators;

    void Awake ()
    {
        if(SignalStates == null)
        {
            SignalStates = new bool[Signals];
        }

        signals = SignalStates;

        // Set the signal indicators to what you're controlling
        string indicatorText = "";

        for(int i = 0; i < controlling.Length; i++)
        {
            indicatorText += (controlling[i] < 0 ? "!" : "") + getControllingIndex(i);
            if(i < controlling.Length - 1)
            {
                indicatorText += '-';
            }
        }

        for(int i = 0; i < signalIndicators.Length; i++)
        {
            signalIndicators[i].text = indicatorText;
        }
    }
    
    /// <summary>
    /// Inverts all the signals that this signal is controlling
    /// </summary>
    public void Trigger()
    {
        // Don't trigger stuff if the SignalStates array is null
        if (SignalStates == null) return;

        for(int i = 0; i < controlling.Length; i++)
        {
            SignalStates[getControllingIndex(i)] = !SignalStates[getControllingIndex(i)];
        }
    }

    void Update()
    {
        bool happy = IsHappy();

        // Update the graphics for the mesh indicators
        for (int i = 0; i < isActiveIndicators.Length; i++)
        {
            isActiveIndicators[i].material = happy ? activeMaterial : inactiveMaterial;
        }
    }

    /// <summary>
    /// Returns true if all the states the signal is controlling are the desired value
    /// </summary>
    public bool IsHappy()
    {
        for (int i = 0; i < controlling.Length; i++)
        {
            if((!(SignalStates == null) && SignalStates[getControllingIndex(i)]) != (controlling[i] >= 0))
            {
                return false;
            }
        }

        return true;
    }

    int getControllingIndex(int index)
    {
        return controlling[index] < 0 ? (controlling[index] * -1 - 1) : controlling[index];
    }
}
