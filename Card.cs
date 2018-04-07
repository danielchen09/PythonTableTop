using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card{

	private Handler handler;

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
	private Player owner;
	private Planet planet_deploy;
	private int deploy_mode;

	public Card(string name, GameObject parent){
		this.name = name;
		this.parent = parent;
		setTag ();
		setAttributes ();
	}

	public Card(string name, GameObject parent, Vector2 position, Vector2 size){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;
		setAttributes ();
		setTag ();

		instantiateCard ();
	}

	private void setTag(){
		if (name.Equals ("CRYSTAL") || name.Equals ("IRON") || name.Equals ("STONE"))
			tag = "M";
		else if (name.Equals ("EMP") || name.Equals ("VOLCANO"))
			tag = "X";
		else
			tag = "E";
	}

	public string getTag(){
		return tag;
	}
		
	public void instantiateCard(){
		card = new GameObject();
		card.AddComponent<Image> ();
		card.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("img/cards/"+name);

		card.GetComponent<RectTransform> ().anchoredPosition = position;
		card.GetComponent<RectTransform> ().sizeDelta = size;

		card.AddComponent<Button> ();

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

	public void setListener(string mode){
		switch (mode) {
		case "purchase":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (purchase);
			break;
		case "sell":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (sell);
			break;
		case "normal":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			break;
		default:
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			break;
		}
	}

	public void purchase(){
		if (handler.getPlaying ().getBalance () >= cost) {
			handler.getShop ().purchaseCard (this, handler.getPlaying ());
			handler.getPlaying ().setBalance (handler.getPlaying ().getBalance () - this.cost);
			this.value = this.cost;
			this.clone.gameObject.SetActive (false);
			if (clone_preview != null)
				GameObject.Destroy (clone_preview.gameObject);

		}
	}

	public void sell(){
		handler.getPlaying ().sellCard (this, handler.getShop ());
		handler.getPlaying ().setBalance (handler.getPlaying ().getBalance () + this.value);
		this.clone.gameObject.SetActive (false);
		if (clone_preview != null)
			GameObject.Destroy (clone_preview.gameObject);

	}

	public void attacking(){
		
	}

	public void deploy(){
	
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

	public void destroyClone(){
		GameObject.Destroy (this.clone.gameObject);
	}

	public void setAttributes(){
		/*deploy_mode
		 *0 undeployable
		 *1 deployable
		 *2 deploy while attack
		 */
		switch (name) {
		case "CRYSTAL":
			value = 20;
			cost = 0;
			attack = 0;
			defense = 0;
			deploy_mode = 1;
			break;
		case "EMP":
			value = 0;
			cost = 0;
			attack = 0;
			defense = 0;
			deploy_mode = 0;
			break;
		case "IRON":
			value = 10;
			cost = 0;
			defense = 2;
			attack = 0;
			deploy_mode = 1;
			break;
		case "LASER":
			value = 0;
			cost = 100;
			defense = 0;
			attack = 0;
			deploy_mode = 1;
			break;
		case "MECH":
			value = 0;
			cost = 50;
			defense = 0;
			attack = setAttack (2, 1, 0);
			deploy_mode = 2;
			break;
		case "RECON":
			value = 0;
			cost = 10;
			defense = 0;
			attack = 0;
			deploy_mode = 0;
			break;
		case "SHIELD":
			value = 0;
			cost = 30;
			defense = setDefense (2, 2, 0);
			attack = 0;
			deploy_mode = 1;
			break;
		case "SPACECRAFT":
			value = 0;
			cost = 50;
			defense = 0;
			attack = 0;
			deploy_mode = 0;
			break;
		case "STONE":
			value = 5;
			cost = 0;
			defense = 1;
			attack = 0;
			deploy_mode = 1;
			break;
		case "VOLCANO":
			value = 0;
			cost = 0;
			defense = 0;
			attack = 0;
			deploy_mode = 0;
			break;
		case "WALL":
			value = 0;
			cost = 50;
			defense = setDefense (4, 2, 0);
			attack = 0;
			deploy_mode = 1;
			break;
		case "WEAPON":
			value = 0;
			cost = 30;
			defense = 0;
			attack = setAttack (1, 1, 0);
			deploy_mode = 2;
			break;
		default:
			value = 0;
			cost = 0;
			attack = 0;
			defense = 0;
			deploy_mode = 0;
			Debug.Log ("no such card");
			break;
		}
	}

	public int setAttack(int atk, int per, int total){
		return atk*total/per;
	}

	public int setDefense(int def, int per, int total){
		return def*total/per;
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
	public void setHandler(Handler handler){
		this.handler = handler;
	}

	public string getName(){
		return name;
	}
}
