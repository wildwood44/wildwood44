#include "Player3.h"


Player3::Player3(string name, int id)
{
	this->name = name;
	this->PlayerID = id;
	remaining = 0;
}


void Player3::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

string Player3::Name() {
	return name;
}

int Player3::Id() {
	return PlayerID;
}

int Player3::Remaining() {
	return remaining;
}

bool Player3::IsEmpty() {
	//If top is empty return true otherwise return false
	return (cards.empty());
}

Card Player3::Pop(int value) {
	//Return the top element of stackData and decrement
	remaining--;
	Card temp = cards.at(value);
	cout << value;
	cards.erase(cards.begin() + (value));
	return temp;
}

bool Player3::Compare() {
	int count = 0;
	for (const Card& i : cards) {
		int count2 = 0;
		for (const Card& j : cards) {
			if (i.getNumber() == j.getNumber() and
					i.getSuit() != j.getSuit()){
				cout <<  name << " had the ";
				i.print();
				cout << " and ";
				j.print();
				cout << endl;
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

void Player3::Take(Card index) {
	cards.push_back( index );
	remaining++;
}

Card Player3::getCards(int index) {
	return cards.at(index);
}

void Player3::Display() {
	cout << name <<" has "<< remaining <<" cards: The ";
	int count = 1;
	for (const Card& i : cards) {
		cout << count << "(";
		i.print();
		cout << "; ";
		count++;
	}
	cout << endl;
}

