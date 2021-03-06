﻿using UnityEngine;
using System.Collections;

public class coin_manager : MonoBehaviour {
	private static GameObject big_staff;
	private static GameObject mid_staff;
	private static GameObject sml_staff;

	public int hv_coin { get; private set; }

	private SphereCollider coin_collector;

	void Start() {

		coin_collector = GameObject.Find ("coin_collector").GetComponent<SphereCollider> ();

		big_staff = Resources.Load<GameObject> ("coin/big_staff");
		mid_staff = Resources.Load<GameObject> ("coin/mid_staff");
		sml_staff = Resources.Load<GameObject> ("coin/sml_staff");

	}

	public static coin drop(int coin_num, Vector3 position) {
		GameObject to_ins = sml_staff;
		if (coin_num > 10) {
			to_ins = mid_staff;
		}
		if (coin_num > 100) {
			to_ins = big_staff;
		}

		GameObject c = (GameObject)Instantiate (to_ins, position, Quaternion.Euler(Vector3.zero));
		c.tag = "coin";
		coin c_s = c.AddComponent<coin> ();

		c_s.coin_num = coin_num;

		return c_s;
	}

	public void gain(int coin_num) {
		hv_coin += coin_num;
	}

	// Update is called once per frame
	void Update () {
		coin_collector.radius = com.p.am.Calc_CoinRaidus();
	}
}
