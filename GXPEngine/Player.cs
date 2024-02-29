using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Player : AnimationSprite
{
    private SoundChannel collect_sound;
    private SoundChannel health_pickup;
    private SoundChannel energy_pickup;
    private SoundChannel player_hurt;
    private SoundChannel flashing_light;

    string resetLevelName;
    float jumpStrength;
    float speed; 
    float gravity;
    int counter;
    int currentGameFrame = 0;
    int frame;
    public int health;
    public int energy;
    bool usingFlashlight = false;
    public int takenDamage = 0;

    //System.Random random = new System.Random();
    public Player(string filename, int Px, int Py, TiledObject obj=null) : base("chick_player.png", 7, 2) // base(filename) - when player is a sprite
    {
        x = Px;
        y = Py;
    }

    public Player(TiledObject obj = null) : base("chick_player.png",7,2)
    {
        resetLevelName = obj.GetStringProperty("resetLevelName", "mainmenu.tmx");
        jumpStrength = obj.GetFloatProperty("jumpStrength", 8.2f);
        speed = obj.GetFloatProperty("speed", 3.7f);
        gravity = obj.GetFloatProperty("gravity", 9.8f);
        energy = obj.GetIntProperty("energy", 0);
        health = 2;
    }

    Vector2 moveTo;

    int deltaTimeClamped = Mathf.Min(Time.deltaTime, 40);
    int pickedUpItems;

    void Update() // Update holds the movement controller (with boundaries) and collision checks
    {
        if(currentGameFrame >= 60)
        {
            currentGameFrame = 0;
        }
        else
        {
            currentGameFrame++;
        }

        if (currentGameFrame % 12 == 0)
        {
            counter++;
            SetFrame(frame);
        }

        Level myLevel = (Level)parent;                    // Is the parent always a Level...? (moving platforms?)
        
        float oldX = x;
        float oldY = y;

        if (myLevel != null)
        {
            if (y >= (myLevel.maxY))
            {
                ((MyGame)game).LoadLevel(resetLevelName);
                ((MyGame)game).timerSeconds = 180;
            }
        }
        
        MoveUntilCollision(moveTo.x, 0);
        MoveUntilCollision(0, moveTo.y);

        if (Input.GetKeyDown(Key.SPACE) && usingFlashlight == false && energy != 0)
        {
            flashing_light = new Sound("FlashLight_On.wav", false, false).Play();
            usingFlashlight = true;
            energy--;
        }

        if (Input.GetKeyUp(Key.SPACE) && usingFlashlight == true)
        {
            usingFlashlight = false;
        }

        if (Input.GetKey(Key.A)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.x = -speed;

            if (counter < 16)
            {
                frame++;
                if (frame == 13) // 13 - end of walking to the left
                {
                    frame = 10;
                }
                SetFrame(frame); // AnimationSprite method
            }
        }
        else if (Input.GetKey(Key.D)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.x = speed;

            if (counter < 16)
            {
                frame++;
                if (frame == 3) // 3 - end of walking to the right
                {
                    frame = 0;
                }
                SetFrame(frame); // AnimationSprite method
            }
        }
        else
        {
            moveTo.x = 0;
            frame = 0;
        }

        if (Input.GetKey(Key.S)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.y = speed;

            if (counter < 16)
            {
                frame++;
                if (frame == 3) // 3 - end of walking to the right
                {
                    frame = 0;
                }
                SetFrame(frame); // AnimationSprite method
            }
        }
        else if (Input.GetKey(Key.W)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.y = -speed;

            if (counter < 16)
            {
                frame++;
                if (frame == 13) // 13 - end of walking to the left
                {
                    frame = 10;
                }
                 // AnimationSprite method
            }
        }
        else
        {
            moveTo.y = 0;
            frame = 0;
        }

        GameObject[] collisions = GetCollisions();
        if (collisions.Length>0)
        {
            //Console.WriteLine("Number of Collissions: " + collisions.Length);
        }
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is Target)
            {
                collisions[i].Destroy();
                ((MyGame)game).Score += 25;
                collect_sound = new Sound("Collect_points.wav", false, false).Play();
            }
            else if (collisions[i] is Ghost || collisions[i] is Specter)
            {
                if (health == 0)
                {
                    ((MyGame)game).LoadLevel("badscreen.tmx");
                }
                else
                {
                    health--;
                    ((MyGame)game).Score -= 5;
                    collisions[i].Destroy();
                    takenDamage++;
                    player_hurt = new Sound("Hurt_losing_one_life.wav", false, false).Play();
                }
            }
            else if (collisions[i] is HealthPickup)
            {
                if (health == 2)
                {
                    Console.WriteLine("Already at full health!");
                }
                else if (health < 2 && health > 0)
                {
                    collisions[i].Destroy();
                    health++;
                    collect_sound = new Sound("Collect_life.wav", false, false).Play();
                }
            }
            else if (collisions[i] is Battery)
            {
                if (energy == 3)
                {
                    Console.WriteLine("Already at full charge!");
                }
                else if (energy <= 2 && energy >= 0)
                {
                    energy++;
                    collect_sound = new Sound("Collect_flashlight_charge.wav", false, false).Play();
                    collisions[i].Destroy();
                }
            }
            else if (collisions[i] is Gate)
            {
                ((MyGame)game).levelComplete = true;
                Gate gate = (Gate)collisions[i];
                ((MyGame)game).LoadLevel(gate.LevelName); // using collisions it gets the name of the level from the gate and loads it using LoadLevel
            }
            else
            {
                Console.WriteLine("Colliding with " + collisions[i].name); // Adds collision with anything that is not special (i.e. targets, destroyers, gates)
                x = oldX;
                y = oldY;
            }
        }
    }
}