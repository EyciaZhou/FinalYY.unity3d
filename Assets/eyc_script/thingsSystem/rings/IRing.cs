using UnityEngine;
using System.Collections;

public interface IRing : IThing {
	IBuff Buff { get; }
	RingUtils.Color Rare { get; }

	void Unuse();
}
