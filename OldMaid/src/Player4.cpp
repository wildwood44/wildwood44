#include "Player4.h"


Player4::Player4(bool active)
{
	if (active == true)
	{
		//cout << "Player 4";
	}
}

void Player4::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

string Player4::Name() {
	return name;
}

int Player4::Remaining() {
	return remaining;
}

bool Player4::IsEmpty() {
	//If top is empty return true otherwise return false
	return (cards.empty());
}

Card Player4::Pop(int value) {
	//Return the top element of stackData and decrement
	remaining--;
	Card temp = cards.at(value);
	cout << value;
	cards.erase(cards.begin() + (value));
	return temp;
}

bool Player4::Compare() {
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

void Player4::Take(Card index) {
	cards.push_back( index );
	remaining++;
}

Card Player4::getCards(int index) {
	return cards.at(index);
}

void Player4::Display() {
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

