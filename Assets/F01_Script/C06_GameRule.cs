using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//================================================
// GameRuleClass
//================================================
public class C06_GameRule : MonoBehaviour {
	private C93_UIText c93_UIText;
	private GameObject player;
	private C13_Status c13_Status;

	private  bool isGameOver = false;

	//--------------------------------------------------
	// Start()
	//--------------------------------------------------
	void Start() {
		c93_UIText		=	GameObject.Find("GameRoot").GetComponent< C93_UIText >();
		player				=	GameObject.FindGameObjectWithTag("Player") as GameObject;
		c13_Status		=	player.GetComponent< C13_Status >();
	}

	//--------------------------------------------------
	// Update()
	//--------------------------------------------------
	void Update() {
		if (isGameOver) {
			return;
		}

		isPlayerDropDown();	// Playerが落ちたかどうか
		isPlayerDead();			// PlayerのHPチェック
	}

	//--------------------------------------------------
	// GameOverProcessing
	//--------------------------------------------------
	public void gameOverProcessing() {
		isGameOver = true;
		c93_UIText.showGameOverText();		// Text表示
	}

	//--------------------------------------------------
	// getIsGameOver
	//--------------------------------------------------
	public bool getIsGameOver() {
		return isGameOver;
	}

	//--------------------------------------------------
	// Playerが落ちたかどうか
	//--------------------------------------------------
	private void isPlayerDropDown() {
		if (player.transform.position.y <= -50.0f) {
			gameOverProcessing();
			player.GetComponent< C01_PlayerController >().enabled = false;
		}
	}

	//--------------------------------------------------
	// PlayerのHPチェック
	//--------------------------------------------------
	private void isPlayerDead() {
		if(c13_Status.getHP() == 0) {
			gameOverProcessing();
			player.GetComponent< C01_PlayerController >().enabled = false;
		}
	}
}
