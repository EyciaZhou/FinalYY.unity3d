using UnityEngine;
using System.Collections;

public interface buff_interface {
	void calculate(attributes_manager.t_mid_attributes mid);
	attributes_manager am { set; }
	System.Guid guid { get; }
}
