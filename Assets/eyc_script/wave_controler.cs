using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wave_controler : MonoBehaviour
{
	public List<monster> monsters = new List<monster> ();
	public GameObject nearest_monster;

	public int wave = 0;

	public void init ()
	{
		wave = 0;
	}
	//void init (float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, int hp_rec = 0)
	monster GenMonster (monster_look_i zm1, float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, Vector3 pos)
	{
		return monster.new_monster (zm1, 5, 1, 10, 40, 100, 20, 30, Vector3.zero);
	}

	void Update ()
	{
		if (wave % 2 == 1 && monsters.Count == 0) {
			wave++;
		} else {
			float max = 1e10f;
			foreach (monster go in monsters) {
				if ((go.transform.position - com.tr.position).sqrMagnitude < max) {
					max = (go.transform.position - com.tr.position).sqrMagnitude;
					nearest_monster = go.gameObject;
				}
			}
			monsters.RemoveAll (item => item.GetComponent<monster> ().status == monster.monster_status.dead);
		}
		if (wave / 2 == 0 && wave % 2 == 0) {
			for (int i = 0; i < 100; i++) {
				monsters.Add (GenMonster ("k1", 10, 1, 10, 20, 100, 10, 30, Vector3.zero + Vector3.forward * i * 1f));
			}
			wave++;
		}
	}
}
