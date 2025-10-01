using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace TextRPG // 공유를 위한 클래스 써보기
{
    public class Intro
    {
        public int intromessage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================================");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n" +
                              "이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n " +
                              "1. 상태보기 \n 2. 인벤토리\n 3. 랜덤 모험");
            Console.WriteLine("원하시는 행동을 입력해주세요. >>");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================================");
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
        public int exp = 0;
        public int stamina = 20;

        public PlayerInfo() // 기본 공격력과 추가된 총 공격력 구별
        {
            attack = baseAttack;
            defense = baseDefense;
        }

        public void UpdateEffect(List<Item> items) // 아이템 정보 능력치에 더해주기
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
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nLV.{level}\n{name}({job})\n공격력: {attack}\n방어력:" +
                                  $"{defense}\n체력: {hp}\nGold: {gold}\n경험치: {exp}\n스태미나:{stamina}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=======================================================");
                Console.WriteLine($"0. 나가기\n원하시는 행동을 입력해주세요.\n>>");
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
            Console.WriteLine($"\n{Emark}{Name} | {Type} + {Effect} | {Description}"); // 참일시 Name 앞에 추가

        }

    }
    public class Inventory
    {
        public List<Item> Items { get; private set; }

        public void InitializeItem()
        {
            Items = new List<Item>()
            {

                new Item("무쇠갑옷", StatType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다."),
                new Item("낡은 검", StatType.Attack, 2, "쉽게 볼 수 있는 낡은 검 입니다."),
                new Item("연습용 창", StatType.Attack, 3, "검보다는 그대로 창이 다루기 쉽죠."),
                new Item("마법 지팡이", StatType.Attack, 4, " 마법으로 된 화살이 나가는 지팡이입니다."),

            };
        }
        public void DisplayInventory(PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n인벤토리\n보유 중인 아이템을 관리할 수 있습니다.");
                Console.ResetColor();
                foreach( var item in Items)
                {
                    item.DisplayItem();
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n1.장착 관리\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");
                int input;

                if (int.TryParse(Console.ReadLine(), out input) && input == 0)
                {
                    keep = false;
                }
                else if (input == 1)
                {
                    new Equipment().Equip(Items, player);
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
        public void Equip(List<Item> items, PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("인벤토리 - 장착관리\n\n[아이템 목록]");
                Console.ResetColor();

                for (int i = 0; i < items.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    items[i].DisplayItem();
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n원하시는 아이템 번호를 입력해주세요.\n\n0. 나가기\n>>");
                int input;
                int.TryParse(Console.ReadLine(), out input);
                if (input == 0)
                {
                    keep = false;
                }
                else if (input >= 1 && input <= items.Count)
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
    public enum EventType
    {
        Monster,
        Nothing,
        MeetNpc
    }

    public class Event
    {
        public int Gold{  get; set; }
        public int Exp { get; set; }
        public int Effect{  get; set; }
        public int Stamina { get; set; }
        public EventType EType { get; set; }



        public Event(int gold, int exp, int effect, int stamina, EventType etype)
        {
            this.Gold = gold;
            this.Exp = exp;
            this.Effect = effect;
            this.Stamina = stamina;
            this.EType = etype;
           
        }

        public void DisplayEvent()
        {
            switch (EType)
            {
                case EventType.Monster:
                Console.WriteLine($"\n골드 {Gold} 획득! | 경험치 {Exp} 획득!");
                    break;
                case EventType.Nothing:
                    Console.WriteLine("\n아무 일도 일어나지 않았다");
                    break;
            }
        }

    }

    public class Adventure
    {
        private List<Event> events;
        private Random random = new Random();

        public void EventInitialized()
        {
            events = new List<Event>()
            {
                new Event(500, 10, 0, -10, EventType.Monster),
                new Event(0, 0, 0, -10, EventType.Nothing),
            };
        }

        public void AdvStart(PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n랜덤 모험 시 스태미나를 소비합니다");
                Console.ResetColor();

                
                if(player.stamina < 10)
                {
                    Console.WriteLine("!!! 스태미나가 부족해서 모험을 할 수 없습니다.");
                    Console.Write("아무키나 누르면 메인메뉴로 돌아갑니다.");
                    Console.ReadKey();
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n1.모험 진행");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");
                Console.ResetColor();


                int input;
                int.TryParse(Console.ReadLine(), out input);
                if (input == 0)
                {
                    keep = false;
                }
                else if(input == 1)
                {
                    int index = random.Next(events.Count);
                    Event e = events[index];

                    player.stamina += e.Stamina;
                    player.gold += e.Gold;
                    if (player.stamina < 0) player.stamina = 0; // 

                    Console.Clear();
                    e.DisplayEvent();
                    Console.WriteLine("\n아무 키나 눌러 계속");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("===잘못된 입력입니다.===");
                    Console.ReadKey();
                }
                Console.Clear();

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
            Adventure adventure = new Adventure();
            adventure.EventInitialized();
            inventory.InitializeItem();
            bool isGameStart = true;

   
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
                        inventory.DisplayInventory(playerinfo);
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("랜덤 모험을 시작합니다");
                        adventure.AdvStart(playerinfo);
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

