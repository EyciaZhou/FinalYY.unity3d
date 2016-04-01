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
	private float hurt_cd;
	private hp_handler hp;
	private int exp, coin_min, coin_max, hurt;
	private monster_look ml;
	private hourglass_manager.dot dot;

	private NavMeshAgent nav_mesh_agent;

	public System.Guid guid { get; private set; }

	private Rigidbody rig;

	static public monster new_monster(monster_look mo_typ, float v, float hurt_cd, int coin_min, 
		int coin_max, int hp, int hurt, int exp, Vector3 pos) {
		GameObject mo = (GameObject)Instantiate (mo_typ.go);
		mo.tag = "em";

		monster m = mo.AddComponent<monster> ();
		m.hp = mo.AddComponent<hp_handler> ();
		//m.v = v;
		m.hurt_cd = hurt_cd;
		m.coin_min = coin_min;
		m.coin_max = coin_max;
		m.hp.init ();
		m.hp.add_dead_callback (m.dead);
		m.hp.max_point = hp;
		m.hurt = hurt;
		m.exp = exp;

		m.ml = mo_typ;
		m.guid = System.Guid.NewGuid ();

		mo.transform.position = pos;

		if (m.GetComponent<NavMeshAgent> () == null) {
			m.nav_mesh_agent = m.gameObject.AddComponent<NavMeshAgent> ();
		} else {
			m.nav_mesh_agent = m.GetComponent<NavMeshAgent> ();
		}

		//m.rig.isKinematic = true;

		m.nav_mesh_agent.speed = v;

		return m;
	}

	private void destory() {
		Destroy (gameObject);
	}

	private void dead() {
		status = monster_status.dead;
		coin_manager.drop (Random.Range (coin_min, coin_max), gameObject.transform.position);
		gameObject.GetComponent<CapsuleCollider> ().enabled = false;
		gameObject.GetComponent<Animation> ().CrossFade (ml.dead_look);
		nav_mesh_agent.enabled = false;
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
			gameObject.GetComponent<NavMeshAgent>().SetDestination(com.p.transform.position);
			if ((transform.position - com.p.transform.position).sqrMagnitude < 2) {
				status = monster_status.stop_to_attack;
				dot.not_pause_and_start_immedia ();
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

	IEnumerator back(Vector3 direction) {
		float f = 10;
		float mcl = 5;
		while (f > 0) {
			f -= mcl * Time.deltaTime;
			transform.position += direction * f * Time.deltaTime;
			yield return null;
		}
	}

	public void hit_back(Vector3 direction) {
		StartCoroutine (back (direction));
	}
}
