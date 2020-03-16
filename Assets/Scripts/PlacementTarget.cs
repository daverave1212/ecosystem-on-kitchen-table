using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementTarget : MonoBehaviour
{
    [SerializeField]
    private ARSessionOrigin arOrigin;

    [SerializeField]
    private GameObject placementIndicator;
    
    private Pose _targetPlacement;
    
    [SerializeField]
    private ARRaycastManager raycastManager;

    private bool _placementPoseIsValid = false;
    
    private void Update()
    { 
      UpdatePlacementPose();
      UpdatePlacementTarget();
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        _placementPoseIsValid = hits.Count > 0;
        if (_placementPoseIsValid)
        {
            _targetPlacement = hits[0].pose;
        }
    }

    private void UpdatePlacementTarget()
    {
        if (_placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(_targetPlacement.position, _targetPlacement.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }

    }
}
