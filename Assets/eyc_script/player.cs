
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object;
	public camera_and_input ci;

	public hp_handler hp;
	public mana_handler mp;

	public attributes attr;

	// Use this for initialization
	void Start () {
		com.set_player(this);

		if (game_object != null) {
			ci = game_object.AddComponent<camera_and_input> ();
			hp = game_object.AddComponent<hp_handler> ();
			mp = game_object.AddComponent<mana_handler> ();
			attr = game_object.AddComponent<attributes> ();

			attr.bind_controller (hp);
			attr.bind_controller (mp);
			hp.add_dead_callback (mp.dead);

			ci.init ();
			hp.init ();
			mp.init ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
