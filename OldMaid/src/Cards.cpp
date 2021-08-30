/*
 * Cards.cpp
 *
 *  Created on: 22 Aug 2021
 *      Author: james
 */
//Source: https://cplus-plusprogramming.blogspot.com/2012/05/card-game-example-it-is-long-progrmming.html
#include "Cards.h"

void Card::print(){
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
		case club: cout<<"clubs"<<endl;break;
		case diamond: cout<<"diamonds"<<endl;break;
		case heart: cout<<"hearts"<<endl;break;
		case spade: cout<<"spades"<<endl;break;
		case joker: cout<<"joker"<<endl;break;
	}
}
bool Card::isequal(Card c2){
	return (number==c2.number && st==c2.st) ? true :false;
}
Card::Card(){
}

suit Card::getSuit(){
    return st;
}

int Card::getNumber(){
    return number;
}
