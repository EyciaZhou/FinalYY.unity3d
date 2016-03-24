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
	private float alive_time_after_losse { get; set; }
	private float speed { get; set; }
	public fire_status status { get; private set; }
	public System.Guid uuid { get; private set; }
	public GameObject target { get; set; }
	public Vector3 size {
		get { return this.size; }
		set {
			if (go != null) {
				go.transform.localScale = value;
			}
		}
	}

	public static fireball new_fireball(GameObject go_to_instantiate, Vector3 default_scale, 
		float alive_time_after_loose, float speed, Vector3 default_postion) {

		GameObject go = (GameObject) Instantiate(go_to_instantiate, default_postion, new Quaternion());
		fireball fb = go.AddComponent<fireball> ();

		fb.uuid = System.Guid.NewGuid();
		fb.alive_time_after_losse = alive_time_after_loose;
		fb.size = default_scale;
		fb.status = fire_status.stay;
		fb.go = go;
		fb.speed = speed;

		return fb;
	}

	public static fireball new_fireball(GameObject go_to_instantiate, Vector3 default_scale, 
		float alive_time_after_loose, GameObject target, Vector3 default_postion, float speed) {

		fireball fb = new_fireball (go_to_instantiate, default_scale, alive_time_after_loose, speed, default_postion);
		fb.losse(target);

		return fb;
	}

	public static fireball new_fireball(GameObject go_to_instantiate, Vector3 default_scale, 
		float alive_time_after_loose, GameObject target, Vector3 default_postion, float speed,
		float time_to_loose) {

		fireball fb = new_fireball (go_to_instantiate, default_scale, alive_time_after_loose, speed, default_postion);
		com.ts.add_hourglass(time_to_loose, () => fb.losse(target));

		return fb;
	}

	public void losse(GameObject target) {
		this.status = fire_status.losse;
		this.target = target;
		go.transform.parent = null;

		com.ts.add_hourglass (alive_time_after_losse, () => {
			Destroy(go);
			status = fire_status.invaild;
		});
	}

	void Update() {
		switch (status) {
		case fire_status.losse:
			if (go != null) {
				if (target != null) {
					go.transform.LookAt (target.transform.position);
				}
				go.transform.Translate (Vector3.forward * speed * Time.deltaTime);
			}
			break;
		case fire_status.invaild:
			break;
		case fire_status.boom:
			break;
		case fire_status.stay:
			break;
		}
	}
}