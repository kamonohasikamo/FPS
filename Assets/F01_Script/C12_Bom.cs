using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C12_Bom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("bom"); //コルーチン開始
	}

	//-------------------
	// ボムの爆破処理
	//-------------------
	IEnumerator bom() {
		yield return new WaitForSeconds(2.5f); //2.5s処理を待機
		Destroy(gameObject);
	}
}
