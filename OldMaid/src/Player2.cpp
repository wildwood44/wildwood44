#include "Player2.h"


Player2::Player2(bool active)
{
	if (active == true)
	{
		//cout << "Player 2";
	}
}

void Player2::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

string Player2::Name() {
	return name;
}

int Player2::Remaining() {
	return remaining;
}

bool Player2::IsEmpty() {
	//If top is empty return true otherwise return false
	return (cards.empty());
}

Card Player2::Pop(int value) {
	//Return the top element of stackData and decrement
	remaining--;
	Card temp = cards.at(value);
	cards.erase(cards.begin() + (value));
	return temp;
}

bool Player2::Compare() {
	int count = 0;
	for (const Card& i : cards) {
		int count2 = 0;
		for (const Card& j : cards) {
			if (i.getNumber() == j.getNumber() and
					i.getSuit() != j.getSuit()){
				cout <<  name <<" had the ";
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

void Player2::Take(Card index) {
	cards.push_back( index );
	remaining++;
}

Card Player2::getCards(int index) {
	return cards.at(index);
}

void Player2::Display() {
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

