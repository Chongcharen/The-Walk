using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class GeneralSharing : MonoBehaviour
{
	public static GeneralSharing instance;
	#region PUBLIC_VARIABLES
	public Texture2D MyImage;
	#endregion
	
	#region UNITY_DEFAULT_CALLBACKS
	void Start(){
		instance = this;
	}
	public void OnEnable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving += ScreenshotSaved;
	}
	
	void OnDisable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving -= ScreenshotSaved;
	}
	#endregion
	
	#region DELEGATE_EVENT_LISTENER
	void ScreenshotSaved ()
	{
		#if UNITY_IPHONE || UNITY_IPAD
		GeneralSharingiOSBridge.ShareTextWithImage (ScreenshotHandler.savedImagePath, "Hello !!! \nThis is post from TheAppGuruz");
		#endif
	}
	#endregion
	
	#region CO_ROUTINES
	IEnumerator ShareAndroidText ()
	{
		yield return new WaitForEndOfFrame ();
		#if UNITY_ANDROID
		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);
		
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");

		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Text Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Text Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Text Sharing Android Demo");
		
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		
		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here
		
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
		
		//			string uriPath =  uriObject.Call<string>("getPath");
		bool fileExist = fileObject.Call<bool>("exists");
		Debug.Log("File exist : " + fileExist);
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
		
		#endif
	}
	
	IEnumerator SaveAndShare ()
	{
		yield return new WaitForEndOfFrame ();
		#if UNITY_ANDROID
		
		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);
		
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
		
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Media Sharing Android Demo");
		
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");

		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here
		
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
		
		//			string uriPath =  uriObject.Call<string>("getPath");
		bool fileExist = fileObject.Call<bool>("exists");
		if (fileExist)
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
		#endif
		
	}
	#endregion

	IEnumerator CaptureImage (string urlPath,Vector2 size)
	{

		yield return new WaitForEndOfFrame ();

		Texture2D texture = new Texture2D ((int)size.x, (int)size.y, TextureFormat.RGB24, false);
		WWW www = new WWW (urlPath);
		yield return www;
		www.LoadImageIntoTexture (texture);
		byte[] bytes = texture.EncodeToJPG();
		string path = Application.persistentDataPath + "/screenShotThewalk.jpg";
		File.WriteAllBytes(path, bytes);

	
		#if UNITY_ANDROID
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			intentObject.Call<AndroidJavaObject>("setType", "image/*");
			

			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

			bool fileExist = fileObject.Call<bool>("exists");
			// Attach image to intent
			if (fileExist)
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Subject");
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Title");
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Message");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			currentActivity.Call ("startActivity", intentObject);
		#endif



	}






	#region BUTTON_CLICK_LISTENER
	
	public void OnShareSimpleText ()
	{
		#if UNITY_ANDROID
		StartCoroutine (ShareAndroidText ());
		#elif UNITY_IPHONE || UNITY_IPAD
		GeneralSharingiOSBridge.ShareSimpleText ("Hello !!! \nThis is post from TheAppGuruz");
		#endif
	}
	public void OnShareTextWithLinkImage(string url , Vector2 size){
		#if UNITY_ANDROID
		StartCoroutine (CaptureImage (url,size));
		#elif UNITY_IPHONE || UNITY_IPAD
		byte[] bytes = MyImage.EncodeToPNG ();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes (path, bytes);
		string path_ = "MyImage.png";

		StartCoroutine (ScreenshotHandler.Save (path_, "Media Share", true));
		#endif
	}
	public void OnShareTextWithImage ()
	{
		Debug.Log ("Media Share");
		#if UNITY_ANDROID
		StartCoroutine (SaveAndShare ());
		#elif UNITY_IPHONE || UNITY_IPAD
		byte[] bytes = MyImage.EncodeToPNG ();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes (path, bytes);
		string path_ = "MyImage.png";
		
		StartCoroutine (ScreenshotHandler.Save (path_, "Media Share", true));
		#endif
	}
	#endregion
	
}
