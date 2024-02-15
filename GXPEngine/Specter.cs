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
public class Specter : AnimationSprite
{
    float targetDistance;
    int counter;
    int frame;
    int speed = 1;
    static Random random = new Random();
    int direction = 1;
    float directionSwitchTime;

    public Specter(int Px, int Py, TiledObject obj = null) : base("Ghost.png", 4, 3)
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
        directionSwitchTime = random.Next(2, 5);
    }

    public Specter(TiledObject obj = null) : base("Ghost.png", 4, 3)
    {
        collider.isTrigger = true;
        targetDistance = obj.GetFloatProperty("moveDistance");
        directionSwitchTime = random.Next(2, 5);
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

    void checkBounds()
    {
        if (parent != null)
        {
            Level myLevel = (Level)parent;
            if (x <= 0 || x >= myLevel.maxX)
            {
                switchDirection();
                directionSwitchTime = random.Next(2, 5);
            }
            if (y <= 0 || y >= myLevel.maxY)
            {
                switchDirection();
                directionSwitchTime = random.Next(2, 5);
            }
        }
    }

    void Update()
    {
        float oldX = x;
        float oldY = y;

        float elapsedTime = Time.deltaTime / 1000f; // Time.deltaTime is in milliseconds, so convert to seconds
        directionSwitchTime -= elapsedTime; // Reduce the time remaining for direction switch

        if (directionSwitchTime <= 0)
        {
            switchDirection();
            directionSwitchTime = random.Next(2, 5); // Randomize the next switch time between 2 to 4 seconds
        }

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
        checkBounds();
    }
}