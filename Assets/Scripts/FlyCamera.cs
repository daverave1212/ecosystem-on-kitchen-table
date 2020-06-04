using UnityEngine;
using System.Collections;
 
public class FlyCamera : MonoBehaviour {
    
    float speed = 10;

    void Update() {
        if (Input.GetKey("s")) { gameObject.transform.position += new Vector3(speed * Time.deltaTime, 0, 0); }
        if (Input.GetKey("w")) { gameObject.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0); }
        if (Input.GetKey("d")) { gameObject.transform.position += new Vector3(0, 0, speed * Time.deltaTime); }
        if (Input.GetKey("a")) { gameObject.transform.position -= new Vector3(0, 0, speed * Time.deltaTime); }
    }

}