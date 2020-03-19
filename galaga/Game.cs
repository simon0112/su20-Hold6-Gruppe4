using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.IO;
using System;
using System.Collections.Generic;
using DIKUArcade.Physics;
using galaga.Squadron;
using galaga.MovementStrategy;
using galaga.GalagaStates;
using galaga.GalagaTypes;



public class Game : IGameEventProcessor<object>
{
    private Window win;
    private DIKUArcade.Timers.GameTimer gameTimer;
    private GameEventBus<object> eventBus;

    public Game() 
    {
        
        // TODO: Choose some reasonable values for the window and timer constructor.
        // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
        win = new Window("Main" , 500, 500);
        gameTimer = new GameTimer(60, 60);

        eventBus = galaga.GalagaBus.GetBus();
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);

        // Look at the file and consider why we place the number '4' here.
 
    }
    public void GameLoop() {
        while (win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                galaga.GalagaBus.GetBus().ProcessEvents();
                stateMachine.ActiveState.UpdateGameLogic();
            }
            if (gameTimer.ShouldRender()) {
                win.Clear();
                stateMachine.ActiveState.RenderState();
                win.SwapBuffers();
            }
            if (gameTimer.ShouldReset()) {
                // could display something, 1 second has passed
            }
        }
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
        }
    }
}