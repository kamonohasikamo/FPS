using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=====================================================
// ステータスに関するクラス
//=====================================================
public class C13_Status : MonoBehaviour {
	public int HP 	= 10;
	public int ATK	= 1;
	//----------------------------------------------
	// setter, getter
	//----------------------------------------------
	public void setHP (int hp) {
		HP = hp;
	}

	public void setATK(int atk) {
		ATK = atk;
	}

	public int getHP() {
		return HP;
	}

	public int getATK() {
		return ATK;
	}

	//-----------------------------------------------
	// ダメージ処理
	//-----------------------------------------------
	public void damage(C13_Status status) {
		HP = Mathf.Max(0, HP - status.getATK()); // 0 もしくは HP-敵ATK の値で、大きい方をHPに入れる (必ず0以上)
	}
}
