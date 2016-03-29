/*Copyright (c) 2013-2015 Eycia Zhou <zhou.eycia@gmail.com>
 * 
 * 许可权特此免费授与获得本软件的副本以及相关文档（“软件”）的任何人不受限制地处理本软件的权利，包括不受限制地使用、
 * 复制、修改、合并、出版、分发、分许可、和／或销售此软件的副本的权利，同时把这样的权利授与获得本软件副本的人，
 * 但必须遵循以下条件：
 * 
 * 本软件的所有副本或实质性部分必须包含以上版权声明和本许可声明。
 * 
 * 本软件以“按现状”的基础提供，不附有任何形式的（无论是明示的还是默示的）保证，包括（但不限于）适销性或适用于某特定
 * 用途的默示保证。无论何种情况，无论是合同诉讼、侵权还是其他诉讼，作者或版权所有者对于任何由于本软件导致的或与本软
 * 件的使用或其他处理有关的任何索赔、损失或其他责任，均不负任何法律责任。
 */
//PASSED
using UnityEngine;
using System.Collections;

public class rigv3 : MonoBehaviour
{
	/* 手写版刚体 版本3
	 * 作者： Eycia Zhou <zhou.eycia@gmail.com>
	 * 
	 * apis :
	 * 		e_rotate(ang)
	 * 			在已有角度上再进行旋转，ang为欧拉角
	 * 		e_rotate_to(ang)
	 * 			旋转至，ang为欧拉角
	 * 		e_rotate_to_q(qua)
	 * 			旋转至，ang为四值数
	 * 		e_move_local(distant)
	 * 			以私有坐标进行移动
	 * 		e_jump(upv)
	 * 			跳跃，向上速率为upv
	 * 		e_lookat(vector3 other)
	 * 			看向
	 * 		bool e_jumped()
	 * 			返回是否跳起
	 */
	float	coll_radius;
	//状态值 人物胶囊的角度
	float	coll_height;
	//状态值	人物胶囊的高度
	Vector3	coll_center;
	//状态值	人物胶囊相对于人物中心点的位移
	Vector3	target_angle;
	//状态值	储存将要旋转至的角度（因为使用了平滑旋转，目标实际朝向与理论朝向有出入，此变量即储存理论朝向）
	Vector3	delta_move;
	//状态值	上一帧至此时理应运动的位移	未进行地形测试
	float	jmp_poor = 0;
	//jump池，解决在LATE_UPDATE中对v同时读写时出现的错误
	float	jmp_v = 0;
	//当前目标纵向速率
	
	public void e_rotate (Vector3 ang)
	{
		target_angle += ang;
	}

	public void e_rotate_to_q (Quaternion ang)
	{
		target_angle = ang.eulerAngles;
	}

	public void e_rotate_to (Vector3 ang)
	{
		target_angle = ang;
	}

	public void e_move_local (Vector3 dis)
	{
		delta_move += dis;
	}

	public void e_jump (float uv)
	{
		jmp_poor -= uv;
	}

	public bool e_jumped ()
	{
		return jmp_v != 0;
	}

	public void e_lookat (Vector3 pos)
	{
		Quaternion tmp = transform.rotation;
		transform.LookAt (pos);
		target_angle = transform.eulerAngles;
		transform.rotation = tmp;
	}

	public void e_lookat_and_keep_vertical (Vector3 pos) {
		Quaternion tmp = transform.rotation;
		transform.LookAt (pos);
		target_angle = new Vector3(0, transform.eulerAngles.y, 0);
		transform.rotation = tmp;
	}

	Vector3 check (Vector3 p1, Vector3 p2, Vector3 movement, int c, bool callback = false)
	{
		/* 
		 * 返回对于由p1， p2， coll_radius构成的胶囊进行movement移动的最大限度
		 * c为反弹递归层数
		 * callback为是否触发OnBeStop
		 * 
		 * void OnBeStop(collider) 当物体被阻挡即返回值不等于movement时触发，参数为阻挡物的collider
		 */
		//p1, p2, movement 全局坐标

		 //print(delta_move);

		if (c > 2) {	//最多递归2层		TODO 疑似超过2层有bug
			return Vector3.zero;
		}

		if (movement == Vector3.zero) {
			return Vector3.zero;
		}

		float dis = movement.magnitude;	//距离，即运动向量长度
		Vector3 dir = movement / dis;	//运动向量方向

		RaycastHit[] rhs = Physics.CapsuleCastAll (p1, p2, coll_radius, dir, dis);	//得到所有碰撞
		float min = 1e9f;
		int mi = -1;
		for (int i = 0; i < rhs.Length; i++) {	//找出最近的碰撞，因为是直线碰撞，所以distance即为最小运动距离
			if (rhs [i].distance < min && rhs [i].collider.gameObject != gameObject) {
				min = rhs [i].distance;
				mi = i;
			}
		}
		if (min == 0) {
			return Vector3.zero;
		}
		movement = Vector3.ClampMagnitude (movement, min);	//返回长度为min，movement中较小值，方向为movement的向量

		if (mi >= 0) {	//如果碰撞了，就加个保护罩，疑似在u3d中当胶囊与阻拦物距离为0时忽略两者碰撞，故将位置部分回移，防止距离为0
			movement -= dir * 0.001f;
		}

		if (mi >= 0 && Mathf.Abs(movement.y) < Mathf.Epsilon) {	//进行反弹，如果对有y轴移动的物体进行反弹，会使角色出现漂移
			Vector3 refc = Vector3.Reflect (dir, rhs [mi].normal) * 0.1f;
			Debug.Log ("reflect" +  refc);
			movement += check (p1 + movement, p2 + movement, refc, c + 1) - dir * 0.001f;
		}
		if (callback && mi >= 0 && c == 1) {	//仅对第一层递归进行触发
			SendMessage ("OnBeStop", rhs [mi].collider, SendMessageOptions.DontRequireReceiver);
		}
		return movement;
	}

	void Start ()
	{
		CapsuleCollider coll = GetComponent<CapsuleCollider> ();
		coll_radius = coll.radius;
		coll_height = coll.height - 2 * coll_radius;
		coll_center = coll.center;
		target_angle = transform.localEulerAngles;
	}

	void LateUpdate ()
	{
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (target_angle), 20 * Time.deltaTime);

		float v = jmp_v;
		if (!e_jumped ()) {
			v += jmp_poor;
		}
		jmp_poor = 0;
		Vector3 p1 = transform.position + coll_center + Vector3.up * -coll_height * .5f;
		Vector3 p2 = p1 + Vector3.up * coll_height;
		Vector3 center = Vector3.zero;
		float foots = 5 * Time.deltaTime;
		v += 10 * Time.deltaTime;

		Vector3 src = Vector3.down * (v * Time.deltaTime - foots);	//跳跃、掉落判定
		Vector3 res = check (p1, p2, src, 1);
		p1 += res;
		p2 += res;
		center += res;


		src = transform.TransformDirection (delta_move);	//判定移动
		res = check (p1, p2, src, 1, true);
		p1 += res;
		p2 += res;
		center += res;


		src = Vector3.down * foots;	//判定脚步掉落
		res = check (p1, p2, src, 1);
		p1 += res;
		p2 += res;
		center += res;


		if (Physics.CapsuleCast (p1, p2, coll_radius, Vector3.down, 0.001f)) {
			if (v >= 0) {	//考虑到跳跃问题
				v = 0;
			}
		}

		jmp_v = v;

		transform.Translate (center, Space.World);
		delta_move = Vector3.zero;
	}
}
