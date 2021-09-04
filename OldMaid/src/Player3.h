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
	string Name();
	int Remaining();
	bool IsEmpty();
	Card Pop(int value);
	bool Compare();
	void Take(Card index);
	Card getCards(int index);
	void Display();
};

