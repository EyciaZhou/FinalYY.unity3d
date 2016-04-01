using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class bars : MonoBehaviour, view_interface {
	int BAR_HEIGHT = 28;

	RectTransform bar_hp_bkg_transform;
	RectTransform bar_hp_transform;

	RectTransform bar_mp_bkg_transform;
	RectTransform bar_mp_transform;

	RectTransform level_number_transfrom;
	Text level_number_text;

	Text debug_info;

	RectTransform bar_exp_transform;
	RectTransform bar_exp_bkg_transform;

	RectTransform bar_top_center_transfrom;

	RectTransform bag_transform;

	private bool modified = false;

	// Use this for initialization
	void Start () {
		bar_hp_bkg_transform = GameObject.Find ("bar_hp_bkg").GetComponent<RectTransform> ();
		bar_hp_transform = GameObject.Find ("bar_hp").GetComponent<RectTransform> ();

		bar_mp_bkg_transform = GameObject.Find ("bar_mp_bkg").GetComponent<RectTransform> ();
		bar_mp_transform = GameObject.Find ("bar_mp").GetComponent<RectTransform> ();

		level_number_transfrom = GameObject.Find ("level_number").GetComponent<RectTransform> ();
		level_number_text = GameObject.Find ("level_number").GetComponent<Text> ();

		bar_exp_bkg_transform = GameObject.Find ("bar_exp_bkg").GetComponent<RectTransform> ();
		bar_exp_transform = GameObject.Find ("bar_exp").GetComponent<RectTransform> ();

		bar_top_center_transfrom = GameObject.Find ("bar_top_center").GetComponent<RectTransform> ();

		bag_transform = GameObject.Find ("bag").GetComponent<RectTransform> ();

		debug_info = GameObject.Find ("debug").GetComponent<Text> ();

		com.p.hp.bind_view (this);
		com.p.mp.bind_view (this);
		com.p.exp.OnExpValueChange += update_view;
	}

	void Update() {
		update_view ();
	}

	public void update_view() {
		modified = true;
	}


	int debug_frameCounter = 0;
	float debug_timeCounter = 0.0f;
	float debug_lastFramerate = 0.0f;
	public float debug_refreshTime = 0.5f;

	void LateUpdate() {
		if (debug_timeCounter < debug_refreshTime) {
			debug_timeCounter += Time.deltaTime;
			debug_frameCounter++;
		} else {
			//This code will break if you set your debug_refreshTime to 0, which makes no sense.
			debug_lastFramerate = (float)debug_frameCounter / debug_timeCounter;
			debug_frameCounter = 0;
			debug_timeCounter = 0.0f;
		}
		debug_info.text = "";
		debug_info.text += debug_lastFramerate;


		BAR_HEIGHT = Screen.height / 20;

		if (modified) {
			modified = false;

			int hp_mx = com.p.hp.max_point;
			int hp_cur = com.p.hp.point;

			bag_transform.anchoredPosition = new Vector2 (BAR_HEIGHT * 2, 0);
			bag_transform.sizeDelta = new Vector2 (BAR_HEIGHT * 3, BAR_HEIGHT * 3);

			bar_top_center_transfrom.anchoredPosition = new Vector2 (0, -BAR_HEIGHT);
			bar_top_center_transfrom.sizeDelta = new Vector2 (-BAR_HEIGHT * 1.5f, 0);

			bar_hp_bkg_transform.sizeDelta = new Vector2 (hp_mx, BAR_HEIGHT);
			bar_hp_bkg_transform.anchoredPosition = new Vector2 (-hp_mx / 2, 0);

			bar_hp_transform.sizeDelta = new Vector2 (hp_cur, BAR_HEIGHT);
			bar_hp_transform.anchoredPosition = new Vector2 (-hp_cur / 2, 0);

			int mp_mx = com.p.mp.max_point;
			int mp_cur = com.p.mp.point;

			bar_mp_bkg_transform.sizeDelta = new Vector2 (mp_mx, BAR_HEIGHT);
			bar_mp_bkg_transform.anchoredPosition = new Vector2 (mp_mx / 2, 0);

			bar_mp_transform.sizeDelta = new Vector2 (mp_cur, BAR_HEIGHT);
			bar_mp_transform.anchoredPosition = new Vector2 (mp_cur / 2, 0);

			long exp_mx = com.p.exp.limit;
			float exp_cur = com.p.exp.current;

			bar_exp_bkg_transform.sizeDelta = new Vector2 (0, BAR_HEIGHT);
			bar_exp_bkg_transform.anchoredPosition = new Vector2 (0, BAR_HEIGHT / 2);

			int width = (int)(Screen.width * exp_cur / exp_mx);
			bar_exp_transform.sizeDelta = new Vector2 (Screen.width * exp_cur / exp_mx, BAR_HEIGHT);
			bar_exp_transform.anchoredPosition = new Vector2 (width / 2, 0);

			level_number_transfrom.sizeDelta = new Vector2 (BAR_HEIGHT, BAR_HEIGHT);

			level_number_text.text =  "lv " + com.p.exp.lv;
		}
	}
}

