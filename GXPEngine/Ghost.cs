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
    Player player;
    float targetDistance;
    int counter;
    int currentGameFrame = 0;
    int frame;
    int speed = 1;
    static Random random = new Random();
    int direction = 1;
    int killDistance = 312;
    int playerEnergy = 0;

    private SoundChannel hurt_enemy;

    public Ghost(int Px, int Py, TiledObject obj = null) : base("Stumbler_anim.png", 8, 8)
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public Ghost(TiledObject obj = null) : base("Stumbler_anim.png", 8, 8)
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
    }

    void Update()
    {
        player = ((MyGame)game).player;

        if (currentGameFrame >= 60)
        {
            currentGameFrame = 0;
        }
        else
        {
            currentGameFrame++;
        }

        if (direction == 0)
        {
            if (counter <= 16)
            {
                frame++;
                if (frame > 15) // 3 - end of walking to the right
                {
                    frame = 0;
                }
            }
            else
            {
                counter = 0;
            }
        }
        else if (direction == 1)
        {
            if (counter <= 16)
            {
                frame++;
                if (frame > 31) // 3 - end of walking to the right
                {
                    frame = 16;
                }
            }
            else
            {
                counter = 0;
            }
        }
        else if (direction == 2)
        {
            if (counter <= 16)
            {
                frame++;
                if (frame > 63) // 3 - end of walking to the right
                {
                    frame = 48;
                }
            }
            else
            {
                counter = 0;
            }
        }
        else if (direction == 3)
        {
            if (counter <= 16)
            {
                frame++;
                if (frame > 47) // 3 - end of walking to the right
                {
                    frame = 32;
                }
            }
            else
            {
                counter = 0;
            }
        }

        float oldX = x;
        float oldY = y;
        walkTo();
        if (currentGameFrame % 12 == 0)
        {
            counter++;
            SetFrame(frame);
        }
        if (player != null)
        {
            if (Input.GetKeyDown(Key.SPACE) && playerEnergy != player.energy)
            {
                // Calculate distance between ghost and player
                float distance = Mathf.Sqrt(Mathf.Pow(player.x - x, 2) + Mathf.Pow(player.y - y, 2));

                // If player is within 312 pixels
                if (distance <= 312)
                {
                    // Destroy the ghost
                    hurt_enemy = new Sound("Monster_dying.wav", false, false).Play();
                    ((MyGame)game).Score += 3;
                    LateDestroy();
                    return;
                }
                else
                {
                    // Print how far away the player is
                    Console.WriteLine("Player is " + (distance - 312) + " pixels away from the Ghost.");
                }
                playerEnergy = player.energy;
            }
        }

        GameObject[] collisions = GetCollisions();
        if (collisions.Length > 0)
        {
            //Console.WriteLine("Number of Collissions: " + collisions.Length);
        }
        for (int i = 0; i < collisions.Length; i++)
        {
           /* Console.WriteLine("Colliding with " + collisions[i].name);*/ // Adds collision with anything that is not special (i.e. targets, destroyers, gates)
            x = oldX;
            y = oldY;
            switchDirection();
        }
    }
}