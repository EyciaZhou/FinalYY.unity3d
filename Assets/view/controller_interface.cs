using System;

public interface controller_interface
{
	attributes_manager am { get; set; }
	void update_controller ();
	void bind_view(view_interface v);
	System.Guid guid { get; }
}