using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C03_MapCreate : MonoBehaviour {
	public	GameObject[]	prefab_BLOCK;													// 床ブロックのぷれふぁぶ配列
	private	GameObject[,]	map_BLOCK;														// マップに配置したブロック
	private	int						MAP_SIZE_X	= 10;											// マップ横Xサイズ
	private	int						MAP_SIZE_Z	= 10;											// マップ奥行きZサイズ
	private	Vector3				BLOCK_SIZE	= new Vector3(4, 4, 4);		// ブロックの縦横奥行きサイズ
	private GameObject		block_folder;													// 作成したブロック格納用

	//------------------------------------------
	// Start()関数よりも先に実行される初期化関数
	//------------------------------------------
	void Awake () {
		block_folder = new GameObject();
		block_folder.name = "BLOCK_Folder";
	}
	// Use this for initialization
	void Start () {
		setBlockInMap();	// 初期ブロック配置
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

				map_BLOCK[x,z] = block;						// 作成したブロックを、マップ配列に格納

				blockXNum = (blockXNum + 1) % prefab_BLOCK.Length;				// 次に作るブロックの番号
			}
			blockZNum = (blockZNum + 1) % prefab_BLOCK.Length;					// Z列の最初に来るブロックの番号
			blockXNum = blockZNum;
		}
	}
}
