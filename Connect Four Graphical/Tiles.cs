using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Connect_Four_Graphical
{
    class Tiles
    {
        private int tileHeight;
        private int tileWidth;
        private int formWidth = 800;
        private int formHeight = 600;
        private int xPos;
        private int yPos;
        private int xTiles = Form1.xTiles;
        private int yTiles = Form1.yTiles;
        public Tiles(int _xPos, int _yPos)
        {
            xPos = _xPos;
            yPos = _yPos;
            tileWidth = tileSize(formWidth, xTiles);
            tileHeight = tileSize(formHeight, yTiles);
        }

        public Button initializeEmptyButton(int xFormSize, int yFormSize, int xNumberOfTiles, int yNumberOfTiles, Form1 form)
        {
            Button button = new Button(); //initialising a button
            button.Location = new System.Drawing.Point(xPos, yPos); //sets location
            button.Size = new System.Drawing.Size(tileSize(xFormSize, xNumberOfTiles), tileSize(yFormSize, yNumberOfTiles)); //sets size of one tile based on calculation from tile size method
            button.BackgroundImage = Connect_Four_Graphical.Properties.Resources.emptyTile; //Puts in the image (emptyTile.png at the moment)
            button.BackgroundImageLayout = ImageLayout.Stretch;
            if( Form1.mode == "local")
            {
                button.Click += delegate (Object sender, EventArgs e) { form.button_click_local(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
                
            else if (Form1.mode == "easy")
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_easy(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            else
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_hard(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            
            return button;
        }
        public Button initializeRedButton(int xFormSize, int yFormSize, int xNumberOfTiles, int yNumberOfTiles, Form1 form)
        {
            Button button = new Button(); //initialising a button
            button.Location = new System.Drawing.Point(xPos, yPos); //sets location
            button.Size = new System.Drawing.Size(tileSize(xFormSize, xNumberOfTiles), tileSize(yFormSize, yNumberOfTiles)); //sets size of one tile based on calculation from tile size method
            button.BackgroundImage = Connect_Four_Graphical.Properties.Resources.redTile; //Puts in the image (redTile.png at the moment)
            button.BackgroundImageLayout = ImageLayout.Stretch;
            if (Form1.mode == "local")
            {
                button.Click += delegate (Object sender, EventArgs e) { form.button_click_local(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }            
            else if (Form1.mode == "easy")
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_easy(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            else
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_hard(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            
            return button;
        }
        public Button initializeYellowButton(int xFormSize, int yFormSize, int xNumberOfTiles, int yNumberOfTiles, Form1 form)
        {
            Button button = new Button(); //initialising a button
            button.Location = new System.Drawing.Point(xPos, yPos); //sets location
            button.Size = new System.Drawing.Size(tileSize(xFormSize, xNumberOfTiles), tileSize(yFormSize, yNumberOfTiles)); //sets size of one tile based on calculation from tile size method
            button.BackgroundImage = Connect_Four_Graphical.Properties.Resources.yellowTile; //Puts in the image (yellowTile.png at the moment)
            button.BackgroundImageLayout = ImageLayout.Stretch;
            if (Form1.mode == "local")
            {
                button.Click += delegate (Object sender, EventArgs e) { form.button_click_local(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }            
            else if (Form1.mode == "easy")
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_easy(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            else
            {

                button.Click += delegate (Object sender, EventArgs e) { form.button_click_hard(sender, e, button, xTiles, yTiles, tileWidth, formWidth, formHeight); };
            }
            
            return button;
        }




        public static int tileSize(int formSize, int numberOfTiles) 
        {
            return formSize / numberOfTiles; //total size of form divided by number of tiles (size of one tiles in pixels (in one direction))
        }
    }
}
