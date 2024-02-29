using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class BatteryTracker : AnimationSprite
{
    public string LevelName;
    int frame = 0;
    int energy;

    public BatteryTracker(int Px, int Py, TiledObject obj = null) : base("overlay_batteries.png", 1, 4, addCollider: false)
    {
        x = Px;
        y = Py;
    }

    public BatteryTracker(TiledObject obj = null) : base("overlay_batteries.png", 1, 4, addCollider: false)
    {
        scale = 0.75f;
    }

    void Update()
    {
        if (((MyGame)game).player != null)
        {
            energy = ((MyGame)game).player.energy;
            if (energy == 3)
            {
                frame = 0;
            }
            else if (energy == 2)
            {
                frame = 1;
            }
            else if (energy == 1)
            {
                frame = 2;
            }
            else if (energy == 0)
            {
                frame = 3;
            }    
            SetFrame(frame);
        }
    }
}