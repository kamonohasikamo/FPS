using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayer : MonoBehaviour {
	//先ほど作成したJoystick
	[SerializeField]
	private Joystick _joystick = null;

	//移動速度
	private const float SPEED = 0.1f;

	private void Update () {
	  Vector3 pos = transform.position;
    pos.x += _joystick.Position.x * SPEED;
    pos.z += _joystick.Position.y * SPEED;

    transform.position = pos;
  }
}
