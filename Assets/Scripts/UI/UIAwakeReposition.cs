using UnityEngine;

public class UIAwakeReposition : MonoBehaviour
{
	public Vector3 localPosition;
	public Vector3 localEulerAngles;
	public Vector3 localScale;

	private void Awake ()
	{
		Transform tr = GetComponent<Transform>();
		tr.localPosition = localPosition;
		tr.localEulerAngles = localEulerAngles;
		tr.localScale = localScale;
		Destroy(this);
	}
}
