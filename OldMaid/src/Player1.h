#pragma once
#include <iostream>
#include "Cards.h"
#include "Deck.h"
#include "P1Hand.h"
#include "HashTable.h"

using namespace std;

class Player1
{
private:
	int PlayerID = 1;
	string name = "You";
	int remaining;
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	Player1(bool active);
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

