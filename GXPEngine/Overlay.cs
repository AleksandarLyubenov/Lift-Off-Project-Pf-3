using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class Overlay : AnimationSprite
{
    public string LevelName;
    int frame = 0;
    int counter = 0;
    int flashDuration = 15;
    int playerEnergy = 0;
    public Overlay(int Px, int Py, TiledObject obj = null) : base("overlay_dark.png", 2, 1,addCollider:false)
    {
        x = Px;
        y = Py;
        //collider.isTrigger = false;
    }

    public Overlay(TiledObject obj = null) : base("overlay_dark.png", 2, 1, addCollider:false)
    {
        //blendMode = BlendMode.MULTIPLY;
        //collider.isTrigger = false;
    }

    void Update()
    {
        if (((MyGame)game).player != null)
        {
            if (Input.GetKeyDown(Key.SPACE) && ((MyGame)game).player.energy != playerEnergy)
            {
                counter = 0;
                counter++;
            }
            if (counter > 0 && counter < flashDuration)
            {
                frame = 1;
                counter++;
            }
            else if (counter == flashDuration)
            {
                frame = 0;
                counter = 0;
            }
            SetFrame(frame);
            playerEnergy = ((MyGame)game).player.energy;
        }
    }
}