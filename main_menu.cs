using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour {

	public InputField input;
	public Text placeHolder;
	public Button button_start;
	public List<string> players;

	private int player_num = 0;
	private int player_count = 0;

	public AudioClip inputClip;
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		players = new List<string> ();

		input.onEndEdit.AddListener (submitName);
		placeHolder.text = "ENTER PLAYER AMOUNT...";
		audioSource.clip = inputClip;

		button_start.onClick.AddListener (startGame);
		button_start.gameObject.SetActive (false);

	}
	
	// Update is called once per frame
	void submitName(string arg0){
		if (player_count == 0) {
			player_num = player_count = int.Parse (arg0);
			input.text = "";
			placeHolder.text = "ENTER PLAYER " + (player_num - player_count + 1) + " NAME....";
			audioSource.Play ();
		} else if (player_count > 1) {
			player_count -= 1;
			placeHolder.text = "ENTER PLAYER " + (player_num - player_count + 1) + " NAME....";
			players.Add (arg0);
			input.text = "";
			audioSource.Play ();
		} else {
			players.Add (arg0);
			placeHolder.text = "PRESS START";
			input.text = "";
			button_start.gameObject.SetActive (true);
			audioSource.Play ();
		}
	}

	void startGame(){
		PlayerPrefs.SetInt ("player_num", player_num);
		for(int i=0; i<player_num; i++){
			Debug.Log (players [i]);
			PlayerPrefs.SetString ("name_"+i, players[i]);
		}
		SceneManager.LoadScene ("Game");
	}
}
