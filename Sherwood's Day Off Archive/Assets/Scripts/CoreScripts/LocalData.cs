using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalData : MonoBehaviour {

    public int ID;

    public int level = 9000;

    //Collectables
    public List<bool> collB;
    public int cCounter = 0;
    public GameObject cCounterText;

    //Health
    public List<bool> heartB;
    public int hCounter = 2;
    public GameObject hCounterText;

    //Lives
    public int lCounter = 4;
    public GameObject lCounterText;

    //Bird
    public List<bool> birdB;
    public int bCounter;
    public GameObject bCounterText;

    //Bonus
    public List<bool> bonusB;
    public int bClock;
    public GameObject bClockText;

    public static LocalData lD;

    public static bool loaded = false;

    GameObject p;

    SceneLoader sLd;

    //UNIVERSAL BIRD COUNT

    public List<List<bool>> cBirds = new List<List<bool>>
    {
        //Map (empty)
        new List<bool>
        {

        },

        //Thicc Thicket (7) 
        new List<bool>{
            true,
            true,
            true,
            true,
            true,
            true,
            true
        },

        //Acorn Pass
        new List<bool>
        {
            true,
            true
        }
    };

    void Awake ()
    {
        if (lD == null)
        {
            DontDestroyOnLoad(gameObject);
            lD = this;
        }
        else if (lD != this)
        {
            Destroy(gameObject);
        }

        //Initialize Lists
        collB = new List<bool>();
        birdB = new List<bool>();
        heartB = new List<bool>();
        bonusB = new List<bool>();
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
        InitializeLevel();
    }

    void Update () {
        if (level != 0)
        {
            p = GameObject.Find("Player");

            cCounterText = GameObject.Find("Collectable Counter");
            if (!p.GetComponent<PlayerMovement>().inBonus)
            {
                cCounterText.GetComponent<Text>().text = cCounter.ToString();
            }
            else
            {
                cCounterText.GetComponent<Text>().text = p.GetComponent<PlayerMovement>().bonusCollectable.ToString() + "/100";
            }

            hCounterText = GameObject.Find("Health Counter");
            if (!p.GetComponent<PlayerMovement>().inBonus)
            {
                hCounterText.GetComponent<Text>().text = hCounter.ToString();
            }
            else
            {
                hCounterText.GetComponent<Text>().text = p.GetComponent<PlayerMovement>().bonusHealth.ToString();
            }
            hCounterText.GetComponent<Text>().text = hCounter.ToString();

            bCounterText = GameObject.Find("Bird Counter");
            bCounterText.GetComponent<Text>().text = bCounter.ToString() + "/" + birdB.Count.ToString();

            bClockText = GameObject.Find("Bonus Clock");
            if (p.GetComponent<PlayerMovement>().inBonus)
            {
                bClockText.GetComponent<Text>().text = bClock.ToString();
            }
            else
            {
                bClockText.GetComponent<Text>().text = "";
            }

            lCounterText = GameObject.Find("Life Counter");
            lCounterText.GetComponent<Text>().text = lCounter.ToString();
        }
    }

    public void ScanForCollectables()
    {
        for (int i = 0; i < collB.Count; i++)
        {
            if (GameObject.Find("collectable (" + i.ToString() + ")") == null)
            {
                collB[i] = false;
            }
        }
        for (int i = 0; i < heartB.Count; i++)
        {
            if (GameObject.Find("heart (" + i.ToString() + ")") == null)
            {
                heartB[i] = false;
            }
        }
        for (int i = 0; i < birdB.Count; i++)
        {
            if (GameObject.Find("bird (" + i.ToString() + ")") == null)
            {
                birdB[i] = false;
            }
        }
        for (int i = 0; i < bonusB.Count; i++)
        {
            if (GameObject.Find("Bonus Token (" + i.ToString() + ")") == null)
            {
                bonusB[i] = false;
            }
        }
    }

    public void InitializeLevel()
    {
        sLd = gameObject.GetComponent<SceneLoader>();
        if (level != SceneLoader.level)
        {
            level = SceneLoader.level;
            loaded = false;
        }

        bClock = 0;

        if (!loaded && level != 0)
        {
            loaded = true;
            bCounter = 0;
            cCounter = 0;
            collB.Clear();
            birdB.Clear();
            heartB.Clear();
            bonusB.Clear();
            foreach (GameObject c in GameObject.FindGameObjectsWithTag("collectable"))
            {
                collB.Add(true);
            }
            foreach (GameObject h in GameObject.FindGameObjectsWithTag("heart"))
            {
                heartB.Add(true);
            }
            for (int i = 0; i < cBirds[level].Count; i++)
            {
                birdB.Add(cBirds[level][i]);
            }
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("bonus"))
            {
                bonusB.Add(true);
            }
        }
    }

    public void GameOver()
    {
        lCounter = 4;
        cCounter = 0;
        /*
        for(int i = 0; i < collB.Count; i++)
        {
            collB[i] = true;
        }
        for (int i = 0; i < heartB.Count; i++)
        {
            heartB[i] = true;
        }
        for (int i = 0; i < birdB.Count; i++)
        {
            birdB[i] = true;
        }
        for (int i = 0; i < bonusB.Count; i++)
        {
            bonusB[i] = true;
        }*/
        loaded = false;
        sLd.LoadWPos(level, true);
    }

    public void SaveData()
    {
        //Update Collected Birds
        for (int i = 0; i < cBirds[level].Count; i++)
        {
            cBirds[level][i] = birdB[i];
        }

        //Save Data to file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream save;
        if (File.Exists(Application.persistentDataPath + "/Saves/testSave.dat"))
        {
            save = File.Open(Application.persistentDataPath + "/Saves/testSave.dat", FileMode.Open);
        } else
        {
            save = File.Create(Application.persistentDataPath + "/Saves/testSave.dat");
        }

        SaveFile s = new SaveFile();
        s.lives = lCounter;
        for (int i = 0; i < s.collectedBirds.Count; i++)
        {
            for (int j = 0; j < s.collectedBirds[i].Count; j++)
            {
                s.collectedBirds[i][j] = cBirds[i][j];
            }
        }

        bf.Serialize(save, s);
        save.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Saves/testSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream save = File.Open(Application.persistentDataPath + "/Saves/testSave.dat", FileMode.Open);
            SaveFile s = (SaveFile)bf.Deserialize(save);
            save.Close();

            lCounter = s.lives;
            for (int i = 0; i < s.collectedBirds.Count; i++)
            {
                for (int j = 0; j < s.collectedBirds[i].Count; j++)
                {
                    cBirds[i][j] = s.collectedBirds[i][j];
                    Debug.Log(cBirds[i][j]);
                }
            }
        }
    }

    public void ResetData()
    {
        for (int i = 0; i < cBirds[level].Count; i++)
        {
            cBirds[level][i] = true;
        }
        if (File.Exists(Application.persistentDataPath + "/Saves/testSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream save = File.Open(Application.persistentDataPath + "/Saves/testSave.dat", FileMode.Open);
            SaveFile s = new SaveFile();

            //s.lives = 4;
            /*for (int i = 0; i < s.collectedBirds.Count; i++)
            {
                for (int j = 0; j < s.collectedBirds[i].Count; j++)
                {
                    s.collectedBirds[i][j] = true;
                }
            }*/

            bf.Serialize(save, s);
            save.Close();
        }
    }

    [Serializable]
    class SaveFile
    {
        public int lives;

        //UNIVERSAL BIRD COUNT

        public List<List<bool>> collectedBirds = new List<List<bool>>
        {
            //Map (empty)
            new List<bool>
            {

            },

            //Thicc Thicket (7) 
            new List<bool>{
                true,
                true,
                true,
                true,
                true,
                true,
                true
                    },

        //Acorn Pass
        new List<bool>
        {
            true,
            true
        }
        };
    }

}
