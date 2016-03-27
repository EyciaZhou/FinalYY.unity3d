using UnityEngine;
using System.Collections;

public interface buff_interface {
	void calculate(attributes_manager.t_mid_attributes mid);
	attributes_manager.t_buff_change_callback change_callback { get; set; }
	int priority { get; set; }
	bool vaild { get; }
	attributes_manager am { get; set; }
	System.Guid guid { get; }
}
