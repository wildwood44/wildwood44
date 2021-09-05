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
	string cardFace;
	string cardSuit;
	std::vector<Card> cards;
public:
	Player3(string name, int id);
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

