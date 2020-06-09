using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour {

    SceneLoader sL;

	void Start () {
        sL = GameObject.Find("CoreScripts").GetComponent<SceneLoader>();
    }
	
	void Update () {
		if (Input.GetAxis("Map") != 0)
        {
            LoadLevel(0);
        }
	}

    public void LoadLevel(int level)
    {
        sL.SetPos(sL.levelPos[level]);
        sL.LoadWPos(level, true);
    }

    public void ResetProgress()
    {
        LocalData.lD.ResetData();
    }

    public void LoadData()
    {
        LocalData.lD.LoadData();
    }

}
