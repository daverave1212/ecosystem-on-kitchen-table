using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementTarget : MonoBehaviour
{
    [SerializeField] private GameObject placementIndicator;

    private Pose _targetPlacement;

    [SerializeField] private GameObject whatObject;

    [SerializeField] private ARRaycastManager raycastManager;

    [SerializeField] private List<GameObject> arHelpers;

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
            prefabRef = Instantiate(whatObject, Vector3.zero, _targetPlacement.rotation);
            StartCoroutine(ParentHack(_targetPlacement.position));
        }
        else
        {
            prefabRef.transform.position = _targetPlacement.position * 15;
        }
    }

    IEnumerator ParentHack(Vector3 pos)
    {
        yield return null;
        var objectCenter = new GameObject();
        List<GameObject> generatedItems = new List<GameObject>();
        generatedItems.AddRange(GameObject.FindGameObjectsWithTag("Plant"));
        generatedItems.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        generatedItems.AddRange(GameObject.FindGameObjectsWithTag("Animal"));
        generatedItems.AddRange(GameObject.FindGameObjectsWithTag("Particles"));
        foreach (var generatedItem in generatedItems)
        {
            generatedItem.transform.parent = objectCenter.transform;
        }

        objectCenter.transform.position = pos * 15;
        prefabRef.transform.localScale = Vector3.zero;
        prefabRef = objectCenter;
    }

    private void UpdatePlacementPose()
    {
        if (Camera.current)
        {
            Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            RaycastHit physicsHit;
            Physics.Raycast(Camera.current.transform.position, Camera.current.transform.forward, out physicsHit,
                Mathf.Infinity);
            

            _placementPoseIsValid = hits.Count > 0 && physicsHit.collider == null;
            if (_placementPoseIsValid)
            {
                _targetPlacement = hits[0].pose;
                arHelpers.ForEach(helper => helper.SetActive(false));
            }
            else
            {
                arHelpers.ForEach(helper => helper.SetActive(true));
            }
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