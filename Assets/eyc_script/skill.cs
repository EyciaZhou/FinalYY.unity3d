using UnityEngine;
using System.Collections;

public class skill : MonoBehaviour {
	public GameObject bar_go;

	private bool small_fireball_colddown = true;

	private GameObject big_fire_ring_go;

	private AttributesManager am;

	public void small_fireball_from_bar(Quaternion rotation) {
		if (small_fireball_colddown) {
			if (com.p.mp.cost (am.Attr.SmallFireball_Cost)) {
				com.p.GetComponent<Animator> ().SetTrigger ("fire");
				com.ts.add_hourglass (
					am.Attr.SmallFireball_Cooldown, 
					() => {
						small_fireball_colddown = true;
					}
				);
				com.fires.new_fireball_default (
					bar_go.transform, 
					rotation, 
					am.Attr.SmallFireball_Hurt
				);
				small_fireball_colddown = false;
			}
		}
	}

	public void big_fire_ring() {
		//judge mana

		fireball fb = com.p.fr.Remove ();

		if (fb != null) {
			fb.destory ();
			Instantiate (big_fire_ring_go, com.p.transform.position, Quaternion.Euler (Vector3.zero));
		}
	}

	public skill() {
	}

	void Start() {
		am = com.p.am;

		bar_go = GameObject.Find ("bar_local");

		big_fire_ring_go = Resources.Load<GameObject> ("big_fire_ring/big_fire_ring");
	}

	void Update() {
	}
}
