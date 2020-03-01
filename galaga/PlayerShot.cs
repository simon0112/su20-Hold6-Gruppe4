using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using System;
using DIKUArcade.Math;
public class PlayerShot : Entity {
    public Entity Entity {get; private set;}
    public PlayerShot(DynamicShape shape, IBaseImage image) : base(shape, image) 
    {
        Entity = new Entity(shape, image);
        Direction(new Vec2F(0.0000f, 0.01f));
    }  
    public void Move()
    {
        Entity.Shape.Move();
    } 
    public void Direction(Vec2F dir)
    {
        Entity.Shape.AsDynamicShape().ChangeDirection(dir);

    }
}

