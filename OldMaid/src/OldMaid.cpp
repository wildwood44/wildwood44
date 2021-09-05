/*
 * OldMaid.cpp
 *
 *  Created on: 21 Aug 2021
 *      Author: james
 */
#include <iostream>
#include "Player1.h"
#include "Player2.h"
#include "Player3.h"
#include "Player4.h"
#include "Deck.h"
#include <random>

using namespace std;

int main()
{
	int pickACard, take;
	bool turn;
	Deck *deck = new Deck(53);
	Player1 *p1 = new Player1("You", 1);
	Player2 *p2 = new Player2("Player 2", 2);
	Player2 *p3 = new Player2("Player 3", 3);
	Player2 *p4 = new Player2("Player 4", 4);


	deck->Shuffle();
	while (!deck->IsEmpty() == false){
		try{
			if(!deck->IsEmpty() == false){p1->Draw(deck->Pop());};
			if(!deck->IsEmpty() == false){p2->Draw(deck->Pop());};
			if(!deck->IsEmpty() == false){p3->Draw(deck->Pop());};
			if(!deck->IsEmpty() == false){p4->Draw(deck->Pop());};
		}
		catch (out_of_range& e){
			cout << "Error: out of range" << endl;
		}
	}
	p1->Display();
	//cout << "Player 2 has" << endl;
	//p2->Display();
	//cout << "Player 3 has" << endl;
	//p3->Display();
	//cout << "Player 4 has" << endl;
	//p4->Display();
	//cout << deck->Peek() << endl;
	while(p1->Compare() == true){}
	while(p2->Compare() == true){}
	while(p3->Compare() == true){}
	while(p4->Compare() == true){}
	while (!p1->IsEmpty() and (!p2->IsEmpty() or !p3->IsEmpty() or !p4->IsEmpty())){
		p1->Display();
		turn = true;
		if (!p2->IsEmpty()){
			cout << p2->Name() << " has " << p2->Remaining() << " cards. " << endl;
			while (turn == true){
				cout << "Take a card: ";
				cin >> pickACard;
				if(pickACard > 0 and pickACard <= p2->Remaining()){
					pickACard--;
					p1->Take(p2->Pop(pickACard));
					turn = false;
				}
				else{
					cout << "Selected card must be greater then 0 and less than or equal to the max number of cards the player has." << endl;
				}
			}
			if(p2->IsEmpty()){
				cout << p2->Name() << " ran out of cards!!" << endl;
			}
		}
		else if (!p3->IsEmpty()){
			cout << p3->Name() << " has " << p3->Remaining() << " cards. " << endl;
			while (turn == true){
				cout << "Take a card: ";
				cin >> pickACard;
				if(pickACard > 0 and pickACard <= p3->Remaining()){
					pickACard--;
					p1->Take(p3->Pop(pickACard));
					turn = false;
				}
				else{
					cout << "Selected card must be greater then 0 and less than or equal to the max number of cards the player has." << endl;
				}
			}
			if(p3->IsEmpty()){
				cout << p3->Name() << " ran out of cards!!" << endl;
			}
		}
		else if (!p4->IsEmpty()){
			cout << p4->Name() << " has " << p4->Remaining() << " cards. " << endl;
			while (turn == true){
				cout << "Take a card: ";
				cin >> pickACard;
				if(pickACard > 0 and pickACard <= p4->Remaining()){
					pickACard--;
					p1->Take(p4->Pop(pickACard));
					turn = false;
				}
				else{
					cout << "Selected card must be greater then 0 and less than or equal to the max number of cards the player has." << endl;
				}
			}
			if(p4->IsEmpty()){
				cout << p4->Name() << " ran out of cards!!" << endl;
			}
		}
		while(p1->Compare() == true){}
		//Player 2's turn
		if (!p2->IsEmpty()){
			if (!p3->IsEmpty()){
				cout << p3->Name() << " has " << p3->Remaining() << " cards. " << endl;
				if (p3->Remaining() != 1){
					take = rand() % (p3->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}
				cout << p2->Name() << " took a card from " << p3->Name() << endl;
				p2->Take(p3->Pop(take));
				if(p3->IsEmpty()){
					cout << p3->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p4->IsEmpty()){
				cout << p4->Name() << " has " << p4->Remaining() << " cards. " << endl;
				if (p4->Remaining() != 1){
					take = rand() % (p4->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}
				cout << p2->Name() << " took a card from " << p4->Name() << endl;
				p2->Take(p4->Pop(take));
				if(p4->IsEmpty()){
					cout << p4->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p1->IsEmpty()){
				if (p1->Remaining() != 1){
					take = rand() % (p1->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}
				cout << p2->Name() << " took the ";
				p1->getCards(take).print();
				cout << endl;
				p2->Take(p1->Pop(take));
				if(p1->IsEmpty()){
					cout << p1->Name() << " ran out of cards!!" << endl;
				}
			}
			while(p2->Compare() == true){}
			if(p2->IsEmpty()){
				cout << p2->Name() << " ran out of cards!!" << endl;
			}
		}
		//Player 3's turn
		if (!p3->IsEmpty()){
			if (!p4->IsEmpty()){
				cout << p4->Name() << " has " << p4->Remaining() << " cards. " << endl;
				if (p4->Remaining() != 1){
					take = rand() % (p4->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}
				cout << p3->Name() << " took a card from " << p4->Name() << endl;				//p3->getCards(take).print();
				p3->Take(p4->Pop(take));
				if(p4->IsEmpty()){
					cout << p4->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p1->IsEmpty()){
				if (p1->Remaining() != 1){
					take = rand() % (p1->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}				cout << p3->Name() << " took the ";
				p1->getCards(take).print();
				cout << endl;
				p3->Take(p1->Pop(take));
				if(p1->IsEmpty()){
					cout << p1->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p2->IsEmpty()){
				cout << p2->Name() << " has " << p2->Remaining() << " cards. " << endl;
				if (p2->Remaining() != 1){
					take = rand() % (p2->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}				cout << p3->Name() << " took a card from " << p2->Name() << endl;
				p3->Take(p2->Pop(take));
				if(p2->IsEmpty()){
					cout << p2->Name() << " ran out of cards!!" << endl;
				}
			}
			while(p3->Compare() == true){}
			if(p3->IsEmpty()){
				cout << p3->Name() << " ran out of cards!!" << endl;
			}
		}
		//Player 4's turn
		if (!p4->IsEmpty()){
			if (!p1->IsEmpty()){
				if (p1->Remaining() != 1){
					take = rand() % (p1->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}
				cout << p4->Name() << " took the ";
				p1->getCards(take).print();
				cout << endl;
				p4->Take(p1->Pop(take));
				if(p1->IsEmpty()){
					cout << p1->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p2->IsEmpty()){
				cout << p2->Name() << " has " << p2->Remaining() << " cards. " << endl;
				if (p2->Remaining() != 1){
					take = rand() % (p2->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}				cout << p4->Name() << " took a card from " << p2->Name() << endl;
				p4->Take(p2->Pop(take));
				if(p2->IsEmpty()){
					cout << p2->Name() << " ran out of cards!!" << endl;
				}
			}
			else if (!p3->IsEmpty()){
				cout << p3->Name() << " has " << p3->Remaining() << " cards. " << endl;
				if (p3->Remaining() != 1){
					take = rand() % (p3->Remaining()-1) + 0;
				}
				else {
					take = 0;
				}				cout << p4->Name() << " took a card from " << p3->Name() << endl;
				p4->Take(p3->Pop(take));
				if(p3->IsEmpty()){
					cout << p3->Name() << " ran out of cards!!" << endl;
				}
			}
			while(p4->Compare() == true){}
			if(p4->IsEmpty()){
				cout << p4->Name() << " ran out of cards!!" << endl;
			}
		}
		cout << endl;
		//cout << p1->IsEmpty() << p2->IsEmpty() << p3->IsEmpty() << p4->IsEmpty();
	}
	cout << "Game end!!" << endl;
	if(p1->IsEmpty() and !p2->IsEmpty() and !p3->IsEmpty() and !p4->IsEmpty()){
		cout << "You Win!!" << endl;
	}
	else if(p1->IsEmpty() and p2->IsEmpty() and !p3->IsEmpty() and !p4->IsEmpty() or
			p1->IsEmpty() and !p2->IsEmpty() and p3->IsEmpty() and !p4->IsEmpty() or
			p1->IsEmpty() and !p2->IsEmpty() and !p3->IsEmpty() and p4->IsEmpty()){
		cout << "You came second!!" << endl;
	}
	else if(p1->IsEmpty() and p2->IsEmpty() and p3->IsEmpty() and !p4->IsEmpty() or
			p1->IsEmpty() and !p2->IsEmpty() and p3->IsEmpty() and p4->IsEmpty() or
			p1->IsEmpty() and p2->IsEmpty() and !p3->IsEmpty() and p4->IsEmpty()){
		cout << "You came second!!" << endl;
	}
	else if(p1->getCards(0).getSuit() == joker){
		cout << "You have the joker!!" << endl;
		cout << "You Lose!!" << endl;
	}
	system("Pause");
    return 0;
}
