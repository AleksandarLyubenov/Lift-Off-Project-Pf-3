using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using TiledMapParser;
using System.Globalization;
using GXPEngine.Core;
using System.Collections.Generic;                           // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {
    EasyDraw HUD;
    EasyDraw information;

    public string currentLevel;

    int oldScore = 0;
	public int Score = 0;
    private SoundChannel bg_music;

    public MyGame() : base(1366, 768, false, true, 1366, 768, false)     // Create a window that's 800x600 and NOT fullscreen
	{
        // Draw some things on a canvas:

        //SetVSync(true);
        EasyDraw canvas = new EasyDraw(1366, 768, false);
        
		canvas.Clear(Color.Blue); // Background 

        AddChild(canvas);

        LoadLevel("mainmenu.tmx"); // Loads initial level
        bg_music = new Sound("background_music.mp3", true, true).Play();
        bg_music.Volume = 0.5f;

		// Add the canvas to the engine to display it:
		
		Console.WriteLine("MyGame initialized");
	}

    void HUDCreator() // Creates the HUD in the bottom left of the screen
    {               
        HUD = new EasyDraw(100, 50);
        HUD.TextAlign(CenterMode.Min, CenterMode.Center);
        HUD.SetXY(5, height - 55);
        HUD.Text("Score: 0");
    }

    public void ShowHUDText(String text) // Changes and displays the score
    {
        HUD.ClearTransparent();
        HUD.Text(text);
    }

    public void GameOverScreen() // Draw the game over screen
    {
        information = new EasyDraw(800, 600);
        information.Fill(255, 255, 255);
        information.TextAlign(CenterMode.Center, CenterMode.Center);
        information.Text("GAME OVER!!");
        information.SetOrigin(0, 0);
        information.SetXY(0, 0);
        AddChild(information);
    }

    void DestroyAll() // Called when loading new level
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.Destroy();
        }
    }

    public void LoadLevel(string filename)
    {
        DestroyAll();
        AddChild(new Level(filename));
        if (currentLevel != "mainmenu.tmx" && currentLevel != "goodscreen.tmx")
        {
            HUDCreator();
            AddChild(HUD);
        }
        
    }

    void Update() { // Update is used to update the HUD
        if (oldScore != Score)
        {
            ShowHUDText("Score: " + Score);
            oldScore = Score;
        }
        if (Input.GetKeyDown(Key.F1))
        {
            Console.WriteLine( GetDiagnostics());
            Console.WriteLine(currentFps);
        }
    }

	static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
	}

    
}