using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using System;
using DIKUArcade.Math;

public class Player : IGameEventProcessor<object> 
{
    public Entity Entity {get; private set;}
    public Player(DynamicShape shape, IBaseImage image) 
    {
        Entity = new Entity(shape, image);
    }
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
        throw new NotImplementedException();  
    }
    public void Direction(Vec2F dir)
    {
        Entity.Shape.AsDynamicShape().ChangeDirection(dir);

    }
    
    public void Move()
    {
        if (Entity.Shape.Position.X > 0f && Entity.Shape.Position.X < 0.9f) 
        {
            Entity.Shape.Move();
        } else if (Entity.Shape.Position.X <= 0f && Entity.Shape.AsDynamicShape().Direction.X > 0f) 
        {
            Entity.Shape.Move();
        } else if (Entity.Shape.Position.X >= 0.9f && Entity.Shape.AsDynamicShape().Direction.X < 0f) 
        {
            Entity.Shape.Move();
        }
    }

}
   
