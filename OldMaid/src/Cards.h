//Reference: https://cplus-plusprogramming.blogspot.com/2012/05/card-game-example-it-is-long-progrmming.html
#pragma once
#include <iostream>
#include "Deck.h"
#include <string>

using namespace std;

enum suit{club,diamond,heart,spade, joker};
const int jack=11;
const int queen=12;
const int king=13;
const int ace=1;

class Card;
class Card{
private:
	int number;
	suit st;
public:
	Card();
	Card(int n ,suit s):number(n), st(s){}
	void print() const;
	bool isequal(Card);
	suit getSuit() const;
	int getNumber() const;
};


