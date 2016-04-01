using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class exp_handler : MonoBehaviour
{
	public delegate void LvUpAction();
	public event LvUpAction OnLvUp;

	public float current { get; private set; }
	public long limit { get; private set; }
	public int lv { get; private set; }

	public void clear ()
	{
		lv = 10;
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
		OnLvUp ();
	}

	public void lv_up (long lvs_to_up)
	{
		for (int i = 0; i < lvs_to_up; i++) {
			_lvup ();
		}
	}

	public void gain_exp (int exp)
	{
		current += exp;
		while (current >= limit) {
			current -= limit;
			_lvup ();
		}
	}
}
