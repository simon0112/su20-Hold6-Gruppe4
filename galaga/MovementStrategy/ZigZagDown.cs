using DIKUArcade.Entities;
using System;


namespace galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy{

        public void UpdateDir(Enemy enemy) {
            
        }

        public void MoveEnemy(Enemy enemy) {
            UpdateDir(enemy);
            enemy.Shape.Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemyList) {
            foreach (Enemy enemy in enemyList) {
                MoveEnemy(enemy);
            }
        }
    }
}