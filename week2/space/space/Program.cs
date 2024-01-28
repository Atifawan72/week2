using System;
using System.Collections.Generic;
using System.Threading;

public class Program
{
    public const int Height = 30;
    public const int Width = 100;
    public const int PlayerSize = 3;
    public const int EnemySize = 3;
    const int MaxBullets = 10;
    const int BulletSpeed = 1;
    const char PlayerSymbol = '@';
    const char EnemySymbol = 'E';

    static void Main()
    {
        Player player = new Player(PlayerSymbol, Width / 2 - PlayerSize / 2, Height - 5);
        List<Enemy> enemies = new List<Enemy>();
        enemies.Add(new Enemy(EnemySymbol, Width / 2 - EnemySize / 2, 2));
        enemies.Add(new Enemy(EnemySymbol, Width / 2 - EnemySize / 2, 6));

        char[,] canvas = new char[Height, Width];
        List<Bullet> bullets = new List<Bullet>();

        int shootCounter = 0;
        int enemyShootCounter = 0;
        int health = 10;
        int score = 0;

        DrawCanvas(canvas);
        StartScreen();

        while (true)
        {
            RedrawCanvas(canvas, health, score);
            RemoveCharacter(canvas, player.X, player.Y);

            foreach (var enemy in enemies)
            {
                if (enemy.Health > 0)
                {
                    RemoveCharacter(canvas, enemy.X, enemy.Y);
                }
            }

            Thread.Sleep(100);

            if (CheckBulletCollision(player, enemies, bullets))
            {
                --health;
            }

            if (health <= 0)
            {
                GameOverScreen();
                break;
            }

            foreach (var enemy in enemies)
            {
                if (CheckBulletCollision(player, enemies, bullets))
                {
                    score += 10;
                    --enemy.Health;
                }
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    ShootBullet(bullets, player.X + PlayerSize / 2, player.Y, ref shootCounter, 5);
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    player.MoveRight();
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    player.MoveLeft();
                }
            }

            DrawPlayer(canvas, player.X, player.Y);

            foreach (var enemy in enemies)
            {
                if (enemy.Health > 0)
                {
                    enemy.Patrol();
                    DrawEnemy(canvas, enemy.X, enemy.Y);
                    ShootBullet(bullets, enemy.X + EnemySize / 2, enemy.Y + EnemySize / 2, ref enemyShootCounter, 10);
                }
            }

            MoveBullets(canvas, bullets, BulletSpeed);

            if (health <= 0)
            {
                GameOverScreen();
                break;
            }

            if (enemies.TrueForAll(enemy => enemy.Health <= 0))
            {
                YouWinScreen();
                break;
            }
        }
    }

    static void RedrawCanvas(char[,] canvas, int health, int score)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"Health: {health} | Score: {score}");

        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                Console.Write(canvas[i, j]);
            }
            Console.WriteLine();
        }
    }

    static void DrawCanvas(char[,] canvas)
    {
        for (int i = 0; i < Height; ++i)
        {
            for (int j = 0; j < Width; ++j)
            {
                canvas[i, j] = ' ';
            }
        }

        for (int i = 0; i < Height; ++i)
        {
            canvas[i, 0] = '#';
            canvas[i, Width - 1] = '#';
        }

        for (int j = 0; j < Width; ++j)
        {
            canvas[0, j] = '#';
            canvas[Height - 1, j] = '#';
        }
    }

    static void DrawPlayer(char[,] canvas, int x, int y)
    {
        for (int i = 0; i < PlayerSize; i++)
        {
            for (int j = 0; j < PlayerSize; j++)
            {
                canvas[y + i, x + j] = '@';
            }
        }
    }

    static void DrawEnemy(char[,] canvas, int x, int y)
    {
        for (int i = 0; i < EnemySize; i++)
        {
            for (int j = 0; j < EnemySize; j++)
            {
                canvas[y + i, x + j] = 'E';
            }
        }
    }

    static void RemoveCharacter(char[,] canvas, int x, int y)
    {
        for (int i = 0; i < PlayerSize; i++)
        {
            for (int j = 0; j < PlayerSize; j++)
            {
                canvas[y + i, x + j] = ' ';
            }
        }
    }

    static bool CheckBulletCollision(Player player, List<Enemy> enemies, List<Bullet> bullets)
    {
        foreach (var bullet in bullets)
        {
            if (bullet.IsActive && bullet.X >= player.X && bullet.X < player.X + PlayerSize &&
                bullet.Y >= player.Y && bullet.Y < player.Y + PlayerSize)
            {
                return true;
            }

            foreach (var enemy in enemies)
            {
                if (bullet.IsActive && bullet.X >= enemy.X && bullet.X < enemy.X + EnemySize &&
                    bullet.Y >= enemy.Y && bullet.Y < enemy.Y + EnemySize)
                {
                    return true;
                }
            }
        }
        return false;
    }


    static void MoveBullets(char[,] canvas, List<Bullet> bullets, int speed)
    {
        foreach (var bullet in bullets)
        {
            if (bullet.IsActive)
            {
                RemoveCharacter(canvas, bullet.X, bullet.Y);
                bullet.MoveUp(speed);
                canvas[bullet.Y, bullet.X] = '.';
            }
        }
    }

    static void ShootBullet(List<Bullet> bullets, int x, int y, ref int shootCounter, int interval)
    {
        shootCounter++;
        if (shootCounter >= interval)
        {
            foreach (var bullet in bullets)
            {
                if (!bullet.IsActive)
                {
                    bullet.Activate(x, y);
                    shootCounter = 0;
                    break;
                }
            }
        }
    }

    static void StartScreen()
    {
        Console.Clear();
        Console.WriteLine("===============================");
        Console.WriteLine("      Welcome to Your Game      ");
        Console.WriteLine("===============================");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();
    }

    static void GameOverScreen()
    {
        Console.Clear();
        Console.WriteLine("===============================");
        Console.WriteLine("         Game Over!            ");
        Console.WriteLine("===============================");
    }

    static void YouWinScreen()
    {
        Console.Clear();
        Console.WriteLine("Congratulations! You won!");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}

public class Player
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Player(char symbol, int x, int y)
    {
        Symbol = symbol;
        X = x;
        Y = y;
    }

    public char Symbol { get; }

    public void MoveLeft()
    {
        if (X > 1)
        {
            X--;
        }
    }

    public void MoveRight()
    {
        if (X < Program.Width - Program.PlayerSize - 1)
        {
            X++;
        }
    }
}

public class Enemy
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Health { get; set; }
    public char Symbol { get; }

    public Enemy(char symbol, int x, int y)
    {
        Symbol = symbol;
        X = x;
        Y = y;
        Health = 5; // Example initial health
    }

    public void Patrol()
    {
        // Example patrol behavior
        // You can implement custom logic here
        if (X > 1 && X < Program.Width - Program.EnemySize - 1)
        {
            X += (X % 2 == 0) ? -1 : 1;
        }
    }
}

public class Bullet
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsActive { get; private set; }

    public void Activate(int x, int y)
    {
        X = x;
        Y = y;
        IsActive = true;
    }

    public void MoveUp(int speed)
    {
        if (Y > 1)
        {
            Y -= speed;
        }
        else
        {
            IsActive = false;
        }
    }
}
