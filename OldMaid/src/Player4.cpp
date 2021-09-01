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

int Player4::P1Cards(int remaining) {
	return remaining;
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

int Player4::Take(int key) {
	remaining++;
	return key;
}

void Player4::Display() {
	cout <<  name <<" has ";
	int count = 1;
	for (const Card& i : cards) {
		cout << count << "(";
		i.print();
		cout << "; ";
		count++;
	}
	cout << endl;
}

