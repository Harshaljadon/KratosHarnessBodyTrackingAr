using UnityEngine;
using UnityEngine.Events;

public class TriggerEventUnderRadius : MonoBehaviour
{
    public UnityEvent underRangeEvent, outOfRangeEvevnt;
    [SerializeField]
    float underRange = 1.5f;
    [SerializeField]
    float outRange = 1.5f;
    DistBetObjAndCam dBOC;
    UnderRangeStates uRS;
    bool eventInvokes;
    // Start is called before the first frame update
    void Start()
    {
        dBOC = GetComponent<DistBetObjAndCam>();
        uRS = GetComponent<UnderRangeStates>();
        uRS.enableScript += URS_enableScript;
        eventInvokes = true;
    }

    private void URS_enableScript()
    {
        if (!dBOC.enabled)
        {

        dBOC.enabled = true;
        eventInvokes = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dBOC.Distance <= underRange && !eventInvokes )
        {
            underRangeEvent?.Invoke();
            eventInvokes = true;
        }
        if(dBOC.Distance > outRange && eventInvokes)
        {
            eventInvokes = false;
            outOfRangeEvevnt?.Invoke();
            dBOC.enabled = false;
        }
    }

    public void EventInvoke()
    {
        eventInvokes = true;
    }
}


