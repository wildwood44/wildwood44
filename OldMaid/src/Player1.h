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
	Card *inHand;
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	int turn;
	Player1(bool active);
	void Draw(Card index);
	int P1Cards(int remaining);
	bool IsEmpty();
	Card Pop(int value);
	bool Compare();
	int Take(int key);
	void Display();
};

