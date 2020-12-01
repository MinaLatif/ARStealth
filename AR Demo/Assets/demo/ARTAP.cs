using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;

public class ARTAP : MonoBehaviour
{
    public GameObject objectplaced;
    public GameObject placementIndicator;
    private ARRaycastManager arCast;
    private Pose placementpose; 
 
 
    void Start()
    {
        arCast = FindObjectOfType<ARRaycastManager>();
        placementIndicator = transform.GetChild(0).gameObject;

        placementIndicator.SetActive(false);

    }

    void Update()
    {
        

        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arCast.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            placementpose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementpose.rotation = Quaternion.LookRotation(cameraBearing);

            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);    
        }
        else
        {
            placementIndicator.SetActive(false);
        }

        if(hits.Count > 0 && Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            placeobject();
        }
    }

    private void placeobject()
    {
        Instantiate(objectplaced, placementpose.position, placementpose.rotation);
    }

    /*private void updatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            
        }
    }

    private void updatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arCast.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0; 
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
        }
    }*/
}
