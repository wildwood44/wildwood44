package com.game.WandW.main;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;

public class Menu {
	public Menu(){
		
	}

	public void paint (Graphics g) {	
		
		Graphics2D g2d = (Graphics2D) g;
		Font f = new Font("ariel", Font.BOLD, 50);
		g.setFont(f);
		g.setColor(Color.black);
		g.drawString("Wild and Wyrd", 175, 100);

		g.fillRect(400, 350, 200, 150);
	}
	public void drawCursor (Graphics g) {
		
	}
	public void select() {
		
	}
}

