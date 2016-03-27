using UnityEngine;
using System.Collections;

public class create_ri : MonoBehaviour {
	public GameObject rippleObj;    //涟漪实例
	int ti;        //计时器
	void Start () 
	{       
	}

	void Update () 
	{
		ti++;
		if (ti < 100)
		{
			GameObject tempObj = Instantiate(rippleObj) as GameObject;        //复制涟漪物体
			tempObj.transform.parent = transform;
			//tempObj.animation.Play();
			tempObj.transform.position=transform.position+new Vector3(f.RandomFloat(10f, -10f),0,f.RandomFloat(10f,-10f));//移动涟漪物体到一个随机位置
		}
	}
}
