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




public class Game : IGameEventProcessor<object>
{
    private Score score;
    private ImageStride imageStride;
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
    private TimedEvent EnemyRes = new TimedEvent(TimeSpanType.Seconds, 2, "Explosion_Done");
    private int SquareDifficulty = 0;
    private int BoxDifficulty = 0;
    private int RectDifficulty = 0;
    private Square squarePlace;
    private Box boxPlace;
    private Rectangle RectPlace;
    private string prevSquad;

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
                GameEventType.PlayerEvent,
                GameEventType.StatusEvent,
                });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, player);

        // Look at the file and consider why we place the number '4' here.
        enemyStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemies = new List<Enemy>();
        
        imageStride = new ImageStride(80, enemyStrides);

        playerShots = new List<PlayerShot>();

        explosionStrides = ImageStride.CreateStrides(4,
            Path.Combine("Assets", "Images", "Explosion.png"));
        explosions = new AnimationContainer(explosionLength);

        score = new Score(new Vec2F(0.4f, -0.25f), new Vec2F(0.3f, 0.3f));

        AddEnemies();
 
    }
    public void AddEnemies()
    {
        switch (prevSquad) {
            case "Square":
                boxPlace = new Box(BoxDifficulty);

                boxPlace.CreateEnemies(enemyStrides);
                foreach (Enemy enemy in boxPlace.Enemies) {
                    enemies.Add(enemy);
                    enemy.Image = this.imageStride;
                }

                BoxDifficulty++;
                prevSquad = "Box";
                break;
            case "Box":
                RectPlace = new Rectangle(RectDifficulty);

                RectPlace.CreateEnemies(enemyStrides);
                foreach (Enemy enemy in RectPlace.Enemies) {
                    enemies.Add(enemy);
                    enemy.Image = this.imageStride;
                }

                RectDifficulty++;
                prevSquad = "Rectangle";

                break;
            case "Rectangle":
                squarePlace = new Square(SquareDifficulty);

                squarePlace.CreateEnemies(enemyStrides);
                foreach (Enemy enemy in squarePlace.Enemies) {
                    enemies.Add(enemy);
                    enemy.Image = this.imageStride;
                }

                SquareDifficulty++;
                prevSquad = "Square";
                break;
            default:
                prevSquad = "Square";
                break;
        }
    }

    // PLANNED PLACEMENT OF IMPLEMENTATION OF MOVEMENT STRATEGIES, CURRENTLY TOO CLOSE TO DEADLINE TO BE ABLE TO FIX RENDERING ISSUES WHEN USING MOVEMENT STRATEGIES

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

    public void CheckEnemy() {
        if (enemies.Count == 0) {
            while (EnemyRes.HasExpired()) {
                AddEnemies();
                EnemyRes.ResetTimer();
            }
        } else {
            EnemyRes.ResetTimer();
        }
    }
    
    public void IterateShots() {
        PlayerShot DeletedShot = null;
        foreach (var shot in playerShots) {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f) {
                shot.DeleteEntity();
                DeletedShot = shot;
            } else {
                int i = 1;
                Enemy enemyDelete = null;
                foreach (var enemy in enemies) {
                    i++;
                    if (CollisionDetection.Aabb(shot.Entity.Shape.AsDynamicShape(), enemy.Shape).Collision == true) {
                        score.AddPoint();
                        enemyDelete = enemy;
                        DeletedShot = shot;
                        AddExplosion(enemy.Shape.Position.X, enemy.Shape.Position.Y, enemy.Shape.Extent.X, enemy.Shape.Extent.Y);
                        explosions.RenderAnimations();
                        shot.DeleteEntity();
                        enemy.DeleteEntity();

                    }
                }
                if (enemyDelete != null) {
                    enemyDelete.DeleteEntity();
                    enemyDelete.RenderEntity();
                    enemies.Remove(enemyDelete);

                }
                shot.RenderEntity();
            }
        }
        if (DeletedShot != null) {
            DeletedShot.DeleteEntity();
            DeletedShot.RenderEntity();
            playerShots.Remove(DeletedShot);
        }
    }
    public void GameLoop() {
        while(win.IsRunning()) {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate()) {
                win.PollEvents();
                // Update game logic here
                player.Move();
                CheckEnemy();
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
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.PlayerEvent, this, player, "KEY_LEFT", "KEY_PRESS", ""));
                    CheckEnemy();
            break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.PlayerEvent, this, player, "KEY_RIGHT", "KEY_PRESS", ""));
                    CheckEnemy();
            break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.InputEvent, this, "KEY_SPACE", "", ""));
                    addShot();
                    CheckEnemy();
                    
            break;

        }
    }
    public void KeyRelease(string key) {
        switch (key) {
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.PlayerEvent, this, player, "KEY_RIGHT", "KEY_RELEASE", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                    GameEventType.PlayerEvent, this, player, "KEY_LEFT", "KEY_RELEASE", ""));
                break;
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
        } else if (eventType == GameEventType.InputEvent) {
            switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                   KeyPress(gameEvent.Message);
                   break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
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