﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countAppOpen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (Globals.firstopen) {
            LogUseApp.SaveOpenApp();
            Globals.firstopen = false;
		}
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}