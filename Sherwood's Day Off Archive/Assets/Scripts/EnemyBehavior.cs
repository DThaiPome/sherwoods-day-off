using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    public int enemyId;

    public List<int> miscInt;
    public List<float> miscFloat;
    public List<bool> miscBool;

    /*If the params are set respectively...
     * miscGameObject[0] = hitCollider
     * miscGameObject[1] = jumpCollider
     */
    public List<GameObject> miscGameObject;

    GameObject player;

    bool isAttackable;
    bool isJumpable;
    bool directDamage;

    /* ENEMY IDS:
     * 0 - Empty
     * 1 - Stationary Ground Wolf
     * 2 - Crawling Ground Wolf
     * 3 - Spike
     * 4 - Vertical Moving Owl
     * 5 - Horizontal Moving Owl
     * 6 - Circular Movement Owl
     * 7 - Bombing Owl
     * 8 - ^ Bomb
     * 9 - ^ Bomb Explosion
     * 10 - ^ Bomb Scheduler
     * 11 - Squirrel
     * 12 - ^ Nut
     * 13 - White Squirrel
     * 14 - ^ Spiky Nut
     * 15 - Beetle
     * */

	void Start () {
        player = GameObject.Find("Player");
		//Object Initiations
        switch (enemyId)
        {
            case 1:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;
                break;
            case 2:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;
                break;
            case 3:
                isAttackable = false;
                isJumpable = false;
                directDamage = true;
                break;
            case 4:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;
                break;
            case 5:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;
                break;
            case 6:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;
                miscFloat[2] = transform.position.x;
                miscFloat[3] = transform.position.y;
                break;
            case 7:
                isAttackable = false;
                isJumpable = false;
                directDamage = true;
                if (miscInt[1] != 0)
                {
                    miscFloat[3] = -(0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[1]) / (miscInt[2] * miscFloat[2]);
                } else
                {
                    miscFloat[3] = 0;
                }
                break;
            case 8:
                isAttackable = false;
                isJumpable = false;
                directDamage = true;
                break;
            case 9:
                isAttackable = false;
                isJumpable = false;
                directDamage = true;
                break;
            case 11:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;

                miscFloat[3] = transform.position.x + (2 * miscInt[0]);
                miscFloat[5] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).x;
                miscFloat[6] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).y;
                miscFloat[7] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).z;

                miscFloat[4] = miscFloat[3] - miscFloat[9];

                break;
            case 12:
                isAttackable = false;
                isJumpable = false;
                directDamage = false;
                break;
            case 13:
                isAttackable = true;
                isJumpable = true;
                directDamage = true;

                miscFloat[3] = transform.position.x + (2 * miscInt[0]);
                miscFloat[5] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).x;
                miscFloat[6] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).y;
                miscFloat[7] = QuadraticRegression(new Vector2(miscFloat[3], transform.position.y + 2), new Vector2(miscFloat[3] + (miscFloat[0] / 2), transform.position.y + 2 + miscFloat[1]), new Vector2(miscFloat[3] + miscFloat[0], transform.position.y + 2)).z;

                miscFloat[4] = miscFloat[3] - miscFloat[9];
                break;
            case 14:
                isAttackable = false;
                isJumpable = false;
                directDamage = false;
                break;
            case 15:
                isAttackable = false;
                isJumpable = false;
                directDamage = true;
                if (miscInt[1] != 0)
                {
                    miscFloat[5] = -(0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[1]) / (miscInt[2] * miscFloat[2]);
                }
                else
                {
                    miscFloat[5] = 0;
                }

                if (miscFloat[5] >= 0 && miscFloat[5] < (0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[2]) / (miscInt[2] * miscFloat[2]))
                {
                    miscBool[0] = true;
                    transform.position = Vector3.Lerp(new Vector3(miscFloat[0], transform.position.y, transform.position.z), new Vector3(miscFloat[1], transform.position.y, transform.position.z), miscFloat[5] / ((0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) * miscInt[2]) / (miscInt[2] * miscFloat[2])));
                }

                miscFloat[6] = -miscFloat[3];
                break;
        }
    }
	
	void Update () {
        //Object Behavior
        switch (enemyId)
        {
            case 1:
                break;
            case 2:
                float x1 = miscFloat[0];
                float x2 = miscFloat[1];
                float moveSpeed = miscFloat[2];

                float pos = transform.position.x;

                if (pos >= x2)
                {
                    miscInt[0] = -1;
                } else if (pos <= x1)
                {
                    miscInt[0] = 1;
                }

                int direction = miscInt[0];

                if (direction == 1)
                {
                    transform.position = new Vector2(pos + (moveSpeed * Time.deltaTime), transform.position.y);
                } else
                {
                    transform.position = new Vector2(pos - (moveSpeed * Time.deltaTime), transform.position.y);
                }
                break;
            case 4:
                float y1 = miscFloat[0];
                float y2 = miscFloat[1];
                float flySpeed = miscFloat[2];

                float posY = transform.position.y;

                if (posY >= y2)
                {
                    miscInt[0] = -1;
                }
                else if (posY <= y1)
                {
                    miscInt[0] = 1;
                }

                int dir = miscInt[0];

                if (dir == 1)
                {
                    transform.position = new Vector2(transform.position.x, posY + (flySpeed * Time.deltaTime));
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, posY - (flySpeed * Time.deltaTime));
                }
                break;
            case 5:
                float X1 = miscFloat[0];
                float X2 = miscFloat[1];
                float flSpeed = miscFloat[2];

                float posX = transform.position.x;

                if (posX >= X2)
                {
                    miscInt[0] = -1;
                }
                else if (posX <= X1)
                {
                    miscInt[0] = 1;
                }

                int di = miscInt[0];

                if (di == 1)
                {
                    transform.position = new Vector2(posX + (flSpeed * Time.deltaTime), transform.position.y);
                }
                else
                {
                    transform.position = new Vector2(posX - (flSpeed * Time.deltaTime), transform.position.y);
                }
                break;
            case 6:
                //miscFloat[0] = rad
                //miscFloat[1] = speed
                //miscFloat[2] = originX
                //miscFloat[3] = originY
                //miscFloat[4] = circleProgress (set to 0)
                //miscInt[0] = directon (1 = clockwise, -1 = cclockwise)
                transform.position = new Vector2(miscFloat[2] + (miscFloat[0] * Mathf.Cos(miscFloat[4] * (Mathf.PI / 180))), miscFloat[3] + (miscFloat[0] * Mathf.Sin(miscFloat[4] * (Mathf.PI / 180))));
                miscFloat[4] = miscFloat[4] + ((miscFloat[1] * Time.deltaTime) * -miscInt[0]);
                if (Mathf.Abs(miscFloat[4]) >= 360)
                {
                    miscFloat[4] = 0 + ((Mathf.Abs(miscFloat[4]) - 360) * -miscInt[0]);
                }
                break;
            case 7:
                //miscFloat[0] = x1
                //miscFloat[1] = x2
                //miscFloat[2] = speed
                //miscFloat[3] = progress
                //miscFloat[4] = frequency (1 = right, -1 = left) << DELETE
                //miscFloat[5] = bombProgress
                //miscFloat[6] = bombFrequency
                //miscFloat[7] = bombRange
                //miscInt[0] = direction
                //miscInt[1] = schedulerId
                //miscInt[2] = maxScheduler
                //miscBool[0] = droppedBomb; Set to false
                //miscBool[1] = inProgress; Set to false
                //miscGameObject[2] = emptyParent

                if (miscFloat[3] >= 0)
                {
                    if (!miscBool[1])
                    {
                        transform.position = new Vector2(miscFloat[0], transform.position.y);
                    }
                    miscBool[1] = true;
                    transform.position = new Vector2(transform.position.x + (miscFloat[2] * Time.deltaTime * miscInt[0]), transform.position.y);
                    if (transform.position.x <= player.transform.position.x + miscFloat[7] && transform.position.x >= player.transform.position.x - miscFloat[7])
                    {
                        if (miscFloat[5] >= miscFloat[6])
                        {
                            miscBool[0] = true;
                            miscFloat[5] = 0;
                        }
                    }
                } else
                {
                    transform.position = new Vector2(-1000000 * miscInt[0], transform.position.y);
                }
                miscFloat[3] = miscFloat[3] + Time.deltaTime;
                miscFloat[5] = miscFloat[5] + Time.deltaTime;
                if ((miscInt[0] == 1 && transform.position.x >= miscFloat[1]) || (miscInt[0] == -1 && transform.position.x <= miscFloat[1]))
                {
                    miscBool[0] = false;
                    miscBool[1] = false;
                    miscFloat[3] = 0;
                }

                bool hit = miscGameObject[0].GetComponent<playerFloorCollider>().check[1];
                if (hit)
                {
                    if (miscBool[1])
                    {
                        miscFloat[3] = -0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) / miscFloat[2] * (1 - ((Mathf.Abs(miscFloat[1] - miscFloat[0]) - Mathf.Abs(miscFloat[1] - transform.position.x)) / Mathf.Abs(miscFloat[1] - miscFloat[0])));
                    }
                    miscBool[0] = false;
                    miscBool[1] = false;
                }
                if (isJumpable)
                {
                    bool jumpedOn = miscGameObject[1].GetComponent<playerFloorCollider>().check[0];
                    if (jumpedOn)
                    {
                        player.GetComponent<PlayerMovement>().Jump(1.5f, 0);
                        miscBool[0] = false;
                        miscBool[1] = false;
                        miscFloat[3] = -0.99975576923f * (Mathf.Abs(miscFloat[1] - miscFloat[0])) / miscFloat[2] * ((Mathf.Abs(miscFloat[1] - miscFloat[0]) - Mathf.Abs(miscFloat[1] - transform.position.x)) / Mathf.Abs(miscFloat[1] - miscFloat[0]));
                    }
                }
                break;
            case 8:
                
                break;
            case 9:
                //miscGameObject[0] = self
                //miscGameObject[1] = pairedBomb
                //miscFloat[0] = limit
                //miscFloat[1] = limit
                //miscBool[0] = bombExploded
                //miscBool[1] = cannotExplode
                miscBool[0] = miscGameObject[1].GetComponent<EnemyBehavior>().miscBool[2];
                if (miscBool[0] && !miscBool[1])
                {
                    miscBool[1] = true;
                    transform.position = new Vector2(miscGameObject[1].transform.position.x, miscGameObject[1].transform.position.y + 1);
                    miscFloat[0] = 0;
                } else if (!miscBool[0])
                {
                    miscBool[1] = false;
                }
                if (miscFloat[0] >= miscFloat[1])
                {
                    transform.position = new Vector2(1000000000, 1000000000);
                }
                miscFloat[0] = miscFloat[0] + Time.deltaTime;
                break;
            case 11:

                //miscFloat[0] = targDistance
                //miscFloat[1] = peakY
                //miscFloat[2] = trueDistance;
                //miscFloat[3] = originX;
                //miscFloat[4] = progress;
                //miscFloat[5] = pathRegA;
                //miscFloat[6] = pathRegB;
                //miscFloat[7] = pathRegC;
                //miscFloat[8] = speed;
                //miscFloat[9] = delay;
                //miscInt[0] = direction;
                //miscBool[0] = nutShot;
                //miscBool[1] = nutRetracted;
                //miscGameObject[2] = nut;

                if ((miscInt[0] == 1 && (miscFloat[4] > miscFloat[3] && miscFloat[4] < miscFloat[3] + miscFloat[2])) || (miscInt[0] == -1 && (miscFloat[4] < miscFloat[3] && miscFloat[4] > miscFloat[3] + miscFloat[2])))
                {
                    miscBool[1] = false;
                    miscBool[0] = true;
                }
                else
                {
                    if (!miscBool[1])
                    {
                        miscBool[1] = true;
                        miscFloat[4] = miscFloat[3] - miscFloat[9];
                    }
                    miscBool[0] = false;
                }

                miscFloat[4] += Time.deltaTime * miscFloat[8];

                break;
            case 12:
                //miscBool[0] = nutShot
                //miscFloat[0] = progress
                //miscFloat[1] = A
                //miscFloat[2] = B
                //miscFloat[3] = C
                //miscGameObject[2] = parentSquirrel

                miscBool[0] = miscGameObject[2].GetComponent<EnemyBehavior>().miscBool[0];
                miscFloat[0] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4];
                miscFloat[1] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[5];
                miscFloat[2] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[6];
                miscFloat[3] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[7];

                if (miscBool[0])
                {
                    transform.position = new Vector3(miscFloat[0], (Mathf.Pow(miscFloat[0], 2) * miscFloat[1]) + (miscFloat[0] * miscFloat[2]) + miscFloat[3], transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(10000, 10000, transform.position.z);
                }

                if (gameObject.GetComponent<playerFloorCollider>().check[0])
                {
                    if (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] >= miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3])
                    {
                        //Debug.Log(miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                        miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[9] - (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                    }
                }

                if (miscGameObject[1].GetComponent<playerFloorCollider>().check[0])
                {
                    player.GetComponent<PlayerMovement>().Jump(1.5f, 0);
                    if (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] >= miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3])
                    {
                        //Debug.Log(miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                        miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[9] - (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                    }
                }

                if (miscGameObject[0].GetComponent<playerFloorCollider>().check[0])
                {
                    player.GetComponent<PlayerMovement>().Hurt(1);
                    if (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] >= miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3])
                    {
                        //Debug.Log(miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                        miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[9] - (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                    }
                }

                break;
            case 13:

                //miscFloat[0] = targDistance
                //miscFloat[1] = peakY
                //miscFloat[2] = trueDistance;
                //miscFloat[3] = originX;
                //miscFloat[4] = progress;
                //miscFloat[5] = pathRegA;
                //miscFloat[6] = pathRegB;
                //miscFloat[7] = pathRegC;
                //miscFloat[8] = speed;
                //miscFloat[9] = delay;
                //miscInt[0] = direction;
                //miscBool[0] = nutShot;
                //miscBool[1] = nutRetracted;
                //miscGameObject[2] = nut;

                if ((miscInt[0] == 1 && (miscFloat[4] > miscFloat[3] && miscFloat[4] < miscFloat[3] + miscFloat[2])) || (miscInt[0] == -1 && (miscFloat[4] < miscFloat[3] && miscFloat[4] > miscFloat[3] + miscFloat[2])))
                {
                    miscBool[1] = false;
                    miscBool[0] = true;
                }
                else
                {
                    if (!miscBool[1])
                    {
                        miscBool[1] = true;
                        miscFloat[4] = miscFloat[3] - miscFloat[9];
                    }
                    miscBool[0] = false;
                }

                miscFloat[4] += Time.deltaTime * miscFloat[8];
                if ((miscInt[0] == 1 && miscFloat[4] > miscFloat[3] + miscFloat[2]) || (miscInt[0] == -1 && miscFloat[4] < miscFloat[3] + miscFloat[2]))
                {
                    miscFloat[4] = miscFloat[3] + miscFloat[2] + (0.001f * miscInt[0]);
                }

                break;
            case 14:
                //miscBool[0] = nutShot
                //miscFloat[0] = progress
                //miscFloat[1] = A
                //miscFloat[2] = B
                //miscFloat[3] = C
                //miscGameObject[2] = parentSquirrel

                miscBool[0] = miscGameObject[2].GetComponent<EnemyBehavior>().miscBool[0];
                miscFloat[0] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4];
                miscFloat[1] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[5];
                miscFloat[2] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[6];
                miscFloat[3] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[7];

                if (miscBool[0])
                {
                    transform.position = new Vector3(miscFloat[0], (Mathf.Pow(miscFloat[0], 2) * miscFloat[1]) + (miscFloat[0] * miscFloat[2]) + miscFloat[3], transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(10000, 10000, transform.position.z);
                }

                if (miscGameObject[0].GetComponent<playerFloorCollider>().check[0])
                {
                    player.GetComponent<PlayerMovement>().Hurt(1);
                    if (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] >= miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3])
                    {
                        //Debug.Log(miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                        miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4] = miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[9] - (miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[3] + miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[2] - miscGameObject[2].GetComponent<EnemyBehavior>().miscFloat[4]);
                    }
                }

                break;
            case 15:
                //miscInt[0] = direction
                //miscInt[1] = schedulerID
                //miscInt[2] = maxScheduler
                //miscFloat[0] = originX
                //miscFloat[1] = finalX
                //miscFloat[2] = speed
                //miscFloat[3] = stepLength
                //miscFloat[4] = stepDelay
                //miscFloat[5] = majorProgress
                //miscFloat[6] = minorProgress
                //miscBool[0] = inProgress
                //miscBool[1] = parentDone
                //miscGameObject[1] = player

                miscGameObject[1] = GameObject.Find("Player");

                //Start sequence only when major progress is over 0
                if (miscFloat[5] >= 0)
                {
                    //Move beetle to initial location
                    if (!miscBool[0])
                    {
                        transform.position = new Vector3(miscFloat[0], transform.position.y);
                        miscBool[0] = true;
                    }
                    //Move before pausing...
                    if (miscFloat[6] < 0)
                    {
                        transform.position = new Vector3(transform.position.x + (miscFloat[2] * miscInt[0] * Time.deltaTime), transform.position.y);
                    }

                    //Reset at end of track
                    if ((miscInt[0] == 1 && transform.position.x >= miscFloat[1]) || (miscInt[0] == -1 && transform.position.x <= miscFloat[1]))
                    {
                        miscBool[0] = false;
                        miscFloat[5] = 0;
                    }
                } else
                {
                    transform.position = new Vector3(100000 * miscInt[0], transform.position.y);
                }

                //Reset minor progress once stepDelay is elapsed
                if (miscFloat[6] >= miscFloat[4])
                {
                    miscFloat[6] = -miscFloat[3];
                }

                if (miscFloat[6] < 0)
                {
                    miscFloat[5] += Time.deltaTime;
                }
                miscFloat[6] += Time.deltaTime;

                if (transform.GetChild(0).GetChild(0).GetComponent<playerFloorCollider>().check[0])
                {
                    miscBool[1] = false;
                    miscGameObject[1].transform.parent = gameObject.transform;
                } else if (!miscBool[1])
                {
                    miscBool[1] = true;
                    miscGameObject[1].transform.parent = null;
                }

                break;
        }

        if (isAttackable)
        {
            bool hit = miscGameObject[0].GetComponent<playerFloorCollider>().check[1];
            if (hit)
            {
                Destroy(gameObject);
            }
        }
        if (isJumpable)
        {
            bool jumpedOn = miscGameObject[1].GetComponent<playerFloorCollider>().check[0];
            if (jumpedOn)
            {
                player.GetComponent<PlayerMovement>().Jump(1.5f, 0);
                Destroy(gameObject);
            }
        }
        if (directDamage)
        {
            if (miscGameObject[0].GetComponent<playerFloorCollider>().check[0])
            {
                player.GetComponent<PlayerMovement>().Hurt(1);
            }
        }
	}
    void FixedUpdate()
    {
        switch (enemyId)
        {
            case 8:
                //miscGameObject[0] = parentOwl (leave empty)
                //miscBool[0] = active
                //miscBool[1] = activeBuffer
                //miscBool[2] = exploded

                Rigidbody2D rb0 = gameObject.GetComponent<Rigidbody2D>();

                miscGameObject[0] = gameObject;
                miscGameObject[1] = transform.parent.gameObject;
                if (!miscGameObject[1].GetComponent<EnemyBehavior>().miscBool[0])
                {
                    miscBool[2] = false;
                }
                if (!miscBool[2])
                {
                    miscBool[0] = miscGameObject[1].GetComponent<EnemyBehavior>().miscBool[0];
                }
                else
                {
                    miscBool[0] = false;
                }
                gameObject.GetComponent<Rigidbody2D>().isKinematic = !miscBool[0];
                if (!miscBool[1] && miscBool[0])
                {
                    miscBool[2] = false;
                    transform.position = new Vector2(miscGameObject[1].transform.position.x, miscGameObject[1].transform.position.y - 2);
                    miscBool[1] = true;
                }
                else if (!miscBool[0])
                {
                    miscBool[1] = false;
                    transform.position = new Vector2(-1000000, -1000000);
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }
                playerFloorCollider a = gameObject.GetComponent<playerFloorCollider>();
                if (a.check[0] || a.check[1] || a.check[2])
                {
                    miscBool[2] = true;
                    miscGameObject[1].GetComponent<EnemyBehavior>().miscBool[0] = false;
                }

                if (rb0.velocity.y <= -40)
                {
                    rb0.velocity = new Vector2(rb0.velocity.x, -40);
                }

                break;
        }
    }

    public Vector3 QuadraticRegression (Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        Vector3 result = new Vector3(0, 0, 0);
        float x1 = pointA.x;
        float x2 = pointB.x;
        float x3 = pointC.x;
        float y1 = pointA.y;
        float y2 = pointB.y;
        float y3 = pointC.y;

        /*Matrix A;
         * 
         *  a11 a12 a13
         * 
         *  a21 a22 a23
         * 
         *  a31 a32 a33
         *  */
        float a11 = Mathf.Pow(x1, 2);
        float a12 = x1;
        float a13 = 1;
        float a21 = Mathf.Pow(x2, 2);
        float a22 = x2;
        float a23 = 1;
        float a31 = Mathf.Pow(x3, 2);
        float a32 = x3;
        float a33 = 1;

        //Matrix B;
        float b11 = y1;
        float b21 = y2;
        float b31 = y3;

        //Determinant of A
        float detA = (a11 * ((a22 * a33) - (a23 * a32))) - (a12 * ((a21 * a33) - (a23 * a31))) + (a13 * ((a21 * a32) - (a22 * a31)));

        /*Transposed Matrix;
         * 
         *  aM11 aM12 aM13          a11 a21 a31
         * 
         *  aM21 aM22 aM23          a12 a22 a32
         * 
         *  aM31 aM32 aM33          a13 a23 a33
         *  */
        float aM11 = a11;
        float aM12 = a21;
        float aM13 = a31;
        float aM21 = a12;
        float aM22 = a22;
        float aM23 = a32;
        float aM31 = a13;
        float aM32 = a23;
        float aM33 = a33;

        /*Minor Determinants;
         * 
         *  aD11 aD12 aD13
         * 
         *  aD21 aD22 aD23
         * 
         *  aD31 aD32 aD33
         *  */
        float aD11 = ((aM22 * aM33) - (aM23 * aM32)) / detA;
        float aD12 = -((aM21 * aM33) - (aM23 * aM31)) / detA;
        float aD13 = ((aM21 * aM32) - (aM22 * aM31)) / detA;
        float aD21 = -((aM12 * aM33) - (aM13 * aM32)) / detA;
        float aD22 = ((aM11 * aM33) - (aM13 * aM31)) / detA;
        float aD23 = -((aM11 * aM32) - (aM12 * aM31)) / detA;
        float aD31 = ((aM12 * aM23) - (aM13 * aM22)) / detA;
        float aD32 = -((aM11 * aM23) - (aM13 * aM21)) / detA;
        float aD33 = ((aM11 * aM22) - (aM12 * aM21)) / detA;

        //Final Matrix

        float res11 = (aD11 * b11) + (aD12 * b21) + (aD13 * b31);
        float res21 = (aD21 * b11) + (aD22 * b21) + (aD23 * b31);
        float res31 = (aD31 * b11) + (aD32 * b21) + (aD33 * b31);

        result = new Vector3(res11, res21, res31);

        return result;
    }

}