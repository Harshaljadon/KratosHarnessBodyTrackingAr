using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HarnessTouchControls : MonoBehaviour
{
    public HarnessManager harnessManager;

    public float ZoomSpeed = 0.01f, rotationspeed; 

    private float rotationRate = 0.2f;

    //Cloth currecntHarnessCloth;
    bool readyTosetInitialiData;

    private void Start()
    {
        startTime = Time.time;   // Record the start time
        
    }


    void Update()
    {
        if (harnessManager.CurrentHarness?.gameObject != null)
        {
            if (!readyTosetInitialiData)
            {
                readyTosetInitialiData = true;
                ResetDataObject();
                //var harnessShellPos = harnessManager.CurrentHarness.helmetShel.transform.position;
                //endPoint = new Vector3(initialPosition.x, initialPosition.y + harnessManager.CurrentHarness.yAxisPosOffset, initialPosition.z);
            }
            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                //currecntHarnessCloth = harnessManager.CurrentHarness.GetComponentInChildren<Cloth>();
                //if (currecntHarnessCloth != null)
                //{
                //    currecntHarnessCloth.enabled = false;

                //}
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


                harnessManager.CurrentHarness.transform.localScale += Vector3.one * (deltaMagnitudeDiff * ZoomSpeed);

                float tempScale = Mathf.Clamp(harnessManager.CurrentHarness.transform.localScale.x, 0.6f, 2f);

                harnessManager.CurrentHarness.transform.localScale = Vector3.one * tempScale;
            }
            else if (Input.touchCount == 1)
            {
                //currecntHarnessCloth = harnessManager.CurrentHarness.GetComponentInChildren<Cloth>();
                //currecntHarnessCloth = harnessManager.CurrentHarness.GetComponentInChildren<Cloth>();
                //if (currecntHarnessCloth != null)
                //{

                //    currecntHarnessCloth.enabled = true;
                //}
                foreach (Touch touch in Input.touches)
                {
                    //Debug.Log("Touching at: " + touch.position);

                    if (touch.phase == TouchPhase.Began)
                    {
                        //Debug.Log("Touch phase began at: " + touch.position);
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        //Debug.Log("Touch phase Moved");
                        harnessManager.CurrentHarness.transform.Rotate(touch.deltaPosition.y * rotationRate,
                                         -touch.deltaPosition.x * rotationRate, 0, Space.World);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (SceneManag.Instance.currentProductCatagory == ProductCatagory.BODY)
                        {
                            harnessManager.CurrentHarness.transform.rotation = Quaternion.Euler(90, 180, 0);
                        }
                        else
                        {
                            harnessManager.CurrentHarness.transform.rotation = Quaternion.identity;

                        }
                        //Debug.Log("Touch phase Ended");
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.touchCount == 0 && harnessManager.CurrentHarness != null )
        {
            if (SceneManag.Instance._currentProductSubCatagory == ProductSubCatagory.EYEGLASS)
            {
            harnessManager.CurrentHarness.transform.Rotate(Vector3.up * rotationspeed * Time.deltaTime);

            }
            if ( SceneManag.Instance.currentProductCatagory == ProductCatagory.BODY)
            {
                harnessManager.CurrentHarness.transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
            }
            if (SceneManag.Instance._currentProductSubCatagory == ProductSubCatagory.HELMET)
            {
                ScaleUpDownRotat();
            }
        }

        
    }


   

    public Vector3 endPoint;   // Reference to the end position
    public Vector3 initialPosition; // Store the initial position of the object

    public float speed = 1.0f;   // Speed of movement
    public float scaleDuration = 1.0f;   // Duration for scaling down
    public float rotationDuration = 2.0f; // Duration for rotation
    public float movementSpeed = 1.0f;   // Speed of movement

    private float startTime;     // Time when the movement started
    
    private bool movementComplete = false;
    private bool scalingComplete = false;
    private bool rotationComplete = false;
    private bool stopHelmetMotion;
    private Vector3 initialScale; // Store the initial scale of the object
    private Quaternion initialRotation; // Store the initial rotation of the object




    void ScaleUpDownRotat()
    {
        if (harnessManager.CurrentHarness == null)
        {
            return;
        }
        if (!movementComplete)
        {
            // Calculate the time elapsed since movement started
            float timeElapsed = Time.time - startTime;

            // Calculate the interpolation factor based on time and speed
            float interpolationFactor =  Mathf.Clamp01(timeElapsed / speed);

            // Use Vector3.Lerp to interpolate between start and end positions
            harnessManager.CurrentHarness.helmetShel.transform.localPosition = Vector3.Lerp(initialPosition, endPoint, interpolationFactor);

            // If interpolationFactor reaches 1, movement is complete
            if (interpolationFactor >= 1.0f)
            {
                movementComplete = true;
                startTime = Time.time; // Reset start time for scaling down
            }
        }
        else if (!scalingComplete)
        {
            // Calculate the time elapsed since scaling down started
            float scaleTimeElapsed = Time.time - startTime;

            // Calculate the scale interpolation factor based on time and scale duration
            float scaleInterpolationFactor = Mathf.Clamp01(scaleTimeElapsed / scaleDuration);

            // Use Vector3.Lerp to interpolate between original scale and zero scale
            harnessManager.CurrentHarness.transform.localScale = Vector3.Lerp(initialScale, initialScale/2, scaleInterpolationFactor);

            // If scaleInterpolationFactor reaches 1, scaling down is complete
            if (scaleInterpolationFactor >= 1.0f)
            {
                // Optionally, you can perform actions here once scaling is done
                scalingComplete = true;
                startTime = Time.time; // Reset start time for rotation
            }
        }
        else if (!rotationComplete)
        {
            // Rotation logic
            //float rotationTimeElapsed = Time.time - startTime;
            //// Calculate the rotation interpolation factor based on time and rotation duration
            //float rotationInterpolationFactor = Mathf.Clamp01(rotationTimeElapsed / rotationDuration);

            //// Calculate the target rotation (360 degrees on the Y-axis)
            //Quaternion targetRotation = Quaternion.Euler(0f, 359, 0f);

            // Use Quaternion.Slerp to interpolate between current rotation and target rotation
            //harnessManager.CurrentHarness.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationInterpolationFactor);
            harnessManager.CurrentHarness.transform.Rotate(Vector3.up * rotationspeed * Time.deltaTime);

            // If rotationInterpolationFactor reaches 1, rotation is complete
            //if (rotationInterpolationFactor >= 1.0f)
            //Debug.Log(harnessManager.CurrentHarness.transform.eulerAngles.y);
            if (harnessManager.CurrentHarness.transform.eulerAngles.y >= 355)
            {
                rotationComplete = true;
                var helmetrot = harnessManager.CurrentHarness.transform.rotation;
                harnessManager.CurrentHarness.transform.rotation = Quaternion.Euler(helmetrot.x, 360, helmetrot.z);
                startTime = Time.time; // Reset start time 

                // Optionally, you can perform actions here once rotation is done
            }
        }
        else
        {
            if (stopHelmetMotion)
            {
                return;
            }
            // Lerping back to initial position and scaling up
            float lerpBackTimeElapsed = Time.time - startTime;

            // Calculate the lerp interpolation factor based on time and movement speed
            float lerpBackInterpolationFactor = Mathf.Clamp01(lerpBackTimeElapsed / 3);

            // Use Vector3.Lerp to interpolate between current position and initial position
            harnessManager.CurrentHarness.helmetShel.transform.localPosition = Vector3.Lerp(endPoint, initialPosition, lerpBackInterpolationFactor);

            // Use Vector3.Lerp to interpolate between current scale and initial scale
            harnessManager.CurrentHarness.transform.localScale = Vector3.Lerp(initialScale/2, initialScale, lerpBackInterpolationFactor);

            // If lerpBackInterpolationFactor reaches 1, lerping back is complete
            if (lerpBackInterpolationFactor >= 1.0f)
            {
                movementComplete = scalingComplete = rotationComplete = false;
                startTime = Time.time;
                // Optionally, you can perform actions here once lerping back is done
            }
        }
    }

    // Method to reset the object's behavior
    void ResetDataObject()
    {
        

        initialScale = harnessManager.CurrentHarness.transform.localScale; // Store initial scale
        //initialPosition = harnessManager.CurrentHarness.helmetShel.transform.position; // Store initial position
        initialRotation = harnessManager.CurrentHarness.transform.rotation; // Store initial rotation
    }

    public void ResetAllTransFormCompoenet(bool restartProcess)
    {
        startTime = Time.time;   // Reset start time
        movementComplete = restartProcess;
        scalingComplete = restartProcess;
        rotationComplete = restartProcess;
        stopHelmetMotion = restartProcess;

        harnessManager.CurrentHarness.transform.localScale = initialScale; // Store initial scale
        harnessManager.CurrentHarness.helmetShel.transform.localPosition = initialPosition; // Store initial position
        harnessManager.CurrentHarness.transform.rotation = initialRotation; // Store initial rotation
    }

}