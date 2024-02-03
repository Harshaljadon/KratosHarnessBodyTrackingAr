using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Import HolisticBarracuda
using MediaPipe.Holistic;

public class Visuallizer : MonoBehaviour
{
    //Vector4 leftHandWristPosUi, leftHandWristPosUi1;
    //Vector3 leftHandPosWord, leftHandPosWord1;
    public Canvas canvas;

    float screenWeidth, screenHeight;
    public int indexVal;
    public GameObject debugBallLeft, debugBallRight;
    public Transform leftparent, rightParent;
    List<Transform> leftHandDebug, rightHanDebug;
    [SerializeField] Camera cam;
    [SerializeField] WebCamInput webCamInput;
    [SerializeField] RawImage image;
    [SerializeField] Shader poseShader;
    [SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;
    [SerializeField] Shader faceShader;
    [SerializeField] Mesh faceLineTemplateMesh;
    [SerializeField] Shader handShader;
    [SerializeField, Range(0, 1)] float handScoreThreshold = 0.5f;
    // Select inference type with pull down on the Unity Editor.
    [SerializeField] HolisticInferenceType holisticInferenceType = HolisticInferenceType.full;

    HolisticPipeline holisticPipeline;
    Material poseMaterial;
    Material faceMeshMaterial;
    Material handMaterial;

    // Lines count of body's topology.
    const int BODY_LINE_NUM = 35;
    // Pairs of vertex indices of the lines that make up body's topology.
    // Defined by the figure in https://google.github.io/mediapipe/solutions/pose.
    readonly List<Vector4> linePair = new List<Vector4>{
        new Vector4(0, 1), new Vector4(1, 2), new Vector4(2, 3), new Vector4(3, 7), new Vector4(0, 4), 
        new Vector4(4, 5), new Vector4(5, 6), new Vector4(6, 8), new Vector4(9, 10), new Vector4(11, 12), 
        new Vector4(11, 13), new Vector4(13, 15), new Vector4(15, 17), new Vector4(17, 19), new Vector4(19, 15), 
        new Vector4(15, 21), new Vector4(12, 14), new Vector4(14, 16), new Vector4(16, 18), new Vector4(18, 20), 
        new Vector4(20, 16), new Vector4(16, 22), new Vector4(11, 23), new Vector4(12, 24), new Vector4(23, 24), 
        new Vector4(23, 25), new Vector4(25, 27), new Vector4(27, 29), new Vector4(29, 31), new Vector4(31, 27), 
        new Vector4(24, 26), new Vector4(26, 28), new Vector4(28, 30), new Vector4(30, 32), new Vector4(32, 28)
    };

    public GameObject ballPrefab; // Prefab of the ball GameObject
    public int totalBalls = 21;
    public int ballsPerGroup = 4;
    public int groupsPerParent = 5;

    public LeftRightHandBone _leftRightHandBone;
    private float initialAngle;


    void Start()
    {

        for (int i = 0; i < 5; i++)
        {
        var lefthandfingerRefColl = _leftRightHandBone._lefthandbone._leftCommonHandTransformComponetBoneCollection.fingers[i];
        lefthandfingerRefColl.childBones.bone = lefthandfingerRefColl.parentBones.GetComponentsInChildren<Transform>();

            var righthandfingerRefColl = _leftRightHandBone.rightHandBone._rightCommonHandTransformComponetBoneCollection.fingers[i];
            righthandfingerRefColl.childBones.bone = righthandfingerRefColl.parentBones.GetComponentsInChildren<Transform>();

        }


        leftHandDebug = new List<Transform>();
        rightHanDebug = new List<Transform>();
        // Make instance of HolisticPipeline
        holisticPipeline = new HolisticPipeline();

        poseMaterial = new Material(poseShader);
        faceMeshMaterial = new Material(faceShader);
        handMaterial = new Material(handShader);

        screenHeight = Screen.height;
        screenWeidth = Screen.width;



        CeateLeftrightHandballs(debugBallLeft, leftparent, "leftHand", leftHandDebug);
        CeateLeftrightHandballs(debugBallRight, rightParent, "RightHand", rightHanDebug);

        Vector3 vectorAB = lefthandCoordinat[4] - lefthandCoordinat[20];

        initialAngle = Vector3.SignedAngle(Vector3.forward, vectorAB, Vector3.up);

    }

    void CeateLeftrightHandballs(GameObject ballPrefab, Transform parent, string handName, List<Transform> handCollectionTransform)
    {
        GameObject parentBall = Instantiate(ballPrefab, parent);
        parentBall.name = handName; // Assign a name to the parent ball
        handCollectionTransform.Add(parentBall.transform);

        int ballCounter = 1;

        for (int groupIndex = 0; groupIndex < groupsPerParent; groupIndex++)
        {
            GameObject groupParent = Instantiate(ballPrefab, parentBall.transform);
            groupParent.name = $"GroupParent_{groupIndex + 1}";
            handCollectionTransform.Add(groupParent.transform);
            GameObject previousBall = groupParent; // Initialize with the group parent as the first previous ball

            for (int ballInGroup = 0; ballInGroup < ballsPerGroup; ballInGroup++)
            {
                GameObject ball = Instantiate(ballPrefab, previousBall.transform);
                ball.name = $"Ball_{ballCounter}";
                ballCounter++;
                handCollectionTransform.Add(ball.transform);
                previousBall = ball; // Update previous ball to the current ball
            }
        }


    }


    void LateUpdate()
    {
        image.texture = webCamInput.inputImageTexture;




        // Inference. Switchable inference type anytime.
        holisticPipeline.ProcessImage(webCamInput.inputImageTexture, holisticInferenceType);


        StoreLeftRightUiX_YPose();
    }

    public Vector3[] lefthandCoordinat = new Vector3[21];
    public Vector3[] righthandCoordinat = new Vector3[21];

    public Quaternion[] lefthandrotation = new Quaternion[21];
    public Quaternion[] righthandrotation = new Quaternion[21];
    int sum, lefthandFingerCount, righthandfingerCount;

    public float offsetRotX, offsetRotY, offsetRotZ;

    public Quaternion lefthandwristPointMiddleFingPointRotationIndex, newQuaternion, lefthandwristPointThumbPointRotationIndex;

    void StoreLeftRightUiX_YPose()
    {
        //left, right hand
        for (int i = 0; i < 21; i++)
        {
            var mapCordinate_1to_negative_1_lefthand = holisticPipeline.GetLeftHandLandmark(i); // -1 to 1
            var mapCordinate_1to_negative_1_righthand = holisticPipeline.GetRightHandLandmark(i); // -1 to 1

            var screenPosConvertLefthand = new Vector3(screenWeidth * (mapCordinate_1to_negative_1_lefthand.x), screenHeight * (mapCordinate_1to_negative_1_lefthand.y), 0);
            var screenPosConvertRightHand = new Vector3(screenWeidth * (mapCordinate_1to_negative_1_righthand.x), screenHeight * (mapCordinate_1to_negative_1_righthand.y), 0);

            var worldXYCoordinateLefthand = cam.ScreenToWorldPoint(screenPosConvertLefthand);
            var worldXYCoordinateRighthand = cam.ScreenToWorldPoint(screenPosConvertRightHand);

            lefthandCoordinat[i] = new Vector3(worldXYCoordinateLefthand.x, worldXYCoordinateLefthand.y, 0);
            righthandCoordinat[i] = new Vector3(worldXYCoordinateRighthand.x, worldXYCoordinateRighthand.y, 0);

            leftHandDebug[i].position = new Vector3(lefthandCoordinat[i].x, lefthandCoordinat[i].y, leftHandDebug[i].position.z);
            rightHanDebug[i].position = new Vector3(righthandCoordinat[i].x, righthandCoordinat[i].y, rightHanDebug[i].position.z);

            //sum += 1;
            //if (sum < 21)
            //{
            //    var diffLeft = mapCordinate_1to_negative_1_lefthand - holisticPipeline.GetLeftHandLandmark(sum) ;
            //    var diffRight = mapCordinate_1to_negative_1_righthand - holisticPipeline.GetRightHandLandmark(sum);

            //    var lefthandrotationIndex = Quaternion.FromToRotation(Vector3.up, diffLeft );
            //    var righthandrotationIndex = Quaternion.FromToRotation(Vector3.up, diffRight);

            //    lefthandrotation[i] = lefthandrotationIndex;
            //    righthandrotation[i] = righthandrotationIndex;
            //    leftHandDebug[i].rotation = lefthandrotation[i];
            //    rightHanDebug[i].rotation = righthandrotation[i];
            //}
            //else
            //{
            //    sum = 0;
            //}
        }
        int x = 0;
        int k = 0;

        for (int r = 0; r < 5; r++)
        {
            for (int j = 1 + k; j < 4 + x; j++)
            {
                var diffLeft = holisticPipeline.GetLeftHandLandmark(j + 1) - holisticPipeline.GetLeftHandLandmark(j);
                var diffRight = holisticPipeline.GetRightHandLandmark(j + 1) - holisticPipeline.GetRightHandLandmark(j);


                var lefthandrotationIndex = Quaternion.FromToRotation(Vector3.up, diffLeft);
                var righthandrotationIndex = Quaternion.FromToRotation(Vector3.up, diffRight);

                lefthandrotation[j] = lefthandrotationIndex;
                righthandrotation[j] = righthandrotationIndex;

                leftHandDebug[j].rotation = lefthandrotation[j];
                rightHanDebug[j].rotation = righthandrotation[j];
            }
            x += 4;
            k += 4;
        }


        var leftWrist = _leftRightHandBone._lefthandbone._leftCommonHandTransformComponetBoneCollection;
        var rightWrist = _leftRightHandBone.rightHandBone._rightCommonHandTransformComponetBoneCollection;


        //rightWrist.wrist.position = new Vector3(righthandCoordinat[0].x, righthandCoordinat[0].y, rightWrist.wrist.position.z);


        leftWrist.wrist.position = new Vector3(lefthandCoordinat[0].x, lefthandCoordinat[0].y, leftWrist.wrist.position.z); //lefthandCoordinat[0].z
        //leftWrist.wrist.SetParent(leftHandDebug[0].transform);

        var wristPointMiddleFingPointDiff = holisticPipeline.GetLeftHandLandmark(0) - holisticPipeline.GetLeftHandLandmark(9);
        var wristPointthumbFingPointDiff = holisticPipeline.GetLeftHandLandmark(0) - holisticPipeline.GetLeftHandLandmark(1);

        lefthandwristPointMiddleFingPointRotationIndex = Quaternion.FromToRotation(-Vector3.up, wristPointMiddleFingPointDiff);
        lefthandwristPointThumbPointRotationIndex = Quaternion.FromToRotation(-Vector3.up, wristPointthumbFingPointDiff);




        //newQuaternion = new Quaternion(lefthandwristPointMiddleFingPointRotationIndex.z, lefthandrotation[0].x, lefthandwristPointMiddleFingPointRotationIndex.z, lefthandwristPointMiddleFingPointRotationIndex.w) ;// lefthandrotation[0]; //lefthandrotation[0].y, lefthandrotation[0].z * Quaternion.Euler(offsetRotX, offsetRotY, offsetRotZ)
        //newQuaternion = new Quaternion(lefthandwristPointThumbPointRotationIndex.z, lefthandwristPointMiddleFingPointRotationIndex.y, lefthandwristPointMiddleFingPointRotationIndex.z, lefthandwristPointMiddleFingPointRotationIndex.w) ;// lefthandrotation[0]; //lefthandrotation[0].y, lefthandrotation[0].z * Quaternion.Euler(offsetRotX, offsetRotY, offsetRotZ)
        //leftWrist.wrist.rotation = lefthandwristPointMiddleFingPointRotationIndex;

        Vector3 vectorAB = lefthandCoordinat[4] - lefthandCoordinat[20];

        float currentAngle = Vector3.SignedAngle(Vector3.forward, vectorAB, Vector3.up);

        // Calculate the rotation angle variation
        float rotationAngleVariation = currentAngle - initialAngle;
        // Apply the rotation variation to Point C
        leftWrist.wrist.Rotate(Vector3.up, rotationAngleVariation, Space.Self);

        //float rotationAngle = Vector3.Angle(Vector3.forward, vectorAB);

        //// Calculate the rotation axis based on the cross product of forward and vector AB
        //Vector3 rotationAxis = Vector3.Cross(Vector3.forward, vectorAB);

        //// Create a quaternion representing the rotation
        //Quaternion rotation = Quaternion.Euler(rotationAxis * rotationAngle );

        //// Apply the rotation to Point C
        //leftWrist.wrist.rotation = rotation;


        // Update the initial angle for the next frame
        initialAngle = currentAngle;


        //float rotationValueX = lefthandrotation[0].x;  /* your value from -0.01 to 0.01 here */
        //float rotationValueY = lefthandrotation[0].y;  /* your value from -0.01 to 0.01 here */
        //float rotationValueZ = lefthandrotation[0].z;  /* your value from -0.01 to 0.01 here */
        //float degreeValueX = (rotationValueX + 0.01f) * 450f;
        //float degreeValueY = (rotationValueY + 0.01f) * 450f;
        //float degreeValueZ = (rotationValueZ + 0.01f) * 450f;
        //degreeValueX = degreeValueX % 360;
        //degreeValueY = degreeValueY % 360;
        //degreeValueZ = degreeValueZ % 360;
        //var newEu = new Vector3(0, -degreeValueX, 0);
        //midPosTarget.rotation = Quaternion.Euler(newEu);

        //rightWrist.wrist.rotation = righthandrotation[0];

        lefthandFingerCount = righthandfingerCount = 1;

        for (int i = 0; i < 5; i++)
        {
            //var lefthandfingerRefColl = _leftRightHandBone._lefthandbone._leftCommonHandTransformComponetBoneCollection.fingers[i];

            //for (int j = 0; j < 4; j++)
            //{
            //    lefthandfingerRefColl.childBones.bone[j].position = new Vector3(lefthandCoordinat[lefthandFingerCount].x, lefthandCoordinat[lefthandFingerCount].y, lefthandfingerRefColl.childBones.bone[j].position.z);
            //    //lefthandfingerRefColl.childBones.bone[j].localRotation = lefthandrotation[lefthandFingerCount];

            //    lefthandFingerCount++;
            //}



            //var righthandfingerRefColl = _leftRightHandBone.rightHandBone._rightCommonHandTransformComponetBoneCollection.fingers[i];

            //for (int k = 0; k < 4; k++)
            //{
            //    righthandfingerRefColl.childBones.bone[k].position = new Vector3(righthandCoordinat[righthandfingerCount].x, righthandCoordinat[righthandfingerCount].y, righthandfingerRefColl.childBones.bone[k].position.z);
            //    righthandfingerCount++;
            //}



        }

    }



    //void OnRenderObject(){
    //    if(holisticInferenceType != HolisticInferenceType.face_only) PoseRender();
    //    if(holisticInferenceType == HolisticInferenceType.pose_only) return;

    //    if( holisticInferenceType == HolisticInferenceType.full || 
    //        holisticInferenceType == HolisticInferenceType.pose_and_face || 
    //        holisticInferenceType == HolisticInferenceType.face_only)
    //    {
    //        FaceRender();
    //    }

    //    if( holisticInferenceType == HolisticInferenceType.full || 
    //        holisticInferenceType == HolisticInferenceType.pose_and_hand)
    //    {
    //        HandRender(false);
    //        HandRender(true);
    //    }
    //}

    void PoseRender(){
        //var w = image.rectTransform.rect.width;
        //var h = image.rectTransform.rect.height;

        //// Set inferenced pose landmark results.
        //poseMaterial.SetBuffer("_vertices", holisticPipeline.poseLandmarkBuffer);
        //// Set pose landmark counts.
        //poseMaterial.SetInt("_keypointCount", holisticPipeline.poseVertexCount);
        //poseMaterial.SetFloat("_humanExistThreshold", humanExistThreshold);
        //poseMaterial.SetVector("_uiScale", new Vector2(w, h));
        //poseMaterial.SetVectorArray("_linePair", linePair);

        //// Draw 35 body topology lines.
        //poseMaterial.SetPass(0);
        //Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, BODY_LINE_NUM);

        //// Draw 33 landmark points.
        //poseMaterial.SetPass(1);
        //Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, holisticPipeline.poseVertexCount);
    }

    void FaceRender(){
        //var w = image.rectTransform.rect.width;
        //var h = image.rectTransform.rect.height;
        //faceMeshMaterial.SetVector("_uiScale", new Vector2(w, h));

        //// FaceMesh
        //// Set inferenced face landmark results.
        //faceMeshMaterial.SetBuffer("_vertices", holisticPipeline.faceVertexBuffer);
        //faceMeshMaterial.SetPass(0);
        //Graphics.DrawMeshNow(faceLineTemplateMesh, Vector3.zero, Quaternion.identity);

        //// Left eye
        //// Set inferenced eye landmark results.
        //faceMeshMaterial.SetBuffer("_vertices", holisticPipeline.leftEyeVertexBuffer);
        //faceMeshMaterial.SetVector("_eyeColor", Color.yellow);
        //faceMeshMaterial.SetPass(1);
        //Graphics.DrawProceduralNow(MeshTopology.Lines, 64, 1);

        //// Right eye
        //// Set inferenced eye landmark results.
        //faceMeshMaterial.SetBuffer("_vertices", holisticPipeline.rightEyeVertexBuffer);
        //faceMeshMaterial.SetVector("_eyeColor", Color.cyan);
        //faceMeshMaterial.SetPass(1);
        //Graphics.DrawProceduralNow(MeshTopology.Lines, 64, 1);
    }

    void HandRender(bool isRight){
        //var w = image.rectTransform.rect.width;
        //var h = image.rectTransform.rect.height;
        //handMaterial.SetVector("_uiScale", new Vector2(w, h));
        //handMaterial.SetVector("_pointColor", isRight ? Color.cyan : Color.yellow);
        //handMaterial.SetFloat("_handScoreThreshold", handScoreThreshold);
        //// Set inferenced hand landmark results.
        //handMaterial.SetBuffer("_vertices", isRight ? holisticPipeline.rightHandVertexBuffer : holisticPipeline.leftHandVertexBuffer);

        //// Draw 21 key point circles.
        //handMaterial.SetPass(0);
        //Graphics.DrawProceduralNow(MeshTopology.Triangles, 96, holisticPipeline.handVertexCount);

        //// Draw skeleton lines.
        //handMaterial.SetPass(1);
        //Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
    }

    void OnDestroy(){
        // Must call Dispose method when no longer in use.
        holisticPipeline.Dispose();
    }
}


[System.Serializable]
public class LeftRightHandBone
{
    public Lefthandbone _lefthandbone;
    public RightHandBone rightHandBone;
}

[System.Serializable]
public class Lefthandbone
{

    public CommonHandTransformComponet _leftCommonHandTransformComponetBoneCollection;
}

[System.Serializable]
public class RightHandBone
{
    public CommonHandTransformComponet _rightCommonHandTransformComponetBoneCollection;
}

[System.Serializable]
public struct CommonHandTransformComponet
{
    public Transform wrist;
    public Fingers[] fingers;

}

[System.Serializable]
public class Fingers
{
    public string parentboneName;
    public Transform parentBones;
    public BoneData childBones;

}

[System.Serializable]
public class BoneData
{
    public Transform[] bone;

}
