using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(player))]
public class pEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		player p = (player)target;

		if (p.exp != null) {
			EditorGUILayout.LongField ("level", p.exp.lv);
			EditorGUILayout.LongField ("exp", p.exp.limit);
			EditorGUILayout.FloatField ("exp_reach", p.exp.current);
			EditorGUILayout.LongField ("hp", p.hp.point);
			EditorGUILayout.LongField ("hp_max", p.hp.max_point);
		}
	}
}
