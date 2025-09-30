using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

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

        public int DisplayPinfo()
        {
            Console.WriteLine($"LV.{level}\n{name}({job})\n공격력: {attack}\n방어력:" +
                              $"{defense}\n체력: {hp}\nGold: {gold}\n\n" +
                              $"0. 나가기\n원하시는 행동을 입력해주세요.\n>>");
            int input = int.Parse(Console.ReadLine());
            return input;
        }
        public class Items
        {
            string[] itemname = new string[3];
            int[] effect = new int[3];
            string[] type = new string[3];
            string[] Description = new string[3];

            public Items()
            {
                itemname[0] = "무쇠갑옷";
                effect[0] = 5;
                type[0] = "방어력";
                Description[0] = "무쇠로 만들어져 튼튼한 갑옷입니다.";

                itemname[1] = "낡은 검";
                effect[1] = 2;
                type[1] = "공격력";
                Description[1] = "쉽게 볼 수 있는 낡은 검 입니다.";

                itemname[2] = "연습용 창";
                effect[2] = 3;
                type[2] = "공격력";
                Description[2] = "검보다는 그대로 창이 다루기 쉽죠.";
            }

            public void DisplayItem(int index)
            {
                Console.WriteLine($"{itemname[index]} | {type[index]} + {effect[index]} | {Description[index]}");
            }
        }
     

        

        static void Main(string[] args)
        {
            Intro intro = new Intro();
            PlayerInfo playerinfo = new PlayerInfo();
            bool isGameStart = true;
            Items item = new Items();
            



            while (isGameStart)
            {
                
                int choice = intro.intromessage();
                
                switch (choice)
                {

                    case 1:
                        int back = playerinfo.DisplayPinfo();
                        if (back == 0)
                        {
                            Console.Clear();
                            continue; // Intro 메뉴 다시 출력
                        }
                        else
                        {
                            Console.WriteLine("숫자를 다시 입력해주세요");
                        }
                        break;

                    case 2:
                        item.DisplayItem(1);
                        break;

                    default:
                        Console.WriteLine("숫자를 다시 입력해주세요");
                        break;
                }

            }

        }
    }

    
}