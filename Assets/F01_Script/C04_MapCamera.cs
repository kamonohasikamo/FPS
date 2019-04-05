using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C04_MapCamera : MonoBehaviour
{
	private GameObject player;

	private int cameraViewMode	= 0;																	// CameraViewMode
	private int CAMERAMODENUM		= 3;																	// Camera表示モードの個数
	private Vector3 modeA_basicPosition = new Vector3( 0 , 12 ,   0);	// orthographicモードの基本位置
	private Vector3 modeA_basicRotation = new Vector3(90 ,  0 ,   0);	// orthographicモードの基本角度
	private Vector3 modeB_basicPosition = new Vector3( 0 , 30 , -20);	// Perspectiveモードの基本位置
	private Vector3 modeB_basicRotation = new Vector3(60 ,  0 ,   0);	// Perspectiveモードの基本角度

	private float offsetZ;																						// Z座標のオフセット


	//--------------------------------------------------
	// Start()
	//--------------------------------------------------
	void Start() {
		player	=	GameObject.FindGameObjectWithTag("Player") as GameObject;
		offsetZ	=	modeA_basicPosition.z;

		cameraPositionUpdate(true);		// 位置更新。trueを渡すとX座標も変更
	}

	//--------------------------------------------------
	// Update()
	//--------------------------------------------------
	void Update() {
		if (Input.GetKeyDown(KeyCode.Tab)) {	// TabKeyが押されたら画面変更
			changeCameraSight();
		}
		cameraPositionUpdate(false);
	}


	//--------------------------------------------------
	// 位置更新
	//--------------------------------------------------
	private void cameraPositionUpdate(bool isCameraPositionXCange) {
		Vector3 position	= transform.position;
		if (isCameraPositionXCange) {	// TrueならばX座標も更新
			position.x = player.transform.position.x;
		}
		position.z					=	player.transform.position.z + offsetZ;	// Zには、オフセット値を乗せる
		transform.position	=	position;
	}

	//--------------------------------------------------
	// 視点切り替え命令
	//--------------------------------------------------
	private void changeCameraSight() {
		cameraViewMode = (cameraViewMode + 1) % CAMERAMODENUM;

		switch (cameraViewMode) {
			case 0:
				GetComponent<Camera>().orthographic			= true;							// Projection の orthographic を trueにする
				GetComponent<Camera>().orthographicSize	= 11.88f;						// orthographicのsize(表示距離)を変更.
				changeCameraStatus(GetComponent<Camera>().orthographic);		// Camera位置切り替え
				break;
			case 1:
				GetComponent<Camera>().orthographicSize	= 20.0f;						// orthographicのsize(表示距離)を変更.
				break;
			case 2:
				GetComponent<Camera>().orthographic			= false;						// Projection の orthographic を falseにする
				changeCameraStatus(GetComponent<Camera>().orthographic);		// Camera位置切り替え
				break;
		}
	}

	//--------------------------------------------------
	// Camera位置切り替え
	//--------------------------------------------------
	private void changeCameraStatus(bool isCameraStatus) {
		Vector3 position	=	transform.position;

		if (isCameraStatus) {
			position.y						=	modeA_basicPosition.y;	// 高さ変更
			offsetZ								=	modeA_basicPosition.z;	// オフセットZ変更
			transform.eulerAngles	= modeA_basicRotation;		// 角度変更
		} else {
			position.y						= modeB_basicPosition.y;	// 高さ変更
			offsetZ								= modeB_basicPosition.z;	// オフセットZ変更
			transform.eulerAngles	= modeB_basicRotation;		// 角度変更
		}
		position.z						= player.transform.position.z + offsetZ;	// Zには、オフセット値を乗せる
		transform.position 		= position;
	}
}
