using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;
public class Ghost : AnimationSprite
{
    float targetDistance;
    int counter;
    int frame;
    int speed = 1;
    float startX;

    public Ghost(int Px, int Py, TiledObject obj = null) : base("Ghost.png",4,3)
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
        startX = x;
    }

    public Ghost(TiledObject obj = null) : base("Ghost.png",4,3)
    {
        collider.isTrigger = true;
        targetDistance = obj.GetFloatProperty("moveDistance");
    }


    void walkTo()
    {
        // Check if the Destroyer is at the target position
        if (Mathf.Abs(x - startX) >= targetDistance)
        {
            // Change direction when reaching the target
            speed = -speed;
            startX = x; // Update the startX position for the next iteration
        }

        Move(speed, 0); // Move in the current direction
    }

    void Update()
    {
        walkTo();
        counter++;
        if (counter > 18)
        {
            counter = 0;
            frame++;
            if (frame == 3) // 3 - end of walking to the right
            {
                frame = 0;
            }
            SetFrame(frame); // AnimationSprite method
        }
    }

}