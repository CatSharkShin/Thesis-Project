using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRayController : MonoBehaviour
{
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AlignToCamera();
    }
    void AlignToCamera()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit,100))
        {
            transform.LookAt(hit.point);
        }
        else
        {
            transform.LookAt(camera.transform.position + ray.direction.normalized*10);

        }
        /*
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
            transform.LookAt(hit.point);
        }*/
    }
}
