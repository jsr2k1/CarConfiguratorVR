using UnityEngine;
using System.Collections;

public class InsideCamera : MonoBehaviour
{
	public BoxCollider boxCollider;
	bool contains;

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		StartCoroutine(CheckPosition());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	IEnumerator CheckPosition()
	{
		yield return new WaitForSeconds(0.5F);
		contains = PointInOABB(transform.position, boxCollider);
//		Debug.Log(contains);
		Globals.instance.camPosition = contains ? Globals.CamPositions.INTERIOR : Globals.CamPositions.EXTERIOR;
		StartCoroutine(CheckPosition());
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	bool PointInOABB(Vector3 point, BoxCollider box)
	{
		Vector3 new_point = box.transform.InverseTransformPoint(point) - box.center;

		float halfX = (box.size.x * 0.5f);
//		float halfY = (box.size.y * 0.5f);
		float halfZ = (box.size.z * 0.5f);

//		if(new_point.x < halfX && new_point.x > -halfX && new_point.y < halfY && new_point.y > -halfY && new_point.z < halfZ && new_point.z > -halfZ)
		if(new_point.x < halfX && new_point.x > -halfX && new_point.z < halfZ && new_point.z > -halfZ)
			return true;
		else
			return false;
	}
}
