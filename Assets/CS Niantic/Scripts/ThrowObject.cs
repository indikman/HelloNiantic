using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Utilities.Input.Legacy;


public class ThrowObject : MonoBehaviour
{
    public GameObject throwObjectPrefab;
    public float throwForce;
    public Camera cam;
    public float timeToDestroy = 10;

    Touch touch;
    Vector3 throwPosition;
    GameObject throwObject;

    void Start()
    {
        
    }

   
    void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) return;
        if (cam == null || throwObjectPrefab == null) return;

        touch = PlatformAgnosticInput.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            throwPosition = cam.transform.position + cam.transform.forward;

            throwObject = Instantiate(throwObjectPrefab, throwPosition, Quaternion.identity);

            throwObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce);

            Destroy(throwObject, timeToDestroy);
        }

    }
}
