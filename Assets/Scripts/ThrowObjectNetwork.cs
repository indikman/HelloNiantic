using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.Networking;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;


public class ThrowObjectNetwork : MonoBehaviour
{
    // Object to instantiate over network
    [SerializeField] private NetworkedUnityObject throwObjectPrefab;


    [SerializeField] private float throwForce;
    [SerializeField] private Camera cam;
    [SerializeField] private float timeToDestroy = 10;

    Touch touch;
    Vector3 throwPosition;
    NetworkedUnityObject throwObject;

    
    
    private NetworkedUnityObject SpawnObjectForAllPeers()
    {
        // This will spawn the object not just in the host, but also in other peers and return it
        return throwObjectPrefab.NetworkSpawn();
    }

    
    private void DestroyObjectFromAllPeers(NetworkedUnityObject objectToDestroy)
    {
        // This method will destroy the spawned object from all peers
        objectToDestroy.NetworkDestroy();
    }

    private void DestroyObjectFromAllPeers(NetworkedUnityObject objectToDestroy, float delay)
    {
        // This method will destroy the spawned object from all peers, after a delay
        StartCoroutine(NetworkDestroyObjectWithDelay(objectToDestroy, delay));
    }


    void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) return;
        if (cam == null || throwObjectPrefab == null) return;

        touch = PlatformAgnosticInput.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            throwPosition = cam.transform.position + cam.transform.forward;

            // Get the touch input and instantiate an object

            throwObject = SpawnObjectForAllPeers();
            throwObject.transform.position = throwPosition;
            throwObject.transform.rotation = Quaternion.identity;

            throwObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce);

            // Destroy the object after a delay from the network
            DestroyObjectFromAllPeers(throwObject, timeToDestroy);
        }

    }

    IEnumerator NetworkDestroyObjectWithDelay(NetworkedUnityObject objectToDestroy, float delay)
    {
        // Temp object to hold the value
        NetworkedUnityObject tempObj = objectToDestroy;

        yield return new WaitForSeconds(delay);

        tempObj.NetworkDestroy();
    }
}
