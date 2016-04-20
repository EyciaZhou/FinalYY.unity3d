using UnityEngine;
using System.Collections;

public class SelectController : MonoBehaviour {
	//gameObject is disk
	//pressed = hover
	//and normal = disabled

	public delegate void UseEvt();

	private bool stepTwo = false;
	private IThing thing;

	//private System.Guid thingID;
	private UseEvt evt;

	public void SetThingIdAndEvent(IThing thing, UseEvt evt) {
		this.thing = thing;
		this.evt = evt;
	}

	private void showInfo() {
		com.things.ui.setInfo (thing.Name, thing.Description, thing.color);
	}

	private void stepOneClick() {
		stepTwo = true;
		UIButton btn = gameObject.GetComponent<UIButton>();
		btn.normalSprite = btn.pressedSprite;

		StartCoroutine (coSetNormal ());
	}

	private IEnumerator coSetNormal() {
		yield return CoroutineUtil.WaitForRealSeconds (2);
		setNormal ();
	}

	private void setNormal() {
		stepTwo = false;
		UIButton btn = gameObject.GetComponent<UIButton>();
		btn.normalSprite = btn.disabledSprite;
	}

	private void stepTwoClick() {
		setNormal ();

		evt ();
	}

	public void OnClick() {
		if (enabled) {
			showInfo ();
			if (stepTwo) {
				stepTwoClick ();
			} else {
				stepOneClick ();
			}
		}
	}

	// Use this for initialization
	void Start () {
		//Debug.Log (thingID);
		//Debug.Log (com.things.thingsInBag);

		//thing = com.things.thingsInBag [thingID];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
