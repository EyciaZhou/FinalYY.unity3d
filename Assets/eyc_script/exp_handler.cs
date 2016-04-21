using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class exp_handler : MonoBehaviour
{
	public delegate void LvUpAction();
	public event LvUpAction OnLvUp;

	private int LvUpWaitGroup;

	public float current { get; private set; }
	public long limit { get; private set; }
	public int lv { get; private set; }

	private int level_pool;

	public void LockLvUp() {
		lock (this) {
			LvUpWaitGroup++;
		}
	}

	public void UnLockLvUp() {
		lock (this) {
			if (LvUpWaitGroup > 0) {
				LvUpWaitGroup--;
			} else {
				Debug.Log ("Error, Unlock before lock");
			}
		}
	}

	void Update()
	{
		if (LvUpWaitGroup == 0 && level_pool > 0) {
			something_process_when_lvup ();
		}
	}

	private long exp_calu(int lv) {
		return 10 * lv * lv; //TODO:
	}

	public void init ()
	{
		lv = 1;
		current = 0;
		limit = exp_calu (lv);
	}

	private void something_process_when_lvup ()
	{
		lv++;
		limit = exp_calu (lv);
		level_pool--;
		OnLvUp ();
	}

	private void lvUpToPool() {
		level_pool++;
	}

	public void LvUp (int lvs_to_up)
	{
		for (int i = 0; i < lvs_to_up; i++) {
			lvUpToPool ();
		}
	}

	public void GainExp (int exp)
	{
		current += exp;
		while (current >= limit) {
			current -= limit;
			lvUpToPool ();
		}
	}
}
