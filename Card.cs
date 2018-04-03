using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Card{

	private GameObject card;
	private GameObject clone;

	private GameObject cardPreview;
	private GameObject clone_preview;

	private string name;
	private GameObject parent;
	private Vector2 position;
	private Vector2 size;
	private string tag;
	private int cost;
	private int value;
	private int attack;
	private int defense;

	public Card(string name){
		this.name = name;
	}

	public Card(string name, GameObject parent, Vector2 position, Vector2 size){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;

		instantiateCard ();
	}

	private void setTag(){
		if (name.Equals ("CRYSTAL") || name.Equals ("IRON") || name.Equals ("STONE"))
			tag = "M";
		else if (name.Equals ("LASER") || name.Equals ("MECH") || name.Equals ("SPACECRAFT"))
			tag = "E";
		else if (name.Equals ("EMP") || name.Equals ("VOLCANO"))
			tag = "D";
		else
			tag = "NONE";
	}

	public void instantiateCard(){
		card = new GameObject();
		card.AddComponent<Image> ();
		card.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("img/cards/"+name);

		card.GetComponent<RectTransform> ().anchoredPosition = position;
		card.GetComponent<RectTransform> ().sizeDelta = size;

		card.AddComponent<Button> ();

		card.AddComponent<AudioListener> ();

		card.AddComponent<EventTrigger> ();

		clone = GameObject.Instantiate (card, GameObject.Find(parent.name).GetComponent<RectTransform>(), false);
		clone.transform.SetParent (parent.transform);
		GameObject.Destroy (card);

		EventTrigger.Entry mouseEnter = new EventTrigger.Entry ();
		mouseEnter.eventID = EventTriggerType.PointerEnter;
		mouseEnter.callback.AddListener ((data) => {
			OnPointerEnterDelegate ((PointerEventData)data);
		});
		EventTrigger.Entry mouseExit = new EventTrigger.Entry ();
		mouseExit.eventID = EventTriggerType.PointerExit;
		mouseExit.callback.AddListener ((data) => {
			OnPointerExitDelegate ((PointerEventData)data);
		});

		clone.GetComponent<EventTrigger> ().triggers.Add (mouseEnter);
		clone.GetComponent<EventTrigger> ().triggers.Add (mouseExit);
		clone.gameObject.tag = "CARD";
	}

	public void OnPointerEnterDelegate (PointerEventData data){
		cardPreview = new GameObject();
		cardPreview.AddComponent<Image> ();
		cardPreview.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("img/cards/"+name);

		cardPreview.GetComponent<RectTransform> ().anchoredPosition = new Vector2(position.x, position.y+300);
		cardPreview.GetComponent<RectTransform> ().sizeDelta = new Vector2((int)(size.x*1.5), (int)(size.y*1.5));

		cardPreview.AddComponent<Button> ();

		cardPreview.AddComponent<EventTrigger> ();

		clone_preview = GameObject.Instantiate (cardPreview, GameObject.Find(parent.name).GetComponent<RectTransform>(), false);
		clone_preview.transform.SetParent (parent.transform);
		GameObject.Destroy (cardPreview);

	}
	public void OnPointerExitDelegate (PointerEventData data){
		GameObject.Destroy (clone_preview);
		GameObject.Destroy (cardPreview);
	}

	public void skill(){
	
	}

	public void hide(){
		clone.gameObject.SetActive (false);
	}

	public void setPositionAndSize(Vector2 position, Vector2 size){
		this.position = position;
		this.size = size;
	}
	public void setParent(GameObject parent){
		this.parent = parent;
	}
}
