using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour {

	public GameObject c1;
	public GameObject c2;
	public GameObject c3;
	public GameObject c4;
	public GameObject c5;
	public GameObject c6;
	public GameObject c7;
	public GameObject c8;
	public GameObject c9;
	public GameObject[] canvas;

	private int now;

	// Use this for initialization
	void Start () {
		canvas = new GameObject[9];

		canvas [0] = c1;
		canvas [1] = c2;
		canvas [2] = c3;
		canvas [3] = c4;
		canvas [4] = c5;
		canvas [5] = c6;
		canvas [6] = c7;
		canvas [7] = c8;
		canvas [8] = c9;

		for (int i = 1; i < canvas.Length; i++) {
			canvas [i].gameObject.SetActive (false);
		}

		canvas [0].gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (now < canvas.Length - 1) {
				canvas [now].gameObject.SetActive (false);
				canvas [now + 1].gameObject.SetActive (true);
				now++;
			} else {
				SceneManager.LoadScene ("Game");
			}
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			if(now > 0){
				canvas[now].gameObject.SetActive(false);
				canvas[now-1].gameObject.SetActive(true);
				now--;
			}
		}
	}
}
