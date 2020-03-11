using DIKUArcade.Entities;
using System.Collections.Generic;

namespace galaga.MovementStrategy {
    public class NoMove : IMovementStrategy {
        public NoMove(List<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                enemy.Shape.AsDynamicShape().Direction.X = 0f;
                enemy.Shape.AsDynamicShape().Direction.Y = 0f;
            }
        }

        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemyList) {
            foreach (Enemy enemy in enemyList) {
                MoveEnemy(enemy);
            }
        }
    }
}