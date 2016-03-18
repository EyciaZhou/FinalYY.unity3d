
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object;
	public camera_and_input ci;

	public hp_handler hp;
	public mana_handler mp;

	// Use this for initialization
	void Start () {
		com.set_player(this);

		if (game_object != null) {
			ci = game_object.AddComponent<camera_and_input> ();
	
			ci.init ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
