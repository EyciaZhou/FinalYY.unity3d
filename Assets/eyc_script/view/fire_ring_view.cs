using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_ring_view : MonoBehaviour {
	const float ANG_V_DEG = 100;
	const float R = 1.2f;

	player p;
	private GameObject ring;

	private fire_ring module;

	List<Vector3> fireball_in_ring_local_should = new List<Vector3> ();

	public void add_into_ring(GameObject go) {
		go.transform.SetParent (ring.transform);
		go.transform.localPosition = Vector3.zero;
	}

	public void update_postion() {
		float deg_each = 360f / module.fireball_in_ring.Count;

		fireball_in_ring_local_should = new List<Vector3> (module.fireball_in_ring.Count);

		int k = 0;
		for (LinkedListNode<fireball> fb = module.fireball_in_ring.First; fb != null; fb = fb.Next) {
			fireball_in_ring_local_should.Add(new Vector3 (f.sin_deg (deg_each * k) * R, f.cos_deg (deg_each * k) * R));
			k++;
		}
	}

	// Use this for initialization
	void Start () {
		p = com.p;
		module = com.p.fr;
		p.am.add_attr_calc ((mid, attr)=>{
			attr.duration_hatch_fireball_in_ring = 10;
		},"duration_hatch_fireball_in_ring");

		ring = new GameObject ("ring");

		ring.transform.localPosition = Vector3.back * 0.5f + Vector3.up * 1.4f;
		ring.transform.localEulerAngles = Vector3.zero;
	}

	// Update is called once per frame
	void Update () {
		int k = 0;
		for (LinkedListNode<fireball> fb = module.fireball_in_ring.First; fb != null; fb = fb.Next) {
			f.slow_follow_localpostion (fb.Value.transform, fireball_in_ring_local_should [k], 0.1f);
			k++;	
		}//TODO: stop when finish

		f.slow_follow (ring.transform, p.transform.TransformPoint (0, 1.4f, -0.8f), p.transform.rotation.eulerAngles + Vector3.forward * Time.time * ANG_V_DEG, 5f);

	}
}
