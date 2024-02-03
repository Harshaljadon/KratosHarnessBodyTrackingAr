using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleBetVect 
{
    Vector3 playerPos,ObjectPos, objectForwardDirection, directionInXyPlaneNormal;
    float finalAngle;

    public AngleBetVect(Vector3 userPos, Vector3 objPos, Vector3 forwardDirectionObject)
    {
        this.playerPos = userPos;
        this.ObjectPos = objPos;
        this.objectForwardDirection = forwardDirectionObject;
    }

    public float AngleBetween()
    {
        var a1 = new Vector3(playerPos.x, 0, playerPos.z);
        var a2 = new Vector3(ObjectPos.x, 0, ObjectPos.z);
        Vector3 directionInXyPlane = a1 - a2;
        float angle = Vector3.SignedAngle(directionInXyPlane, objectForwardDirection, Vector3.up);
        finalAngle = angle;
        return angle;
    }

    public float CustomeAdjustAngle(float val)
    {
        return finalAngle * val;
    }

}
