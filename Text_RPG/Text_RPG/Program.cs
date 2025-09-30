using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;

namespace TextRPG
{
    public class Intro
    {
        public int intromessage()
        {

            while (true)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n" +
                                  "이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n " +
                                  "1. 상태보기 \n 2. 인벤토리");
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");

                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("상태보기를 선택했습니다.\n");
                        return 1;
                    case 2:
 
                        Console.Clear();
                        Console.WriteLine("인벤토리를 선택했습니다.\n");
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.Clear();
                        break;
                }
            }

        }

    }
    public class PlayerInfo
    {
        public int level = 10;
        public string name = "Carl";
        public string job = "Warrior";
        public int attack = 5;
        public int defense = 2;
        public int hp = 100;
        public int gold = 1000;

        public void DisplayPinfo()
        {
            Console.WriteLine($"LV.{level}\n{name}({job})\n공격력: {attack}\n방어력:" +
                              $"{defense}\n체력: {hp}\nGold: {gold}\n\n" +
                              $"0. 나가기\n원하시는 행동을 입력해주세요.\n>>");
            int input = int.Parse(Console.ReadLine());
            if (input == 0)
            {
                
            }
        }
        static void Main(string[] args)
        {
            Intro intro = new Intro();
            PlayerInfo playerinfo = new PlayerInfo();
            bool isGameStart = true;
   

            while (isGameStart)
            {
                
                int choice = intro.intromessage();

                switch (choice)
                {
                    case 0: Console.Clear();
                        break; // 다시 intro 실행

                    case 1:
                        playerinfo.DisplayPinfo();
                        break;

                    case 2:
                        Console.WriteLine("인벤 없음");
                        break;

                    default:
                        isGameStart = false;
                        break;
                }

            }

        }
    }

    
}