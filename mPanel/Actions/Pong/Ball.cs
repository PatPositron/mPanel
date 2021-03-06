﻿using System;
using System.Drawing;
using mPanel.Matrix;
using mPanel.Extra;
using mPanel.Extra.Color;

namespace mPanel.Actions.Pong
{
    public class Ball
    {
        private static readonly Random Random = new Random();
        private readonly Frame Frame;
        private double DeltaV;

        public Color Fill { get; set; }
        public Direction Direction { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Ball(Frame frame, int x, int y)
        {
            Frame = frame;
            X = x;
            Y = y;

            Direction = new Direction(Random.NextDouble() > 0.5 ? 1 : -1, Random.NextDouble() > 0.5 ? 1 : -1);

            Randomize();
        }

        public void Randomize()
        {
            Fill = ColorHelper.HsvToColor(Random.NextDouble(), 1.0, 1.0);
            DeltaV = Random.Next(2);
        }

        public void Move()
        {
            if (Direction.DeltaX < 0)
                X = X - 1 - (int) (DeltaV / 2);
            else
                X = X + 1 + (int) (DeltaV / 2);

            if (Direction.DeltaY < 0)
                Y = Y - 1 - (int) (DeltaV / 2);
            else
                Y = Y + 1 + (int) (DeltaV / 2);
        }

        public void Draw()
        {
            using (var fill = new SolidBrush(Fill))
            {
                Frame.Graphics.FillRectangle(fill, X, Y, 1, 1);
            }
        }
    }
}
