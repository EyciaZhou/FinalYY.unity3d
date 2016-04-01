using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_manager : MonoBehaviour {
	private static GameObject go_fireball_normal_hit;
	public static GameObject go_fireball_boom;
	public static GameObject go_p_hot_rock;
	public static GameObject go_p_fire_hit_01;

	public Dictionary<System.Guid, fireball> fireballs { get; private set; }

	public fire_manager() {
		this.fireballs = new Dictionary<System.Guid, fireball> ();
	}

	void Start() {
		go_fireball_normal_hit = Resources.Load<GameObject> ("fireball/normal_hit");
		go_fireball_boom = Resources.Load<GameObject> ("fireball/boom_green");
		go_p_hot_rock = Resources.Load<GameObject> ("fireball/p_hot_rock");
		go_p_fire_hit_01 = Resources.Load<GameObject> ("fireball/p_fire_hit_01");
	}

	public fireball new_fireball_default(Transform parent, Quaternion world_rotation, float hurt) {
		fireball fb = fireball.new_fireball(
			go_p_hot_rock,
			1,
			10,
			10,
			parent.position,
			world_rotation,
			hurt,
			go_p_fire_hit_01,
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
