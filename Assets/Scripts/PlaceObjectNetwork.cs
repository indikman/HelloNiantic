using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Utilities.Input.Legacy;
using Niantic.ARDK.Networking;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Authority;


public class PlaceObjectNetwork : MonoBehaviour
{
    [SerializeField] private SharedARBasic arNetwork;

    // Object to instantiate over network
    [SerializeField] private NetworkedUnityObject placementObjectPrefab;

    [SerializeField] private Camera cam;

    Vector3 placedPosition;

    private NetworkedUnityObject SpawnObjectForAllPeers(Vector3 pos, Quaternion rot)
    {
        // This will spawn the object not just in the host, but also in other peers and return it
        return placementObjectPrefab.NetworkSpawn(arNetwork.networking, pos, rot, Role.Authority);
    }

    public void PlaceObject()
    {
        placedPosition = cam.transform.position + cam.transform.forward * 0.1f;

        // Get the touch input and instantiate an object
        SpawnObjectForAllPeers(placedPosition, cam.transform.rotation);
    }

    
}
