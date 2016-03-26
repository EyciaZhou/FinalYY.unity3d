//Copyright (c) 2013-2015 Eycia Zhou <zhou.eycia@gmail.com>
//
//许可权特此免费授与获得本软件的副本以及相关文档（“软件”）的任何人不受限制地处理本软件的权利，
//包括不受限制地使用、复制、修改、合并、出版、分发、分许可、和／或销售此软件的副本的权利，同时
//把这样的权利授与获得本软件副本的人，但必须遵循以下条件：
//
//本软件的所有副本或实质性部分必须包含以上版权声明和本许可声明。
//
//本软件以“按现状”的基础提供，不附有任何形式的（无论是明示的还是默示的）保证，包括（但不限于）
//适销性或适用于某特定用途的默示保证。无论何种情况，无论是合同诉讼、侵权还是其他诉讼，作者或版
//权所有者对于任何由于本软件导致的或与本软件的使用或其他处理有关的任何索赔、损失或其他责任，均
//不负任何法律责任。

using UnityEngine;
using System.Collections;

/*
 * 镜头与人物控制器
 * ---------------------------------------------------------------------------
 * 使用：
 * 	将本与其他公共脚本放在一个物体上，本脚本依赖com， rigv3
 * ---------------------------------------------------------------------------
 * 作用：
 * 	镜头：可调整各参数以调整镜头姿态
 * 	人物控制：从rigv3中获得当前跳跃数据，从屏幕的操纵杆获得移动数据，通过com组件获取
 * 		当前速度，通过rigv3实现移动与转向。摄像机移动带缓冲池，人物移动与转向缓冲池
 * 		在rigv3中。
 * ---------------------------------------------------------------------------
 */

public class camera_and_input : MonoBehaviour
{
	Camera cam;

	string idleClipName = "CombatModeB";
	string runClipName = "Run";
	string jumpClipName = "JumoRun";
	string movingJoystickName = "MovingJoystick";
	string AttackJoystickName = "AttackJoystick";

	//camera
	//ScrollWheel
	float	sw_Dist = 0.0f;
	//*滚轮目前位置 ∈[0,1]
	float	sw_Spd = 1.0f;
	//滚轮灵敏度，越大越灵敏

	//Mode 0
	float cam_Max = 30.0f;
	//摄像机与人最远距离，为长度值，供滚动用
	float cam_Min = 10.0f;
	//摄像机与人最近距离，为长度值，供滚动用
	float cam_Up = 1.0f;
	//摄像机与【瞄准位置】的高度差，以人物中心点为0，详见图
	float cam_ang_deg = 50.0f;
	//摄像机下潜角度

	//重力模块
	rigv3	rig = null;

	public void init ()
	{
		rig = GetComponent<rigv3> ();
		cam = Camera.main;

		if (rig == null) {
			rig = gameObject.AddComponent<rigv3> ();
		}
	}

	void Update ()
	{
		UpdateSW ();	//测定滚轮程度
	}

	void LateUpdate ()
	{
		UpdateCamera ();
	}

	void OnEnable ()
	{
		EasyJoystick.On_JoystickMove += OnJoystickMove;
		EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;
	}

	//移动摇杆结束
	void OnJoystickMoveEnd (MovingJoystick move)
	{
		//停止时，角色恢复idle
		if (move.joystickName == movingJoystickName) {
			GetComponent<Animation>().CrossFade (idleClipName);
		}
	}

	void OnJoystickMove (MovingJoystick move)
	{
		if (move.joystickName == movingJoystickName) {
			if (move.joystickAxis.magnitude == 0) {
				GetComponent<Animation>().CrossFade (idleClipName);
			} else {
				if (!rig.e_jumped ())
					GetComponent<Animation>().CrossFade (runClipName);
				else
					GetComponent<Animation>().CrossFade (jumpClipName);

				rig.e_rotate_to (new Vector3 (0, 90 - f.vector2_to_angle_deg (move.joystickAxis), 0));
				rig.e_move_local (Vector3.forward * com.c_Spd * Time.deltaTime * move.joystickValue.magnitude);
			}
		}

		if (move.joystickName == AttackJoystickName) {
			com.fires.new_fire_ball_default ().target = GameObject.Find ("target");
			Debug.Log (GameObject.Find ("target").name);
		}
	}

	//更新滚轮状态
	void UpdateSW ()
	{
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			sw_Dist += -Input.GetAxis ("Mouse ScrollWheel") * sw_Spd;
		}
		if (sw_Dist > 1) {
			sw_Dist = 1;
		}
		if (sw_Dist < 0) {
			sw_Dist = 0;
		}
	}

	//摄像机的平滑转向
	void doCamRota (Vector3 pos, Vector3 ea)
	{
		if (10 * Time.deltaTime > 1) {
			cam.transform.position = pos;
			cam.transform.eulerAngles = ea;
		} else {
			cam.transform.position = Vector3.Slerp (cam.transform.position, pos, 10 * Time.deltaTime);
			cam.transform.rotation = Quaternion.Slerp (cam.transform.rotation, Quaternion.Euler (ea), 10 * Time.deltaTime);
		}
	}

	//摄像头位置
	void UpdateCamera ()
	{
		float cam_Dist = cam_Min + (cam_Max - cam_Min) * sw_Dist;
		Vector3 p_pos = transform.position;
		doCamRota (new Vector3 (
			p_pos.x,
			p_pos.y + cam_Dist * f.sin_deg (cam_ang_deg) + cam_Up,
			p_pos.z - cam_Dist * f.cos_deg (cam_ang_deg)
		), new Vector3 (cam_ang_deg, 0, 0));
	}
}
