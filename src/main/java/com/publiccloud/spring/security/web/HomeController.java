package com.publiccloud.spring.security.web;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller("/")
public class HomeController {

    @GetMapping("/")
    public String root() {
        return "index";
    }

    @GetMapping("/user")
    public String userIndex() {
        return "user/index";
    }

    @GetMapping("/login")
    public String login(Model model) {
        return "login";
    }

    @GetMapping("/lowRisk")
    public String lowRisk() {
        return "lowRisk";
    }

    @GetMapping("/highRisk")
    public String highRisk() {
        return "highRisk";
    }

    @GetMapping("/history")
    public String history() {
        return "history";
    }

    @GetMapping("/access-denied")
    public String accessDenied() {
        return "/error/access-denied";
    }
}
