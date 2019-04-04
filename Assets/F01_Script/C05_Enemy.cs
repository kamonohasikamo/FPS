using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C05_Enemy : MonoBehaviour
{
	private Animator animator;		// Animator コンポーネント用変数


	void Start() {
		animator	=	GetComponent< Animator >();	// Animator コンポーネント取得
	}

	void damage() {
		animator.SetBool("dead", true);	// Animator の変数 dead をtrueに変える処理
	}
}
