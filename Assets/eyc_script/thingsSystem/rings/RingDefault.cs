using System;
using UnityEngine;
using System.Collections.Generic;

public class RingDefault : IRing {
	bool _using = false;
	public void Use ()
	{
		if (!_using) {
			_using = com.things.UseRing (this);
		}
	}

	public void Unuse ()
	{
		if (_using) {
			_using = !com.things.UnuseRing (this);
		}
	}


	#region IRing implementation
	public Guid Guid { get; private set; }
	public IBuff Buff { get; private set; }
	public GameObject gameObject { get; private set; }
	public RingUtils.Color Rare { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }
	#endregion

	public RingDefault(IBuff buff, GameObject gameObject, RingUtils.Color rare, string name, string description) {
		this.Guid = System.Guid.NewGuid ();
		this.gameObject = gameObject;
		this.Rare = rare;
		this.Name = name;
		this.Description = description;
		this.Buff = buff;
	}
}
