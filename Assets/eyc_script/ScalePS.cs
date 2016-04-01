using UnityEngine;
using System.Collections;

public class ScalePS : MonoBehaviour {
	public float scale = 1;

	public static void ScaleParticleSystem(GameObject gameObj, float scale)
	{
		var hasParticleObj = false;
		var particles=gameObj.GetComponentsInChildren<ParticleSystem>(true);
		foreach (ParticleSystem p in particles)
		{
			hasParticleObj = true;
			p.startSize *= scale;
			p.startSpeed *= scale;
			p.startRotation *= scale;
			p.transform.localScale *= scale;
		}
		if (hasParticleObj)
		{
			gameObj.transform.localScale = new Vector3(scale, scale, 1);
		}
	}

	// Use this for initialization
	void Start () {
		ScaleParticleSystem (gameObject, scale);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
