using DIKUArcade.Entities;

namespace galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        public Down() {

        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Direction.Y = -0.003f;
            enemy.Shape.Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemyList) {
            foreach (Enemy enemy in enemyList) {
                enemy.Shape.AsDynamicShape().Direction.Y = -0.03f;
                MoveEnemy(enemy);
            }
        }
    }
}