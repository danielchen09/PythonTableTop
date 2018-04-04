using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{

	private string name;
	private List<Card> cards;
	private int money;

	public Player(string name){
		this.name = name;
		money = 0;
		cards = new List<Card> ();
	}

	public string getName(){
		return name;
	}

	public void displayCards(){
		GameObject.Destroy (GameObject.FindWithTag ("CARD"));
		//x from -224 to 261
		//y -252
		for (int i = 0; i < cards.Count; i++) {
			cards [i].setPositionAndSize (new Vector2 (-224 + 485 / cards.Count / 2 * i+485/4, -242), new Vector2 (142, 197));
			cards [i].instantiateCard ();
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void removeCard(Card card, MainSet mainSet){
		mainSet.addCard (card);
		cards.Remove (card);
	}

	public void removeCardByTag(string tag, MainSet mainSet){
		for (int i = cards.Count - 1; i >= 0; i++) {
			if (cards [i].getTag ().Equals(tag)) {
				mainSet.addCard (cards [i]);
				cards.Remove (cards [i]);
			}
		}
	}

}
