using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//==============================================
// Scoreを扱うクラス
//==============================================
public class C94_Score : MonoBehaviour {
	private int score = 0;
	private C93_UIText c93_UI;

	// Start is called before the first frame update
	void Start() {
		c93_UI = GetComponent< C93_UIText >(); // 同じオブジェクトが持っている《C93_Ui》コンポーネントを取得
	}

	// Score + 1
	public void addScore() {
		score++;
		c93_UI.changeTextScore(score);
	}
}
