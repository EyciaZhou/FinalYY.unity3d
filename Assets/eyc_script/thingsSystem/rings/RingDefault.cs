using System;
using UnityEngine;
using System.Collections.Generic;

public class RingDefault : IRing {
	#region IRing implementation
	public Guid Guid { get; private set; }
	public IBuff Buff { get; private set; }
	public UISprite Sprite { get; private set; }
	public RingUtils.Color Rare { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }
	#endregion

	public RingDefault(IBuff buff, UISprite sprite, RingUtils.Color rare, string name, string description) {
		this.Guid = System.Guid.NewGuid ();
		this.Sprite = sprite;
		this.Rare = rare;
		this.Name = name;
		this.Description = description;
	}
}
