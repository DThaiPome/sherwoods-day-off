using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    public int cameraMode;

    public GameObject p;

    public float xTarg;
    public float yTarg;

    public float pYO;

    public Vector2 xThresh;
    public Vector2 yThresh;

    public Vector2 posTarg;

    Vector3 dest;
    float cameraSpeed;

    void Start() {
        cameraMode = 0;
        xThresh = new Vector2(-1000000, 1000000);
        yThresh = new Vector2(-1000000, 1000000);
        transform.position = new Vector3(p.transform.position.x, p.transform.position.y + pYO, transform.position.z);
    }

    /*CAMERA MODES:
     * 0 - Follow player directly within specified threshold
     * 1 - Follow player X at specified Y within specified threshold
     * 2 - Follow player Y at specified X within specified threshold
     * 3 - Freeze at position X, Y
     * */

    void Update() {
        switch (cameraMode)
        {
            case 0:
                posTarg = new Vector2(p.transform.position.x, p.transform.position.y + pYO);
                break;
            case 1:
                posTarg = new Vector2(p.transform.position.x, yTarg + pYO);
                break;
            case 2:
                posTarg = new Vector2(xTarg, p.transform.position.y + pYO);
                break;
            case 3:
                posTarg = new Vector2(xTarg, yTarg + pYO);
                break;
        }
        if (cameraMode != 2 && cameraMode != 3)
        {
            if (posTarg.x < xThresh.x)
            {
                posTarg = new Vector2(xThresh.x, posTarg.y);
            }
            else if (posTarg.x > xThresh.y)
            {
                posTarg = new Vector2(xThresh.y, posTarg.y);
            }
        }
        if (cameraMode != 1 && cameraMode != 3)
        {
            if (posTarg.y < yThresh.x)
            {
                posTarg = new Vector2(posTarg.x, yThresh.x);
            }
            else if (posTarg.y > yThresh.y)
            {
                posTarg = new Vector2(posTarg.x, yThresh.y);
            }
        }
        dest = new Vector3(posTarg.x, posTarg.y, transform.position.z);
        float dist = Vector2.Distance(transform.position, new Vector2(dest.x, dest.y));
        float cameraSpeed;
        if (dist < 65.5)
        {
            cameraSpeed = (0.00000000000000005f * Mathf.Pow((dist - 100), 8));
        }
        else
        {
            cameraSpeed = 1;
        }
        transform.position = Vector3.Lerp(transform.position, dest, cameraSpeed);
    }

    public void SetCamera(int newMode, float newXTarg, float newYTarg, float xThreshL = -1000000, float xThreshH = 1000000, float yThreshL = -1000000, float yThreshH = 1000000)
    {
        cameraMode = newMode;
        xTarg = newXTarg;
        yTarg = newYTarg;
        xThresh = new Vector2(xThreshL, xThreshH);
        yThresh = new Vector2(yThreshL, yThreshH);
    }
}
