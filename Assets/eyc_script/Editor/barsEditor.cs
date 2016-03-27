using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(bars))]
public class barsEditor : Editor {

	[MenuItem ("SB FINAL YY/bar/Add")]
	static void  AddBar() {
		GameObject go = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/view/bars/bars.prefab");
		Instantiate (go);
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Build Object"))
		{
			Debug.Log ("aaa");
		}
	}
}
