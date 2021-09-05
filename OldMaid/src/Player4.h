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
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	Player4(string name, int id);
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

