using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wave_controler : MonoBehaviour
{
	public List<monster> monsters = new List<monster> ();
	public GameObject nearest_monster;

	private List<monster_look> monster_looks = new List<monster_look> ();

	private string [,]configs = new string[5, 5] {
		{"zm/Zombie_1", "Dead_1_Falling_front", "Walk_1", "hit", "idle"},
		{"zm/Zombie_2", "Dead_1_Falling_front", "Walk_2", "hit", "idle_2"},
		{"zm/Zombie_3", "Dead_2_Falling_Back", "Walk_3", "Attack_2", "idle_3"},
		{"zm/Zombie_4", "Dead_2_Falling_Back", "Walk_4", "Attack_2", "idle_4"},
		{"zm/Zombie_5", "Dead_2_Falling_Back", "Walk_5", "Attack_3", "idle_5"}};
	
	public int wave = 0;

	void Start() {
		wave = 0;
		for (int i = 0; i < configs.Length; i++) {
			monster_looks.Add (new monster_look ((GameObject)Resources.Load (configs [i, 0]), configs [i, 1], configs [i, 2], configs [i, 3], configs [i,4]));
		}
	}

	//void init (float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, int hp_rec = 0)
	monster GenMonster (monster_look zm1, float v, float hurt_cd, int coin_min, int coin_max, int hp, int hurt, int exp, Vector3 pos)
	{
		return monster.new_monster (zm1, v, hurt_cd, coin_min, coin_max, hp, hurt, exp, pos);
	}

	void Update ()
	{
		if (wave % 2 == 1 && monsters.Count == 0) {
			wave++;
		} else {
			float max = 1e10f;
			foreach (monster go in monsters) {
				if ((go.transform.position - com.p.transform.position).sqrMagnitude < max) {
					max = (go.transform.position - com.p.transform.position).sqrMagnitude;
					nearest_monster = go.gameObject;
				}
			}
			monsters.RemoveAll (item => item.GetComponent<monster> ().status == monster.monster_status.dead);
		}
		if (wave / 2 == 0 && wave % 2 == 0) {
			for (int i = 0; i < 100; i++) {
				monsters.Add (GenMonster (monster_looks[0], 0.5f, 1, 10, 40, 30, 20, 30, Vector3.right + Vector3.forward * i * 10f));
			}
			wave++;
		}
	}
}
