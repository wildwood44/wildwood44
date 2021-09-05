package com.game.WandW.main;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;

import javax.swing.JFrame;

public class Game {
	public Game() {
		
	}
	
	public enum State{
		MainMenu, Casual, Battle, GameOver;
	}
	
	private State state = State.MainMenu;
	
	public static void main(String[] args) {	
		JFrame window = new JFrame("Wild and Wyrd");
		window.setSize(900, 650);
		window.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		window.setResizable(false);
		window.setLocationRelativeTo(null);
		window.setVisible(true);
		
		
		Menu m = new Menu();
	}
	public void paintComponent (Graphics g) {	
		
		Graphics2D g2d = (Graphics2D) g;
		Font f = new Font("ariel", Font.BOLD, 50);
		g.setFont(f);
		g.setColor(Color.black);
		g.drawString("Wild and Wyrd", 175, 100);

		g.fillRect(0, 0, 200, 150);
	}
}
