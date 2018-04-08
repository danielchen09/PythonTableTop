import java.awt.Button;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Scanner;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

class Card{
	private String name;
	private String tag;
	
	public Card(String name) {
		this.name = name;
		setTag();
	}
	
	public String getName() {
		return name;
	}
	
	public String getTag() {
		return tag;
	}
	
	public void setTag() {
		if(name.equals("STONE")||name.equals("IRON")||name.equals("CRYSTAL")) tag = "M";
		else if(name.equals("MECH")||name.equals("LASER")||name.equals("SPACECRAFT")) tag = "E";
		else if(name.equals("VOLCANO")||name.equals("EMP")) tag = "D";
		else tag = "O";
	}
	
	public String toString() {
		return name;
	}
}

class Player{
	private ArrayList<Card> cardset;
	private boolean alive;
	private String name;
	private Display display;
	
	public Player(String name) {
		cardset = new ArrayList<Card>();
		display = new Display();
		alive = true;
		this.name = name;
	}
	
	public ArrayList<Card> getCardset(){
		return cardset;
	}
	
	public void addCard(Card card) {
		cardset.add(card);
	}
	
	public void removeCard(Card card) {
		cardset.remove(card);
	}
	
	public void removeCardByTag(String tag, MainSet mainSet) {
		for(int i=cardset.size()-1; i>=0; i--) 
			if(cardset.get(i).getTag().equals(tag)) {
				mainSet.addCard(cardset.get(i));
				cardset.remove(cardset.get(i));
			}
	}
	
	public void getAllCard() throws IOException {
		System.out.print("cards:\n{");
		for(Card card:cardset)
			System.out.print(card + " ");
		System.out.println("}");
		
	}
	
	public void die() {
		alive = false;
	}
	
	public boolean isAlive() {
		return alive;
	}
}

class MainSet{
	private ArrayList<Card> cardset;
	private int maxSize;
	private Display display;
	
	public MainSet(int maxSize) throws IOException {
		cardset = new ArrayList<Card>();
		this.display = new Display();
		this.maxSize = maxSize;
		initializeSet();
	}
	
	public void addCard(Card card) {
		cardset.add(card);
	}
	
	public void drawByTag(Player player, String tag) {
		Card picked;
		int r;
		
		r = (int)(Math.random()*maxSize);
    	picked = cardset.get(r);
    	
    	if(picked.getTag().equals(tag)) {
    		player.addCard(picked);
    		maxSize -= 1;
        	cardset.remove(r);
    	}else {
    		drawByTag(player, tag);
    	}
    	
	}
	
	public Card drawCard(Player player) throws IOException {
		Scanner scanner = new Scanner(System.in);
		Card picked;
		int r;
		
		r = (int)(Math.random()*maxSize);
    	picked = cardset.get(r);
    	
    	showCard(picked);
        
    	System.out.println(picked.getName());
    	
    	if(picked.getTag().equals("D")) {
    		System.out.println("DISASTER!");
    		if(picked.getName().equals("VOLCANO"))
    			player.removeCardByTag("M", this);
    		else if(picked.getName().equals("EMP"))
    			player.removeCardByTag("E", this);;
    	}
    	
    	System.out.println("Press enter to close");
    	JOptionPane.showMessageDialog(null, "Close");
    	
    	display.disposeCard();
    	
        player.addCard(picked);
        maxSize -= 1;
        cardset.remove(r);
        
        return picked;
	}
	
	
	private void showCard(Card picked) throws IOException {
		display.displayPickedCard(picked);
	}
	
	private void initializeSet() throws IOException {
		int count = 0;
		String everything;
		String[] tuple;
    	BufferedReader br = new BufferedReader(new FileReader("C:\\Users\\Danniel\\Pictures\\cards\\stat.txt"));
    	
    	try {
    	    StringBuilder sb = new StringBuilder();
    	    String line = br.readLine();

    	    while (line != null) {
    	        sb.append(line);
    	        line = br.readLine();
    	    }
    	    everything = sb.toString();
    	} finally {
    	    br.close();
    	}
    	
    	tuple = everything.split(";");
    	for(int i=0; i<tuple.length; i++) {
    		System.out.println(tuple[i].split(",")[0] + " " + (int)(Double.parseDouble(tuple[i].substring(tuple[i].indexOf(",")+1))*maxSize));
    		for(int j=0; j<(int)(Double.parseDouble(tuple[i].substring(tuple[i].indexOf(",")+1))*maxSize); j++) {
    			count++;
    			cardset.add(new Card(tuple[i].split(",")[0]));
    		}
    	}
	}
}

class Display {
	private JFrame frame;
	private JLabel lbl;
	public boolean cont;
	private Card drawed;
	
	public Display() {
		frame = new JFrame();
		lbl = new JLabel();
		this.cont = false;
	}
	
	public void displayPickedCard(Card picked) throws IOException {
		BufferedImage img=ImageIO.read(new File("C:\\Users\\Danniel\\Pictures\\cards\\" + picked + ".png"));
        ImageIcon icon=new ImageIcon(img);
        frame.setLayout(new FlowLayout());
        frame.setBackground(Color.BLACK);
        frame.setSize(755, 1050);
        lbl.setIcon(icon);
        frame.add(lbl);
        frame.setVisible(true);
	}
	
