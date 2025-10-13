using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Xml.Serialization;




namespace TextRPG // 공유를 위한 클래스 써보기
{
    public static class Global
    {
        public static Random rand = new Random();
    }

    public class Intro
    {
        public int intromessage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=======================================================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n" +
                              "이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n " +
                              "1. 상태보기 \n 2. 인벤토리\n 3. 랜덤 모험\n 4. 마을 순찰하기\n 5. 훈련하기\n 6. 상점\n 7. 던전입장\n 8. 휴식하기\n 9. 게임 저장");
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
        public int level = 1;
        public string name = "Carl";
        public string job = "Warrior";
        public double baseAttack = 5;
        public int baseDefense = 2;
        public double attack;
        public int defense;
        public int hp = 100;
        public int gold = 1000;
        public int exp = 0;
        public int stamina = 20;

        public int maxHp = 100;
        public int maxStamina = 100;

        private Dictionary<int, int> expTable = new Dictionary<int, int>()
    {
        {1, 50},
        {2, 80},
        {3, 150},
        {4, 500}
    };
        public PlayerInfo() // 기본 공격력과 추가된 총 공격력 구별
        {
            attack = baseAttack;
            defense = baseDefense;
        }

        public void AddExp(int gainedExp)
        {
            exp += gainedExp;
            Console.WriteLine($"\n경험치 {gainedExp} 획득! (현재 {exp} / 필요 {GetRequiredExp()})");

            while (true)
            {
                int requiredExp = GetRequiredExp();
                if (exp < requiredExp)
                    break;
                exp -= requiredExp;
                LevelUp();
            }
        }

