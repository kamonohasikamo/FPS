using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_PlayerController : MonoBehaviour {

    private CharacterController charaController;//charaコンポーネント用変数
    private Vector3 move = Vector3.zero;        //chara移動量ベクトル
    private float speed = 5.0f;                 //スピード

	// Use this for initialization
	void Start () {
        charaController = GetComponent<CharacterController>();	
	}
	
	// Update is called once per frame
	void Update () {
        move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        charaController.Move(move * speed * Time.deltaTime);    //chara移動
	}
}
