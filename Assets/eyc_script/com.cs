using UnityEngine;
using System.Collections;

public class com : MonoBehaviour
{
	public static player p;

	public	static	wave_controler	mon;
	//public	static	thing_control things;
	public	static	hourglass_manager ts;
	public	static	camera_and_input ci;
	public	static	fire_manager fires;
	public	static	coin_manager coins;


	public	static	float	c_Jmped;
	public	static	float	c_Spd = 10.0f;

	
	static	float	Spd = 30.0f;
	static	float	SpdUpTime = 0.0f;

	public static void set_player(player pl) {
		com.p = pl;
	}

	public static void reborn ()
	{
		p.hp.reborn ();
		p.mp.reborn ();
	}

	void Start ()
	{
		mon = gameObject.AddComponent<wave_controler> ();
		ts = gameObject.AddComponent<hourglass_manager> ();
		fires = gameObject.AddComponent<fire_manager> ();
		coins = gameObject.AddComponent<coin_manager> ();
	}

	public static void API_GainExp (int ex)
	{
		p.exp.gain_exp (ex);
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