using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Round : MonoBehaviour {

    float now;
    int count_player = 3;
    int round = 3;

    public Text player;

	// Use this for initialization
	void Start () {
        now = Time.time * 1000;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time * 1000 - now >= 3000) {
            player.text = "Player " + (round % count_player + 1);
            round++;
            now = Time.time * 1000;
        }
	}
}
