package com.publiccloud.spring.security;

import com.publiccloud.spring.security.model.User;

public interface IAdminLog {
	public User Login(int id);
	public boolean logoff();
}