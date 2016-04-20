using UnityEngine;
using System.Collections;

public class ri : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}

	public void DestroyMe ()
	{
		transform.position = com.p.transform.position + new Vector3 (f.RandomFloat (30f, -30f), 0, f.RandomFloat (30f, -30f));
	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
