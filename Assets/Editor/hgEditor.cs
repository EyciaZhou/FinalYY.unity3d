using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(hourglass_manager))]
public class hgEditor : Editor {
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();


		hourglass_manager hgm = (hourglass_manager)target;
	}
}
