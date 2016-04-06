using UnityEngine;
using System.Collections;

public class exp_handler_player : exp_handler, IBuff {
	//public attributes_manager am { get; set; }
	public System.Guid Guid { get; private set; }
	public delegate void ExpValueChangeAction ();
	public event ExpValueChangeAction OnExpValueChange;

	public void calculate (attributes_manager.t_mid_attributes mid)
	{
		//TODO:
		mid.intelligence += 10 * lv;
		mid.agility += 10 * lv;
		mid.strength += 10 * lv;
		mid.speed_base += 5 + lv / 2;
		mid.speed_mutiply += (int)((1 + 0.5f / lv) * 100);
		mid.exp_mutiply += 100;
		mid.coin_raidus += 1;
	}

	public exp_handler_player() : base() {
		this.Guid = System.Guid.NewGuid ();
		OnLvUp += (() => {
			com.p.am.buff_changed_from("ehib");
		});
	}

	public new void gain_exp(int exp) {
		base.gain_exp ((int)(exp*com.p.am.attr.exp_mutiply + com.p.am.attr.exp_extra));
		OnExpValueChange ();
	}
}
