using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================================
// Camera関係を扱うクラス
//========================================================
public class C02_CameraController : MonoBehaviour {
	private C01_PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = GameObject.FindWithTag("Player").GetComponent<C01_PlayerController>();
	}

	//------------------------------
	// 視点切り替え
	//------------------------------
	public void changeSight(bool type) {
		if (type) {
			changeCameraMode_1stPerson();
		} else {
			changeCameraMove_3rdPerson();
		}
	}

	//------------------------------
	// 一人称視点処理
	//------------------------------
	private void changeCameraMode_1stPerson() {
		if (transform.parent == null) {                       // もし親オブジェクトがいなければ
			transform.parent = playerController.transform;      // 自身の親オブジェクトに、playerオブジェクトを指定.
			transform.localPosition = Vector3.zero;             // カメラの相対位置を零に
			transform.localEulerAngles = Vector3.zero;          // カメラの相対角度を零に(プレイヤーが向いている方向に)
		}
	}

	//------------------------------
	// 三人称視点処理
	//------------------------------
	private void changeCameraMove_3rdPerson() {
		if (transform.parent != null) {                   // もし親オブジェクトがいるなら
			transform.parent = null;                        // 親子関係を解除
			transform.position = new Vector3(0, 4, -10);    // カメラ位置を基本視点に変更。
			transform.localEulerAngles = Vector3.zero;      // カメラ角度を基本視点に変更
		}
	}

}
