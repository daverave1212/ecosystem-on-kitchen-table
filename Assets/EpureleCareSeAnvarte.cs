using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpureleCareSeAnvarte : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 45.0f;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
    }
}
