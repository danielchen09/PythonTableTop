using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop{

	private Handler handler;

	private List<Card> cards;
	private int size;


	public Shop(int size){
		this.size = size;
		cards = new List<Card> ();
	}

	public void initializeShop(){
		for (int i = 0; i < size; i++)
			handler.getMainSet ().drawByTag ("E", this);
	}
		
	public void displayCardPurchase(){
		//x: -346 338 y: 105
		//y:-86
		//size: 108, 151
		GameObject.Destroy (GameObject.FindWithTag ("CARD"));
		for (int i = 0; i < cards.Count/2; i++) {
			cards [i].setPositionAndSize (new Vector2 (-346 + 900 / cards.Count * 2 * i, 105), new Vector2 (108, 151));
			cards [i].setParent (handler.canvas_purchase);
			cards [i].instantiateCard ();
			cards [i + cards.Count / 2].setPositionAndSize (new Vector2 (-346 + 900 / cards.Count * 2 * i, -86), new Vector2 (108, 151));
			cards [i + cards.Count / 2].setParent (handler.canvas_purchase);
			cards [i + cards.Count / 2].instantiateCard ();
		}
	}

	public void displayCardSell(){
		//-412 340
		GameObject.Destroy (GameObject.FindWithTag ("CARD"));
		List<Card> tmp = handler.getPlaying ().getCards ();
		for (int i = 0; i < tmp.Count; i++) {
			tmp [i].setPositionAndSize (new Vector2 (-412 + 500 / cards.Count / 2 * i+500/4, 0), new Vector2 (148, 206));
			tmp [i].setParent (handler.canvas_sell);
			tmp [i].instantiateCard ();
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void removeCard(Card card, MainSet mainSet){
		mainSet.addCard (card);
		cards.Remove (card);
		mainSet.draw (this);
	}

	public void purchaseCard(Card card, Player player){
		card.setParent (handler.canvas_game);
		player.addCard (card);
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

	public void setHanlder(Handler handler){
		this.handler = handler;
		initializeShop ();
	}

	public List<Card> getCards(){
		return cards;
	}
}
