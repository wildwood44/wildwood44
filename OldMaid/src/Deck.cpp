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
	int count = 0;
	//Create array
	stackData = new Card[maxSize];
	/*stackData = {Card(1,club), Card(1,spade), Card(1,heart), Card(1,diamond),
			Card(2,club), Card(2,spade), Card(2,heart), Card(2,diamond),
			Card(3,club), Card(3,spade), Card(3,heart), Card(3,diamond),
			Card(4,club), Card(4,spade), Card(4,heart), Card(4,diamond),
			Card(5,club), Card(5,spade), Card(5,heart), Card(5,diamond),
			Card(6,club), Card(6,spade), Card(6,heart), Card(6,diamond),
			Card(7,club), Card(7,spade), Card(7,heart), Card(7,diamond),
			Card(8,club), Card(8,spade), Card(8,heart), Card(8,diamond),
			Card(9,club), Card(9,spade), Card(9,heart), Card(9,diamond),
			Card(10,club), Card(10,spade), Card(10,heart), Card(10,diamond),
			Card(11,club), Card(11,spade), Card(11,heart), Card(11,diamond),
			Card(12,club), Card(12,spade), Card(12,heart), Card(12,diamond),
			Card(13,club), Card(13,spade), Card(13,heart), Card(13,diamond),
			Card(0, joker)
	};*/
	for( const auto &s : suits ) {
		for (int n = 1; n < 14; ++n ) {
			stackData[count++] = Card(n, s);
		}
	}
	stackData[count++] = Card(0, joker);
	for (int i = 0; i < maxSize; i++){
		stackData[i].print();
	}
	//Set top
	top = maxSize;
}

bool Deck::IsEmpty() {
	//If top is empty return true otherwise return false
	return (top == -1);
}

bool Deck::IsFull() {
	//If top is equal to the max size -1 return true otherwise return false
	return (top == (maxSize - 1));
}

Card Deck::Peek() {
	//Return the top element of data
	return stackData[top];
}

Card Deck::Pop() {
	//Return the top element of stackData and decrement
	return stackData[top--];
}

void Deck::Push(int value) {
	this->value = value;
	//Increment top and set the top element of stackData to value
	stackData[++top] = stackData[value];
}

void Deck::Shuffle() {
	for (int i = 0; i < top; i++)
	{
		next = rand() % top;
		*temp = stackData[top];
		stackData[top] = stackData[next];
		stackData[next] = *temp;
	}
}

