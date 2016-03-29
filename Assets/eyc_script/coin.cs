using UnityEngine;
using System.Collections;

public class coin : MonoBehaviour {
	public int coin_num;

	public void init(int coin_num) {
		this.coin_num = coin_num;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "coin_collector") {
			com.coins.gain (coin_num);
			Destroy (gameObject);
		}
	}
}
