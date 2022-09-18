using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AxisYLock : MonoBehaviour
{
    public float YMin;
    private XRGrabInteractable interactable;
    void Start()
    {
        interactable = this.GetComponent<XRGrabInteractable>();
    }
    void Update()
    {
        if (!interactable.isSelected)
        {
            if (transform.position.y < YMin)
            {
                StartCoroutine(Recover());
            }
        }
    }
    IEnumerator Recover()
    {
        while(transform.position.y < YMin)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, YMin, Time.deltaTime * 1.5f), transform.position.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 1.5f);
            yield return new WaitForEndOfFrame();
        }
    }
}
