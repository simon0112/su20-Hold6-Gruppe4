using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;


public class Game : IGameEventProcessor<object>
{
    private Player player;
    private Window win;
    private DIKUArcade.Timers.GameTimer gameTimer;
    public Game() {
        // TODO: Choose some reasonable values for the window and timer constructor.
        // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
        win = new Window("Main" , 500, 500);
        gameTimer = new GameTimer(60, 60);

        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
    }
    public void GameLoop() {
        while(win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                // Update game logic here
            }
            if (gameTimer.ShouldRender()) {
                win.Clear();
                // Render gameplay entities here
                win.SwapBuffers();
            }
            if (gameTimer.ShouldReset()) {
                // 1 second has passed - display last captured ups and fps
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
            }
        }
    }
    public void KeyPress(string key) {
        throw new NotImplementedException();
    }
    public void KeyRelease(string key) {
        throw new NotImplementedException();
    }
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
        throw new NotImplementedException();
}
}