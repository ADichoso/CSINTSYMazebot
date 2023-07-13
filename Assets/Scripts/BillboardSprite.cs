using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(cam != null) transform.LookAt(transform.position + cam.forward);
    }
}
