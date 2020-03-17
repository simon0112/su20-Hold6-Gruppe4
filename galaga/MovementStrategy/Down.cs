using DIKUArcade.Entities;

namespace galaga.MovementStrategy {
    public class Down : IMovementStrategy {
        
        public float spd;
        public Down(float Spd) {
            spd = Spd;
        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().Direction.Y = -(spd);
            enemy.Shape.Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemyList) {
            foreach (Enemy enemy in enemyList) {
                enemy.Shape.AsDynamicShape().Direction.Y = -spd;
                MoveEnemy(enemy);
            }
        }
    }
}