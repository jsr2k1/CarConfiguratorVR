using UnityEngine;
using System.Collections;

//[AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{
	public Transform target;
	public float distance = 5.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	public float zSpeed = 1.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;

	public float smoothTime = 2f;

	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;

	public float velocityX = 0.0f;
	public float velocityY = 0.0f;
	public float velocityZ = 0.0f;

	Rigidbody m_rigidbody;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;

		m_rigidbody = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (m_rigidbody)
		{
			m_rigidbody.freezeRotation = true;
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void LateUpdate()
	{
		if(target)
		{
			if(Input.GetMouseButton(0)){
				velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
				velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
			}
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);

			velocityZ += zSpeed * Input.GetAxis("Mouse ScrollWheel") * 0.02f;
			distance = distance - velocityZ;
			distance = Mathf.Clamp(distance, distanceMin, distanceMax);

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			transform.rotation = rotation;
			transform.position = position;

			velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
			velocityZ = Mathf.Lerp(velocityZ, 0, Time.deltaTime * smoothTime);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}