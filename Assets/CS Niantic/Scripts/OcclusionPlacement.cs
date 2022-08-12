using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.Utilities.Input.Legacy;

using Niantic.ARDK.AR;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;


public class OcclusionPlacement : MonoBehaviour
{

    [SerializeField] private GameObject placementObject;
    [SerializeField] private Camera cam;

    Touch touch;
    IARSession session;

    void Start()
    {
        ARSessionFactory.SessionInitialized += OnSessionInitialized;
    }

    private void OnSessionInitialized(AnyARSessionInitializedArgs args)
    {
        ARSessionFactory.SessionInitialized -= OnSessionInitialized;
        session = args.Session;
    }


    void Update()
    {
        if(PlatformAgnosticInput.touchCount <=0)
        {
            return;
        }

        touch = PlatformAgnosticInput.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            TouchBegan(touch);
        }
    }

    void TouchBegan(Touch touch)
    {
        // check a valid frame
        var currentFrame = session.CurrentFrame;

        if (currentFrame == null) return;

        if (cam == null) return;

        var hit = currentFrame.HitTest(cam.pixelWidth, cam.pixelHeight, touch.position, ARHitTestResultType.ExistingPlaneUsingExtent | ARHitTestResultType.EstimatedHorizontalPlane);

        if (hit.Count == 0) return;

        // Move the object to the position
        placementObject.transform.position = hit[0].WorldTransform.ToPosition();

        // lookat
        placementObject.transform.LookAt(new Vector3(
                currentFrame.Camera.Transform.GetPosition().x,
                placementObject.transform.position.y,
                currentFrame.Camera.Transform.GetPosition().z
            ));

    }
}
