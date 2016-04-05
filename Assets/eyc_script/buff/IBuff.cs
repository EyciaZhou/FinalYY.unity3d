using UnityEngine;
using System.Collections;

public interface IBuff {
	void calculate(attributes_manager.t_mid_attributes mid);
	System.Guid Guid { get; }
}
