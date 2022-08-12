using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Authority;


public class ThrowObjectNetwork : MonoBehaviour
{

    [SerializeField] private SharedARBasic arNetwork;

    // Object to instantiate over network
    [SerializeField] private NetworkedUnityObject throwObjectPrefab;


    [SerializeField] private float throwForce;
    [SerializeField] private Camera cam;
    [SerializeField] private float timeToDestroy = 10;

    Touch touch;
    Vector3 throwPosition;
    GameObject throwObject;

    
    
    private NetworkedUnityObject SpawnObjectForAllPeers(Vector3 pos, Quaternion rot)
    {
        // This will spawn the object not just in the host, but also in other peers and return it
        return throwObjectPrefab.NetworkSpawn(arNetwork.networking, pos, rot, Role.Authority);
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
            throwPosition = cam.transform.position + cam.transform.forward * 0.2f;

            // Get the touch input and instantiate an object

            throwObject = SpawnObjectForAllPeers(throwPosition, Quaternion.identity).gameObject;

            throwObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce);

            // Destroy the object after a delay from the network
            DestroyObjectFromAllPeers(throwObject.GetComponent<NetworkedUnityObject>(), timeToDestroy);
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
