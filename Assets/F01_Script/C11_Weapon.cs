using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C11_Weapon{
	private int weaponType = 0; //武器タイプ
	private int weaponNum  = 2; //武器の種類数

	//----------------
  //  武器変更処理
	//----------------
	public void changeWeapon() {
		weaponType = (weaponType + 1) % weaponNum;
		Debug.Log("現在の武器：" + weaponType);
	}

	//----------------
  //  武器タイプを返す
	//----------------
	public int getWeaponType() {
		return weaponType;
	}
}
