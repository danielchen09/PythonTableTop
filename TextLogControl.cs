using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLogControl : MonoBehaviour {
	public GameObject textTemplate;
	private List<GameObject> textItems = new List<GameObject> ();

	void Start (){
	}

	public void logText(string text, Color color){

		if (textItems.Count == 30) {
			GameObject tmp = textItems [0];
			Destroy (tmp.gameObject);
			textItems.Remove (tmp);
		}

		GameObject newText = Instantiate (textTemplate) as GameObject;
		newText.SetActive (true);

		newText.GetComponent<TextLogItem> ().setText (text, color);
		newText.transform.SetParent (textTemplate.transform.parent, false);

		textItems.Add (newText.gameObject);
	}
}
