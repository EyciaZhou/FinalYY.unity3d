using System;

public interface controller_interface
{
	attributes attr { get; set; }
	void update_controller ();
	System.Guid guid { get; set; }
}