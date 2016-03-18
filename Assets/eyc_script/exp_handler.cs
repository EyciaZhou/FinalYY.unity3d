using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class exp_handler : MonoBehaviour
{
	public delegate void TLvUpCallback ();

	public delegate long TExpCalu (long lv);

	public int limit;
	public int current;
	public int lv;
	
	Dictionary<string, TLvUpCallback> callback = new Dictionary<string, TLvUpCallback> ();
	//TExpCalu ExpCalu;
	/*
	float doub_timer = 0;
	float doub_rate = 1.0f;
	bool doub;
*/
	public void clear ()
	{
		//doub = false;
		lv = 1;
		current = 0;
		//limit = ExpCalu (lv);
	}

	public float getRate ()
	{
		return (float)current / limit;
	}

	void Update ()
	{/*
		if (doub) {
			doub_timer -= Time.deltaTime;
			if (doub_timer < 0) {
				doub = false;
			}
		}*/
		//print("limit : " + limit + " current : " + current + " lv : " + lv);
	}
	/*
	public void set_double_exp (float rate, int time)
	{
		doub = true;
		doub_rate = rate;
		doub_timer = time;
	}
	*/
	/*
	public void set_Lv (long lv)
	{
		this.lv = lv;
		com.calu_buff_c ();
		//limit = ExpCalu (this.lv);
	}*/

	public void init ()
	{
		clear ();
		com.buffs.add_buff (new t.buff ("lv", lv_buff));
	}

	void lv_buff (t.buff_set b)
	{
		b.lv = lv;
	}

	void _lvup ()
	{
		lv++;/*
		com.b.lv = lv;
		com.calu_buff_c ();*/
		//limit = ExpCalu (lv);
		com.cal_buff_and_calu ();
		//com.calu_buff_c ();
		foreach (KeyValuePair<string, TLvUpCallback> cb in callback) {
			cb.Value ();
		}
	}

	public void LvUp (long Lv)
	{
		for (int i = 0; i < lv; i++) {
			_lvup ();
		}
	}

	public void gainExp (int exp)
	{
		current += exp;
		while (current >= limit) {
			current -= limit;
			_lvup ();
			//print (exp);
		}
	}

	public string add_LvUpCallback (TLvUpCallback cb)
	{
		string id = System.Guid.NewGuid ().ToString ();
		callback [id] = cb;
		return id;
	}

	public void remove_LvUpCallback (string id)
	{
		callback.Remove (id);
	}
	/*
	public void set_ExpCalu (TExpCalu ec)
	{
		ExpCalu = ec;
	}*/
}
