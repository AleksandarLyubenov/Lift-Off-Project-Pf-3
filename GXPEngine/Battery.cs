using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Battery : Sprite
{
    public Battery(int Px, int Py, TiledObject obj = null) : base("icon_battery.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Battery(TiledObject obj = null) : base("icon_battery.png")
    {
        collider.isTrigger = true;
    }
}