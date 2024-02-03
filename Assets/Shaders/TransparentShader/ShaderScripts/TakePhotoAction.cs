using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotoAction : MonoBehaviour
{
    public bool snapShotProcessing;
    public SnapShootManager snapShootManager;
    public EventOccurAtcertanGap eventOccurAtcertanGap;
    //public GenerateQR generateQR;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RightHand") && !snapShotProcessing) 
        {
            //generateQR.OnGenerateQRButtonClick();
            snapShotProcessing = true;
            Invoke(nameof(WaitProcess),8);
            snapShootManager.TakeSnapShot();
            //harnessMasserManagerUI.QrBarCodeGuideAnimeScaleUpDown();
            //Debug.Log("rightHand");
        }

        if (other.gameObject.CompareTag("LeftHand") )
        {
            eventOccurAtcertanGap.CloseQrInvokeEventLefthandRise();
        }
    }

    public void WaitProcess()
    {
        snapShotProcessing = false;
    }
}
