using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SignalTextRenderer : MonoBehaviour {

    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        string txt = "";

        if (Signal.SignalStates != null)
        {
            for (int i = 0; i < Signal.SignalStates.Length; i++)
            {
                txt += i + ": " + (Signal.SignalStates[i] ? "true" : "false") + '\n';
            }
        }

        text.text = txt;
    }
}
