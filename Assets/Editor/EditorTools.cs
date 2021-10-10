using UnityEditor;
using UnityEngine;

public static class EditorTools
{
	[MenuItem("Edit/Deselect All &d", false, -101)]
	static void Deselect()
	{
		Selection.activeGameObject = null;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Select ADVANCE &1", false, -101)]
	static void SelectAdvance()
	{
		SelectModel(true, false, false);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Select ADVANCE STYLE &2", false, -101)]
	static void SelectStyle()
	{
		SelectModel(false, true, false);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Select SPORT &3", false, -101)]
	static void SelectSport()
	{
		SelectModel(false, false, true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Enable ALL &4", false, -101)]
	static void SelectAll()
	{
		SelectModel(true, true, true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	static void SelectModel(bool bAdvance, bool bStyle, bool bSport)
	{
		GameObject advance = GameObject.Find("G_Car/Rotation_parent/T-ROC_Advance");
		GameObject style = GameObject.Find("G_Car/Rotation_parent/T-ROC_Advance_Style");
		GameObject sport = GameObject.Find("G_Car/Rotation_parent/T-ROC_Sport");

		advance.SetActive(bAdvance);
		style.SetActive(bStyle);
		sport.SetActive(bSport);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Activate OUTSIDE light &o", false, -101)]
	static void ActivateOut()
	{
		Activate(true);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	[MenuItem("Edit/Activate INSIDE light &i", false, -101)]
	static void ActivateIn()
	{
		Activate(false);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	static void Activate(bool isOut)
	{
		Transform out_active = GameObject.Find("G_Car/EXT_ACTIVE").transform;
		Transform int_active = GameObject.Find("G_Car/Rotation_parent/Touareg/COMMON/INT_ACTIVE").transform;

		foreach(Transform child in out_active){
			child.gameObject.SetActive(isOut);
		}
		foreach(Transform child in int_active){
			child.gameObject.SetActive(!isOut);
		}
	}
}