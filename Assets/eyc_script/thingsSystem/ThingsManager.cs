using System;
using UnityEngine;
using System.Collections.Generic;

public class ThingsManager : MonoBehaviour {
	public IRing LRing, RRing;

	public Dictionary<System.Guid, IThing> thingsInBag = new Dictionary<System.Guid, IThing> ();

	public ThingsUI ui;

	void Start() {
		RingUtils.load ();

		ui = gameObject.AddComponent<ThingsUI> ();

		for (int i = 0; i < 5; i++) {
			GetThing (RingUtils.RandomOneRing ());
		}
	}

	public void GetThing(IThing thing) {
		addThingToBag (thing);
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

