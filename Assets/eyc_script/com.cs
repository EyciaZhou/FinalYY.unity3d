using UnityEngine;
using System.Collections;

public class com : MonoBehaviour
{
	public static player p;

	public	static	wave_controler	monster;
	public	static	thing_control things;
	public	static	buff_manager buffs;
	public	static	exp_handler exp;
	public	static	camera_and_input ci;
	public	static	Transform tr;

	public	static	float	c_Jmped;
	public	static	float	c_Spd = 10.0f;

	public	static	t.buff_set b;
	
	static	float	Spd = 30.0f;
	static	float	SpdUpTime = 0.0f;

	public static void set_player(player pl) {
		com.p = pl;
	}

	public static void cal_buff_and_calu ()
	{
		buffs.cal_buff ();
		calu_buff_c ();
	}

	public static void calu_buff_c ()
	{
		//TODO
		c_Spd = 20 + b.footstep_spd * 3;

		int exp_limit = b.lv * b.lv * 10;
		exp.limit = exp_limit;
	}

	public static void reborn ()
	{
		p.hp.reborn ();
		p.mp.reborn ();
	}

	void Start ()
	{
		/*monster = GetComponent<wave_controler> ();
		things = GetComponent<thing_control> ();
		buffs = GetComponent<buff_manager> ();
		hp = GetComponent<hp_handler> ();
		mp = GetComponent<mana_handler> ();
		exp = GetComponent<exp_handler> ();
		go = GetComponent<player_controler> ();
		rig = GetComponent<rigv3> ();
		tr = GetComponent<Transform> ();*/

		/*
		monster.init ();
		things.init ();
		buffs.init ();
		hp.init ();
		mp.init ();
		exp.init ();
		go.init ();
		rig.init ();
		*/
	}

	public static void API_GainExp (int ex)
	{
		exp.gainExp (ex);
	}

	public static void API_Drop_Coin (Vector3 pos, int coin)
	{

	}

	public static void API_Hurt (int hurt)
	{
		//cal arm
		p.hp.hurt (hurt);
	}

	void Update ()
	{

	}
}