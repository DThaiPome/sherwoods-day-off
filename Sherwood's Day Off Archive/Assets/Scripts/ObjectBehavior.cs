using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

    public int objectId;

    public List<int> miscInt;
    public List<float> miscFloat;
    public List<bool> miscBool;

    public List<GameObject> miscGameObject;

    GameObject player;

    bool oWP;
    bool oWPA = false;

    /*OBJECT IDS
     * 0 - Empty
     * 1 - Collectable
     * 2 - Spring
     * 3 - One Way Platform
     * 4 - Heart
     * 5 - Bird
     * 6 - Bonus Token
     * 7 - Retracting Branch
     * 8 - Leaf
     * 9 - No-Jump One Way Platform
     * 10 - Float Leaf
     */

	void Start () {
        player = GameObject.Find("Player");
        //Object Initiation
        switch (objectId)
        {
            case 1:
                oWP = false;
                int el = 0;
                foreach (GameObject c in GameObject.FindGameObjectsWithTag("collectable"))
                {
                    if (gameObject.name == "collectable (" + el.ToString() + ")")
                    {
                        miscInt[0] = el;
                    }
                    el++;
                }
                if (!LocalData.lD.collB[miscInt[0]])
                {
                    Destroy(gameObject);
                }
                break;
            case 2:
                oWP = false;
                miscGameObject[0] = transform.Find("hitCollider").gameObject;
                break;
            case 4:
                oWP = false;
                int hel = 0;
                foreach (GameObject h in GameObject.FindGameObjectsWithTag("heart"))
                {
                    if (gameObject.name == "heart (" + hel.ToString() + ")")
                    {
                        miscInt[0] = hel;
                    }
                    hel++;
                }
                if (!LocalData.lD.heartB[miscInt[0]])
                {
                    Destroy(gameObject);
                }

                miscFloat[2] = transform.position.y;

                break;
            case 5:
                oWP = false;
                int bel = 0;
                foreach (GameObject b in GameObject.FindGameObjectsWithTag("bird"))
                {
                    if (gameObject.name == "bird (" + bel.ToString() + ")")
                    {
                        miscInt[0] = bel;
                    }
                    bel++;
                }
                if (!LocalData.lD.birdB[miscInt[0]])
                {
                    LocalData.lD.bCounter++;
                    Destroy(gameObject);
                }

                miscFloat[0] = transform.position.x;
                miscFloat[1] = transform.position.y;
                break;
            case 6:
                oWP = false;
                int oel = 0;
                foreach (GameObject o in GameObject.FindGameObjectsWithTag("bonus"))
                {
                    if (gameObject.name == "Bonus Token (" + oel.ToString() + ")")
                    {
                        miscInt[0] = oel;
                    }
                    oel++;
                }
                if (!LocalData.lD.bonusB[miscInt[0]])
                {
                    LocalData.lD.bCounter++;
                    Destroy(gameObject);
                }
                miscFloat[2] = transform.position.y;
                break;
            case 7:
                oWP = true;
                break;
            case 8:
                oWP = true;
                miscFloat[2] = transform.position.x;
                miscFloat[3] = transform.position.y;
                break;
            case 9:
                oWP = true;
                miscGameObject[0] = GameObject.Find("Player");
                break;
            case 10:
                oWP = true;
                miscGameObject[0] = GameObject.Find("Player");
                miscFloat[4] = transform.position.x;

                miscFloat[3] = -(0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * (-((((miscFloat[0] - miscFloat[1]) - (transform.position.y - miscFloat[1])) / (miscFloat[0] - miscFloat[1])) * miscInt[1])) / (miscInt[1] * miscFloat[2]));

                if (miscFloat[3] >= 0 && miscFloat[3] < (0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[1]) / (miscInt[1] * miscFloat[2]))
                {
                    miscBool[0] = true;
                    transform.position = Vector3.Lerp(new Vector3(transform.position.x, miscFloat[0], transform.position.z), new Vector3(transform.position.x, miscFloat[1], transform.position.z), miscFloat[3] / ((0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[1]) / (miscInt[1] * miscFloat[2])));
                }
                break;
        }
	}

	void Update () {
		//Object Behavior
        switch (objectId)
        {
            case 1:
                //miscInt[0] = collectableId
                transform.eulerAngles = (new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (-100 * Time.deltaTime)));
                break;
            case 2:
                if (miscGameObject[0].GetComponent<playerFloorCollider>().check[0])
                {
                    player.GetComponent<PlayerMovement>().Jump(miscFloat[0], 10);
                }
                break;
            case 3:
                //miscBool[0] = playerAbove
                //miscFloat[0] = playerBoostHeight
                miscBool[0] = player.transform.position.y - 2 < transform.position.y;

                gameObject.GetComponent<BoxCollider2D>().isTrigger = miscBool[0];

                if (gameObject.GetComponent<playerFloorCollider>().check[0] && miscBool[0])
                {
                    player.GetComponent<PlayerMovement>().Jump(miscFloat[0], 0);
                }

                break;
            case 4:
                //miscInt[0] = heartId
                //miscFloat[0] = progress
                //miscFloat[1] = floatSpeed
                //miscFloat[2] = initialY
                transform.position = new Vector2(transform.position.x, miscFloat[2] + (0.5f * Mathf.Sin(miscFloat[0] * (Mathf.PI / 180))));
                miscFloat[0] += miscFloat[1] * Time.deltaTime;
                if (miscFloat[0] >= 360)
                {
                    miscFloat[0] = 360 - miscFloat[0];
                }
                break;
            case 5:
                //miscInt[0] = birdId
                //miscFloat[0] = originX
                //miscFloat[1] = originY
                if (player.GetComponent<PlayerMovement>().inBonus && player.GetComponent<PlayerMovement>().bonusCollectable < 100)
                {
                    transform.position = new Vector3(-1000000, -1000000, transform.position.z);
                } else
                {
                    transform.position = new Vector3(miscFloat[0], miscFloat[1], transform.position.z);
                }

                if (GetComponent<playerFloorCollider>().check[0])
                {
                    LocalData.lD.bCounter++;
                    if (player.GetComponent<PlayerMovement>().inBonus)
                    {
                        player.GetComponent<PlayerMovement>().bonusComplete = true;
                    }
                    Destroy(gameObject);
                }
                break;
            case 6:
                //miscInt[0] = heartId
                //miscFloat[0] = progress
                //miscFloat[1] = floatSpeed
                //miscFloat[2] = initialY
                //miscFloat[3] = bX
                //miscFloat[4] = bY
                transform.position = new Vector2(transform.position.x, miscFloat[2] + (0.5f * Mathf.Sin(miscFloat[0] * (Mathf.PI / 180))));
                miscFloat[0] += miscFloat[1] * Time.deltaTime;
                if (miscFloat[0] >= 360)
                {
                    miscFloat[0] = 360 - miscFloat[0];
                }
                break;
            case 8:
                //miscFloat[0] = breakDelay
                //miscFloat[1] = respawnDelay
                //miscFloat[2] = x1
                //miscFloat[3] = y1
                //miscFloat[4] = progress
                //miscBool[0] = playerTouch
                //miscBool[1] = breakStarted
                //miscBool[2] = broken
                //miscBool[3] = floorActive
                //miscGameObject[0] = player

                miscGameObject[0] = GameObject.Find("Player");
                miscBool[0] = transform.GetChild(1).gameObject.GetComponent<playerFloorCollider>().check[0];

                miscBool[3] = oWPA;

                if (miscBool[0] && !miscBool[3])
                {
                    if (!miscBool[1] && !miscBool[2])
                    {
                        miscFloat[4] = 0;
                    }
                    miscBool[1] = true;
                }

                if (miscBool[1])
                {
                    miscFloat[4] += Time.deltaTime;
                    if (miscFloat[4] >= miscFloat[0])
                    {
                        miscFloat[4] = 0;
                        miscBool[1] = false;
                        miscBool[2] = true;
                    }
                }

                if (miscBool[2])
                {
                    transform.position = new Vector3(-1000000, -1000000, transform.position.z);
                    miscFloat[4] += Time.deltaTime;
                    if (miscFloat[4] >= miscFloat[1])
                    {
                        miscFloat[4] = 0;
                        miscBool[2] = false;
                    }
                } else
                {
                    transform.position = new Vector3(miscFloat[2], miscFloat[3], transform.position.z);
                }

                if (miscBool[1])
                {
                    transform.position = new Vector3(miscFloat[2] + (Mathf.Cos(miscFloat[4] * 40) * 0.25f), transform.position.y, transform.position.z);
                }

                break;
            case 10:
                //miscInt[0] = schedulerId
                //miscInt[1] = maxScheduler
                //miscFloat[0] = y1
                //miscFloat[1] = y2
                //miscFloat[2] = speed
                //miscFloat[3] = progress
                //miscFloat[4] = originX
                //miscBool[0] = inProgress
                //miscBool[1] = parentDone
                //miscGameObject[1] = parent
                if (miscFloat[2] > 0)
                {
                    if (!miscBool[0])
                    {
                        miscBool[0] = true;
                        transform.position = new Vector3(transform.position.x, miscFloat[0], transform.position.z);
                    }
                    transform.position = new Vector3(miscFloat[4] + (Mathf.Cos(transform.position.y * 0.5f) * 1.5f), transform.position.y - (miscFloat[2] * Time.deltaTime));
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, 1000000, transform.position.z);
                }

                if (transform.position.y <= miscFloat[1])
                {
                    miscFloat[3] = 0;
                    miscBool[0] = false;
                }

                if (transform.GetChild(0).GetComponent<playerFloorCollider>().check[0] && !oWPA && miscFloat[3] != 0)
                {
                    miscBool[1] = false;
                    miscGameObject[0].transform.parent = gameObject.transform;
                }
                else if (!miscBool[1])
                {
                    miscBool[1] = true;
                    miscGameObject[0].transform.parent = null;
                }

                if (miscFloat[3] == 0 && !miscBool[1])
                {
                    miscBool[1] = true;
                    miscGameObject[0].transform.parent = null;
                }

                miscFloat[3] = Time.deltaTime;
                break;
        }

        //miscGameObject[0] MUST be the Player
        //and floor collider must be first child

        if (oWP)
        {
            //Make more flexible later
            if (miscGameObject[0].transform.position.y - 2.25f > transform.position.y + 0.5f)
            {
                oWPA = false;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
                gameObject.transform.GetChild(0).gameObject.tag = "floor";
            }
            else if (miscGameObject[0].transform.position.y < transform.position.y + 0.5f)
            {
                oWPA = true;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                gameObject.transform.GetChild(0).gameObject.tag = "Untagged";
            }
        }
	}

    private void FixedUpdate()
    {
        switch(objectId)
        {
            case 7:
                //miscFloat[0] = y1
                //miscFloat[1] = y2
                //miscBool[0] = occupied
                //miscBool[1] = playerOver
                //miscBool[2] = parentDone
                //miscBool[3] = playerBelow;
                //miscGameObject[0] = player;
                miscGameObject[0] = GameObject.Find("Player");

                miscBool[0] = transform.GetChild(1).gameObject.GetComponent<playerFloorCollider>().check[0];

                if (miscBool[0] && !oWPA)
                {
                    miscBool[1] = true;
                    miscBool[2] = false;
                    miscGameObject[0].transform.parent = gameObject.transform;
                    //if (miscGameObject[0].transform.position.y - 2 < transform.position.y + 0.5f)
                    //{
                    //    miscGameObject[0].transform.position = new Vector3(miscGameObject[0].transform.position.x, transform.position.y + 2.5f, miscGameObject[0].transform.position.z);
                    //}
                }
                else
                {
                    if (!miscBool[2])
                    {
                        miscBool[2] = true;
                        miscGameObject[0].transform.parent = null;
                    }
                }

                if (miscBool[1])
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, miscFloat[1], transform.position.z), 0.04f);
                    if (miscGameObject[0].transform.position.x > transform.position.x + 5 || miscGameObject[0].transform.position.x < transform.position.x - 5)
                    {
                        miscBool[1] = false;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, miscFloat[0], transform.position.z), 0.05f);
                }

                break;
        }
    }

}
