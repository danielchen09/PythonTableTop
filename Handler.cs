using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handler{

	private int attack;
	private int defense;
	private int troops;

	private GameScript gameScript;
	private Planet planet;
	private Planet planet_attacked;
	private Card card;
	private MainSet mainSet;
	private Player player;
	private Shop shop;
	public List<Card> cards;

	public GameObject canvas_game;
	public GameObject canvas_menu;
	public GameObject canvas_planetInfo;
	public GameObject canvas_deploy;
	public GameObject canvas_log;
	public GameObject canvas_option;
	public GameObject canvas_purchase;
	public GameObject canvas_sell;
	public GameObject canvas_block;

	public TextLogControl textLogControl;

	public Text ActionInfo;
	public InputField troopInput;
	public Text troopPlaceholder;
	public Text text_result;
	public Text text_balance_sell;
	public Text text_balance_purchase;

	public Handler(GameScript gameScript, MainSet mainSet, Shop shop, GameObject[] canvas, TextLogControl textLogControl){
		this.gameScript = gameScript;
		this.mainSet = mainSet;
		this.shop = shop;

		this.cards = new List<Card> ();
		canvas_game = canvas [0];
		canvas_menu = canvas [1];
		canvas_planetInfo = canvas [2];
		canvas_deploy = canvas [3];
		canvas_log = canvas [4];
		canvas_option = canvas [5];
		canvas_purchase = canvas [6];
		canvas_sell = canvas [7];
		canvas_block = canvas [8];
		this.ActionInfo = canvas_game.transform.Find("ActionInfo").GetComponent<Text> (); 
		this.troopInput = canvas_game.transform.Find("InputField").GetComponent<InputField> (); 
		this.troopPlaceholder = canvas_game.transform.Find("InputField/Placeholder").GetComponent<Text> (); 
		this.textLogControl = textLogControl;
		this.troopInput.onEndEdit.AddListener (setTroops);
		this.troopInput.gameObject.SetActive (false);

		this.text_balance_sell = canvas_purchase.transform.Find ("text_balance").GetComponent<Text> ();
		this.text_balance_purchase = canvas_sell.transform.Find("text_balance").GetComponent<Text> ();
	}

	public void setTroops(string arg0){
		textLogControl.logText (player + " ATTEMPTED TO ATTACK " + planet_attacked.owner + "'S PLANET (" + planet_attacked.getName() + ")", Color.white);
		player.removeByName ("SPACECRAFT", mainSet);
		if (int.Parse (arg0) < getPlanet ().getPopulation ()) {
			player.removeIfSelected (mainSet);

			textLogControl.logText (player + " 'S ATTACK: " + getAttack(), Color.white);
			textLogControl.logText (player + " 'S TROOPS: " + arg0, Color.white);

			this.troops = int.Parse (arg0);
			this.troopInput.gameObject.SetActive (false);

			int a = getAttack ();
			int s = getPlanetAttacked ().getShield ();
			int p = getPlanetAttacked ().population;
			int t = getTroops ();

			textLogControl.logText (player + " 'S ATTACK: " + a, Color.white);
			textLogControl.logText (player + " 'S TROOPS: " + t, Color.white);
			textLogControl.logText ("PLANET (" + planet_attacked.getName() + ") SHIELD: " + s, Color.white);
			textLogControl.logText ("PLANET (" + planet_attacked.getName() + ") POPULATION: " + p, Color.white);

			if (planet_attacked.hasLaser <= 0) {
				if (a + t > s + p) {
					Debug.Log ("attack succeeded" + a + " " + t + " " + s + " " + p);
					textLogControl.logText (player + "'S ATTEMPTED TO ATTACK HAS SUCCEEDED", Color.white);

					if (a + t - s - p >= t) {
						planet_attacked.owner_p.planets.Remove (planet_attacked);
						planet_attacked.owner_p = getPlaying ();
						getPlaying ().addPlanet (planet_attacked);
						planet_attacked.owner = getPlaying ().getName ();
						planet_attacked.shield = 0;
						planet_attacked.population = Mathf.Min (t, planet_attacked.populationMax);
						getPlanet ().setPopulation (getPlanet ().getPopulation () + t - getPlanet ().populationMax);
						textLogControl.logText ("PLANET (" + planet_attacked.getName ().ToUpper() + ") NOW HAS " + planet_attacked.getPopulation() + " POPULATION", Color.white);
						setTroops (0);
						attack = 0;
						foreach (Card card in planet_attacked.cards) {
							player.addCard (card);
						}
						planet_attacked.cards = new List<Card> ();
						planet_attacked.text.GetComponent<Text> ().text = planet_attacked.owner;

					} else {
						planet_attacked.owner_p.planets.Remove (planet_attacked);
						planet_attacked.owner_p = getPlaying ();
						getPlaying ().addPlanet (planet_attacked);
						planet_attacked.owner = getPlaying ().getName ();
						planet_attacked.shield = 0;
						planet_attacked.population = Mathf.Min (a + t - s - p, planet_attacked.populationMax);
						getPlanet ().setPopulation (getPlanet ().getPopulation () + a + t - s - p - getPlanet ().populationMax);
						textLogControl.logText ("PLANET (" + planet_attacked.getName ().ToUpper() + ") NOW HAS " + planet_attacked.getPopulation() + " POPULATION", Color.white);
						setTroops (0);
						attack = 0;
						foreach (Card card in planet_attacked.cards) {
							player.addCard (card);
						}
						planet_attacked.cards = new List<Card> ();
						planet_attacked.text.GetComponent<Text> ().text = planet_attacked.owner;

					}
				} else if (a + t < s + p) {
					Debug.Log ("attack failed");
					textLogControl.logText (player + "'S ATTEMPTED TO ATTACK HAS FAILED", Color.white);
					if (s + p - a - t >= p) {
						planet_attacked.shield = s + p - t - a - p;
						textLogControl.logText ("PLANET (" + planet_attacked.getName () + ") NOW HAS " + planet_attacked.shield + " SHIELD", Color.white);
						textLogControl.logText ("PLANET (" + planet_attacked.getName () + ") NOW HAS " + p + " POPULATION", Color.white);
						planet_attacked.population = Mathf.Min (p, planet_attacked.populationMax);
						setTroops (0);
						attack = 0;
					} else {
						planet_attacked.shield = 0;
						planet_attacked.population = Mathf.Min (a + t - s - p, planet_attacked.populationMax);
						textLogControl.logText ("PLANET (" + planet_attacked.getName () + ") NOW HAS 0 SHIELD", Color.white);
						textLogControl.logText ("PLANET (" + planet_attacked.getName () + ") NOW HAS " + planet_attacked.population + " POPULATION", Color.white);
						setTroops (0);
						attack = 0;
					}
				} else {
					textLogControl.logText ("DRAW", Color.white);
					textLogControl.logText ("PLANET (" + planet_attacked.getName () + ") IS NOW EMPTY", Color.white);
					Debug.Log ("draw");
					planet_attacked.removeByName ("LASER", mainSet);
					planet_attacked.owner = null;
					planet_attacked.owner_p.removePlanet (planet_attacked);
					getPlaying ().removePlanet (planet_attacked);
					planet_attacked.owner_p = null;
					planet_attacked.shield = 0;
					planet_attacked.population = 0;
					setTroops (0);
					attack = 0;
				}
			} else {
				textLogControl.logText (player + "'S ATTEMPTED TO ATTACK HAS FAILED: LASER", Color.white);
				setTroops (0);
				attack = 0;
			}
			foreach (Card card in player.getCards())
				card.destroyClone ();
			foreach (Planet planet in gameScript.planets) {
				planet.setListener ("normal");
				planet.resetColor ();
			}
			gameScript.nextRound ();
			player.displayCards ();
			getGameScript ().winCondition ();
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void clearCard(){
		cards = new List<Card> ();
	}

	public void setTroops(int arg0){
		troops = arg0;
	}

	public int getTroops(){
		return troops;
	}

	public void setPlaceHolderText(){
		troopPlaceholder.text = "ENTER NUMBER OF TROOPS(MAX: " + (getPlanet ().getPopulation () - 1) + ")";
	}

	public void addAttack(int attack){
		this.attack += attack;
		GameObject.Find ("Game/ActionInfo").GetComponent<Text> ().text = this.attack + "";
	}

	public void addDefense(int defense){
		this.defense += defense;
		GameObject.Find ("Game/ActionInfo").GetComponent<Text> ().text = this.defense + "";
	}

	public void setPlanet(Planet planet){
		this.planet = planet;
	}

	public void setPlanetAttacked(Planet planet){
		this.planet_attacked = planet;
	}

	public void setCard(Card card){
		this.card = card;
	}

	public void setPlaying(Player player){
		this.player = player;
		text_balance_sell.text = "BALANCE: " + getPlaying ().getBalance ();
		text_balance_purchase.text = "BALANCE: " + getPlaying ().getBalance ();
	}

	public GameScript getGameScript(){
		return gameScript;
	}

	public Planet getPlanet(){
		return planet;
	}

	public Planet getPlanetAttacked(){
		return planet_attacked;
	}

	public Card getCard(){
		return card;
	}

	public Player getPlaying(){
		return player;
	}

	public MainSet getMainSet(){
		return mainSet;
	}

	public Shop getShop(){
		return shop;
	}

	public int getAttack(){
		return attack;
	}

	public int getDefense(){
		return defense;
	}

	public TextLogControl getLogControl(){
		return textLogControl;
	}
}
