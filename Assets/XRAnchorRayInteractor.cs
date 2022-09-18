using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRAnchorRayInteractor : XRRayInteractor
{
    public ActionBasedContinuousTurnProvider TurnProvider;
    public ActionBasedContinuousMoveProvider MoveProdiver;
    private float _turnProviderSpeed;
    private float _moveProviderSpeed;
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        _moveProviderSpeed = MoveProdiver.moveSpeed;
        MoveProdiver.moveSpeed = 0;

        _turnProviderSpeed = TurnProvider.turnSpeed;
        TurnProvider.turnSpeed = 0;
    }
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        TurnProvider.turnSpeed = _turnProviderSpeed;
        MoveProdiver.moveSpeed = _moveProviderSpeed;
    }
}
