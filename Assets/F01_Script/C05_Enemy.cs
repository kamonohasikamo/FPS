using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//==================================================
// Enemy関係を扱うクラス
//==================================================
public class C05_Enemy : MonoBehaviour {
	private Animator animator;											// Animator コンポーネント用変数
	private GameObject player;
	private Vector3 move	=	Vector3.zero;						// 移動量保存用変数
	private bool isDead		=	false;

	public float moveSpeed			=	0.5f;
	private const float GRAVITY	=	9.8f;
	private float rotationSpeed	=	180.0f;						// 回転速度

	private C13_Status c13_Status;									// C13_Statusコンポーネント用の変数
	private CharacterController charaController;		// CharacterControllerコンポーネント用の変数

	//-----------------------------------------------------
	// Start()
	//-----------------------------------------------------
	void Start() {
		animator				=	GetComponent< Animator >();							// Animator コンポーネント取得
		c13_Status			=	GetComponent< C13_Status >();						// C13_Status コンポーネント取得
		charaController	=	GetComponent< CharacterController >();	// CharactorController コンポーネント取得
		player					=	GameObject.FindWithTag("Player") as GameObject;
	}

	//-----------------------------------------------------
	// Update()
	//-----------------------------------------------------
	void Update() {
		if (isDead) {
			return;			// 死亡しているので
		}
		enemyMove();	// 移動処理
	}

	//-----------------------------------------------------
	// 攻撃を受けた時に呼び出される関数
	//-----------------------------------------------------
	public void atkDamage(C13_Status atk_status){
		c13_Status.damage(atk_status);				// ダメージ処理
		if(c13_Status.getHP() == 0){					// HPがゼロだったら
			isDead									=	true;			// 死亡判定
			animator.SetBool("dead" , true);		// Animatorの変数deadを true に変更.
			charaController.enabled	=	false;		//CharacterController》コンポーネントをオフに
			Destroy(gameObject, 3.0f);					//3秒後に削除
		}
	}

	//-----------------------------------------------------
	// 敵キャラクターの移動処理
	//-----------------------------------------------------
	private void enemyMove() {
		// 移動量の取得
		float enemyMoveY		=	move.y;
		move								=	(player.transform.position - transform.position).normalized * moveSpeed;	// (プレイヤー位置－自分自身の位置)の正規化 × 移動速度
		move.y							=	0.0f;		// Y方向は不要なので0とする
		// 向きの変更
		Quaternion q				=	Quaternion.LookRotation(move);			// 向きたい方角をQuaternionn型に直す .
		transform.rotation	=	Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);	// 向きを q に向けてじわ～っと変化させる
		// 重力処理
		move.y							=	enemyMoveY;
		move.y							-=	GRAVITY * Time.deltaTime;
		// 移動処理
		charaController.Move(move * Time.deltaTime);	// 移動
	}
}
