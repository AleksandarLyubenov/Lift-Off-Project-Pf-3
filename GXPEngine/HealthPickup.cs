using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class HealthPickup : Sprite
{
    public HealthPickup(int Px, int Py, TiledObject obj = null) : base("icon_heart.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public HealthPickup(TiledObject obj = null) : base("icon_heart.png")
    {
        collider.isTrigger = true;
    }
}
