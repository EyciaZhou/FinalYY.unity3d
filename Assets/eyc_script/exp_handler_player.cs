using UnityEngine;
using System.Collections;

public class exp_handler_player : exp_handler, IBuff {
	//public attributes_manager am { get; set; }
	public System.Guid Guid { get; private set; }
	public delegate void ExpValueChangeAction ();
	public event ExpValueChangeAction OnExpValueChange;
	public AttributesManager AM { get; set; }

	public void calculate (AttributesManager.MidAttributes mid)
	{
		//TODO:
		mid.Intelligence += 10 * lv;
		mid.Agility += 10 * lv;
		mid.Strength += 10 * lv;
		mid.SpeedBase += 5 + lv / 5;
		mid.SpeedMutiply += (int)((1 + 0.5f / lv / lv) * 100);
		mid.ExpMutiply += 100;
		mid.CollectRaidus += 1;
	}

	public exp_handler_player() : base() {
		this.Guid = System.Guid.NewGuid ();
		OnLvUp += (() => {
			AM.ReCalc();
			com.p.hp.recovery(0x3fffffff);
			com.p.mp.recovery(0x3fffffff);
		});
	}

	public new void gain_exp(int exp) {
		base.GainExp (com.p.am.Calc_ReallyExp(exp));
		OnExpValueChange ();
	}
}
