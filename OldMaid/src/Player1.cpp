#include "Player1.h"


Player1::Player1(bool active)
{
	if (active == true)
	{
		cout << "You have" << endl;
	}
}

void Player1::Draw(Card index) {
	cards.push_back( index );
	remaining++;
}

int Player1::P1Cards(int remaining) {
	return remaining;
}

bool Player1::IsEmpty() {
	//If top is empty return true otherwise return false
	//f (top != -1){
	//	return true;
	//}
	//else{
	//	return false;
	//}
	//return (cards.empty());
}

Card Player1::Pop(int value) {
	//Return the top element of stackData and decrement
	remaining--;
	return cards.at(value--);
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

int Player1::Take(int key) {
	remaining++;
	return key;
}

void Player1::Display() {
	cout << name <<" have the ";
	int count = 1;
	for (const Card& i : cards) {
		i.print();
		cout << "; ";
		count++;
	}
	cout << endl;
}

