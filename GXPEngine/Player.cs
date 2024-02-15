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

    string resetLevelName;
    float jumpStrength;
    float speed; 
    float gravity;
    int counter;
    int frame;


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
    }

    Vector2 moveTo;

    int deltaTimeClamped = Mathf.Min(Time.deltaTime, 40);
    int pickedUpItems;

    void Update() // Update holds the movement controller (with boundaries) and collision checks
    {
        counter++;
        Level myLevel = (Level)parent;                    // Is the parent always a Level...? (moving platforms?)
        
        float oldX = x;
        float oldY = y;

        if (y >= (myLevel.maxY))
        {
            ((MyGame)game).LoadLevel(resetLevelName);
        }

        MoveUntilCollision(moveTo.x, 0);
        MoveUntilCollision(0, moveTo.y);

        if (Input.GetKey(Key.A)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.x = -speed;

            if (counter > 10)
            {
                counter = 0;
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

            if (counter > 10)
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
        else
        {
            moveTo.x = 0;
            frame = 0;
        }

        if (Input.GetKey(Key.S)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.y = speed;

            if (counter > 10)
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
        else if (Input.GetKey(Key.W)) // && !(Input.GetKey(Key.LEFT_SHIFT)))
        {
            moveTo.y = -speed;

            if (counter > 10)
            {
                counter = 0;
                frame++;
                if (frame == 13) // 13 - end of walking to the left
                {
                    frame = 10;
                }
                SetFrame(frame); // AnimationSprite method
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
            Console.WriteLine("Number of Collissions: " + collisions.Length);
        }
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i] is Target)
            {
                collisions[i].Destroy();
                ((MyGame)game).Score++;
                pickedUpItems++;
                Console.WriteLine(pickedUpItems);
                collect_sound = new Sound("collect_sound.mp3", false, false).Play();
            }
            else if (collisions[i] is Ghost || collisions[i] is Specter)
            {
                ((MyGame)game).Score = ((MyGame)game).Score - pickedUpItems;
                pickedUpItems = 0;
                //Console.WriteLine(((MyGame)game).Score);
                ((MyGame)game).LoadLevel(resetLevelName);

            } else if (collisions[i] is Gate)
            {
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