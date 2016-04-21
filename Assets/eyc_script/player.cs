
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object { get; private set; }
	public camera_and_input ci { get; private set; }
	public hp_handler hp { get; private set; }
	public mana_handler mp { get; private set; }
	public exp_handler_player exp { get; private set; }
	public fire_ring fr { get; private set; }
	public AttributesManager am { get; private set; }
	public skill sk { get; private set; }

	public fire_ring_view fr_view { get; private set; }

	// Use this for initialization
	void Start () {
		com.set_player(this);
		if (game_object == null) {
			game_object = gameObject;
		}
			
		am = game_object.AddComponent<AttributesManager> ();
		ci = game_object.AddComponent<camera_and_input> ();
		hp = game_object.AddComponent<hp_handler> ();
		mp = game_object.AddComponent<mana_handler> ();
		exp = game_object.AddComponent<exp_handler_player> ();
		fr = game_object.AddComponent<fire_ring> ();
		sk = game_object.AddComponent<skill> ();
		fr_view = game_object.AddComponent<fire_ring_view> ();

		am.OnAttributesChange += hp.update_controller;
		am.OnAttributesChange += mp.update_controller;
		hp.am = mp.am = am;

		hp.add_dead_callback (mp.dead);

		am.AddBuff (exp);

		ci.init ();
		hp.init (0x3fffffff);
		mp.init ();
		exp.init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
