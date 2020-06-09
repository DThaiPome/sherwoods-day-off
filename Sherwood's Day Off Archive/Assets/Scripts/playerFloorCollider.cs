using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFloorCollider : MonoBehaviour {

    public List<bool> check;

    public List<GameObject> collidedObject;

    public List<string> tagParam;

    /* 0 - Disabled
     * 1 - True
     * 2 - False
     */ 
    public List<int> enterParam;
    public List<int> stayParam;
    public List<int> exitParam;

	void Start () {
        for(int i = 0; i < check.Count; i++)
        {
            check[i] = false;
        }
	}
	
	void Update () {
        /*RaycastHit2D floor = closestFloor(2, 2);
        Debug.Log(floor.collider.gameObject.name + "; " + Mathf.Sqrt(Mathf.Pow(transform.position.x - floor.point.x, 2) + Mathf.Pow(transform.position.y - floor.point.y, 2)));
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - floor.point.x, 2) + Mathf.Pow(transform.position.y - floor.point.y, 2)) < maxDistance)
        {
            Debug.Log("a");
            transform.GetComponentInParent<PlayerMovement>().SnapTo(new Vector2(transform.position.x, floor.point.y + 2));
        }*/
	}

    /*RaycastHit2D closestFloor (int checks, float width)
    {
        //As empty of a raycast as I can get
        RaycastHit2D result = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down);
        //Empty distance
        float maxDist = Mathf.Infinity;
        for (int i = 0; i < (checks * 2) + 1; i++)
        {
            RaycastHit2D test = Physics2D.Raycast(new Vector2(transform.position.x - (width / 2) + ((i * width) / (2 *checks)), transform.position.y), Vector2.down, Mathf.Infinity, LayerMask.GetMask("floor"));
            Debug.DrawRay(new Vector3(transform.position.x - (width / 2) + ((i * width) / (2 *checks)), transform.position.y, transform.position.z), Vector3.down);
            float testDist = transform.position.y - test.point.y;
            //Test for lower distance
            if (testDist < maxDist)
            {
                result = test;
            }
        }
        return result;
    }*/

    void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tagParam.Count; i++)
        {
            if (collision.gameObject.tag == tagParam[i])
            {
                switch (enterParam[i])
                {
                    case 1:
                        collidedObject[i] = collision.gameObject;
                        check[i] = true;
                        break;
                    case 2:
                        collidedObject[i] = collision.gameObject;
                        check[i] = false;
                        break;
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        for (int i = 0; i < tagParam.Count; i++)
        {
            if (collision.gameObject.tag == tagParam[i])
            {
                switch (stayParam[i])
                {
                    case 1:
                        collidedObject[i] = collision.gameObject;
                        check[i] = true;
                        break;
                    case 2:
                        collidedObject[i] = collision.gameObject;
                        check[i] = false;
                        break;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < tagParam.Count; i++)
        {
            if (collision.gameObject.tag == tagParam[i])
            {
                switch (exitParam[i])
                {
                    case 1:
                        collidedObject[i] = collision.gameObject;
                        check[i] = true;
                        break;
                    case 2:
                        collidedObject[i] = collision.gameObject;
                        check[i] = false;
                        break;
                }
            }
        }
    }
}