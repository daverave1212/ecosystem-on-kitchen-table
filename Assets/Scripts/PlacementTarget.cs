using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject placementIndicator;
    
    private Pose _targetPlacement;

    [SerializeField] private GameObject whatObject;
    
    [SerializeField]
    private ARRaycastManager raycastManager;

    private bool _placementPoseIsValid = false;

    private GameObject prefabRef;
    
    private void Update()
    { 
      UpdatePlacementPose();
      UpdatePlacementTarget();
      
      if (_placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
      {
          PlaceObject();
      }
    }

    private void PlaceObject()
    {
        if (!prefabRef)
        {
            prefabRef = Instantiate(whatObject, _targetPlacement.position, _targetPlacement.rotation);
        }
        else
        {
            prefabRef.transform.position = _targetPlacement.position;
        }
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
