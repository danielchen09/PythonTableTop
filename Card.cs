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
	private GameObject costPreview;
	private GameObject cost_clone;
	public GameObject text;

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
	public bool isSelected;

	private EventTrigger.Entry mouseExit;
	private EventTrigger.Entry mouseEnter;

	public Card(string name, GameObject parent){
		this.name = name;
		this.parent = parent;
		this.isSelected = false;
		setTag ();
		setAttributes ();
	}

	public Card(string name, GameObject parent, Vector2 position, Vector2 size){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;
		setTag ();

		instantiateCard ();
	}

	public void setTag(){
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
		
	public GameObject instantiateCard(){
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

		mouseEnter = new EventTrigger.Entry ();
		mouseEnter.eventID = EventTriggerType.PointerEnter;
		mouseEnter.callback.AddListener ((data) => {
			OnPointerEnterDelegate ((PointerEventData)data);
		});
		mouseExit = new EventTrigger.Entry ();
		mouseExit.eventID = EventTriggerType.PointerExit;
		mouseExit.callback.AddListener ((data) => {
			OnPointerExitDelegate ((PointerEventData)data);
		});
		clone.GetComponent<EventTrigger> ().triggers.Add (mouseEnter);
		clone.GetComponent<EventTrigger> ().triggers.Add (mouseExit);
		clone.gameObject.tag = "CARD";

		return clone;
	}

	public void setListener(string mode){
		EventTrigger.Entry mouseEnterS = new EventTrigger.Entry ();
		mouseEnterS.eventID = EventTriggerType.PointerEnter;
		mouseEnterS.callback.AddListener ((data) => {
			OnPointerEnterDelegateShop ((PointerEventData)data);
		});
		EventTrigger.Entry mouseExitS = new EventTrigger.Entry ();
		mouseExitS.eventID = EventTriggerType.PointerExit;
		mouseExitS.callback.AddListener ((data) => {
			OnPointerExitDelegateShop ((PointerEventData)data);
		});
		switch (mode) {
		case "purchase":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (purchase);
			break;
		case "sell":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (sell);
			break;
		case "attack":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (attacking);
			break;
		case "defense":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (defending);

			break;
		case "normal":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (recon);
			break;
		default:
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (recon);
			break;
		}

	}

	public void recon(){
		if (this.name.Equals ("RECON")) {
			handler.ActionInfo.text = "CLICK ON A PLANET TO DEPLOY A RECON TEAM";
			handler.getGameScript ().button_cancel.gameObject.SetActive (true);
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			foreach (Planet planet in handler.getGameScript().planets) {
				planet.setListener ("recon");
			}
			handler.setCard (this);
		}
	}

	public void resetColor(){
		clone.GetComponent<Image> ().color = new Color (0xFF, 0xFF, 0xFF, 0xFF);
	}

	public void purchase(){

		if (handler.getPlaying ().getBalance () >= cost) {
			handler.getGameScript ().action = true;
			handler.getShop ().purchaseCard (this, handler.getPlaying ());
			handler.getPlaying ().setBalance (handler.getPlaying ().getBalance () - this.cost);
			this.value = this.cost;
			this.clone.gameObject.SetActive (false);
			if (clone_preview != null)
				GameObject.Destroy (clone_preview.gameObject);
			handler.textLogControl.logText (handler.getPlaying ().getName () + " HAS PURCHASED A CARD", Color.white);

		}
		handler.canvas_purchase.transform.Find ("text_balance").GetComponent<Text> ().text = "BALANCE: " + handler.getPlaying ().getBalance ();
	}

	public void sell(){

		handler.getGameScript ().action = true;
		handler.getPlaying ().removeCard (this, handler.getMainSet());
		handler.getPlaying ().setBalance (handler.getPlaying ().getBalance () + this.value);
		this.clone.gameObject.SetActive (false);
		if (clone_preview != null)
			GameObject.Destroy (clone_preview.gameObject);
		handler.textLogControl.logText (handler.getPlaying ().getName () + " HAS SOLD A CARD", Color.white);

		handler.canvas_sell.transform.Find ("text_balance").GetComponent<Text> ().text = "BALANCE: " + handler.getPlaying ().getBalance () + "";
	}

	public void attacking(){
		setAttributes ();

		if (!isSelected && this.deploy_mode == 2) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			isSelected = !isSelected;
			handler.addAttack (this.attack);
		} else if ( this.deploy_mode == 2) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0xFF, 0xFF, 0xFF);
			isSelected = !isSelected;
			handler.addAttack (-1 * this.attack);
		}
		handler.ActionInfo.text = "CLICK THE PLANET TO ATTACK AFTER SELECTING CARDS\nATTACK: " + handler.getAttack();
	}

	public void defending(){
		setAttributes ();
		if (!isSelected && this.deploy_mode == 1) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			isSelected = !isSelected;
			handler.addDefense (this.defense);
			handler.addCard (this);
		} else if ( this.deploy_mode == 1) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0xFF, 0xFF, 0xFF);
			isSelected = !isSelected;
			handler.addDefense (-1 * this.defense);
		}
		handler.ActionInfo.text = "DEFENSE: " + handler.getDefense();
	}

	public void selectColorChange(){
		if (!isSelected) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			isSelected = !isSelected;
		} else {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0xFF, 0xFF, 0xFF);
			isSelected = !isSelected;
		}
	}

	public void OnPointerEnterDelegateShop (PointerEventData data){
		
		costPreview = new GameObject ();
		costPreview.AddComponent<Text> ();
		costPreview.GetComponent<Text> ().font = Resources.Load<Font> ("font/PIXEL");
		costPreview.GetComponent<Text> ().fontSize = 20;
		costPreview.GetComponent<Text> ().color = Color.white;
		costPreview.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		costPreview.GetComponent<Text> ().text = "COST: " + cost;

		costPreview.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-25, -232);
		costPreview.GetComponent<RectTransform> ().sizeDelta = new Vector2 (160, 46);
		costPreview.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		costPreview.AddComponent <EventTrigger> ();

		cost_clone = GameObject.Instantiate (costPreview, GameObject.Find(parent.name).GetComponent<RectTransform>(), false);
		cost_clone.transform.SetParent (parent.transform);
		GameObject.Destroy (costPreview);

	}

	public void OnPointerExitDelegateShop(PointerEventData data){
		GameObject.Destroy (cost_clone);
		GameObject.Destroy (costPreview);
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
		GameObject.Destroy (clone_preview.gameObject);
		GameObject.Destroy (cardPreview.gameObject);
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
			attack = setAttack (2, 1);
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
			defense = setDefense (2, 2);
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
			defense = setDefense (4, 2);
			attack = 0;
			deploy_mode = 1;
			break;
		case "WEAPON":
			value = 0;
			cost = 30;
			defense = 0;
			attack = setAttack (1, 1);
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

	public int setAttack(int atk, int per){
		if (handler.getPlanet () == null)
			return 0;
		return atk*handler.getPlanet().getPopulation()/per;
	}

	public int setDefense(int def, int per){
		if (handler.getPlanet () == null)
			return 0;
		return def*handler.getPlanet().getPopulation()/per;
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

		setAttributes ();
	}

	public string getName(){
		return name;
	}

	public GameObject getClone(){
		return clone;
	}
}
