﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Core
{
    struct Vector2
    {
        public int X, Y;

        public Vector2(int x = 0, int y = 0)
        {
            (X, Y) = (x, y);
        }
    }
}