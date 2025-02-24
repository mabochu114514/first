namespace GreedySnack
{
    struct pos
    {
        public int x;
        public int y;
        public pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    enum E_Drection
    {
        Left,
        Right,
        Up,
        Down,
    }
    class Snake
    {
        public  static E_Drection direct=E_Drection.Down;
       
        public static pos[] Snakepos=new pos[1000];
        public static int SnakeLength=1;
        public static void SnakeDraw()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < SnakeLength; i++)
            {
                Console.SetCursorPosition(Snakepos[i].x, Snakepos[i].y);
                Console.Write("□");
            }
        }
        
        public static void Turn()
        {
            if (Console.KeyAvailable) {
                char dir = Console.ReadKey(true).KeyChar;
                switch (dir)
                {
                    case 'a':
                        direct = E_Drection.Left;
                        break;
                    case 'd':
                        direct = E_Drection.Right;
                        break;
                    case 'w':
                        direct = E_Drection.Up;
                        break;
                    case 's':
                        direct = E_Drection.Down;
                        break;
                }
            }
        }
        public static bool Eat(pos fdp)
        {
            if (Snakepos[0].x == fdp.x && Snakepos[0].y==fdp.y)
            {
                SnakeLength++;
                return true;
            }
            return false;
        }
        public static void Extend(pos fdp)
        {
            Snakepos[SnakeLength - 1].x = fdp.x;
            Snakepos[SnakeLength-1].y = fdp.y;
        }
        public static void Move()
        {
            Console.SetCursorPosition(Snakepos[SnakeLength - 1].x, Snakepos[SnakeLength - 1].y);
            Console.Write("  ");
            for (int i = SnakeLength - 1; i>0; i--)
            {
                Snakepos[i]= Snakepos[i-1];
            }
            switch (direct)
            {
                case E_Drection.Left:
                    Snakepos[0].x =Snakepos[0].x - 2;
                    break;
                    case E_Drection.Right:
                    Snakepos[0].x = Snakepos[0].x+2;
                    break;
                    case E_Drection.Up:
                    Snakepos[0].y = Snakepos[0].y -1;
                    break;
                    case E_Drection.Down:
                    Snakepos[0].y = Snakepos[0].y + 1;
                    break;
            }
            SnakeDraw();
        }
        public static bool IsCover()
        {
            bool b=false;
            for(int i = 1; i<SnakeLength; i++)
            {
                if (Snakepos[0].x == Snakepos[i].x&& Snakepos[0].y == Snakepos[i].y)
                {
                    b= true; break;
                }
            }
            return b;
        }
        public static bool IsCollison(int w,int h)
        {
            
            if(Snakepos[0].x==0|| Snakepos[0].x == w - 2)
            {
                return true;
            }
            if (Snakepos[0].y == 0 || Snakepos[0].y == h - 1)
            { 
                return true;
            }
            if (IsCover())
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
    class food
    {
        public pos P;
      
        public string icon="fd";
        public static pos Generate(int w,int h)
        {
            Random r = new Random();
            bool isRepeat=true;
            int x = r.Next(2, w - 4);
            int y = r.Next(2, h - 3);
            
            while (isRepeat)
            {
                isRepeat = false;
                x = r.Next(2, w - 4);
                y = r.Next(2, h - 3);
                for (int i = 0; i < Snake.SnakeLength; i++)
                {
                    if ((x == Snake.Snakepos[i].x && y == Snake.Snakepos[i].y)||x%2!=0||y%2!=0)
                    {
                        isRepeat = true;
                    }
                }
            }
                
            pos P = new pos(x,y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(x, y);
            Console.Write("fd");
            return P;
        }
        
    }
    internal class Program
    {
        enum E_scene
        {
            Start,
            Game,
            End,
        }
       
        static void DrawMap(int w,int h)
        {
            int i;
            Console.ForegroundColor = ConsoleColor.Red;
            for (i = 0; i < w - 2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
            }
            for (i = 0; i < w - 2; i++)
            {
                Console.SetCursorPosition(i, h-1);
                Console.Write("■");
            }
            for (i = 0; i < h; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }
            for (i = 0; i < h; i++)
            {
                Console.SetCursorPosition(w-2, i);
                Console.Write("■");
            }
        }
        static void GamePerform(int w,int h,ref E_scene Scene)
        {
            DrawMap(w, h);
            pos pp = new pos();
            pp = food.Generate(w, h);
            Snake.Snakepos[0].x = 6;
            Snake.Snakepos[0].y = 6;
            Snake.SnakeDraw();
            while (true)
            {
                Thread.Sleep(160);
                Snake.Turn();
                Snake.Move();
                if (Snake.Eat(pp))
                {
                    Snake.Extend(pp);
                    pp=food.Generate(w, h);
                }
                else if(Snake.IsCollison(w, h))
                {
                    Scene = E_scene.End;
                    break;
                }
            }
        }
        static void GameEnd(int w,int h,ref E_scene sceneType)
        {

            Console.SetWindowSize(w, h);
            Console.SetBufferSize(w, h);
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(w / 2 - 4, 12);
            Console.Write("重新游玩");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(w / 2 - 4, 14);
            Console.Write("结束游戏");
            int index = 1;
            bool isquit = false;
            while (true)
            {
                char c = Console.ReadKey(true).KeyChar;
                switch (c)
                {
                    case 'w':
                        index = 1;
                        break;
                    case 's':
                        index = 0;
                        break;
                    case 'j':
                        if (index == 0)
                        {
                            Environment.Exit(0);
                        }
                        if (index == 1)
                        {
                            sceneType = E_scene.Start;
                            isquit = true;
                        }
                        break;

                }
                Console.ForegroundColor = index == 1 ? ConsoleColor.Red : ConsoleColor.White;
                Console.SetCursorPosition(w / 2 - 4, 12);
                Console.Write("重新游玩");
                Console.ForegroundColor = index == 1 ? ConsoleColor.White : ConsoleColor.Red;
                Console.SetCursorPosition(w / 2 - 4, 14);
                Console.Write("结束游戏");
                if (isquit)
                {
                    break;
                }
            }
        }
        static void GameStart(int w,int h,ref E_scene sceneType)
        {
            Console.SetWindowSize(w,h);
            Console.SetBufferSize(w,h);
            Console.SetCursorPosition(w / 2 - 3, 5);
            Console.Write("贪吃蛇");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(w / 2 - 4, 12);
            Console.Write("开始游戏");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(w / 2 - 4, 14);
            Console.Write("结束游戏");
            int index = 1;
            bool isquit = false;
            while (true)
            {
                char c=Console.ReadKey(true).KeyChar;
                switch(c)
                {
                    case 'w':
                        index = 1;
                        break;
                    case 's':
                        index = 0;
                        break;
                    case 'j':
                        if (index == 0)
                        {
                            Environment.Exit(0);
                        }
                        if (index == 1)
                        {
                            sceneType = E_scene.Game;
                            isquit = true;
                        }
                        break;

                }
                Console.ForegroundColor = index==1?ConsoleColor.Red:ConsoleColor.White;
                Console.SetCursorPosition(w / 2 - 4, 12);
                Console.Write("开始游戏");
                Console.ForegroundColor = index == 1 ? ConsoleColor.White: ConsoleColor.Red;
                Console.SetCursorPosition(w / 2 - 4, 14);
                Console.Write("结束游戏");
                if (isquit)
                {
                    break;
                }
            }
            
        }
        static void Main(string[] args)
        {
           E_scene SceneType = new E_scene();
            Console.CursorVisible= false;
            int w=60; int h=30;

            while (true)
            {
                switch (SceneType)
                {
                    case E_scene.Start:
                        GameStart(w,h,ref SceneType);
                        break;
                        case E_scene.Game:
                        Console.Clear();
                        GamePerform(w, h,ref SceneType);
                        
                        break;
                        case E_scene.End:
                        Console.Clear();
                        GameEnd(w,h,ref SceneType);
                        break;

                }
            }

           
        }
    }
}
