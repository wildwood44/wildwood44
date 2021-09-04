#pragma once
#include <iostream>
#include "Cards.h"
#include "Deck.h"

using namespace std;

class Player4
{
private:
	int PlayerID = 4;
	string name = "Player 4";
	int remaining;
	int index, i, j;
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	int turn;
	Player4(bool active);
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

