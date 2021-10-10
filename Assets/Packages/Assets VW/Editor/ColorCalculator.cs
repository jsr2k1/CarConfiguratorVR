using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ColorCalculator : EditorWindow 
{
	private Color _color;
    private Color _linearcolor;

    [MenuItem("Gridcell/Color Calculator")]

	static void Init() 
	{
		UnityEditor.EditorWindow window = GetWindow(typeof(ColorCalculator));
		window.position = new Rect(100, 100, 250, 150);
		window.Show();
		
	}

	void OnInspectorUpdate() 
	{
		Repaint();
	}

	void OnGUI() 
	{           
		_color = (Color)UnityEditor.EditorGUI.ColorField(new Rect(3, 3, position.width - 6, 20), "Gamma", _color);
        GUI.Label(new Rect(3, 3 + 25, position.width - 6, 20), _color.ToString());
        _linearcolor = (Color)UnityEditor.EditorGUI.ColorField(new Rect(3, 3 + 50, position.width - 6, 20), "Linear", _linearcolor);
        GUI.Label(new Rect(3, 3 + 75, position.width - 6, 20), _linearcolor.ToString());
        if (GUI.Button(new Rect(3, 3 + 100, position.width - 6, 20), "Gamma -> Linear"))
		{
            _linearcolor = _color.linear;
		}
        if (GUI.Button(new Rect(3, 3 + 125, position.width - 6, 20), "Linear -> Gamma"))
        {
            _color = _linearcolor.gamma;
        }
    }
}


	