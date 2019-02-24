using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C92_Sound : MonoBehaviour {
	private AudioSource audioSourceComponent;	//AudioSourceコンポーネント格納用変数
	public AudioClip[] sound;									//効果音格納用変数　複数使いたいので配列

	// Use this for initialization
	void Start () {
		audioSourceComponent = gameObject.AddComponent<AudioSource>();	// AudioSorceコンポーネントを追加し、変数に代入
		audioSourceComponent.loop = false;	//ループなし
	}

  private void soundStart(int value) {
		// 指定された番号が負、またはsound配列に格納されている数―1より大きい、または配列の中身が空ならreturn
		if ( (value < 0) || (value > sound.Length - 1) ) {
			return;
		}
		if (sound[value] == null) {
			return;
		}
		audioSourceComponent.PlayOneShot(sound[value]); // 1回だけ音を再生
	}
}
