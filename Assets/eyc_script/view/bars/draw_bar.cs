using UnityEngine;
using System.Collections;

public class draw_bar : MonoBehaviour
{
	public Texture coin;
	public Texture []numbers;

	public Texture mo_bac;
	public Texture mo_for;
	
	long jyfix = (long)(50 * (50.0f / 35));
	long barHei = 50;
	float barRate = 0.075f * (50.0f / 35);
	int drawed_coin = 0;

	void Start()
	{	
	}
	
	void OnGUI()
	{
		//long h = Screen.height; 

		foreach (monster go in com.mon.monsters) {
			float hei = go.GetComponent<Collider>().bounds.size.y * go.transform.localScale.y;
			Vector3 w_pos = new Vector3(go.transform.position.x, go.transform.position.y + hei, go.transform.position.z);
			Vector2 pos = GetComponent<Camera>().WorldToScreenPoint(w_pos);
			pos = new Vector2(pos.x, Screen.height - pos.y);
			GUI.DrawTexture(new Rect(pos.x - 20, pos.y - 10, 40, 8), mo_bac);
			GUI.DrawTexture(new Rect(pos.x - 20, pos.y - 10, 40 * go.GetComponent<hp_handler>().getRate(), 8), mo_for);
		}

		long w = Screen.width;

		GUI.DrawTexture(new Rect(0, 0, 100, 100), coin);
		int c_coin = com.coins.hv_coin;
		if (drawed_coin > c_coin) {
			drawed_coin--;
		} else if (drawed_coin < c_coin) {
			drawed_coin++;
		}
		int pos_i = 0;
		int t_coin = drawed_coin;
		do {
			GUI.DrawTexture(new Rect(300 - 50 * pos_i, 10, 50, 80), numbers[t_coin % 10]);

			t_coin /= 10;
			pos_i++;
		} while(t_coin > 0);
	}
}
