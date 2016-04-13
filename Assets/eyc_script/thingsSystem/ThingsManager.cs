using System;
using UnityEngine;
using System.Collections.Generic;

public class ThingsManager : MonoBehaviour {
	public IRing LRing, RRing;

	public Dictionary<System.Guid, IThing> thingsInBag = new Dictionary<System.Guid, IThing> ();

	public ThingsUI ui;

	void Start() {
		RingUtils.load ();
		pointsUtils.load ();

		ui = gameObject.AddComponent<ThingsUI> ();

		for (int i = 0; i < 5; i++) {
			GetThing (RingUtils.RandomOneRing ());
		}

		for (int i = 0; i < 5; i++) {
			GetThing (pointsUtils.RandomOneRing ());
		}
	}

	public void GetThing(IThing thing) {
		addThingToBag (thing);
	}

	public void UsePoint(ThingPoint point) {
		if (point.typ == pointsUtils.TypeOfPoint.HP) {
			com.p.hp.recovery(point.point);
		} else if (point.typ == pointsUtils.TypeOfPoint.MP) {
			com.p.mp.recovery(point.point);
		} else if (point.typ == pointsUtils.TypeOfPoint.EXP) {
			com.p.exp.gain_exp(point.point);
		}
		if (thingsInBag.ContainsKey(point.Guid)) {
			removeThingInBag (point);
		}
	}

	public bool UseRing(IRing ring) {
		if (!thingsInBag.ContainsKey (ring.Guid)) {
			return false;
		}

		if (LRing != null && RRing != null) {
			return false;
		}

		if (LRing == null) {
			LRing = ring;
		} else {
			RRing = ring;
		}

		removeThingInBag (ring);
		com.p.am.add_buff (ring.Buff);
		return true;
	}

	public bool UnuseRing(IRing ring) {
		if (LRing != ring && RRing != ring) {
			return false;
		}

		if (LRing == ring) {
			LRing = null;
		} else {
			RRing = null;
		}

		addThingToBag (ring);

		com.p.am.remove_buff (ring.Buff);
		return true;
	}

	private void removeThingInBag(IThing thing) {
		thingsInBag.Remove (thing.Guid);
		ui.ReGenerateUI ();
	}

	private void addThingToBag(IThing thing) {
		thingsInBag.Add (thing.Guid, thing);
		ui.ReGenerateUI ();
	}
}

