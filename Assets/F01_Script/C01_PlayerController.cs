using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_PlayerController : MonoBehaviour {

    private CharacterController charaController;//charaコンポーネント用変数
    private Vector3             move = Vector3.zero;        //chara移動量ベクトル
	private Vector3				attackPoint;				//攻撃位置
    private float               speed = 5.0f;                 //スピード
    private const float         GRAVITY = 20.8f;         //重力定数
    private float               jumpPower = 15.0f;            //跳躍力
    private float rotationSpeed = 180.0f;
    private bool moveType = true;

    public GameObject targetEnemy = null;
	public GameObject prefab_hitEffect1; //攻撃時のヒットエフェクト

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
        setTargetEnemy();
        leftClickAction();
        if (moveType)
        {
            playerCameraPosition_1stPerson();
        }else
        {
            playerCameraPosition_3rdPerson();
        }
        
	}
	//------------------------------
	// ターゲット情報を取得
	//------------------------------
    private void setTargetEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //クリックした位置から真っ直ぐ奥に行く光線
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 10))// カメラから距離10の光線を出し、もし何かに当たったら
        {
            if(hitInfo.collider.gameObject.tag == "Enemy")// その当たったオブジェクトのタグ名が Enemy なら
            {
                targetEnemy = hitInfo.collider.gameObject;// 当たったオブジェクトを参照
				attackPoint = hitInfo.point;			//マウスの位置情報を取得
                return;// ターゲットが見つかったので処理を抜ける
            }
        }

        targetEnemy = null;
    }
	//------------------------------
	//左クリックで敵を攻撃
	//------------------------------
    private void leftClickAction()
    {
        if (Input.GetMouseButton(0) && targetEnemy != null)//もし左クリックを押され、かつ敵がnullでなければ
        {
			GameObject effect = Instantiate(prefab_hitEffect1, attackPoint, Quaternion.identity) as GameObject; //エフェクト発生
			Destroy(effect, 0.2f); //effect削除
            Destroy(targetEnemy);//敵を削除
        }
    }

    public void changeModeType(bool type)
    {
        moveType = type;
    }
    
    private void playerCameraPosition_1stPerson()
    {
        float y = move.y;                                       //move.yの値を保持しておく。
        move = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        move *= speed;                                          //移動速度を乗算
        
        //ジャンプ&重力処理
        move.y += y;                                            //move.yの値を戻す
        if (charaController.isGrounded)                         //地面に足がついているかどうか
        {
            if (Input.GetKeyDown(KeyCode.Space))                    //Spaceが押されたら跳躍力分上にあげる
            {
                move.y = jumpPower;
            }
        }
        move.y = move.y - (GRAVITY * Time.deltaTime);           //y軸方向に重力を追加(代入)

        //playerの向き判定等
        Vector3 playerDir = new Vector3(Input.GetAxis("Horizontal"), 0.0f,0.0f);
        playerDir = transform.TransformDirection(playerDir);
        if (playerDir.magnitude > 0.1f)
        {
            Quaternion q = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
        }
        charaController.Move(move * Time.deltaTime);            //chara移動
    }

    private void playerCameraPosition_3rdPerson()
    {
        //移動量
        float y = move.y;
        move = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        Vector3 playerDir = move;
        move *= speed;                                          //移動速度を乗算

        //ジャンプ&重力処理
        move.y += y;                                            //move.yの値を戻す
        if (charaController.isGrounded)                         //地面に足がついているかどうか
        {
            if (Input.GetKeyDown(KeyCode.Space))                    //Spaceが押されたら跳躍力分上にあげる
            {
                move.y = jumpPower;
            }
        }
        move.y = move.y - (GRAVITY * Time.deltaTime);           //y軸方向に重力を追加(代入)

        //playerの向き判定等
        if (playerDir.magnitude > 0.1f)
        {
            Quaternion q = Quaternion.LookRotation(playerDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
        }
        charaController.Move(move * Time.deltaTime);            //chara移動
    }
}


