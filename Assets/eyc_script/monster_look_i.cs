using UnityEngine;
using System.Collections;

public interface monster_look_i {
	string dead_look { get; }
	string walk_look { get; }
	string attack_look { get; }
	string idle_look { get; }

	GameObject go { get; }
}
