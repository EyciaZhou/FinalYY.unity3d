using UnityEngine;
using System.Collections;

public interface IBuff {
	void calculate(AttributesManager.MidAttributes mid);
	System.Guid Guid { get; }
	AttributesManager AM { set; }
}
