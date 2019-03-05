using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C03_MapCreate : MonoBehaviour {
	private struct BLOCK_Pos_XZ {	// BLOCKのポジションに関するint型の構造体
		public int x;
		public int z;
	}
	private BLOCK_Pos_XZ position_now;													// 現在の位置
	private BLOCK_Pos_XZ position_before;												// 前回の位置
	private GameObject		player;																// player
	public	GameObject[]	prefab_BLOCK;													// 床ブロックのぷれふぁぶ配列
	private	GameObject[,]	map_BLOCK;														// マップに配置したブロック
	private	int						map_size_x	= 10;											// マップ横Xサイズ
	private	int						map_size_z	= 10;											// マップ奥行きZサイズ
	private int						MAP_SIZE_X;
	private int						MAP_SIZE_Z;
	private int 					MAP_HARFSIZE_X;
	private int						MAP_HARFSIZE_Z;
	private	Vector3				BLOCK_SIZE	= new Vector3(4, 4, 4);		// ブロックの縦横奥行きサイズ
	private GameObject		block_folder;													// 作成したブロック格納用

	void Awake(){
		block_folder = new GameObject();				// 作成したブロックを入れるフォルダー(空オブジェクト)を作成
		block_folder.name = "BLOCK_Folder";						// 名前を変更.
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");	// playerObjectを探す

		mapSizeCrrection();	// mapSizeSetting
		setBlockInMap();	// 初期ブロック配置
		setPlayerInMap();	// PlayerStartSetting
	}

	// Update
	void Update () {
		renewalBLOCKInMap(getPlayerPosition());	// playerの位置座標の差分からブロック生成
	}

	//------------------------------
	// mapSizeSetting
	// 奇数ならそのまま、偶数なら+1をする
	//------------------------------
	void mapSizeCrrection() {
		MAP_SIZE_X = (map_size_x % 2 == 1) ? map_size_x : map_size_x + 1;
		MAP_SIZE_Z = (map_size_z % 2 == 1) ? map_size_z : map_size_z + 1;
		MAP_HARFSIZE_X = Mathf.FloorToInt(MAP_SIZE_X / 2);
		MAP_HARFSIZE_Z = Mathf.FloorToInt(MAP_SIZE_Z / 2);
	}

	//------------------------------
	// 初期ブロック配置
	//------------------------------
	void setBlockInMap () {
		map_BLOCK = new GameObject[MAP_SIZE_X, MAP_SIZE_Z];		// マップサイズ分の配列
		int blockXNum = 0;	// x列のブロックを順に配置するための変数
		int blockZNum = 0;	// z列のブロックを順に配置するための変数

		for(int z = 0 ; z < MAP_SIZE_Z ; z++){
			for(int x = 0 ; x < MAP_SIZE_X ; x++){
				Vector3 block_position = new Vector3(
					BLOCK_SIZE.x * x ,
					- BLOCK_SIZE.y / 2 ,
					BLOCK_SIZE.z * z);	// ブロック位置の算出
				GameObject block = Instantiate(
					prefab_BLOCK[blockXNum] ,
					block_position ,
					Quaternion.identity) as GameObject;		// プレハブ作成

				block.name = "BLOCK[" + x + "," + z + "]";
				block.transform.parent = block_folder.transform;

				map_BLOCK[x, z] = block;						// 作成したブロックを、マップ配列に格納

				blockXNum = (blockXNum + 1) % prefab_BLOCK.Length;				// 次に作るブロックの番号
			}
			blockZNum = (blockZNum + 1) % prefab_BLOCK.Length;					// Z列の最初に来るブロックの番号
			blockXNum = blockZNum;
		}
	}

	//------------------------------
	// 初期プレイヤー位置設定
	//------------------------------
	void setPlayerInMap() {
		position_now.x = MAP_HARFSIZE_X;
		position_now.z = MAP_HARFSIZE_Z;

		player.transform.position = new Vector3(	// playerの位置指定
			position_now.x * BLOCK_SIZE.x,
			player.transform.position.y,
			position_now.z * BLOCK_SIZE.z
		);
	}

	private void renewalBLOCKInMap(BLOCK_Pos_XZ change_pos) {
		// 列方向のブロック削除・作成
		if (change_pos.x != 0) {
			int newBLOCK_posX = position_now.x;
			newBLOCK_posX += (change_pos.x > 0) ? MAP_HARFSIZE_X : - MAP_HARFSIZE_X; // 新規ブロック列の位置 = (現在座標 ± マップ幅÷2)
			int x = newBLOCK_posX % MAP_SIZE_X; // 操作列 = (新規ブロック位置 % マップ幅)
			if (x < 0) { // 操作列が負の値の場合は、マップ幅を足して正値にする
				x += MAP_SIZE_X;
			}

			int newBLOCK_posZ = position_now.z - MAP_HARFSIZE_Z; // 新規ブロック行の初期位置 = (現在座標 - マップ幅÷2)
			int map_z = newBLOCK_posZ % MAP_SIZE_Z; // 配列の初期座標(行)の取得
			if ( map_z < 0) { // 負の場合は正値にする
				map_z += MAP_SIZE_Z;
			}

			int n = (position_now.x + position_now.z) % prefab_BLOCK.Length;	// 最初に作るブロックの番号算出.
			if (n < 0) {
				n += prefab_BLOCK.Length;
			}

			for (int z = 0; z < MAP_SIZE_Z; z++) {　//配列内全列に対し操作
				Destroy(map_BLOCK[x, (z + map_z) % MAP_SIZE_Z].gameObject); // 配列内ブロック削除

				Vector3 block_pos = new Vector3( // block位置の算出
					BLOCK_SIZE.x * newBLOCK_posX,
					- BLOCK_SIZE.y / 2,
					BLOCK_SIZE.z * (newBLOCK_posZ + z)
				);
				GameObject block = Instantiate(
					prefab_BLOCK[n],
					block_pos,
					Quaternion.identity
				) as GameObject;		// プレハブ作成

				block.name = "BLOCK[" + x + "," + (z + map_z) % MAP_SIZE_Z + "]";				// 作成したブロックの名前変更
				block.transform.parent = block_folder.transform;	// 作成したブロックの親、フォルダーにする
				map_BLOCK[x, (z + map_z) % MAP_SIZE_Z] = block;			// 作成したブロックを、マップ配列に格納

				n = (n + 1) % prefab_BLOCK.Length;
			}
		}

		// 行方向についてのブロック削除・生成
		if(change_pos.z != 0){
			int newBLOCK_posZ = position_now.z;
			newBLOCK_posZ += (change_pos.z > 0) ? MAP_HARFSIZE_Z : -MAP_HARFSIZE_Z;	// 新規ブロック行の位置 = (現在座標 ± マップ幅÷2)

			int z = newBLOCK_posZ % MAP_SIZE_Z;	// 操作列 = (新規ブロック位置 % マップ幅)
			if (z < 0) { // 操作列が負の値の場合は、マップ幅を足して正値にする
				z += MAP_SIZE_Z;
			}

			int newBLOCK_posX = position_now.x - MAP_HARFSIZE_X;	// 新規ブロック列の初期位置 = (現在座標 - マップ幅÷2)
			int map_x = newBLOCK_posX % MAP_SIZE_X;	// 配列の初期座標(列)の取得
			if (map_x < 0) { // 負の場合は正値にする
				map_x += MAP_SIZE_X;
			}

			int n = (position_now.x + position_now.z) % prefab_BLOCK.Length;	// 最初に作るブロックの番号算出.
			if (n < 0) {
				n += prefab_BLOCK.Length;
			}

			for (int x = 0 ; x < MAP_SIZE_X ; x++){	// 配列内全行に対し操作
				Destroy(map_BLOCK[(x + map_x) % MAP_SIZE_X, z].gameObject);	// 配列の中に入っているブロックを削除

				Vector3 block_pos = new Vector3(	// ブロック位置の算出
					BLOCK_SIZE.x * (newBLOCK_posX + x),
					-BLOCK_SIZE.y / 2,
					BLOCK_SIZE.z * newBLOCK_posZ
				);
				GameObject block = Instantiate(
					prefab_BLOCK[n],
					block_pos,
					Quaternion.identity
				) as GameObject;		// プレハブ作成

				block.name = "BLOCK[" + (x + map_x) % MAP_SIZE_X + "," + z + "]";// 作成したブロックの名前変更
				block.transform.parent = block_folder.transform;// 作成したブロックの親、フォルダーにする

				map_BLOCK[(x + map_x) % MAP_SIZE_X, z] = block;// 作成したブロックを、マップ配列に格納

				n = (n + 1) % prefab_BLOCK.Length;// 次に作るブロックの番号
			}
		}
	}

	//---------------------------------------------------
	// プレイヤー座標を取得し、前回座標との差分を返す
	//---------------------------------------------------
	private BLOCK_Pos_XZ getPlayerPosition() {
		BLOCK_Pos_XZ ret_pos;								// 差分用

		position_before = position_now;						// 前回座標を取得
		position_now.x = Mathf.FloorToInt((player.transform.position.x + BLOCK_SIZE.x/2) / BLOCK_SIZE.x);	// 現在座標Xを取得
		position_now.z = Mathf.FloorToInt((player.transform.position.z + BLOCK_SIZE.z/2) / BLOCK_SIZE.z);	// 現在座標Zを取得

		ret_pos.x = position_now.x - position_before.x;		// Xの座標差分を取得
		ret_pos.z = position_now.z - position_before.z;		// Zの座標差分を取得

		return ret_pos;			// 差分を返す.
	}
}
