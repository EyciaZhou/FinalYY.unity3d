using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class coldDownTimer
{
	float now_time = 0;
	float target_time = 1;
	bool is_start = false;

	public void set_target (float target)
	{
		target_time = target;
	}

	public bool set_target_and_start (float target)
	{
		//lock
		if (is_start) {
			return false;
		}
		set_target (target);
		start ();
		return true;
		//unlock
	}

	public void start ()
	{
		is_start = true;
	}

	public void clear_now ()
	{
		now_time = 0;
		is_start = false;
	}

	public float getrate ()
	{
		if (!is_start) {
			return 0;
		}
		return now_time / target_time;
	}

	public void call (float delta)
	{
		if (!is_start) {
			return;
		}
		now_time += delta;
		if (now_time > target_time) {
			now_time = 0;
			is_start = false;
		}
	}

	public coldDownTimer (float target_time)
	{
		this.target_time = target_time;
	}
}

class qianyao_bars
{
	public delegate void Callback ();

	Dictionary<string, timer> qianyaos = new Dictionary<string, timer> ();

	class timer
	{
		float target_time;
		float time;
		Callback cb;
		bool isend = false;

		public string name;

		public timer (float time, string name)
		{
			this.target_time = time;
			this.time = 0;
			this.isend = false;
		}

		public void set_cb (Callback cb)
		{
			this.cb = cb;
		}

		public float getrate ()
		{
			return time / target_time;
		}

		public void call (float delta)
		{
			time += delta;
			if (time > target_time && !isend) {
				isend = true;
				if (cb != null) {
					cb ();
				}
			}
		}
	}

	
	public string add (string name, Callback cb, float time)
	{
		string uuid = System.Guid.NewGuid ().ToString ();
		qianyaos.Add (uuid, new timer (time, name));
		qianyaos [uuid].set_cb (delegate() {
			cb ();
			qianyaos.Remove (uuid);
		});
		return uuid;
	}

	public void call (float delta)
	{
		List<string> keys = new List<string> (qianyaos.Keys);
		foreach (string uuid in keys) {
			qianyaos [uuid].call (Time.deltaTime);
		}
	}
}

public class skill_manager : MonoBehaviour
{
	public	GameObject	bigfireball;
	public	GameObject	fireball;
	
	public	Transform bar_transform;
	public	Transform hand_transform;

	const string shot_animation = "Shot_Straight";

	const string j_normal_hit = "normal_hit";

	Dictionary<string, coldDownTimer> timers = new Dictionary<string, coldDownTimer> ();
	qianyao_bars qianyao = new qianyao_bars ();
	
	// Use this for initialization
	void Start ()
	{
		GetComponent<Animation>() [shot_animation].layer = 3;
		GetComponent<Animation>() [shot_animation].wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update ()
	{
		foreach (coldDownTimer i in timers.Values) {
			i.call (Time.deltaTime);
		}
		qianyao.call (Time.deltaTime);
	}

	void small_fireball ()
	{
		
	}

	public void FireABall ()
	{
		if (!timers.ContainsKey (j_normal_hit)) {
			timers.Add (j_normal_hit, new coldDownTimer (2.0f));
		}
		if (timers [j_normal_hit].set_target_and_start (2.0f)) { //com.normal_hit.colddown
			print (com.exp.lv);
			GameObject[] gas = new GameObject[com.exp.lv];
			for (int i = 0; i < com.exp.lv; i++) {
				gas [i] = (GameObject)Instantiate (fireball, bar_transform.position, Quaternion.Euler (new Vector3 (0, transform.eulerAngles.y, i * (360.0f / (com.exp.lv)))));
				gas [i].transform.parent = bar_transform;
			}
			qianyao.add (j_normal_hit, delegate() {
				foreach (GameObject go in gas) {
					go.GetComponent<fire> ().biu ();
				}
			}, 0.5f);
			GetComponent<Animation>().Stop (shot_animation);
			GetComponent<Animation>() [shot_animation].AddMixingTransform (hand_transform, true);
			GetComponent<Animation>().CrossFade (shot_animation);
		}
		//Transform tr = transform.Find("NoFrameName00/visual_scenes_0/Scene_Root/MapRoot/NoFrameName0/Bip01/Bip01_Pelvis/Bip01_Spine/Bip01_Spine1");
	}

	public void BigFireBall ()
	{
		Instantiate (bigfireball, transform.position + Vector3.up * 2, transform.localRotation);
	}
}
