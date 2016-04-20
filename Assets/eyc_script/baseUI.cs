using UnityEngine;
using System.Collections;

public class baseUI : MonoBehaviour {
	public static GameObject UIBag;
	public static GameObject UIBar;
	public static draw_bar UIDrawBar;
	public static EasyJoystick UILeftJoy;
	public static EasyJoystick UIRightJoy;

	void ShowBag() {
		UIBag.SetActive (true);

		UIBar.SetActive (false);
		UIDrawBar.enabled = false;
		UILeftJoy.enabled = false;
		UIRightJoy.enabled = false;

		Time.timeScale = 0;
	}

	void HideBag() {
		UIBag.SetActive (false);

		UIBar.SetActive (true);
		UIDrawBar.enabled = true;
		UILeftJoy.enabled = true;
		UIRightJoy.enabled = true;

		Time.timeScale = 1;
	}

	// Use this for initialization
	void Start () {
		UIBag = GameObject.Find ("bag");
		UIBar = GameObject.Find ("bars");
		UIDrawBar = Camera.main.GetComponent<draw_bar> ();
		UILeftJoy = GameObject.Find ("MovingJoystick").GetComponent<EasyJoystick> ();
		UIRightJoy = GameObject.Find ("AttackJoystick").GetComponent<EasyJoystick> ();

		Invoke ("HideBag", 0.01f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBigFireRingClick() {
		com.p.sk.big_fire_ring ();
	}

	public void OnOpenBagClick() {
		ShowBag ();
	}


	public void OnCloseBagClick() {
		HideBag ();
	}
}
