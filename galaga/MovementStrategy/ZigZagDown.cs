using DIKUArcade.Entities;
using System;


namespace galaga.MovementStrategy {
    public class ZigZagDown : IMovementStrategy{
        public float spd;
        public ZigZagDown(float Spd) {
            spd = Spd;
        }
        public void UpdateDir(Enemy enemy) {
            var newYPos = enemy.Shape.Position.Y - spd;
            float newXPos = (float)(enemy.startingPos.X + (0.05f*Math.Sin((2*Math.PI*(enemy.startingPos.Y-newYPos))/0.045f)));
            enemy.Shape.Position.Y = newYPos;
            enemy.Shape.Position.X = newXPos;
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