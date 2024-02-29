using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class DamagedOverlay : AnimationSprite
{
    public string LevelName;
    int frame = 0;
    int counter = 0;
    int flashDuration = 15;
    int initialHealth = 0;
    public DamagedOverlay(int Px, int Py, TiledObject obj = null) : base("overlay_damage.png", 2, 1, addCollider: false)
    {
        x = Px;
        y = Py;
    }

    public DamagedOverlay(TiledObject obj = null) : base("overlay_damage.png", 2, 1, addCollider: false)
    {

    }

    void Update()
    {
        if (((MyGame)game).player != null)
        {
            if (initialHealth != ((MyGame)game).player.takenDamage)
            {
                counter++;
                initialHealth++;
            }
            if (counter > 0 && counter < flashDuration)
            {
                frame = 1;
                counter++;
                Console.WriteLine(counter);
            }
            else if (counter == flashDuration)
            {
                frame = 0;
                counter = 0;
            }
            SetFrame(frame);
        }
        else
        {
            Console.WriteLine("No player found!");
        }
    }
}