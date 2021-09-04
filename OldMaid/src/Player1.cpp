#include "Player1.h"


Player1::Player1(bool active)
{
	if (!cards.empty())
	{
		active = true;
	}
	else{
		active = false;
	}
	remaining = 0;
}

void Player1::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

string Player1::Name() {
	return name;
}


int Player1::Remaining() {
	return remaining;
}

bool Player1::IsEmpty() {
	//If top is empty return true otherwise return false
	return (cards.empty());
}

Card Player1::Pop(int value) {
	//Return the top element of stackData and decrement
	remaining--;
	Card temp = cards.at(value);
	cards.erase(cards.begin() + (value));
	return temp;
}

bool Player1::Compare() {
	int count = 0;
	for (const Card& i : cards) {
		int count2 = 0;
		for (const Card& j : cards) {
			if (i.getNumber() == j.getNumber() and
					i.getSuit() != j.getSuit()){
				cout << name << " had the ";
				i.print();
				cout << " and ";
				j.print();
				cout << endl;
				//cout << "Removing ";
				//cards.at(count).print();
				//cout << " and ";
				//cards.at(count2-1).print();
				//cout << endl;
				cards.erase(cards.begin() + count);
				remaining--;
				cards.erase(cards.begin() + (count2 -1));
				remaining--;
				return true;
			}
			count2++;
		}
		count++;
	}
	return false;
}

void Player1::Take(Card index) {
	cout << name << " got the ";
	index.print();
	cout << endl;
	cards.push_back( index );
	remaining++;
}

Card Player1::getCards(int index) {
	return cards.at(index);
}

void Player1::Display() {
	cout << name <<" have "<< remaining <<" cards: The ";
	int count = 1;
	for (const Card& i : cards) {
		i.print();
		cout << "; ";
		count++;
	}
	cout << endl;
}

