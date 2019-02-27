using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class C93_UIText : MonoBehaviour {
	public Text[] textGunNum;		// UIのテキスト情報を格納する変数配列

	//-----------------------
	// 残弾数を変更する(Text)
	//-----------------------
	public void changeTextGunNum(int gunNum) {
		foreach(Text t in textGunNum) {	// textGunNum配列の中身を操作する
			if (t != null) {							// tがnullでない(textGunNum配列の中身)ならばテキスト変更
				t.text = "残弾：" + gunNum;
			}
		}
	}
}
