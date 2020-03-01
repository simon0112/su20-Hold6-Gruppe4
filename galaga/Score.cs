using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using System.IO;
using System;
using System.Collections.Generic;
using DIKUArcade.Physics;

public class Score
{
    private int score;
    private Text display;
    public Score(Vec2F position, Vec2F extent) 
    {
       score = 0;
       display = new Text(score.ToString(), position, extent);
    }
    public void AddPoint()
    {
       score++;
    }
    public void RenderScore() 
    {
       display.SetText(string.Format("Score: {0}", score.ToString()));
       display.SetColor(new Vec3I(255, 0, 0));
       display.RenderText();
    }

}