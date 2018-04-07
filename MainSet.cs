using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSet{

	private Handler handler;

	private List<Card> mainSet;
	private int size;
	public string[] ALL_CARDS_NAME = {"MECH", "LASER", "SHIELD", "WALL", "SPACECRAFT", "RECON", "WEAPON", "VOLCANO", "EMP", "CRYSTAL", "IRON", "STONE"};
	public double[] ALL_CARDS_PROBABILITY = {7.5, 1.5, 6.5, 7, 7.5, 8.5, 6.5, 2.5, 2.5, 25, 12.5, 12.5};

	public MainSet(int size){
		this.mainSet = new List<Card> ();
		this.size = size;
	}

	public void initializeSet(){
		for(int i=0; i<ALL_CARDS_NAME.Length; i++){
			for (int j = 0; j < ALL_CARDS_PROBABILITY [i] * size / 100; j++) {
				mainSet.Add (new Card(ALL_CARDS_NAME[i], handler.canvas_game, new Vector2(0,0), new Vector2(0,0)));
				mainSet [i].setHandler (handler);
				mainSet [i].hide();
			}
		}
	}

	public void draw(Player player){
		int r = Random.Range (0, mainSet.Count);
		if (!mainSet [r].getTag ().Equals ("X")) {
			player.addCard (mainSet [r]);
			mainSet.RemoveAt (r);
		} else {
			if (mainSet [r].getName ().Equals ("VOLCANO"))
				player.removeCardByTag ("M", this);
			else
				player.removeCardByTag ("E", this);
		}
	}

	public void draw(Shop shop){
		int r = Random.Range (0, mainSet.Count);
		if (!mainSet [r].getTag ().Equals ("X")) {
			shop.addCard (mainSet [r]);
			mainSet.RemoveAt (r);
		} else {
			if (mainSet [r].getName ().Equals ("VOLCANO"))
				shop.removeCardByTag ("M", this);
			else
				shop.removeCardByTag ("E", this);
		}
	}

	public void drawByTag(string tag, Player player){
		int r = Random.Range (0, mainSet.Count);
		Debug.Log (r);
		while (!mainSet [r].getTag ().Equals (tag)) {
			r = Random.Range (0, mainSet.Count);
		}
		player.addCard (mainSet [r]);
		mainSet.RemoveAt (r);
	
	}

	public void drawByTag(string tag, Shop shop){
		int r = Random.Range (0, mainSet.Count);
		while (!mainSet [r].getTag ().Equals (tag)) {
			r = Random.Range (0, mainSet.Count);
		}
		shop.addCard (mainSet [r]);
		mainSet.RemoveAt (r);

	}

	public void addCard(Card card){
		mainSet.Add (card);
	}

	public void setHandler(Handler handler){
		this.handler = handler;
		initializeSet ();
	}
}
