using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C12_Bom : MonoBehaviour {
	public GameObject prefab_HitBombEffecf;
	private C92_Sound c92Sound;
	private int bomb_explosion_sound = 2;		// 投げた時の音No.

	// Use this for initialization
	void Start () {
		c92Sound = GameObject.Find("Sound").GetComponent< C92_Sound >();
		StartCoroutine("bom"); //コルーチン開始
	}

	//-------------------
	// ボムの爆破処理
	//-------------------
	IEnumerator bom() {
		yield return new WaitForSeconds(2.5f); //2.5s処理を待機

		GameObject effect = Instantiate(prefab_HitBombEffecf, transform.position, Quaternion.identity) as GameObject; //ボムエフェクト発生
		Destroy(effect, 1.0f); //ボムエフェクト1秒後削除

		c92Sound.SendMessage("soundStart", bomb_explosion_sound); // ボム攻撃時の音
		bombAttack();

		Destroy(gameObject);
	}

	//-------------------
	// ボムの攻撃処理
	//-------------------
	private void bombAttack() {
		Collider[] targets = Physics.OverlapSphere(transform.position, 1.0f); //ボムの半径0.7以内にいるColliderを探し、配列に格納
		foreach(Collider obj in targets) { //targets配列内を順に処理、その時に仮名objとする
			if (obj.tag == "Enemy") { //objのtagがEnemyなら消す処理
				Destroy(obj.gameObject);
			}
		}
	}
}
