#pragma once
#include <iostream>
#include "Cards.h"
#include "Deck.h"

using namespace std;

class Player3
{
private:
	int PlayerID = 3;
	string name = "Player 3";
	int remaining;
	int index, i, j;
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	int turn;
	Player3(bool active);
	void Draw(Card index);
	int P1Cards(int remaining);
	bool Compare();
	int Take(int key);
	void Display();
};

