using UnityEngine;
using System.Collections;

public class RotationController : MonoBehaviour
{
	public float rot_speed = 20;
	public enum AxisTypes{
		X,
		Y,
		Z
	}
	public AxisTypes rotAxis;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, Globals.instance.current_rotation, transform.eulerAngles.z);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Update()
	{
		if(Globals.instance.bCamModeRotation)
		{
			if(Globals.instance.rotate_left)
				transform.Rotate((rotAxis == AxisTypes.Y ? Vector3.up : Vector3.forward) * rot_speed * Time.deltaTime);

			if(Globals.instance.rotate_right)
				transform.Rotate((rotAxis == AxisTypes.Y ? Vector3.up : Vector3.forward) * -rot_speed * Time.deltaTime);
		}

//		Globals.instance.current_rotation = transform.eulerAngles.y;
//		iTween.RotateTo(gameObject, Vector3.up*Globals.instance.current_rotation, 0.5F);
	}
}
