using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.EventBus;
using System;
using DIKUArcade;
using DIKUArcade.Timers;
using System.Collections.Generic;
using DIKUArcade.Physics;
using galaga.Squadron;
using galaga.MovementStrategy;

namespace galaga.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance = null;
        private Random rand = new Random();
        private ImageStride imageStride;
        private Player player;
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
        private float SpdDiff;
        private ZigZagDown ZigZagMove = new ZigZagDown(0f);
        private Down DownMove = new Down(0f);
        private NoMove noMove = new NoMove();
        private bool GameOverActive = false;
        private int Pattern = 0;
        private Score score;
        private GameEventBus<object> eventBus;

        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
        }

        public void GameLoop() {
            
        }

        public void InitializeGameState() {
            player = new Player(
                new DynamicShape(new Vec2F(0.43f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));

            enemies = new List<Enemy>();
        
            imageStride = new ImageStride(80, enemyStrides);

            playerShots = new List<PlayerShot>();

            explosionStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "Explosion.png"));

            explosions = new AnimationContainer(explosionLength);

            score = new Score(new Vec2F(0.4f, -0.25f), new Vec2F(0.3f, 0.3f));

            eventBus = galaga.GalagaBus.GetBus();
            eventBus.Subscribe(GameEventType.PlayerEvent, player);

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
                    SpdDiff += 0.0005f;
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
                    SpdDiff += 0.0005f;
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
                    SpdDiff += 0.0005f;
                    prevSquad = "Square";
                    break;
                default:
                    prevSquad = "Square";
                    break;
            }
        }

    public void MoveEnemy(List<Enemy> EnemyList) {
            switch (Pattern) {
                case 0:
                    ZigZagMove.spd = SpdDiff;
                    foreach (Enemy enemy in EnemyList) {
                        ZigZagMove.MoveEnemy(enemy);
                    }
                    break;
                case 1:
                    DownMove.spd = SpdDiff;
                    foreach (Enemy enemy in EnemyList) {
                        DownMove.MoveEnemy(enemy);
                    }
                    break;
                case 2:
                    foreach (Enemy enemy in EnemyList) {
                        noMove.MoveEnemy(enemy);
                    }
                    break;
            }
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
        public void NewPattern() {
            Pattern = rand.Next(3);
        }

        public void CheckEnemy() {
            if (this.enemies.Count == 0) {
                NewPattern();
                if (EnemyRes.HasExpired()) {
                    AddEnemies();
                    EnemyRes.ResetTimer();
                }
            } else {
                EnemyRes.ResetTimer();
            }

            foreach (Enemy enemy in this.enemies) {
                if (enemy.Shape.Position.Y <= 0f) {
                    GameOverActive = true;
                }
            }
        }
        public void GameOver() {
            GameOverActive = true;
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
                    foreach (var enemy in this.enemies) {
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

        public void UpdateGameLogic() {
            if (!GameOverActive) {
                player.Move();
                MoveEnemy(this.enemies);
                CheckEnemy();
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESS") {
                KeyPress(keyValue);
            } else if (keyAction == "KEY_RELEASE") {
                KeyRelease(keyValue);
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
                    break;
                case "KEY_RIGHT":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForSpecificProcessor(
                        GameEventType.PlayerEvent, this, player, "KEY_RIGHT", "KEY_PRESS", ""));
                    break;
                case "KEY_SPACE":
                    eventBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.InputEvent, this, "KEY_SPACE", "", ""));
                        addShot();
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
        public void RenderState() {
            if (GameOverActive) {
                // Render score only
                score.RenderScore();
            } else {
                score.RenderScore();
                foreach (Enemy enemy in this.enemies)
                {
                    enemy.RenderEntity();
                }
                IterateShots();
                player.Entity.RenderEntity();
                explosions.RenderAnimations();
            }
        }
    }
}