using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform cam_transform;
	public Transform[] pos_int;
	public Transform pos_ext; //de momento, no se usa
	public float speed;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		Messenger.AddListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);
		Messenger.AddListener(MessengerEventsEnum.CHANGE_INT_POS, OnChangeIntPos);
		Messenger<int>.AddListener(MessengerEventsEnum.MOVE_CAMERA, OnMoveCamera);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		OnChangeGlobalPos();
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnDestroy()
	{
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_GLOBAL_POS, OnChangeGlobalPos);
		Messenger.RemoveListener(MessengerEventsEnum.CHANGE_INT_POS, OnChangeIntPos);
		Messenger<int>.RemoveListener(MessengerEventsEnum.MOVE_CAMERA, OnMoveCamera);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnChangeGlobalPos()
	{
		if(Globals.instance.camPosition == Globals.CamPositions.EXTERIOR){
			transform.position = GetCurrentPos().position;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, GetCurrentPos().eulerAngles.y, transform.eulerAngles.z);
			Globals.instance.camPosition = Globals.CamPositions.INTERIOR;
		} else{
			transform.position = pos_ext.position;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, pos_ext.eulerAngles.y, transform.eulerAngles.z);
			Globals.instance.camPosition = Globals.CamPositions.EXTERIOR;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnChangeIntPos()
	{
		transform.position = GetCurrentPos().position;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	Transform GetCurrentPos()
	{
		return pos_int[(int)Globals.instance.currentModel];
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void OnMoveCamera(int dir)
	{
		Vector3 dir_forward = new Vector3(cam_transform.forward.x, 0, cam_transform.forward.z);
		Vector3 new_pos = transform.position + dir * dir_forward * speed;
		iTween.MoveTo(gameObject, new_pos, 1F);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

		float speed_mult = isShiftKeyDown ? 2F : 1F;

		Vector3 dir_forward = new Vector3(cam_transform.forward.x, 0, cam_transform.forward.z);
		Vector3 dir_right = new Vector3(cam_transform.right.x, 0, cam_transform.right.z);

		//Move camera
//		if(!Globals.instance.bCamModeRotation)
//		{
			if(Globals.instance.move_forward)
				transform.Translate(dir_forward * speed * speed_mult * Time.deltaTime, Space.World);

			if(Globals.instance.move_backwards)
				transform.Translate(dir_forward * -speed * speed_mult * Time.deltaTime, Space.World);

			if(Globals.instance.move_left)
				transform.Translate(dir_right * -speed * speed_mult * Time.deltaTime, Space.World);

			if(Globals.instance.move_right)
				transform.Translate(dir_right * speed * speed_mult * Time.deltaTime, Space.World);
//		}
		//Rotate car
//		else{
			if(Globals.instance.move_up)
				transform.Translate(Vector3.up * speed * speed_mult * Time.deltaTime);

			if(Globals.instance.move_down)
				transform.Translate(Vector3.up * -speed * speed_mult * Time.deltaTime);
//		}
	}
}
