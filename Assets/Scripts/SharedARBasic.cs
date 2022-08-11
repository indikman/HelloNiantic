using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Niantic.ARDK;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.AR;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.AR.Awareness.Semantics;

public class SharedARBasic : MonoBehaviour
{
    // Prefab that indicates the players
    [SerializeField] private GameObject playerIndicator;

    // ARDK referrences
    private IARNetworking arNetworking;
    private IMultipeerNetworking networking;
    private IARSession arSession;

    // Dictionary to hold the players connected
    private Dictionary<IPeer, GameObject> playerIndicators = new Dictionary<IPeer, GameObject>();


    private void Awake()
    {
        // Setting up an event to execute OnInitialized when the networking initialized
        ARNetworkingFactory.ARNetworkingInitialized += OnInitialized;
    }

    private void OnInitialized(AnyARNetworkingInitializedArgs args)
    {
        // Assigning values from the current AR Network
        arNetworking = args.ARNetworking;
        arSession = arNetworking.ARSession;
        networking = arNetworking.Networking;

        arNetworking.PeerStateReceived += OnPeerStateReceived;
        arNetworking.PeerPoseReceived += OnPeerPoseReceived;

    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.Log("New peer joined! " + args.Peer + " - " + args.State);
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        // This method will be executed all the time when a player moves in the AR session
         
        // If the player is not in the dictionary, we have to create an indicator
        if (!playerIndicators.ContainsKey(args.Peer))
        {
            // A new gameobject will be created for the player pose
            playerIndicators.Add(args.Peer, Instantiate(playerIndicator));
        }

        // TryGetValue will check if a peer ID is available, and change the poseIndicator to the associated GameObject
        GameObject poseIndicator;
        if(playerIndicators.TryGetValue(args.Peer, out poseIndicator))
        {
            // position of the indicator will be updated
            poseIndicator.transform.position = args.Pose.GetPosition() + new Vector3(0,0.1f,0.1f);
        }
    }

}
