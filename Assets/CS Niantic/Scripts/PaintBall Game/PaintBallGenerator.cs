using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Authority;

public class PaintBallGenerator : MonoBehaviour
{
    [SerializeField] private GameNetworkManager arNetwork;

    // Object to instantiate over network
    [SerializeField] private NetworkedUnityObject throwObjectPrefab;


    [SerializeField] private float throwForce;
    [SerializeField] private Camera cam;
    [SerializeField] private float timeToDestroy = 10;

    Vector3 throwPosition;
    GameObject throwObject;

    
    public void ShootPaintBall()
    {
        throwPosition = cam.transform.position + cam.transform.forward * 0.2f;
        throwObject = SpawnObjectForAllPeers(throwPosition, Quaternion.identity).gameObject;

        //Setup the generator of the paintball
        throwObject.GetComponent<PaintBall>().SetGenerator(this);

        // Throw the object
        throwObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce);

        // Destroy the object after a delay from the network
        DestroyObjectFromAllPeers(throwObject.GetComponent<NetworkedUnityObject>(), timeToDestroy);
    }

    private NetworkedUnityObject SpawnObjectForAllPeers(Vector3 pos, Quaternion rot)
    {
        // This will spawn the object not just in the host, but also in other peers and return it
        return throwObjectPrefab.NetworkSpawn(arNetwork.networking, pos, rot, Role.Authority);
    }


    public void DestroyObjectFromAllPeers(NetworkedUnityObject objectToDestroy)
    {
        // This method will destroy the spawned object from all peers
        objectToDestroy.NetworkDestroy();
    }

    public void DestroyObjectFromAllPeers(NetworkedUnityObject objectToDestroy, float delay)
    {
        // This method will destroy the spawned object from all peers, after a delay
        StartCoroutine(NetworkDestroyObjectWithDelay(objectToDestroy, delay));
    }

    IEnumerator NetworkDestroyObjectWithDelay(NetworkedUnityObject objectToDestroy, float delay)
    {
        // Temp object to hold the value
        NetworkedUnityObject tempObj = objectToDestroy;

        yield return new WaitForSeconds(delay);

        tempObj.NetworkDestroy();
    }
}
