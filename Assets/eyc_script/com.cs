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
	public	static	ThingsManager things;

	public static void set_player(player pl) {
		com.p = pl;
	}

	void Start ()
	{
		mon = gameObject.AddComponent<wave_controler> ();
		ts = gameObject.AddComponent<hourglass_manager> ();
		fires = gameObject.AddComponent<fire_manager> ();
		coins = gameObject.AddComponent<coin_manager> ();
		things = gameObject.AddComponent<ThingsManager> ();
	}

	void Update ()
	{

	}
}