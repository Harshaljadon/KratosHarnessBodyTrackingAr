using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System;
using System.IO;
using UnityEngine.Networking;
using RestSharp;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public class SnapShootManager : MonoBehaviour
{
    bool permitionGranted, doNotAskAgain, isPicturePermission = true;
    public GameObject OpenSettingButton,permissionPanelGuide,requestPermisionButton,BallsParent, snapShotPanel, SnapShotAnimator; //screemShotBackButton
	public TextMeshProUGUI tmp, count;
	//public TMP_InputField mailId;ffff
	public float farAway;

	float lerpedValue = 0f, targetValue = 1f, duration = 5f;
	int repeatCount = 5, ssCounter = 0;
	public string savePsth, emailId = "mohd@karam.in", imageUrl;

	public AudioSource audioSource;
	public AudioClip guideVoice;

	public Image fillRoundCount;


	[SerializeField]
	GameObject[] uiHideUndie;

	Texture2D ss;
	byte[] imageData;

	[SerializeField]
	RawImage rI;

	public string urlUploadImage, uploadImageUrlgenrate = "https://app.arresto.in/#/ar_media?c=250&media="; // https://app.arresto.in/#/ar_media?c=250&media=652e65f051970.png&p=423
	public QrImageUploadRespone qrImageUploadRespone = new QrImageUploadRespone();

	public HarnessCaryForwardData harnessCaryForwardData;
	public GenerateQR generateQR;
	public HarnessMasserManagerUI harnessMasserManagerUI;

	public HashSet<int> usedIds = new HashSet<int>();


	private void Awake()
    {
		if (NativeCamera.DeviceHasCamera())
			return;
        NativeCamera.Permission a = NativeCamera.CheckPermission(isPicturePermission);
        switch (a)
        {
            case NativeCamera.Permission.Granted:
                permitionGranted = true;
                break;
            case NativeCamera.Permission.Denied:
                if (NativeCamera.CanOpenSettings())
                {
					// button appear to user response to Grand permission setting with why persion required
                    permissionPanelGuide.SetActive(true);
					tmp.text = "Please click on this button to open phone setting to set camera permission for this app";
                    OpenSettingButton.SetActive(true);
					OpenSettingButton.GetComponent<Button>().onClick.AddListener(() => { OpenPhoneSettinToGrandPermissinApp(); });
					requestPermisionButton.SetActive(false);

				}
				else
                {
					tmp.text = "To take snapshot work enable th camera permission for this setting in phone setting";
                    permissionPanelGuide.SetActive(true);
					OpenSettingButton.SetActive(false);
					requestPermisionButton.SetActive(false);
					// Text panel to guide how to Grant permission

				}
                break;
            case NativeCamera.Permission.ShouldAsk:
				// button to user decide grand permission or not
                    permissionPanelGuide.SetActive(true);
					tmp.text = "Please grant permission to access camera for snapshot";
				OpenSettingButton.SetActive(false);
				requestPermisionButton.SetActive(true);
				requestPermisionButton.GetComponent<Button>().onClick.AddListener(() => { RequestPermissionToAccessPhoneSetting(); });

				break;
            default:
                break;
        }
    }

	void SendOldData()
    {
		//if (harnessCaryForwardData.prevouslyData.productUrl == string.Empty || harnessCaryForwardData.prevouslyData.fileStr == null ||
		//	harnessCaryForwardData.prevouslyData.fileName == string.Empty || harnessCaryForwardData.prevouslyData.puId == string.Empty)
		//	return;
		RestClient client = new RestClient(urlUploadImage); //https://api.arresto.in/api/client/250/ar/media_uploads
		RestRequest request = new RestRequest(Method.POST);

		//string TextureArray = Convert.ToBase64String(harnessCaryForwardData.prevouslyData.fileStr);


		//request.AddParameter("productUrl", harnessCaryForwardData.prevouslyData.productUrl);
		//request.AddParameter("file_str", TextureArray);
		//request.AddParameter("filename", harnessCaryForwardData.prevouslyData.fileName);
		//request.AddParameter("puid", harnessCaryForwardData.prevouslyData.puId);

		client.ExecuteAsync(request, response =>
		{
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				////Debug.Log(response.Content);
				//qrImageUploadRespone = JsonUtility.FromJson<QrImageUploadRespone>(response.Content);
				////Debug.Log("qrImageUploadRespone "+qrImageUploadRespone.qrString);
				////imageUrl = qrImageUploadRespone.qrString;
				////Debug.Log("imageUrl" + imageUrl);
				//harnessCaryForwardData.imageUploadUrlQr = qrImageUploadRespone.qrString;
				urlRecived = true;
				harnessMasserManagerUI.takingSnap = false;
				var empetyData = string.Empty;
				//harnessCaryForwardData.prevouslyData.productUrl = empetyData;
				//harnessCaryForwardData.prevouslyData.fileStr = new byte[0];
				//harnessCaryForwardData.prevouslyData.fileName = empetyData;
				//harnessCaryForwardData.prevouslyData.puId = empetyData;
				//SendEmail(emailId);
			}
			else
			{
				//harnessMasserManagerUI.takingSnap = true;
				Debug.LogError("Image upload failed: " + response.ErrorMessage);
			}
		});
	}


     void OpenPhoneSettinToGrandPermissinApp()
	 {

      NativeCamera.OpenSettings();
		OffPermissionPanelUI();

	 }

     void RequestPermissionToAccessPhoneSetting()
	 {

        NativeCamera.RequestPermission(true);
		OffPermissionPanelUI();

	 }

	void OffPermissionPanelUI()
    {
		permissionPanelGuide.SetActive(false);

	}


	public void TakeScreenShotwithDelay(float delay)
	{
		Invoke(nameof(TakeScreenShot), delay);
	}

	private void Start()
	{
		snapShotPanel.SetActive(false);

		if (PlayerPrefs.HasKey("ssCounter"))
		{
			ssCounter = PlayerPrefs.GetInt("scCounter");
		}
		else
		{
			PlayerPrefs.SetInt("scCounter", 0);
		}


		//SendOldData();

		//screemShotBackButton.GetComponent<Button>().onClick.AddListener(() => { ReturnFromScreemShot(); });
		//reportButton.GetComponent<Button>().onClick.AddListener(() => { ReportProcess_OpenMailBox(); });
		//string path = Application.persistentDataPath + "/" + "Harness_ScreenShot" + ssCounter + ".png";
		//ssCounter++;
		//PlayerPrefs.SetInt("scCounter", ssCounter);
		//ScreenCapture.CaptureScreenshot("Harness_ScreenShot" + ssCounter + ".png");
	}


	private void TakeScreenShot()
	{
		StartCoroutine(TakeScreenshotAndSave());
	}

	private IEnumerator TakeScreenshotAndSave()
	{
		// photo taking animation start
		audioSource.PlayOneShot(guideVoice);
        fillRoundCount.fillAmount = 0;
		snapShotPanel.SetActive(true);
		InvokeRepeating(nameof(ChangeNumTextInvokeRepeat), 1, 1);
		StartCoroutine(ChangeFloatOverTime());
		yield return new WaitForSeconds(5.2f);

        HideAllUI();
		BallsParent.SetActive(true);
		BallsParent.transform.localScale = new Vector3(1,1,1);
		snapShotPanel.SetActive(false);

		yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

		TempScremShotSend();

		//ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		//ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		//ss.Apply();

		//ApplyOnQuadPlane(ss);
		// set name
		//string name = "Harness_ScreenShot" + ssCounter + ".png";
		//ssCounter++;
		//PlayerPrefs.SetInt("scCounter", ssCounter);

		//// slash screem shot animation
		//SnapShotAnimator.SetActive(true);
		//      // Save the screenshot to Gallery/Photos
		//      NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "HarnessAR", name, (success, path) => {
		//	//if (success)
		//	//{
		//	//	Debug.Log("Image saved to gallery at path: " + path);
		//	//}
		//	//else
		//	//{
		//	//	Debug.Log("Failed to save image to gallery.");
		//	//}
		//} ); //savePsth = path
		//imageData = ss.EncodeToPNG();
		//savePsth = Path.Combine(Application.persistentDataPath, "TempScreenshot.png");
		//File.WriteAllBytes(savePsth, imageData);

		//// Create a Web Form
		//WWWForm form = new WWWForm();
		//form.AddField("frameCount", Time.frameCount.ToString());
		//form.AddBinaryData("file", imageData, "screenShot.png", "image/png");

		//// Upload to a cgi script
		//using (var w = UnityWebRequest.Post(urlUploadImage, form))
		//{
		//	yield return w.SendWebRequest();
		//	if (w.result != UnityWebRequest.Result.Success)
		//	{
		//		print(w.error);
		//	}
		//	else
		//	{
		//		print("Finished Uploading Screenshot");
		//	}
		//}
		

		//SendEmail(emailId); // mohd@karam.in  unique time stamp

    }

	public bool urlRecived;

	[ContextMenu("Do Something")]
	void TempScremShotSend()
    {
				//Debug.Log("called");
		ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		//ApplyOnQuadPlane(ss);
        // set name
        string name = "Harness_ScreenShot" + ssCounter + ".png";
		ssCounter++;
		PlayerPrefs.SetInt("scCounter", ssCounter);

		// Save the screenshot to Gallery/Photos
		NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "HarnessAR", name, (success, path) => {
			//if (success)
			//{
			//	Debug.Log("Image saved to gallery at path: " + path);
			//}
			//else
			//{
			//	Debug.Log("Failed to save image to gallery.");
			//}
		}); //savePsth = path

		SnapShotAnimator.SetActive(true);
		imageData = ss.EncodeToPNG();

		string TextureArray = Convert.ToBase64String(imageData);
		//string jsonOutput = JsonUtility.ToJson(new StoreJson(TextureArray));

		//byte[] bytes;

  //      using (MemoryStream ms = new MemoryStream())
  //      {
  //          BinaryFormatter bf = new BinaryFormatter();
  //          bf.Serialize(ms, ss);
  //          bytes = ms.ToArray();
  //      }

  //      string enc = Convert.ToBase64String(bytes);
		//Debug.Log(TextureArray);


  //      savePsth = Path.Combine(Application.persistentDataPath, "TempScreenshot.png");
		//File.WriteAllBytes(savePsth, imageData);

		// Create a RestSharp client and request.
		RestClient client = new RestClient(urlUploadImage); //https://api.arresto.in/api/client/250/ar/media_uploads
		RestRequest request = new RestRequest(Method.POST);
		var productUrl = harnessMasserManagerUI.productCodeData.wrokStationDetail.currentUrl;//https://api.arresto.in/api/client/250/ar/media_uploads
		// Add the image data as a parameter to the request.
		//request.AddFile("file", imageData, "screenshot.png", "image/png");


		var uniqId = GenerateUniqueID().ToString();

		// Get the current UTC time
		DateTime currentTime = DateTime.UtcNow;

		// Calculate the Unix timestamp by subtracting the Unix epoch (January 1, 1970)
		TimeSpan timeSpan = currentTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		// Get the total seconds and convert it to a long integer
		long unixTimestamp = (long)timeSpan.TotalSeconds;

		// Your base URL
		//string baseUrl = "https://app.arresto.in/#/ar_media?c=250&media=";


		string fileNamePasse = unixTimestamp + ".png";
		var puidPass = uniqId.ToString();
        //Debug.Log("Unix Timestamp: \n" + finalUrl);

        //harnessCaryForwardData.prevouslyData.productUrl = productUrl;
        //harnessCaryForwardData.prevouslyData.fileStr = imageData;
        //harnessCaryForwardData.prevouslyData.fileName = fileNamePasse;
        //harnessCaryForwardData.prevouslyData.puId = puidPass;


        request.AddParameter("productUrl", productUrl);
		request.AddParameter("file_str", TextureArray);
		request.AddParameter("filename", fileNamePasse);
		request.AddParameter("puid", puidPass);


		// Execute the request asynchronously.
		client.ExecuteAsync(request, response =>
		{
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				//Debug.Log(response.Content);
                qrImageUploadRespone = JsonUtility.FromJson<QrImageUploadRespone>(response.Content);
				//Debug.Log("qrImageUploadRespone "+qrImageUploadRespone.qrString);
				//imageUrl = qrImageUploadRespone.qrString;
				//Debug.Log("imageUrl" + imageUrl);
                harnessCaryForwardData.imageUploadUrlQr = qrImageUploadRespone.qrString;
				urlRecived = true;
				harnessMasserManagerUI.takingSnap = false;
                //var empetyData = string.Empty;
                //harnessCaryForwardData.prevouslyData.productUrl = empetyData;
                //harnessCaryForwardData.prevouslyData.fileStr = new byte[0];
                //harnessCaryForwardData.prevouslyData.fileName = empetyData;
                //harnessCaryForwardData.prevouslyData.puId = empetyData;
                //SendEmail(emailId);
            }
			else
			{
				urlRecived = false;
				//harnessMasserManagerUI.takingSnap = true;
				Debug.LogError("Image upload failed: " + response.ErrorMessage);
			}
		});
		//harnessCaryForwardData.imageUploadUrlQr = imageUrl;
		Invoke(nameof(HarnessSizeChangeGetEnable), 3);

		// Append the Unix timestamp and the rest of the URL
		string finalUrl = uploadImageUrlgenrate + unixTimestamp + ".png&p=" + uniqId;
		imageUrl = finalUrl;
		generateQR.UploadImgeOnGenerateQRUrl(imageUrl);

		//StartCoroutine(SendData());
	}

	int GenerateUniqueID()
	{
		int randomId;

		do
		{
			randomId = UnityEngine.Random.Range(100, 100000);
		} while (usedIds.Contains(randomId));

		usedIds.Add(randomId);
		return randomId;
	}
	/// <summary>
	/// if url not response in 3 second user can modev hand to change harness size
	/// </summary>
	void HarnessSizeChangeGetEnable()
    {
		SnapShotAnimator.SetActive(false);

		if (!urlRecived)
        {
		harnessMasserManagerUI.takingSnap = false;

        }

	}

    private void FixedUpdate()
    {
        if (urlRecived)
        {

			urlRecived = false;
        }
    }

    IEnumerator SendData()
    {
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("image", imageData, "screenShot.png", "image/png");

		// Upload to a cgi script
		using (var w = UnityWebRequest.Post(urlUploadImage, form))
		{

			yield return w.SendWebRequest();
			
			if (w.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(w.error);
			}
			else
			{
				Debug.Log(w.result + "Finished Uploading Screenshot");
			}
		}
	}


	void ChangeNumTextInvokeRepeat()
    {
        if (repeatCount == 0)
        {
			repeatCount = 5;
			snapShotPanel.SetActive(false);
			count.text = repeatCount.ToString();
			//Debug.Log("cancel call");
			CancelInvoke(nameof(ChangeNumTextInvokeRepeat));
			return;
        }
		count.text = repeatCount.ToString();
			//Debug.Log("calls" + repeatCount);
		repeatCount--;
		//Debug.Log(repeatCount);
	}




	private IEnumerator ChangeFloatOverTime()
	{
		float startTime = Time.time;

		while (Time.time - startTime < duration)
		{
			float elapsed = Time.time - startTime;
			lerpedValue = Mathf.Lerp(0f, targetValue, elapsed / duration);
			fillRoundCount.fillAmount = lerpedValue;
			//Debug.Log("Lerped Value: " + lerpedValue);

			//yield return new WaitForSeconds(1f); // Wait for 1 second before the next iteration
			yield return null; // Wait for 1 second before the next iteration
		}

		// Ensure the final value is exactly the target value
		//lerpedValue = targetValue;
		//Debug.Log("Lerped Value: " + lerpedValue);
	}
	//private void Update()
	//   {
	//       if (quad != null)
	//       {
	//		quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * farAway;
	//		quad.transform.forward = Camera.main.transform.forward;
	//		quad.transform.localScale = new Vector3(1f, Screen.height / (float)Screen.width, 1f);
	//	}
	//   }



    void ApplyOnQuadPlane(Texture2D ss)
    {
		//// Assign texture to a temporary quad and destroy it after 5 seconds
		//quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		//quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
		//quad.transform.forward = Camera.main.transform.forward;
		//quad.transform.localScale = new Vector3(1f, Screen.height / (float)Screen.width, 1f);

		//Material material = quad.GetComponent<Renderer>().material;
		//if (!material.shader.isSupported) // happens when Standard shader is not included in the build
		//	material.shader = Shader.Find("Legacy Shaders/Diffuse");

		//material.mainTexture = ss;
		rI.GetComponent<RawImage>().texture = ss;
		rI.gameObject.SetActive(true);
		//screemShotBackButton.SetActive(true);
		//reportButton.SetActive(true);
	}

	bool snapShootProcessComplete;
	public GameObject closwQrImage;
	public void TakeSnapShot()
    {
		//Debug.Log("pick");
        if (!closwQrImage.activeInHierarchy)
		{
			snapShootProcessComplete = false;
		generateQR.OnGenerateQRButtonClick();
		harnessMasserManagerUI.QrBarCodeGuideAnimeScaleUpDown();
			harnessMasserManagerUI.takingSnap = true;
		StartCoroutine(TakeScreenshotAndSave());
        }
	}

	/// <summary>
	/// Restart Session
	/// </summary>
	void ReturnFromScreemShot()
    {
		//Destroy(quad);
		Destroy(ss);
		//screemShotBackButton.SetActive(false);
		//reportButton.SetActive(false);
		UnHideAllUI();
		BallsParent.SetActive(false);
		rI.gameObject.SetActive(false);

	}

	/// <summary>
    /// to send data via mail
    /// </summary>
	public void ReportProcess_OpenMailBox()
    {
        // check mail id is correct format
  //      if (mailId.text != string.Empty)
  //      {
		//SendEmail(mailId.text);

  //      }
		// sent mail with photo
	}


	private void SendEmail(string mailID = "mohd@karam.in", Action<object, AsyncCompletedEventArgs> callback = null)
	{

		// Replace these with your SMTP server details and login credentials
		string smtpServer = "email-smtp.ap-south-1.amazonaws.com";          // "your-smtp-server";
		int port = 587; // Port number depends on your SMTP server configuration
		string from_senderEmail = "info@arresto.in";    //"your-sender-email";
		string senderPassword = "BArZgs5LdZ0nT5rTuJbVarwu2GlrMr99BWMe/OJA702q";// "your-sender-password";
		// smtp user
		string to_recipientEmail = mailID;
		string subject = "Test Email";
		string body = "This is a test email sent from Unity.";
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

		MailMessage mailMessage = new MailMessage(from_senderEmail, to_recipientEmail, subject, body);
		using (var client = new SmtpClient(smtpServer, port))
		{
			try
            {
				client.Credentials = new NetworkCredential("AKIA2H4V66P6RTLGAWLD", senderPassword) as ICredentialsByHost;
                //client.Credentials = new NetworkCredential(from_senderEmail, senderPassword) as ICredentialsByHost;
				//client.Timeout = 10000;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.UseDefaultCredentials = false;
				client.EnableSsl = true;

				ServicePointManager.ServerCertificateValidationCallback =
				  delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
				  { return true; };

				if (!string.IsNullOrEmpty(savePsth) && System.IO.File.Exists(savePsth))
				{
					mailMessage.Attachments.Add(new Attachment(savePsth));
				}
				mailMessage.IsBodyHtml = true;
				if (callback == null)
				{
					callback = SampleCallback;
				}
				client.SendCompleted += new SendCompletedEventHandler(callback);
				client.SendAsync(mailMessage, "");

                Debug.Log("Email sent async...");
				//foreach (string path in attachmentPaths)
				//{
				//}
				mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

				client.SendAsync(mailMessage, "");
			}
			catch (Exception ex)
			{
				Debug.Log("Error: " + ex.Message);
				callback("", new AsyncCompletedEventArgs(ex, true, "Exception occured"));
			}
		}
			File.Delete(savePsth);

		//Debug.Log("Email sent successfully!");
	}
	private static void SampleCallback(object sender, AsyncCompletedEventArgs e)
	{
		if (e.Cancelled || e.Error != null)
		{
			Debug.Log("Error: " + e.Error.Message);
		}
		else
		{
			
			Debug.Log("Email sent successfully.");
		}

	}



	private void TakePicture(int maxSize)
	{
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			Debug.Log("Image path: " + path);
			if (path != null)
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize);
				if (texture == null)
				{
					Debug.Log("Couldn't load texture from " + path);
					return;
				}

				// Assign texture to a temporary quad and destroy it after 5 seconds
				GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
				quad.transform.forward = Camera.main.transform.forward;
				quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

				Material material = quad.GetComponent<Renderer>().material;
				if (!material.shader.isSupported) // happens when Standard shader is not included in the build
					material.shader = Shader.Find("Legacy Shaders/Diffuse");

				material.mainTexture = texture;

				Destroy(quad, 5f);

				// If a procedural texture is not destroyed manually, 
				// it will only be freed after a scene change
				Destroy(texture, 5f);
			}
		}, maxSize);

		Debug.Log("Permission result: " + permission);
	}

	private void RecordVideo()
	{
		NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
		{
			Debug.Log("Video path: " + path);
			if (path != null)
			{
				// Play the recorded video
				//Handheld.PlayFullScreenMovie("file://" + path);
			}
		});

		Debug.Log("Permission result: " + permission);
	}


	void HideAllUI()
    {
   //     foreach (var item in uiHideUndie)
   //     {
			//item.SetActive(false);
   //     }
    }

	void UnHideAllUI()
    {
		//foreach (var item in uiHideUndie)
		//{
		//	item.SetActive(true);
		//}
	}
}


[System.Serializable]
public class QrImageUploadRespone
{
	public string status;
	public string message;
	public string qrString;

}