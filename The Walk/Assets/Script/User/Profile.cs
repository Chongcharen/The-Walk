using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
public class Profile : MonoBehaviour {
	public static Profile GetInstance;
	private string _cache_id;
	public bool isLogin = false;
	public User user = null;
	public List<PlaceDelivery> placeDeliverys;
	public List<Cart> carts;
	public int address_id;
	public UserDoing doing = UserDoing.None;
	public string cache_data = "";
	public string cache_id{
		get{
			if (_cache_id == string.Empty) {
				_cache_id = Generate_Cache_id (8);
			}
			return _cache_id;
		}

	}
	void Awake(){
		GetInstance = this;
	}
	void Start(){
		SaveManager.instance.GetUserFromData ();
	}
	public void Logout(){
		SaveManager.instance.ClearData ();
		isLogin = false;
		user = null;
		if (placeDeliverys != null) {
			placeDeliverys.Clear ();
		}
		carts.Clear();
		MallEvent.instance.UserLogoutComplete ();
	}
	string Generate_Cache_id(int size){
		string input = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";	
		StringBuilder builder = new StringBuilder();
		char ch;
		System.Random rand = new System.Random ();
		for (int i = 0; i < size; i++)
		{
			ch = input[rand.Next(0, input.Length)];
			builder.Append(ch);
		}

		return builder.ToString();
	}

	public void GetCacheData(){
		cache_data = PlayerPrefs.GetString ("userData");


		/*LoginForm login = JsonConvert.DeserializeObject<LoginForm> (cache_data);
		Profile.GetInstance.user = new User (tempEmail,login);
		Profile.GetInstance.isLogin = true;
		MallEvent.instance.UserLoginComplete ();
		MallEvent.instance.SelectPage ((int)Page.EditProfilePage);
		MallEvent.instance.CallEditorProfilePage ((int)ProfilePageSelect.PersonalInformationPage);
		ServiceRequest.instance.GetMyCart ();*/


	}
	public void SaveCacheData(string data){
		PlayerPrefs.SetString ("userdata",data);

	}
}

public enum UserDoing{
	None,
	PaymentButNoAddress
}


[SerializeField]
public class User{
	public int member_id;
	public string fname,lname,avatar;
	public string email;
	public User(string _email,LoginForm login){
		email = _email;
		member_id = login.member_id;
		fname = login.fname;
		lname = login.lname;
		avatar = login.avatar;
	}

}

