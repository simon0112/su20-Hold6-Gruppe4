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

        public Square(EntityContainer<Enemy> enemyList, float speed) 
        {
            
        }
        private EntityContainer<Enemy> Enemies() 
        {   
            EntityContainer<Enemy> enemies = new EntityContainer<Enemy>();
            return enemies;
        }
        private int MaxEnemies() 
        { 
            return 2;
        }
        public void CreateEnemies(List<Image> enemyStrides)
        {

        }
    }
}
