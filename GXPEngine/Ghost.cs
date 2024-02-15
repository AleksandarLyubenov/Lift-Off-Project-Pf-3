using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    static Random random = new Random();
    int direction = 1;

    public Ghost(int Px, int Py, TiledObject obj = null) : base("Ghost.png",4,3)
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Ghost(TiledObject obj = null) : base("Ghost.png",4,3)
    {
        collider.isTrigger = true;
        targetDistance = obj.GetFloatProperty("moveDistance");
        switchDirection();
    }

    void walkTo()
    {
        // Move(speed, 0); // Move in the current direction
        switch (direction)
        {
            case 0: // Left
                x -= speed;
                break;
            case 1: // Right
                x += speed;
                break;
            case 2: // Up
                y -= speed;
                break;
            case 3: // Down
                y += speed;
                break;
        }
    }

    void switchDirection()
    {
        direction = random.Next(4);
        Console.WriteLine(direction);
    }

    void Update()
    {
        float oldX = x;
        float oldY = y;
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

        GameObject[] collisions = GetCollisions();
        if (collisions.Length > 0)
        {
            Console.WriteLine("Number of Collissions: " + collisions.Length);
        }
        for (int i = 0; i < collisions.Length; i++)
        {
            Console.WriteLine("Colliding with " + collisions[i].name); // Adds collision with anything that is not special (i.e. targets, destroyers, gates)
            x = oldX;
            y = oldY;
            switchDirection();
        }
    }
}