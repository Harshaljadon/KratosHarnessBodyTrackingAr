using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiClickIdentify : MonoBehaviour
{
    IdHolder idH;
    IdCompare idC;
    RaycastUtilities rU;
    [SerializeField][HideInInspector]
    Vector2 myTouchPosOnScreen;
    [SerializeField]
    GameObject holder;
    [SerializeField]
    GameObject guidePanel;

    // Start is called before the first frame update
    void Start()
    {
        rU = new RaycastUtilities();
        idH = GetComponent<IdHolder>();
        idC = new IdCompare(idH, holder);
    }
    [SerializeField][HideInInspector]
    int idGet;
    Touch userTouch;
    [SerializeField] [HideInInspector]
    float timeTouchEnded;
    [SerializeField] [HideInInspector]
    float timeDiff = .04f;
    bool userClickRes, userMovingFinger, backClick;
    [SerializeField]
    GameObject anchorTextUi;
    [SerializeField]
    GameObject bracketTextUi;
    [SerializeField]
    GameObject wrinchTextUi;
    [SerializeField]
    GameObject srlTextUi;
    // Update is called once per frame
    void Update()
    {
        #region userTouchResponse
        if (Input.touchCount > 0)
        {
            userTouch = Input.GetTouch(0);
            if (userTouch.phase == TouchPhase.Began)
            {
                timeTouchEnded = Time.time;
                userMovingFinger = false;
            }
            if (userTouch.phase == TouchPhase.Moved)
            {
                userMovingFinger = true;
            }

            if (userTouch.phase == TouchPhase.Ended && userMovingFinger == false && !backClick)
            {
                if (Time.time - timeTouchEnded > timeDiff )
                {
                    myTouchPosOnScreen = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                    userMovingFinger = false;
                    userClickRes = true;
                    if (rU.PointerIsOverUI(myTouchPosOnScreen) && userClickRes)
                    {
                        userClickRes = false;
                        idGet = rU.firstHitObj.GetInstanceID();
                        if (idC.IdComparation(idGet) )
                        {
                            guidePanel.SetActive(true);
                            byte condition = idC.caseMatch;
                            switch (condition)
                            {
                                case 0:
                                    anchorTextUi.SetActive(true);
                                    bracketTextUi.SetActive(false);
                                    wrinchTextUi.SetActive(false);
                                    srlTextUi.SetActive(false);
                                    break;
                                case 1:
                                    anchorTextUi.SetActive(false);
                                    bracketTextUi.SetActive(true);
                                    wrinchTextUi.SetActive(false);
                                    srlTextUi.SetActive(false);
                                    break;
                                case 2:
                                    anchorTextUi.SetActive(false);
                                    bracketTextUi.SetActive(false);
                                    wrinchTextUi.SetActive(true);
                                    srlTextUi.SetActive(false);
                                    break;
                                case 3:
                                    anchorTextUi.SetActive(false);
                                    bracketTextUi.SetActive(false);
                                    wrinchTextUi.SetActive(false);
                                    srlTextUi.SetActive(true);
                                    break;
   

                            }
                        }
                        else
                        {
                            //Debug.Log("click");
                            guidePanel.SetActive(false);
                            anchorTextUi.SetActive(false);
                            bracketTextUi.SetActive(false);
                            wrinchTextUi.SetActive(false);
                            srlTextUi.SetActive(false);

                        }
                    }
                }
            }
            else
            {
                backClick = false;
            }
        }
        #endregion


    }
    // item panel back button have refernce of this method
    public void GettingCloseThePanel()
    {
        backClick = true;
    }

    public void EnteringArModeCloseAllGuidePanel()
    {
        guidePanel.SetActive(false);
        anchorTextUi.SetActive(false);
        bracketTextUi.SetActive(false);
        wrinchTextUi.SetActive(false);
        srlTextUi.SetActive(false);
    }
}




#region raycastUti
/// <summary>
/// hover over ui to get game object 
/// </summary>
public class RaycastUtilities
{
    public GameObject firstHitObj;
    public bool PointerIsOverUI(Vector2 screenPos)
    {
        firstHitObj = UIRaycast(ScreenPosToPointerData(screenPos));
        return firstHitObj != null && firstHitObj.layer == LayerMask.NameToLayer("UI");
    }

    public GameObject UIRaycast(PointerEventData pointerData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count < 1 ? null : results[0].gameObject;
    }

    PointerEventData ScreenPosToPointerData(Vector2 screenPos)
    {
        return new(EventSystem.current) { position = screenPos };
    }

    // or 
    //PointerEventData ScreenPosToPointerData(Vector2 screenPos)
    //=> new(EventSystem.current) { position = screenPos };
}
#endregion