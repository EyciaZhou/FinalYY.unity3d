using UnityEngine;
using System.Collections;

public class flash_thunder : MonoBehaviour {
	public Light lig;

	IEnumerator flash() {
		while (true) {
			float large = f.RandomFloat (0.6f, 1.5f);
			float round_start = Time.time;

			while (Time.time - round_start <= 0.1) {
				float delta = Time.time - round_start;

				lig.intensity = large * 36 * delta;
				yield return null;
			}
			lig.intensity = 0;

			float wait_duration = f.RandomFloat (0.1f, 10.0f);

			yield return new WaitForSeconds (wait_duration);
		}
	}

	void Start() {
		StartCoroutine (flash ());
		StartCoroutine (flash ());
	}
}
