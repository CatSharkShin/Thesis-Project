using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class XrGrabbableMover : MonoBehaviour
{
    public Transform[] AttachTransformsToAffect;
    public InputActionReference rotate;
    public InputActionReference translate;
    public InputActionReference grab;
    public bool _grabbed = false;
    private void Awake()
    {
        grab.action.performed += GrabToggle;
    }
    private void OnDestroy()
    {
        grab.action.performed -= GrabToggle;
    }
    private void Update()
    {
        Vector2 pos = translate.action.ReadValue<Vector2>();
        Vector2 rot = rotate.action.ReadValue<Vector2>();
        if (_grabbed)
        {
            foreach(Transform attach in AttachTransformsToAffect)
            {
                attach.position = (transform.position - attach.position).normalized * pos.y;
                attach.localRotation *= Quaternion.Euler(0f,0f, rot.x);
            }
        }
    }
    private void GrabToggle(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Started)
        {
            _grabbed = true;
        }
        if (ctx.phase == InputActionPhase.Canceled)
        {
            _grabbed = false;
        }
    }
}
