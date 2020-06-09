using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public Rigidbody2D rb;
    public GameObject aO;

    float horizontalInput;
    float jumpInput;
    float jumpMultiplier;
    int jumpTick;
    bool canJump;
    int multiJump;

    float resetInput;

    float attackInput;
    bool canAttack;
    bool canPress;

    playerFloorCollider pFC;
    playerFloorCollider hC;

    public float movementSpeed;
    bool sprinting;
    public float jumpSpeed;
    public float maxJumpSpeed;
    public float jumpLength;
    public float leadFeet;
    public float maxFallSpeed;
    public int initialJumpLength;

    int trueJumpLength;

    bool onGround = false;
    bool inJump;
    float pAngle;

    int direction;

    public GameObject mCamera;

    public bool inBonus;
    public int bonusHealth;
    public bool bonusComplete;
    public int bonusCollectable;

    bool canHurt;

    Vector2 sPos;

    SceneLoader sL;

    int level;

	void Start () {
        pAngle = 0;

        sprinting = false;

        sL = GameObject.Find("CoreScripts").GetComponent<SceneLoader>();
        /*sPos = sL.shPos;
        transform.position = sPos;*/
        
        pFC = transform.Find("floorCollider").gameObject.GetComponent<playerFloorCollider>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        direction = 1;

        hC = transform.Find("hitCollider").gameObject.GetComponent<playerFloorCollider>();

        jumpMultiplier = 1;
        jumpTick = 0;
        canJump = true;
        multiJump = 2;
        inJump = false;

        aO.GetComponent<PlayerAttack>().SetPos(new Vector2(1000000.0f, 1000000.0f));
        canAttack = true;
        canPress = true;

        LocalData.lD.hCounter = 2;
        canHurt = true;

        mCamera = GameObject.Find("Main Camera");

        inBonus = false;
        bonusHealth = 2;
        bonusComplete = false;
    }
	
    void FixedUpdate ()
    {
        float jump = jumpSpeed;
        float trueJumpSpeed = maxJumpSpeed * jumpMultiplier;
        trueJumpLength = initialJumpLength * Convert.ToInt32(Mathf.Floor(jumpMultiplier));
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetAxis("Jump");
        attackInput = Input.GetAxis("Attack");

        if (trueJumpLength < initialJumpLength)
        {
            trueJumpLength = initialJumpLength;
        }

        if (trueJumpSpeed > maxJumpSpeed * 3)
        {
            trueJumpSpeed = maxJumpSpeed * 3;
        }

        //This is to allow for the direct modification of specific elements of the velocity without modifying the entire vector at once
        Vector2 movement = rb.velocity;

        //Movement
        if (horizontalInput != 0)
        {
            direction = Convert.ToInt32(Mathf.Sign(horizontalInput));
            float h;
            if (sprinting)
            {
                h = horizontalInput * movementSpeed * Time.deltaTime;
            } else
            {
                h = (horizontalInput * movementSpeed * Time.deltaTime) / 1.5f;
            }
            if (!onGround)
            {
                movement.x = h;
            } else
            {
                movement.x = h * Mathf.Cos(pAngle * (Mathf.PI / 180));
                movement.y = h * Mathf.Sin(pAngle * (Mathf.PI / 180));
            }
            /*//Stick to ramps
            if (onGround)
            {
                movement.y = -7.5f;
            }*/
        }
        else
        {
            //If the object isn't moving, stop it
            movement.x = 0;
            if (onGround)
            {
                movement.y = 0;
            }
        }

        //Jumping
        if (jumpInput != 0)
        {
            if (onGround && canJump)
            {
                inJump = true;
                movement.y = jump * (Mathf.Pow(jumpMultiplier, 0.125f));
                onGround = false;
                canJump = false;
            }
            else if (canJump)
            {
                switch (multiJump)
                {
                    case 2:
                        inJump = true;
                        movement.y = jump * 0.75f;
                        jumpMultiplier = 0.75f;
                        jumpTick = 0;
                        canJump = false;
                        multiJump--;
                        break;
                    case 1:
                        inJump = true;
                        movement.y = jump * 0.25f;
                        jumpMultiplier = 0.25f;
                        jumpTick = 7;
                        canJump = false;
                        multiJump--;
                        break;
                }
            }
        }

        if (jumpInput != 0 || jumpTick <= trueJumpLength)
        {
            jumpTick++;
        }

        if (jumpInput == 0)
        {
            canJump = true;
        }

        //This is only active while the jump is in progress
        if (inJump)
        {
            //If the jump key is still pressed or if there is still time left in the extended jump
            if (jumpInput != 0 && jumpTick <= trueJumpLength + jumpLength || jumpTick <= trueJumpLength)
            {
                canJump = false;
                movement.y = jump * (Mathf.Pow(jumpMultiplier, 0.125f));
            }
            else
            {
                inJump = false;
                movement.y = 8 * jumpMultiplier;
            }
        }

        //This is just to give a slight "lead feet" effect. Not sure if it actually has any impact on the jump but whatever.
        if (movement.y < 0)
        {
            inJump = false;
            movement.y = movement.y * (1 + leadFeet);
        }

        if (movement.y < -maxFallSpeed)
        {
            movement.y = -maxFallSpeed;
        }

        //Sets a cap for how high/fast the player can jump (prevents chaotic variances in jump height) (although I'm pretty sure removing Time.deltaTime from the jump script fixed that anyway)
        if (movement.y > trueJumpSpeed)
        {
            movement.y = trueJumpSpeed;
        }

        if (movement.x == 0 && onGround)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        rb.velocity = movement;
        onGround = pFC.check[0];

        //Platform Angle
        if (onGround)
        {
            Transform pForm = pFC.collidedObject[0].transform.parent;
            pAngle = pForm.rotation.eulerAngles.z;
        }
        Debug.Log(onGround);
    }

	void Update () {
        level = LocalData.lD.level;
        //Sprinting (only updates when on ground)
        if (onGround)
        {
            sprinting = Input.GetAxis("Sprint") != 0;
        }

        //Attack
        if (attackInput != 0 && canAttack && canPress)
        {
            canPress = false;
            StartCoroutine(Attack());
        }

        if (attackInput == 0)
        {
            canPress = true;
        }

        //Collectable

        if (hC.check[0])
        {
            Destroy(hC.collidedObject[0]);
            if (!inBonus)
            {
                LocalData.lD.cCounter++;
            } else
            {
                bonusCollectable++;
            }
        }

        if (LocalData.lD.cCounter >= 100)
        {
            LocalData.lD.cCounter = 0;
            LocalData.lD.lCounter++;
        }
        
        //Hearts

        if (hC.check[3])
        {
            Destroy(hC.collidedObject[3]);
            //hp++;
            if (LocalData.lD.hCounter < 2)
            {
                LocalData.lD.hCounter++;
            } else
            {
                LocalData.lD.lCounter++;
            }
        }

        /*resetInput = Input.GetAxis("Reset");

        if (resetInput != 0)
        {

            sL.SetPos(new Vector2(0, 1));
            sL.LoadWPos(0);
        }*/

        if (LocalData.lD.hCounter <= 0)
        {
            LocalData.lD.bCounter = 0;
            if (LocalData.lD.lCounter > 0)
            {
                sL.LoadWPos(level);
                LocalData.lD.lCounter--;
            }
            else
            {
                LocalData.lD.GameOver();
            }
        }

        if (Input.GetAxis("Reset") != 0)
        {
            LocalData.lD.GameOver();
        }

        //hpCounter.GetComponent<Text>().text = hp.ToString();
        switch (canHurt)
        {
            case true:
                LocalData.lD.hCounterText.GetComponent<Text>().color = Color.green;
                break;
            case false:
                LocalData.lD.hCounterText.GetComponent<Text>().color = Color.red;
                break;
        }


        //Die if fall
        if (hC.check[1])
        {
            if (!inBonus)
            {
                LocalData.lD.hCounter = 0;
            } else
            {
                bonusHealth = 0;
            }
        }

        if (LocalData.lD.hCounter > 2)
        {
            LocalData.lD.hCounter = 2;
        }

        //Checkpoint
        if (hC.check[2])
        {
            LocalData.lD.ScanForCollectables();
            sL.SetPos(hC.collidedObject[2].transform.position);
            Destroy(hC.collidedObject[2]);
        }

        //Camera
        if (hC.check[4])
        {
            CameraColliderData cC = hC.collidedObject[4].GetComponent<CameraColliderData>();
            mCamera.GetComponent<CameraBehavior>().SetCamera(cC.cameraMode, cC.xTarg, cC.yTarg, cC.xThresh.x, cC.xThresh.y, cC.yThresh.x, cC.yThresh.y);
        }

        //Bonus Round
        if (hC.check[5] && !inBonus)
        {
            StartCoroutine(BonusRound(hC.collidedObject[5]));
        }

    }

    IEnumerator Attack ()
    {
        canAttack = false;
        float tD = direction;
        for (int i = 0; i < 10; i++)
        {
            aO.GetComponent<PlayerAttack>().SetPos(new Vector2(transform.position.x + (1.75f * tD), transform.position.y));
            yield return new WaitForFixedUpdate();
        }
        aO.GetComponent<PlayerAttack>().SetPos(new Vector2(1000000.0f, 1000000.0f));
        yield return new WaitForSeconds(0.3f);
        canAttack = true;
    }

    IEnumerator BonusRound(GameObject bToken)
    {
        inBonus = true;
        bool bDone = false;
        Vector3 returnPos = new Vector3(bToken.transform.position.x, bToken.transform.position.y, transform.position.z);
        Vector3 startPos = new Vector3(bToken.GetComponent<ObjectBehavior>().miscFloat[3], bToken.GetComponent<ObjectBehavior>().miscFloat[4], transform.position.z);
        Destroy(bToken);
        bonusCollectable = 0;
        bonusHealth = LocalData.lD.hCounter;
        transform.position = startPos;
        StartCoroutine(BonusCountdown(60));
        while (!bDone)
        {
            if (!(LocalData.lD.bClock > 0 && bonusHealth > 0 && !bonusComplete))
            {
                bDone = true;
            }
            yield return null;
        }
        if (bonusComplete)
        {
            bonusComplete = false;
        }
        if (bonusHealth == 0)
        {
            LocalData.lD.hCounter = 1;
        } else
        {
            LocalData.lD.hCounter = bonusHealth;
        }
        LocalData.lD.cCounter += bonusCollectable;
        transform.position = returnPos;
        inBonus = false;
    }

    IEnumerator BonusCountdown(int s)
    {
        LocalData.lD.bClock = s;
        while (LocalData.lD.bClock > 0)
        {
            yield return new WaitForSeconds(1.0f);
            LocalData.lD.bClock--;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            //onGround = true;
            jumpTick = 0;
            multiJump = 2;
            jumpMultiplier = 1;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            //onGround = true;
            jumpTick = 0;
            multiJump = 2;
            jumpMultiplier = 1;
        }
    }

    /*void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onGround = false;
        }    
    }*/

    public void SnapTo (Vector2 pointA)
    {
        transform.position = new Vector3(pointA.x, pointA.y, transform.position.z);
    }

    public void Jump (float vM, int jLength)
    {
        onGround = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * (Mathf.Pow(vM, 0.125f)));
        multiJump = 2;
        jumpMultiplier = vM;
        jumpTick = jLength;
        canJump = false;
        inJump = true;
    }

    public void Hurt (int damage)
    {
        if (canHurt)
        {
            StartCoroutine(TakeHealth(damage));
        }
    }

    IEnumerator TakeHealth(int damage)
    {
        canHurt = false;
        if (!inBonus)
        {
            LocalData.lD.hCounter = LocalData.lD.hCounter - damage;
        } else
        {
            bonusHealth -= damage;
        }
        yield return new WaitForSeconds(1.5f);
        canHurt = true;
    }

}