/*
 * Cards.cpp
 *
 *  Created on: 22 Aug 2021
 *      Author: james
 */
//Source: https://cplus-plusprogramming.blogspot.com/2012/05/card-game-example-it-is-long-progrmming.html
#include "Cards.h"

void Card::print() const{
	if (number>=2 && number<=10){
		cout<< number<<" of ";
	}
	else{
		switch(number){
			case jack: cout<<"jack of ";break;
			case queen: cout<<"queen of ";break;
			case king: cout<<"king of ";break;
			case ace: cout<<"ace of ";break;
		}
	}
	switch(st){
		case club: cout<<"clubs";break;
		case diamond: cout<<"diamonds";break;
		case heart: cout<<"hearts";break;
		case spade: cout<<"spades";break;
		case joker: cout<<"joker";break;
	}
}
bool Card::isequal(Card c2){
	return (number==c2.number && st==c2.st) ? true :false;
}

suit Card::getSuit() const{
    return st;
}

int Card::getNumber() const{
    return number;
}
