using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CategoryMenu : MonoBehaviour {
	public GameObject button_category;
	public List<Button> buttonChoose = new List<Button>();
	void OnEnable(){
		MallEvent.OnCategoryLoadComplete += MallEvent_OnCategoryLoadComplete;
	}
	void OnDisable(){
		MallEvent.OnCategoryLoadComplete -= MallEvent_OnCategoryLoadComplete;
	}
	void MallEvent_OnCategoryLoadComplete ()
	{
		GameObject go;


		CreateCategoryButton (-2, "ทั้งหมด");
		CreateCategoryButton (-1, "ยอดฮิต");
		for (int i = 0; i < Mall.GetInstance.categoryList.Count; i++) {
			go = Instantiate (button_category);
			go.transform.SetParent (this.transform);
			go.transform.localScale = Vector3.one;
			go.GetComponent<CategoryButton> ().Setdata (Mall.GetInstance.categoryList [i].category_id, Mall.GetInstance.categoryList [i].name);
		}

		MallEvent.instance.SelectCategory(-2);
	}

	void CreateCategoryButton(int category_id,string name){
		GameObject go;
		go = Instantiate (button_category);
		go.transform.SetParent (this.transform);
		go.transform.localScale = Vector3.one;
		go.GetComponent<CategoryButton> ().Setdata (category_id,name);
	}
		
}
