using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraDisplayFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (Display.displays.Length <= 1)
        {
            GetComponent<Camera>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
