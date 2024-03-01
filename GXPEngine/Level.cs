using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;

class Level : GameObject
{
    // ugly singleton:
    //public static Level Instance { get; private set; }

    Player player;
    TiledLoader loader;
    string currentLevelName;


    public float maxX;
    public float maxY;// public for now...
    public Level(string filename)
    {
        currentLevelName = filename;
        loader = new TiledLoader(filename);
        //loader.map.Width * loader.map.TileWidth;

        maxX = loader.map.Width * loader.map.TileWidth;
        maxY = loader.map.Height * loader.map.TileWidth;
        CreateLevel();

        ((MyGame)game).currentLevel = currentLevelName;

        //Instance = this; 
    }

    void CreateLevel(bool includeImageLayers = true)
    {
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.rootObject = this;
        loader.autoInstance = true;
        loader.LoadTileLayers(0); // Ground
        loader.LoadTileLayers(2); // Carpets
        loader.addColliders = true;
        loader.LoadObjectGroups(); // Loads objects
        loader.LoadTileLayers(1); // Walls
        loader.addColliders = false;
        loader.LoadTileLayers(3); // WallDecor
        loader.LoadTileLayers(4); // Cobwebs



        player = FindObjectOfType<Player>();

        ((MyGame)game).timerSeconds = 180;
    }

    void Update()
    {
        if (player != null)
        {
            float playerScreenX = player.x; // Scrolling
            x = -playerScreenX + game.width/2;
            //if (playerScreenX > game.width / 2 + 32)
            //{
            //    float overshoot = playerScreenX - (game.width / 2 + 32);
            //    x -= overshoot / 15;
            //}
            //if (playerScreenX < game.width / 2 - 32)
            //{
            //    float overshoot = -playerScreenX + (game.width / 2 - 32);
            //    x += overshoot / 15;
            //}

            float playerScreenY = player.y; // Scrolling
            y = -playerScreenY + game.height/2;
            //if (playerScreenY > game.height / 2 + 32)
            //{
            //    float overshootY = playerScreenY - (game.height / 2 + 32);
            //    y -= overshootY / 15;
            //}
            //if (playerScreenY < game.height / 2 - 32)
            //{
            //    float overshootY = -playerScreenY + (game.height / 2 - 32);
            //    y += overshootY / 15;
            //}
        }
    }
}
