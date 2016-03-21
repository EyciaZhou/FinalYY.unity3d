using UnityEngine;
using System.Collections;

public class bars : MonoBehaviour, view_interface {
	const int BAR_HEIGHT = 28;

	GameObject bar_hp_bkg;
	RectTransform bar_hp_bkg_transform;
	GameObject bar_hp;
	RectTransform bar_hp_transform;

	GameObject bar_mp_bkg;
	RectTransform bar_mp_bkg_transform;
	GameObject bar_mp;
	RectTransform bar_mp_transform;

	GameObject level_number;

	GameObject bar_exp_bkg;
	GameObject bar_exp;

	private bool modified = false;

	// Use this for initialization
	void Start () {
		bar_hp_bkg = GameObject.Find ("bar_hp_bkg");
		bar_hp_bkg_transform = bar_hp_bkg.GetComponent<RectTransform> ();
		bar_hp = GameObject.Find ("bar_hp");
		bar_hp_transform = bar_hp.GetComponent<RectTransform> ();


		bar_mp_bkg = GameObject.Find ("bar_mp_bkg");
		bar_mp_bkg_transform = bar_mp_bkg.GetComponent<RectTransform> ();
		bar_mp = GameObject.Find ("bar_mp");
		bar_mp_transform = bar_mp.GetComponent<RectTransform> ();

		level_number = GameObject.Find ("level_number");

		bar_exp_bkg = GameObject.Find ("bar_exp_bkg");
		bar_exp = GameObject.Find ("bar_exp");

		com.p.hp.bind_view (this);
		com.p.mp.bind_view (this);
		com.p.mp.bind_view (this);
	}

	void Update() {
		update_view ();
	}

	public void update_view() {
		modified = true;
	}

	void LateUpdate() {
		if (modified) {
			modified = false;

			int hp_mx = com.p.hp.max_point;
			int hp_cur = com.p.hp.point;

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

			int exp_mx = com.p.exp.limit;
			int exp_cur = com.p.exp.current;

			bar_exp_transform.sizeDelta = new Vector2 (mp_cur, BAR_HEIGHT);
			bar_exp_transform.anchoredPosition = new Vector2 (mp_cur / 2, 0);
		}
	}
}

