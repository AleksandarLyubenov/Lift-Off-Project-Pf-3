using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Gate : Sprite
{
    public string LevelName;
    public Gate(int Px, int Py, TiledObject obj = null) : base("gate.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Gate(TiledObject obj = null) : base("gate.png")
    {
        collider.isTrigger = true;
        LevelName = obj.GetStringProperty("nextLevel", "map.tmx");
    }
}
