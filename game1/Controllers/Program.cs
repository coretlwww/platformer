﻿using System;

namespace game1.source;
public static class Program
{
    [STAThread]
    static void Main()
    {
        var game = new Game1();
        game.Run();
    }
}