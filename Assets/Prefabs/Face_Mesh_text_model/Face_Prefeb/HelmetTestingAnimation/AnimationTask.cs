using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string> { };

public class AnimationTask : MonoBehaviour
{
    public UnityAnimationEvent OnAnimationComplete;

    public Animator myAnime;
    ProductTestingManager productTestingManager;
    public AnimationName_Parameter _animationName_Parameter;
    // Start is called before the first frame update
    void Start()
    {
        myAnime = GetComponent<Animator>();
        productTestingManager = FindObjectOfType<ProductTestingManager>();
    }


    public void PerformTask(string AnimationParameter_NamePeform, bool condition)
    {
        //myAnime.Play(0);
        myAnime.SetBool(AnimationParameter_NamePeform, condition);
        CreateEvent(AnimationParameter_NamePeform);
    }

    void CreateEvent(string snimationParamenter)
    {
        for (int i = 0; i < myAnime.runtimeAnimatorController.animationClips.Length; i++)
        {

            AnimationClip clip = myAnime.runtimeAnimatorController.animationClips[i];
            if (clip.events.Length < myAnime.runtimeAnimatorController.animationClips.Length)
            {
                AnimationEvent animationStartEvent = new AnimationEvent();
                animationStartEvent.time = clip.length;
                animationStartEvent.functionName = "AnimationEndHandler";
                //_animationName_Parameter.animeName = clip.name;
                //_animationName_Parameter.parameterName = snimationParamenter;
                //animationStartEvent.stringParameter = clip.name;
                animationStartEvent.stringParameter = snimationParamenter;
                //Debug.Log(clip.name);
                clip.AddEvent(animationStartEvent);
            }

        }

            

    }


    public void AnimationEndHandler(string name)
    {
        
        //Debug.Log($"{name} animation start.");
        productTestingManager.AnimationCompleteCallBack(name);
        //OnAnimationComplete?.Invoke(name);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class AnimationName_Parameter
{
    public string animeName;
    public string parameterName;
}
