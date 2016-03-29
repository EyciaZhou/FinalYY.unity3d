using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_manager : MonoBehaviour {
	private static GameObject go_fireball_normal_hit;
	private static GameObject go_fireball_boom;

	public Dictionary<System.Guid, fireball> fireballs { get; private set; }

	public fire_manager() {
		this.fireballs = new Dictionary<System.Guid, fireball> ();
	}

	void Start() {
		go_fireball_normal_hit = Resources.Load<GameObject> ("fireball/normal_hit");
		go_fireball_boom = Resources.Load<GameObject> ("fireball/boom_green");
	}

	public fireball new_fireball_default(Transform parent, Quaternion world_rotation, float hurt) {
		fireball fb = fireball.new_fireball(
			go_fireball_normal_hit,
			1,
			10,
			10,
			parent.position,
			world_rotation,
			hurt,
			go_fireball_boom,
			null,
			0.5f);
		fb.go.transform.parent = parent;
		fireballs.Add (fb.uuid, fb);
		return fb;
	}

	public fireball new_fireball_in_ring_default() {
		fireball fb = fireball.new_fireball (go_fireball_normal_hit,
			1.4f, 99999, 20f, com.p.transform.position, Quaternion.Euler(Vector3.zero), 0, go_fireball_boom);

		fireballs.Add (fb.uuid, fb);
		return fb;
	}

	public void add_fireball(fireball fb) {
		fireballs.Add(fb.uuid, fb);
	}

	public void remove_fire_ball(fireball fb) {
		fireballs.Remove (fb.uuid);
	}
}
