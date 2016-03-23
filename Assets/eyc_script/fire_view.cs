using UnityEngine;
using System.Collections;

public class fire_view : MonoBehaviour {

	public class fire {
		public System.Guid uuid { get; private set; }

		public float alive_time_after_losse {
			get {
				return this.alive_time_after_losse;
			}
			set {
				this.alive_time_after_losse = value;
			}
		}

		public fire() {
			this.uuid = System.Guid.NewGuid();
		}

		public void resize() {
			
		}

		public void set_target() {
		
		}
	}

	public fire new_fire_ball() {
		return null;
	}
}
