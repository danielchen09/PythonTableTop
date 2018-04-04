using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Planet{

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

	private string deployName;

	public Planet(string name, GameObject parent, Vector2 position, Vector2 size, GameObject info){
		this.name = name;
		this.parent = parent;
		this.position = position;
		this.size = size;
		this.info = info;
		this.owner = null;
		audioSource = GameObject.Find ("AudioObject").GetComponent<AudioSource> ();

		instantiatePlanet ();
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

	public void deployTroop(){
		clip_deploy = Resources.Load<AudioClip> ("audio/sound effect/deploy");
		audioSource.clip = clip_deploy;
		if (string.IsNullOrEmpty (owner)||deployName.Equals(owner)) {
			audioSource.Play ();

			owner = deployName;
			population++;

			planetInfo ();
		} else {
			setListener ("normal");
		}
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

	public GameObject getPlanet(){return planet;}
	public string getName(){return name;}
	public GameObject getParent(){return parent;}
	public Vector2 getPosition(){return position;}
	public Vector2 getSize(){return size;}

}
