using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarnessRendererMaterialholder : MonoBehaviour
{
    public GameObject[] harnessPartMaterial;
    public Transform centerPos;
    SelfRegester sR;
    public GameObject dummyModel;
    public JointdataTransform[] jointPosdata;
    public Transform headkTrans, bottomtrans;
    //[SerializeField]
    //Spine_Z_Rotation spine_Z_Rotation;

    int size;

    private void Start()
    {
        sR = FindObjectOfType<SelfRegester>();
        if (sR != null)
        {

        sR.AssignPrefeb(this.gameObject, dummyModel);
        }

        //size = spine_Z_Rotation.spineRef_Range_Z_Rots.Length;
        //sR.model = dummyModel;
        previousRotationAmount = new float[size];
    }

     float[] previousRotationAmount ;

    private void Update()
    {
        if (harnessPartMaterial[0] == null)
        {
            harnessPartMaterial[0] = this.gameObject;
        }
        //for (int i = 0; i < size; i++)
        //{
        //    if (spine_Z_Rotation.spineRef_Range_Z_Rots[i].zAxisrange != previousRotationAmount[i])
        //    {
        //        var a = spine_Z_Rotation.spineRef_Range_Z_Rots[i];
        //        Vector3 currentRotation = a.spine.eulerAngles;

        //        float rotationChange = a.zAxisrange - previousRotationAmount[i];

        //        currentRotation.z += rotationChange;

        //        a.spine.eulerAngles = currentRotation;

        //        previousRotationAmount[i] = a.zAxisrange;
        //    }
        //}
    }

    public void FindSelfRef()
    {
        sR = FindObjectOfType<SelfRegester>();
        sR.AssignPrefeb(this.gameObject, dummyModel);
        sR.model = dummyModel;
    }

    private void OnDisable()
    {
        //if (sR != null)
        //{
        //    sR.AssignPrefeb(null, null);
        //    sR.model = null;

        //}
    }

    //private void OnDestroy()
    //{
    //    if (sR != null)
    //    {
    //    sR.AssignPrefeb(null,null);
    //    sR.model = null;

    //    }
    //}


}



[System.Serializable]
public class JointdataTransform
{
    public BallJointType jointPoseGroupType;
    public Transform joint1Pos, joint2Pos;
}

public enum Group
{
    joints_A,
    joints_B,
    joints_C,
    joints_D,
    joints_E
}


//[System.Serializable]
//public class Spine_Z_Rotation
//{
//    public SpineRef_Range_Z_Rot[] spineRef_Range_Z_Rots;
//}

//[System.Serializable]
//public struct SpineRef_Range_Z_Rot
//{
//    public string spineName;
//    public Transform spine;
//    [Range(-180,180)]
//    public float zAxisrange;
//    //[Range(-25, 25)]
//    //public float previousRotationAmount;

//}