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




public class Game : IGameEventProcessor<object>
{
    private Score score;
    private ImageStride imageStride;
    private ImageStride imageStride2;
    private Window win;
    private Player player;
    private DIKUArcade.Timers.GameTimer gameTimer;
    private GameEventBus<object> eventBus;
    public List<Image> enemyStrides;
    public List<PlayerShot> playerShots;
    public List<Enemy> enemies;
    private List<Image> explosionStrides;
    private AnimationContainer explosions;
    private int explosionLength = 500;

    public Game() 
    {
        // TODO: Choose some reasonable values for the window and timer constructor.
        // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
        win = new Window("Main" , 500, 500);
        gameTimer = new GameTimer(60, 60);

        player = new Player(
         new DynamicShape(new Vec2F(0.43f, 0.1f), new Vec2F(0.1f, 0.1f)),
         new Image(Path.Combine("Assets", "Images", "Player.png")));

        eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType>() {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window 
                });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);

        // Look at the file and consider why we place the number '4' here.
        enemyStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemies = new List<Enemy>();
        
        imageStride = new ImageStride(80, enemyStrides);

        // imageStride2 = new ImageStride(60, explosionStrides);

        playerShots = new List<PlayerShot>();

        AddEnemies();

        explosionStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "Explosion.png"));
        explosions = new AnimationContainer(explosionLength);

        score = new Score(new Vec2F(0.02f, 0.65f), new Vec2F(0.3f, 0.3f));
 
    }
    public void AddEnemies()
    {
       Enemy enemy = new Enemy(
         new DynamicShape(new Vec2F(0.43f, 0.88f), new Vec2F(0.1f, 0.1f)),
         new Image(Path.Combine("Assets", "Images", "BlueMonster.png")));
       enemies.Add(enemy);
       enemy.Image = this.imageStride;
       enemy = new Enemy(
         new DynamicShape(new Vec2F(0.3f, 0.88f), new Vec2F(0.1f, 0.1f)),
         new Image(Path.Combine("Assets", "Images", "BlueMonster.png")));
       enemies.Add(enemy);
       enemy.Image = this.imageStride;
       enemy = new Enemy(
         new DynamicShape(new Vec2F(0.56f, 0.88f), new Vec2F(0.1f, 0.1f)),
         new Image(Path.Combine("Assets", "Images", "BlueMonster.png")));
       enemies.Add(enemy);

       enemy.Image = this.imageStride;

    //    enemy.Image = this.imageStride2;
    }
    public void addShot()
    {
       PlayerShot playerShot = new PlayerShot(
       new DynamicShape(new Vec2F(player.Entity.Shape.Position.X + 0.045f, 0.2f), new Vec2F(0.008f, 0.027f)), 
       new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
       
       playerShots.Add(playerShot);
    }
    private void AddExplosion(float posX, float posY, float extentX, float extentY) 
    {
        explosions.AddAnimation(
        new StationaryShape(posX, posY, extentX, extentY), explosionLength,
        new ImageStride(explosionLength / 8, explosionStrides));
    }
    
    public void IterateShots() {
        foreach (var shot in playerShots) 
        {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f) 
            {
                shot.DeleteEntity();
            } else 
            {
                int i = 1;
                Enemy enemyDelete = null;
                foreach (var enemy in enemies) 
                {
                    i++;
                    if (CollisionDetection.Aabb(shot.Entity.Shape.AsDynamicShape(), enemy.Shape).Collision == true)
                    {
                        score.AddPoint();
                        enemyDelete = enemy;
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y, enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                        explosions.RenderAnimations();
                        shot.DeleteEntity();
                        enemy.DeleteEntity();

                    }
            }
            if (enemyDelete != null)
            {
                enemyDelete.DeleteEntity();
                enemyDelete.RenderEntity();
                enemies.Remove(enemyDelete);

            }
            shot.RenderEntity();
        }
    }
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
                score.RenderScore();
                foreach (Enemy enemy in this.enemies)
                {
                    enemy.RenderEntity();
                }
                IterateShots();
                eventBus.ProcessEvents();
                player.Entity.RenderEntity();
                explosions.RenderAnimations();
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
                    player.Direction(new Vec2F(-0.0060f, 0.0000f));
            break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.InputEvent, this, "KEY_RIGHT", "", ""));
                    player.Direction(new Vec2F(0.0060f, 0.0000f));
            break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.InputEvent, this, "KEY_SPACE", "", ""));
                    addShot();
                    
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
        player.Direction(new Vec2F(0.0000f, 0.0000f));
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