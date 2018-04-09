using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop{

	private Handler handler;

	private List<Card> cards;
	private int size;

	private List<Card> tmp;
	private List<GameObject> tmpClone = new List<GameObject> ();

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
		foreach (Card card in cards)
			card.destroyClone ();
		for (int i = 0; i < cards.Count/2; i++) {
			cards [i].setPositionAndSize (new Vector2 (-346 + 700 / cards.Count * 2 * i, 105), new Vector2 (108, 151));
			cards [i].setParent (handler.canvas_purchase);
			cards [i].instantiateCard ();
		}
		for (int i = cards.Count / 2; i < cards.Count; i++) {
			cards [i].setPositionAndSize (new Vector2 (-346 + 700 / cards.Count * 2 * (i - cards.Count / 2), -86), new Vector2 (108, 151));
			cards [i].setParent (handler.canvas_purchase);
			cards [i].instantiateCard ();
		}
	}

	public void displayCardSell(){
		//-412 340
		tmp = handler.getPlaying ().getCards ();
		for (int i = 0; i < tmp.Count; i++) {
			//tmp [i].setPositionAndSize (new Vector2 (-412 + 1200 / cards.Count / 2 * i+1200/4, 0), new Vector2 (148, 206));
			tmp [i].setPositionAndSize (new Vector2 (-346 + 700 / cards.Count * 2 * i, 0), new Vector2 (108, 151));
			tmp [i].setParent (handler.canvas_sell);
			tmpClone.Add (tmp [i].instantiateCard ());
		}
	}

	public void destroyTmp(){
		foreach (GameObject clone in tmpClone) {
			GameObject.Destroy (clone.gameObject);
		}
	}

	public void addCard(Card card){
		cards.Add (card);
	}

	public void fillCard(){
		for (int i = 0; i < size - cards.Count; i++) {
			handler.getMainSet ().drawByTag ("E", this);
		}
	}

	public void removeCard(Card card, MainSet mainSet){
		mainSet.addCard (card);
		cards.Remove (card);
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
