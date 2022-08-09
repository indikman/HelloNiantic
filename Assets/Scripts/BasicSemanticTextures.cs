using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

using Niantic.ARDK.AR;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.AR.Awareness;
using Niantic.ARDK.AR.Awareness.Semantics;

public class BasicSemanticTextures : MonoBehaviour
{
    public ARSemanticSegmentationManager semanticManager;
    public RawImage overlayImage;
    public TMP_Dropdown availableChannelsDropDown;

    string channelName = "sky";
    Texture2D semanticTexture;


    void Start()
    {
        semanticManager.SemanticBufferInitialized += OnSemanticBufferInitialized;
        semanticManager.SemanticBufferUpdated += OnSemanticBufferUpdated;
    }

    private void OnSemanticBufferInitialized(ContextAwarenessArgs<ISemanticBuffer> args)
    {
        semanticManager.SemanticBufferInitialized -= OnSemanticBufferInitialized;

        // clear dropdown
        availableChannelsDropDown.ClearOptions();

        // Get available channels
        string[] channels = semanticManager.SemanticBufferProcessor.Channels;
        List<string> channelsList = channels.OfType<string>().ToList();

        // Add all available channels to the dropdown
        availableChannelsDropDown.AddOptions(channelsList);

        availableChannelsDropDown.onValueChanged.AddListener(OnChannelChanged);
    }

    public void OnChannelChanged(int item)
    {
        channelName = availableChannelsDropDown.options[item].text;
        Debug.Log(channelName);
    }

    private void OnSemanticBufferUpdated(ContextAwarenessStreamUpdatedArgs<ISemanticBuffer> args)
    {
        ISemanticBuffer semanticBuffer = args.Sender.AwarenessBuffer;

        int channel = semanticBuffer.GetChannelIndex(channelName);

        semanticManager.SemanticBufferProcessor.CopyToAlignedTextureARGB32(
            channel: channel,
            texture: ref semanticTexture,
            orientation: Screen.orientation
            );

        overlayImage.texture = semanticTexture;
    }

    private void OnDestroy()
    {
        semanticManager.SemanticBufferUpdated -= OnSemanticBufferUpdated;
    }

}
