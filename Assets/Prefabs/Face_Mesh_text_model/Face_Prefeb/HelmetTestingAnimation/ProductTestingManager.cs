using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ProductTestingManager : MonoBehaviour
{


    public Animator currecntProductitemonScreen;
    public ActivityToolToTest[] activityToolToTests;
    HarnessManager harnessManager;
    UiManager uiManager;
    private void OnEnable()
    {
        harnessManager = FindObjectOfType<HarnessManager>();
        uiManager = FindObjectOfType<UiManager>();
        // get riference 
        harnessManager.testingActivityEvent += HarnessManager_testingActivityEvent;
        //execute
        uiManager.triggerTestingEvent += UiManager_triggerTestingEvent;
    }


    int buttonIndexPressed;
    private void UiManager_triggerTestingEvent(string arg1, int arg2)
    {
        if (this.transform.childCount != 0)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
        buttonIndexPressed = arg2;
        foreach (var item in activityToolToTests)
        {
            if (item.productSubCatagory == SceneManag.Instance._currentProductSubCatagory)
            {
                var TargetFortest = item.tooldatas[buttonIndexPressed].ToolAndTarget[0];
                item.tooldatas[buttonIndexPressed].ToolAndTarget[0].objectToControlAnimation = currecntProductitemonScreen.transform.GetComponent<Animator>();
                currecntProductitemonScreen.gameObject.GetComponent<AnimationTask>().PerformTask(TargetFortest.animationNamenadParamenter[0].nameParameter, TargetFortest.animationNamenadParamenter[0].trueFaleCondidtion);

                //foreach (var datacollection in item.tooldatas[buttonIndexPressed].ToolAndTarget)
                //{
                //    var size = datacollection.animationNamenadParamenter.Length;
                //    for (int i = 0; i < size; i++)
                //    {
                //        datacollection.animationNamenadParamenter[i].TaskComplete = false;

                //    }
                //}
                var sizeoftoolCollection = item.tooldatas[buttonIndexPressed].tool.Length;
                for (int i = 0; i < sizeoftoolCollection; i++)
                {
                    var toolCreate = Instantiate(item.tooldatas[buttonIndexPressed].tool[i], this.transform);
                    toolCreate.transform.localScale = Vector3.zero;
                    item.tooldatas[buttonIndexPressed].ToolAndTarget[i + 1].objectToControlAnimation = toolCreate.GetComponent<Animator>();

                    var tool = item.tooldatas[buttonIndexPressed].ToolAndTarget[i + 1];
                    toolCreate.GetComponent<AnimationTask>().PerformTask(tool.animationNamenadParamenter[0].nameParameter, tool.animationNamenadParamenter[0].trueFaleCondidtion);

                }


                
                break;
            }
        }

    }

    public int currentIndex;

    public void AnimationCompleteCallBack(string callBackAnimationNameParameter)
    {
        foreach (var item in activityToolToTests)
        {
            if (item.productSubCatagory == SceneManag.Instance._currentProductSubCatagory)  //helmet
            {
                foreach (var itemData in item.tooldatas[buttonIndexPressed].ToolAndTarget)
                {

                    var size = itemData.animationNamenadParamenter.Length;
                    for (int i = 0; i < size; i++)
                    {
                        if (itemData.animationNamenadParamenter[i].nameParameter == callBackAnimationNameParameter)
                        {
                            Debug.Log(callBackAnimationNameParameter+" " + i);
                            //currentIndex = i;
                            if (!itemData.animationNamenadParamenter[i].TaskComplete)
                            {
                            itemData.animationNamenadParamenter[i].TaskComplete = true;
                            itemData.animationNamenadParamenter[i].callOnAfterAnimationComlete?.Invoke();

                            break;
                            }
                        }
                    }

                    //foreach (var itemEventInvoke in itemData.animationNamenadParamenter)
                    //{
                    //    if (itemEventInvoke.nameParameter == callBackAnimationNameParameter)
                    //    {
                    //        //itemEventInvoke.callOnAfterAnimationComlete.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                    //        //itemEventInvoke.callOnAfterAnimationComlete?.Invoke();
                    //        break;
                    //    }
                    //}
                    
                }
                break;
            }
        }
    }

    public void ResetAlltaskCompletionStat()
    {
        StartCoroutine(ResetTaskCompleteion());
    }

    IEnumerator ResetTaskCompleteion()
    {
        yield return new WaitForSeconds(.5f);
        foreach (var item in activityToolToTests)
        {
            if (item.productSubCatagory == SceneManag.Instance._currentProductSubCatagory)  //helmet
            {
                foreach (var itemData in item.tooldatas[buttonIndexPressed].ToolAndTarget)
                {
                    var size = itemData.animationNamenadParamenter.Length;
                    for (int i = 0; i < size; i++)
                    {
                        itemData.animationNamenadParamenter[i].TaskComplete = false;

                    }
                }
            }
        }
    }

    public void SeriesOfAnimation(string objectName)
    {
        switch (SceneManag.Instance._currentProductSubCatagory)
        {
            case ProductSubCatagory.HELMET:

                foreach (var item in activityToolToTests[0].tooldatas[buttonIndexPressed].ToolAndTarget)
                {
                    if (item.objectName == objectName)
                    {
                        int sizeParameter = item.animationNamenadParamenter.Length;
                        for (int i = 0; i < sizeParameter; i++)
                        {
                            if (!item.animationNamenadParamenter[i].TaskComplete)
                            {
                                //item.animationNamenadParamenter[i].TaskComplete = true;
                                var taskRef = item.objectToControlAnimation.gameObject.GetComponent<AnimationTask>();
                                taskRef.PerformTask(item.animationNamenadParamenter[i].nameParameter, item.animationNamenadParamenter[i].trueFaleCondidtion);
                            Debug.Log("objectName= " + objectName + " parameterName = " + item.animationNamenadParamenter[i].nameParameter
                                + " parameterTypeCondition = " + item.animationNamenadParamenter[i].trueFaleCondidtion);

                                break;
                            }
                        }
                        break;
                        //int nextIndex = currentIndex + 1;
                            //Debug.Log("sizeParameter= " + sizeParameter + " nextIndex = " + nextIndex + " objectName = " + objectName );
                        //if (nextIndex < sizeParameter)
                        //{
                        //    var parameterName = item.animationNamenadParamenter[nextIndex].nameParameter;
                        //    var parameterTypeCondition = item.animationNamenadParamenter[nextIndex].trueFaleCondidtion;
                        //    taskRef.PerformTask(parameterName, parameterTypeCondition);
                        //    break;
                        //}
                    }
                }



                //var collection = activityToolToTests[0].tooldatas[buttonIndexPressed].ToolAndTarget;
                //int size = collection.Length;
                //for (int i = 0; i < size; i++)
                //{
                //    if (collection[i].objectName == objectName)
                //    {
                //        collection[i].eventCounts++;
                //        var indexCount = collection[i].eventCounts;
                //        var parameterName = collection[i].animationNamenadParamenter[indexCount].nameParameter;
                //        var parameterTypeCondition = collection[i].animationNamenadParamenter[indexCount].trueFaleCondidtion;
                //        Debug.Log("objectName= "+ objectName + " count = " +collection[i].eventCounts + " parameterName = " + parameterName + " parameterTypeCondition = " + parameterTypeCondition);
                //        var taskRef = collection[i].objectToControlAnimation.gameObject.GetComponent<AnimationTask>();
                //        taskRef.PerformTask(parameterName, parameterTypeCondition);
                //        break;
                //    }
                //}
                break;
            default:
                break;
        }
    }


    private void OnDisable()
    {
        harnessManager.testingActivityEvent -= HarnessManager_testingActivityEvent;
        uiManager.triggerTestingEvent -= UiManager_triggerTestingEvent;


    }
    private void HarnessManager_testingActivityEvent()
    {
        currecntProductitemonScreen = harnessManager.CurrentHarness.testingActivityAnimatorController;
    }

    void StartActivityProcess()
    {

    }
}

[System.Serializable]
public class ActivityToolToTest
{
    public ProductSubCatagory productSubCatagory;
    public Tooldata[] tooldatas;
    //public RuntimeAnimatorController animeContol;
}

[System.Serializable]
public struct Tooldata
{
    public string activityname;
    public GameObject[] tool;
    public ToolAndTarget[] ToolAndTarget;
    //public UnityAnimationEvent OnAnimationStart;
}

[System.Serializable]
public struct ToolAndTarget
{
    public string objectName;
    public int eventCounts;
    public Animator objectToControlAnimation;
    public AnimationNamenadParamenter[] animationNamenadParamenter;
}



[System.Serializable]
public struct AnimationNamenadParamenter
{

    public string nameParameter;
    public bool trueFaleCondidtion;
    public bool TaskComplete;
    public UnityEvent callOnAfterAnimationComlete;
}


[System.Serializable]
public class SeriesAnimationEventContoll : UnityEvent<string, int> { };