        // ✅ 현재 레벨에서 필요 경험치 반환
        private int GetRequiredExp()
        {
            if (expTable.ContainsKey(level))
                return expTable[level];
            else
                return int.MaxValue; // 최고 레벨 도달 시 더 이상 레벨업 없음
        }
        public void LevelUp()
        {
            level++;
            maxHp += 20;
            maxStamina += 10;
            baseAttack += 0.5;
            baseDefense += 1;

            // 회복
            hp = maxHp;
            stamina = maxStamina;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n레벨 업! 현재 레벨: {level}");
            Console.WriteLine($"공격력 +0.5 → {baseAttack}, 방어력 +1 → {baseDefense}");
            Console.ResetColor();
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
                Console.WriteLine($"\nLV.{level}\n{name}({job})\n공격력: {attack:F1}\n방어력:" +
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

        public int Price { get; set; }

        public Item(string name, StatType type, int effect, string description, int price) //Main에서 초기화하기
        {
            Name = name;
            Type = type;
            Effect = effect;
            Description = description;
            Price = price;
        }

        public Item Clone() => new Item(Name, Type, Effect, Description, Price);


        public void DisplayItem()
        {
            string Emark = IsEquipped ? "[E]" : ""; // [E] 마크 생성 로직
            string statInfo = $"{Type} +{Effect}";
            Console.WriteLine($"\n{Emark}{Name,-10} | {statInfo,-10} | {Description,-30}"); // 참일시 Name 앞에 추가

        }

    }

    
    public class Inventory
    {
        public List<Item> Items { get;  set; }


        public Inventory()
        {
            InitializeItem();
        }
        public void InitializeItem()
        {
            Items = new List<Item>()
            {

                new Item("무쇠갑옷", StatType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 700),
                new Item("낡은 검", StatType.Attack, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600),
                new Item("연습용 창", StatType.Attack, 3, "검보다는 그대로 창이 다루기 쉽죠.", 800),
                new Item("마법 지팡이", StatType.Attack, 4, " 마법으로 된 화살이 나가는 지팡이입니다.", 1200),

            };
        }
        public void DisplayInventory(PlayerInfo player)
        {
            bool keep = true;
            while (keep)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n===== 인벤토리 =====");
                Console.ResetColor();

                Console.WriteLine($"{"이름",-20} | {"능력치",-18} | 설명");
                Console.WriteLine("------------------------------------------------------------");

                foreach (var item in Items)
                    item.DisplayItem();

                Console.WriteLine("------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("2. 이름 길이순 정렬");
                Console.WriteLine("3. 장착 순 정렬");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요. >>");
                Console.ResetColor();

                int input;
                if (!int.TryParse(Console.ReadLine(), out input))
                    input = -1;

                switch (input)
                {
                    case 0:
                        keep = false;
                        break;

                    case 1:
                        new Equipment().Equip(Items, player);
                        break;

                    case 2:
                        SortByNameLength();
                        break;

                    case 3:
                        SortByEquipped();
                        break;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void SortByNameLength()
        {
            // 이름 길이 내림차순 (긴 이름이 위로)
            Items = Items.OrderByDescending(i => i.Name.Length).ToList();
            Console.WriteLine("\n[이름 길이순 내림차순 정렬]");
            Console.ReadKey();
        }

        public void SortByEquipped()
        {
            // 장착된 아이템이 위로, 나머지는 아래로
            // 동일하면 이름순 정렬 
            Items = Items
                .OrderByDescending(i => i.IsEquipped)
                .ThenBy(i => i.Name)
                .ToList();

            Console.WriteLine("\n[장착된 아이템 우선 정렬]");
            Console.ReadKey();
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
                    if (!select.IsEquipped)
                    {
                        foreach (Item item in items)
                        {
                            if (item.IsEquipped && item.Type == select.Type)
                            {
                                item.IsEquipped = false;
                            }
                        }
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
        public int Gold { get; set; }
        public int Exp { get; set; }
        public int Effect { get; set; }
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


                if (player.stamina < 10)
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
                else if (input == 1)
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

    public class Patrol
    {
        public void PatrolVillage(PlayerInfo player)
        {
            player.stamina -= 5;
            if (player.stamina < 0)
            {
                Console.WriteLine("!!! 스태미나가 부족해서 모험을 할 수 없습니다.");
                Console.Write("아무키나 누르면 메인메뉴로 돌아갑니다.");
                Console.ReadKey();
                return;
            }
            int random = Global.rand.Next(0, 100);

            if (random < 10)
            {
                Console.WriteLine("\n마을 아이들이 모여있다. 간식을 사줘볼까? [500] G 소비");
                if (player.gold > 500)
                {
                    player.gold -= 500;
                }
                else if (player.gold < 500)
                {
                    Console.WriteLine("골드가 부족합니다!");
                }


            }
            else if (random < 20)
            {
                Console.WriteLine("\n촌장님을 만나서 심부름을 했다. [2000] G 획득");
                player.gold += 2000;

            }
            else if (random < 40)
            {
                Console.WriteLine("\n길 잃은 사람을 안내해주었다.[1000] G 획득");
                player.gold += 1000;

            }
            else if (random < 70)
            {
                Console.WriteLine("\n마을 주민과 인사를 나눴다.선물을 받았다. [500] G 획득 ");
                player.gold += 500;

            }
            else if (random < 100)
            {
                Console.WriteLine("\n아무 일도 일어나지 않았다");

            }

            if (player.stamina < 0)
                player.stamina = 0;

            // 결과 출력
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n현재 Gold: {player.gold}");
            Console.WriteLine($"현재 스태미나: {player.stamina}");
            Console.ResetColor();

            Console.WriteLine("\n아무 키나 누르면 메인 메뉴로 돌아갑니다...");
            Console.ReadKey();

        }
    }
    public class Store
    {
        public List<Item> Items { get; private set; }
        public void StoreInitialize()
        {
            Items = new List<Item>()
                {
                    // 인벤토리 아이템
                 new Item("무쇠갑옷", StatType.Defense, 5, "무쇠로 만들어져 튼튼한 갑옷입니다.", 700),
                 new Item("낡은 검", StatType.Attack, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600),
                 new Item("연습용 창", StatType.Attack, 3, "검보다는 그대로 창이 다루기 쉽죠.", 800),
                 new Item("마법 지팡이", StatType.Attack, 4, " 마법으로 된 화살이 나가는 지팡이입니다.", 1200),
                    // 신규 아이템

                new Item("수련자 갑옷", StatType.Defense, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("스파르타의 갑옷", StatType.Defense, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item("청동 도끼", StatType.Attack, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500),
                new Item("스파르타의 창", StatType.Attack, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.",2000),

                };
        }

        public void StoreBuy(PlayerInfo player, Inventory inventory)
        {
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===== 상점 - 구매 =====");
                Console.ResetColor();
                Console.WriteLine($"보유 Gold: {player.gold}\n");

                Console.WriteLine("번호 | 이름           | 능력치       | 설명                               | 가격/상태");
                Console.WriteLine("---------------------------------------------------------------------------------------");

                for (int i = 0; i < Items.Count; i++)
                {
                    var sItem = Items[i];
                    var ownedItem = inventory.Items.FirstOrDefault(it => it.Name == sItem.Name);
                    bool owned = ownedItem != null;
                    bool equipped = owned && ownedItem.IsEquipped;

                    string statInfo = $"{sItem.Type} +{sItem.Effect}";
                    string status = owned ? (equipped ? "[구매완료·장착중]" : "[구매완료]") : $"{sItem.Price} G";

                    Console.WriteLine($"{i + 1,3}. | {sItem.Name,-14} | {statInfo,-11} | {sItem.Description,-34} | {status}");
                }

                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("구매할 아이템 번호를 입력하세요. 0. 나가기");
                Console.Write(">> ");

                if (!int.TryParse(Console.ReadLine(), out int sel)) sel = -1;
                if (sel == 0) break;
                if (sel < 1 || sel > Items.Count)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                    continue;
                }

                var chosen = Items[sel - 1];

                // 이미 보유하면 구매 불가(“구매완료” 표기 유지)
                if (inventory.Items.Any(it => it.Name == chosen.Name))
                {
                    Console.WriteLine("이미 보유 중인 아이템입니다. 중복 구매는 불가합니다.");
                    Console.ReadKey();
                    continue;
                }

                if (player.gold < chosen.Price)
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    Console.ReadKey();
                    continue;
                }

                // 결제 & 인벤토리에 복제본 추가
                player.gold -= chosen.Price;
                inventory.Items.Add(chosen.Clone());  // 상점 리스트와 분리된 인스턴스
                inventory.Items[^1].IsEquipped = false;
                player.UpdateEffect(inventory.Items);

                Console.WriteLine($"'{chosen.Name}'을(를) 구매했습니다! 남은 Gold: {player.gold}");
                Console.WriteLine("아무 키나 누르면 계속...");
                Console.ReadKey();
            }
        }

        public void StoreSell(PlayerInfo player, Inventory inventory)
        {
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===== 상점 - 판매 (판매가는 상점가의 85%) =====");
                Console.ResetColor();
                Console.WriteLine($"보유 Gold: {player.gold}\n");

                if (inventory.Items.Count == 0)
                {
                    Console.WriteLine("(인벤토리에 아이템이 없습니다.)");
                    Console.WriteLine("\n0. 나가기");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("번호 | 이름           | 능력치       | 설명                               | 판매가");
                Console.WriteLine("---------------------------------------------------------------------------------------");

                for (int i = 0; i < inventory.Items.Count; i++)
                {
                    var it = inventory.Items[i];
                    string statInfo = $"{it.Type} +{it.Effect}";
                    int sellPrice = (int)(it.Price * 0.85);
                    string equipMark = it.IsEquipped ? "[E]" : "";
                    Console.WriteLine($"{i + 1,3}. | {equipMark}{it.Name,-14} | {statInfo,-11} | {it.Description,-34} | {sellPrice} G");
                }

                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("판매할 아이템 번호를 입력하세요. 0. 나가기");
                Console.Write(">> ");

                if (!int.TryParse(Console.ReadLine(), out int sel)) sel = -1;
                if (sel == 0) break;
                if (sel < 1 || sel > inventory.Items.Count)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();
                    continue;
                }

                var chosen = inventory.Items[sel - 1];

                if (chosen.IsEquipped)
                {
                    Console.WriteLine("장착 중인 아이템은 판매할 수 없습니다. 먼저 장착 해제하세요.");
                    Console.ReadKey();
                    continue;
                }

                int price = (int)(chosen.Price * 0.85);
                player.gold += price;
                inventory.Items.RemoveAt(sel - 1);
                player.UpdateEffect(inventory.Items);

                Console.WriteLine($"'{chosen.Name}'을(를) {price} G에 판매했습니다. 현재 Gold: {player.gold}");
                Console.WriteLine("아무 키나 누르면 계속...");
                Console.ReadKey();
            }
        }

        public void OpenStore(PlayerInfo player, Inventory inventory)
        {
            bool keep = true;
            while (keep)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===== 상 점 =====");
                Console.ResetColor();
                Console.WriteLine($"보유 Gold: {player.gold}\n");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요. >> ");

                if (!int.TryParse(Console.ReadLine(), out int input)) input = -1;

                switch (input)
                {
                    case 0: keep = false; break;
                    case 1: StoreBuy(player, inventory); break;
                    case 2: StoreSell(player, inventory); break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }




    public class Training
    {
        public void Ptraining(PlayerInfo player)
        {

            if (player.stamina < 15)
            {
                Console.WriteLine($"!!! 스태미나가 부족해서 훈련을 할 수 없습니다.\n\n [현재 스태미나: {player.stamina}]");
                Console.Write("아무키나 누르면 메인메뉴로 돌아갑니다.");
                Console.ReadKey();
                return;
            }
            player.stamina -= 15;
            int random = Global.rand.Next(0, 100);

            if (random < 15)
            {
                Console.WriteLine("훈련이 잘 되었습니다!");
                player.AddExp(60);
            }
            else if (random < 60)
            {
                Console.WriteLine("오늘 하루 열심히 훈련했습니다");
                player.AddExp(40);
            }
            else if (random < 25)
            {
                Console.WriteLine("하기 싫다... 훈련이...");
                player.AddExp(30);
            }

            if (player.stamina < 0)
                player.stamina = 0;

            // 결과 출력
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n현재 경험치: {player.exp}");
            Console.WriteLine($"현재 스태미나: {player.stamina}");
            Console.ResetColor();

            Console.WriteLine("\n아무 키나 누르면 메인 메뉴로 돌아갑니다...");
            Console.ReadKey();



        }
    }

    public class Rest
    {
        public void Recharge(PlayerInfo player)
        {

            bool isRest = true;
            while (isRest)
            {

                Console.WriteLine($"\n500 G를 내면 체력을 회복 할 수 있습니다. 보유골드: {player.gold}");
                Console.WriteLine($"\n현재 체력: {player.hp} 현재 스태미나: {player.stamina}\n1.휴식하기\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요\n >>");
                int input;
                string userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("잘못된 입력입니다. 0 또는 1을 입력해주세요!");
                    Console.WriteLine("아무키나 누르면 재시작합니다..");
                    Console.ReadKey();
                    continue; // 루프 재시작
                }

                switch (input)
                {
                    case 0:
                        isRest = false;
                        break;
                    case 1:
                        if (player.gold >= 500)
                        {
                            Console.Clear();
                            Console.WriteLine("휴식을 완료했습니다.");
                            player.gold -= 500;
                            player.hp += 100;
                            player.stamina += 20;
                            //최대치 제한
                            player.hp = Math.Min(player.hp, player.maxHp);
                            player.stamina = Math.Min(player.stamina, player.maxStamina);

                        }

                        else if (player.gold < 500)
                        {
                            Console.Clear();
                            player.gold = 0;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("골드가 부족합니다!");
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("잘못된 입력입니다. 0 또는 1을 입력해주세요!");
                        Console.WriteLine("아무키나 누르면 재시작합니다..");
                        Console.ReadKey();
                        break;


                }


            }
        }
    }
    public class Dungeon
    {
        public void EnterDungeon(PlayerInfo player)
        {
            bool isEnter = true;
            int random = Global.rand.Next(0, 100);
            while (isEnter)
            {
                Console.WriteLine($"이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다. 현재 Hp: {player.hp}");
                Console.WriteLine("\n1. 쉬운 던전    | 방어력 5 이상 권장");
                Console.WriteLine("\n2. 일반 던전    | 방어력 11 이상 권장");
                Console.WriteLine("\n3. 어려운 던전    | 방어력 17 이상 권장");
                Console.WriteLine("\n0. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
                int input;
                string userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("잘못된 입력입니다. 0 또는 1을 입력해주세요!");
                    Console.ReadKey();
                    continue; // 루프 재시작
                }
                if (player.hp < 20)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n체력이 너무 적어 던전을 탐험할 수 없습니다!");
                    Console.ResetColor();
                    Console.WriteLine("최소 20이상의 체력이 필요합니다.");
                    Console.WriteLine("휴식 후 다시 도전하세요.");
                    Console.ReadKey();
                    return; //메인 화면으로 복귀
                }
                switch (input)
                {
                    case 0:
                        isEnter = false;
                        break;
                    case 1:
                        TryDungeon(player, "쉬운 던전", 5, 1000, 50);
                        break;
                    case 2:
                        TryDungeon(player, "일반 던전", 11, 1700, 100);
                        break;
                    case 3:
                        TryDungeon(player, "어려운 던전", 17, 2500, 200);
                        break;
                }
            }
        }
        private void TryDungeon(PlayerInfo player, string dungeonName, int recommendedDefense, int rewardGold, int rewardExp)
        {

            Console.Clear();
            Console.WriteLine($"{dungeonName}에 입장합니다!\n");

            // 방어력이 권장치보다 낮다면 40% 확률로 실패
            if (player.defense < recommendedDefense)
            {
                int chance = Global.rand.Next(0, 100);
                if (chance < 40)
                {

                    FailDungeon(player, dungeonName);
                    return;
                }
            }

            // 위 조건에서 실패하지 않았다면 던전 성공 진행
            SuccessDungeon(player, recommendedDefense, dungeonName, rewardGold, rewardExp);
        }
        public void FailDungeon(PlayerInfo player, string dungeonName)
        {
            int beforeHp = player.hp;
            player.hp /= 2;
            if (player.hp < 0) player.hp = 0;


            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("던전 실패!");
            Console.ResetColor();

            Console.WriteLine($"{dungeonName} 탐험 중 몬스터에게 당했습니다...");
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {beforeHp} -> {player.hp}");

            Console.WriteLine("\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
            while (Console.ReadLine() != "0")
            {
                Console.WriteLine("잘못된 입력입니다. 0을 입력해주세요.");
            }
            Console.Clear();
        }

        public void SuccessDungeon(PlayerInfo player, int recommendedDefense, string dungeonName, int rewardgold, int rewardexp)
        {
            //데미지 계산식
            int beforeHp = player.hp;
            int beforeGold = player.gold;
            int beforeExp = player.exp;
            int beforeLevel = player.level;
            int defenseGap = recommendedDefense - player.defense;
            int minDamage = 20 + defenseGap;
            int maxDamage = 35 + defenseGap;

            if (minDamage < 5) minDamage = 5;
            if (maxDamage < minDamage) maxDamage = minDamage + 1;

            int damage = Global.rand.Next(minDamage, maxDamage + 1);
            player.hp -= damage;
            if (player.hp < 0)
            {
                player.hp = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("체력이 없어서 더이상 던전을 탐험 할 수 할 수 없습니다.");
                return;
            }

            //보상 식
            int baseReward = rewardgold;
            int baseExp = rewardexp;
           

            // === 공격력 기반 추가 보상 ===
            // 예: 공격력 10 → 10~20% 범위
            double minBonusRate = player.attack * 0.01;
            double maxBonusRate = player.attack * 0.02;

            // 예외처리 (보상 비율이 너무 높아지는 걸 방지)
            if (maxBonusRate > 1.0) maxBonusRate = 1.0; // 최대 +100% 제한

            double randomRate = Global.rand.NextDouble() * (maxBonusRate - minBonusRate) + minBonusRate;

            int totalReward = (int)(baseReward * (1 + randomRate));
            int totalExp = (int)(baseExp * (1 + randomRate));
            player.gold += totalReward;
            player.AddExp(totalExp);


            int afterHp = player.hp;
            int afterGold = player.gold;
            int afterExp = player.exp;
            int afterLevel = player.level;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("던전 클리어");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("축하합니다!");
            Console.ResetColor();

            Console.WriteLine($"{dungeonName}을(를) 클리어 하였습니다.\n");

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"현재 레벨: {beforeLevel} -> {afterLevel}");
            Console.WriteLine($"체력 {beforeHp} -> {afterHp}");
            Console.WriteLine($"Gold {beforeGold} G -> {afterGold} G");
            Console.WriteLine($"Exp {beforeExp} -> {afterExp}");

            Console.WriteLine($"(공격력 {player.attack}의 영향으로 총 보상 {(int)(randomRate * 100)}% 추가 획득!)");
            Console.WriteLine("\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");




            while (true)
            {
                string input = Console.ReadLine();
                if (input == "0") break;
                Console.WriteLine("잘못된 입력입니다. 0을 입력해주세요.");
            }

            Console.Clear();
        }
    }

    public static class GameSaveManager
    {
        private static string savePath = "savegame.json";

        //저장하기
        public static void SaveGame(PlayerInfo player, Inventory inventory)
        {
            var saveData = new SaveData
            {
               Player = player,
               Inventory = inventory            
            };
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true
            };
            string json = JsonSerializer.Serialize(saveData, options);
            File.WriteAllText(savePath, json);
            Console.WriteLine("게임이 저장되었습니다");

        }
        
        //불러오기
        public static bool LoadGame(out PlayerInfo player, out Inventory inventory)
        {
            if (!File.Exists(savePath))
            {
                Console.WriteLine("저장된 데이터가 없습니다.");
                player = null;
                inventory = null;
                return false;
            }

            string json = File.ReadAllText(savePath);

            var options = new JsonSerializerOptions()
            {
                IncludeFields = true
            };
            var saveData = JsonSerializer.Deserialize<SaveData>(json, options);

            player = saveData.Player;
            inventory = saveData.Inventory;

            Console.WriteLine("저장된 데이터를 불러왔습니다.");
            return true;

        }
    }

    public class SaveData
    {
        public PlayerInfo Player { get; set; }
        public Inventory Inventory { get; set; }
    }

    public class Game
    {
        static void Main(string[] args)
        {
            Intro intro = new Intro();
            PlayerInfo playerinfo = new PlayerInfo();
            Inventory inventory = new Inventory();
            
            //저장 코드
            Console.WriteLine("저장된 게임을 불러오시겠습니까? (Y/N)");
            string choice = Console.ReadLine().ToUpper();
            
            if (choice == "Y")
            {
                bool loaded = GameSaveManager.LoadGame(out playerinfo, out inventory);

                if (loaded)
                {
                    Console.WriteLine("불러오기에 성공했습니다.");
                    playerinfo.UpdateEffect(inventory.Items);
                }
                else
                {
                    Console.WriteLine("저장된 데이터를 찾을 수 없습니다. 신규 게임을 시작합니다.");
                    Console.ReadKey();
                    playerinfo = new PlayerInfo();
                    inventory = new Inventory();
                    inventory.InitializeItem();
                    
                }
            }

            else
            {
                Console.WriteLine("신규 게임을 시작합니다.");
                Console.ReadKey();
                playerinfo = new PlayerInfo();
                inventory = new Inventory();
                inventory.InitializeItem();
            }

            Adventure adventure = new Adventure();
            Patrol patrol = new Patrol();
            Training training = new Training();
            Store store = new Store();
            Rest rest = new Rest();
            Dungeon dungeon = new Dungeon();

            adventure.EventInitialized();
            store.StoreInitialize();

            bool isGameStart = true;


            while (isGameStart)
            {
                Console.Clear();
                int choiceNum = intro.intromessage();

                switch (choiceNum)
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
                    case 4:
                        Console.Clear();
                        Console.WriteLine("마을 순찰하기");
                        patrol.PatrolVillage(playerinfo);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("훈련하기");
                        training.Ptraining(playerinfo);
                        break;
                    case 6:
                        Console.Clear();
                        Console.WriteLine("상점");
                        store.OpenStore(playerinfo, inventory);
                        break;
                    case 7:
                        Console.Clear();
                        Console.WriteLine("던전 입장");
                        dungeon.EnterDungeon(playerinfo);
                        break;
                    case 8:
                        Console.Clear();
                        Console.WriteLine("휴식하기");
                        rest.Recharge(playerinfo);
                        break;

                    case 9:
                        GameSaveManager.SaveGame(playerinfo, inventory);
                        Console.ReadKey();
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







