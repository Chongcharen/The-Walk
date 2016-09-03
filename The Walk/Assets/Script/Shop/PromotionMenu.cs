using UnityEngine;
using System.Collections;

public class PromotionMenu : MonoBehaviour {

	public GameObject promotionBox;
	void OnEnable(){
		MallEvent.OnPromotionLoadComplete += MallEvent_OnPromotionLoadComplete;
	}

	void OnDisable(){
		MallEvent.OnPromotionLoadComplete -= MallEvent_OnPromotionLoadComplete;
	}


	void MallEvent_OnPromotionLoadComplete ()
	{
		GameObject go;
		for (int i = 0; i < Mall.GetInstance.promotionList.Count; i++) {
			go = Instantiate (promotionBox);
			go.transform.SetParent (this.transform);
			go.transform.localScale = Vector3.one;
			go.GetComponent<PromotionLabel> ().SetData (i,(Promotion)Mall.GetInstance.promotionList[i]);
		}
	}
	/*void MallEvent_OnFoodLoadComplete ()
	{
		GameObject go;
		for (int i = 0; i < Mall.GetInstance.foodList.Count; i++) {
			go = Instantiate (promotionBox);
			go.transform.SetParent (this.transform);
			go.transform.localScale = Vector3.one;
			go.GetComponent<FoodBoard> ().SetData ((Food)Mall.GetInstance.foodList[i]);

		}

	}*/
}
