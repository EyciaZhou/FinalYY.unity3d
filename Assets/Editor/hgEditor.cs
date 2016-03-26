using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(hourglass_manager))]
public class hgEditor : Editor {
	private bool foldhg = false;
	private bool folddot = false;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		hourglass_manager hgm = (hourglass_manager)target;

		foldhg = EditorGUILayout.Foldout (foldhg, "hourglass");

		if (foldhg) {	
			EditorGUILayout.LabelField ("cnt", hgm.hourglasses.Count + "");

			foreach (KeyValuePair<System.Guid, hourglass_manager.hourglass> kp in hgm.hourglasses) {
				EditorGUILayout.LabelField (kp.Key + "", kp.Value.target + "");
			}
		}

		folddot = EditorGUILayout.Foldout (folddot, "dots");

		if (folddot) {
			EditorGUILayout.LabelField ("cnt", hgm.dots.Count + "");

			foreach (KeyValuePair<System.Guid, hourglass_manager.dot> kp in hgm.dots) {
				EditorGUILayout.LabelField (kp.Key + "", kp.Value.reach_total + "/" + kp.Value.target);
			}

		}

		EditorUtility.SetDirty( target );
	}
}
