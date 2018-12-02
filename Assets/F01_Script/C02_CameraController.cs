using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C02_CameraController : MonoBehaviour {
    private C01_PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = GameObject.FindWithTag("Player").GetComponent<C01_PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            changeCameraMove();
        }
	}

    private void changeCameraMove()
    {
        if(transform.parent == null)
        {
            transform.parent        = playerController.transform;
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.parent        = null;
            transform.position      = new Vector3(0, 4, -10);
        }
    }
}
