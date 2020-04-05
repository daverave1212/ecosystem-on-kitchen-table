using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    
    public Vector3 fromPoint;
    public Vector3 toPoint;

    public float speed = 1f;
    public float startTime;
    public float journeyLength;

    bool isSliding = false;

    public void SlideTo(Vector3 to) {
        fromPoint = gameObject.transform.position;
        toPoint = to;
        startTime = Time.time;
        journeyLength = Vector3.Distance(fromPoint, toPoint);
        isSliding = true;
    }

    void Update() {
        if (!isSliding) return;
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(fromPoint, toPoint, fractionOfJourney);
        if (fractionOfJourney >= 1) {
            isSliding = false;
        }
    }
}
