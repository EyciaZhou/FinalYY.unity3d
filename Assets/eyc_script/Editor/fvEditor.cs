using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(fire_manager))]
public class fvEditor : Editor {
	private bool foldfv = false;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		fire_manager fv = (fire_manager)target;

		foldfv = EditorGUILayout.Foldout (foldfv, "fire_manager");

		if (foldfv) {	
			EditorGUILayout.LabelField ("cnt", fv.fireballs.Count + "");

			foreach (KeyValuePair<System.Guid, fireball> kp in fv.fireballs) {
				EditorGUILayout.ObjectField (kp.Key + "", kp.Value.target, typeof(GameObject));
			}
		}

		EditorUtility.SetDirty( target );
	}
}
