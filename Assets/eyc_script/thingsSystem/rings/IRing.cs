using UnityEngine;
using System.Collections;

public interface IRing : IThing {
	//System.Guid Guid { get; }
	IBuff Buff { get; }
	//GameObject gameObject { get; }
	RingUtils.Color Rare { get; }

	void Unuse();
	//string Name { get; }
	//string Description { get; }
}
