using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class C93_UIText : MonoBehaviour {
	public Text[] textGunNum;							// UIのテキスト情報を格納する変数配列
	public Text textBomb;									// ボムの表示用
	public RawImage weaponTypeRawImage;		// 武器画像を表示させるRawImageオブジェクト格納用
	public Texture[] textureWeapon;				// 武器画像
	private Animator animatorTextBomb;		// textBombオブジェクトが持っているAnimatorコンポーネントを操作する変数
	public Text textPlayerHP;							// PlayerのHP表示用
	private C13_Status playerStatus;			// Playerのstatus参照用
	public Image panelFlashMonitor;				// damageを受けた時の画面描写
	public Text GameOverText;							// GameOverText

	//------------------------------------------
	// Start()関数よりも先に実行される初期化関数
	//------------------------------------------
	void Awake() {
		animatorTextBomb = GameObject.Find("BombNumTextFront").GetComponent< Animator >();
	}

	//------------------------------------------
	// damageを受けた時の画面描写
	//------------------------------------------
	IEnumerator monitorFlash() {
		panelFlashMonitor.enabled = true;
		yield return new WaitForSeconds(0.1f);
		panelFlashMonitor.enabled = false;
	}

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

	//-----------------------
	// ボムのテキスト変更
	//-----------------------
	public void changeTextBomb(bool used) {
		if (textBomb != null) {
			textBomb.text = used ? "装填中" : "使用可";		// 条件分岐
			animatorTextBomb.SetBool("isUsed", used);		 // bool型の変数"isUsed"に、usedの値に変更.
		}
	}

	//-------------------------------------
	// 武器タイプによるテキスト表示のON, Off
	//-------------------------------------
	public void isChangeText(int type) {
		switch(type) {
			case 0:			// 銃のとき
				foreach(Text t in textGunNum){
					t.enabled = true;
				}
				textBomb.enabled = false;
				break;
			case 1:			// ボムのとき
				foreach(Text t in textGunNum){
					t.enabled = false;
				}
				textBomb.enabled = true;
				break;
		}
	}

	//-----------------------
	// 武器画像を変更する(RawImage)
	//-----------------------
	public void changeWeaponTypeRawImage(int weaponNo) {
		if (weaponNo > textureWeapon.Length - 1) {	// 渡された数字が不正ならアウト
			weaponTypeRawImage.texture = null;
			return;
		}

		weaponTypeRawImage.texture = textureWeapon[weaponNo];		// 画像変更
	}


	//---------------------------------------------------------------
	// テキスト初期化用
	//---------------------------------------------------------------
	public void changeTextPlayerHP() {
		if (textPlayerHP != null) {
			textPlayerHP.text = "体力：" + playerStatus.getHP();
		}
	}

	//---------------------------------------------------------------
	// テキスト初期化用
	//---------------------------------------------------------------
	public void initialize(int type, int num, bool used, C13_Status status){
			isChangeText(type);	// 武器タイプによるテキストの表示オン／オフ

			changeTextGunNum(num);		// 残弾数を変更
			changeTextBomb(used);			// 手榴弾のテキスト変更
			playerStatus = status;
			changeTextPlayerHP();			// PlayerHPChange
	}

	//---------------------------------------------------------------
	// GameOverText
	//---------------------------------------------------------------
	public void showGameOverText() {
		GameOverText.enabled = true;
	}
}
