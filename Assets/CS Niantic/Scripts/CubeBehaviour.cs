using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities.Logging;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using System;

public class CubeBehaviour : NetworkedBehaviour
{
    //private NetworkedField<Vector3> throwForce;

    protected override void SetupSession(out Action onNetworkingDidInitialize, out int order)
    {
        onNetworkingDidInitialize = () =>
        {
            SharedARBasic net = FindObjectOfType<SharedARBasic>();

            // Creating a new descriptor
            /*NetworkedDataDescriptor authToObserverDescriptor = Owner.Auth.AuthorityToObserverDescriptor(TransportType.UnreliableUnordered);

            throwForce = new NetworkedField<Vector3>
            (
                "force",
                authToObserverDescriptor,
                Owner.Group
            );*/

            if(Owner.SpawningPeer.Identifier != net.self.Identifier)
            {
                GameObject peerGameObject;

                if(net.playerIndicators.TryGetValue(Owner.SpawningPeer, out peerGameObject))
                {
                    //Attach the object to that peer
                    transform.SetParent(peerGameObject.transform);
                    transform.localPosition = Vector3.zero;
                }

            }
        };

        order = 0;
    }


    
}
