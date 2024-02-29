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
    Player player;
    float targetDistance;
    int counter;
    int currentGameFrame = 0;
    int frame;
    int speed = 1;
    static Random random = new Random();
    int direction = 1;
    float directionSwitchTime;
    int killDistance = 312;
    int playerEnergy = 0;

    private SoundChannel hurt_enemy;

    public Specter(int Px, int Py, TiledObject obj = null) : base("Stumbler_anim.png", 8, 8)
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
        directionSwitchTime = random.Next(2, 5);
    }

    public Specter(TiledObject obj = null) : base("Stumbler_anim.png", 8, 8)
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
        if(currentGameFrame >= 60)
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

        player = ((MyGame)game).player;

        if (player == null)
        {
            Console.WriteLine("Player not found in the scene.");
            return;
        }

        float oldX = x;
        float oldY = y;

        if (currentGameFrame % 12 == 0)
        {
            counter++;
            SetFrame(frame);
        }

        float elapsedTime = Time.deltaTime / 1000f; // Time.deltaTime is in milliseconds, so convert to seconds
        directionSwitchTime -= elapsedTime; // Reduce the time remaining for direction switch

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
                    ((MyGame)game).Score += 4;
                    LateDestroy();
                    return;
                }
                else
                {
                    // Print how far away the player is
                    Console.WriteLine("Player is " + (distance - 312) + " pixels away from the Specter.");
                }
                playerEnergy = player.energy;
            }
        }

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