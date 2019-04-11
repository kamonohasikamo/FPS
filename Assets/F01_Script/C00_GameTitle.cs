using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//=====================================================
// GameTitle画面からGame画面へ遷移させるための処理
//=====================================================
public class C00_GameTitle : MonoBehaviour {
	private bool isStart = false;	//GameStart判定

	public Image panel;
	private Color alpha	=	new Color(0, 0, 0, 1.0f);	// 画面切り替え用の透明度変数
	// Update is called once per frame
	void Update() {
		if (isStart) {
			panel.color += alpha * Time.deltaTime;			// panelの透明度を徐々に足していく
			return;
		}
		if (Input.GetMouseButtonDown(0)) {						// 左クリックされたらGameStart
			isStart				=	true;
			panel.enabled	=	true;
			StartCoroutine("sceneTransition");					// コルーチン開始
		}
	}

	//----------------------------------------------
	// 画面遷移用コルーチン
	//----------------------------------------------
	IEnumerator sceneTransition() {
		yield return new WaitForSeconds(1.0f);					//1.0s 処理待機
		Application.LoadLevel("GameScene");						// GameSceneへ遷移
	}
}
