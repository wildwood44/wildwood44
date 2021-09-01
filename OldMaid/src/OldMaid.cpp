/*
 * OldMaid.cpp
 *
 *  Created on: 21 Aug 2021
 *      Author: james
 */
#include <iostream>
#include "Player1.h"
#include "P1Hand.h"
#include "Player2.h"
#include "P2Hand.h"
#include "Player3.h"
#include "P3Hand.h"
#include "Player4.h"
#include "P4Hand.h"
#include "Deck.h"
#include "HashTable.h"
#include "GameObject.h"
#include <random>

using namespace std;

int main()
{
	Deck *deck = new Deck(53);
	Player1 *p1 = new Player1(true);
	Player2 *p2 = new Player2(true);
	Player3 *p3 = new Player3(true);
	Player4 *p4 = new Player4(true);
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
	p1->Display();
	system("Pause");
    return 0;
}
