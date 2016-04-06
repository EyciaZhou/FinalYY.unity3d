using UnityEngine;
using System.Collections;

public class ThingsUI : MonoBehaviour {
	public GameObject rDisk;

	private UILabel bag_item_info;
	private UILabel bag_item_name;
	private GameObject item_scroll_view;
	private GameObject lring, rring;


	private ThingsManager manager;

	private void ClearChildOfTransform(Transform tr) {
		foreach (Transform child in tr) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void ReGenerateUI() {
		ClearChildOfTransform (item_scroll_view.transform);
		ClearChildOfTransform (lring.transform);
		ClearChildOfTransform (rring.transform);

		int index = 0;
		foreach (IThing thing in manager.thingsInBag.Values) {
			GameObject disk = (GameObject)Instantiate (rDisk);
			SelectController sc = disk.AddComponent<SelectController> ();
			sc.SetThingIdAndEvent (thing, thing.Use);
			GameObject go = (GameObject)Instantiate (thing.gameObject);

			go.transform.SetParent (disk.transform, false);
			disk.transform.SetParent (item_scroll_view.transform, false);
			disk.transform.localPosition = Vector3.right * 160 * index;
			index++;
		}

		if (manager.LRing != null) {
			GameObject ring = (GameObject)Instantiate (manager.LRing.gameObject);
			ring.transform.SetParent (lring.transform, false);
			lring.GetComponent<SelectController>().enabled = true;
			lring.GetComponent<SelectController> ().SetThingIdAndEvent (manager.LRing, manager.LRing.Unuse);
		} else {
			lring.GetComponent<SelectController>().enabled = false;
		}

		if (manager.RRing != null) {
			GameObject ring = (GameObject)Instantiate (manager.RRing.gameObject);
			ring.transform.SetParent (rring.transform, false);
			rring.GetComponent<SelectController> ().enabled = true;
			rring.GetComponent<SelectController> ().SetThingIdAndEvent (manager.RRing, manager.RRing.Unuse);
		} else {
			rring.GetComponent<SelectController>().enabled = false;
		}
			

		item_scroll_view.transform.position = Vector3.zero;
	}

	public void setInfo(string name, string desp) {
		bag_item_name.text = name;
		bag_item_info.text = desp;
	}

	void Awake() {
		rDisk = Resources.Load<GameObject> ("thing/ui/disk");

		bag_item_info = GameObject.Find ("bag_item_info").GetComponent<UILabel> ();
		bag_item_name = GameObject.Find ("bag_item_name").GetComponent<UILabel> ();
		item_scroll_view = GameObject.Find ("item_scroll_view");
		lring = GameObject.Find ("lring");
		rring = GameObject.Find ("rring");
		manager = GetComponent<ThingsManager> ();
		ReGenerateUI ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
