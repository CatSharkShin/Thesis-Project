using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class BetterToggle : Toggle
{
    public ToggleEvent onValueChangedReverse;
    public void Awake()
    {
        onValueChanged.AddListener(Reverse);
    }
    void Reverse(bool value)
    {
        onValueChangedReverse.Invoke(!value);
    }
}
