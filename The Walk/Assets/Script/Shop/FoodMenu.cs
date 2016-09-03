using UnityEngine;
using System.Collections;

public class FoodMenu : MonoBehaviour {
	public GameObject foodBox;
	void OnEnable(){
		MallEvent.OnFoodLoadComplete += MallEvent_OnFoodLoadComplete;
	}


	void OnDisable(){
		MallEvent.OnFoodLoadComplete -= MallEvent_OnFoodLoadComplete;
	}
	void MallEvent_OnFoodLoadComplete ()
	{
		GameObject go;
		for (int i = 0; i < Mall.GetInstance.foodList.Count; i++) {
			go = Instantiate (foodBox);
			go.transform.SetParent (this.transform);
			go.transform.localScale = Vector3.one;
			go.GetComponent<FoodBoard> ().SetData ((Food)Mall.GetInstance.foodList[i]);

		}

	}
}
