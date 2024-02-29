using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine;
using TiledMapParser;
public class RestartButton : Sprite
{
    EasyDraw FinalScore;
    public string LevelName;
    public RestartButton(int Px, int Py, TiledObject obj = null) : base("RestartButton.png")
    {
        x = Px;
        y = Py;
        collider.isTrigger = true;
    }

    public RestartButton(TiledObject obj = null) : base("RestartButton.png")
    {
        collider.isTrigger = true;
        ScoreCreator();
    }

    void Update()
    {

    }

    void ScoreCreator() // Creates the HUD in the bottom left of the screen
    {
        FinalScore = new EasyDraw(250, 100);
        FinalScore.SetXY(35, y + 70);
        FinalScore.TextSize(32);
        FinalScore.TextAlign(CenterMode.Center, CenterMode.Center);
        FinalScore.Text("" + ((MyGame)game).Score);
        AddChild(FinalScore);
    }
}