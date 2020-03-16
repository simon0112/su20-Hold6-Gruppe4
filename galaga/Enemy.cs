using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using DIKUArcade.Math;
using System.Collections.Generic;
public class Enemy : Entity {

    public Vec2F startingPos{ private set; get;}

    public Vec2F PreviousPos;
    public Enemy(DynamicShape shape, IBaseImage image, Vec2F start) : base(shape, image) 
    {
          startingPos = start;
          startingPos = start;
    }   
}