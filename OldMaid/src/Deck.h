#pragma once
#include "Cards.h"

class Card;

class Deck
{
private:
	int remaining;
	int maxSize;
	Card *stackData;
	int top;
	int next;
	int value;
	Card *temp;
public:
	Deck(int maxSize);
	bool IsEmpty();
	bool IsFull();
	Card Peek();
	Card Pop();
	void Push(int value);
	void Shuffle();
};

