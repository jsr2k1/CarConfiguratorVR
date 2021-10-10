using UnityEngine;
using System.Collections;

public class RotationControllerClient : MonoBehaviour
{
	void LateUpdate()
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, Globals.instance.current_rotation, transform.eulerAngles.z);
	}
}
