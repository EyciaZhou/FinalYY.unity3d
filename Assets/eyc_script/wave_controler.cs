using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wave_controler : MonoBehaviour
{
	public List<GameObject> monsters = new List<GameObject> ();
	public GameObject nearest_monster;
	public GameObject k1;
	public GameObject k2;
	public GameObject k3;
	public int wave = 0;

	public void init ()
	{
		wave = 0;
	}
	//void init (float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, int hp_rec = 0)
	GameObject GenMonster (string typ, float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, Vector3 pos)
	{
		switch (typ) {
		case "k1":
			GameObject go = (GameObject)Instantiate (k1, pos, transform.localRotation);
			//go.AddComponent<monster> ();
			//go.GetComponent<monster> ().init (v, hurt_cd, coin_min, coin_max, hp, hurt, exp, 0);
			return go;
		}
		return k1;
	}

	void Update ()
	{
		if (wave % 2 == 1 && monsters.Count == 0) {
			wave++;
		} else {
			float max = 1e10f;
			foreach (GameObject go in monsters) {
				if ((go.transform.position - com.tr.position).sqrMagnitude < max) {
					max = (go.transform.position - com.tr.position).sqrMagnitude;
					nearest_monster = go;
				}
			}
			//monsters.RemoveAll (item => item.GetComponent<monster> ().status == monster.monster_status.dead);
		}
		if (wave / 2 == 0 && wave % 2 == 0) {
			for (int i = 0; i < 100; i++) {
				print (i);
				monsters.Add (GenMonster ("k1", 10, 1, 10, 20, 100, 10, 30, Vector3.zero + Vector3.forward * i * 1f));
			}
			wave++;
		}
	}
}
