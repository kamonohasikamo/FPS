using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C01_PlayerController : MonoBehaviour {

  private CharacterController charaController;  //charaコンポーネント用変数
  private C92_Sound c92_Sound;
  private C93_UIText c93_UI;
  private Vector3 move = Vector3.zero;          //chara移動量ベクトル
  private Vector3	attackPoint;                  //攻撃位置
  private C11_Weapon weapon;                    //武器
  private float        speed = 5.0f;            //スピード
  private const float  GRAVITY = 20.8f;         //重力定数
  private float        jumpPower = 15.0f;       //跳躍力
  private float        rotationSpeed = 180.0f;  //Playerの回転速度
  private bool         moveType = true;         //一人称視点動作
  private bool bomb_used = false;               // ボムの連射制限
  private const int GUN_MAX_BULLETNUM = 20;     // 弾数最大値
  private int gun_sound = 0;                    // 銃撃音No
  private int bomb_throw_sound = 1;             // ボム投げる音No

  public int gunBulletNum;                      //弾数
  public GameObject targetEnemy = null;         //ターゲット格納用変数
	public GameObject prefab_hitEffect1;          //攻撃時のヒットエフェクト
  public GameObject prefab_bom;                 //ボム


    /*
     * C#のお話:
     * constは定数という意味。定数なので重力はすべて大文字で宣言。
     */

	// Use this for initialization
  // Gameが始まるとまずここが読まれる
	void Start () {
    charaController = GetComponent<CharacterController>();
    c93_UI = GameObject.Find("GameRoot").GetComponent< C93_UIText >();
    weapon = new C11_Weapon();  //C11_Weapon型のweaponの変数のメモリ領域を確保し、そこを参照せよ
    gunBulletNum = GUN_MAX_BULLETNUM; // 弾数代入
    // c93_UI.changeTextGunNum(gunBulletNum); // 下記のinitialize()に集約するため、削除
    c92_Sound = GameObject.Find("Sound").GetComponent< C92_Sound >();
    c93_UI.initialize(weapon.getWeaponType() , gunBulletNum , bomb_used);	// テキストの初期化
	}

	// Update is called once per frame
	void Update () {
    setTargetEnemy();
    if(Input.GetMouseButtonDown(0)) {  //左クリックで攻撃(長押しで連射)(あんまりよくないかも) -> GetMouseButtonDownで単発
      weaponAttack();
    }
    if(Input.GetMouseButtonDown(1)) {  //右クリックで武器切り替え
      changeWeaponMode();
    }
    if (moveType) {
      playerCameraPosition_1stPerson();
    } else {
      playerCameraPosition_3rdPerson();
    }

	}
	//------------------------------
	// ターゲット情報を取得
	//------------------------------
    private void setTargetEnemy() {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //クリックした位置から真っ直ぐ奥に行く光線
      RaycastHit hitInfo;

      if(Physics.Raycast(ray, out hitInfo, 10)) {// カメラから距離10の光線を出し、もし何かに当たったら
        if(hitInfo.collider.gameObject.tag == "Enemy") {// その当たったオブジェクトのタグ名が Enemy なら
          targetEnemy = hitInfo.collider.gameObject;// 当たったオブジェクトを参照
				  attackPoint = hitInfo.point;			//マウスの位置情報を取得
          return;// ターゲットが見つかったので処理を抜ける
        }
      }

      targetEnemy = null;
    }
	//------------------------------
	// 左クリックで敵を攻撃
	//------------------------------
    private void weaponAttack() {
      switch(weapon.getWeaponType()) {
        case 0: //銃攻撃
          attack01_gun();
          break;
        case 1: //ボム攻撃
          attack02_bom();
          break;
      }
    }

    //------------------------------
  	// 銃の攻撃
  	//------------------------------
    private void attack01_gun() {
      if (gunBulletNum <= 0) {
        return; //残弾がないので
      }
      if(targetEnemy != null) { //target(敵)が存在すれば
        GameObject effect = Instantiate(prefab_hitEffect1, attackPoint, Quaternion.identity) as GameObject; //エフェクト発生
        Destroy(effect, 0.2f); //effect削除
        Destroy(targetEnemy);  //敵削除
      }

      c92_Sound.SendMessage("soundStart", gun_sound);  // 音を鳴らす

      gunBulletNum--;
      c93_UI.changeTextGunNum(gunBulletNum);  // UITextの表示を変える
      if (gunBulletNum <= 0) {
        StartCoroutine("reChargeGun");  //銃のコルーチン開始
      }
    }

    //------------------------------
    // 銃の使用制限コルーチン
    //------------------------------
    IEnumerator reChargeGun() {
      yield return new WaitForSeconds(3.0f);  // 3.0s処理を待機
      gunBulletNum = GUN_MAX_BULLETNUM;
      c93_UI.changeTextGunNum(gunBulletNum);  // UITextの表示を変える
    }

    //------------------------------
  	// ボムの攻撃
  	//------------------------------
    private void attack02_bom() {
      if (!bomb_used) {
        Vector3 pos = transform.position + transform.TransformDirection(Vector3.forward); // プレイヤー位置　+　プレイヤー正面にむけて１進んだ距離
        GameObject bom = Instantiate(prefab_bom , pos , Quaternion.identity) as GameObject; //ボム作成

        Vector3 bom_speed = transform.TransformDirection(Vector3.forward) * 5; // ボムの移動速度。『プレイヤー正面に向けての速度ベクトル』を５
        bom_speed += Vector3.up * 5; //ボムのz軸方向の速度を加算
        bom.GetComponent< Rigidbody >().velocity = bom_speed; //ボム速度を代入

        bom.GetComponent< Rigidbody >().angularVelocity = Vector3.forward * 7; //ボムの回転速度を代入

        c92_Sound.SendMessage("soundStart", bomb_throw_sound); // 音を鳴らす

        bomb_used = true;                 // 手榴弾を使用不可能にする
        c93_UI.changeTextBomb(bomb_used); // 手榴弾のテキスト変更
        StartCoroutine("reChargeBomb");   // ボムの連射制限コルーチンを開始(数秒後、ボムを再び使用可にする)
      }
    }

    //------------------------------
    // ボムの連射制限コルーチン
    //------------------------------
    IEnumerator reChargeBomb() {
      yield return new WaitForSeconds(2.5f);  // 2.5s処理を待機
      bomb_used = false;
      c93_UI.changeTextBomb(bomb_used);		// 手榴弾のテキスト変更
    }

    //------------------------------
    // 武器変更
    //------------------------------
    private void changeWeaponMode() {
      weapon.changeWeapon();
      c93_UI.changeWeaponTypeRawImage(weapon.getWeaponType());    // 武器画像を変更
      c93_UI.isChangeText(weapon.getWeaponType());                // 武器テキスト表示切替
    }

    //------------------------------
  	// 視点モードの切り替え
  	//------------------------------
    public void changeModeType(bool type) {
      moveType = type;
    }

    //------------------------------
    // 一人称視点
    //------------------------------
    private void playerCameraPosition_1stPerson() {
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

    //------------------------------
    // 三人称視点
    //------------------------------
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
