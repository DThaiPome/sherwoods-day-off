using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour {

    GameObject mCamera;

	void Start () {
        mCamera = GameObject.Find("Main Camera");
	}
	
	void Update () {
        transform.position = mCamera.GetComponent<CameraBehavior>().posTarg;
	}
}
