
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object;
	public camera_and_input ci;

	public hp_handler hp;
	public mana_handler mp;
	public exp_handler exp;

	public fire_ring fr;

	public attributes_manager attr;

	// Use this for initialization
	void Start () {
		com.set_player(this);

		if (game_object != null) {
			ci = game_object.AddComponent<camera_and_input> ();
			hp = game_object.AddComponent<hp_handler> ();
			mp = game_object.AddComponent<mana_handler> ();
			exp = game_object.AddComponent<exp_handler> ();
			attr = game_object.AddComponent<attributes_manager> ();

			attr.bind_controller (hp);
			attr.bind_controller (mp);
			attr.bind_controller (exp);

			hp.add_dead_callback (mp.dead);

			attr.add_buff (exp);

			ci.init ();
			hp.init ();
			mp.init ();
			exp.init ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
