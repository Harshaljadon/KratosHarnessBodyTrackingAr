using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System.Collections.Generic;
using System.Collections;

public class GenerateQR : MonoBehaviour
{
    public RawImage qrImage, uploadedimageQr;
    public GameObject uploadedimageQrClose;
    [Tooltip("True to save QR")]
    public bool saveQr;
    public string url, imageUrl;

    HarnessMasserManagerUI harnessMasserManagerUI;
    public HarnessCaryForwardData harnessCaryForwardData;
    public SnapShootManager _SnapShootManager;
    private void Start()
    {
        harnessMasserManagerUI = FindObjectOfType<HarnessMasserManagerUI>();
        _SnapShootManager = FindObjectOfType<SnapShootManager>();
  
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    Texture2D GenerateQRTexture(string text, int width = 256, int height = 256)
    {
        var encoded = new Texture2D(width, height);
        var color32 = Encode(text, width, height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    /// <summary>
    /// Generate qr with URL
    /// </summary>
    public void OnGenerateQRButtonClick()
    {
        if (harnessMasserManagerUI != null)
        {
            harnessMasserManagerUI.ProductCodeUrl();
            url = harnessMasserManagerUI.productCodeData.wrokStationDetail.currentUrl;
            //Debug.Log(url);
        }
        Texture2D myQR = GenerateQRTexture(url);
        qrImage.texture = myQR;
        if (saveQr)
        {
        SaveQrCpde(myQR);

        }

    }

    /// <summary>
    /// Generate qr with URL to access uploaded image on server
    /// </summary>
    public void UploadImgeOnGenerateQRUrl(string myUrl)
    {

        //Invoke(nameof(GetUrlUpdate), 1);
        imageUrl = myUrl;
        StartCoroutine(StronglyGetUrl());

        
    }

    IEnumerator StronglyGetUrl()
    {
        //imageUrl = _SnapShootManager.imageUrl;
        ////Debug.Log(_SnapShootManager.imageUrl);
        //imageUrl = harnessCaryForwardData.imageUploadUrlQr;
        ////Debug.Log(harnessCaryForwardData.imageUploadUrlQr);
        //imageUrl = _SnapShootManager.qrImageUploadRespone.qrString;
        //Debug.Log(_SnapShootManager.qrImageUploadRespone.qrString);
        if (imageUrl == string.Empty)
        {
            StopCoroutine(StronglyGetUrl());
        }
        Texture2D myQR = GenerateQRTexture(imageUrl);
        uploadedimageQr.texture = myQR;
        yield return new WaitForSeconds(4f);
        uploadedimageQr.gameObject.SetActive(true);
        uploadedimageQrClose.gameObject.SetActive(true);
        //yield return new WaitForSeconds(1);
        if (saveQr)
        {
            SaveQrCpde(myQR);

        }

    }

    void SaveQrCpde(Texture2D qr)
    {
        // Save the QR code as an image file (PNG)
        byte[] bytes = qr.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/QRCode.png", bytes);
        Debug.Log("QR Code saved as QRCode.png");
    }
}
