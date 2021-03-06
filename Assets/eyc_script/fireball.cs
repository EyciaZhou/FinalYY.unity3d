﻿using UnityEngine;
using System.Collections;

public class fireball : MonoBehaviour {
	public enum fire_status {
		stay,
		losse,
		boom,
		invaild,
	}

	public GameObject go { get; private set; }
	private GameObject boom { get; set; }
	private float alive_time_after_losse { get; set; }
	private float speed { get; set; }
	public fire_status status;
	public System.Guid uuid { get; private set; }
	public GameObject target { get; set; }
	public float size { get; private set; }
	public Quaternion rotation_after_losse { get; private set; }
	public float Hurt { get; private set; }


	private float _scale;
	public float scale {
		get { return _scale; }
		set {
			_scale = value;
			if (go != null) {
				go.GetComponent<ParticleSystem> ().startSize = size * value;
			}
		}
	}

	public static fireball new_fireball(GameObject go_to_instantiate, float default_size, 
		float alive_time_after_losse, float speed, Vector3 default_postion, Quaternion rotation_after_losse, 
		float hurt, GameObject boom) {

		GameObject go = (GameObject) Instantiate(go_to_instantiate, default_postion, new Quaternion());
		fireball fb = go.AddComponent<fireball> ();

		fb.rotation_after_losse = rotation_after_losse;
		fb.uuid = System.Guid.NewGuid();
		fb.alive_time_after_losse = alive_time_after_losse;
		fb.size = default_size;
		fb.status = fire_status.stay;
		fb.go = go;
		fb.speed = speed;
		fb.Hurt = hurt;
		fb.boom = boom;
		fb.go.GetComponent<ParticleSystem> ().simulationSpace = ParticleSystemSimulationSpace.Local;
		go.GetComponent<CapsuleCollider> ().enabled = false;

		return fb;
	}

	public static fireball new_fireball(GameObject go_to_instantiate, float default_size, 
		float alive_time_after_loose, float speed, Vector3 default_postion, Quaternion rotation_after_losse, float hurt, GameObject boom, GameObject target) {

		fireball fb = new_fireball (go_to_instantiate, default_size, alive_time_after_loose, speed, default_postion, rotation_after_losse, hurt, boom);
		fb.target = target;
		fb.losse();

		return fb;
	}

	public static fireball new_fireball(GameObject go_to_instantiate, float default_size, 
		float alive_time_after_loose, float speed, Vector3 default_postion, Quaternion rotation_after_losse, float hurt, GameObject boom,
		GameObject target, float time_to_loose) {

		fireball fb = new_fireball (go_to_instantiate, default_size, alive_time_after_loose, speed, default_postion, rotation_after_losse, hurt, boom);
		fb.target = target;
		com.ts.add_hourglass(time_to_loose, () => {
			fb.losse();
		});

		return fb;
	}

	public void losse() {
		this.status = fire_status.losse;
		go.transform.parent = null;
		go.transform.rotation = rotation_after_losse;
		go.GetComponent<ParticleSystem> ().simulationSpace = ParticleSystemSimulationSpace.World;
		go.GetComponent<CapsuleCollider> ().enabled = true;

		com.ts.add_hourglass (alive_time_after_losse, () => {
			if (status != fire_status.invaild) {
				destory();
			}
		});
	}

	public void destory() {
		status = fire_status.invaild;
		Instantiate (boom, transform.position, transform.rotation);
		Destroy (gameObject);
	}

	void Update() {
		switch (status) {
		case fire_status.stay:
			break;

		case fire_status.losse:
			if (go != null) {
				if (target != null) {
					Vector3 dist = target.transform.position - transform.position;

					go.transform.rotation = Quaternion.Slerp (
						go.transform.rotation, 
						Quaternion.LookRotation (dist),
						Time.deltaTime * Mathf.Max (4.5f, 30 / dist.magnitude));
				}
				go.transform.Translate (Vector3.forward * speed * Time.deltaTime);
			}
			break;

		case fire_status.boom:
			break;
		
		case fire_status.invaild:
			break;
		}
	}

	void OnTriggerEnter (Collider renwu) {
		if (renwu.gameObject.tag == "em" && status == fire_status.losse) {
			hp_handler hp = renwu.GetComponent<hp_handler> ();
			if (hp != null) {
				hp.hurt ((int)(Hurt));
			}
			destory ();
		}
	}
}
