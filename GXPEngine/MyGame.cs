using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;
using TiledMapParser;
using System.Globalization;
using GXPEngine.Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel.Design; // System.Drawing contains drawing tools such as Color definitions

public class MyGame : Game {
    EasyDraw HUD;
    EasyDraw FinalScore;
    Overlay overlay;
    DamagedOverlay damagedOverlay;
    HeartTracker heartTracker;
    BatteryTracker batteryTracker;

    public Player player;

    Font win_command = Utils.LoadFont("windows_command_prompt.ttf", 24, FontStyle.Regular);
    Font win_command_score = Utils.LoadFont("windows_command_prompt.ttf", 48, FontStyle.Regular);

    public string currentLevel;
    public bool levelComplete = false;

    public int finalScore = 0;

    public int Score = 0;
    private SoundChannel bg_music;
    private SoundChannel menu_music;
    private SoundChannel menu_confirm;
    private SoundChannel player_dies;
    private SoundChannel level_cleared;
    private SoundChannel ticking;

    bool playerDiedCheck = false;
    bool isTickerPlaying = false;
    bool menuIsPlaying = false;

    public int timerSeconds = 180;
    float elapsedTime = 0;

    public MyGame() : base(1366, 768, false, false, 1366, 768, false)     // Create a window that's 800x600 and NOT fullscreen
	{
        //SetVSync(true);
        EasyDraw canvas = new EasyDraw(1366, 768, false);
        
		canvas.Clear(Color.Blue); // Background 

        AddChild(canvas);

        LoadLevel("mainmenu.tmx"); // Loads initial level
        timerSeconds = 180;

        //Loading all sounds
        menuThemePlay();

        Console.WriteLine("MyGame initialized");
    }

    void menuConfirmPlay()
    {
        menu_confirm = new Sound("Menu_confirm.wav", false, false).Play();
    }

    void menuThemePlay()
    {
        menu_music = new Sound("Menu Music - Idea 1.wav", true, false).Play();
        menuIsPlaying = true;
    }
    void playerDiesPlay() 
    {
        player_dies = new Sound("Player_Death.wav", false, false).Play();
        playerDiedCheck = true;
    }

    void tickerPlay()
    {
        ticking = new Sound("Clock_Ticking_Fx.wav", false, false).Play();
        isTickerPlaying = true;
    }

    void HUDCreator() // Creates the HUD in the bottom left of the screen
    {
        HUD = new EasyDraw(100, 50);
        HUD.TextAlign(CenterMode.Center, CenterMode.Min);
        HUD.TextFont(win_command);
        HUD.SetXY(width/2, 5);
        HUD.Text("03:00");
    }

    public void ShowHUDText()
    {
        HUD.ClearTransparent();
        int minutes = timerSeconds / 60;
        int seconds = timerSeconds % 60;
        HUD.Text(minutes.ToString("00") + ":" + seconds.ToString("00"));
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
        if (levelComplete == true)
        {
            finalScore += Score;
            finalScore += timerSeconds;
            Score = 0;
            level_cleared = new Sound("Level_passed.wav", false, false).Play();
        }

        levelComplete = false;

        player = FindObjectOfType<Player>();
        if (currentLevel == "mainmenu.tmx" || currentLevel == "badscreen.tmx" || currentLevel == "goodscreen.tmx")
        {

        }
        else
        {
            OverlayCreator();
            AddChild(overlay);
            DamagedOverlayCreator();
            AddChild(damagedOverlay);
            HUDCreator();
            AddChild(HUD);
            HeartsGUI();
            AddChild(heartTracker);
            EnergyGUI();
            AddChild(batteryTracker);
        }
    }

    void HeartsGUI()
    {
        heartTracker = new HeartTracker();
    }

    void EnergyGUI()
    {
        batteryTracker = new BatteryTracker();
        batteryTracker.SetXY(width - 322.5f, 0);
    }

    void DamagedOverlayCreator()
    {
        damagedOverlay = new DamagedOverlay();
    }
    void OverlayCreator()
    {
        overlay = new Overlay();
    }

    void Update() { // Update is used to update the HUD
        if (timerSeconds <= 0 && (currentLevel != "badscreen.tmx" || currentLevel != "goodscreen.tmx"))
        {
            // Game over or handle timer finish
            LoadLevel("badscreen.tmx");
        }

        if (Input.GetKeyDown(Key.F1))
        {
            Console.WriteLine(GetDiagnostics());
            Console.WriteLine(currentFps);
        }

        // If one second has passed, decrement timer by 1
        elapsedTime++;
        if (elapsedTime == 60)
        {
           timerSeconds--;
           elapsedTime = 0;
        }

        // Update HUD
        if (currentLevel == "mainmenu.tmx" || currentLevel == "goodscreen.tmx" || currentLevel == "badscreen.tmx")
        {
            
        }
        else
        {
            ShowHUDText();
        }
        menuNavigation();

        if (timerSeconds == 10 && isTickerPlaying == false)
        {
            tickerPlay();
        }
    }

    bool scoreCreated = false;

    void ScoreCreator() // Creates the HUD in the bottom left of the screen
    {
        if (scoreCreated == false)
        {
            FinalScore = new EasyDraw(420, 200);
            FinalScore.TextAlign(CenterMode.Center, CenterMode.Center);
            FinalScore.TextFont(win_command_score);
            FinalScore.SetXY(width / 2 - 200, height / 2 - 50);
            FinalScore.Text("Final Score: " + finalScore);
            AddChild(FinalScore);
            scoreCreated = true;
        }
    }

    void menuNavigation()
    {
        if (currentLevel == "goodscreen.tmx")
        {
            timerSeconds = int.MaxValue;
            HUD.Destroy();
            if (Input.GetKeyDown(Key.SPACE))
            {
                LoadLevel("mainmenu.tmx");
                bg_music.Stop();
                menuThemePlay();
            }
        }
        else if (currentLevel == "mainmenu.tmx" || currentLevel == "badscreen.tmx")
        {
            timerSeconds = int.MaxValue;
            //HUD.Destroy();
            if (Input.GetKeyDown(Key.SPACE))
            {
                timerSeconds = 15;
                if (currentLevel == "badscreen.tmx")
                {
                    menuConfirmPlay();
                    player_dies.Stop();
                    bg_music = new Sound("Game Music - Idea 1.wav", true, false).Play();
                    bg_music.Volume = 0.5f;
                    playerDiedCheck = false;
                    menuIsPlaying = false;
                }
                else if (currentLevel == "mainmenu.tmx")
                {
                    menuConfirmPlay();
                    menu_music.Stop();
                    bg_music = new Sound("Game Music - Idea 1.wav", true, false).Play();
                    bg_music.Volume = 0.5f;
                }
                LoadLevel("lvl1.tmx");
                Score = 0;
                finalScore = 0;
                timerSeconds = 180;
                scoreCreated = false;
            }
        }

        if (currentLevel == "goodscreen.tmx")
        {
            timerSeconds = int.MaxValue;
            if (isTickerPlaying == true)
            {
                ticking.Stop();
                isTickerPlaying = false;
            }
            menu_music.Stop();
            bg_music.Stop();
            ScoreCreator();
        }

        if (currentLevel == "badscreen.tmx")
        {
            if (isTickerPlaying == true)
            {
                ticking.Stop();
                isTickerPlaying = false;
            }
            bg_music.Stop();
            if (playerDiedCheck == false) 
            {
                playerDiesPlay();
            }
        }
    }

	static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
	}
}