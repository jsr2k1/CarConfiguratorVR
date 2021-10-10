using UnityEditor;

public static class DeselectAll
{
	[MenuItem("Edit/Deselect All &d", false, -101)]
	static void Deselect() {
		Selection.activeGameObject = null;
	}
}