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

namespace galaga.Squadron {
    public class Rectangle : ISquadron
    {
        public Rectangle(int Diff) 
        {
            switch (Diff) {
                case 0:
                    MaxEnemies = 3;
                    break;
                case 1:
                    MaxEnemies = 8;
                    break;
                case 2:
                    MaxEnemies = 15;
                    break;
                case 3:
                    MaxEnemies = 24;
                    break;
            }
            Difficulty = Diff;
        }

        private int Difficulty;
        public EntityContainer<Enemy> Enemies {get; set;} = new EntityContainer<Enemy>();
        public int MaxEnemies {get; set;}
        public void CreateEnemies(List<Image> enemyStrides)
        {
            switch (Difficulty) {
                case 0:
                    for (float i = (0.9f/3f); i < (0.9f/3f)+(2f*0.13f); i += 0.13f) {
                       var enemy = new Enemy(
                                new DynamicShape(new Vec2F(i, 0.9f), new Vec2F(0.1f, 0.1f)),
                                new Image(Path.Combine("Assets", "Images", "BlueMonster.png")),
                                new Vec2F(i, 0.9f));
                            Enemies.AddDynamicEntity(enemy); 
                    }
                    break;
                case 1:
                    for (float x = (0.95f/4f); x <= ((0.95f/4f) + (3*0.13f)); x += 0.13f) {
                        for (float y = 0.9f; y >= 0.9f - 0.13f; y -= 0.13f) {
                            var enemy = new Enemy(
                                new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.1f)),
                                new Image(Path.Combine("Assets", "Images", "BlueMonster.png")),
                                new Vec2F(x, y));
                            Enemies.AddDynamicEntity(enemy);
                        }
                    }
                    break;
                case 2:
                    for (float x = (0.9f/5f); x <= ((0.9f/5f)+(4f*0.13f)); x += 0.13f) {
                        for (float y = 0.9f; y >= (0.9f - (2f*0.13f)); y -= 0.13f) {
                            var enemy = new Enemy(
                                new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.1f)),
                                new Image(Path.Combine("Assets", "Images", "BlueMonster.png")),
                                new Vec2F(x, y));
                            Enemies.AddDynamicEntity(enemy);
                        }
                    }
                    break;
                case 3:
                    for (float x = 0.1f; x <= 0.85f; x += 0.13f) {
                        for (float y = 0.9f; y >= (0.9f - (3f*0.13f)); y -= 0.13f) {
                            var enemy = new Enemy(
                                new DynamicShape(new Vec2F(x, y), new Vec2F(0.1f, 0.1f)),
                                new Image(Path.Combine("Assets", "Images", "BlueMonster.png")),
                                new Vec2F(x, y));
                            Enemies.AddDynamicEntity(enemy);
                        }
                    }
                    break;

            }
        }
    }
}
