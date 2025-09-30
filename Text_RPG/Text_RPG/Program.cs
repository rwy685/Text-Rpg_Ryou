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
                        return 2;
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
        public enum StatType
        {
            Attack,
            Defense
        }
        public class Item // 배열형, 리스트로 다시 작업. -> items 
        {
            public string Name { get; set; }
            public StatType Type { get; set; }   // 공격력 / 방어력 구분
            public int Value { get; set; }       // 수치 (예: +5)
            public string Description { get; set; }

            public Item(string name, StatType type, int value, string description)
            {
                Name = name;
                Type = type;
                Value = value;
                Description = description;
            }

            public void DisplayInfo()
            {
                Console.WriteLine($"{Name} | {Type} +{Value} | {Description}");
            }
            public List<Item> ItemList { get; private set; }
            public Item[] ItemArray => ItemList.ToArray();  // 필요하면 배열처럼 접근 가능

            public Item()
            {
                ItemList = new List<Item>
        {
            new Item("무쇠갑옷", StatType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다."),
            new Item("낡은 검", StatType.Attack, 2, "쉽게 볼 수 있는 낡은 검입니다."),
            new Item("연습용 창", StatType.Attack, 3, "검보다는 그대로 창이 다루기 쉽죠.")
        };
            }

            public void DisplayItemById(int id)
            {
                int index = id - 1;
                if (index >= 0 && index < ItemList.Count)
                    ItemList[index].DisplayInfo();
                else
                    Console.WriteLine("해당 아이템이 존재하지 않습니다.");
            }

            public void DisplayAllItems()
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    ItemList[i].DisplayInfo();
                }
            }

        }
     

        

        static void Main(string[] args)
        {
            Intro intro = new Intro();
            PlayerInfo playerinfo = new PlayerInfo();
            bool isGameStart = true;
            Item item = new Item();
            



            while (isGameStart) // if문으로 하자
            {
                
                int choice = intro.intromessage();
                
                if(choice == 1)
                {
                    bool inStat = true;
                    while (inStat)
                    {
                        int back = playerinfo.DisplayPinfo();
                        if(back == 0)
                        {
                            Console.Clear();
                            inStat = false; // intro로 돌아가기
                        }
                        else
                        {
                            
                            Console.Clear();
                            Console.WriteLine("=== 0을 눌러서 돌아가주세요. ===");
                        }
                    }
                }
                else if (choice == 2)
                {

                    item.DisplayAllItems();
                }

                else
                {
                    isGameStart = false;
                }

            }

        }
    }

    
}