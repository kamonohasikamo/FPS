using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================================
// ダメージ系を扱うクラス
//========================================================
public class C14_DamageStatus : MonoBehaviour {
	private C13_Status c13_Status;

	//------------------------------------------------------
	// Start()
	//------------------------------------------------------
	void Start() {
		c13_Status	=	GetComponent< C13_Status >();
	}

	//------------------------------------------------------
	// Is Trigger状態のこりだーに接触している間だけ呼び出される関数
	//------------------------------------------------------
	void OnTriggerStay(Collider otherObject) {
		if (otherObject.gameObject.tag == "HitArea") { //ぶつかっている相手がHitAreaタブなら
			otherObject.transform.root.GetComponent< C01_PlayerController >().clashDamage(c13_Status);	// 接触ダメージを与える
		}
	}
}
