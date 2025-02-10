using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_Four_Graphical
{
    public partial class Form1 : Form
    {
        //550x556 image
        //playerturn % 2 = 0 | player one turn (red)
        //playerturn % 2 = 1 | player two turn (yellow)
        int formWidth = 800;
        int formHeight = 600;
        public static string mode;
        static char[,] board;
        public static int xTiles = 7; //x size of board
        public static int yTiles = 6; //y size of board
        string playerOneName;
        string playerTwoName;
        static bool gameOver = false;
        static int playerTurn = 0;
        static Button[,] buttons;
        private static Timer timer;
        static int ticks = 0;
        public Form1()
        {
            InitializeComponent();
            StartupSequence();
        }
        static void checkBoard(int xTiles, int yTiles) 
        {
            int number = 1;
            for(int row = 0; row < yTiles; row++) 
            {
                for(int column = 0; column < xTiles; column++) 
                {
                    MessageBox.Show(number.ToString() + ". (" + column.ToString() + ", " + row.ToString() + ") = " + board[column, row]);
                    number++;
                }
            
            }
        }
        static char[,] createBoard(int xTiles, int yTiles) //creates empty board
        {
            int number = 1;
            board = new char[xTiles, yTiles];
            for (int row = 0; row < yTiles; row++)
            {
                for (int column = 0; column < xTiles; column++)
                {
                    board[column, row] = ' ';
                    //MessageBox.Show(number.ToString() + ". (" + column.ToString() + ", " + row.ToString() + ") = " + board[column, row]);
                    number++;
                }
            }
            return board;

        }
        static char checkPlayerTurn(int playerTurn)  //'r' or 'y' depending on modulus of playerTurn
        {
            char tile;
            if (playerTurn % 2 == 0)
            {
                tile = 'r';
            }
            else
            {
                tile = 'y';
            }
            return tile;
        }
        static bool AddTileToBoard(int column, int yTiles, int playerTurn) //checks if tile is able to be added to that column (if unable, false is returned and nothing else happens)
        {
            for (int row = yTiles - 1; row >= 0; row--) //for every tile in the column of tiles (starting at 0)
            {
                //MessageBox.Show(board[1, 5].ToString());
                //MessageBox.Show(board[column, row].ToString());
                if (board[column, row] == ' ') //if the tile is empty
                {
                    //MessageBox.Show("column is " + (column + 1).ToString());
                    //MessageBox.Show("row is " + (row + 1).ToString());
                    //board[column, i] /* tile chosen to put counter in */ = tile;
                    return true;
                }
            }
            return false;
        }
        static int CheckRowAI(int column, int yTiles) //checks what row the tile is to be added to and adds tile to board PROBLEM SOMEHOW?
        {
            char tile = checkPlayerTurn(playerTurn); //'r' or 'y' depending on modulus of playerTurn (unnecessary line?)
            for (int y = yTiles - 1; y >= 0; y--) //for every tile in the column of tiles (starting at 0)
            {
                if (board[column, y] == ' ') //if the tile is empty
                {
                    /* tile chosen to put counter in */
                    return y;
                }
            }
            return -1;
        }
        static int CheckRow(int column, int yTiles, int playerTurn) //checks what row the tile is to be added to and adds tile to board
        {
            char tile = checkPlayerTurn(playerTurn); //'r' or 'y' depending on modulus of playerTurn
            for (int y = yTiles - 1; y >= 0; y--) //for every tile in the column of tiles (starting at 0)
            {
                 if (board[column, y] == ' ') //if the tile is empty
                {
                    /* tile chosen to put counter in */
                    board[column, y] = tile;
                    return y;
                }
            }
            return -1;
        }
        static bool checkHorizontal(char[,] board, int xTiles, int yTiles, int playerTurn)
        {
            char tile = checkPlayerTurn(playerTurn);

            for (int y = 0; y < yTiles; y++)        
            {
                for (int x = 0; x <= xTiles - 4; x++) //xTiles = 7 | xTiles - 4 = 3
                {
                    if ((board[x, y] == tile) && (board[x + 1, y] == tile) && (board[x + 2, y] == tile) && (board[x + 3, y] == tile))
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        static bool checkVertical(char[,] board, int xTiles, int yTiles, int playerTurn)
        {
            char tile = checkPlayerTurn(playerTurn);

            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y <= yTiles - 4; y++)
                {
                    if ((board[x, y] == tile) && (board[x, y + 1] == tile) && (board[x, y + 2] == tile) && (board[x, y + 3] == tile))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool checkDiagonalDown(char[,] board, int xTiles, int yTiles, int playerTurn)
        {
            char tile = checkPlayerTurn(playerTurn);

            for (int y = 0; y <= yTiles - 4; y++)
            {
                for (int x = 0; x <= xTiles - 4; x++)
                {
                    if ((board[x, y] == tile) && (board[x + 1, y + 1] == tile) && (board[x + 2, y + 2] == tile) && (board[x + 3, y + 3] == tile))
                    {
                        return true;
                    }
                }
            }
                
            return false;
        }

        static bool checkDiagonalUp(char[,] board, int xTiles, int yTiles, int playerTurn)
        {
            char tile = checkPlayerTurn(playerTurn);

            for (int y = yTiles - 1; y >= 3; y--)
            {
                for (int x = 0; x <= xTiles - 4; x++)
                {
                    if ((board[x, y] == tile) && (board[x + 1, y - 1] == tile) && (board[x + 2, y - 2] == tile) && (board[x + 3, y - 3] == tile))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static bool checkWinner(char[,] board, int xTiles, int yTiles, int playerTurn)
        {
            if (checkHorizontal(board, xTiles, yTiles, playerTurn))
            {
                return true;
            }
            if (checkVertical(board, xTiles, yTiles, playerTurn))
            {
                return true;
            }

            if (checkDiagonalUp(board, xTiles, yTiles, playerTurn))
            {
                return true;
            }

            if (checkDiagonalDown(board, xTiles, yTiles, playerTurn))
            {
                return true;
            }

            return false;
        }
        void drawBoard(int xTiles, int yTiles, int formWidth, int formHeight) 
        {
            Tiles[,] tiles = new Tiles[xTiles, yTiles];
            Button[,] button = new Button[xTiles, yTiles];
            for (int row = 0; row <= yTiles - 1; row++)
            {
                for (int column = 0; column <= xTiles - 1; column++)
                {
                    tiles[column, row] = new Tiles((column * Tiles.tileSize(formWidth, xTiles)), (row * Tiles.tileSize(formHeight, yTiles))); //adds the pictures to the board rown the right place
                    if (board[column, row] == ' ')
                    {
                        button[column, row] = tiles[column, row].initializeEmptyButton(formWidth, formHeight, xTiles, yTiles, this);
                    }
                    else if (board[column, row] == 'r')
                    {
                        button[column, row] = tiles[column, row].initializeRedButton(formWidth, formHeight, xTiles, yTiles, this);
                    }
                    else
                    {
                        button[column, row] = tiles[column, row].initializeYellowButton(formWidth, formHeight, xTiles, yTiles, this);
                    }
                    Controls.Add(button[column, row]);
                    
                }
            }
            buttons = button;
        }

        static void easyAI(int xTiles, int yTiles)
        {
            int randomNumber;
            int possibleColumnsIndex = 0;
            int numberOfAvilableColumns = xTiles;
            for (int x = 0; x < xTiles; x++)
            {
                if (board[x, 0] != ' ')
                {
                    numberOfAvilableColumns--;
                }


            }
            int[] possibleColumns = new int[numberOfAvilableColumns];
            for (int x = 0; x < xTiles; x++)
            {

                if (board[x, 0] != ' ')
                {
                    continue;
                }
                else
                {
                    possibleColumns[possibleColumnsIndex] = x;
                    possibleColumnsIndex++;
                }

            }


            //TESTING START
            //MessageBox.Show("Possible columns are: ");
               //for (int i = 0; i < numberOfAvilableColumns; i++)
               //{
                   //MessageBox.Show(possibleColumns[i].ToString());
               //}
               //MessageBox.Show("(press enter to continue program)");
             
            Random rnd = new Random();
            randomNumber = rnd.Next(0, numberOfAvilableColumns);
            int column = possibleColumns[randomNumber];
        /*
        MessageBox.Show("Chosen column is: " + column.ToString());
        TESTNG END*/

        //Make move
        int row = CheckRow(column, yTiles, playerTurn);
            //MessageBox.Show("AI move is [" + column.ToString() + ", " + row.ToString() + "]");
            if (playerTurn % 2 == 0)
            {
                //Controls.Remove(buttons[column, row]);
                buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile;
                //Controls.Add(buttons[column, row]);
            }
            else
            {
                //Controls.Remove(buttons[column, row]);
                buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile;
                //Controls.Add(buttons[column, row]);
            }

        }
        static void HardAI(int xTiles, int yTiles) 
        {
            int possibleColumnsIndex = 0;
            int numberOfAvilableColumns = xTiles;
            int score = 0;
            int highestScore = 0;
            int row;
            int bestMoveColumn = 3; //PROBLEM?
            //int bestMoveRow = -1;
            for (int x = 0; x < xTiles; x++)
            {
                if (board[x, 0] != ' ')
                {
                    numberOfAvilableColumns--;
                }


            }
            int[] possibleColumns = new int[numberOfAvilableColumns];
            for (int x = 0; x < xTiles; x++) // for loop to make array of free columns
            {

                if (board[x, 0] != ' ') // if the top tile is NOT empty (the column is full) do  nothing
                {
                    continue;
                }
                else // if the column is empty, update the possible colums array
                {
                    possibleColumns[possibleColumnsIndex] = x;
                    possibleColumnsIndex++;
                }
                //MessageBox.Show("possible colums ", possibleColumns[possibleColumnsIndex].ToString()); // DEBUG LINE
            }
            //foreach (int possibleColumn in possibleColumns) { MessageBox.Show("possible column", possibleColumn.ToString()); } // DEBUG LINE
            for (int i = 0; i < possibleColumns.Length; i++) 
            {
                row = CheckRowAI(possibleColumns[i], yTiles);
                score += MinimaxCheckHorizontally(board, xTiles,yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckVertically(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckDiagonallyUp(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckDiagonallyDown(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckHorizontallyD(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckVerticallyD(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckDiagonallyUpD(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                score += MinimaxCheckDiagonallyDownD(board, xTiles, yTiles, playerTurn, possibleColumns[i], row);
                //MessageBox.Show("score for column " + possibleColumns[i].ToString() + " is " + score.ToString()) DEBUG LINE;

                if (score >= highestScore) // >= necessary otherwise it will sometimes choose the default value of middle row, even when the middle row is full because all the columns are bad moves so they all have then same score
                {
                    bestMoveColumn = possibleColumns[i];
                    highestScore = score;
                }
                if (board[3, yTiles-1] == ' ') { bestMoveColumn = 3; } //ensures that at the start, the ai plays in the middle (if the middle column is empty, play there)
                score = 0;
            }

            row = CheckRow(bestMoveColumn, yTiles, playerTurn);
            //MessageBox.Show("AI row = " + row.ToString()); // DEBUG LINE
            //MessageBox.Show("AI column = " + bestMoveColumn.ToString()); // DEBUG LINE
            if (playerTurn % 2 == 0)
            {
                //Controls.Remove(buttons[column, row]);
                buttons[bestMoveColumn, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile;// problem row=-1?  
                //Controls.Add(buttons[column, row]);
            }
            else
            {
                //Controls.Remove(buttons[column, row]);
                buttons[bestMoveColumn, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile;
                //Controls.Add(buttons[column, row]);
            }

        }
        public void button_click_local(Object sender, EventArgs e, Button button, int xTiles, int yTiles, int tileWidth, int formWidth, int formHeight)
        {
            Point buttonLocation = button.Location;

            int column = ColumnClicked(buttonLocation, tileWidth, xTiles);
            //MessageBox.Show("Column clicked was " + column.ToString());
            int row;
            bool columnAvailable;
            if (!gameOver)
            {
                if (column != -1)
                {
                    //MessageBox.Show(buttonLocation.X.ToString() + ", " + buttonLocation.Y.ToString());
                    columnAvailable = AddTileToBoard(column, yTiles, playerTurn);
                    if (columnAvailable)
                    {
                        row = CheckRow(column, yTiles, playerTurn);
                        if (playerTurn % 2 == 0)
                        {
                            //Controls.Remove(buttons[column, row]);
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile;
                            //Controls.Add(buttons[column, row]);
                        }
                        else
                        {
                            //Controls.Remove(buttons[column, row]);
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile;
                            //Controls.Add(buttons[column, row]);

                        }
                        if (checkWinner(board, xTiles, yTiles, playerTurn))
                        {
                            if (playerTurn % 2 == 0) { MessageBox.Show("Red player wins!"); }
                            else { MessageBox.Show("Yellow player wins!"); }

                            GameOverSequence();
                        }
                        playerTurn++;
                        if (playerTurn == (yTiles * xTiles)) 
                        {
                            MessageBox.Show("Draw!");
                            GameOverSequence();
                        }


                    }
                }
            }



        }

        public void button_click_easy(Object sender, EventArgs e, Button button, int xTiles, int yTiles, int tileWidth, int formWidth, int formHeight) 
        {
            Point buttonLocation = button.Location;

            int column = ColumnClicked(buttonLocation, tileWidth, xTiles);
            //MessageBox.Show("Column clicked was " + column.ToString());
            int row;
            bool columnAvailable;
            if (!gameOver)
            {
                if (column != -1)
                {
                    //MessageBox.Show(buttonLocation.X.ToString() + ", " + buttonLocation.Y.ToString());
                    columnAvailable = AddTileToBoard(column, yTiles, playerTurn);
                    if (columnAvailable)
                    {
                        row = CheckRow(column, yTiles, playerTurn);
                        if (playerTurn % 2 == 0)
                        {
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile;
                        }
                        else
                        {
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile;

                        }
                        if (checkWinner(board, xTiles, yTiles, playerTurn))
                        {
                            if (playerTurn % 2 == 0) 
                            { 
                                MessageBox.Show("Red player wins!"); 
                            }
                            else 
                            { 
                                MessageBox.Show("Yellow player wins!"); 
                            }
                            GameOverSequence();
                        }
                        playerTurn++;
                        if (playerTurn == (yTiles * xTiles)) 
                        {
                            MessageBox.Show("Draw!");
                            GameOverSequence();
                        }

                    }

                    //AI turn
                    if (!gameOver)
                    {
                        Pause();
                        easyAI(xTiles, yTiles);
                        if (checkWinner(board, xTiles, yTiles, playerTurn))
                        {
                            if (playerTurn % 2 == 0) { MessageBox.Show("Red player wins!"); }
                            else { MessageBox.Show("Yellow player wins!"); }
                            GameOverSequence();
                        }
                        playerTurn++;
                        if (playerTurn == (yTiles * xTiles)) 
                        {
                            MessageBox.Show("Draw!");
                            GameOverSequence();
                        }
                    }
                }
            }
        }
            
        public  void button_click_hard(Object sender, EventArgs e, Button button, int xTiles, int yTiles, int tileWidth, int formWidth, int formHeight) 
        {
            Point buttonLocation = button.Location;

            int column = ColumnClicked(buttonLocation, tileWidth, xTiles);
            int row;
            bool columnAvailable;


            if (!gameOver)
            {
                if (column != -1)
                {
                    //MessageBox.Show(buttonLocation.X.ToString() + ", " + buttonLocation.Y.ToString());
                    columnAvailable = AddTileToBoard(column, yTiles, playerTurn);
                    if (columnAvailable)
                    {
                        row = CheckRow(column, yTiles, playerTurn);
                        if (playerTurn % 2 == 0)
                        {
                            //Controls.Remove(buttons[column, row]);
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile;
                            //Controls.Add(buttons[column, row]);
                        }
                        else
                        {
                            //Controls.Remove(buttons[column, row]);
                            buttons[column, row].BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile;
                            //Controls.Add(buttons[column, row]);
                        }
                        if (checkWinner(board, xTiles, yTiles, playerTurn))
                        {
                            if (playerTurn % 2 == 0) { MessageBox.Show("Red player wins!"); }
                            else { MessageBox.Show("Yellow player wins!"); }
                            GameOverSequence();
                        }
                        playerTurn++;
                        if (playerTurn == (yTiles * xTiles)) 
                        {
                            MessageBox.Show("Draw!");
                            GameOverSequence();
                        }
                    }
                }
            }

            if (!gameOver)
            {
                Pause();
                HardAI(xTiles, yTiles);
                if (checkWinner(board, xTiles, yTiles, playerTurn))
                {
                    if (playerTurn % 2 == 0) 
                    { 
                        MessageBox.Show("Red player wins!"); 
                    }
                    else 
                    { 
                        MessageBox.Show("Yellow player wins!"); 
                    }
                    GameOverSequence();
                }
                playerTurn++;
                if (playerTurn == (yTiles * xTiles)) 
                {
                    MessageBox.Show("Draw!");
                    GameOverSequence();
                }
            }


        }
        public static int ColumnClicked(Point mouse, int tileWidth, int xTiles)
        {
            for (int i = 0; i < xTiles; i++)
            {
                if ((mouse.X >= (i * tileWidth)) && (mouse.X < (i + 1) * tileWidth))
                {
                    //MessageBox.Show("Column clicked was column " + i.ToString());
                    //MessageBox.Show("Tile width " + tileWidth.ToString());
                    //MessageBox.Show(mouse.X.ToString() + ", " + mouse.Y.ToString());
                    return i;
                }
            }
            //MessageBox.Show("Column clicked was column -1");
            //MessageBox.Show(mouse.X.ToString() + ", " + mouse.Y.ToString());
            return -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void StartupSequence()
        {
            //Form1 form1 = new Form1();
            gameOver = false;
            Controls.Clear();
            InitializeStartupLabel();
            InitializeLocalButton();
            InitializeEasyButton();
            InitializeHardButton();
            timer = InitializeTimer();
            //board size regular, large
            //if (board size == regular) { xTiles = 7, yTiles = 6 }
            //else { xTiles = 10, yTiles = 8 }

        }
        void BoardSizeSequence() 
        {
            InitializeBoardSizeLabel();
            InitializeRegularBoardSizeButton();
            InitializeLargeBoardSizeButton();
        }

        void GameOverSequence()
        {
            gameOver = true;
            playerTurn = 0;
            timer.Stop();
            Controls.Clear();
            InitializeHomeButton();
            InitializeExitButton();
        }

        static int MinimaxCheckHorizontally(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)

        {
            char tile = checkPlayerTurn(playerTurn);
            char opponentTile = checkPlayerTurn(playerTurn + 1);
            int numberOfTiles = 0;
            int numberOfTilesPositive = 0;
            int numberOfTilesNegative = 0;
            int possibleTilesPositive = 0;
            int possibleTilesNegative = 0;
            int possibleTiles = 0;
            //account for both directions
            int positiveEdge;
            int negativeEdge;
            bool positiveEdgeBroken = false;
            bool negativeEdgeBroken = false;

            //Right side of the tile START
            if ((column + 4) > xTiles) { positiveEdge = xTiles; }
            else { positiveEdge = (column + 4); }

            for (int x = (column + 1); x < positiveEdge; x++)
            {
                if (board[x, row] == tile) 
                { 
                    numberOfTilesPositive++; 
                    possibleTilesPositive++; 
                }
                else if (board[x, row] == opponentTile)
                {
                    positiveEdgeBroken = true;
                    break;
                }
                else 
                {
                    possibleTilesPositive++;
                }

            }
            //Right side of the tile END


            //Left side of the tile START
            if ((column - 4) < 0) { negativeEdge = 0; }
            else { negativeEdge = column - 4; }

            for (int x = (column - 1); x >= negativeEdge; x--) 
            {
                if(board[x, row] == tile) 
                { 
                    numberOfTilesNegative++;
                    possibleTilesNegative++;
                }
                else if(board[x, row] == opponentTile) 
                {
                    negativeEdgeBroken = true;
                    break;
                }
                else 
                {
                    possibleTilesNegative++;
                }
            }
            //Left side of the tile END
            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            possibleTiles = possibleTilesPositive + possibleTilesNegative;
            if(possibleTiles < 3) 
            {
                numberOfTiles = 0;
            }

            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }
            //MessageBox.Show("number of Tiles connected Horizontally: " + numberOfTiles.ToString());
            if(numberOfTiles == 0) 
            {
                numberOfTiles = numberOfTiles * 10;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 13;
            }
            return numberOfTiles;
        }
        static int MinimaxCheckDiagonallyUp(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)
        {
            char tile = checkPlayerTurn(playerTurn);
            char opponentTiles = checkPlayerTurn(playerTurn + 1);
            int numberOfPossibleTiles = 0;
            int numberOfTiles;


            int positiveEdgeX;
            int positiveEdgeY;
            int smallestPositiveEdge = 100;
            bool positiveEdgeXCut = false;
            int positiveEdgeXCutSize = -1;
            bool positiveEdgeYCut = false;
            int positiveEdgeYCutSize = -1;
            int numberOfTilesPositive = 0;
            bool positiveEdgeBroken = false;

            int negativeEdgeX;
            int negativeEdgeY;
            int smallestNegativeEdge = 100;
            bool negativeEdgeXCut = false;
            int negativeEdgeXCutSize = -1;
            bool negativeEdgeYCut = false;
            int negativeEdgeYCutSize = -1;
            int numberOfTilesNegative = 0;
            bool negativeEdgeBroken = false;

            if ((column + 4) > xTiles) 
            {
                positiveEdgeX = xTiles;
                positiveEdgeXCut = true;
                positiveEdgeXCutSize = (xTiles - column);
                if (positiveEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestPositiveEdge = positiveEdgeXCutSize;
                }
            }
            else 
            {
                positiveEdgeX = (column + 4);
                if (4 < smallestNegativeEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            if ((row - 4) < 0)
            {
                positiveEdgeY = 0;
                positiveEdgeYCut = true;
                positiveEdgeYCutSize = row;
                if (positiveEdgeYCutSize < smallestPositiveEdge)
                {
                    smallestPositiveEdge = positiveEdgeYCutSize;
                }
            }
            else 
            {
                positiveEdgeY = row - 4;
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            for(int i = 1; i < smallestPositiveEdge; i++) 
            {
                //MessageBox.Show("column + i = " + (column + i).ToString());
                //MessageBox.Show("row - i = " + (row - i).ToString());
                if (board[(column + i), (row - i)] == tile) 
                {
                    numberOfTilesPositive++;
                    numberOfPossibleTiles++;
                }
                else if(board[(column + i), (row - i)] == ' ') 
                {
                    numberOfPossibleTiles++;
                }
                else 
                {
                    positiveEdgeBroken = true;
                    break;
                }

            }

            if((column - 4) < 0) 
            {
                negativeEdgeX = 0;
                negativeEdgeXCut = true;
                negativeEdgeXCutSize = column;
                if (negativeEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeXCutSize;
                }
            }
            else 
            {
                negativeEdgeX = column - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            if ((row + 4) >= yTiles) 
            {
                negativeEdgeY = yTiles - 1;
                negativeEdgeYCut = true;
                negativeEdgeYCutSize = yTiles - row;
                if (negativeEdgeYCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeYCutSize;
                }
            }
            else 
            {
                negativeEdgeY = row + 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            for (int i = 1; i < smallestNegativeEdge; i++) 
            {
                if(board[(column - i), (row + i)] == tile) 
                {
                    numberOfTilesNegative++;
                    numberOfPossibleTiles++;
                }
                else if(board[(column - i), (row + i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else 
                {
                    negativeEdgeBroken = true;
                    break;
                }
            }

            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            if(numberOfPossibleTiles < 3) 
            {
                numberOfTiles = 0;
            }
            if(numberOfTiles >= 3) 
            {
                numberOfTiles = 10000;
            }

            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 10;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 13;
            }
            //MessageBox.Show("number of Tiles connected diagonally up: " + numberOfTiles.ToString());
            return numberOfTiles;
        }

        static int MinimaxCheckDiagonallyDown(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)
        {
            char tile = checkPlayerTurn(playerTurn);
            char opponentTiles = checkPlayerTurn(playerTurn + 1);
            int numberOfPossibleTiles = 0;
            int numberOfTiles;


            int positiveEdgeX;
            int positiveEdgeY;
            int smallestPositiveEdge = 100;
            bool positiveEdgeXCut = false;
            int positiveEdgeXCutSize = -1;
            bool positiveEdgeYCut = false;
            int positiveEdgeYCutSize = -1;
            int numberOfTilesPositive = 0;
            bool positiveEdgeBroken = false;

            int negativeEdgeX;
            int negativeEdgeY;
            int smallestNegativeEdge = 100;
            bool negativeEdgeXCut = false;
            int negativeEdgeXCutSize = -1;
            bool negativeEdgeYCut = false;
            int negativeEdgeYCutSize = -1;
            int numberOfTilesNegative = 0;
            bool negativeEdgeBroken = false;

            if ((column + 4) > xTiles)
            {
                positiveEdgeX = xTiles;
                positiveEdgeXCut = true;
                positiveEdgeXCutSize = (xTiles - column);
                if (positiveEdgeXCutSize < smallestPositiveEdge) 
                {
                    smallestPositiveEdge = positiveEdgeXCutSize;
                }
            }
            else
            {
                positiveEdgeX = (column + 4);
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            if ((row + 4) >= yTiles)
            {
                positiveEdgeY = yTiles - 1;
                positiveEdgeYCut = true;
                positiveEdgeYCutSize = yTiles - row;
                if (positiveEdgeYCutSize < smallestPositiveEdge)
                {
                    smallestPositiveEdge = positiveEdgeYCutSize;
                }
            }
            else
            {
                positiveEdgeY = row + 4;
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }
            
            for (int i = 1; i < smallestPositiveEdge; i++)
            {
                if (board[(column + i), (row + i)] == tile)
                {
                    numberOfTilesPositive++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column + i), (row + i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    positiveEdgeBroken = true;
                    break;
                }

            }

            if ((column - 4) < 0)
            {
                negativeEdgeX = 0;
                negativeEdgeXCut = true;
                negativeEdgeXCutSize = column;
                if (negativeEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeXCutSize;
                }
            }
            else
            {
                negativeEdgeX = column - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            if ((row - 4) < 0)
            {
                negativeEdgeY = 0;
                negativeEdgeYCut = true;
                negativeEdgeYCutSize = row;
                if (negativeEdgeYCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeYCutSize;
                }
            }
            else
            {
                negativeEdgeY = row - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            for (int i = 1; i <= smallestNegativeEdge; i++)
            {
                if (board[(column - i), (row - i)] == tile)
                {
                    numberOfTilesNegative++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column - i), (row - i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    negativeEdgeBroken = true;
                    break;
                }
            }

            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            if (numberOfPossibleTiles < 4)
            {
                numberOfTiles = 0;
            }
            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }
            // MessageBox.Show("number of Tiles connected diagonally down: " + numberOfTiles.ToString());
            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 10;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 13;
            }
            return numberOfTiles;
        }
        static int MinimaxCheckVertically(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)

        {
            char tile = checkPlayerTurn(playerTurn);
            char opponentTile = checkPlayerTurn(playerTurn + 1);
            int numberOfTiles = 0;
            //account for both directions
            int edge;

            //Right side of the tile START
            if ((row + 4) >= yTiles) { edge = yTiles - 1; }
            else { edge = (row + 4); }

            for (int y = row + 1; y <= edge; y++)
            {
                if (board[column, y] == tile)
                {
                    numberOfTiles++;
                }
                else if (board[column, y] == opponentTile)
                {
                    break;
                }

            }
            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }

            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 10;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 13;
            }

            // MessageBox.Show("number of Tiles connected vertically: " + numberOfTiles.ToString());
            return numberOfTiles;
        }


        static int MinimaxCheckHorizontallyD(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)

        {
            char tile = checkPlayerTurn(playerTurn + 1);
            char opponentTile = checkPlayerTurn(playerTurn);
            int numberOfTiles = 0;
            int numberOfTilesPositive = 0;
            int numberOfTilesNegative = 0;
            int possibleTilesPositive = 0;
            int possibleTilesNegative = 0;
            int possibleTiles = 0;
            //account for both directions
            int positiveEdge;
            int negativeEdge;
            bool positiveEdgeBroken = false;
            bool negativeEdgeBroken = false;

            //Right side of the tile START
            if ((column + 4) > xTiles) { positiveEdge = xTiles; }
            else { positiveEdge = (column + 4); }

            for (int x = (column + 1); x < positiveEdge; x++)
            {
                if (board[x, row] == tile)
                {
                    numberOfTilesPositive++;
                    possibleTilesPositive++;
                }
                else if (board[x, row] == opponentTile)
                {
                    positiveEdgeBroken = true;
                    break;
                }
                else
                {
                    possibleTilesPositive++;
                }

            }
            //Right side of the tile END


            //Left side of the tile START
            if ((column - 4) < 0) { negativeEdge = 0; }
            else { negativeEdge = column - 4; }

            for (int x = (column - 1); x >= negativeEdge; x--)
            {
                if (board[x, row] == tile)
                {
                    numberOfTilesNegative++;
                    possibleTilesNegative++;
                }
                else if (board[x, row] == opponentTile)
                {
                    negativeEdgeBroken = true;
                    break;
                }
                else
                {
                    possibleTilesNegative++;
                }
            }
            //Left side of the tile END
            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            possibleTiles = possibleTilesPositive + possibleTilesNegative;
            if (possibleTiles < 3)
            {
                numberOfTiles = 0;
            }

            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }
            //MessageBox.Show("number of Tiles connected Horizontally: " + numberOfTiles.ToString());
            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 8;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 9;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            return numberOfTiles;
        }
        static int MinimaxCheckDiagonallyUpD(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)
        {
            char tile = checkPlayerTurn(playerTurn + 1);
            char opponentTiles = checkPlayerTurn(playerTurn);
            int numberOfPossibleTiles = 0;
            int numberOfTiles;


            int positiveEdgeX;
            int positiveEdgeY;
            int smallestPositiveEdge = 100;
            bool positiveEdgeXCut = false;
            int positiveEdgeXCutSize = -1;
            bool positiveEdgeYCut = false;
            int positiveEdgeYCutSize = -1;
            int numberOfTilesPositive = 0;
            bool positiveEdgeBroken = false;

            int negativeEdgeX;
            int negativeEdgeY;
            int smallestNegativeEdge = 100;
            bool negativeEdgeXCut = false;
            int negativeEdgeXCutSize = -1;
            bool negativeEdgeYCut = false;
            int negativeEdgeYCutSize = -1;
            int numberOfTilesNegative = 0;
            bool negativeEdgeBroken = false;

            if ((column + 4) > xTiles)
            {
                positiveEdgeX = xTiles;
                positiveEdgeXCut = true;
                positiveEdgeXCutSize = (xTiles - column);
                if (positiveEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestPositiveEdge = positiveEdgeXCutSize;
                }
            }
            else
            {
                positiveEdgeX = (column + 4);
                if (4 < smallestNegativeEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            if ((row - 4) < 0)
            {
                positiveEdgeY = 0;
                positiveEdgeYCut = true;
                positiveEdgeYCutSize = row;
                if (positiveEdgeYCutSize < smallestPositiveEdge)
                {
                    smallestPositiveEdge = positiveEdgeYCutSize;
                }
            }
            else
            {
                positiveEdgeY = row - 4;
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            for (int i = 1; i < smallestPositiveEdge; i++)
            {
                //MessageBox.Show("column + i = " + (column + i).ToString());
                //MessageBox.Show("row - i = " + (row - i).ToString());
                if (board[(column + i), (row - i)] == tile)
                {
                    numberOfTilesPositive++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column + i), (row - i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    positiveEdgeBroken = true;
                    break;
                }

            }

            if ((column - 4) < 0)
            {
                negativeEdgeX = 0;
                negativeEdgeXCut = true;
                negativeEdgeXCutSize = column;
                if (negativeEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeXCutSize;
                }
            }
            else
            {
                negativeEdgeX = column - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            if ((row + 4) >= yTiles)
            {
                negativeEdgeY = yTiles - 1;
                negativeEdgeYCut = true;
                negativeEdgeYCutSize = yTiles - row;
                if (negativeEdgeYCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeYCutSize;
                }
            }
            else
            {
                negativeEdgeY = row + 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            for (int i = 1; i < smallestNegativeEdge; i++)
            {
                if (board[(column - i), (row + i)] == tile)
                {
                    numberOfTilesNegative++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column - i), (row + i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    negativeEdgeBroken = true;
                    break;
                }
            }

            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            if (numberOfPossibleTiles < 3)
            {
                numberOfTiles = 0;
            }
            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }

            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 8;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 9;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            //MessageBox.Show("number of Tiles connected diagonally up: " + numberOfTiles.ToString());
            return numberOfTiles;
        }

        static int MinimaxCheckDiagonallyDownD(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)
        {
            char tile = checkPlayerTurn(playerTurn + 1);
            char opponentTiles = checkPlayerTurn(playerTurn);
            int numberOfPossibleTiles = 0;
            int numberOfTiles;


            int positiveEdgeX;
            int positiveEdgeY;
            int smallestPositiveEdge = 100;
            bool positiveEdgeXCut = false;
            int positiveEdgeXCutSize = -1;
            bool positiveEdgeYCut = false;
            int positiveEdgeYCutSize = -1;
            int numberOfTilesPositive = 0;
            bool positiveEdgeBroken = false;

            int negativeEdgeX;
            int negativeEdgeY;
            int smallestNegativeEdge = 100;
            bool negativeEdgeXCut = false;
            int negativeEdgeXCutSize = -1;
            bool negativeEdgeYCut = false;
            int negativeEdgeYCutSize = -1;
            int numberOfTilesNegative = 0;
            bool negativeEdgeBroken = false;

            if ((column + 4) > xTiles)
            {
                positiveEdgeX = xTiles;
                positiveEdgeXCut = true;
                positiveEdgeXCutSize = (xTiles - column);
                if (positiveEdgeXCutSize < smallestPositiveEdge)
                {
                    smallestPositiveEdge = positiveEdgeXCutSize;
                }
            }
            else
            {
                positiveEdgeX = (column + 4);
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            if ((row + 4) >= yTiles)
            {
                positiveEdgeY = yTiles - 1;
                positiveEdgeYCut = true;
                positiveEdgeYCutSize = yTiles - row;
                if (positiveEdgeYCutSize < smallestPositiveEdge)
                {
                    smallestPositiveEdge = positiveEdgeYCutSize;
                }
            }
            else
            {
                positiveEdgeY = row + 4;
                if (4 < smallestPositiveEdge)
                {
                    smallestPositiveEdge = 4;
                }
            }

            for (int i = 1; i < smallestPositiveEdge; i++)
            {
                if (board[(column + i), (row + i)] == tile)
                {
                    numberOfTilesPositive++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column + i), (row + i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    positiveEdgeBroken = true;
                    break;
                }

            }

            if ((column - 4) < 0)
            {
                negativeEdgeX = 0;
                negativeEdgeXCut = true;
                negativeEdgeXCutSize = column;
                if (negativeEdgeXCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeXCutSize;
                }
            }
            else
            {
                negativeEdgeX = column - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            if ((row - 4) < 0)
            {
                negativeEdgeY = 0;
                negativeEdgeYCut = true;
                negativeEdgeYCutSize = row;
                if (negativeEdgeYCutSize < smallestNegativeEdge)
                {
                    smallestNegativeEdge = negativeEdgeYCutSize;
                }
            }
            else
            {
                negativeEdgeY = row - 4;
                if (4 < smallestNegativeEdge)
                {
                    smallestNegativeEdge = 4;
                }
            }

            for (int i = 1; i <= smallestNegativeEdge; i++)
            {
                if (board[(column - i), (row - i)] == tile)
                {
                    numberOfTilesNegative++;
                    numberOfPossibleTiles++;
                }
                else if (board[(column - i), (row - i)] == ' ')
                {
                    numberOfPossibleTiles++;
                }
                else
                {
                    negativeEdgeBroken = true;
                    break;
                }
            }

            numberOfTiles = numberOfTilesPositive + numberOfTilesNegative;
            if (numberOfPossibleTiles < 4)
            {
                numberOfTiles = 0;
            }
            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }
            // MessageBox.Show("number of Tiles connected diagonally down: " + numberOfTiles.ToString());
            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 8;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 9;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 11;
            }
            return numberOfTiles;
        }
        static int MinimaxCheckVerticallyD(char[,] board, int xTiles, int yTiles, int playerTurn, int column, int row)

        {
            char tile = checkPlayerTurn(playerTurn + 1);
            char opponentTile = checkPlayerTurn(playerTurn);
            int numberOfTiles = 0;
            //account for both directions
            int edge;

            //Right side of the tile START
            if ((row + 4) >= yTiles) { edge = yTiles - 1; }
            else { edge = (row + 4); }

            for (int y = row + 1; y <= edge; y++)
            {
                if (board[column, y] == tile)
                {
                    numberOfTiles++;
                }
                else if (board[column, y] == opponentTile)
                {
                    break;
                }

            }
            if (numberOfTiles >= 3)
            {
                numberOfTiles = 10000;
            }

            if (numberOfTiles == 0)
            {
                numberOfTiles = numberOfTiles * 8;
            }
            else if (numberOfTiles == 1)
            {
                numberOfTiles = numberOfTiles * 9;
            }
            else if (numberOfTiles == 2)
            {
                numberOfTiles = numberOfTiles * 11;
            }

            // MessageBox.Show("number of Tiles connected vertically: " + numberOfTiles.ToString());
            return numberOfTiles;
        }


        void InitializeHomeButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Location = new System.Drawing.Point(200, 500); //sets location
            button.Size = new System.Drawing.Size(400, 200); //sets size
            button.Text = "Home";
            button.Click += delegate (Object sender, EventArgs e) { InitializeHomeButtonClick(sender, e); };
            Controls.Add(button);
        }

        void InitializeExitButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Location = new System.Drawing.Point(1200, 500); //sets location
            button.Size = new System.Drawing.Size(400, 200); //sets size
            button.Text = "Exit";
            button.Click += delegate (Object sender, EventArgs e) { InitializeExitButtonClick(sender, e); };
            Controls.Add(button);
        }
        void InitializeStartupLabel()
        {
            Label label = new Label();
            label.Font = new Font(label.Font.FontFamily, 20);
            label.Location = new System.Drawing.Point(750, 100); //sets location in the center (horizontally)
            label.Size = new System.Drawing.Size(700, 150); //sets size
            label.Text = "Welcome to Connect Four! Select mode: ";
            
            Controls.Add(label);
        }
        void InitializeLocalButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Size = new System.Drawing.Size(200, 100); //sets size
            button.Location = new System.Drawing.Point(200, 500); //sets location
            button.Text = "Local";
            button.Click += delegate (Object sender, EventArgs e) { InitializeLocalButtonClick(sender, e); };
            Controls.Add(button);
        }
        void InitializeEasyButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Size = new System.Drawing.Size(200, 100); //sets size
            button.Location = new System.Drawing.Point(900, 500); //sets location
            button.Text = "Easy";
            button.Click += delegate (Object sender, EventArgs e) { InitializeEasyButtonClick(sender, e); };
            Controls.Add(button);   
        }
        void InitializeHardButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Size = new System.Drawing.Size(200, 100); //sets size
            button.Location = new System.Drawing.Point(1600, 500); //sets location
            button.Text = "Hard";
            button.Click += delegate (Object sender, EventArgs e) { InitializeHardButtonClick(sender, e); };
            Controls.Add(button);
        }
        void InitializeBoardSizeLabel()
        {
            Label label = new Label();
            label.Font = new Font(label.Font.FontFamily, 20);
            label.Location = new System.Drawing.Point((this.Width / 2) - (label.Width / 2), 100); //sets location in the center (horizontally)
            label.Size = new System.Drawing.Size(700, 150); //sets size
            label.Text = "Select board size: ";

            Controls.Add(label);
        }

        void InitializeRegularBoardSizeButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Location = new System.Drawing.Point((this.Width / 3) - (button.Width / 2), 600); //sets location
            button.Size = new System.Drawing.Size(200, 100); //sets size
            button.Text = "Regular";
            button.Click += delegate (Object sender, EventArgs e) { InitializeRegularBoardSizeButtonClick(sender, e); };
            Controls.Add(button);
        }

        void InitializeLargeBoardSizeButton()
        {
            Button button = new Button();
            button.Font = new Font(button.Font.FontFamily, 16);
            button.Location = new System.Drawing.Point(((this.Width / 3) * 2) - (button.Width / 2), 600); //sets location
            button.Size = new System.Drawing.Size(200, 100); //sets size
            button.Text = "Large";
            button.Click += delegate (Object sender, EventArgs e) { InitializeLargeBoardSizeButtonClick(sender, e); };
            Controls.Add(button);
        }
        void InitializeRegularBoardSizeButtonClick(object sender, EventArgs e)//game start
        {
            xTiles = 7;
            yTiles = 6;
            Controls.Clear();
            board = createBoard(xTiles, yTiles);
            drawBoard(xTiles, yTiles, formWidth, formHeight);
            //pause 
            timer.Start();
            if(mode == "hard") 
            {
                Pause();
                HardAI(xTiles, yTiles);
                playerTurn++;
            }

        }
        void InitializeLargeBoardSizeButtonClick(object sender, EventArgs e)//game start
        {
            xTiles = 9;
            yTiles = 7;
            Controls.Clear();
            board = createBoard(xTiles, yTiles);
            drawBoard(xTiles, yTiles, formWidth, formHeight);
            timer.Start();
            if (mode == "hard")
            {
                Pause();
                HardAI(xTiles, yTiles); 
                playerTurn++;
            }
        }
        void InitializeLocalButtonClick(object sender, EventArgs e) 
        {
            mode = "local";
            Controls.Clear();
            //PlayerOne Enter your name
            //playerOneName = response;
            //playerTwo enter your name
            //playerTwoName = response;
            //Controls.Clear();
            BoardSizeSequence();
        }

        void InitializeEasyButtonClick(object sender, EventArgs e) 
        {
            mode = "easy";
            Controls.Clear();
            //Enter your name
            //playerOneName = response;
            //Controls.Clear();
            BoardSizeSequence();
        }

        void InitializeHardButtonClick(object sender, EventArgs e)
        {
            mode = "hard";
            Controls.Clear();
            //Enter your name
            //playerOneName = response;
            BoardSizeSequence();
        }


        void InitializeHomeButtonClick(object sender, EventArgs e) 
        {
            StartupSequence();
        }


        void InitializeExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
            //Environment.Exit(0);
        }
        public Timer InitializeTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Enabled = false;
            timer.Tick += delegate (object sender, EventArgs e) { TimerTick(sender, e); };
            //timer.
            return timer;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            ticks++;
            this.Text = (ticks / 10).ToString();
        }
        
        public static async void Pause() 
        {
            Task.Delay(500).Wait();
        }

    }

}
