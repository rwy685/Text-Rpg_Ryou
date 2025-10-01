using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TextRPG;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace TextRPG // 공유를 위한 클래스 써보기
{
    public class Intro
    {
        public int intromessage()
        {

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n" +
                              "이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n " +
                              "1. 상태보기 \n 2. 인벤토리");
            Console.WriteLine("원하시는 행동을 입력해주세요. >>");

            int input;
            if (!int.TryParse(Console.ReadLine(), out input))
            {
                input = -1;
            }
            return input;

        }

    }

    public class PlayerInfo
    {
        public int level = 10;
        public string name = "Carl";
        public string job = "Warrior";
        public int baseAttack = 5;
        public int baseDefense = 2;
        public int attack;
        public int defense;
        public int hp = 100;
        public int gold = 1000;

        public PlayerInfo() // 기본 공격력과 추가된 총 공격력 구별
        {
            attack = baseAttack;
            defense = baseDefense;
        }

        public void UpdateEffect(Item[] items) // 아이템 정보 능력치에 더해주기
        {
            attack = baseAttack;
            defense = baseDefense;

            foreach (Item item in items)
            {
                if (item.IsEquipped)
                {
                    if (item.Type == StatType.Attack)
                    {
                        attack += item.Effect;
                    }
                    else if (item.Type == StatType.Defense)
                    {
                        defense += item.Effect;
                    }
                }

            }
        }

        public void DisplayPinfo()
        {
            bool keep = true;
            while (keep)
            {
                Console.WriteLine($"\nLV.{level}\n{name}({job})\n공격력: {attack}\n방어력:" +
                                  $"{defense}\n체력: {hp}\nGold: {gold}\n\n" +
                                  $"0. 나가기\n원하시는 행동을 입력해주세요.\n>>");
                int input;
                if (int.TryParse(Console.ReadLine(), out input) && input == 0)
                {
                    keep = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("===0을 입력해서 intro 화면으로 가주세요===");
                }

            }

        }
    }
    public enum StatType
    {
        Attack,
        Defense
    }
    public class Item // 배열형으로 
    {
        public string Name { get; set; }
        public StatType Type { get; set; }
        public int Effect { get; set; }
        public string Description { get; set; }

        public bool IsEquipped { get; set; }

        public Item(string name, StatType type, int effect, string description) //Main에서 초기화하기
        {
            this.Name = name;
            this.Type = type;
            this.Effect = effect;
            this.Description = description;
        }

        public void DisplayItem()
        {
            string Emark = IsEquipped ? "[E]" : ""; // [E] 마크 생성 로직
            Console.WriteLine($"{Emark}{Name} | {Type} + {Effect} | {Description}"); // 참일시 Name 앞에 추가

        }

    }
    public class Inventory
    {
        public void DisplayInventory(Item[] items, PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.WriteLine("\n인벤토리\n보유 중인 아이템을 관리할 수 있습니다.");
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].DisplayItem();
                }

                Console.WriteLine("\n1.장착 관리\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");
                int input;

                if (int.TryParse(Console.ReadLine(), out input) && input == 0)
                {
                    keep = false;
                }
                else if (input == 1)
                {
                    Equipment equipment = new Equipment();
                    equipment.Equip(items, player);

                }
                else
                {
                    Console.WriteLine("===잘못된 입력입니다.===");
                    Console.ReadLine();
                }
                Console.Clear();

            }
        }
    }

    public class Equipment
    {
        public void Equip(Item[] items, PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착관리\n보유 중인 아이템을 관리할 수 있습니다.\n\n[아이템 목록]");

                for (int i = 0; i < items.Length; i++)
                {
                    Console.Write($"{i + 1}. ");
                    items[i].DisplayItem();
                }
                Console.WriteLine("\n원하시는 아이템 번호를 입력해주세요.\n\n0. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
                int input;
                int.TryParse(Console.ReadLine(), out input);
                if (input == 0)
                {
                    keep = false;
                }
                else if (input >= 1 && input <= items.Length)
                {
                    Item select = items[input - 1];

                    // [E] 출력
                    if (select.IsEquipped == false)
                    {
                        select.IsEquipped = true;

                    }
                    else if (select.IsEquipped == true)
                    {
                        select.IsEquipped = false;
                    }
                    player.UpdateEffect(items);
                }
            }

        }
    }

    public class Game
    {
        static void Main(string[] args)
        {
            Intro intro = new Intro();
            PlayerInfo playerinfo = new PlayerInfo();
            Inventory inventory = new Inventory();
            Equipment equipment = new Equipment();
            bool isGameStart = true;

            Item[] item = new Item[]  //아이템 배열화
            {
                    new Item("무쇠갑옷", StatType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다."),
                    new Item("낡은 검", StatType.Attack, 2, "쉽게 볼 수 있는 낡은 검 입니다."),
                    new Item("연습용 창", StatType.Attack, 3, "검보다는 그대로 창이 다루기 쉽죠.")
            };

            while (isGameStart)
            {
                Console.Clear();
                int choice = intro.intromessage();

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("상태보기를 선택했습니다.");
                        playerinfo.DisplayPinfo();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("인벤토리를 선택했습니다.");
                        inventory.DisplayInventory(item, playerinfo);
                        break;
                    default:
                        Console.WriteLine("해당하는 숫자를 입력해주세요");
                        Console.ReadLine();
                        break;
                }
            }


        }
    }









}

