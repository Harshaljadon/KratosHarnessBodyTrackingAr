using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class HarnessSizeColliderHandTrigger : MonoBehaviour
{
    //SetClipPlanes setClipPlanes;

    public float scalingDuration = 2.0f; // Duration of the scaling animation in seconds

    public bool leftHandEnter, rightHandEnter, bothHandEnetered, swapDone;
    HarnessMasserManagerUI harnessMasserManagerUI;
    public int countIndex;
    //public int currentSizeIndex;


    public Transform leftShoulderPos, rightHandPos ;


    public float leftShoulderRightHandDistandXAxis, exitEnterXAxisDis;

    public HarnessSorting harnessSorting;

    

    public Vector2 enterPoint, nextFrameEnterPoint, directionHand;
    public float angleRightDarection;
    private void OnTriggerEnter(Collider other)
    {
        //countIndex = (int)SceneManag.Instance.harnessSize;

        if (other.gameObject.CompareTag("LeftHand") )
        {
            //leftHandEnter = true;
            //var scaleRef = setClipPlanes.harnessSizePanel.GetChild(0);
            //if (scaleRef.localScale == Vector3.zero)
            //{
            //    //setClipPlanes.harnessSizePanel.GetChild(0).DOScale(1, 1);
            //    //initialScale = new Vector3(1, 1, 1);
            //    StartCoroutine(ScaleDownCoroutine(Vector3.zero,Vector3.one, scaleRef));

            //}
            //else
            //{
            //    StartCoroutine(ScaleDownCoroutine(Vector3.one,Vector3.zero, scaleRef));
            ////setClipPlanes.harnessSizePanel.GetChild(0).DOScale(0, 1);

            //}
        }

        if (other.gameObject.CompareTag("RightHand"))
        {
            rightHandEnter = true;
            rightHandPos = other.gameObject.transform;
            enterPoint = new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y);
            StartCoroutine(nameof(NextFrameHandPos));
            //if (firstEnter)
            //{
            //exitEnterXAxisDis = Vector3.Distance(new Vector3(enterPoint.x, 0, 0), new Vector3(exitPoint.x, 0, 0));
            //Debug.Log(exitEnterXAxisDis);

            //}
        }


    }

    IEnumerator NextFrameHandPos()
    {
        yield return new WaitForSeconds(.1f);
        if (rightHandPos != null)
        {
        nextFrameEnterPoint = new Vector2(rightHandPos.transform.position.x, rightHandPos.transform.position.y);
            if (nextFrameEnterPoint != enterPoint)
            {
                directionHand = (nextFrameEnterPoint - enterPoint).normalized;
                angleRightDarection = Vector2.Angle(directionHand, Vector2.right);
                //angleRightDarection = Mathf.Atan2(directionHand.y, directionHand.x) * Mathf.Rad2Deg;
                //Debug.Log(angleRightDarection);
            }
        }

        if (rightHandEnter && rightHandPos != null)
        {

            //leftShoulderRightHandDistandXAxis = Vector3.Distance(new Vector3(leftShoulderPos.position.x, 0, 0), new Vector3(rightHandPos.position.x, 0, 0));
            //Debug.Log(leftShoulderRightHandDistandXAxis);

            if (!swapDone && angleRightDarection < 50) //&& exitEnterXAxisDis > .03f && leftShoulderRightHandDistandXAxis < 0.05f
            {


                swapDone = true;
                HarnessSizeIndex();
            }
        }
    }

    void HarnessSizeIndex()
    {
        if (rightHandEnter)
        {
            //Debug.Log(harnessSorting.harnessSizetype);
            switch (harnessSorting.harnessSizetype)
            {

                case HarnessSizetype.twoTypeSize:
                    if (countIndex == 1)
                    {
                        countIndex = 0;
                        ChangeHarnessSize(1); // medium size
                    }
                    else
                    {
                        countIndex++;
                        ChangeHarnessSize(3); // xl size

                    }
                    break;
                case HarnessSizetype.threeTypeSize:
                    if (countIndex == 2)
                    {
                        countIndex = 0;

                    }
                    else
                    {
                    countIndex++;

                    }

  
                    switch (countIndex)
                    {
                        case 0:
                            // small
                            ChangeHarnessSize(0);

                            break;
                        case 1:
                            // large
                            ChangeHarnessSize(2);

                            break;
                        case 2:
                            // xl
                            ChangeHarnessSize(3);
                            break;
                    }

                    break;
                case HarnessSizetype.universal:
                    if (countIndex < 5)
                    {

                        if (countIndex == 4)
                        {
                            countIndex = 0;
                            ChangeHarnessSize(countIndex);
                        }
                        else
                        {
                            countIndex++;
                            ChangeHarnessSize(countIndex);

                        }
                    }
                    break;
            }
           
            bothHandEnetered = true;
        }
    }

    void ChangeHarnessSize(int id)
    {
        if (harnessMasserManagerUI != null && !harnessMasserManagerUI.takingSnap)
        {
            SceneManag.Instance.harnessSize = (HarnessSize)id;

            harnessMasserManagerUI.AdjustHarnessSizeHandInteraction(id);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand"))
        {
            leftHandEnter = false;
            //var scaleRef = setClipPlanes.harnessSizePanel.GetChild(0);
            //if (scaleRef.localScale == Vector3.zero)
            //{
            //    //setClipPlanes.harnessSizePanel.GetChild(0).DOScale(1, 1);
            //    //initialScale = new Vector3(1, 1, 1);
            //    StartCoroutine(ScaleDownCoroutine(Vector3.zero,Vector3.one, scaleRef));

            //}
            //else
            //{
            //    StartCoroutine(ScaleDownCoroutine(Vector3.one,Vector3.zero, scaleRef));
            ////setClipPlanes.harnessSizePanel.GetChild(0).DOScale(0, 1);

            //}
        }

        if (other.gameObject.CompareTag("RightHand"))
        {
            swapDone = false;
            rightHandEnter = false;
            //rightHandPos = other.gameObject.transform;
            //exitPoint = rightHandPos.position;

            //if (!firstEnter)
            //{
            //    firstEnter = true;
            //    exitEnterXAxisDis = Vector3.Distance(new Vector3(enterPoint.x, 0, 0), new Vector3(exitPoint.x, 0, 0));
            //    Debug.Log(exitEnterXAxisDis);

            //}
        }
        if (!rightHandEnter && !leftHandEnter && bothHandEnetered)
        {
            bothHandEnetered = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        harnessMasserManagerUI = FindObjectOfType<HarnessMasserManagerUI>();
        countIndex = (int)SceneManag.Instance.harnessSize;
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            harnessSorting = FindObjectOfType<HarnessSorting>();
        }
        //setClipPlanes = FindObjectOfType<SetClipPlanes>();
    }

    //private void Update()
    //{
       
    //}


    //private IEnumerator ScaleDownCoroutine(Vector3 initial, Vector3 final,Transform targetObjTransform)
    //{
    //    float elapsedTime = 0f;
    //    Vector3 targetScale = final; // This is the scale you want to reach

    //    while (elapsedTime < scalingDuration)
    //    {
    //        Debug.Log("scalling");
    //        targetObjTransform.localScale = Vector3.Lerp(initial, final, elapsedTime / scalingDuration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Ensure the object is at the target scale when the coroutine finishes
    //    targetObjTransform.localScale = targetScale;
    //}
}
