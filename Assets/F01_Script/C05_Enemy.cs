using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//==================================================
// Enemy関係を扱うクラス
//==================================================
public class C05_Enemy : MonoBehaviour {
	private Animator animator;											// Animator コンポーネント用変数

	private C13_Status c13_Status;									// C13_Statusコンポーネント用の変数
	private CharacterController charaController;		// CharacterControllerコンポーネント用の変数

	//-----------------------------------------------------
	// Start()
	//-----------------------------------------------------
	void Start() {
		animator				=	GetComponent< Animator >();							// Animator コンポーネント取得
		c13_Status			=	GetComponent< C13_Status >();						// C13_Status コンポーネント取得
		charaController	=	GetComponent< CharacterController >();	// CharactorController コンポーネント取得
	}

	//-----------------------------------------------------
	// 攻撃を受けた時に呼び出される関数
	//-----------------------------------------------------
	public void atkDamage(C13_Status atk_status){
		c13_Status.damage(atk_status);				// ダメージ処理
		if(c13_Status.getHP() == 0){					// HPがゼロだったら
			animator.SetBool("dead" , true);		// Animatorの変数deadを true に変更.

			charaController.enabled = false;		//CharacterController》コンポーネントをオフに
			Destroy(gameObject, 3.0f);					//3秒後に削除
		}
	}
}
