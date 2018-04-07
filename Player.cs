using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{

	private string name;
	private List<Card> cards;
	private int balance;

	private Handler handler;

	public Player(string name){
		this.name = name;
		balance = 10000;
		cards = new List<Card> ();
	}

	public string getName(){
		return name;
	}

	public void displayCards(){
		GameObject.Destroy (GameObject.FindWithTag ("CARD"));
		//x from -224 to 261
		//y -252
		int i=0;
		foreach (Card card in cards) {
			card.setPositionAndSize (new Vector2 (-224 + 485 / cards.Count / 2 * i+485/4, -242), new Vector2 (142, 197));
			card.setParent (handler.canvas_game);
			card.instantiateCard ();
			i++;
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void removeCard(Card card, MainSet mainSet){
		mainSet.addCard (card);
		cards.Remove (card);
	}

	public void sellCard(Card card, Shop shop){
		shop.addCard (card);
		cards.Remove (card);
	}

	public void removeCardByTag(string tag, MainSet mainSet){
		for (int i = cards.Count - 1; i >= 0; i--) {
			if (cards [i].getTag ().Equals(tag)) {
				mainSet.addCard (cards [i]);
				cards.Remove (cards [i]);
			}
		}
	}

	public List<Card> getCards(){
		return cards;
	}

	public int getBalance(){
		return balance;
	}

	public void setBalance(int balance){
		this.balance = balance;
	}

	public void setHandler(Handler handler){
		this.handler = handler;
	}

}
