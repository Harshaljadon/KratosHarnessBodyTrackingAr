using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class EventOccurAtcertanGap : MonoBehaviour
{
     
    public UnityEvent performAtCertaingap;
    public RectTransform target1, target2;
    public float gapRequired, dist;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dist = Vector2.Distance(target1.position, target2.position);
        if (dist < gapRequired)
        {
            performAtCertaingap?.Invoke();
        }
    }

    public void CloseQrInvokeEventLefthandRise()
    {
            performAtCertaingap?.Invoke();

    }
}
