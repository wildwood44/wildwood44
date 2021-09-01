#pragma once
#include "Cards.h"
#include <vector>
#include <random>
#include <algorithm>

class Card;

class Deck
{
private:
	int remaining;
	int maxSize;
	//Card *stackData;
	int top;
	int next;
	int value;
	Card *temp;
	std::vector<Card> cards;
public:
	Deck(int maxSize);
	bool IsEmpty();
	bool IsFull();
	Card Peek();
	Card Pop();
	void Push(int value);
	void Shuffle();
};

