using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetDotFromJoint : MonoBehaviour
{
    [SerializeField]
    BallOffsetData[] ballOffsetData;
    SelfRegester selfRegester;
    // Start is called before the first frame update
    void Start()
    {
        selfRegester = FindObjectOfType<SelfRegester>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selfRegester.HarnesPrefebinsticated == null)
        {
            return;
        }
        CheckAllRef();
        foreach (var ballJoint in ballOffsetData)
        {
            if (ballJoint._alligment == Alligment.vertical)
            {
                // Calculate the desired position based on the distance and target object's position
                Vector3 desiredPosition1 = ballJoint.joint1.position + SelectDirectionJoint1(ballJoint.jointDirection1, ballJoint) * ballJoint.offsetBetJoint1Ball1 +
                                           SelectDirectionJoint1(ballJoint.zAxis, ballJoint) * ballJoint.zAxisOffset ;

                // Set the position of this game object to the desired position
                ballJoint.ball1.position = desiredPosition1;

                // Calculate the desired position based on the distance and target object's position
                //Vector3 desiredPosition2 = ballJoint.joint2.position + SelectDirectionJoint1(ballJoint.jointDirection2, ballJoint) * ballJoint.offsetBetJoint2Ball2;
                //Vector3 desiredPosition2 = new Vector3(ballJoint.joint1.position.x, ballJoint.joint2.position.y, ballJoint.joint1.position.z) ;

                // Set the position of this game object to the desired position
                ballJoint.ball2.position = new Vector3(desiredPosition1.x, ballJoint.joint2.position.y, desiredPosition1.z);
            }
            else if (ballJoint._alligment == Alligment.horizontal)
            {
                // Calculate the desired position based on the distance and target object's position
                //Vector3 desiredPosition1 = ballJoint.joint1.position + SelectDirectionJoint1(ballJoint.jointDirection1, ballJoint) * ballJoint.offsetBetJoint1Ball1;

                //// Set the position of this game object to the desired position
                //ballJoint.ball1.position = desiredPosition1 + SelectDirectionJoint1(ballJoint.zAxis, ballJoint) * ballJoint.zAxisOffset;

                //// Calculate the desired position based on the distance and target object's position
                //Vector3 desiredPosition2 = ballJoint.joint2.position + SelectDirectionJoint2(ballJoint.jointDirection2, ballJoint) * -ballJoint.offsetBetJoint1Ball1;

                //// Set the position of this game object to the desired position
                //ballJoint.ball2.position = desiredPosition2 + SelectDirectionJoint1(ballJoint.zAxis, ballJoint) * ballJoint.zAxisOffset;
            }
            else
            {
                Vector3 centerpoint = Vector3.Lerp(ballJoint.joint1.position, ballJoint.joint2.position, 0.5f);
                // Calculate the desired position based on the distance and target object's position
                Vector3 desiredPosition1 = centerpoint + SelectDirectionJoint1(ballJoint.jointDirection1, ballJoint) * ballJoint.offsetBetJoint1Ball1;

                // Set the position of this game object to the desired position
                ballJoint.ball1.position = desiredPosition1 + SelectDirectionJoint1(ballJoint.zAxis, ballJoint) * ballJoint.zAxisOffset;

                // Calculate the desired position based on the distance and target object's position
                Vector3 desiredPosition2 = centerpoint + SelectDirectionJoint2(ballJoint.jointDirection2, ballJoint) * -ballJoint.offsetBetJoint1Ball1;

                // Set the position of this game object to the desired position
                ballJoint.ball2.position = desiredPosition2 + SelectDirectionJoint1(ballJoint.zAxis, ballJoint) * ballJoint.zAxisOffset;
            }

        }
    }


    void CheckAllRef()
    {
    

        var jointCollection = selfRegester.HarnesPrefebinsticated.GetComponent<HarnessRendererMaterialholder>().jointPosdata;
        foreach (var item in jointCollection)
        {
            foreach (var ballData in ballOffsetData)
            {
                if (item.jointPoseGroupType == ballData.ballType)
                {
                    ballData.joint1 = item.joint1Pos;
                    ballData.joint2 = item.joint2Pos;
                    break;
                }
            }
        }
    }

    Vector3 SelectDirectionJoint1(directionJoint dj1, BallOffsetData ballJoint)
    {
        switch (dj1)
        {
            case directionJoint.forwardD:
                return ballJoint.joint1.forward;
            case directionJoint.backwardD:
                return -ballJoint.joint1.forward;
            case directionJoint.rightD:
                return ballJoint.joint1.right;
            case directionJoint.leftD:
                return -ballJoint.joint1.right;
            case directionJoint.topD:
                return ballJoint.joint1.up;
            case directionJoint.bottomD:
                return -ballJoint.joint1.up;
            default:
                return Vector3.zero;
        }
        //return Vector3.zero;
    }

    Vector3 SelectDirectionJoint2(directionJoint dj2, BallOffsetData ballJoint)
    {
        switch (dj2)
        {
            case directionJoint.forwardD:
                return ballJoint.joint2.forward;
            case directionJoint.backwardD:
                return -ballJoint.joint2.forward;
            case directionJoint.rightD:
                return ballJoint.joint2.right;
            case directionJoint.leftD:
                return -ballJoint.joint2.right;
            case directionJoint.topD:
                return ballJoint.joint2.up;
            case directionJoint.bottomD:
                return -ballJoint.joint2.up;
            default:
                return Vector3.zero;
                
        }
        //return Vector3.zero;
    }

    void SelectDirectionJoint2()
    {

    }

}


[System.Serializable]
public class BallOffsetData
{
    public Alligment _alligment;
    public BallJointType ballType;
    public directionJoint jointDirection1,jointDirection2, zAxis;
    public Transform joint1, joint2, ball1, ball2;
    public float offsetBetJoint1Ball1, offsetBetJoint2Ball2, zAxisOffset;

}


public enum BallJointType
{
A_Head_Toe,B_RightLeftArm,C_Hip_Neck3,D_LeftUpRightUpLeg,E_LeftUpLegLeftLeg,F_RightUpLegRightLeg

}

public enum directionJoint
{
    forwardD,
    backwardD,
    topD,
    bottomD,
    leftD,
    rightD,
    Zero

}

public enum Alligment
{
    horizontal,vertical,width
}
