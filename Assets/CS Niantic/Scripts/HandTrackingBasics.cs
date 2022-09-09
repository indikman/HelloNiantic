using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDK.AR.Awareness;
using ARDK.Extensions;

public class HandTrackingBasics : MonoBehaviour
{
    [SerializeField]
    ARHandTrackingManager handTrackingManager;

    void Start()
    {
        handTrackingManager.HandTrackingUpdated += OnHandTrackingUpdated;
    }

    private void OnHandTrackingUpdated(HumanTrackingArgs args)
    {
        var data = args.TrackingData;

        for (int i = 0; i < data.AlignedDetections.Count; i++)
        {
            var item = data.AlignedDetections[i];
            Debug.Log($"{item.X} - {item.Y} - {item.Width} - {item.Height}");
        }
    }
}
