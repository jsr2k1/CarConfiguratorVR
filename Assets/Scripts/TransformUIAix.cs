using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUIAix : MonoBehaviour {
    public bool NoRootX = true;
    public bool NoRootY = true;
    public bool NoRootZ = true;
    public float RootX = 0;
    public float RootY = 0;
    public float RootZ = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TransformUI()
    {
        Vector3 rotation = GetComponent<RectTransform>().localRotation.eulerAngles;
        float RX = NoRootX ? rotation.x : RootX;
        float RY = NoRootY ? rotation.y : RootY;
        float RZ = NoRootZ ? rotation.z : RootZ;

        GetComponent<RectTransform>().localRotation = Quaternion.Euler(RX, RY, RZ);
    }
}
