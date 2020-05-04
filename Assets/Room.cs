using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ballGame
{
    public class Room : MonoBehaviour
    {
        public class TileSingle : MonoBehaviour
        {
            int x;
            int y;
            bool notEmpty;

            public TileSingle(int X, int Y, bool n)
            {
                x = X; y = Y; notEmpty = n;
            }
        }


        public bool isRightGoal, isLeftGoal;
        public bool scored;
        public bool[] floorNotEmpty = new bool[56];

        public BoxCollider2D rightEntry, leftEntry;
        MapManager mapManager; 
        GameManagerNew gameManager;

        Tilemap tilemap;
        BoundsInt bounds;
        TileBase[] allTiles;
        public List<TileSingle> tileList = new List<TileSingle>();
        public bool[,] tileArr = new bool[56,36];



        

        
        // Start is called before the first frame update
        void Start()
        {
            mapManager = GameObject.FindWithTag("Map Manager").GetComponent<MapManager>();
            gameManager = GameObject.FindWithTag("Game Manager").GetComponent<GameManagerNew>();
            tilemap = GetComponent<Tilemap>();
            
            

            GetTiles(tileList);
        }

        void Update()
        {

        }

        public void TeamScore()
        {
            scored = false;
            if(isRightGoal && !isLeftGoal)
                gameManager.RightScore();
                
            else if(!isRightGoal && isLeftGoal)
                gameManager.LeftScore();

            
        }

        void GetTiles(List<TileSingle> tileList)
        {
            tilemap.CompressBounds();
            bounds = tilemap.cellBounds;
            allTiles = tilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++) 
            {
                for (int y = 0; y < bounds.size.y; y++) 
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null) 
                    {
                        //tileList.Add(new TileSingle(x, y, true));
                        Debug.Log(this.name +"x:" + x + " y:" + y + " tile:" + tile.name);
                        tileArr[x,y] = true;
                    }
                    else
                    { 
                        //tileList.Add(new TileSingle(x, y, false));
                        Debug.Log(this.name + "x:" + x + " y:" + y + " tile: (null)");
                        tileArr[x,y] = false;
                    }
                }
            }        
        }
        public void SwitchRight()
        {
            mapManager.SwitchRight();
        }
        public void SwitchLeft()
        {
            mapManager.SwitchLeft();
        }
    }
}
