#pragma once
#include <iostream>
#include "Cards.h"
#include "Deck.h"

using namespace std;

class Player1
{
private:
	int PlayerID;
	string name;
	int remaining;
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	Player1(string name, int id);
	void Draw(Card index);
	string Name();
	int Id();
	int Remaining();
	bool IsEmpty();
	Card Pop(int value);
	bool Compare();
	void Take(Card index);
	Card getCards(int index);
	void Display();
};

