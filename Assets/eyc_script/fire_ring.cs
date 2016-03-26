using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_ring : MonoBehaviour {
	public enum ring_status {
		full,
		hatch
	}

	player p;

	List<fireball> fireball_in_ring = new List<fireball>();

	fireball hatch;
	float hatched_time;

	// Use this for initialization
	void Start () {
		p = com.p;

	}
	
	// Update is called once per frame
	void Update () {
		if (fireball_in_ring.Count < p.exp.lv) {
			
		}
	}
}
