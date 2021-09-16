package com.publiccloud.spring.security;

import com.publiccloud.spring.security.model.User;

public class Guest implements IGuestLog{
	private String username, password, email, group, occupation, key;
	
	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}
	
	public String getGroup() {
		return group;
	}

	public void setGroup(String group) {
		this.group = group;
	}

	public String getOccupation() {
		return occupation;
	}

	public void setOccupation(String occupation) {
		this.occupation = occupation;
	}

	public String getKey() {
		return key;
	}

	public void setKey(String key) {
		this.key = key;
	}

	@Override
	public User Login(int id) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public boolean logoff() {
		// TODO Auto-generated method stub
		return false;
	}
}
