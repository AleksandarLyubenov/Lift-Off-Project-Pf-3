using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class HeartTracker : AnimationSprite
{
    public string LevelName;
    int frame = 0;
    int currentHealth;
    
    public HeartTracker(int Px, int Py, TiledObject obj = null) : base("overlay_hearts.png", 1, 3, addCollider: false)
    {
        x = Px;
        y = Py;
    }

    public HeartTracker(TiledObject obj = null) : base("overlay_hearts.png", 1, 3, addCollider: false)
    {
        scale = 0.75f;
    }

    void Update()
    {
        if (((MyGame)game).player != null)
        {
            currentHealth = ((MyGame)game).player.health;
            if (currentHealth == 2)
            {
                frame = 0;
            }
            else if (currentHealth == 1)
            {
                frame = 1;
            }
            else if (currentHealth == 0)
            {
                frame = 2;
            }
            SetFrame(frame);
        }
    }
}