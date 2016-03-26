
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object { get; private set; }
	public camera_and_input ci { get; private set; }
	public hp_handler hp { get; private set; }
	public mana_handler mp { get; private set; }
	public exp_handler exp { get; private set; }
	public fire_ring fr { get; private set; }
	public attributes_manager am { get; private set; }

	// Use this for initialization
	void Start () {
		com.set_player(this);
		if (game_object == null) {
			game_object = gameObject;
		}
			
		am = game_object.AddComponent<attributes_manager> ();
		ci = game_object.AddComponent<camera_and_input> ();
		hp = game_object.AddComponent<hp_handler> ();
		mp = game_object.AddComponent<mana_handler> ();
		exp = game_object.AddComponent<exp_handler> ();
		fr = game_object.AddComponent<fire_ring> ();

		am.bind_controller (hp);
		am.bind_controller (mp);
		am.bind_controller (exp);

		hp.add_dead_callback (mp.dead);

		am.add_buff (exp);

		ci.init ();
		hp.init ();
		mp.init ();
		exp.init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
