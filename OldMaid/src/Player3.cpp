#include "Player3.h"


Player3::Player3(bool active)
{
	if (active == true)
	{
		//cout << "Player 3";
	}
}


void Player3::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

int Player3::P1Cards(int remaining) {
	return remaining;
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

int Player3::Take(int key) {
	remaining++;
	return key;
}

void Player3::Display() {
	cout << name << " has ";
	int count = 1;
	for (const Card& i : cards) {
		cout << count << "(";
		i.print();
		cout << "; ";
		count++;
	}
	cout << endl;
}

