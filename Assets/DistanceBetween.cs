using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceBetween : MonoBehaviour
{
    public Transform target1, target2;
    public float dis;
    private LineRenderer lineRenderer;
    public float offset;
    [Range(0,1)]
    public float rangbetBall;
    public ballDirectionRef _ballDirectionRefAxis;
    public GameObject distanceTextPanel;
    public TextMeshProUGUI distnaceText;
    public Camera arCamera; // Reference to the AR camera


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of points to 2 (start and end points)
        lineRenderer.positionCount = 2;
        arCamera = Camera.main; // Assign the AR camera reference

    }


    // Update is called once per frame
    void Update()
    {
        //distanceTextPanel.transform.LookAt(new Vector3(arCamera.transform.position.x, distanceTextPanel.transform.position.y, arCamera.transform.position.z));

        // Set the positions of the line renderer's points
        UIDisTanceTextUpdate();
        LineBetweenBalls();
        UiPanelDistancePos();
    }


    void UIDisTanceTextUpdate()
    {
        dis = Vector3.Distance(target1.position, target2.position);

        distnaceText.text = dis.ToString("F2") + " m";
    }
    void LineBetweenBalls()
    {
        lineRenderer.SetPosition(0, target1.position);
        lineRenderer.SetPosition(1, target2.position);
    }

    void UiPanelDistancePos()
    {
        
        Vector3 centerPos = Vector3.Lerp(target1.transform.position, target2.transform.position, rangbetBall);

        Vector3 desirDirect = centerPos + SelectDirectionJoint1(_ballDirectionRefAxis) * offset;
        distanceTextPanel.transform.position = desirDirect;
    }

    Vector3 SelectDirectionJoint1(ballDirectionRef dj1)
    {
        switch (dj1)
        {
            case ballDirectionRef.forwardD:
                return Vector3.forward;
            case ballDirectionRef.backwardD:
                return -Vector3.forward;
            case ballDirectionRef.rightD:
                return Vector3.right;
            case ballDirectionRef.leftD:
                return -Vector3.right;
            case ballDirectionRef.topD:
                return Vector3.up;
            case ballDirectionRef.bottomD:
                return -Vector3.up;
            default:
                return Vector3.zero;
        }
        //return Vector3.zero;
    }
}

public enum ballDirectionRef
{
    forwardD,
    backwardD,
    topD,
    bottomD,
    leftD,
    rightD,

}
