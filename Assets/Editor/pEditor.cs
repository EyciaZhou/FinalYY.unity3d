using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(player))]
public class pEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		player p = (player)target;

		if (p.exp != null) {
			EditorGUILayout.LabelField ("level", p.exp.lv + "");
		}
	}
}
