using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Button : Sprite
{
    public string LevelName;
    public Button(int Px, int Py, TiledObject obj = null) : base("Button.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Button(TiledObject obj = null) : base("Button.png")
    {
        collider.isTrigger = true;
        LevelName = obj.GetStringProperty("nextLevel", "level2.tmx");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ((MyGame)game).LoadLevel(LevelName);
        }
    }
}