using UnityEngine;
using System.Collections;

public interface buff_interface {
	void calculate(attributes.mid_attributes mid);
	attributes.t_buff_change_callback change_callback { get; set; }
	int priority { get; set; }
	bool vaild { get; }
	attributes attr { get; set; }
	System.Guid guid { get; set; }
}
