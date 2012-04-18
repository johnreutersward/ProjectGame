using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;


namespace ProjectGame
{
    class TileMap
    {
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 100;
        public int MapHeight = 100;
        public Random rand;

        public TileMap()
        {
            rand = new Random();
            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(rand.Next(4)));
                }
                Rows.Add(thisRow);
            }

            // Create Sample Map Data
            //Rows[0].Columns[3].TileID = 2;
            //Rows[0].Columns[4].TileID = 2;
            //Rows[0].Columns[5].TileID = 2;
            //Rows[0].Columns[6].TileID = 2;
            //Rows[0].Columns[7].TileID = 2;

            //Rows[1].Columns[3].TileID = 2;
            //Rows[1].Columns[4].TileID = 1;
            //Rows[1].Columns[5].TileID = 1;
            //Rows[1].Columns[6].TileID = 1;
            //Rows[1].Columns[7].TileID = 1;

            //Rows[2].Columns[2].TileID = 3;
            //Rows[2].Columns[3].TileID = 2;
            //Rows[2].Columns[4].TileID = 1;
            //Rows[2].Columns[5].TileID = 1;
            //Rows[2].Columns[6].TileID = 1;
            //Rows[2].Columns[7].TileID = 1;

            //Rows[3].Columns[2].TileID = 3;
            //Rows[3].Columns[3].TileID = 2;
            //Rows[3].Columns[4].TileID = 1;
            //Rows[3].Columns[5].TileID = 2;
            //Rows[3].Columns[6].TileID = 2;
            //Rows[3].Columns[7].TileID = 2;

            //Rows[4].Columns[2].TileID = 3;
            //Rows[4].Columns[3].TileID = 2;
            //Rows[4].Columns[4].TileID = 1;
            //Rows[4].Columns[5].TileID = 2;
            //Rows[4].Columns[6].TileID = 2;
            //Rows[4].Columns[7].TileID = 2;

            //Rows[5].Columns[2].TileID = 3;
            //Rows[5].Columns[3].TileID = 2;
            //Rows[5].Columns[4].TileID = 1;
            //Rows[5].Columns[5].TileID = 2;
            //Rows[5].Columns[6].TileID = 2;
            //Rows[5].Columns[7].TileID = 2;

            // End Create Sample Map Data


        }
    }
}