	public void disposeCard() {
		frame.setVisible(false);
    	frame.dispose();
	}
	
	public void displaySet(Player player, MainSet mainSet) throws IOException {
		ArrayList<JLabel> cards = new ArrayList<JLabel>();
		ArrayList<Button> buttons = new ArrayList<Button>();
		int width = 0, height = 0, x, y;
		frame = new JFrame();
		frame.setLayout(null);
		frame.setBackground(Color.BLACK);
		frame.setSize(1280, 920);
		for(Card c:player.getCardset()) {
			JLabel l = new JLabel();
			
			width = (int)(755*((double)1280/player.getCardset().size()/755));
			height = (int)(1050*((double)1280/player.getCardset().size()/755));
			
			ImageIcon icon = new ImageIcon(ImageIO.read(new File("C:\\Users\\Danniel\\Pictures\\cards\\" + c + ".png")));
			Image image = icon.getImage();
			Image newImg = image.getScaledInstance(width, height, Image.SCALE_SMOOTH);
			icon = new ImageIcon(newImg);
			l.setIcon(icon);
			
			cards.add(l);
		}
		
		for(int i=0; i<cards.size(); i++) {
			JButton b = new JButton("delete");
			Card cardRef;
			JLabel card = cards.get(i);
			
			card.setBounds(width*i, 0, width, height);
			b.setBounds(width*i+width/2, 0, width/3, height/5);
			
			cardRef = player.getCardset().get(i);
			
			b.addActionListener(new ActionListener() {
				@Override
				public void actionPerformed(ActionEvent e) {
					card.setVisible(false);
					player.removeCard(cardRef);
					mainSet.addCard(cardRef);
				}
			});
			
			frame.add(b);
			frame.add(cards.get(i));
		}
		
		JButton close = new JButton("Close");
		close.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent arg0) {
				// TODO Auto-generated method stub
				cont = true;
				frame.setVisible(false);
				frame.dispose();
			}
			
		});
		close.setBounds(640, 763, width/3, height/7);
		
		frame.add(close);
		frame.requestFocus();
		frame.setVisible(true);
	}
	
	public void displayDraw(Player player, MainSet mainSet) {
		frame = new JFrame();
		frame.setLayout(null);
		frame.setBackground(Color.BLACK);
		frame.setSize(300, 300);
		
		JButton draw = new JButton("Draw");
		draw.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent arg0) {
				// TODO Auto-generated method stub
				try {
					mainSet.drawCard(player);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
			
		});
		draw.setBounds(50, 30, 100, 70);
		
		JButton close = new JButton("Close");
		close.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent arg0) {
				// TODO Auto-generated method stub
				cont = true;
				frame.setVisible(false);
				frame.dispose();
			}
			
		});
		close.setBounds(50, 100, 100, 70);
		
		frame.add(draw);
		frame.add(close);
		frame.setVisible(true);
		
	}
}

public class Game {

    public static void main(String args[]) throws IOException
    {	
    	int playerNum, round, playing;
    	String next, draw, life, name;
    	MainSet mainSet = new MainSet(200);
    	ArrayList<Player> players = new ArrayList<Player>();
    	Scanner scanner = new Scanner(System.in);
    	Display display = new Display();
    	
    	playerNum = Integer.parseInt(JOptionPane.showInputDialog(null, "Enter Player: "));
    	round = playerNum;
    	
    	for(int i=0; i<playerNum; i++) {
    		name = JOptionPane.showInputDialog(null, "Player " + (i+1) + " enter name: ");
    		players.add(new Player(name));
    	}
    	
    	for(int i=0; i<playerNum; i++) {
    		for(int j=0; j<5; j++) {
    			mainSet.drawByTag(players.get(i), "M");
    		}
    	}
    	
    	System.out.println("Start?(Y/N)");
    	next = JOptionPane.showInputDialog("Start?(Y/N)");
    	//game loop
    	while(next.toUpperCase().charAt(0)!='N') {
    		playing = round % playerNum;
    		if(players.get(playing).isAlive()) {
    			life = JOptionPane.showInputDialog(null, "Is player " + (playing + 1) + " alive(Y/N)");
	    		if(life.toUpperCase().charAt(0)!='Y') { 
	    			players.get(playing).die();
	    		}
	    		if(players.get(playing).isAlive()) {
	    			players.get(playing).getAllCard();
	    			display.displaySet(players.get(playing), mainSet);
	    			
	    			while(!display.cont) {
	    				System.out.print("a");
	    			}
	    			display.cont = false;
	    			
		    		draw = JOptionPane.showInputDialog(null, "Player " + (playing + 1) + " draw card?(Y/N)");
		    		if(draw.toUpperCase().charAt(0)=='Y') {
		    			display.cont = false;
		    			display.displayDraw(players.get(playing), mainSet);
		    			while(!display.cont) {
		    				System.out.print("a");
		    			}
		    			display.cont = false;
		    		}
		    		display.displaySet(players.get(playing), mainSet);
	    			
	    			while(!display.cont) {
	    				System.out.print("a");
	    			}
	    			display.cont = false;
		    		
		    		System.out.println("Continue?(Y/N)");
		    		next = JOptionPane.showInputDialog(null, "Continue?(Y/N)");
	    		}
    		}
    		round++;
    	}
    }
}
