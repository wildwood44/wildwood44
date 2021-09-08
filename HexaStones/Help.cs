using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace HexaStones
{
    public partial class Help : Form
    {
        String text1, text2, text3, text4, text5, text6, text7;

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbList.SelectedIndex == 0)
            {
                lblTitle.Text = "Hexa-Stones";
                tbxText.Text = text1;
                if (btnPrev.Enabled == true)
                {
                    btnPrev.Enabled = false;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 1)
            {
                lblTitle.Text = "Interface";
                tbxText.Text = text2;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 2)
            {
                lblTitle.Text = "Pieces";
                tbxText.Text = text3;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 3)
            {
                lblTitle.Text = "Setup/Drawing";
                tbxText.Text = text4;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 4)
            {
                lblTitle.Text = "Moving";
                tbxText.Text = text5;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 5)
            {
                lblTitle.Text = "Attacking";
                tbxText.Text = text6;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled != true)
                {
                    btnNext.Enabled = true;
                }
            }
            else if (cbList.SelectedIndex == 6)
            {
                lblTitle.Text = "How do you win";
                tbxText.Text = text7;
                if (btnPrev.Enabled != true)
                {
                    btnPrev.Enabled = true;
                }
                else if (btnNext.Enabled == true)
                {
                    btnNext.Enabled = false;
                }
            }
        }

        //List<string> file =
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            /*try
            {
                //System.IO.File.ReadAllText("@Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\HexaStones\\HexaStones\bin\\Debug\\help.txt");
                //string ConnectString = "@Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\HexaStones\\HexaStones\bin\\Debug\\help.txt";
                StreamReader pages = new StreamReader("@Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\HexaStones\\HexaStones\bin\\Debug\\help.txt");
                string page = pages.ReadToEnd();
                text1 = page;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }*/
            text1 = "Welcome to the Hexa-Stones help guide.\r\n\r\nQuick Guide:\r\nStep 1: Hover over hexagonal pieces on left hand side\r\nStep 2: Click on it if rank 1\r\nStep 3: Click on any blue hexagonal space on the board\r\nStep 4: Repeat for any other pieces you want on the board\r\nStep 5: Click next button to move stage\r\nStep 6: Click on piece on the board that you want to move\r\nStep 7: Click on any blue space on the board to move it\r\nStep 8: Repeat for any other pieces you want to move\r\nStep 9: Click next button for attack stage\r\nStep 10: Click on piece on the board to see if any of them can attack\r\nStep 11: If enemy piece is within red attack spaces\r\nStep 12: Repeat for any other pieces that can attack\r\nStep 13: Click next button for draw stage, timer is reset\r\nStep 14: Repeat steps 1 - 3 however you may only draw one piece before being automatically being sent to the move stage.";
            text2 = "Interface: The following will detail what you see when you open the game.\r\n\r\nLeft of screen:\r\nThe hand: The first thing you should take note of the five hexagonal shapes underneath the two digit number.The shapes are the pieces you will be using to play the game.\r\nTo get more information about the piece hover your mouse over it and the pieces name,attack power, health, spaces it can move, rank and attack type will appear.\r\nIn order to select a piece you will need to click on it and click on another piece to unselect it.\r\nThe Roster: The two digit number above the pieces are the remaining pieces in your roster, it starts at forty but for each piece that is added to your hand it goes down by one.\r\nThe Grave: For each of your pieces that are destroyed the counter goes up by one.\r\n\r\nCenter:The Board: 142 hexagonal spaces are layed out next to each other on all possible sides, this makes it possible for the piece to move in at least six different directions.\r\n\r\nTop of screen: The top of the interface gives information on the current status of the game.\r\nStage and Next: At the top left there is text to state what stage you are on, to change this you need to press the next button on the right hand side.\r\nType counters: In order to summon a piece of a higher rank you will need a few pieces of the same type. So if you had two 'Mercenary' pieces then the counter for Melee will go up to two to let you know that you can add a rank 2 pieces of the same type to the board. The exception to this rule are rank 1's\r\nTimer: In order to prevent stalling a timer has been added. When it reaches zero then the system will skip to the opponent's turn. Turn counter: Shows how many turns you have played.\r\n\r\nBottom of screen:\r\nPiece Information: Gives information about the selected piece.";
            text3 = "Pieces: Hexagon Shaped pieces with varying details, these are available to you in the hand on the left of the interface. These details including its name can be viewed by hovering your mouse over the intended piece and are as follows:\r\n\r\nAttack: This shows how much damage your piece can deal to an opposing piece.\r\nHealth: The is how much health the piece has, this is be lowered when it is attacked by the attack value of the attacking piece.If this reaches zero then the piece will be sent to the grave.\r\nMove: This is how many spaces it can move when it is on the board.\r\nRank: The rank of the piece determines their value. In order to summon a piece you need a number of pieces of the same type to match the rank on the board.The exception to this rule are special types.\r\nType: There are four type Melee, Magic, Long Range and Special. These types effect their attack patterns and in the case of special types how the piece it summoned.";
            text4 = "Setup/Draw: These stages are when you can take pieces from the hand box from the left of the interface to the first four columns on the left of the board that have been changed colour to blue.\r\n\r\nThe setup stage allows you to draw multiple pieces and can only be played at the first stage of the game.\r\n\r\nThe Draw stage on the other hand occurs at the start of your next turn and only one piece can be used before the system automatically changes to the move stage.";
            text5 = "Move: During the move stage when you click on a piece on the board the spaces it can move to will change colour to blue. The number of spaces a piece can move to depends on its move value, this can go up to five.";
            text6 = "Attack: During the attack stage when you click on a piece on the board the spaces where it can attack another piece within the piece types attack range.\r\n\r\nMelee: Can attack any pieces next to it.\r\nMagic: Can attack any pieces three spaces away from it.\r\nLong Range: Can attack any pieces five spaces away from it.\r\nSpecial: Can attack any pieces next to it.";
            text7 = "How to Win: There are three ways to win the game\r\n\r\nIf there are no pieces on your opponent's side\r\nIf there are no pieces in your opponent’s deck when they draw\r\nIf you opponent quits\r\nHowever if these circumstances happen to you then you will lose the game.";
            tbxText.Text = text1;
            btnPrev.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (lblTitle.Text == "Hexa-Stones")
            {
                lblTitle.Text = "Interface";
                tbxText.Text = text2;
                btnPrev.Enabled = true;
            }
            else if (lblTitle.Text == "Interface")
            {
                lblTitle.Text = "Pieces";
                tbxText.Text = text3;
            }
            else if (lblTitle.Text == "Pieces") {
                lblTitle.Text = "Setup/Drawing";
                tbxText.Text = text4;
            }
            else if (lblTitle.Text == "Setup/Drawing")
            {
                lblTitle.Text = "Moving";
                tbxText.Text = text5;
            }
            else if (lblTitle.Text == "Moving")
            {
                lblTitle.Text = "Attacking";
                tbxText.Text = text6;
            }
            else if (lblTitle.Text == "Attacking")
            {
                lblTitle.Text = "How do you win";
                tbxText.Text = text7;
                btnNext.Enabled = false;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (lblTitle.Text == "How do you win")
            {
                lblTitle.Text = "Attacking";
                tbxText.Text = text6;
                btnNext.Enabled = true;
            }
            else if (lblTitle.Text == "Attacking")
            {
                lblTitle.Text = "Moving";
                tbxText.Text = text5;
            }
            else if (lblTitle.Text == "Moving")
            {
                lblTitle.Text = "Setup/Drawing";
                tbxText.Text = text4;
            }
            else if (lblTitle.Text == "Setup/Drawing")
            {
                lblTitle.Text = "Pieces";
                tbxText.Text = text3;
            }
            else if (lblTitle.Text == "Pieces")
            {
                lblTitle.Text = "Interface";
                tbxText.Text = text2;
            }
            else if (lblTitle.Text == "Interface")
            {
                lblTitle.Text = "Hexa-Stones";
                tbxText.Text = text1;
                btnPrev.Enabled = false;
            }
        }
    }
}
