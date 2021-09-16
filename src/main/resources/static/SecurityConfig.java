package com.publiccloud.spring.security.config;

import com.publiccloud.spring.security.service.UserService;
import com.publiccloud.spring.security.web.LoggingAccessDeniedHandler;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.web.util.matcher.AntPathRequestMatcher;

@Configuration
public class SecurityConfig extends WebSecurityConfigurerAdapter {

    //@Autowired
    //private LoggingAccessDeniedHandler accessDeniedHandler;

    @Autowired
    private UserService userService;
    
    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
                .authorizeRequests()
                    .antMatchers(
                            "/",
                    		"/signup",
                            "/js/**",
                            "/css/**",
                            "/img/**",
                            "/webjars/**").permitAll()
                    .anyRequest().authenticated()
                    /*.antMatchers("/user/**").hasRole("USER")*/
                .and()
                	.formLogin()
                    	.loginPage("/login")
                    		.permitAll()
                .and()
                	.logout()
                    	.invalidateHttpSession(true)
                    	.clearAuthentication(true)
                    	.logoutRequestMatcher(new AntPathRequestMatcher("/logout"))
                    	.logoutSuccessUrl("/login?logout")
                .permitAll();
                /*.and()
                .exceptionHandling()
                    .accessDeniedHandler(accessDeniedHandler);*/
    }

    @Bean
    public BCryptPasswordEncoder passwordEncoder(){
        return new BCryptPasswordEncoder();
    }

    @Bean
    public DaoAuthenticationProvider authenticationProvider(){
        DaoAuthenticationProvider auth = new DaoAuthenticationProvider();
        auth.setUserDetailsService(userService);
        auth.setPasswordEncoder(passwordEncoder());
        return auth;
    }

    @Override
    protected void configure(AuthenticationManagerBuilder auth) throws Exception {
        //auth.inMemoryAuthentication()
        //        .withUser("user").password("password").roles("USER")
        //    .and()
        //        .withUser("manager").password("password").roles("MANAGER");
        auth.authenticationProvider(authenticationProvider());
    }

}