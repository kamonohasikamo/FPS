using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//====================================================
// 敵を生成するためのクラス
// C23_MapArrayを分割して、複数ファイルで記述
// partialを付けたので、クラス名とファイル名が違うが大丈夫
// あくまでもMapArrayFloorの続きとして取られる
//====================================================
public partial class MapArrayFloor {
	public GameObject[]	enemy;					// 敵格納用
	private GameObject	enemyFolder;		// 敵格納用フォルダー
	private int					maxEnemy	=	20;	// 敵の最大数

	//-----------------------------------------------------
	// setEnemyObject
	//-----------------------------------------------------
	public void setEnemy(GameObject[] enemyObject) {
		enemy	=	enemyObject;
	}

	//-----------------------------------------------------
	// 敵出現用関数
	//-----------------------------------------------------
	public void enemyArrival() {
		if (enemyFolder.transform.childCount >= maxEnemy) {
			return;	// フォルダー内に敵の数が最大数以上なら、以降は処理しない
		}

		C22_MapAxis.Axis_XZ positionAxis;						// 位置座標の開始地点用
		positionAxis.x	=	axis.getAxisMapStartX();	// 開始始点Xはマップの始点 （現在位置－半マップサイズ）
		positionAxis.z	=	axis.getAxisMapEndZ();		// Zはマップの終端 （現在位置＋半マップサイズ）

		if (enemy.Length != 0) {
			for (int x = 1; x < size.getMapSizeX() - 1; x++) {
				if (enemyFolder.transform.childCount >= maxEnemy) {
					return;
				}
				if (Random.Range(0, 100) <= 10) {
					createEnemyObject(x + positionAxis.x, positionAxis.z);
				}
			}
		}
	}

	//-----------------------------------------------------
	// createEnemyObject
	//-----------------------------------------------------
	private void createEnemyObject(C22_MapAxis.Axis_XZ arg) {
		createEnemyObject(arg.x, arg.z);
	}
	private void createEnemyObject(int x, int z) {
		if (enemy.Length == 0) {
			return;
		}
		int array_x	=	getArrayNumX(x);
		int array_z	=	getArrayNumZ(z);

		if (array[array_x, array_z] == null) {
			Vector3 scale					=	axis.getBlockSize();													// マップのブロックサイズを取得
			Vector3 position			=	new Vector3(scale.x * x, 0, scale.z * z);			// 位置の算出
			GameObject obj				= GameObject.Instantiate (enemy[Random.Range(0 , enemy.Length)], position, Quaternion.Euler(0,180,0)) as GameObject;		// プレハブ作成
			obj.transform.parent	= enemyFolder.transform;									// 作成したオブジェクトの親を、フォルダーにする
		}
	}
}
