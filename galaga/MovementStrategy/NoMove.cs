using DIKUArcade.Entities;
using System.Collections.Generic;

namespace galaga.MovementStrategy {
    public class NoMove : IMovementStrategy {
        public NoMove() {
            
        }

        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Direction.X = 0f;
            enemy.Shape.AsDynamicShape().Direction.Y = 0f;
            enemy.Shape.Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemyList) {
            foreach (Enemy enemy in enemyList) {
                enemy.Shape.AsDynamicShape().Direction.X = 0f;
                enemy.Shape.AsDynamicShape().Direction.Y = 0f;
                MoveEnemy(enemy);
            }
        }
    }
}