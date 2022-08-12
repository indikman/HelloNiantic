using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Niantic.ARDK.Utilities.Input.Legacy;

using Niantic.ARDK.AR;
using Niantic.ARDK.Utilities;

using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;

using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.AR.Awareness.Semantics;

using Niantic.ARDK.Extensions;


public class SemanticBasics : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ARSemanticSegmentationManager semanticManager;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text labelChannelAvailable;

    public string channelToCheck;

    ISemanticBuffer currentBuffer;
    Touch touch;

    void Start()
    {
        // Subcsribe to the semantic buffer
        semanticManager.SemanticBufferUpdated += OnSemanticBufferUpdated;
    }

    private void OnSemanticBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        // get the current awareness buffer and set that value
        currentBuffer = args.Sender.AwarenessBuffer;
    }

    
    void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) return;

        if (cam == null) return;

        touch = PlatformAgnosticInput.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            // Just show all the available segmentations - DEBUG
            /*Debug.Log("Number of Channels available : " + semanticManager.SemanticBufferProcessor.ChannelCount);
            foreach (var item in semanticManager.SemanticBufferProcessor.Channels)
            {
                Debug.Log(item);
            }*/

            // Get the touch input and get the list of channels in that point
            int x = (int)touch.position.x;
            int y = (int)touch.position.y;

            // You have to send the coordintes to get the available channels of that pixel
            int[] channelsInPixel = semanticManager.SemanticBufferProcessor.GetChannelIndicesAt(x, y);

            foreach (var item in channelsInPixel)
            {
                Debug.Log(item);
            }

            string[] channelNamesInPixel = semanticManager.SemanticBufferProcessor.GetChannelNamesAt(x, y);

            foreach (var item in channelNamesInPixel)
            {
                Debug.Log(item);
            }

            //Show the first label in text
            if (channelNamesInPixel.Length > 0)
                labelText.SetText(channelNamesInPixel[0]);

            //Check whether the set semantic it is
            if (semanticManager.SemanticBufferProcessor.DoesChannelExistAt(x, y, channelToCheck))
            {
                labelChannelAvailable.SetText(channelToCheck + " is available");
            }
            else
            {
                labelChannelAvailable.SetText(channelToCheck + " is NOT available");
            }

        }
    }
}
