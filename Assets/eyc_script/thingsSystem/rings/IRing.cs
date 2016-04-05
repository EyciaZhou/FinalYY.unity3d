using UnityEngine;
using System.Collections;

public interface IRing  {
	System.Guid Guid { get; }
	IBuff Buff { get; }
	UISprite Sprite { get; }
	RingUtils.Color Rare { get; }
	string Name { get; }
	string Description { get; }
}
