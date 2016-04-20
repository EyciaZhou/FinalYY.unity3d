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
		lock (LvUpWaitGroup) {
			LvUpWaitGroup++;
		}
	}

	public void UnLockLvUp() {
		lock (LvUpWaitGroup) {
			if (LvUpWaitGroup > 0) {
				LvUpWaitGroup--;
			} else {
				Debug.Log ("Error, Unlock before lock");
			}
		}
	}

	void Update()
	{
		if (!LvUpWaitGroup && level_pool > 0) {
			_lvup ();
		}
	}

	public void clear ()
	{
		lv = 1;
		current = 0;
		limit = exp_calu (lv);
	}

	public exp_handler() {
		clear ();
	}

	private long exp_calu(int lv) {
		return 10 * lv * lv; //TODO:
	}

	public void init ()
	{
		clear ();
	}

	void _lvup ()
	{
		lv++;
		limit = exp_calu (lv);
		level_pool--;
		OnLvUp ();
	}

	void lvUpPool() {
		level_pool++;
	}

	public void lv_up (long lvs_to_up)
	{
		for (int i = 0; i < lvs_to_up; i++) {
			lvUpPool ();
		}
	}

	public void gain_exp (int exp)
	{
		current += exp;
		while (current >= limit) {
			current -= limit;
			lvUpPool ();
		}
	}
}
