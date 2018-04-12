using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{

	private string name;
	private List<Card> cards;
	public List<Planet> planets;
	private int balance;
	private int troopsLeft;

	private Handler handler;

	public Player(string name){
		this.name = name;
		this.troopsLeft = 20;
		balance = 99999;
		cards = new List<Card> ();
		planets = new List<Planet> ();
	}

	public string getName(){
		return name;
	}

	public void displayCards(){
		foreach (Card card in cards)
			card.destroyClone ();
		//x from -224 to 261
		//y -252
		int i=0;
		foreach (Card card in cards) {
			card.setPositionAndSize (new Vector2 (-224 + 485 / cards.Count / 2 * i+485/4, -242), new Vector2 (142, 197));
			card.setParent (handler.canvas_game);	
			card.instantiateCard ();
			card.setListener ("normal");
			i++;
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void removeCard(Card card, MainSet mainSet){
		mainSet.addCard (card);
		card.destroyClone ();
		cards.Remove (card);
	}

	public void removeCardByTag(string tag, MainSet mainSet){
		for (int i = cards.Count - 1; i >= 0; i--) {
			if (cards [i].getTag ().Equals(tag)) {
				cards [i].destroyClone ();
				mainSet.addCard (cards [i]);
				cards.Remove (cards [i]);
			}
		}
	}

	public void removeIfSelected(Planet planet){
		for (int i = cards.Count - 1; i >= 0; i--) {
			if (cards [i].isSelected) {
				planet.addCard (cards [i]);
				cards [i].destroyClone ();
				cards.Remove (cards [i]);
			}
		}
	}

	public void removeIfSelected(MainSet mainSet){
		for (int i = cards.Count - 1; i >= 0; i--) {
			if (cards [i].isSelected) {
				mainSet.addCard (cards [i]);
				cards [i].destroyClone ();
				cards.Remove (cards [i]);
			}
		}
	}

	public void removeByName(string name, MainSet mainSet){
		int i = 0;
		foreach (Card card in cards) {
			if (!cards [i].getName ().Equals (name)) {
				i++;
			}
		}
		if (cards [i].getName ().Equals (name)) {
			mainSet.addCard (cards [i]);
			cards [i].destroyClone ();
			cards.Remove (cards [i]);
		}
	}

	public bool hasCard(string name){
		foreach (Card card in cards)
			if (card.getName ().Equals (name))
				return true;
		return false;
	}

	public void addPlanet(Planet planet){
		this.planets.Add (planet);
	}

	public void removePlanet(Planet planet){
		this.planets.Remove (planet);
	}

	public List<Card> getCards(){
		return cards;
	}

	public int getTroopsLeft(){
		return troopsLeft;
	}

	public int getBalance(){
		return balance;
	}

	public void setTroopsLeft(int troops){
		this.troopsLeft = troops;
	}

	public void setBalance(int balance){
		this.balance = balance;
	}

	public void setHandler(Handler handler){
		this.handler = handler;
	}
		

}