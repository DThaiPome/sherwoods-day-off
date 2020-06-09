using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    static Vector2 loadPos = new Vector2(-101, 230);
    public List<Vector2> levelPos = new List<Vector2>
    {
        //Empty
        new Vector2(0, 0),

        //Thicc Thicket
        new Vector2(0, 1),

        //Acorn Pass
        new Vector2(-101, 230)
    };

    public static int nLevel;
    public static int level;

    GameObject p;
	void Awake () {
        level = SceneManager.GetActiveScene().buildIndex;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (level != 0)
        {
            p = GameObject.Find("Player");
            p.transform.position = new Vector3(loadPos.x, loadPos.y, p.transform.position.z);
        }
    }

    public void SetPos(Vector2 newPos)
    {
        loadPos = newPos;
    }

    public void LoadWPos(int sI, bool levelStart = false)
    {
        if (sI == 0)
        {
            LocalData.lD.ScanForCollectables();
            LocalData.lD.SaveData();
        }
        else if (levelStart)
        {
            SetPos(levelPos[sI]);
        }
        SceneManager.LoadScene(sI);
    }

}
