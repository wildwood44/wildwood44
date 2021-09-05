package com.game.WandW.main;

import javax.swing.JFrame;

public class windowConfiguration {
	private JFrame window;
	private String title;
	private int width, height;
	
	public windowConfiguration(String title, int width, int height){
		this.title = title;
		this.width = width;
		this.height = height;
		
		createWindow();
	}
	
	private void createWindow() {
	}
}
