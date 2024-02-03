using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class FilterHarnessManagerQuestion : MonoBehaviour
{
    public event Action harnessfilterationAction, restartHranessQuestionProcess;
    [SerializeField]
    HarnessManager _harnessManager;
    public RectTransform[] questionPanel123;
    public QuestionEnum qE;
    public TextMeshProUGUI questionTitle;
    public string[] question;
    public int currentQuestionNumberIndex, updatedIndexNumber, totalQuestion;
    public GameObject Skipbutton, questionBgPanel;
    
    private void Start()
    {
        totalQuestion = questionPanel123.Length;

        QuestionFadeOUTInstant(0);
        Skipbutton.SetActive(false);
        questionTitle.gameObject.SetActive(false);
        questionBgPanel.gameObject.SetActive(false);
    }

    public void RestartQuestioning()
    {
        //leftButtonImage.GetComponent<Button>().interactable = false;
        //rightButtonImage.GetComponent<Button>().interactable = false;
        //leftButtonImage.DOFade(0, 0.5f);
        //rightButtonImage.DOFade(0, 0.5f);
        Invoke(nameof(InvokeResetQuestionInstant), 1);
        restartHranessQuestionProcess.Invoke();
    }

    void InvokeResetQuestionInstant()
    {
        questionTitle.gameObject.SetActive(true);
        questionBgPanel.gameObject.SetActive(true);
        Skipbutton.SetActive(true);
        QuestionFadeIN_Instant(0);

    }

    public void Ques1Ans(int ansIndex)
    {
        qE.ans1 = (Ques1)ansIndex;
        currentQuestionNumberIndex = 0;

        QuestionfadingOUt(currentQuestionNumberIndex);


    }
    public void Ques2Ans(int ans2Index)
    {
        qE.ans2 = (Ques2)ans2Index;
        currentQuestionNumberIndex = 1;
        QuestionfadingOUt(currentQuestionNumberIndex);

    }
    public void Ques3Ans(int ans3Index)
    {
        qE.ans3 = (Ques3)ans3Index;
        currentQuestionNumberIndex = 2;
        QuestionfadingOUt(currentQuestionNumberIndex);

    }

    


    public void QuestionfadingOUt(int index)
    {

        var buttonInterectioncoll = questionPanel123[index].GetComponentsInChildren<Button>();

        foreach (var button in buttonInterectioncoll)
        {
            button.interactable = false;
        }

        var imageColl = questionPanel123[index].GetComponentsInChildren<Image>();
        var textColl = questionPanel123[index].GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var imageChild in imageColl)
        {
            imageChild.DOFade(0, 1);
        }
        foreach (var textChild in textColl)
        {
            textChild.DOFade(0, 1);
        }
        questionTitle.DOFade(0, 1);
        questionPanel123[index].GetComponent<Image>().DOFade(0, 1);
        Invoke(nameof(nextStepProceed), 1.5f);
    }

    void nextStepProceed()
    {
        questionPanel123[currentQuestionNumberIndex].gameObject.SetActive(false);
        NextQuestionOrHarnessSelection(currentQuestionNumberIndex);
    }


    void NextQuestionOrHarnessSelection(int indexValue)
    {
        updatedIndexNumber = indexValue + 1;
        if (totalQuestion <= updatedIndexNumber)
        {
            Skipbutton.SetActive(false);
            questionTitle.gameObject.SetActive(false);
            questionBgPanel.gameObject.SetActive(false);


            harnessfilterationAction.Invoke();
            return;
        }

        NextQuestionFadingIn(updatedIndexNumber);
    }


    public void NextQuestionFadingIn(int index)
    {
        var imageColl = questionPanel123[index].GetComponentsInChildren<Image>();
        var textColl = questionPanel123[index].GetComponentsInChildren<TextMeshProUGUI>();

        questionTitle.text = question[index];
        questionTitle.DOFade(1, 1);
        foreach (var imageChild in imageColl)
        {
            imageChild.DOFade(0, 0);
        }
        foreach (var textChild in textColl)
        {
            textChild.DOFade(0, 0);
        }
        questionPanel123[index].gameObject.SetActive(true);
        var buttonInterectioncoll = questionPanel123[index].GetComponentsInChildren<Button>();

        foreach (var button in buttonInterectioncoll)
        {
            button.interactable = true;
        }


        foreach (var imageChild in imageColl)
        {
            imageChild.DOFade(1, 1);
        }
        foreach (var textChild in textColl)
        {
            textChild.DOFade(1, 1);
        }
    }


    public void BackQuestion(int indexPreviousQues)
    {

        QuestionFadeIN_Instant(indexPreviousQues);
        QuestionFadeOUTInstant(indexPreviousQues +1);
    }

    public void QuestionFadeIN_Instant(int index)
    {


        //_Ques1_2_3Panel.DOFade(1, 0);

        questionPanel123[index].gameObject.SetActive(true);
        questionPanel123[index].GetComponent<Image>().DOFade(1, 0);
        var imageColl = questionPanel123[index].GetComponentsInChildren<Image>();
        var textColl = questionPanel123[index].GetComponentsInChildren<TextMeshProUGUI>();
        var buttonInterectioncoll = questionPanel123[index].GetComponentsInChildren<Button>();
        questionTitle.text = question[index];
        questionTitle.DOFade(1, 0);
        foreach (var button in buttonInterectioncoll)
        {
            button.interactable = true;
        }
        foreach (var imageChild in imageColl)
        {
            imageChild.DOFade(1, 0);
        }
        foreach (var textChild in textColl)
        {
            textChild.DOFade(1, 0);
        }
    }

    public void QuestionFadeOUTInstant(int index)
    {
        var imageColl = questionPanel123[index].GetComponentsInChildren<Image>();
        var textColl = questionPanel123[index].GetComponentsInChildren<TextMeshProUGUI>();
        var buttonInterectioncoll = questionPanel123[index].GetComponentsInChildren<Button>();

        foreach (var button in buttonInterectioncoll)
        {
            button.interactable = true;
        }
        foreach (var imageChild in imageColl)
        {
            imageChild.DOFade(0, 0);
        }
        foreach (var textChild in textColl)
        {
            textChild.DOFade(0, 0);
        }
        //questionTitle.text = question[index];
        //questionTitle.DOFade(0, 0);
        //_Ques1_2_3Panel.DOFade(0, 0);
        questionPanel123[index].GetComponent<Image>().DOFade(0, 0);
        questionPanel123[index].gameObject.SetActive(false);
    }
}



[System.Serializable]
public class QuestionEnum
{
    public Ques1 ans1;
    public Ques2 ans2;
    public Ques3 ans3;
}

[System.Serializable]
public enum Ques1
{
    fall_arrest_harness,
    fall_arrest_harness_with_work_position,
    suspension_harness
}
[System.Serializable]
public enum Ques2
{
    Intensive_use,
    frequent_use,
    regular_use,
    Occasional_use
}
[System.Serializable]
public enum Ques3
{
    tool_accessories,
    park_lanyards,
    Rescue_points,
    dorsal_restraint_point_on_belt,
}