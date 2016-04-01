using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class big_fire_ring : MonoBehaviour {
	SphereCollider sc;

	float hurt = 10f;

	HashSet<System.Guid> hited = new HashSet<System.Guid> ();
	HashSet<System.Guid> monsters_need_hit = new HashSet<System.Guid> ();

	// Use this for initialization
	void Start () {
		StartCoroutine (fire_big_ring ());
	}

	IEnumerator fire_big_ring() {

		float speed = 10f;

		//start:
		sc = GetComponent<SphereCollider> ();
		if (sc == null) {
			sc = gameObject.AddComponent<SphereCollider> ();
		}
		foreach (monster mo in com.mon.monsters) {
			monsters_need_hit.Add (mo.guid);
		}

		float start_time = Time.time;

		while (Time.time <= start_time + 20) {
			transform.localScale = Vector3.one * speed * (Time.time - start_time);
			yield return null;
		}

		//big_ing:
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "em") {
			if (other.gameObject != null) {
				monster mo = other.GetComponent<monster> ();
				System.Guid guid = mo.guid;
				if (monsters_need_hit.Contains(guid) && !hited.Contains (guid)) {
					hited.Add (guid);
					hp_handler hp = other.GetComponent<hp_handler> ();
					if (hp != null) {
						hp.hurt ((int)(hurt));
					}
					mo.hit_back ((other.transform.position - transform.position).normalized);
					Instantiate (fire_manager.go_fireball_boom, other.gameObject.transform.position, Quaternion.Euler (0, 0, 0));
				}
			}
		}
	}
}
