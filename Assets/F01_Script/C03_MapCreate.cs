using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===============================================
// map生成を扱うクラス
//===============================================
public class C03_MapCreate : MonoBehaviour {
	private int MAP_SIZE_X = 7;					// mapSize X
	private int MAP_SIZE_Z = 10;				// mapSize Z
	private GameObject player;					// playerObject
	private C21_MapSize size;						// mapSize 変数
	private C22_MapAxis playerAxis;			// playerAxis
	private MapArrayBlock mapBlock;			// mapBlock
	private MapArrayFloor mapFloor;			// mapFloor

	public GameObject[] prefab_Block;
	public GameObject[] prefab_WALL;

	void Start() {
		player		= GameObject.FindGameObjectWithTag("Player");	// プレイヤーオブジェクト格納

		size		= new C21_MapSize(MAP_SIZE_X, MAP_SIZE_Z);		// MapSizeクラスのインスタンス生成
		playerAxis	= new C22_MapAxis(player, size, prefab_Block[0].transform.localScale);	// PlayerAxisクラスのインスタンス生成
		mapBlock	= new MapArrayBlock(prefab_Block, "BLOCK", size, playerAxis);	// 地面用MapArrayクラスのインスタンス生成
		mapFloor	= new MapArrayFloor("FLoor", size, playerAxis);	// 地上用MapArrayクラスのインスタンス生成

		mapFloor.setWall(prefab_WALL);			// 壁オブジェクトを渡す
		mapFloor.setObstacle(prefab_WALL);	// 障害物用に壁オブジェクトを渡す

		initialize();			// プレイヤー位置／マップ初期化
	}

	//--------------------------------------
	//player position / player init
	//--------------------------------------
	private void initialize() {
		playerAxis.initialize();
		mapBlock.startMap_Create(); // 床生成
		mapFloor.startMap_Create(); // 地上生成
	}

	void Update() {
		playerAxis.updateAxis();
		mapBlock.renewal(); // 床更新
		mapFloor.renewal(); // 地上更新
	}

}
