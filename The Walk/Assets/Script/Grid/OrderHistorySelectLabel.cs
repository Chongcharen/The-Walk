using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class OrderHistorySelectLabel : MonoBehaviour {
	public RawImage rawImage;
	public Text food_txt, price_txt, quantity_txt, total_txt;
	public Cart cart;
	public void SetData(Cart c){

		StartCoroutine (LoadImage (c.images));
		food_txt.text = c.name;
		price_txt.text = c.price.ToString ();
		quantity_txt.text = c.quantity.ToString ();
		total_txt.text = ""+(c.quantity * c.price) + " THB";
	}
	IEnumerator LoadImage(string urlPath){
		WWW www = new WWW (urlPath);
		yield return StartCoroutine(new WWWRequest(www));
		rawImage.texture = www.texture;
	}

}
