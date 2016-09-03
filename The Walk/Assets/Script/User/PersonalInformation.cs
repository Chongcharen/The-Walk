using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json;
public class PersonalInformation : MonoBehaviour {

	public InputField input_name, input_lastName, input_email, input_oldPassword, input_newPassword, input_comfirmPassword;
	public Button b_updateProfile;
	ServiceResponse response;
	// Use this for initialization
	void Start () {
		b_updateProfile.onClick.AddListener (OnUpdateProfile);
	}
	
	//{member_id,email,fname,lname,password_old,newpassword}	หลังจากเข้าระบบแล้ว สมาชิกจะสามารถอัพเดทโปรไฟล์ได้จากเมธอดนี้	
	void OnUpdateProfile () {

		/*if(string.IsNullOrEmpty(input_name.text) || string.IsNullOrEmpty(input_lastName.text) ){
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกข้อมูลให้ครบ");
			return;
		}*/
		if (!Utils.IsValidEmailAddress(input_email.text)) {
			PopupManager.instance.ShowAlertPopup ("กรุณากรอกอีเมล์ให้ถูกต้อง");
			return;
		}
		/*if (input_oldPassword.text != SaveManager.instance.GetPassword()) {
			Debug.Log (SaveManager.instance.GetPassword ());
			PopupManager.instance.ShowAlertPopup ("รหัสผ่านเก่าไม่ถูกต้อง");
			return;
		}
		if (input_newPassword.text == input_comfirmPassword.text) {
			StartCoroutine (UpdateProfile ());
		} else {
			PopupManager.instance.ShowAlertPopup ("รหัสผ่านใหม่และยืนยันรหัสผ่านใหม่ไม่ตรงกัน");
			return;
		}*/
		StartCoroutine (UpdateProfile ());
	}
	void OnEnable(){
		MallEvent.OnUserLoginComplete += MallEvent_OnUserLoginComplete;
	}
	void OnDisable(){
		MallEvent.OnUserLoginComplete -= MallEvent_OnUserLoginComplete;
	}
	void MallEvent_OnUserLoginComplete ()
	{
		input_email.text = Profile.GetInstance.user.email;
		input_name.text = Profile.GetInstance.user.fname;
		input_lastName.text = Profile.GetInstance.user.lname;

	}
	IEnumerator UpdateProfile(){
		WWWForm form = new WWWForm ();
		form.AddField ("member_id", Profile.GetInstance.user.member_id);
		form.AddField ("email", input_email.text);
		form.AddField ("fname", input_name.text);
		form.AddField ("lname", input_lastName.text);
		if (!string.IsNullOrEmpty (input_oldPassword.text) ||!string.IsNullOrEmpty (input_newPassword.text)||!string.IsNullOrEmpty (input_comfirmPassword.text)) {
			if (input_oldPassword.text != SaveManager.instance.GetPassword()) {
				PopupManager.instance.ShowAlertPopup ("รหัสผ่านเก่าไม่ถูกต้อง");
				yield break;
			}
			if (input_newPassword.text != input_comfirmPassword.text) {
				PopupManager.instance.ShowAlertPopup ("รหัสผ่านใหม่และยืนยันรหัสผ่านใหม่ไม่ตรงกัน");
				yield break;
			} 
			form.AddField ("password_old", input_oldPassword.text);
			form.AddField ("newpassword", input_newPassword.text);
		}
		WWW www = new WWW ("http://www.thewalklifestylemall.com/service/updateProfile", form);
		yield return StartCoroutine(new WWWRequest(www));
		if (www.isDone) {
			response = JsonHelper.FromSingleJson<ServiceResponse> (www.text);
			if (response.success) {
				if (!string.IsNullOrEmpty (input_newPassword.text)) {
					Debug.Log ("New Password " + input_newPassword.text);
					SaveManager.instance.SetPassword (input_newPassword.text);
				}
				StartCoroutine ("UpdateNewProfile");
			}
			PopupManager.instance.ShowAlertPopup (response.description);

		}

	}
	IEnumerator UpdateNewProfile(){
		WWWForm wwwForm = new WWWForm ();

		wwwForm.AddField ("email",input_email.text);
		wwwForm.AddField ("password", SaveManager.instance.GetPassword());

		WWW www = new WWW ("http://thewalklifestylemall.com/service/login",wwwForm);
		yield return StartCoroutine(new WWWRequest(www));

		if (www.isDone) {
			SaveManager.instance.SetRawDataUser (www.text);
			LoginForm login = JsonHelper.FromSingleJson<LoginForm> (www.text);
			Profile.GetInstance.user = new User (input_email.text,login);

		}
	}
}
