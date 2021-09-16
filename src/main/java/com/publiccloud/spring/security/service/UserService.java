package com.publiccloud.spring.security.service;

import com.publiccloud.spring.security.model.User;
import com.publiccloud.spring.security.web.dto.UserRegistrationDto;
import org.springframework.security.core.userdetails.UserDetailsService;

public interface UserService extends UserDetailsService {

    User findByUsername(String username);

    User save(UserRegistrationDto signup);
}

