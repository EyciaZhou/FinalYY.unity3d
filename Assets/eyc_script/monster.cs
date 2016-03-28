using UnityEngine;
using System.Collections;

public class monster : MonoBehaviour {
	public enum monster_status{
		follow,
		stop_to_attack,
		run_for_life,
		dead,
	}

	public monster_status status;
	private float v, hurt_cd;
	private hp_handler hp;
	private int exp, coin_min, coin_max, hurt;
	private monster_look_i ml;
	private rigv3 rig;
	private hourglass_manager.dot dot;

	static public monster new_monster(monster_look_i mo_typ, float v, float hurt_cd, int coin_min, 
		int coin_max, int hp, int hurt, int exp, Vector3 pos) {
		GameObject mo = Instantiate (mo_typ.go);
		mo.tag = "em";

		monster m = mo.AddComponent<monster> ();
		m.hp = mo.AddComponent<hp_handler> ();
		m.rig = mo.AddComponent<rigv3> ();
		m.v = v;
		m.hurt_cd = hurt_cd;
		m.coin_min = coin_min;
		m.coin_max = coin_max;
		m.hp.init ();
		m.hp.max_point = hp;
		m.hurt = hurt;
		m.exp = exp;

		m.ml = mo_typ;

		mo.transform.position = pos;

		return m;
	}

	private void destory() {
		Destroy (this);
	}

	private void dead() {
		status = monster_status.dead;
		coin_manager.drop (Random.Range (coin_min, coin_max), gameObject.transform.position);
		rig.enabled = false;
		gameObject.GetComponent<CapsuleCollider> ().enabled = false;
		gameObject.GetComponent<Animation> ().CrossFade (ml.dead_look);
		com.p.exp.gain_exp (exp);
		com.ts.cancle_dot (dot);
		Invoke ("destory", 10);
	}

	// Use this for initialization
	void Start () {
		status = monster_status.follow;
		dot = com.ts.add_dot (float.PositiveInfinity, hurt_cd,
			() => {
				com.p.hp.hurt(hurt);
				gameObject.GetComponent<Animation>().CrossFade(ml.attack_look);
			}, () => {
		});
		dot.pause ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (status) {
		case monster_status.follow:
			gameObject.GetComponent<Animation> ().CrossFade (ml.walk_look);
			rig.e_lookat (com.p.transform.position);
			rig.e_move_local (Vector3.forward * Time.deltaTime * v);

			if ((transform.position - com.p.transform.position).sqrMagnitude < 2) {
				status = monster_status.stop_to_attack;
				dot.not_pause ();
			}
			break;

		case monster_status.run_for_life:
			
			break;

		case monster_status.stop_to_attack:
			gameObject.GetComponent<Animation> ().CrossFade (ml.attack_look);
			if ((transform.position - com.p.transform.position).sqrMagnitude >= 2) {
				status = monster_status.follow;
				dot.pause ();
			}
			break;

		case monster_status.dead:
			break;
		}
	}
}
