using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet{

	private Handler handler;

	private AudioSource audioSource;
	private AudioClip clip_deploy;

	private GameObject planet;
	private GameObject clone;

	private string name;
	private GameObject parent;
	private Vector2 position;
	private Vector2 size;
	private GameObject info;

	public Player owner_p;
	public string owner;
	public int population;
	public int shield;
	public List<Card> cards;
	public int populationMax;
	public int resourceMax;
	public int hasLaser;
	public bool isRecon;

	private Text text_owner;
	private Text text_population;
	private Text text_shield;
	public GameObject text;

	public TextLogControl logControl;

	private string deployName;

	private bool isSelected;

	public Planet(string name, GameObject parent, Vector2 position, Vector2 size, GameObject info, int populationMax, int resourceMax){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;
		this.info = info;
		this.owner = null;
		this.isSelected = false;
		this.cards = new List<Card> ();
		this.hasLaser = 0;
		this.populationMax = populationMax;
		this.resourceMax = resourceMax;
		this.isRecon = false;
		audioSource = GameObject.Find ("AudioObject").GetComponent<AudioSource> ();

		instantiatePlanet ();
	}

	public void setLogControl(TextLogControl logControl){
		this.logControl = logControl;
	}

	private void instantiatePlanet(){
		planet = new GameObject();
		planet.AddComponent<Image> ();
		planet.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("img/planets/"+name);

		planet.GetComponent<RectTransform> ().anchoredPosition = position;
		planet.GetComponent<RectTransform> ().sizeDelta = size;

		planet.AddComponent<Button> ();

		text = new GameObject ();

		clone = GameObject.Instantiate (planet, GameObject.Find(parent.name).GetComponent<RectTransform>(), false);
		clone.transform.SetParent (parent.transform);
		clone.GetComponent<Button> ().onClick.AddListener (planetInfo);

		text = new GameObject ();
		text.AddComponent<Text> ();
		text.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		text.GetComponent<Text> ().fontSize = 25;
		text.GetComponent<Text> ().color = Color.white;
		text.GetComponent<Text> ().font = Resources.Load<Font> ("font/PIXEL");

		text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (160, 160);
		//text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (clone.GetComponent<RectTransform> ().anchoredPosition.x, clone.GetComponent<RectTransform> ().anchoredPosition.y - clone.GetComponent<RectTransform> ().sizeDelta.y / 2);

		text = GameObject.Instantiate (text, parent.GetComponent<RectTransform>(), false);


		text.transform.SetParent (clone.transform);
		text.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);


		GameObject.Destroy (planet);
	}

	public void setListener(string mode){
		switch (mode) {
		case "deploy":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (deployTroop);
			break;
		case "attack1":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (attacked1);
			break;
		case "attack2":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (attacked2);
			break;
		case "defense1":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (defending1);
			break;
		case "defense2":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (defending2);
			break;
		case "recon":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (recon);
			break;
		case "normal":
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (planetInfo);
			break;
		default:
			clone.GetComponent<Button> ().onClick.RemoveAllListeners ();
			clone.GetComponent<Button> ().onClick.AddListener (planetInfo);
			break;
		}
	}

	public void recon(){
		isRecon = true;
		planetInfo ();
		foreach (Planet planet in handler.getGameScript().planets) {
			planet.setListener ("normal");
		}
		handler.textLogControl.logText (handler.getPlaying ().getName () + " HAS USED RECON ON " + owner + "'S PLANET", Color.white);
		handler.getPlaying ().removeCard (handler.getCard (), handler.getMainSet ());
		handler.ActionInfo.text = handler.getPlaying() + " IS PLAYING";
	}

	public void attacked1(){
		if (handler.getPlaying ().getName ().Equals (owner)) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			isSelected = !isSelected;

			handler.setPlanet (this);

			foreach (Card card in handler.getPlaying().getCards()) {
				card.setListener ("attack");
			}
			foreach (Planet planet in handler.getGameScript().planets) {
				planet.setListener ("attack2");
			}
			handler.setPlaceHolderText ();
			handler.ActionInfo.text = "CHOOSE CARDS TO ATTACK";
		}
	}

	public void attacked2(){
		if (!handler.getPlaying ().getName ().Equals (owner) && owner != null ) {
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			isSelected = !isSelected;
			handler.setPlanetAttacked (this);
			foreach (Card card in handler.getPlaying().getCards()) {
				card.setListener ("attack");
			}
		}
		foreach(Planet planet in handler.getGameScript().planets){
			planet.setListener ("normal");
		}

		handler.troopInput.gameObject.SetActive (true);


	}

	public void defending1(){
		if (handler.getPlaying ().getName().Equals (owner) && cards.Count < resourceMax) {
			handler.ActionInfo.text = "CHOOSE CARDS TO DEPLOT\nPRESS THIS PLANET AGAIN TO CONFIRM";
			clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
			handler.setPlanet (this);
			foreach (Card card in handler.getPlaying().getCards()) {
				card.setListener ("defense");
			}
			foreach (Planet planet in handler.getGameScript().planets) {
				planet.setListener ("defense2");
			}
		}
	}

	public void defending2(){
		handler.textLogControl.logText (handler.getPlaying ().getName () + " HAS DEPLOYED SOME CARDS TO HIS PLANET", Color.white);
		shield = handler.getDefense ();
		foreach (Card card in handler.cards) {
			cards.Add (card);
			if (card.getName ().Equals ("LASER"))
				hasLaser++;
		}
		handler.clearCard ();
		handler.getPlaying ().removeIfSelected (this);
		foreach (Planet planet in handler.getGameScript().planets) {
			planet.resetColor ();
			planet.setListener ("normal");
		}
		foreach (Card card in handler.getPlaying().getCards()) {
			card.setListener ("normal");
			card.destroyClone ();
		}
		handler.getGameScript ().nextRound ();
		handler.getPlaying ().displayCards ();
		handler.canvas_game.gameObject.SetActive (false);
		handler.canvas_block.gameObject.SetActive (true);
	}

	public void deployTroop(){

		if (string.IsNullOrEmpty (owner) || handler.getPlaying ().getName().Equals (owner) && handler.getPlaying ().getTroopsLeft () > 0 && population < populationMax ) {
			audioSource.Play ();

			if (population == 0) {
				if (handler.getPlaying ().hasCard ("SPACECRAFT")) {
					clip_deploy = Resources.Load<AudioClip> ("audio/sound effect/deploy");
					audioSource.clip = clip_deploy;
					for (int i = cards.Count - 1; i >= 0; i--) {
						handler.getPlaying ().addCard (cards[i]);
						cards.RemoveAt (i);
					}
					handler.getPlaying ().addPlanet (this);

					handler.getPlaying ().removeByName ("SPACECRAFT", handler.getMainSet ());

					owner = deployName;
					owner_p = handler.getPlaying ();

					population++;

					logControl.logText (deployName + " HAS SUCCESSFULLY DEPLOYED HIS TROOP ON (" + name.ToUpper () + ")", Color.white);
					logControl.logText ("(" + name.ToUpper () + ") NOW HAS " + population + " POPULATION", Color.white);

					handler.getGameScript ().action = true;

					planetInfo ();

					handler.getPlaying ().setTroopsLeft (handler.getPlaying ().getTroopsLeft () - 1);
					handler.getGameScript ().nextRound ();
					handler.getGameScript ().button_cancel.gameObject.SetActive (false);
				} else {
					handler.ActionInfo.text = "DEPLOY FAILED: NO SPACECRAFT";
				}
			} else {

				clip_deploy = Resources.Load<AudioClip> ("audio/sound effect/deploy");
				audioSource.clip = clip_deploy;

				owner = deployName;
				owner_p = handler.getPlaying ();

				population++;

				logControl.logText (deployName + " HAS SUCCESSFULLY DEPLOYED HIS TROOP ON (" + name.ToUpper () + ")", Color.white);
				logControl.logText ("(" + name.ToUpper () + ") NOW HAS " + population + " POPULATION", Color.white);

				handler.getGameScript ().action = true;

				planetInfo ();

				handler.getPlaying ().setTroopsLeft (handler.getPlaying ().getTroopsLeft () - 1);
				handler.getGameScript ().nextRound ();
				handler.getGameScript ().button_cancel.gameObject.SetActive (false);
			}
		} else {
			if (population >=  populationMax) {
				handler.ActionInfo.text = "DEPLOY FAILED: MAX POPULATION REACHED";
			}
			if (!deployName.Equals (owner)) {
				handler.ActionInfo.text = "DEPLOY FAILED: PLANET IS ALREADY SEIZED";
			}
			if (!(handler.getPlaying ().getTroopsLeft () > 0)) {
				handler.ActionInfo.text = "DEPLOY FAILED: NO MORE TROOP LEFT";
			}
			Debug.Log ("DEPLOY FAIL" +string.IsNullOrEmpty (owner) + handler.getPlaying ().Equals (owner) + (handler.getPlaying ().getTroopsLeft () > 0) + (population < populationMax) + handler.getPlaying().hasCard("SPACECRAFT"));
		}
		text.GetComponent<Text> ().text = owner;
	}

	public void planetInfo(){
		text_owner = info.transform.Find ("owner_text").gameObject.GetComponent<Text> ();
		text_owner.text = this.owner + "";

		text_population = info.transform.Find ("population_text").gameObject.GetComponent<Text> ();
		text_population.text = population + "";

		if (isRecon||handler.getPlaying().getName().Equals(owner)) {
			text_shield = info.transform.Find ("shield_text").gameObject.GetComponent<Text> ();
			text_shield.text = shield + "";
		} else {
			text_shield = info.transform.Find ("shield_text").gameObject.GetComponent<Text> ();
			text_shield.text = "???";
			isRecon = false;
		}

		parent.gameObject.SetActive (false);
		info.gameObject.SetActive (true);
	}

	public void removeByName(string name, MainSet mainSet){
		int i = 0;
		while (!cards [i].getName ().Equals (name))
			i++;
		mainSet.addCard (cards [i]);
		cards [i].destroyClone ();
		cards.Remove (cards [i]);
	}

	public void addCard(Card card){
		cards.Add(card);
	}
	public void setOwner(string owner){
		this.owner = owner;
	}
	public void setPopulation(int population){
		this.population = population;
	}
	public void setShield(int shield){
		this.shield = shield;
	}
	public void setDeployName (string deployName){
		this.deployName = deployName;
	}
	public void setHandler(Handler handler){
		this.handler = handler;
	}
	public void resetColor(){
		clone.GetComponent<Image> ().color = new Color (0xFF, 0xFF, 0xFF, 0xFF);
	}
	public GameObject getPlanet(){return planet;}
	public string getName(){return name;}
	public GameObject getParent(){return parent;}
	public Vector2 getPosition(){return position;}
	public Vector2 getSize(){return size;}
	public int getPopulation(){return population;}
	public int getShield(){return shield;}

}