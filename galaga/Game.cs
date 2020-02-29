using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.IO;
using System;
using System.Collections.Generic;



public class Game : IGameEventProcessor<object>
{
    private Window win;
    private Player player;
    private DIKUArcade.Timers.GameTimer gameTimer;
    private GameEventBus<object> eventBus;
    public Game() 
    {
        // TODO: Choose some reasonable values for the window and timer constructor.
        // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
        win = new Window("Main" , 500, 500);
        gameTimer = new GameTimer(60, 60);

        player = new Player(
         new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
         new Image(Path.Combine("Assets", "Images", "Player.png")));

        eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window 
                });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);

    }
        public void GameLoop() {
        while(win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                // Update game logic here
                player.Move();
            }
            

            if (gameTimer.ShouldRender()) {
                win.Clear();
                // Render gameplay entities here
                eventBus.ProcessEvents();
                player.Entity.RenderEntity();
                win.SwapBuffers();
            }
            if (gameTimer.ShouldReset()) {
                // 1 second has passed - display last captured ups and fps
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
            }
        }
    }
    private void KeyPress(string key) {
        switch(key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.InputEvent, this, "KEY_LEFT", "", ""));
                    // rigrige syntakse for en vector som input
                    player.Direction(new Vec2F(0.001f, 0.002f));
            break;

        /*
            * TODO: Add cases that call player.Direction with a suitable direction.
            * You can match on cases such as "KEY_UP", "KEY_1", "KEY_A", etc.
            * Remember that the entire screen is 1.0f wide/tall, so choose a
            * fittingly small number for the direction, e.g. (0.01f,0.0f).
        */

        }
    }
    public void KeyRelease(string key) {
        player.Direction(new Vec2F(0.0001f, 0.0001f));
    }
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
        if (eventType == GameEventType.WindowEvent) {
            switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
                    break;
            }
        } else if (eventType == GameEventType.InputEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                   KeyPress(gameEvent.Message);
                   break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
            }     
        }
    }

}