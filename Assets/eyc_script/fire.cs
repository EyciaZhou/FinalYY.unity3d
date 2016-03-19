using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour
{
	public float dispointTime = 20.0f;
	public GameObject boom;
	float realv = 10.0f;
	float timer = 0.0f;

	bool flyed = false;
	Vector3 rou;
	bool dead = false;

	public void biu ()
	{
		flyed = true;
		transform.parent = null;
		if (com.monster.monsters.Count > 0) {
			transform.LookAt (com.monster.nearest_monster.transform.position);
		}
		transform.localEulerAngles = rou;
	}

	void Start ()
	{
		rou = transform.localEulerAngles;
		dead = false;
	}

	// Update is called once per frame
	void LateUpdate ()
	{	
		if (!flyed) {
			return;
		}
		if (com.monster.monsters.Count > 0) {
			Vector3 dis = com.monster.nearest_monster.transform.position + Vector3.up * 1.3f - transform.position;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (dis, transform.up), Time.deltaTime * Mathf.Max (4.5f, 30 / dis.magnitude));
		}
		transform.localEulerAngles += new Vector3 (0, 0, Time.deltaTime * 1080);
		transform.Translate (Vector3.forward * realv * Time.deltaTime + Vector3.left * realv * 0.5f * Time.deltaTime);
		timer += Time.deltaTime;
		if (timer > dispointTime) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider renwu)
	{
		if (renwu.gameObject.tag == "jiangshi" && !dead) {
			renwu.GetComponent<hp_handler> ().hurt (10);
			Instantiate (boom, transform.position, transform.rotation);
			Destroy (gameObject);
			dead = true;
		}
	}
}