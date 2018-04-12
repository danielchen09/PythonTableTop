using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	public AudioClip buttonClip;
	public AudioSource audioSource;

	private int player_num;
	private Player playing;
	public List<Player> players;
	public int round;

	public GameObject canvas_game;
	public GameObject canvas_menu;
	public GameObject canvas_planetInfo;
	public GameObject canvas_deploy;
	public GameObject canvas_log;
	public GameObject canvas_option;
	public GameObject canvas_purchase;
	public GameObject canvas_sell;
	public GameObject canvas_block;
	public GameObject canvas_win;

	private Button button_deploy;
	public Button button_cancel;

	public TextLogControl logControl;

	private Handler handler;

	public List<Planet> planets;

	private MainSet mainSet;

	public bool action = false;
	public bool sel = false;

	public Text text_win;
	public Text textResult;

	public void instantiatePlanet (GameObject parent){
		planets.Add(new Planet ("planet_blue", parent, new Vector2(-413, 128), new Vector2(160, 160), canvas_planetInfo, 2, 5));
		planets.Add(new Planet ("planet_blueBlack", parent, new Vector2(-389, -95), new Vector2(113, 113), canvas_planetInfo, 5, 2));
		planets.Add(new Planet ("planet_greenBlack", parent, new Vector2(-229, 11), new Vector2(100, 100), canvas_planetInfo, 5, 2));
		planets.Add(new Planet ("planet_greenBlue", parent, new Vector2(0, -185), new Vector2(100, 100), canvas_planetInfo, 5, 2));
		planets.Add(new Planet ("planet_orange", parent, new Vector2(154, 145), new Vector2(140, 140), canvas_planetInfo, 4, 4));
		planets.Add(new Planet ("planet_purpleGreen", parent, new Vector2(391, 76), new Vector2(242, 242), canvas_planetInfo, 2, 5));
		planets.Add(new Planet ("planet_rainbow", parent, new Vector2(-174, -140), new Vector2(124, 124), canvas_planetInfo, 4, 4));
		planets.Add(new Planet ("planet_x", parent, new Vector2(-74, 139), new Vector2(100, 100), canvas_planetInfo, 5, 2));
		planets.Add(new Planet ("planet_yellowBlack", parent, new Vector2(193, -154), new Vector2(100, 100), canvas_planetInfo, 5, 2));
		planets.Add(new Planet ("planet_yellowPurple", parent, new Vector2(26, -22), new Vector2(140, 140), canvas_planetInfo, 4, 4));
		foreach (Planet planet in planets) 
			planet.setLogControl (logControl);
	}

	public void deploy(){
		canvas_game.gameObject.SetActive (false);
		canvas_menu.gameObject.SetActive (true);
	}

	public void deploy_card(){
		handler.getGameScript ().button_cancel.gameObject.SetActive (true);
		foreach (Planet planet in planets) {
			planet.setListener ("defense1");
		}
		foreach (Player player in players)
			foreach(Card card in player.getCards())
				card.setHandler (handler);
		canvas_game.gameObject.SetActive (true);
		canvas_menu.gameObject.SetActive (false);
		handler.ActionInfo.text = "CHOOSE THE PLANET TO DEPLOY CARDS TO";
	}

	public void deploy_troop(){
		handler.getGameScript ().button_cancel.gameObject.SetActive (true);
		foreach (Card card in playing.getCards())
			card.destroyClone ();
		foreach(Planet planet in planets){
			planet.setListener ("deploy");
			planet.setDeployName (playing.getName());
		}
		canvas_game.gameObject.SetActive (true);
		canvas_menu.gameObject.SetActive (false);
	}

	public void openLog(){
		canvas_game.gameObject.SetActive (false);
		canvas_log.gameObject.SetActive (true);
	}

	public void exitPlanetInfo(){
		canvas_planetInfo.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		foreach (Planet planet in planets) {
			planet.setListener ("normal");
		}
		playing.displayCards ();
		if (action) {
			canvas_game.gameObject.SetActive (false);
			canvas_block.gameObject.SetActive (true);
			action = false;
		}
	}

	public void exitLog(){
		canvas_log.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
	}

	public void enterSell(){
		foreach (Card card in handler.getPlaying().getCards()) {
			card.destroyClone ();
		}
		canvas_sell.gameObject.SetActive (true);
		canvas_option.gameObject.SetActive (false);
		handler.canvas_sell.transform.Find ("text_balance").GetComponent<Text> ().text = "BALANCE: " + handler.getPlaying ().getBalance ();
		handler.getShop ().displayCardSell ();
		foreach (Card card in handler.getPlaying().getCards()) {
			card.setHandler (handler);
			card.setListener ("sell");
		}
	}

	public void exitSell(){

		canvas_sell.gameObject.SetActive (false);
		canvas_option.gameObject.SetActive (true);
		foreach (Card card in handler.getPlaying().getCards()) {
			card.setHandler (handler);
			card.setListener ("normal");
		}
	}

	public void enterPurchase(){
		canvas_purchase.gameObject.SetActive (true);
		canvas_option.gameObject.SetActive (false);
		handler.canvas_purchase.transform.Find ("text_balance").GetComponent<Text> ().text = "BALANCE: " + handler.getPlaying ().getBalance ();
		handler.getShop ().displayCardPurchase ();
		foreach (Card card in handler.getShop().getCards()) {
			card.setHandler (handler);
			card.setListener ("purchase");
		}
	}

	public void exitPurchase(){
		canvas_purchase.gameObject.SetActive (false);
		canvas_option.gameObject.SetActive (true);
		foreach (Card card in handler.getPlaying().getCards()) {
			card.setHandler (handler);
			card.setListener ("normal");
		}
	}

	public void enterShopOption(){	
		canvas_option.gameObject.SetActive (true);
		canvas_game.gameObject.SetActive (false);
		if (handler.getShop ().getCards ().Count < 8) {
			while (handler.getShop ().getCards ().Count < 8) {
				handler.getShop ().fillCard ();
			}
		}
	}

	public void exitShopOption(){
		canvas_option.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		foreach (Card card in handler.getPlaying().getCards()) {
			card.setParent (canvas_game);
		}
		handler.getShop ().fillCard ();
		foreach (Card card in playing.getCards())
			card.destroyClone ();
		playing.displayCards ();
		if (action) {
			foreach (Card card in playing.getCards())
				card.destroyClone ();
			nextRound ();
			playing.displayCards ();
			action = false;
			canvas_option.gameObject.SetActive (false);
			canvas_game.gameObject.SetActive (false);
			canvas_block.gameObject.SetActive (true);
		}
	}

	public void onCancel(){
		playing.displayCards ();

		foreach (Card card in playing.getCards()) {
			card.setListener ("normal");
			card.resetColor ();
		}
		foreach (Planet planet in planets) {
			planet.setListener ("normal");
			planet.resetColor ();
		}
		button_cancel.gameObject.SetActive (false);
		handler.troopInput.gameObject.SetActive (false);
		handler.ActionInfo.text = playing.getName () + " IS PLAYING";
	}

	public void draw(){
		handler.textLogControl.logText (handler.getPlaying ().getName () + " HAS DREW A CARD", Color.white);
		for(int i=0; i<handler.getPlaying().planets.Count; i++){
			mainSet.drawWithDisaster ("M", playing);
		}
		playing.displayCards ();
		foreach (Card card in playing.getCards())
			card.destroyClone ();
		nextRound ();
		playing.displayCards ();
		canvas_game.gameObject.SetActive (false);
		canvas_block.gameObject.SetActive (true);
	}

	public void exitMenu(){
		canvas_game.gameObject.SetActive (true);
		canvas_menu.gameObject.SetActive (false);
	}

	public void onAttack(){
		if (playing.hasCard ("SPACECRAFT")) {
			button_cancel.gameObject.SetActive (true);
			foreach (Planet planet in planets) {
				planet.setListener ("attack1");
			}
			foreach (Card card in playing.getCards()) {
				card.setAttributes ();
			}
			handler.ActionInfo.text = "CHOOSE PLANET TO DEPLOY TROOPS FROM";
		} else {
			handler.ActionInfo.text = "ATTACK FAILED: NO SPACECRAFT";
		}
	}

	public void nextRound(){
		round++;
		playing = players [round % players.Count];
		handler.setPlaying (playing);
		handler.ActionInfo.text = playing.getName () + " IS PLAYING";
	}
		
	public void continueGame(){
		canvas_game.gameObject.SetActive (true);
		canvas_block.gameObject.SetActive (false);
	}

	public void endGame(){
		Application.Quit ();
	}

	public void restart(){
		

		Start ();
	}

	public void winCondition(){
		for (int i = players.Count - 1; i >= 0; i--) {
			if (players[i].getTroopsLeft () + players[i].planets.Count <= 0) {
				handler.text_result.text += players[i].getName () + " HAS LOST";
				players.Remove (players[i]);
			}
		}
		if (players.Count <= 1) {
			canvas_planetInfo.gameObject.SetActive (false);
			canvas_game.gameObject.SetActive (false);
			canvas_menu.gameObject.SetActive (false);
			canvas_deploy.gameObject.SetActive (false);
			canvas_log.gameObject.SetActive (false);
			canvas_option.gameObject.SetActive (false);
			canvas_sell.gameObject.SetActive (false);
			canvas_purchase.gameObject.SetActive (false);
			button_cancel.gameObject.SetActive (false);
			canvas_win.gameObject.SetActive (true);
			GameObject.Find ("WIN/text_win").GetComponent<Text> ().text = players [0].getName() + " HAS WON";

		}
	}

	// main function
	void Start () {
		//audio on enter
		audioSource.clip = buttonClip;
		audioSource.Play ();

		//canvas visibility
		canvas_planetInfo.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		canvas_menu.gameObject.SetActive (false);
		canvas_deploy.gameObject.SetActive (false);
		canvas_log.gameObject.SetActive (false);
		canvas_option.gameObject.SetActive (false);
		canvas_sell.gameObject.SetActive (false);
		canvas_purchase.gameObject.SetActive (false);
		button_cancel.gameObject.SetActive (false);
		canvas_win.gameObject.SetActive (false);
		//buttons
		GameObject.Find ("Game/button_deploy").GetComponent<Button> ().onClick.AddListener (deploy);
		GameObject.Find ("Game/button_log").GetComponent<Button> ().onClick.AddListener (openLog);
		GameObject.Find ("Game/button_draw").GetComponent<Button> ().onClick.AddListener (draw);

		//sync data from scene 0
		player_num = PlayerPrefs.GetInt ("player_num");
		players = new List<Player> ();

		for (int i = 0; i < player_num; i++) {
			players.Add(new Player(PlayerPrefs.GetString ("name_"+i)));
		}

		//start game
		round = 0;
		playing = players [round];

		planets = new List<Planet> ();
		instantiatePlanet (canvas_game);

		mainSet = new MainSet (200);

		Shop shop = new Shop (8);

		GameObject[] param = {
			canvas_game,
			canvas_menu,
			canvas_planetInfo,
			canvas_deploy,
			canvas_log,
			canvas_option,
			canvas_purchase,
			canvas_sell,
			canvas_block
		};

		handler = new Handler (this, mainSet, shop, param, logControl);
		handler.textLogControl = logControl;

		handler.text_result = canvas_block.transform.Find("text_result").GetComponent<Text> (); 
			
		handler.setPlaying (playing);
		GameObject.Find ("Game/ActionInfo").GetComponent<Text> ().text = playing.getName () + " IS PLAYING";
		handler.text_result.text = "";

		canvas_block.gameObject.SetActive (false);

		mainSet.setHandler (handler);
		shop.setHanlder (handler);
		foreach (Planet planet in planets)
			planet.setHandler (handler);
		
		foreach (Player player in players) {
			player.setHandler (handler);
			mainSet.drawByName ("SPACECRAFT", player);
		}

		foreach (Planet planet in planets)
			for (int i = 0; i < planet.resourceMax; i++)
				mainSet.drawByTag ("M", planet);

		playing.displayCards ();

		winCondition ();
	}

	public void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			if (!sel) {
				canvas_win.gameObject.SetActive (true);
			} else {
				canvas_win.gameObject.SetActive (false);
			}
			sel = !sel;
		}
	}
}
