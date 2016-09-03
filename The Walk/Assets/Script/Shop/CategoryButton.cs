using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CategoryButton : MonoBehaviour {
	public delegate void SelectCategoryEvent (int id);
	public static event SelectCategoryEvent OnSelectCategory;
	public Text category_txt;
	public int category_id;
	public Color colorSelect,colorDeselect;
	void Start(){
		GetComponent<Button> ().onClick.AddListener (OnSelect);
	}
	public void Setdata(int _category_id,string name){
		category_txt.text = name;
		category_id = _category_id;
	}
	void OnEnable(){
		MallEvent.OnSelectCategory += MallEvent_OnSelectCategory;
		MallEvent.OnSelectShop += MallEvent_OnSelectCategory;
	}





	void OnDisable(){
		MallEvent.OnSelectCategory -= MallEvent_OnSelectCategory;
		MallEvent.OnSelectShop -= MallEvent_OnSelectCategory;
	}

	void OnSelect(){
		MallEvent.instance.SelectCategory(category_id);
	}
	void MallEvent_OnSelectCategory (int id)
	{
		if (id == category_id) {
			GetComponent<Image> ().color = colorSelect;
			GetComponentInChildren<Text> ().color = colorDeselect;
		} else {
			GetComponent<Image> ().color = colorDeselect;
			GetComponentInChildren<Text> ().color = colorSelect;
		}
	}
}
