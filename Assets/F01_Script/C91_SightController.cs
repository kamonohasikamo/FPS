using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==================================================
// 視点に関する処理を記載するクラス
//==================================================
public class C91_SightController : MonoBehaviour {
	private C01_PlayerController playerController;
	private C02_CameraController mainCamera, subCamera;
	private bool cameraSight = true;        // １人称視点：true  , ３人称視点：flase

	// Use this for initialization
	void Start() {
		playerController	=	GameObject.Find("Player").GetComponent<C01_PlayerController>();
		mainCamera				=	GameObject.Find("Main Camera").GetComponent<C02_CameraController>();
		subCamera					=	GameObject.Find("Sub Camera").GetComponent<C02_CameraController>();

		changeCameraSight();    // 視点切り替え命令
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			cameraSight = !cameraSight;
			changeCameraSight();    // 視点切り替え命令
		}
	}

	//----------------------------------------
	// 視点切り替え命令
	//----------------------------------------
	private void changeCameraSight() {
		playerController.changeModeType(cameraSight);     // プレイヤーの動作モードタイプ変更
		mainCamera.changeSight(cameraSight);    // メインカメラの視点切り替え
		subCamera.changeSight(!cameraSight);    // サブカメラの視点切り替え
	}
}
