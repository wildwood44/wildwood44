using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace HexaStones
{
    public partial class HexaStones : Form
    {
        int remaining = 40, turn = 1;
        int[] deck;
        Point[] space;
        Point[] hand;
        Point[] topLeft;
        Point[] topRight;
        Point[] LeftS;
        Point[] RightS;
        Point[] bottomLeft;
        Point[] bottomRight;
        int[] tomb;
        int Melee = 0, Magic = 0, LongRange = 0, Special = 0;
        int m = 1, s = 59;
        int[] attackPower;
        bool[] moving;
        bool[] attacking;
        int id;
        int handId;
        int[] pieceId;
        int pieceId2;
        int[] pieceId3;
        int pieceId4;
        int[] pieceLocation;
        bool[] select;
        bool[] select2;
        bool draw = true;
        bool GameOver = false;
        bool setup = true;
        bool drawStage = false;
        bool moveStage = false;
        bool attackStage = false;
        int up = -15, down = 15, upLeft = -8, downRight = 8, upRight = -7, downLeft = 7;
        int moveFor = 0;
        int hp;
        List<int> moveValue = new List<int>();
        List<int> attackValue = new List<int>();
        List<int> extra = new List<int>();
        Random rand = new Random();

        public HexaStones()
        {
            InitializeComponent();
        }

        private void CountDown_Tick(object sender, EventArgs e)
        {
            countDown.Interval = 1000;
            s--;
            if (m == 0 && s < 0)
            {
                pnlBoard.Invalidate();
                attackStage = false;
                drawStage = true;
                lblStage.Text = "Draw Stage";
                try
                {
                    List<int> deckPieces = new List<int>();
                    //Conncet to Deck database
                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string selectString = "Select * From GameDatabase Where UserID = 1 And Active = FALSE";
                    command.CommandText = selectString;
                    OleDbDataReader reader = command.ExecuteReader();
                    //MessageBox.Show(command.CommandText);
                    while (reader.Read())
                    {
                        deckPieces.Add(reader.GetInt32(0));
                    }
                    deck = deckPieces.ToArray();
                    //Set a random piece into the user hand
                    for (int i = 0; i < 6; i++)
                    {
                        int getPiece = rand.Next(deckPieces.Count) + 1;
                        string selectString2 = "select * from GameDatabase where ID = " + getPiece;
                        string updateString = "update GameDatabase Set Active = TRUE Where ID = " + getPiece;
                        OleDbCommand command2 = new OleDbCommand(selectString2, connection);
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            if (pbxHandPiece1.Visible == false)
                            {
                                pbxHandPiece1.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[1] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece1.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                            }
                            else if (pbxHandPiece2.Visible == false)
                            {
                                pbxHandPiece2.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[2] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece2.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                            }
                            else if (pbxHandPiece3.Visible == false)
                            {
                                pbxHandPiece3.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[3] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece3.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                            }
                            else if (pbxHandPiece4.Visible == false)
                            {
                                pbxHandPiece4.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[4] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece4.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                            }
                            else if (pbxHandPiece5.Visible == false)
                            {
                                pbxHandPiece5.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[5] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece5.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                            }
                            lblDeck.Text = remaining.ToString();
                            if (remaining <= 0)
                            {
                                GameOver = true;
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                turn++;
                lblTurn.Text = turn.ToString();
                m = 1; s = 59;
            }
            if (s < 0)
            {
                m = m - 1;
                s = 59;
            }
            lblSec.Text = s.ToString();
            lblMin.Text = m.ToString();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                //Clear Database when quiting
                string deleteString = "Delete * from GameDatabase";
                OleDbCommand command = new OleDbCommand(deleteString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            deck = new int[remaining];
            tomb = new int[40];
            select = new bool[6];
            select2 = new bool[11];
            pieceId3 = new int[11];
            pieceLocation = new int[21];
            attackPower = new int[11];
            moving = new bool[11];
            attacking = new bool[11];
            //Insert Data from Deck and Pieces into GameDatabase
            //Connect to pieces database
            try
            {
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string InsertSql = "Insert Into GameDatabase (ID, UserID, PieceID, Name, Skill, Condition, OriAtk, AltAtk, OriHP, MaxHp, AltHp, Move, Rank, Type, Active, Img1) Select Deck.ID, DeckID, PieceID, Name, Skill, Condition, Attack, Attack, Health, Health, Health, Move, Rank, Type, Active, Img1 From Deck Left Join Pieces ON Deck.PieceID = Pieces.ID";
                command.CommandText = InsertSql;
                OleDbDataReader reader = command.ExecuteReader();
                System.Threading.Thread.Sleep(1000);
                //MessageBox.Show("Connection Successful");
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed " + ex);
            }
            //Connect to the game database
            try
            {
                pieceId = new int[41];
                List<int> deckPieces = new List<int>();
                //Conncet to Deck database
                OleDbConnection connection = new OleDbConnection();
                connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string selectString = "Select * From GameDatabase Where UserID = 1 And Active = FALSE";
                command.CommandText = selectString;
                OleDbDataReader reader = command.ExecuteReader();
                //MessageBox.Show(command.CommandText);
                while (reader.Read())
                {
                    deckPieces.Add(reader.GetInt32(1));
                }
                deck = deckPieces.ToArray();
                //Set a random piece into the user hand
                for (int i = 0; i < 6; i++)
                {
                    int getPiece = rand.Next(deckPieces.Count) + 1;
                    string selectString2 = "select * from GameDatabase where ID = " + getPiece;
                    string updateString = "update GameDatabase Set Active = TRUE Where ID = " + getPiece;
                    OleDbCommand command2 = new OleDbCommand(selectString2, connection);
                    OleDbDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        if (pbxHandPiece1.Visible == false)
                        {
                            pbxHandPiece1.Image = Image.FromFile(reader2["Img1"].ToString());
                            pieceId[1] = Convert.ToInt32(reader2["ID"]);
                            pbxHandPiece1.Visible = true;
                            remaining--;
                            OleDbCommand command3 = new OleDbCommand(updateString, connection);
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            reader3.Read();
                            toolTip1.SetToolTip(pbxHandPiece1, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                        }
                        else if (pbxHandPiece2.Visible == false)
                        {
                            pbxHandPiece2.Image = Image.FromFile(reader2["Img1"].ToString());
                            pieceId[2] = Convert.ToInt32(reader2["ID"]);
                            pbxHandPiece2.Visible = true;
                            remaining--;
                            OleDbCommand command3 = new OleDbCommand(updateString, connection);
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            reader3.Read();
                            toolTip1.SetToolTip(pbxHandPiece2, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                        }
                        else if (pbxHandPiece3.Visible == false)
                        {
                            pbxHandPiece3.Image = Image.FromFile(reader2["Img1"].ToString());
                            pieceId[3] = Convert.ToInt32(reader2["ID"]);
                            pbxHandPiece3.Visible = true;
                            remaining--;
                            OleDbCommand command3 = new OleDbCommand(updateString, connection);
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            reader3.Read();
                            toolTip1.SetToolTip(pbxHandPiece3, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                        }
                        else if (pbxHandPiece4.Visible == false)
                        {
                            pbxHandPiece4.Image = Image.FromFile(reader2["Img1"].ToString());
                            pieceId[4] = Convert.ToInt32(reader2["ID"]);
                            pbxHandPiece4.Visible = true;
                            remaining--;
                            OleDbCommand command3 = new OleDbCommand(updateString, connection);
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            reader3.Read();
                            toolTip1.SetToolTip(pbxHandPiece4, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                        }
                        else if (pbxHandPiece5.Visible == false)
                        {
                            pbxHandPiece5.Image = Image.FromFile(reader2["Img1"].ToString());
                            pieceId[5] = Convert.ToInt32(reader2["ID"]);
                            pbxHandPiece5.Visible = true;
                            remaining--;
                            OleDbCommand command3 = new OleDbCommand(updateString, connection);
                            OleDbDataReader reader3 = command3.ExecuteReader();
                            reader3.Read();
                            toolTip1.SetToolTip(pbxHandPiece5, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                        }
                    }
                    string selectString4 = "Select * From GameDatabase Where ID = 41";
                    OleDbCommand command4 = new OleDbCommand(selectString4, connection);
                    OleDbDataReader reader4 = command4.ExecuteReader();
                    reader4.Read();
                    while (reader4.Read())
                    {
                        if (pbxPlTwoPiece1.Visible == false)
                        {
                            pbxPlTwoPiece1.Image = Image.FromFile(reader4["Img1"].ToString());
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            lblDeck.Text = remaining.ToString();
            lblTurn.Text = turn.ToString();
            lblMelee.Text = Melee.ToString();
            lblMagic.Text = Magic.ToString();
            lblLR.Text = LongRange.ToString();
            lblSpecial.Text = Special.ToString();
            countDown.Start();
            for (int i = 1; i < 6; i++)
            {
                select[i] = false;
            }
        }

        private void pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            space = new Point[143];
            topLeft = new Point[143];
            topRight = new Point[143];
            LeftS = new Point[143];
            RightS = new Point[143];
            bottomLeft = new Point[143];
            bottomRight = new Point[143];
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.LightGray, 10);
            id = 1;
            int average;
            //Create board short rows and long columns
            int y1 = 5, y2 = 5, y3 = 20, y4 = 35, y5 = 35, y6 = 20;
            for (int i = 0; i < 10; i++)
            {
                //Create row with seven spaces
                int x1 = 67, x2 = 52, x3 = 43, x4 = 52, x5 = 67, x6 = 76;
                for (int j = 1; j < 8; j++)
                {
                    //Create Hexagon
                    if (draw == true)
                    {
                        Draw(x1, x2, x3, x4, x5, x6, y1, y2, y3, y4, y5, y6, Brushes.LightGray, e);
                    }
                    //Get location
                    average = (x3 + x6) / 2;
                    topRight[id] = new Point(x1, y1);
                    topLeft[id] = new Point(x2, y2);
                    LeftS[id] = new Point(x3, y3);
                    bottomLeft[id] = new Point(x4, y4);
                    bottomRight[id] = new Point(x5, y5);
                    RightS[id] = new Point(x6, y6);
                    space[id] = new Point(x3 - 6, y1 - 5);
                    //DiSpecialay id number
                    string idDiSpecialay = id.ToString();
                    Font f = new Font("Poor Richard", 10);
                    g.DrawString(idDiSpecialay, f, Brushes.Blue, new Point(average, y1));
                    x1 = x1 + 75; x2 = x2 + 75; x3 = x3 + 75; x4 = x4 + 75; x5 = x5 + 75; x6 = x6 + 75;
                    id++;
                }
                //Move down
                y1 = y1 + 43; y2 = y2 + 43; y3 = y3 + 43; y4 = y4 + 43; y5 = y5 + 43; y6 = y6 + 43;
                id = id + 8;
            }
            id = 8;
            //Create board long rows and short columns
            y1 = 28; y2 = 28; y3 = 43; y4 = 58; y5 = 58; y6 = 43;
            for (int i = 0; i < 9; i++)
            {
                //Create row with eight spaces
                int x1 = 29, x2 = 14, x3 = 5, x4 = 14, x5 = 29, x6 = 38;
                for (int j = 1; j < 9; j++)
                {
                    if (draw == true)
                    {
                        Draw(x1, x2, x3, x4, x5, x6, y1, y2, y3, y4, y5, y6, Brushes.LightGray, e);
                    }
                    //Get location
                    average = (x3 + x6) / 2;
                    topRight[id] = new Point(x1, y1);
                    topLeft[id] = new Point(x2, y2);
                    LeftS[id] = new Point(x3, y3);
                    bottomLeft[id] = new Point(x4, y4);
                    bottomRight[id] = new Point(x5, y5);
                    RightS[id] = new Point(x6, y6);
                    space[id] = new Point(x3 - 6, y1 - 5);
                    //DiSpecialay id number
                    string idDiSpecialay = id.ToString();
                    Font f = new Font("Poor Richard", 10);
                    g.DrawString(idDiSpecialay, f, Brushes.Blue, new Point(average, y1));
                    x1 = x1 + 75; x2 = x2 + 75; x3 = x3 + 75; x4 = x4 + 75; x5 = x5 + 75; x6 = x6 + 75;
                    id++;
                }
                //Move down
                y1 = y1 + 43; y2 = y2 + 43; y3 = y3 + 43; y4 = y4 + 43; y5 = y5 + 43; y6 = y6 + 43;
                //Change row id
                id = id + 7;
            }
        }
        
        private void pnlBoard_Click(object sender, EventArgs e)
        {
            Point point = pnlBoard.PointToClient(Cursor.Position);
            int point1, point2, point3, point4;
            if (drawStage == true || setup == true)
            {
                try
                {
                    string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                    OleDbConnection connection = new OleDbConnection(ConnectionString);
                    connection.Open();
                    string selectString = "select * from GameDatabase Where ID=" + pieceId2;
                    OleDbCommand command = new OleDbCommand(selectString, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    //MessageBox.Show("Connection Successful");
                    if (reader.Read())
                    {
                        if (pbxPlOnePiece1.Visible == false || pbxPlOnePiece2.Visible == false ||
                    pbxPlOnePiece3.Visible == false || pbxPlOnePiece4.Visible == false ||
                    pbxPlOnePiece5.Visible == false || pbxPlOnePiece6.Visible == false ||
                    pbxPlOnePiece7.Visible == false || pbxPlOnePiece8.Visible == false ||
                    pbxPlOnePiece9.Visible == false || pbxPlOnePiece10.Visible == false)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                if (select[i] == true)
                                {
                                    for (int j = 0; j < 143; j++)
                                    {
                                        if (j == 1 || j == 2 || j == 8 || j == 9 || j == 16 || j == 17 || j == 23 || j == 24 ||
                                            j == 31 || j == 32 || j == 38 || j == 39 || j == 46 || j == 47 || j == 53 || j == 54 ||
                                            j == 61 || j == 62 || j == 68 || j == 69 || j == 76 || j == 77 || j == 83 || j == 84 ||
                                            j == 91 || j == 92 || j == 98 || j == 99 || j == 106 || j == 107 || j == 113 || j == 114 ||
                                            j == 121 || j == 122 || j == 128 || j == 129 || j == 136 || j == 137)
                                        {
                                            point1 = (LeftS[j].X * topLeft[j].Y) / 2;
                                            point2 = (LeftS[j].X * bottomLeft[j].Y) / 2;
                                            point3 = (RightS[j].X * topRight[j].Y) / 2;
                                            point4 = (RightS[j].X * bottomRight[j].Y) / 2;
                                            //If a hexagon is clicked on then the piece will move there
                                            if (point.X > topLeft[j].X && point.Y > topLeft[j].Y &&
                                                point.X < topRight[j].X && point.Y > topRight[j].Y &&
                                                point.X > bottomLeft[j].X && point.Y < bottomLeft[j].Y &&
                                                point.X < bottomRight[j].X && point.Y < bottomRight[j].Y)
                                            {
                                                if (select[1])
                                                {//Make The piece from the hand appear on the board
                                                    if (pbxPlOnePiece1.Visible != true)
                                                    {
                                                        pieceId3[1] = pieceId2;
                                                        pbxPlOnePiece1.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece1.Visible = true;
                                                        pbxPlOnePiece1.Location = space[j];
                                                        pieceLocation[1] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece2.Visible != true)
                                                    {
                                                        pieceId3[2] = pieceId2;
                                                        pbxPlOnePiece2.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece2.Visible = true;
                                                        pbxPlOnePiece2.Location = space[j];
                                                        pieceLocation[2] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece2, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece3.Visible != true)
                                                    {
                                                        pieceId3[3] = pieceId2;
                                                        pbxPlOnePiece3.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece3.Visible = true;
                                                        pbxPlOnePiece3.Location = space[j];
                                                        pieceLocation[3] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece3, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece4.Visible != true)
                                                    {
                                                        pieceId3[4] = pieceId2;
                                                        pbxPlOnePiece4.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece4.Visible = true;
                                                        pbxPlOnePiece4.Location = space[j];
                                                        pieceLocation[4] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece4, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece5.Visible != true)
                                                    {
                                                        pieceId3[5] = pieceId2;
                                                        pbxPlOnePiece5.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece5.Visible = true;
                                                        pbxPlOnePiece5.Location = space[j];
                                                        pieceLocation[5] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece5, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece6.Visible != true)
                                                    {
                                                        pieceId3[6] = pieceId2;
                                                        pbxPlOnePiece6.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece6.Visible = true;
                                                        pbxPlOnePiece6.Location = space[j];
                                                        pieceLocation[6] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece6, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece7.Visible != true)
                                                    {
                                                        pieceId3[7] = pieceId2;
                                                        pbxPlOnePiece7.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece7.Visible = true;
                                                        pbxPlOnePiece7.Location = space[j];
                                                        pieceLocation[7] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece7, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece8.Visible != true)
                                                    {
                                                        pieceId3[8] = pieceId2;
                                                        pbxPlOnePiece8.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece8.Visible = true;
                                                        pbxPlOnePiece8.Location = space[j];
                                                        pieceLocation[8] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece8, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece9.Visible != true)
                                                    {
                                                        pieceId3[9] = pieceId2;
                                                        pbxPlOnePiece9.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece9.Visible = true;
                                                        pbxPlOnePiece9.Location = space[j];
                                                        pieceLocation[9] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece9, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece10.Visible != true)
                                                    {
                                                        pieceId3[10] = pieceId2;
                                                        pbxPlOnePiece10.Image = pbxHandPiece1.Image;
                                                        pbxHandPiece1.Visible = false;
                                                        pbxPlOnePiece10.Visible = true;
                                                        pbxPlOnePiece10.Location = space[j];
                                                        pieceLocation[10] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece1.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece10, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                }
                                                else if (select[2])
                                                {
                                                    if (pbxPlOnePiece1.Visible != true)
                                                    {
                                                        pieceId3[1] = pieceId2;
                                                        pbxPlOnePiece1.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece1.Visible = true;
                                                        pbxPlOnePiece1.Location = space[j];
                                                        pieceLocation[1] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece2.Visible != true)
                                                    {
                                                        pieceId3[2] = pieceId2;
                                                        pbxPlOnePiece2.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece2.Visible = true;
                                                        pbxPlOnePiece2.Location = space[j];
                                                        pieceLocation[2] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece2, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece3.Visible != true)
                                                    {
                                                        pieceId3[3] = pieceId2;
                                                        pbxPlOnePiece3.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece3.Visible = true;
                                                        pbxPlOnePiece3.Location = space[j];
                                                        pieceLocation[3] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece3, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece4.Visible != true)
                                                    {
                                                        pieceId3[4] = pieceId2;
                                                        pbxPlOnePiece4.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece4.Visible = true;
                                                        pbxPlOnePiece4.Location = space[j];
                                                        pieceLocation[4] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece4, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece5.Visible != true)
                                                    {
                                                        pieceId3[5] = pieceId2;
                                                        pbxPlOnePiece5.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece5.Visible = true;
                                                        pbxPlOnePiece5.Location = space[j];
                                                        pieceLocation[5] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece5, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece6.Visible != true)
                                                    {
                                                        pieceId3[6] = pieceId2;
                                                        pbxPlOnePiece6.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece6.Visible = true;
                                                        pbxPlOnePiece6.Location = space[j];
                                                        pieceLocation[6] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece6, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece7.Visible != true)
                                                    {
                                                        pieceId3[7] = pieceId2;
                                                        pbxPlOnePiece7.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece7.Visible = true;
                                                        pbxPlOnePiece7.Location = space[j];
                                                        pieceLocation[7] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece7, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece8.Visible != true)
                                                    {
                                                        pieceId3[8] = pieceId2;
                                                        pbxPlOnePiece8.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece8.Visible = true;
                                                        pbxPlOnePiece8.Location = space[j];
                                                        pieceLocation[8] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece8, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece9.Visible != true)
                                                    {
                                                        pieceId3[9] = pieceId2;
                                                        pbxPlOnePiece9.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece9.Visible = true;
                                                        pbxPlOnePiece9.Location = space[j];
                                                        pieceLocation[9] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece9, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece10.Visible != true)
                                                    {
                                                        pieceId3[10] = pieceId2;
                                                        pbxPlOnePiece10.Image = pbxHandPiece2.Image;
                                                        pbxHandPiece2.Visible = false;
                                                        pbxPlOnePiece10.Visible = true;
                                                        pbxPlOnePiece10.Location = space[j];
                                                        pieceLocation[10] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece2.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece10, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                }
                                                else if (select[3])
                                                {
                                                    if (pbxPlOnePiece1.Visible != true)
                                                    {
                                                        pieceId3[1] = pieceId2;
                                                        pbxPlOnePiece1.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece1.Visible = true;
                                                        pbxPlOnePiece1.Location = space[j];
                                                        pieceLocation[1] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece2.Visible != true)
                                                    {
                                                        pieceId3[2] = pieceId2;
                                                        pbxPlOnePiece2.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece2.Visible = true;
                                                        pbxPlOnePiece2.Location = space[j];
                                                        pieceLocation[2] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece2, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece3.Visible != true)
                                                    {
                                                        pieceId3[3] = pieceId2;
                                                        pbxPlOnePiece3.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece3.Visible = true;
                                                        pbxPlOnePiece3.Location = space[j];
                                                        pieceLocation[3] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece3, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece4.Visible != true)
                                                    {
                                                        pieceId3[4] = pieceId2;
                                                        pbxPlOnePiece4.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece4.Visible = true;
                                                        pbxPlOnePiece4.Location = space[j];
                                                        pieceLocation[4] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece4, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece5.Visible != true)
                                                    {
                                                        pieceId3[5] = pieceId2;
                                                        pbxPlOnePiece5.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece5.Visible = true;
                                                        pbxPlOnePiece5.Location = space[j];
                                                        pieceLocation[5] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece5, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece6.Visible != true)
                                                    {
                                                        pieceId3[6] = pieceId2;
                                                        pbxPlOnePiece6.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece6.Visible = true;
                                                        pbxPlOnePiece6.Location = space[j];
                                                        pieceLocation[6] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece6, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece7.Visible != true)
                                                    {
                                                        pieceId3[7] = pieceId2;
                                                        pbxPlOnePiece7.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece7.Visible = true;
                                                        pbxPlOnePiece7.Location = space[j];
                                                        pieceLocation[7] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece7, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece8.Visible != true)
                                                    {
                                                        pieceId3[8] = pieceId2;
                                                        pbxPlOnePiece8.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece8.Visible = true;
                                                        pbxPlOnePiece8.Location = space[j];
                                                        pieceLocation[8] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece8, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece9.Visible != true)
                                                    {
                                                        pieceId3[9] = pieceId2;
                                                        pbxPlOnePiece9.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece9.Visible = true;
                                                        pbxPlOnePiece9.Location = space[j];
                                                        pieceLocation[9] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece9, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece10.Visible != true)
                                                    {
                                                        pieceId3[10] = pieceId2;
                                                        pbxPlOnePiece10.Image = pbxHandPiece3.Image;
                                                        pbxHandPiece3.Visible = false;
                                                        pbxPlOnePiece10.Visible = true;
                                                        pbxPlOnePiece10.Location = space[j];
                                                        pieceLocation[10] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece3.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece10, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                }
                                                else if (select[4])
                                                {
                                                    if (pbxPlOnePiece1.Visible != true)
                                                    {
                                                        pieceId3[1] = pieceId2;
                                                        pbxPlOnePiece1.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece1.Visible = true;
                                                        pbxPlOnePiece1.Location = space[j];
                                                        pieceLocation[1] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece2.Visible != true)
                                                    {
                                                        pieceId3[2] = pieceId2;
                                                        pbxPlOnePiece2.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece2.Visible = true;
                                                        pbxPlOnePiece2.Location = space[j];
                                                        pieceLocation[2] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece2, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece3.Visible != true)
                                                    {
                                                        pieceId3[3] = pieceId2;
                                                        pbxPlOnePiece3.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece3.Visible = true;
                                                        pbxPlOnePiece3.Location = space[j];
                                                        pieceLocation[3] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece3, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece4.Visible != true)
                                                    {
                                                        pieceId3[4] = pieceId2;
                                                        pbxPlOnePiece4.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece4.Visible = true;
                                                        pbxPlOnePiece4.Location = space[j];
                                                        pieceLocation[4] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece4, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece5.Visible != true)
                                                    {
                                                        pieceId3[5] = pieceId2;
                                                        pbxPlOnePiece5.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece5.Visible = true;
                                                        pbxPlOnePiece5.Location = space[j];
                                                        pieceLocation[5] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece5, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece6.Visible != true)
                                                    {
                                                        pieceId3[6] = pieceId2;
                                                        pbxPlOnePiece6.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece6.Visible = true;
                                                        pbxPlOnePiece6.Location = space[j];
                                                        pieceLocation[6] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece6, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece7.Visible != true)
                                                    {
                                                        pieceId3[7] = pieceId2;
                                                        pbxPlOnePiece7.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece7.Visible = true;
                                                        pbxPlOnePiece7.Location = space[j];
                                                        pieceLocation[7] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece7, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece8.Visible != true)
                                                    {
                                                        pieceId3[8] = pieceId2;
                                                        pbxPlOnePiece8.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece8.Visible = true;
                                                        pbxPlOnePiece8.Location = space[j];
                                                        pieceLocation[8] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece8, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece9.Visible != true)
                                                    {
                                                        pieceId3[9] = pieceId2;
                                                        pbxPlOnePiece9.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece9.Visible = true;
                                                        pbxPlOnePiece9.Location = space[j];
                                                        pieceLocation[9] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece9, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece10.Visible != true)
                                                    {
                                                        pieceId3[10] = pieceId2;
                                                        pbxPlOnePiece10.Image = pbxHandPiece4.Image;
                                                        pbxHandPiece4.Visible = false;
                                                        pbxPlOnePiece10.Visible = true;
                                                        pbxPlOnePiece10.Location = space[j];
                                                        pieceLocation[10] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece4.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece10, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                }
                                                else if (select[5])
                                                {
                                                    if (pbxPlOnePiece1.Visible != true)
                                                    {
                                                        pieceId3[1] = pieceId2;
                                                        pbxPlOnePiece1.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece1.Visible = true;
                                                        pbxPlOnePiece1.Location = space[j];
                                                        pieceLocation[1] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece2.Visible != true)
                                                    {
                                                        pieceId3[2] = pieceId2;
                                                        pbxPlOnePiece2.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece2.Visible = true;
                                                        pbxPlOnePiece2.Location = space[j];
                                                        pieceLocation[2] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece2, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece3.Visible != true)
                                                    {
                                                        pieceId3[3] = pieceId2;
                                                        pbxPlOnePiece3.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece3.Visible = true;
                                                        pbxPlOnePiece3.Location = space[j];
                                                        pieceLocation[3] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece3, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece4.Visible != true)
                                                    {
                                                        pieceId3[4] = pieceId2;
                                                        pbxPlOnePiece4.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece4.Visible = true;
                                                        pbxPlOnePiece4.Location = space[j];
                                                        pieceLocation[4] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece4, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece5.Visible != true)
                                                    {
                                                        pieceId3[5] = pieceId2;
                                                        pbxPlOnePiece5.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece5.Visible = true;
                                                        pbxPlOnePiece5.Location = space[j];
                                                        pieceLocation[5] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece5, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece6.Visible != true)
                                                    {
                                                        pieceId3[6] = pieceId2;
                                                        pbxPlOnePiece6.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece6.Visible = true;
                                                        pbxPlOnePiece6.Location = space[j];
                                                        pieceLocation[6] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece6, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece7.Visible != true)
                                                    {
                                                        pieceId3[7] = pieceId2;
                                                        pbxPlOnePiece7.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece7.Visible = true;
                                                        pbxPlOnePiece7.Location = space[j];
                                                        pieceLocation[7] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece7, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece8.Visible != true)
                                                    {
                                                        pieceId3[8] = pieceId2;
                                                        pbxPlOnePiece8.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece8.Visible = true;
                                                        pbxPlOnePiece8.Location = space[j];
                                                        pieceLocation[8] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece8, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece9.Visible != true)
                                                    {
                                                        pieceId3[9] = pieceId2;
                                                        pbxPlOnePiece9.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece9.Visible = true;
                                                        pbxPlOnePiece9.Location = space[j];
                                                        pieceLocation[9] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece9, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                    else if (pbxPlOnePiece10.Visible != true)
                                                    {
                                                        pieceId3[10] = pieceId2;
                                                        pbxPlOnePiece10.Image = pbxHandPiece5.Image;
                                                        pbxHandPiece5.Visible = false;
                                                        pbxPlOnePiece10.Visible = true;
                                                        pbxPlOnePiece10.Location = space[j];
                                                        pieceLocation[10] = j;
                                                        if (reader["Type"].ToString() == "Melee")
                                                        {
                                                            Melee++;
                                                            lblMelee.Text = Melee.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Magic")
                                                        {
                                                            Magic++;
                                                            lblMagic.Text = Magic.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "LongRange")
                                                        {
                                                            LongRange++;
                                                            lblLR.Text = LongRange.ToString();
                                                        }
                                                        else if (reader["Type"].ToString() == "Special")
                                                        {
                                                            Special++;
                                                            lblSpecial.Text = Special.ToString();
                                                        }
                                                        select[i] = false;
                                                        pbxHandPiece5.Image = Image.FromFile("BlankPiece.gif");
                                                        toolTip1.SetToolTip(pbxPlOnePiece10, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                                                        reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                                                        reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                                                        reader["Type"].ToString());
                                                    }
                                                }
                                            }
                                            if (drawStage == true)
                                            {
                                                pnlBoard.Invalidate();
                                                drawStage = false;
                                                moveStage = true;
                                                lblStage.Text = "Move Stage";
                                                for (int k = 0; k < 11; k++)
                                                {
                                                    moving[k] = true;
                                                }
                                            }
                                            Drawing(false);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Failed " + ex);
                }
            }
            else if (moveStage == true)
            {
                Graphics g = pnlBoard.CreateGraphics();
                try
                {
                    int moveValues = 0;
                    int[] moveTo;
                    moveTo = new int[moveValues];
                    string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                    OleDbConnection connection = new OleDbConnection(ConnectionString);
                    connection.Open();
                    string selectString = "select * from GameDatabase where ID=" + pieceId4;
                    OleDbCommand command = new OleDbCommand(selectString, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        for (int i = 0; i < 11; i++)
                        {
                            if (select2[i] == true)
                            {
                                moveFor = Convert.ToInt32(reader["Move"]);
                                for (int j = 0; j < 143; j++)
                                {
                                    point1 = (LeftS[j].X * topLeft[j].Y) / 2;
                                    point2 = (LeftS[j].X * bottomLeft[j].Y) / 2;
                                    point3 = (RightS[j].X * topRight[j].Y) / 2;
                                    point4 = (RightS[j].X * bottomRight[j].Y) / 2;
                                    //If a hexagon is clicked on then the piece will move there
                                    if (point.X > topLeft[j].X && point.Y > topLeft[j].Y &&
                                        point.X < topRight[j].X && point.Y > topRight[j].Y &&
                                        point.X > bottomLeft[j].X && point.Y < bottomLeft[j].Y &&
                                        point.X < bottomRight[j].X && point.Y < bottomRight[j].Y)
                                    {
                                        for (int k = 1; k < moveValue.Count; k++)
                                        {
                                            if (j == moveValue[k])
                                            {
                                                if (select2[1])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[1] = j;
                                                    pbxPlOnePiece1.Location = space[j];
                                                    select2[1] = false;
                                                    moving[1] = false;
                                                }
                                                if (select2[2])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[2] = j;
                                                    pbxPlOnePiece2.Location = space[j];
                                                    select2[2] = false;
                                                    moving[2] = false;
                                                }
                                                if (select2[3])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[3] = j;
                                                    pbxPlOnePiece3.Location = space[j];
                                                    select2[3] = false;
                                                    moving[3] = false;
                                                }
                                                if (select2[4])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[4] = j;
                                                    pbxPlOnePiece4.Location = space[j];
                                                    select2[4] = false;
                                                    moving[4] = false;
                                                }
                                                if (select2[5])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[5] = j;
                                                    pbxPlOnePiece5.Location = space[j];
                                                    select2[5] = false;
                                                    moving[5] = false;
                                                }
                                                if (select2[6])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[6] = j;
                                                    pbxPlOnePiece6.Location = space[j];
                                                    select2[6] = false;
                                                    moving[6] = false;
                                                }
                                                if (select2[7])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[7] = j;
                                                    pbxPlOnePiece7.Location = space[j];
                                                    select2[7] = false;
                                                    moving[7] = false;
                                                }
                                                if (select2[8])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[8] = j;
                                                    pbxPlOnePiece8.Location = space[j];
                                                    select2[8] = false;
                                                    moving[8] = false;
                                                }
                                                if (select2[9])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[9] = j;
                                                    pbxPlOnePiece9.Location = space[j];
                                                    select2[9] = false;
                                                    moving[9] = false;
                                                }
                                                if (select2[10])
                                                {
                                                    pnlBoard.Invalidate();
                                                    pieceLocation[10] = j;
                                                    pbxPlOnePiece10.Location = space[j];
                                                    select2[10] = false;
                                                    moving[10] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Failed " + ex);
                }
            }
            if (attackStage == true)
            {
                try
                {
                    string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                    OleDbConnection connection = new OleDbConnection(ConnectionString);
                    connection.Open();
                    string selectString = "select * from GameDatabase Where ID = 41";
                    OleDbCommand command = new OleDbCommand(selectString, connection);
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < 11; i++)
                        {
                            if (select2[i] == true)
                            {
                                for (int j = 0; j < 143; j++)
                                {
                                    point1 = (LeftS[j].X * topLeft[j].Y) / 2;
                                    point2 = (LeftS[j].X * bottomLeft[j].Y) / 2;
                                    point3 = (RightS[j].X * topRight[j].Y) / 2;
                                    point4 = (RightS[j].X * bottomRight[j].Y) / 2;
                                    //If a hexagon is clicked on then the piece will move there
                                    if (point.X > topLeft[j].X && point.Y > topLeft[j].Y &&
                                        point.X < topRight[j].X && point.Y > topRight[j].Y &&
                                        point.X > bottomLeft[j].X && point.Y < bottomLeft[j].Y &&
                                        point.X < bottomRight[j].X && point.Y < bottomRight[j].Y)
                                    {

                                        extraAttack(j);
                                        for (int k = 0; k < extra.Count; k++)
                                        {
                                            if (pieceLocation[11] == extra[k])
                                            {
                                                hp = Convert.ToInt32(reader["AltHp"]);
                                                //Goblin take damage when on the same space as an attack value
                                                hp = hp - attackPower[i];
                                                string updateDatabase = "Update GameDatabase Set AltHp = " + hp + " Where ID = 41";
                                                OleDbCommand command2 = new OleDbCommand(updateDatabase, connection);
                                                OleDbDataReader reader2 = command2.ExecuteReader();
                                                reader2.Read();
                                                if (hp <= 0)
                                                {
                                                    pbxPlTwoPiece1.Visible = false;
                                                    GameOver = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection Failed " + ex);
                }
            }
        }

        private void pnlHand_Paint_1(object sender, PaintEventArgs e)
        {
            hand = new Point[5];
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.LightGray, 10);
            handId = 1;
            int average;
            int x1 = 29, x2 = 14, x3 = 5, x4 = 14, x5 = 29, x6 = 38;
            int y1 = 4, y2 = 4, y3 = 20, y4 = 36, y5 = 36, y6 = 20;
            for (int i = 0; i < 5; i++)
            {
                Draw(x1, x2, x3, x4, x5, x6, y1, y2, y3, y4, y5, y6, Brushes.LightGray, e);
                average = (x3 + x6) / 2;
                hand[i] = new Point(x3 - 6, y1 - 5);
                string idDiSpecialay = handId.ToString();
                Font f = new Font("Poor Richard", 10);
                g.DrawString(idDiSpecialay, f, Brushes.Blue, new Point(average, y1));

                y1 = y1 + 55; y2 = y2 + 55; y3 = y3 + 55; y4 = y4 + 55; y5 = y5 + 55; y6 = y6 + 55;
                handId++;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (setup == true)
            {
                if (pbxHandPiece1.Visible != false && pbxHandPiece2.Visible != false && pbxHandPiece3.Visible != false &&
                    pbxHandPiece4.Visible != false && pbxHandPiece5.Visible != false)
                {
                    MessageBox.Show("You need to have at least one piece on the board.");
                }
                else
                {
                    try
                    {
                        string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                        OleDbConnection connection = new OleDbConnection(ConnectionString);
                        connection.Open();
                        string selectString = "select * from GameDatabase Where ID = 41";
                        OleDbCommand command = new OleDbCommand(selectString, connection);
                        OleDbDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            pnlBoard.Invalidate();
                            setup = false;
                            moveStage = true;
                            lblStage.Text = "Move Stage";
                            pbxPlTwoPiece1.Location = space[74];
                            pieceLocation[11] = 74;
                            pbxPlTwoPiece1.Visible = true;
                            for (int i = 0; i < 11; i++)
                            {
                                moving[i] = true;
                            }
                            toolTip1.SetToolTip(pbxPlTwoPiece1, "Name: " + reader["Name"].ToString() + "\nAttack: " +
                               reader["OriAtk"].ToString() + "\nHealth: " + reader["OriHp"].ToString() + "\nMove: " +
                               reader["Move"].ToString() + "\nRank: " + reader["Rank"].ToString() + "\nType: " +
                               reader["Type"].ToString());
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Connection Failed " + ex);
                    }
                }
            }
            else if (drawStage == true)
            {
                pnlBoard.Invalidate();
                drawStage = false;
                moveStage = true;
                lblStage.Text = "Move Stage";
                for (int i = 0; i < 11; i++)
                {
                    moving[i] = true;
                }
            }
            else if (moveStage == true)
            {
                pnlBoard.Invalidate();
                moveStage = false;
                attackStage = true;
                lblStage.Text = "Attack Stage";
                for (int i = 0; i < 11; i++)
                {
                    attacking[i] = true;
                }
            }
            else if (attackStage == true)
            {
                pnlBoard.Invalidate();
                attackStage = false;
                drawStage = true;
                lblStage.Text = "Draw Stage";
                try
                {
                    List<int> deckPieces = new List<int>();
                    //Conncet to Deck database
                    OleDbConnection connection = new OleDbConnection();
                    connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                    connection.Open();
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string selectString = "Select * From GameDatabase Where UserID = 1 And Active = FALSE";
                    command.CommandText = selectString;
                    OleDbDataReader reader = command.ExecuteReader();
                    //MessageBox.Show(command.CommandText);
                    while (reader.Read())
                    {
                        deckPieces.Add(reader.GetInt32(0));
                    }
                    deck = deckPieces.ToArray();
                    //Set a random piece into the user hand
                    for (int i = 0; i < 6; i++)
                    {
                        int getPiece = rand.Next(deckPieces.Count) + 1;
                        string selectString2 = "select * from GameDatabase where ID = " + getPiece;
                        string updateString = "update GameDatabase Set Active = TRUE Where ID = " + getPiece;
                        OleDbCommand command2 = new OleDbCommand(selectString2, connection);
                        OleDbDataReader reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            if (pbxHandPiece1.Visible == false)
                            {
                                pbxHandPiece1.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[1] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece1.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                                toolTip1.SetToolTip(pbxHandPiece1, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                            }
                            else if (pbxHandPiece2.Visible == false)
                            {
                                pbxHandPiece2.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[2] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece2.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                                toolTip1.SetToolTip(pbxHandPiece2, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                            }
                            else if (pbxHandPiece3.Visible == false)
                            {
                                pbxHandPiece3.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[3] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece3.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                                toolTip1.SetToolTip(pbxHandPiece3, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                            }
                            else if (pbxHandPiece4.Visible == false)
                            {
                                pbxHandPiece4.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[4] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece4.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                                toolTip1.SetToolTip(pbxHandPiece4, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                            }
                            else if (pbxHandPiece5.Visible == false)
                            {
                                pbxHandPiece5.Image = Image.FromFile(reader2["Img1"].ToString());
                                pieceId[5] = Convert.ToInt32(reader2["ID"]);
                                pbxHandPiece5.Visible = true;
                                OleDbCommand command3 = new OleDbCommand(updateString, connection);
                                OleDbDataReader reader3 = command3.ExecuteReader();
                                reader3.Read();
                                remaining--;
                                toolTip1.SetToolTip(pbxHandPiece5, "Name: " + reader2["Name"].ToString() + "\nAttack: " +
                            reader2["OriAtk"].ToString() + "\nHealth: " + reader2["OriHp"].ToString() + "\nMove: " +
                            reader2["Move"].ToString() + "\nRank: " + reader2["Rank"].ToString() + "\nType: " +
                            reader2["Type"].ToString());
                            }
                            lblDeck.Text = remaining.ToString();
                            if (remaining <= 0)
                            {
                                GameOver = true;
                            }
                            if (GameOver == true)
                            {
                                //Set conditions for winning or losing
                                if (pbxPlTwoPiece1.Visible == false)
                                {
                                    MessageBox.Show("Congratulations! You've Won!");
                                    //Clear Database when quiting
                                    string deleteString = "Delete * from GameDatabase";
                                    OleDbCommand command4 = new OleDbCommand(deleteString, connection);
                                    OleDbDataReader reader4 = command4.ExecuteReader();
                                    connection.Close();
                                    this.Close();
                                }
                                else if (remaining <= 0)
                                {
                                    MessageBox.Show("Sorry! You've Lost!");
                                    //Clear Database when quiting
                                    string deleteString = "Delete * from GameDatabase";
                                    OleDbCommand command4 = new OleDbCommand(deleteString, connection);
                                    OleDbDataReader reader4 = command4.ExecuteReader();
                                    connection.Close();
                                    this.Close();
                                }
                                else if (Melee == 0 && Magic == 0 && LongRange == 0 && Special == 0)
                                {
                                    MessageBox.Show("Sorry! You've Lost!");
                                    //Clear Database when quiting
                                    string deleteString = "Delete * from GameDatabase";
                                    OleDbCommand command4 = new OleDbCommand(deleteString, connection);
                                    OleDbDataReader reader4 = command4.ExecuteReader();
                                    connection.Close();
                                    this.Close();
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
                turn++;
                lblTurn.Text = turn.ToString();
                m = 1; s = 59;
            }
        }

        private void pbxHandPiece1_Click_1(object sender, EventArgs e)
        {
            pieceId2 = pieceId[1];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId2;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                //MessageBox.Show("Connection Successful");
                if (reader.Read())
                {
                    if (pbxHandPiece1.Visible == true)
                    {
                        //int pieceId = reader["ID"];
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["OriAtk"].ToString();
                        lblHp.Text = reader["OriHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select[1] = true;
                        select[2] = false;
                        select[3] = false;
                        select[4] = false;
                        select[5] = false;
                        Drawing(true);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxHandPiece2_Click_1(object sender, EventArgs e)
        {
            pieceId2 = pieceId[2];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId2;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxHandPiece2.Visible == true)
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["OriAtk"].ToString();
                        lblHp.Text = reader["OriHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select[1] = false;
                        select[2] = true;
                        select[3] = false;
                        select[4] = false;
                        select[5] = false;
                        Drawing(true);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxHandPiece3_Click_1(object sender, EventArgs e)
        {
            pieceId2 = pieceId[3];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId2;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxHandPiece3.Visible == true)
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["OriAtk"].ToString();
                        lblHp.Text = reader["OriHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select[1] = false;
                        select[2] = false;
                        select[3] = true;
                        select[4] = false;
                        select[5] = false;
                        Drawing(true);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxHandPiece4_Click_1(object sender, EventArgs e)
        {
            pieceId2 = pieceId[4];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId2;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxHandPiece4.Visible == true)
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["OriAtk"].ToString();
                        lblHp.Text = reader["OriHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select[1] = false;
                        select[2] = false;
                        select[3] = false;
                        select[4] = true;
                        select[5] = false;
                        Drawing(true);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxHandPiece5_Click_1(object sender, EventArgs e)
        {
            pieceId2 = pieceId[5];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId2;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxHandPiece5.Visible == true)
                    {
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["OriAtk"].ToString();
                        lblHp.Text = reader["OriHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select[1] = false;
                        select[2] = false;
                        select[3] = false;
                        select[4] = false;
                        select[5] = true;
                        Drawing(true);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece1_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[1];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece1.Visible)
                    {
                        attackPower[1] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = true;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[1] == true)
                            {
                                Moving(1);
                                //moving[1] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[1] == true)
                            {
                                Attacking(1);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece2_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[2];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece2.Visible)
                    {
                        attackPower[2] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = true;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[2] == true)
                            {
                                Moving(2);
                                //moving[2] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[2] == true)
                            {
                                Attacking(2);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece3_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[3];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece3.Visible)
                    {
                        attackPower[3] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = true;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[3] == true)
                            {
                                Moving(3);
                                //moving[3] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[3] == true)
                            {
                                Attacking(3);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece4_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[4];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece4.Visible)
                    {
                        attackPower[4] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = true;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[4] == true)
                            {
                                Moving(4);
                                //moving[4] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[4] == true)
                            {
                                Attacking(4);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece5_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[5];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece5.Visible)
                    {
                        attackPower[5] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = true;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[5] == true)
                            {
                                Moving(5);
                                //moving[5] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[5] == true)
                            {
                                Attacking(5);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece6_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[6];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece6.Visible)
                    {
                        attackPower[6] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = true;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[6] == true)
                            {
                                Moving(6);
                                //moving[6] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[6] == true)
                            {
                                Attacking(6);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece7_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[7];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece7.Visible)
                    {
                        attackPower[7] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = true;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[7] == true)
                            {
                                Moving(7);
                                //moving[7] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[7] == true)
                            {
                                Attacking(7);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece8_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[8];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece8.Visible)
                    {
                        attackPower[8] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = true;
                        select2[9] = false;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[8] == true)
                            {
                                Moving(8);
                                //moving[8] = false;
                            }
                        }

                        else if (attackStage == true)
                        {
                            if (attacking[8] == true)
                            {
                                Attacking(8);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece9_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[9];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                //MessageBox.Show("Connection Successful");
                if (reader.Read())
                {
                    if (pbxPlOnePiece9.Visible)
                    {
                        attackPower[9] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = true;
                        select2[10] = false;
                        if (moveStage == true)
                        {
                            if (moving[9] == true)
                            {
                                Moving(9);
                                //moving[9] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[9] == true)
                            {
                                Attacking(9);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlOnePiece10_Click(object sender, EventArgs e)
        {
            pieceId4 = pieceId3[10];
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlOnePiece10.Visible)
                    {
                        attackPower[10] = Convert.ToInt32(reader["AltAtk"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        select2[1] = false;
                        select2[2] = false;
                        select2[3] = false;
                        select2[4] = false;
                        select2[5] = false;
                        select2[6] = false;
                        select2[7] = false;
                        select2[8] = false;
                        select2[9] = false;
                        select2[10] = true;
                        if (moveStage == true)
                        {
                            if (moving[10] == true)
                            {
                                Moving(10);
                                //moving[10] = false;
                            }
                        }
                        else if (attackStage == true)
                        {
                            if (attacking[10] == true)
                            {
                                Attacking(10);
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void pbxPlTwoPiece1_Click(object sender, EventArgs e)
        {
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=41";
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (pbxPlTwoPiece1.Visible)
                    {
                        hp = Convert.ToInt32(reader["AltHp"]);
                        lblName.Text = reader["Name"].ToString();
                        lblAtk.Text = reader["AltAtk"].ToString();
                        lblHp.Text = reader["AltHp"].ToString();
                        lblMove.Text = reader["Move"].ToString();
                        lblRank.Text = reader["Rank"].ToString();
                        if (attackStage == true)
                        {
                            for (int i = 0; i < attackValue.Count; i++)
                            {
                                if (pieceLocation[11] == attackValue[i])
                                {
                                    for (int j = 1; j < 11; j++)
                                    {
                                        if (select2[j] == true && attacking[j] == true)
                                        {
                                            //Goblin take damage when on the same space as an attack value
                                            hp = hp - attackPower[j];
                                            string updateDatabase = "Update GameDatabase Set AltHp = " + hp + " Where ID = 41";
                                            attacking[j] = false;
                                            OleDbCommand command2 = new OleDbCommand(updateDatabase, connection);
                                            OleDbDataReader reader2 = command2.ExecuteReader();
                                            reader2.Read();
                                            if (hp <= 0)
                                            {
                                                pbxPlTwoPiece1.Visible = false;
                                                GameOver = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void Draw(int pointx1, int pointx2, int pointx3, int pointx4, int pointx5, int pointx6,
            int pointy1, int pointy2, int pointy3, int pointy4, int pointy5, int pointy6, Brush colour,
            PaintEventArgs e)
        {
            Point[] shape = new Point[6];
            Pen p = new Pen(colour, 10);
            Graphics g = e.Graphics;
            shape[0] = new Point(pointx1, pointy1);  //Top Right
            shape[1] = new Point(pointx2, pointy2);  //Top Left
            shape[2] = new Point(pointx3, pointy3);  //Left
            shape[3] = new Point(pointx4, pointy4);  //Bottem Left
            shape[4] = new Point(pointx5, pointy5);  //Bottem Right
            shape[5] = new Point(pointx6, pointy6);  //Right
            g.DrawPolygon(p, shape);
            g.FillPolygon(colour, shape);
        }

        private void Draw2(int loca, Brush colour)
        {
            Point[] shape = new Point[6];
            Pen p = new Pen(colour, 10);
            Graphics g = pnlBoard.CreateGraphics();
            shape[0] = topRight[loca]; //Top Right
            shape[1] = topLeft[loca];  //Top Left
            shape[2] = LeftS[loca];  //Left
            shape[3] = bottomLeft[loca];  //Bottem Left
            shape[4] = bottomRight[loca];  //Bottem Right
            shape[5] = RightS[loca];  //Right
            g.DrawPolygon(p, shape);
            g.FillPolygon(colour, shape);
        }

        private void Drawing(bool drawing)
        {
            Graphics g = pnlBoard.CreateGraphics();
            if (drawing == true)
            {
                Draw2(1, Brushes.Cyan); Draw2(2, Brushes.Cyan); Draw2(8, Brushes.Cyan); Draw2(9, Brushes.Cyan);
                Draw2(16, Brushes.Cyan); Draw2(17, Brushes.Cyan); Draw2(23, Brushes.Cyan); Draw2(24, Brushes.Cyan);
                Draw2(31, Brushes.Cyan); Draw2(32, Brushes.Cyan); Draw2(38, Brushes.Cyan); Draw2(39, Brushes.Cyan);
                Draw2(46, Brushes.Cyan); Draw2(47, Brushes.Cyan); Draw2(53, Brushes.Cyan); Draw2(54, Brushes.Cyan);
                Draw2(61, Brushes.Cyan); Draw2(62, Brushes.Cyan); Draw2(68, Brushes.Cyan); Draw2(69, Brushes.Cyan);
                Draw2(76, Brushes.Cyan); Draw2(77, Brushes.Cyan); Draw2(83, Brushes.Cyan); Draw2(84, Brushes.Cyan);
                Draw2(91, Brushes.Cyan); Draw2(92, Brushes.Cyan); Draw2(98, Brushes.Cyan); Draw2(99, Brushes.Cyan);
                Draw2(106, Brushes.Cyan); Draw2(107, Brushes.Cyan); Draw2(113, Brushes.Cyan); Draw2(114, Brushes.Cyan);
                Draw2(121, Brushes.Cyan); Draw2(122, Brushes.Cyan); Draw2(128, Brushes.Cyan); Draw2(129, Brushes.Cyan);
                Draw2(136, Brushes.Cyan); Draw2(137, Brushes.Cyan);
            }
        }

        private void Moving(int i)
        {
            pnlBoard.Refresh();
            Graphics g = pnlBoard.CreateGraphics();
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    moveFor = Convert.ToInt32(reader["Move"]);
                    //Clear old values
                    moveValue.Clear();
                    //Get all available spaces to move to
                    for (int k = 0; k < moveFor + 1; k++)
                    {
                        //If piece is at these locations the user cannot go up
                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                        {
                            moveValue.Add(pieceLocation[i] + (up * 1));
                        }
                        if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                        {
                            moveValue.Add(pieceLocation[i] + (down * 1));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8)
                            {
                                moveValue.Add(pieceLocation[i] + (upLeft * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                moveValue.Add(pieceLocation[i] + (downLeft * 1));
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 15)
                            {
                                moveValue.Add(pieceLocation[i] + (upRight * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                moveValue.Add(pieceLocation[i] + (downRight * 1));
                            }
                        }
                        //If piece can move two spaces or more
                        if (k >= 2 && k < moveFor + 1)
                        {
                            if (pieceLocation[i] != 16 && pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 &&
                            pieceLocation[i] != 20 && pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 &&
                            pieceLocation[i] != 24 && pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 &&
                            pieceLocation[i] != 28 && pieceLocation[i] != 29 && pieceLocation[i] != 30)
                            {
                                moveValue.Add(pieceLocation[i] + (up * 2));
                            }
                            if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127)
                            {
                                moveValue.Add(pieceLocation[i] + (down * 2));
                            }
                            if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                            {
                                moveValue.Add(pieceLocation[i] + (up + upLeft));
                                if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                         pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                         pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                         pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                         pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                         pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    moveValue.Add(pieceLocation[i] + (down + downLeft));
                                }
                                if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                            pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                            pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                            pieceLocation[i] != 136)
                                {
                                    if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 &&
                                        pieceLocation[i] != 12 && pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                    {
                                        moveValue.Add(pieceLocation[i] + (upLeft * 2));
                                    }
                                    if (pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                        pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                         pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                         pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (downLeft * 2));
                                    }
                                    moveValue.Add(pieceLocation[i] + (upLeft + downLeft));
                                }
                            }
                            if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                            {
                                moveValue.Add(pieceLocation[i] + (up + upRight));
                                if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                         pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                         pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                         pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 136 &&
                                         pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                         pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    moveValue.Add(pieceLocation[i] + (down + downRight));
                                }
                                if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                pieceLocation[i] != 142)
                                {
                                    if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14)
                                    {
                                        moveValue.Add(pieceLocation[i] + (upRight * 2));
                                    }
                                    if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                        pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                        pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                        pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (downRight * 2));
                                    }
                                    moveValue.Add(pieceLocation[i] + (upRight + downRight));
                                }
                            }
                            //If piece can move three spaces or more
                            if (k >= 3 && k < moveFor + 1)
                            {
                                if (pieceLocation[i] != 31 && pieceLocation[i] != 32 && pieceLocation[i] != 33 && pieceLocation[i] != 34 &&
                               pieceLocation[i] != 35 && pieceLocation[i] != 36 && pieceLocation[i] != 37 && pieceLocation[i] != 38 &&
                               pieceLocation[i] != 39 && pieceLocation[i] != 40 && pieceLocation[i] != 41 && pieceLocation[i] != 42 &&
                               pieceLocation[i] != 43 && pieceLocation[i] != 44 && pieceLocation[i] != 45)
                                {
                                    moveValue.Add(pieceLocation[i] + (up * 3));
                                }
                                if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                    pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                    pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                    pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112)
                                {
                                    moveValue.Add(pieceLocation[i] + (down * 3));
                                }
                                if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                {
                                    moveValue.Add(pieceLocation[i] + (up + up + upLeft));
                                    if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                                     pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                                     pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                     pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                     pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                     pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                     pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                     pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                     pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (down + down + downLeft));
                                    }
                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                pieceLocation[i] != 136)
                                    {
                                        moveValue.Add(pieceLocation[i] + (upLeft + upLeft + up));
                                        if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                    pieceLocation[i] != 142)
                                        {
                                            moveValue.Add(pieceLocation[i] + (downLeft + downLeft + down));
                                        }
                                        if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                            pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                            pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                        {
                                            moveValue.Add(pieceLocation[i] + (upLeft * 3));
                                            if (pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                    pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (downLeft * 3));
                                            }
                                            moveValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft));
                                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (downLeft + downLeft + upLeft));
                                            }
                                        }
                                    }
                                }
                                if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                {
                                    moveValue.Add(pieceLocation[i] + (up + up + upRight));
                                    if (pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                                    pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                                    pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                    pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                    pieceLocation[i] != 134 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                    pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (down + down + downRight));
                                    }
                                    if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                    pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                    pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                    pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (upRight + upRight + up));
                                        if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                    pieceLocation[i] != 142)
                                        {
                                            moveValue.Add(pieceLocation[i] + (downRight + downRight + down));
                                        }
                                        if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                            pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                            pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                        {
                                            moveValue.Add(pieceLocation[i] + (upRight * 3));
                                            if (pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                    pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (downRight * 3));
                                            }
                                            moveValue.Add(pieceLocation[i] + (upRight + upRight + downRight));
                                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (downRight + downRight + upRight));
                                            }
                                        }
                                    }
                                }
                                //If piece can move four spaces or more
                                if (k >= 4 && k < moveFor + 1)
                                {
                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                        pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                                        pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                                        pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                    {
                                        moveValue.Add(pieceLocation[i] + (up * 4));
                                    }
                                    if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                        pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                        pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 &&
                                        pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 &&
                                        pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 &&
                                        pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 &&
                                        pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 &&
                                        pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 &&
                                        pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 &&
                                        pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 &&
                                        pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 &&
                                        pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                        pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                        pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                        pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        moveValue.Add(pieceLocation[i] + (down * 4));
                                    }
                                    if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                        pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                        pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                    {
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                        pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                        {
                                            moveValue.Add(pieceLocation[i] + (up + up + up + upLeft));
                                        }
                                        if (pieceLocation[i] != 83 && pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 &&
                                            pieceLocation[i] != 94 && pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 &&
                                            pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                            pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                        {
                                            moveValue.Add(pieceLocation[i] + (down + down + down + downLeft));
                                        }
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                    pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                    pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 136)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 45)
                                            {
                                                moveValue.Add(pieceLocation[i] + (up + up + upLeft + upLeft));
                                            }
                                            if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (down + down + downLeft + downLeft));
                                            }
                                            if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                                pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                                pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + up));
                                                }
                                                if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + down));
                                                }
                                                if (pieceLocation[i] != 2 && pieceLocation[i] != 17 && pieceLocation[i] != 32 &&
                                    pieceLocation[i] != 47 && pieceLocation[i] != 62 && pieceLocation[i] != 77 &&
                                    pieceLocation[i] != 92 && pieceLocation[i] != 107 && pieceLocation[i] != 122 &&
                                    pieceLocation[i] != 137)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (upLeft * 4));
                                                    }
                                                    if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                        pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                        pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                                        pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                                        pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                                        pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                                        pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                                        pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (downLeft * 4));
                                                    }
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + downLeft));
                                                    }
                                                    moveValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft + downLeft));
                                                    if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + upLeft));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                    {
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                        pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                        {
                                            moveValue.Add(pieceLocation[i] + (up + up + up + upRight));
                                        }
                                        if (pieceLocation[i] != 90 &&
                                        pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 &&
                                        pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 &&
                                        pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 &&
                                        pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 &&
                                        pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 &&
                                        pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 &&
                                        pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 &&
                                        pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 &&
                                        pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 &&
                                        pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                        pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                        pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                        pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            moveValue.Add(pieceLocation[i] + (down + down + down + downRight));
                                        }
                                        if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                        pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                        pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                        pieceLocation[i] != 142)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 45)
                                            {
                                                moveValue.Add(pieceLocation[i] + (up + up + upRight + upRight));
                                            }
                                            if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (down + down + downRight + downRight));
                                            }
                                            if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                                pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                                pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 37)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (upRight + upRight + upRight + up));
                                                }
                                                if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (downRight + downRight + downRight + down));
                                                }
                                                if (pieceLocation[i] != 6 && pieceLocation[i] != 21 && pieceLocation[i] != 36 &&
                                        pieceLocation[i] != 51 && pieceLocation[i] != 66 && pieceLocation[i] != 81 &&
                                        pieceLocation[i] != 96 && pieceLocation[i] != 111 && pieceLocation[i] != 126 &&
                                        pieceLocation[i] != 141)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                        pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                        pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                        pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                        pieceLocation[i] != 29 && pieceLocation[i] != 30)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (upRight * 4));
                                                    }
                                                    if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                        pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                        pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                                        pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                                        pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                                        pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                                        pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                                        pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (downRight * 4));
                                                    }
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                        pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                        pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                        pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (upRight + upRight + upRight + downRight));
                                                    }
                                                    moveValue.Add(pieceLocation[i] + (upRight + upRight + downRight + downRight));
                                                    if (pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (downRight + downRight + downRight + upRight));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //If piece can move five spaces or more
                                    if (k >= 5 && k < moveFor + 1)
                                    {
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                           pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                           pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                           pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                           pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67 && pieceLocation[i] != 68 &&
                           pieceLocation[i] != 69 && pieceLocation[i] != 70 && pieceLocation[i] != 71 && pieceLocation[i] != 72 &&
                           pieceLocation[i] != 73 && pieceLocation[i] != 74 && pieceLocation[i] != 75)
                                        {
                                            moveValue.Add(pieceLocation[i] + (up * 5));
                                        }
                                        if (pieceLocation[i] != 68 && pieceLocation[i] != 69 && pieceLocation[i] != 70 && pieceLocation[i] != 71 &&
                                            pieceLocation[i] != 72 && pieceLocation[i] != 73 && pieceLocation[i] != 74 && pieceLocation[i] != 75 &&
                                            pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                            pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                            pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                            pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            moveValue.Add(pieceLocation[i] + (down * 5));
                                        }
                                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                        pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                        pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                           pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                           pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                           pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                           pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67)
                                            {
                                                moveValue.Add(pieceLocation[i] + (up + up + up + up + upLeft));
                                            }
                                            if (pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                            pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                            pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                            pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (downLeft + down + down + down + down));
                                            }
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                        pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                        pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 136)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                           pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                           pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (up + up + up + upLeft + upLeft));
                                                }
                                                if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                            pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                            pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (downLeft + downLeft + down + down + down));
                                                }
                                                if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                                    pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                                    pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (up + up + upLeft + upLeft + upLeft));
                                                    }
                                                    if (pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + down + down));
                                                    }
                                                    if (pieceLocation[i] != 2 && pieceLocation[i] != 17 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 47 && pieceLocation[i] != 62 && pieceLocation[i] != 77 &&
                                        pieceLocation[i] != 92 && pieceLocation[i] != 107 && pieceLocation[i] != 122 &&
                                        pieceLocation[i] != 137)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45)
                                                        {
                                                            moveValue.Add(pieceLocation[i] + (up + upLeft + upLeft + upLeft + upLeft));
                                                        }
                                                        if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                            pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                            pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                            pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                            pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            moveValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + downLeft + down));
                                                        }
                                                        if (pieceLocation[i] != 10 && pieceLocation[i] != 25 && pieceLocation[i] != 40 &&
                                    pieceLocation[i] != 55 && pieceLocation[i] != 70 && pieceLocation[i] != 85 &&
                                    pieceLocation[i] != 100 && pieceLocation[i] != 115 && pieceLocation[i] != 130)
                                                        {
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                                                pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                                                pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                                                pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                                                pieceLocation[i] != 37)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upLeft * 5));
                                                            }
                                                            if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                            pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (downLeft * 5));
                                                            }
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                pieceLocation[i] != 21 && pieceLocation[i] != 22)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + upLeft + downLeft));
                                                            }
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + downLeft + downLeft));
                                                            }
                                                            if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upLeft + downLeft + downLeft + downLeft + downLeft));
                                                            }
                                                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                        pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft + downLeft + downLeft));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                    pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                    pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                           pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                           pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                           pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                           pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67)
                                            {
                                                moveValue.Add(pieceLocation[i] + (upRight + up + up + up + up));
                                            }
                                            if (pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                            pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                            pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                            pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                moveValue.Add(pieceLocation[i] + (down + down + down + down + downRight));
                                            }
                                            if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                            pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                            pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 142)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                           pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                           pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (upRight + upRight + up + up + up));
                                                }
                                                if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                            pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                            pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    moveValue.Add(pieceLocation[i] + (down + down + down + downRight + downRight));
                                                }
                                                if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                                    pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                                    pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                           pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (upRight + upRight + upRight + up + up));
                                                    }
                                                    if (pieceLocation[i] != 91 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                            pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                            pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                            pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                            pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        moveValue.Add(pieceLocation[i] + (down + down + downRight + downRight + downRight));
                                                    }
                                                    if (pieceLocation[i] != 6 && pieceLocation[i] != 21 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 51 && pieceLocation[i] != 66 && pieceLocation[i] != 81 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 111 && pieceLocation[i] != 126 &&
                                            pieceLocation[i] != 141)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                           pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                           pieceLocation[i] != 45)
                                                        {
                                                            moveValue.Add(pieceLocation[i] + (upRight + upRight + upRight + upRight + up));
                                                        }
                                                        if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                            pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                            pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                            pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                            pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                            pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                            pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            moveValue.Add(pieceLocation[i] + (down + downRight + downRight + downRight + downRight));
                                                        }
                                                        if (pieceLocation[i] != 13 && pieceLocation[i] != 28 && pieceLocation[i] != 43 &&
                                    pieceLocation[i] != 58 && pieceLocation[i] != 73 && pieceLocation[i] != 88 &&
                                    pieceLocation[i] != 103 && pieceLocation[i] != 118 && pieceLocation[i] != 133)
                                                        {
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                           pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                           pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                           pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                           pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                           pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                           pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                           pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                           pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                           pieceLocation[i] != 37)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (upRight * 5));
                                                            }
                                                            if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                            pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                            pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (downRight * 5));
                                                            }
                                                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (downRight + downRight + downRight + upRight + upRight));
                                                                moveValue.Add(pieceLocation[i] + (downRight + downRight + downRight + downRight + upRight));
                                                            }
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                pieceLocation[i] != 21 && pieceLocation[i] != 22)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (downRight + upRight + upRight + upRight + upRight));
                                                            }
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7)
                                                            {
                                                                moveValue.Add(pieceLocation[i] + (downRight + downRight + upRight + upRight + upRight));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }//If 5 move End
                                }//If 4 move End
                            }//If 3 move End
                        }//If 2 move End
                    }//If 1 move End
                    if (select2[i] == true)
                    {
                        for (int k = 1; k < moveValue.Count; k++)
                        {
                            if (moveValue[k] < 1 || moveValue[k] > 142)
                            {
                                moveValue.Remove(moveValue[k]);
                            }
                            Draw2(moveValue[k], Brushes.Cyan);
                        }
                    }
                    else if (select2[i] == false)
                    {
                        pnlBoard.Invalidate();
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            if (i == 0)
            {

            }
        }

        private void Attacking(int i)
        {
            pnlBoard.Refresh();
            Graphics g = pnlBoard.CreateGraphics();
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                //MessageBox.Show("Connection Successful");
                if (reader.Read())
                {
                    //Clear old values
                    attackValue.Clear();
                    //Get all available spaces to attack
                    if (reader["Type"].ToString() == "Melee")
                    {
                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                        {
                            attackValue.Add(pieceLocation[i] + (up));
                        }
                        if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                        {
                            attackValue.Add(pieceLocation[i] + (down));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8)
                            {
                                attackValue.Add(pieceLocation[i] + (upLeft));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downLeft));
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 15)
                            {
                                attackValue.Add(pieceLocation[i] + (upRight));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downRight));
                            }
                        }
                    }
                    else if (reader["Type"].ToString() == "Magic")
                    {
                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                        {
                            attackValue.Add(pieceLocation[i] + (up * 1));
                        }
                        if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                        {
                            attackValue.Add(pieceLocation[i] + (down * 1));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8)
                            {
                                attackValue.Add(pieceLocation[i] + (upLeft * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downLeft * 1));
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 15)
                            {
                                attackValue.Add(pieceLocation[i] + (upRight * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downRight * 1));
                            }
                        }
                        if (pieceLocation[i] != 16 && pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 &&
                        pieceLocation[i] != 20 && pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 &&
                        pieceLocation[i] != 24 && pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 &&
                        pieceLocation[i] != 28 && pieceLocation[i] != 29 && pieceLocation[i] != 30)
                        {
                            attackValue.Add(pieceLocation[i] + (up * 2));
                        }
                        if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                            pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                            pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127)
                        {
                            attackValue.Add(pieceLocation[i] + (down * 2));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                        pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                        pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            attackValue.Add(pieceLocation[i] + (up + upLeft));
                            if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                     pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                     pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                     pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                     pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                     pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (down + downLeft));
                            }
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                        pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                        pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                        pieceLocation[i] != 136)
                            {
                                if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 &&
                                    pieceLocation[i] != 12 && pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                {
                                    attackValue.Add(pieceLocation[i] + (upLeft * 2));
                                }
                                if (pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                    pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                     pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                     pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downLeft * 2));
                                }
                                attackValue.Add(pieceLocation[i] + (upLeft + downLeft));
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                        pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                        pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            attackValue.Add(pieceLocation[i] + (up + upRight));
                            if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                     pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                     pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                     pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 136 &&
                                     pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                     pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (down + downRight));
                            }
                            if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                            pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                            pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                            pieceLocation[i] != 142)
                            {
                                if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                        pieceLocation[i] != 13 && pieceLocation[i] != 14)
                                {
                                    attackValue.Add(pieceLocation[i] + (upRight * 2));
                                }
                                if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                    pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                    pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                    pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downRight * 2));
                                }
                                attackValue.Add(pieceLocation[i] + (upRight + downRight));
                            }
                        }
                        if (pieceLocation[i] != 31 && pieceLocation[i] != 32 && pieceLocation[i] != 33 && pieceLocation[i] != 34 &&
                       pieceLocation[i] != 35 && pieceLocation[i] != 36 && pieceLocation[i] != 37 && pieceLocation[i] != 38 &&
                       pieceLocation[i] != 39 && pieceLocation[i] != 40 && pieceLocation[i] != 41 && pieceLocation[i] != 42 &&
                       pieceLocation[i] != 43 && pieceLocation[i] != 44 && pieceLocation[i] != 45)
                        {
                            attackValue.Add(pieceLocation[i] + (up * 3));
                        }
                        if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                            pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                            pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                            pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112)
                        {
                            attackValue.Add(pieceLocation[i] + (down * 3));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                    pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                    pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            attackValue.Add(pieceLocation[i] + (up + up + upLeft));
                            if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                             pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                             pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                             pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                             pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                             pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                             pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                             pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                             pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (down + down + downLeft));
                            }
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                        pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                        pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                        pieceLocation[i] != 136)
                            {
                                attackValue.Add(pieceLocation[i] + (upLeft + upLeft + up));
                                if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                            pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                            pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                            pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                            pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                            pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                            pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downLeft + downLeft + down));
                                }
                                if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                    pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                    pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                {
                                    attackValue.Add(pieceLocation[i] + (upLeft * 3));
                                    if (pieceLocation[i] != 121 &&
                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                            pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                            pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                            pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                            pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                            pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (downLeft * 3));
                                    }
                                    attackValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft));
                                    if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                        pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (downLeft + downLeft + upLeft));
                                    }
                                }
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                    pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                    pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            attackValue.Add(pieceLocation[i] + (up + up + upRight));
                            if (pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                            pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                            pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                            pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                            pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                            pieceLocation[i] != 134 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                            pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (down + down + downRight));
                            }
                            if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                            pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                            pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                            pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (upRight + upRight + up));
                                if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                            pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                            pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                            pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                            pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                            pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                            pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downRight + downRight + down));
                                }
                                if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                    pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                    pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                {
                                    attackValue.Add(pieceLocation[i] + (upRight * 3));
                                    if (pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                            pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                            pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                            pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                            pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                            pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                            pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (downRight * 3));
                                    }
                                    attackValue.Add(pieceLocation[i] + (upRight + upRight + downRight));
                                    if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                        pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (downRight + downRight + upRight));
                                    }
                                }
                            }
                        }
                    }
                    else if (reader["Type"].ToString() == "LongRange")
                    {
                        //for (int j = 1; j < 143; j++)
                        //{
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                            {
                                attackValue.Add(pieceLocation[i] + (up * 1));
                            }
                            if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (down * 1));
                            }
                            if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                                pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                                pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                            {
                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8)
                                {
                                    attackValue.Add(pieceLocation[i] + (upLeft * 1));
                                }
                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                               pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downLeft * 1));
                                }
                            }
                            if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                            {
                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 15)
                                {
                                    attackValue.Add(pieceLocation[i] + (upRight * 1));
                                }
                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                               pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                {
                                    attackValue.Add(pieceLocation[i] + (downRight * 1));
                                }
                            }
                                if (pieceLocation[i] != 16 && pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 &&
                                pieceLocation[i] != 20 && pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 &&
                                pieceLocation[i] != 24 && pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 &&
                                pieceLocation[i] != 28 && pieceLocation[i] != 29 && pieceLocation[i] != 30)
                                {
                                    attackValue.Add(pieceLocation[i] + (up * 2));
                                }
                                if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                    pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                    pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                    pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127)
                                {
                                    attackValue.Add(pieceLocation[i] + (down * 2));
                                }
                                if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                                pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                                pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                {
                                    attackValue.Add(pieceLocation[i] + (up + upLeft));
                                    if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                             pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                             pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                             pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                             pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                             pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (down + downLeft));
                                    }
                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                pieceLocation[i] != 136)
                                    {
                                        if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 &&
                                            pieceLocation[i] != 12 && pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                        {
                                            attackValue.Add(pieceLocation[i] + (upLeft * 2));
                                        }
                                        if (pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                            pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                             pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                             pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (downLeft * 2));
                                        }
                                        attackValue.Add(pieceLocation[i] + (upLeft + downLeft));
                                    }
                                }
                                if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                {
                                    attackValue.Add(pieceLocation[i] + (up + upRight));
                                    if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                             pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                             pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                             pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 136 &&
                                             pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                             pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                    {
                                        attackValue.Add(pieceLocation[i] + (down + downRight));
                                    }
                                    if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                    pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                    pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                    pieceLocation[i] != 142)
                                    {
                                        if (pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                pieceLocation[i] != 13 && pieceLocation[i] != 14)
                                        {
                                            attackValue.Add(pieceLocation[i] + (upRight * 2));
                                        }
                                        if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (downRight * 2));
                                        }
                                        attackValue.Add(pieceLocation[i] + (upRight + downRight));
                                    }
                                }
                                    if (pieceLocation[i] != 31 && pieceLocation[i] != 32 && pieceLocation[i] != 33 && pieceLocation[i] != 34 &&
                                   pieceLocation[i] != 35 && pieceLocation[i] != 36 && pieceLocation[i] != 37 && pieceLocation[i] != 38 &&
                                   pieceLocation[i] != 39 && pieceLocation[i] != 40 && pieceLocation[i] != 41 && pieceLocation[i] != 42 &&
                                   pieceLocation[i] != 43 && pieceLocation[i] != 44 && pieceLocation[i] != 45)
                                    {
                                        attackValue.Add(pieceLocation[i] + (up * 3));
                                    }
                                    if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                        pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                        pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                        pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112)
                                    {
                                        attackValue.Add(pieceLocation[i] + (down * 3));
                                    }
                                    if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                                pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                                pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                    {
                                        attackValue.Add(pieceLocation[i] + (up + up + upLeft));
                                        if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                                         pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                                         pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                         pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                         pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                         pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                         pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                         pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                         pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (down + down + downLeft));
                                        }
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                    pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                    pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                    pieceLocation[i] != 136)
                                        {
                                            attackValue.Add(pieceLocation[i] + (upLeft + upLeft + up));
                                            if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                        pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                        pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                        pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                        pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                        pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                                        pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                        pieceLocation[i] != 142)
                                            {
                                                attackValue.Add(pieceLocation[i] + (downLeft + downLeft + down));
                                            }
                                            if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                                pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                                pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                            {
                                                attackValue.Add(pieceLocation[i] + (upLeft * 3));
                                                if (pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                        pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                        pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                        pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 137 &&
                                        pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                        pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (downLeft * 3));
                                                }
                                                attackValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft));
                                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                    pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (downLeft + downLeft + upLeft));
                                                }
                                            }
                                        }
                                    }
                                    if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                    {
                                        attackValue.Add(pieceLocation[i] + (up + up + upRight));
                                        if (pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 &&
                                        pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 &&
                                        pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                        pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                        pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                        pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                        pieceLocation[i] != 134 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                        pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (down + down + downRight));
                                        }
                                        if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                        pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                        pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                        pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (upRight + upRight + up));
                                            if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                        pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                        pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                        pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                        pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                        pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                        pieceLocation[i] != 142)
                                            {
                                                attackValue.Add(pieceLocation[i] + (downRight + downRight + down));
                                            }
                                            if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                                pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                                pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                            {
                                                attackValue.Add(pieceLocation[i] + (upRight * 3));
                                                if (pieceLocation[i] != 119 && pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                        pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                        pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                        pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                        pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                        pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (downRight * 3));
                                                }
                                                attackValue.Add(pieceLocation[i] + (upRight + upRight + downRight));
                                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                    pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (downRight + downRight + upRight));
                                                }
                                            }
                                        }
                                    }
                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                            pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                            pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                            pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                                            pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                                            pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                        {
                                            attackValue.Add(pieceLocation[i] + (up * 4));
                                        }
                                        if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                            pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                            pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 &&
                                            pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 &&
                                            pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 &&
                                            pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 &&
                                            pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 &&
                                            pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 &&
                                            pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 &&
                                            pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 &&
                                            pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 &&
                                            pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                            pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                            pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                            pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                        {
                                            attackValue.Add(pieceLocation[i] + (down * 4));
                                        }
                                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                            pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                            pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                            pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                            {
                                                attackValue.Add(pieceLocation[i] + (up + up + up + upLeft));
                                            }
                                            if (pieceLocation[i] != 83 && pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 &&
                                                pieceLocation[i] != 94 && pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 &&
                                                pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                            {
                                                attackValue.Add(pieceLocation[i] + (down + down + down + downLeft));
                                            }
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                        pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                        pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                        pieceLocation[i] != 136)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                            pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                            pieceLocation[i] != 45)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (up + up + upLeft + upLeft));
                                                }
                                                if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                    pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                    pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                    pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                    pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (down + down + downLeft + downLeft));
                                                }
                                                if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                                    pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                                    pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + up));
                                                    }
                                                    if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                    pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                    pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + down));
                                                    }
                                                    if (pieceLocation[i] != 2 && pieceLocation[i] != 17 && pieceLocation[i] != 32 &&
                                        pieceLocation[i] != 47 && pieceLocation[i] != 62 && pieceLocation[i] != 77 &&
                                        pieceLocation[i] != 92 && pieceLocation[i] != 107 && pieceLocation[i] != 122 &&
                                        pieceLocation[i] != 137)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (upLeft * 4));
                                                        }
                                                        if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                            pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                                            pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                                            pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                                            pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                                            pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                                            pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (downLeft * 4));
                                                        }
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + downLeft));
                                                        }
                                                        attackValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft + downLeft));
                                                        if (pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + upLeft));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                    pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                    pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                        {
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                            pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                            pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                                            pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                            {
                                                attackValue.Add(pieceLocation[i] + (up + up + up + upRight));
                                            }
                                            if (pieceLocation[i] != 90 &&
                                            pieceLocation[i] != 91 && pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 &&
                                            pieceLocation[i] != 95 && pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 &&
                                            pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 &&
                                            pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 &&
                                            pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 &&
                                            pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 &&
                                            pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 &&
                                            pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 &&
                                            pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 &&
                                            pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 &&
                                            pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 &&
                                            pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                                            pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                attackValue.Add(pieceLocation[i] + (down + down + down + downRight));
                                            }
                                            if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                            pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                            pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                            pieceLocation[i] != 142)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                                            pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                                            pieceLocation[i] != 45)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (up + up + upRight + upRight));
                                                }
                                                if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                    pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                    pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                    pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                    pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (down + down + downRight + downRight));
                                                }
                                                if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                                    pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                                    pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 37)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (upRight + upRight + upRight + up));
                                                    }
                                                    if (pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                    pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                    pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 && pieceLocation[i] != 117 &&
                                                    pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 && pieceLocation[i] != 121 &&
                                                    pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 && pieceLocation[i] != 125 &&
                                                    pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (downRight + downRight + downRight + down));
                                                    }
                                                    if (pieceLocation[i] != 6 && pieceLocation[i] != 21 && pieceLocation[i] != 36 &&
                                            pieceLocation[i] != 51 && pieceLocation[i] != 66 && pieceLocation[i] != 81 &&
                                            pieceLocation[i] != 96 && pieceLocation[i] != 111 && pieceLocation[i] != 126 &&
                                            pieceLocation[i] != 141)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                            pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                            pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                            pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                            pieceLocation[i] != 29 && pieceLocation[i] != 30)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (upRight * 4));
                                                        }
                                                        if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                            pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                            pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 && pieceLocation[i] != 124 &&
                                                            pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 &&
                                                            pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 &&
                                                            pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 &&
                                                            pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 &&
                                                            pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (downRight * 4));
                                                        }
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (upRight + upRight + upRight + downRight));
                                                        }
                                                        attackValue.Add(pieceLocation[i] + (upRight + upRight + downRight + downRight));
                                                        if (pieceLocation[i] != 126 && pieceLocation[i] != 127 && pieceLocation[i] != 128 && pieceLocation[i] != 129 &&
                                                    pieceLocation[i] != 130 && pieceLocation[i] != 131 && pieceLocation[i] != 132 && pieceLocation[i] != 133 &&
                                                    pieceLocation[i] != 134 && pieceLocation[i] != 135 && pieceLocation[i] != 136 && pieceLocation[i] != 137 &&
                                                    pieceLocation[i] != 138 && pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 &&
                                                    pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (downRight + downRight + downRight + upRight));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                               pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                               pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                               pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                               pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67 && pieceLocation[i] != 68 &&
                               pieceLocation[i] != 69 && pieceLocation[i] != 70 && pieceLocation[i] != 71 && pieceLocation[i] != 72 &&
                               pieceLocation[i] != 73 && pieceLocation[i] != 74 && pieceLocation[i] != 75)
                                            {
                                                attackValue.Add(pieceLocation[i] + (up * 5));
                                            }
                                            if (pieceLocation[i] != 68 && pieceLocation[i] != 69 && pieceLocation[i] != 70 && pieceLocation[i] != 71 &&
                                                pieceLocation[i] != 72 && pieceLocation[i] != 73 && pieceLocation[i] != 74 && pieceLocation[i] != 75 &&
                                                pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                                pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                                pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                                pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                            {
                                                attackValue.Add(pieceLocation[i] + (down * 5));
                                            }
                                            if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                               pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                               pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                               pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                               pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (up + up + up + up + upLeft));
                                                }
                                                if (pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                                pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                                pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                                pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (downLeft + down + down + down + down));
                                                }
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 16 && pieceLocation[i] != 31 &&
                                            pieceLocation[i] != 46 && pieceLocation[i] != 61 && pieceLocation[i] != 76 &&
                                            pieceLocation[i] != 91 && pieceLocation[i] != 106 && pieceLocation[i] != 121 &&
                                            pieceLocation[i] != 136)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                               pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                               pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (up + up + up + upLeft + upLeft));
                                                    }
                                                    if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                                pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                                pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (downLeft + downLeft + down + down + down));
                                                    }
                                                    if (pieceLocation[i] != 9 && pieceLocation[i] != 24 && pieceLocation[i] != 39 &&
                                        pieceLocation[i] != 54 && pieceLocation[i] != 69 && pieceLocation[i] != 84 &&
                                        pieceLocation[i] != 99 && pieceLocation[i] != 114 && pieceLocation[i] != 129)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (up + up + upLeft + upLeft + upLeft));
                                                        }
                                                        if (pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + down + down));
                                                        }
                                                        if (pieceLocation[i] != 2 && pieceLocation[i] != 17 && pieceLocation[i] != 32 &&
                                            pieceLocation[i] != 47 && pieceLocation[i] != 62 && pieceLocation[i] != 77 &&
                                            pieceLocation[i] != 92 && pieceLocation[i] != 107 && pieceLocation[i] != 122 &&
                                            pieceLocation[i] != 137)
                                                        {
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45)
                                                            {
                                                                attackValue.Add(pieceLocation[i] + (up + upLeft + upLeft + upLeft + upLeft));
                                                            }
                                                            if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                attackValue.Add(pieceLocation[i] + (downLeft + downLeft + downLeft + downLeft + down));
                                                            }
                                                            if (pieceLocation[i] != 10 && pieceLocation[i] != 25 && pieceLocation[i] != 40 &&
                                        pieceLocation[i] != 55 && pieceLocation[i] != 70 && pieceLocation[i] != 85 &&
                                        pieceLocation[i] != 100 && pieceLocation[i] != 115 && pieceLocation[i] != 130)
                                                            {
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                    pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                    pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                    pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                    pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                    pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                                                                    pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                                                                    pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                                                                    pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                                                                    pieceLocation[i] != 37)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upLeft * 5));
                                                                }
                                                                if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (downLeft * 5));
                                                                }
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                    pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                    pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                    pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                    pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                    pieceLocation[i] != 21 && pieceLocation[i] != 22)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + upLeft + downLeft));
                                                                }
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                    pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upLeft + upLeft + upLeft + downLeft + downLeft));
                                                                }
                                                                if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                                    pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                                    pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                                    pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upLeft + downLeft + downLeft + downLeft + downLeft));
                                                                }
                                                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upLeft + upLeft + downLeft + downLeft + downLeft));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                                        pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                                        pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                                            {
                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                               pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                               pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60 &&
                               pieceLocation[i] != 61 && pieceLocation[i] != 62 && pieceLocation[i] != 63 && pieceLocation[i] != 64 &&
                               pieceLocation[i] != 65 && pieceLocation[i] != 66 && pieceLocation[i] != 67)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (upRight + up + up + up + up));
                                                }
                                                if (pieceLocation[i] != 76 && pieceLocation[i] != 77 && pieceLocation[i] != 78 && pieceLocation[i] != 79 &&
                                                pieceLocation[i] != 80 && pieceLocation[i] != 81 && pieceLocation[i] != 82 && pieceLocation[i] != 83 &&
                                                pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 && pieceLocation[i] != 87 &&
                                                pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 && pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                {
                                                    attackValue.Add(pieceLocation[i] + (down + down + down + down + downRight));
                                                }
                                                if (pieceLocation[i] != 7 && pieceLocation[i] != 22 && pieceLocation[i] != 37 &&
                                                pieceLocation[i] != 52 && pieceLocation[i] != 67 && pieceLocation[i] != 82 &&
                                                pieceLocation[i] != 97 && pieceLocation[i] != 112 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 142)
                                                {
                                                    if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52 &&
                               pieceLocation[i] != 53 && pieceLocation[i] != 54 && pieceLocation[i] != 55 && pieceLocation[i] != 56 &&
                               pieceLocation[i] != 57 && pieceLocation[i] != 58 && pieceLocation[i] != 59 && pieceLocation[i] != 60)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (upRight + upRight + up + up + up));
                                                    }
                                                    if (pieceLocation[i] != 83 && pieceLocation[i] != 84 && pieceLocation[i] != 85 && pieceLocation[i] != 86 &&
                                                pieceLocation[i] != 87 && pieceLocation[i] != 88 && pieceLocation[i] != 89 && pieceLocation[i] != 90 &&
                                                pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                    {
                                                        attackValue.Add(pieceLocation[i] + (down + down + down + downRight + downRight));
                                                    }
                                                    if (pieceLocation[i] != 14 && pieceLocation[i] != 29 && pieceLocation[i] != 44 &&
                                        pieceLocation[i] != 59 && pieceLocation[i] != 74 && pieceLocation[i] != 89 &&
                                        pieceLocation[i] != 104 && pieceLocation[i] != 119 && pieceLocation[i] != 134)
                                                    {
                                                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45 && pieceLocation[i] != 46 && pieceLocation[i] != 47 && pieceLocation[i] != 48 &&
                               pieceLocation[i] != 49 && pieceLocation[i] != 50 && pieceLocation[i] != 51 && pieceLocation[i] != 52)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (upRight + upRight + upRight + up + up));
                                                        }
                                                        if (pieceLocation[i] != 91 &&
                                                pieceLocation[i] != 92 && pieceLocation[i] != 93 && pieceLocation[i] != 94 && pieceLocation[i] != 95 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 97 && pieceLocation[i] != 98 && pieceLocation[i] != 99 &&
                                                pieceLocation[i] != 100 && pieceLocation[i] != 101 && pieceLocation[i] != 102 && pieceLocation[i] != 103 &&
                                                pieceLocation[i] != 104 && pieceLocation[i] != 105 && pieceLocation[i] != 106 && pieceLocation[i] != 107 &&
                                                pieceLocation[i] != 108 && pieceLocation[i] != 109 && pieceLocation[i] != 110 && pieceLocation[i] != 111 &&
                                                pieceLocation[i] != 112 && pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                        {
                                                            attackValue.Add(pieceLocation[i] + (down + down + downRight + downRight + downRight));
                                                        }
                                                        if (pieceLocation[i] != 6 && pieceLocation[i] != 21 && pieceLocation[i] != 36 &&
                                                pieceLocation[i] != 51 && pieceLocation[i] != 66 && pieceLocation[i] != 81 &&
                                                pieceLocation[i] != 96 && pieceLocation[i] != 111 && pieceLocation[i] != 126 &&
                                                pieceLocation[i] != 141)
                                                        {
                                                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37 && pieceLocation[i] != 38 && pieceLocation[i] != 39 && pieceLocation[i] != 40 &&
                               pieceLocation[i] != 41 && pieceLocation[i] != 42 && pieceLocation[i] != 43 && pieceLocation[i] != 44 &&
                               pieceLocation[i] != 45)
                                                            {
                                                                attackValue.Add(pieceLocation[i] + (upRight + upRight + upRight + upRight + up));
                                                            }
                                                            if (pieceLocation[i] != 98 && pieceLocation[i] != 99 && pieceLocation[i] != 100 && pieceLocation[i] != 101 &&
                                                pieceLocation[i] != 102 && pieceLocation[i] != 103 && pieceLocation[i] != 104 && pieceLocation[i] != 105 &&
                                                pieceLocation[i] != 106 && pieceLocation[i] != 107 && pieceLocation[i] != 108 && pieceLocation[i] != 109 &&
                                                pieceLocation[i] != 110 && pieceLocation[i] != 111 && pieceLocation[i] != 112 && pieceLocation[i] != 113 &&
                                                pieceLocation[i] != 114 && pieceLocation[i] != 115 &&
                                                pieceLocation[i] != 116 && pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 &&
                                                pieceLocation[i] != 120 && pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                            {
                                                                attackValue.Add(pieceLocation[i] + (down + downRight + downRight + downRight + downRight));
                                                            }
                                                            if (pieceLocation[i] != 13 && pieceLocation[i] != 28 && pieceLocation[i] != 43 &&
                                        pieceLocation[i] != 58 && pieceLocation[i] != 73 && pieceLocation[i] != 88 &&
                                        pieceLocation[i] != 103 && pieceLocation[i] != 118 && pieceLocation[i] != 133)
                                                            {
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                               pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                               pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                               pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                               pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                               pieceLocation[i] != 21 && pieceLocation[i] != 22 && pieceLocation[i] != 23 && pieceLocation[i] != 24 &&
                               pieceLocation[i] != 25 && pieceLocation[i] != 26 && pieceLocation[i] != 27 && pieceLocation[i] != 28 &&
                               pieceLocation[i] != 29 && pieceLocation[i] != 30 && pieceLocation[i] != 31 && pieceLocation[i] != 32 &&
                               pieceLocation[i] != 33 && pieceLocation[i] != 34 && pieceLocation[i] != 35 && pieceLocation[i] != 36 &&
                               pieceLocation[i] != 37)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (upRight * 5));
                                                                }
                                                                if (pieceLocation[i] != 113 && pieceLocation[i] != 114 && pieceLocation[i] != 115 && pieceLocation[i] != 116 &&
                                                pieceLocation[i] != 117 && pieceLocation[i] != 118 && pieceLocation[i] != 119 && pieceLocation[i] != 120 &&
                                                pieceLocation[i] != 121 && pieceLocation[i] != 122 && pieceLocation[i] != 123 &&
                                                pieceLocation[i] != 124 && pieceLocation[i] != 125 && pieceLocation[i] != 126 && pieceLocation[i] != 127 &&
                                                pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                                                pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                                                pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (downRight * 5));
                                                                }
                                                                if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                                                pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (downRight + downRight + downRight + upRight + upRight));
                                                                    attackValue.Add(pieceLocation[i] + (downRight + downRight + downRight + downRight + upRight));
                                                                }
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                    pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                                                                    pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                                                                    pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15 && pieceLocation[i] != 16 &&
                                                                    pieceLocation[i] != 17 && pieceLocation[i] != 18 && pieceLocation[i] != 19 && pieceLocation[i] != 20 &&
                                                                    pieceLocation[i] != 21 && pieceLocation[i] != 22)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (downRight + upRight + upRight + upRight + upRight));
                                                                }
                                                                if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                                                                    pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7)
                                                                {
                                                                    attackValue.Add(pieceLocation[i] + (downRight + downRight + upRight + upRight + upRight));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                        //}//If 1 move End
                    }
                    else if (reader["Type"].ToString() == "Special")
                    {
                        if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8 &&
                            pieceLocation[i] != 9 && pieceLocation[i] != 10 && pieceLocation[i] != 11 && pieceLocation[i] != 12 &&
                            pieceLocation[i] != 13 && pieceLocation[i] != 14 && pieceLocation[i] != 15)
                        {
                            attackValue.Add(pieceLocation[i] + (up * 1));
                        }
                        if (pieceLocation[i] != 128 && pieceLocation[i] != 129 && pieceLocation[i] != 130 && pieceLocation[i] != 131 &&
                            pieceLocation[i] != 132 && pieceLocation[i] != 133 && pieceLocation[i] != 134 && pieceLocation[i] != 135 &&
                            pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 && pieceLocation[i] != 139 &&
                            pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                        {
                            attackValue.Add(pieceLocation[i] + (down * 1));
                        }
                        if (pieceLocation[i] != 8 && pieceLocation[i] != 23 && pieceLocation[i] != 38 &&
                            pieceLocation[i] != 53 && pieceLocation[i] != 68 && pieceLocation[i] != 83 &&
                            pieceLocation[i] != 98 && pieceLocation[i] != 113 && pieceLocation[i] != 128)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 8)
                            {
                                attackValue.Add(pieceLocation[i] + (upLeft * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downLeft * 1));
                            }
                        }
                        if (pieceLocation[i] != 15 && pieceLocation[i] != 30 && pieceLocation[i] != 45 &&
                            pieceLocation[i] != 60 && pieceLocation[i] != 75 && pieceLocation[i] != 90 &&
                            pieceLocation[i] != 105 && pieceLocation[i] != 120 && pieceLocation[i] != 135)
                        {
                            if (pieceLocation[i] != 1 && pieceLocation[i] != 2 && pieceLocation[i] != 3 && pieceLocation[i] != 4 &&
                            pieceLocation[i] != 5 && pieceLocation[i] != 6 && pieceLocation[i] != 7 && pieceLocation[i] != 15)
                            {
                                attackValue.Add(pieceLocation[i] + (upRight * 1));
                            }
                            if (pieceLocation[i] != 136 && pieceLocation[i] != 137 && pieceLocation[i] != 138 &&
                           pieceLocation[i] != 139 && pieceLocation[i] != 140 && pieceLocation[i] != 141 && pieceLocation[i] != 142)
                            {
                                attackValue.Add(pieceLocation[i] + (downRight * 1));
                            }
                        }
                    }
                    if (select2[i] == true)
                    {
                        for (int k = 0; k < attackValue.Count; k++)
                        {
                            if (attackValue[k] < 1 || attackValue[k] > 142)
                            {
                                attackValue.Remove(attackValue[k]);
                            }
                            Draw2(attackValue[k], Brushes.Red);
                        }
                    }
                    else if (select2[i] == false)
                    {
                        pnlBoard.Invalidate();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void extraAttack(int i)
        {
            Graphics g = pnlBoard.CreateGraphics();
            try
            {
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\HexaStones\HexaStones\bin\Debug\Pieces.mdb; 
Persist Security Info=False";
                OleDbConnection connection = new OleDbConnection(ConnectionString);
                connection.Open();
                string selectString = "select * from GameDatabase where ID=" + pieceId4;
                OleDbCommand command = new OleDbCommand(selectString, connection);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["Type"].ToString() == "Melee")
                    {
                        for (int j = 0; j < attackValue.Count; j++)
                        {
                            if (i == attackValue[j])
                            {
                                for (int k = 0; k < 11; k++)
                                {
                                    if (i == pieceLocation[k] + down)
                                    {
                                        extra.Add(attackValue[j] + (upLeft * 1));
                                        extra.Add(attackValue[j] + (upRight * 1));
                                    }
                                    else if (i == pieceLocation[k] + up)
                                    {
                                        extra.Add(attackValue[j] + (downLeft * 1));
                                        extra.Add(attackValue[j] + (downRight * 1));
                                    }
                                    else if (i == pieceLocation[k] + downLeft)
                                    {
                                        extra.Add(attackValue[j] + (up * 1));
                                        extra.Add(attackValue[j] + (downRight * 1));
                                    }
                                    else if (i == pieceLocation[k] + downRight)
                                    {
                                        extra.Add(attackValue[j] + (up * 1));
                                        extra.Add(attackValue[j] + (downLeft * 1));
                                    }
                                    else if (i == pieceLocation[k] + upLeft)
                                    {
                                        extra.Add(attackValue[j] + (down * 1));
                                        extra.Add(attackValue[j] + (upRight * 1));
                                    }
                                    else if (i == pieceLocation[k] + upRight)
                                    {
                                        extra.Add(attackValue[j] + (down * 1));
                                        extra.Add(attackValue[j] + (upLeft * 1));
                                    }
                                }
                            }
                        }
                    }
                    else if (reader["Type"].ToString() == "MAG")
                    {
                        for (int j = 0; j < attackValue.Count; j++)
                        {
                            if (i == attackValue[j])
                            {
                                extra.Add(attackValue[j] + (up * 1));
                                extra.Add(attackValue[j] + (down * 1));
                                extra.Add(attackValue[j] + (upLeft * 1));
                                extra.Add(attackValue[j] + (downLeft * 1));
                                extra.Add(attackValue[j] + (upRight * 1));
                                extra.Add(attackValue[j] + (downRight * 1));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
    }
}
