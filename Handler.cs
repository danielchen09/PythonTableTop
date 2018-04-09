using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handler{

	private int attack;
	private int defense;

	private GameScript gameScript;
	private Planet planet;
	private Planet planet_attacked;
	private Card card;
	private MainSet mainSet;
	private Player player;
	private Shop shop;

	public GameObject canvas_game;
	public GameObject canvas_menu;
	public GameObject canvas_planetInfo;
	public GameObject canvas_deploy;
	public GameObject canvas_log;
	public GameObject canvas_option;
	public GameObject canvas_purchase;
	public GameObject canvas_sell;

	public TextLogControl textLogControl;

	public Text ActionInfo;

	public Handler(GameScript gameScript, MainSet mainSet, Shop shop, GameObject[] canvas, TextLogControl textLogControl){
		this.gameScript = gameScript;
		this.mainSet = mainSet;
		this.shop = shop;
		this.ActionInfo = GameObject.Find ("Game/ActionInfo").GetComponent<Text> ();
		canvas_game = canvas [0];
		canvas_menu = canvas [1];
		canvas_planetInfo = canvas [2];
		canvas_deploy = canvas [3];
		canvas_log = canvas [4];
		canvas_option = canvas [5];
		canvas_purchase = canvas [6];
		canvas_sell = canvas [7];
		this.textLogControl = textLogControl;
	}

	public void addAttack(int attack){
		this.attack += attack;
		GameObject.Find ("Game/ActionInfo").GetComponent<Text> ().text = this.attack + "";
	}

	public void setPlanet(Planet planet){
		this.planet = planet;
	}

	public void setPlanetAttacked(Planet planet){
		this.planet_attacked = planet;
	}

	public void setCard(Card card){
		this.card = card;
	}

	public void setPlaying(Player player){
		if (this.player != null) {
			foreach (Card card in player.getCards())
				card.destroyClone ();	
		}
		this.player = player;
	}

	public GameScript getGameScript(){
		return gameScript;
	}

	public Planet getPlanet(){
		return planet;
	}

	public Card getCard(){
		return card;
	}

	public Player getPlaying(){
		return player;
	}

	public MainSet getMainSet(){
		return mainSet;
	}

	public Shop getShop(){
		return shop;
	}

	public TextLogControl getLogControl(){
		return textLogControl;
	}
}
