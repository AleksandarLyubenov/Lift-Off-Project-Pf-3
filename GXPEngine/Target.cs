using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Target : Sprite
{
    public Target(int Px, int Py, TiledObject obj = null) : base("icon_points.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Target(TiledObject obj = null) : base("icon_points.png")
    {
        collider.isTrigger = true;
    }
}
