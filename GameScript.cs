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
	private List<Player> players;
	private int round;

	public GameObject canvas_game;
	public GameObject canvas_menu;
	public GameObject canvas_planetInfo;
	public GameObject canvas_deploy;
	public GameObject canvas_log;
	public GameObject canvas_option;
	public GameObject canvas_purchase;
	public GameObject canvas_sell;

	private Button button_deploy;

	public TextLogControl logControl;

	private Handler handler;

	private List<Planet> planets;

	public void instantiatePlanet (GameObject parent){
		planets.Add(new Planet ("planet_blue", parent, new Vector2(-413, 128), new Vector2(160, 160), canvas_planetInfo));
		planets.Add(new Planet ("planet_blueBlack", parent, new Vector2(-389, -95), new Vector2(113, 113), canvas_planetInfo));
		planets.Add(new Planet ("planet_greenBlack", parent, new Vector2(-229, 11), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_greenBlue", parent, new Vector2(0, -185), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_orange", parent, new Vector2(154, 145), new Vector2(140, 140), canvas_planetInfo));
		planets.Add(new Planet ("planet_purpleGreen", parent, new Vector2(391, 76), new Vector2(242, 242), canvas_planetInfo));
		planets.Add(new Planet ("planet_rainbow", parent, new Vector2(-174, -140), new Vector2(124, 124), canvas_planetInfo));
		planets.Add(new Planet ("planet_x", parent, new Vector2(-74, 139), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_yellowBlack", parent, new Vector2(193, -154), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_yellowPurple", parent, new Vector2(26, -22), new Vector2(140, 140), canvas_planetInfo));
		foreach (Planet planet in planets)
			planet.setLogControl (logControl);
	}

	public void deploy(){

		foreach(Planet planet in planets){
			planet.setListener ("deploy");
			planet.setDeployName (playing.getName());
		}
		round++;
		playing = players [round % player_num];
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
	}

	public void exitLog(){
		canvas_log.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
	}

	public void enterSell(){
		canvas_sell.gameObject.SetActive (true);
		canvas_option.gameObject.SetActive (false);
		handler.getShop ().displayCardSell ();
		foreach (Card card in handler.getShop().getCards()) {
			card.setListener ("sell");
		}
	}

	public void exitSell(){
		canvas_sell.gameObject.SetActive (false);
		canvas_option.gameObject.SetActive (true);
	}

	public void enterPurchase(){
		canvas_purchase.gameObject.SetActive (true);
		canvas_option.gameObject.SetActive (false);
		handler.getShop ().displayCardPurchase ();
	}

	public void exitPurchase(){
		canvas_purchase.gameObject.SetActive (false);
		canvas_option.gameObject.SetActive (true);
	}

	public void enterShopOption(){	
		canvas_option.gameObject.SetActive (true);
		canvas_game.gameObject.SetActive (false);
	}

	public void exitShopOption(){
		canvas_option.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		playing.displayCards ();
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
		//buttons
		GameObject.Find ("Game/button_deploy").GetComponent<Button> ().onClick.AddListener (deploy);
		GameObject.Find ("Game/button_log").GetComponent<Button> ().onClick.AddListener (openLog);

		//sync data from scene 0
		player_num = PlayerPrefs.GetInt ("player_num");
		players = new List<Player> ();

		Debug.Log ("num" + player_num);
		for (int i = 0; i < player_num; i++) {
			Debug.Log ("name_"+i + PlayerPrefs.GetString ("name_"+i));
			players.Add(new Player(PlayerPrefs.GetString ("name_"+i)));
		}

		//start game
		round = 0;
		playing = players [round];

		planets = new List<Planet> ();
		instantiatePlanet (canvas_game);

		MainSet mainSet = new MainSet (200);

		Shop shop = new Shop (8);

		GameObject[] param = {
			canvas_game,
			canvas_menu,
			canvas_planetInfo,
			canvas_deploy,
			canvas_log,
			canvas_option,
			canvas_purchase,
			canvas_sell
		};

		handler = new Handler (mainSet, shop, param);

		handler.setPlaying (playing);

		playing.setHandler (handler);
		mainSet.setHandler (handler);
		shop.setHanlder (handler);

		for (int i = 0; i < 20; i++) {
			mainSet.drawByTag ("M", playing);
		}
		playing.displayCards ();
	}
}
