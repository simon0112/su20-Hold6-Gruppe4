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
    public class Square : ISquadron
    {
        public Square(int Diff) 
        {
            switch (Diff) {
                case 0:
                    MaxEnemies = 1;
                    break;
                case 1:
                    MaxEnemies = 4;
                    break;
                case 2:
                    MaxEnemies = 9;
                    break;
                case 3:
                    MaxEnemies = 16;
                    break;
            }
            Difficulty = Diff;
        }

        private int Difficulty;
        public EntityContainer<Enemy> Enemies {get; set;}
        public int MaxEnemies {get; set;}
        public void CreateEnemies(List<Image> enemyStrides)
        {
            
        }
    }
}
