using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
//	bool m_enable;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
//		m_enable = UnityEngine.VR.VRSettings.loadedDeviceName == "Oculus";
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		//Camera movement
		Globals.instance.move_left = Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.Left);
		Globals.instance.move_right = Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.Right);
		Globals.instance.move_forward = Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.Up);
		Globals.instance.move_backwards = Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.Down);

		Globals.instance.move_up = Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.Button.Up);
		Globals.instance.move_down = Input.GetKey(KeyCode.E) || OVRInput.Get(OVRInput.Button.Down);

		//Car rotation
		Globals.instance.rotate_left = Input.GetKey(KeyCode.LeftArrow) || OVRInput.Get(OVRInput.Button.Left);
		Globals.instance.rotate_right = Input.GetKey(KeyCode.RightArrow) || OVRInput.Get(OVRInput.Button.Right);

		//NOTA: Para que funcione el mando de las Oculus tiene que haber un objeto en la escena con el componente OVRManager
		//Oculus remote: button one
		if(OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
			Globals.instance.bCamModeRotation = !Globals.instance.bCamModeRotation;
		}
	}
}
