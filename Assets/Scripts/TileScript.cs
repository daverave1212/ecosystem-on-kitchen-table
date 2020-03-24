using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public Material grassMaterial;
    public Material waterMaterial;
    public Material sandMaterial;

    public static string GRASS = "grass";
    public static string WATER = "water";
    public static string SAND  = "sand";



    string type = "grass";
    int row = 0;
    int col = 0;

    public void Initialize(float x, float y, float z, int i, int j, string type) {
        var yOffset = 0.05f;
        this.type = type;
        gameObject.transform.position = new Vector3(x, y, z);
        var material = grassMaterial;
        if (type == "grass") {
            gameObject.transform.localScale += new Vector3(0, yOffset, 0);
            gameObject.transform.position += new Vector3(0, yOffset / 2, 0);
        }
        if (type == "sand") {
            material = sandMaterial;
        }
        if (type == "water") {
            material = waterMaterial;
            gameObject.transform.localScale -= new Vector3(0, yOffset, 0);
            gameObject.transform.position -= new Vector3(0, yOffset / 2, 0);
        }
        GetComponent<Renderer>().material = material;
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
