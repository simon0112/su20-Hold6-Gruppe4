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

        score = new Score(new Vec2F(0.02f, 0.65f), new Vec2F(0.3f, 0.3f));

        AddEnemies();
 
    }
    public void AddEnemies()
    {
        Enemy enemy;

        for (float i = 0.3f; i <= 0.56f; i += 0.13f) {
            enemy = new Enemy(
                new DynamicShape(new Vec2F(i, 0.88f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "BlueMonster.png")));
            enemies.Add(enemy);
            enemy.Image = this.imageStride;
        }
        /*
        enemy = new Enemy(
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
        enemy.Image = this.imageStride;*/
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