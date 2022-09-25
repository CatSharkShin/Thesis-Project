using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRBetterDirectInteractor : XRDirectInteractor
{
    public InputActionReference gripForce;
    public float gripTreshold;
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return gripForce.action.ReadValue<float>() > gripTreshold;
    }
}