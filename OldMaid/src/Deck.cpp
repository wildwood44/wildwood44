#include "Deck.h"


Deck::Deck(int maxSize)
{
	const suit suits[]  = { diamond, heart, spade, club };
	//Set Size
	this->maxSize = maxSize;
	temp = 0;
	value = 0;
	next = 0;
	remaining = maxSize;
	//Create array
	for( const auto &s : suits ) {
		for (int n = 1; n < 14; ++n ) {
			cards.push_back( Card{n, s} );
		}
	}
	cards.push_back( Card{0, joker});
	//for (const Card& i : cards) {
		//i.print();
	//}
	//Set top
	top = maxSize - 1;
}

bool Deck::IsEmpty() {
	//If top is empty return true otherwise return false
	if (top != -1){
		return true;
	}
	else{
		return false;
	}
	//return (cards.empty());
}

bool Deck::IsFull() {
	//If top is equal to the max size -1 return true otherwise return false
	return (top == (maxSize - 1));
}

Card Deck::Peek() {
	//Return the top element of data
	return cards.at(top);
}

Card Deck::Pop() {
	//Return the top element of stackData and decrement
	remaining--;
	return cards.at(top--);
}

void Deck::Push(int value) {
	this->value = value;
	//Increment top and set the top element of stackData to value
	cards.at(++top) = cards.at(value);
}

int myrandom (int i) { return rand()%i;}

void Deck::Shuffle() {
	srand(unsigned(time(0)));
	random_shuffle(cards.begin(), cards.end());
	//for (const Card& i : cards) {
		//i.print();
	//}
}

