﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_PlayerController : MonoBehaviour {

    private CharacterController charaController;//charaコンポーネント用変数
    private Vector3 move = Vector3.zero;        //chara移動量ベクトル
    private float speed = 5.0f;                 //スピード
    private const float GRAVITY = 20.8f;         //重力定数
    private float jumpPower = 15.0f;            //跳躍力

    /*
     * C#のお話:
     * constは定数という意味。定数なので重力はすべて大文字で宣言。
     */

	// Use this for initialization
	void Start () {
        charaController = GetComponent<CharacterController>();	
	}
	
	// Update is called once per frame
	void Update () {
        float y = move.y;                                       //move.yの値を保持しておく。
        move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move *= speed;                                          //移動速度を乗算
        move.y += y;                                            //move.yの値を戻す
        if (charaController.isGrounded)                         //地面に足がついているかどうか
        { 
            if (Input.GetKeyDown(KeyCode.Space))                    //Spaceが押されたら跳躍力分上にあげる
            {
                move.y = jumpPower;
            }
        }
        move.y = move.y - (GRAVITY * Time.deltaTime);           //y軸方向に重力を追加(代入)
        charaController.Move(move * Time.deltaTime);            //chara移動
	}
    /*Updateの中身
     *方程式で考えると
     * 速度V = 初速度V0 + 重力加速度g * 時間t
     * を計算している。
     */
}
