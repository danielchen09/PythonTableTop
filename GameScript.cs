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
	private Button button_deploy;

	private List<Planet> planets;

	public void instantiatePlanet (GameObject parent){
		planets.Add(new Planet ("planet_blue", parent, new Vector2(-413, 128), new Vector2(160, 160), canvas_planetInfo));
		planets.Add(new Planet ("planet_blueBlack", parent, new Vector2(-389, -140), new Vector2(113, 113), canvas_planetInfo));
		planets.Add(new Planet ("planet_greenBlack", parent, new Vector2(-229, 11), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_greenBlue", parent, new Vector2(0, -185), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_orange", parent, new Vector2(154, 145), new Vector2(140, 140), canvas_planetInfo));
		planets.Add(new Planet ("planet_purpleGreen", parent, new Vector2(391, 76), new Vector2(242, 242), canvas_planetInfo));
		planets.Add(new Planet ("planet_rainbow", parent, new Vector2(-174, -140), new Vector2(124, 124), canvas_planetInfo));
		planets.Add(new Planet ("planet_x", parent, new Vector2(-74, 139), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_yellowBlack", parent, new Vector2(193, -154), new Vector2(100, 100), canvas_planetInfo));
		planets.Add(new Planet ("planet_yellowPurple", parent, new Vector2(26, -22), new Vector2(140, 140), canvas_planetInfo));
	}

	public void deploy(){

		foreach(Planet planet in planets){
			planet.setListener ("deploy");
			planet.setDeployName (playing.getName());
		}
	}

	public void exitPlanetInfo(){
		canvas_planetInfo.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		foreach (Planet planet in planets) {
			planet.setListener ("normal");
		}
	}

	// Use this for initialization
	void Start () {
		audioSource.clip = buttonClip;
		audioSource.Play ();

		canvas_planetInfo.gameObject.SetActive (false);
		canvas_game.gameObject.SetActive (true);
		canvas_menu.gameObject.SetActive (false);
		canvas_deploy.gameObject.SetActive (false);

		GameObject.Find ("Game/button_deploy").GetComponent<Button> ().onClick.AddListener (deploy);

		player_num = PlayerPrefs.GetInt ("player_num");
		players = new List<Player> ();

		Debug.Log ("num" + player_num);
		for (int i = 0; i < player_num; i++) {
			Debug.Log ("name_"+i + PlayerPrefs.GetString ("name_"+i));
			players.Add(new Player(PlayerPrefs.GetString ("name_"+i)));
		}
		round = 0;
		playing = players [round];

		planets = new List<Planet> ();
		instantiatePlanet (canvas_game);

		MainSet mainSet = new MainSet (200, canvas_game);

		for (int i = 0; i < 10; i++) {
			mainSet.draw (players [round]);
			players [round].displayCards ();
		}
	}

	void FixedUpdate(){
		GameObject.Find ("Game/Debug").GetComponent<Text> ().text = Input.mousePosition.x + " " + Input.mousePosition.y;
	}
}
