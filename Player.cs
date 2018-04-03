using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SerializeField]
	private string name;

	[SyncVar(hook = "onPlayer_numChange")]
	private int player_num;

	private int player_count;

	private List<string> players;

	private GameObject image;

	[SerializeField]
	public GameObject placeholder;
	// Use this for initialization
	void Start () {
		this.GetComponent<InputField> ().onEndEdit.AddListener (submitName);
		this.gameObject.SetActive(false);
		player_num = 0;
		player_count = 0;

		players = new List<string> ();

		image = new GameObject ();
		image.AddComponent<Image> ();
		image.AddComponent<RectTransform> ();
		image.GetComponent<RectTransform> ().sizeDelta = new Vector2 (50, 50);
		image.GetComponent<RectTransform> ().position = new Vector2 (0, 0);

		if (this.isServer) {
			this.gameObject.SetActive (true);
			this.GetComponentInChildren<Transform>().Find("Placeholder").GetComponent<Text>().text = "ENTER PLAYER NUM...";
		}
	}

	void onPlayer_numChange(int player_num){
		if (!this.isServer && player_num > 0) {
			this.gameObject.SetActive (true);
		}
		Debug.Log ("onPlayer_numChange");
	}

	void onPlayer_countChange(int player_count){
		if (player_count>=player_num) {
			this.gameObject.SetActive (false);
		}
		Debug.Log ("onPlayer_countChange");
	}


	void submitName(string arg0){
		Debug.Log ("submitName");
		if (!this.isServer) {
			Debug.Log ("lksdgfadlk");
			image = Instantiate (image, new Vector2 (0, 0), Quaternion.identity);
			image.transform.SetParent (GameObject.Find ("BackGround").transform);
		}
		if (this.isServer) {
			player_num = int.Parse (arg0);
			Debug.Log ("server");
		} else {
			players.Add (arg0);
			player_count++;
			Debug.Log ("client");
		}
	}

	void submitNameFromClient(string arg0){
	
	}

	void FixedUpdate(){

	}
}
