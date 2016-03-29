using UnityEngine;
using System.Collections;

public class monster_look {
	public string dead_look;
	public string walk_look;
	public string attack_look;
	public string idle_look;
	public GameObject go;

	public monster_look(GameObject go, string dead_look, string walk_look, string attack_look, string idle_look) {
		this.go = go;
		this.dead_look = dead_look;
		this.walk_look = walk_look;
		this.attack_look = attack_look;
		this.idle_look = idle_look;
	}
}
