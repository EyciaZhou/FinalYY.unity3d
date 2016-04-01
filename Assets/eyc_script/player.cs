
using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {
	public GameObject game_object { get; private set; }
	public camera_and_input ci { get; private set; }
	public hp_handler hp { get; private set; }
	public mana_handler mp { get; private set; }
	public exp_handler_player exp { get; private set; }
	public fire_ring fr { get; private set; }
	public attributes_manager am { get; private set; }
	public skill sk { get; private set; }

	public fire_ring_view fr_view { get; private set; }

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
		exp = game_object.AddComponent<exp_handler_player> ();
		fr = game_object.AddComponent<fire_ring> ();
		sk = game_object.AddComponent<skill> ();
		fr_view = game_object.AddComponent<fire_ring_view> ();

		am.bind_controller (hp);
		am.bind_controller (mp);

		hp.add_dead_callback (mp.dead);

		am.add_buff (exp);

		ci.init ();
		hp.init ();
		mp.init ();
		exp.init ();

		am.add_attr_calc ((mid, attr) => { //TODO: 
			attr.max_hp = mid.strength * 20;
			attr.recovery_per_second = mid.strength * 0.1f;

			attr.max_mp = mid.intelligence * 20;
			attr.recovery_mana_per_second = mid.intelligence * 1f;

			attr.speed = (mid.speed_base + mid.speed_addition) * mid.speed_mutiply;

			attr.exp_extra = mid.exp_extra;
			attr.exp_mutiply = mid.exp_mutiply;
			attr.coin_raidus = mid.coin_raidus;
		}, "hp&mp&speed");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
