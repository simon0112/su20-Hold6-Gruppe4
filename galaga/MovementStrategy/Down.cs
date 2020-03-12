using DIKUArcade.Entities;

namespace galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        public Down(EntityContainer<Enemy> enemyList, float speed) {
            foreach (Enemy enemy in enemyList) {
                enemy.Shape.AsDynamicShape().Direction.Y = speed;
            }
            MoveEnemies(enemyList);
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