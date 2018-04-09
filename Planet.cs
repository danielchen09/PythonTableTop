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

	private string owner;
	private int population;
	private int shield;

	private Text text_owner;
	private Text text_population;
	private Text text_shield;

	public TextLogControl logControl;

	private string deployName;

	private bool isSelected;

	public Planet(string name, GameObject parent, Vector2 position, Vector2 size, GameObject info){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;
		this.info = info;
		this.owner = null;
		this.isSelected = false;
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

		clone = GameObject.Instantiate (planet, GameObject.Find(parent.name).GetComponent<RectTransform>(), false);
		clone.transform.SetParent (parent.transform);
		clone.GetComponent<Button> ().onClick.AddListener (planetInfo);

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

	public void attacked1(){
		if (!isSelected) {
			if (!handler.getPlaying ().getName ().Equals (owner) && owner != null) {
				clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
				isSelected = !isSelected;
				handler.setPlanetAttacked (this);

				handler.ActionInfo.text = "CHOOSE PLANET TO DEPLOY TROOPS FROM";
			}
		}
		setListener ("attack2");
	}

	public void attacked2(){
		if (!isSelected) {
			if (handler.getPlaying ().getName ().Equals (owner)) {
				clone.GetComponent<Image> ().color = new Color (0xFF, 0x00, 0x00, 0xFF);
				isSelected = !isSelected;
				handler.setPlanet (this);
				foreach (Card card in handler.getPlaying().getCards()) {
					card.setListener ("attack");
				}
				handler.ActionInfo.text = "CHOOSE CARDS TO DEPLOY";
			}
		}
		setListener ("normal");
	}

	public void deployTroop(){
		clip_deploy = Resources.Load<AudioClip> ("audio/sound effect/deploy");
		audioSource.clip = clip_deploy;
		if (string.IsNullOrEmpty (owner)||deployName.Equals(owner)) {
			audioSource.Play ();

			owner = deployName;
			population++;

			logControl.logText(deployName + " HAS SUCCESSFULLY DEPLOYED HIS TROOP ON (" + name.ToUpper() + ").", Color.white);
			logControl.logText("(" + name.ToUpper() + ") NOW HAS " + population + " POPULATION.", Color.white);

			logControl.logText ("12345", Color.white);

			planetInfo ();
		} else {
			setListener ("normal");
		}
		handler.getGameScript ().nextRound ();
	}

	public void planetInfo(){
		text_owner = info.transform.Find ("owner_text").gameObject.GetComponent<Text> ();
		text_owner.text = this.owner + "";

		text_population = info.transform.Find ("population_text").gameObject.GetComponent<Text> ();
		text_population.text = population + "";

		text_shield = info.transform.Find ("shield_text").gameObject.GetComponent<Text> ();
		text_shield.text = shield + "";

		parent.gameObject.SetActive (false);
		info.gameObject.SetActive (true);
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

}