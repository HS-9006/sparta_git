//namespace CSharpDay;

using System.ComponentModel.Design;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;



class Program
{
    static void Main(string[] args)
    {
        var gameLogic = new GameLogic();
        gameLogic.StartGame();
    }
}

class GameLogic
{
    private Player _player;
    private bool _isGameOver = false;

    public void StartGame()
    {
        Init();

        while (!_isGameOver)
        {
            InputHandler();
        }

        Console.WriteLine("게임이 종료되었습니다.");
    }

    private void InputHandler()
    {
        var input = Console.ReadKey();
        if (input.Key == ConsoleKey.Escape)
        {
            _isGameOver = true;
        }
    }

    private void Init()
    {

        Console.Clear();
        Console.WriteLine("스파르타 던전에 오신것을 환영합니다.\n이름을 입력하세요.");
        string? playerName = Console.ReadLine();

        if (string.IsNullOrEmpty(playerName))
        {
            Console.WriteLine("잘못된 이름입니다.");
            Thread.Sleep(1000);
            Init();
        }
        else
        {
            _player = new Player(playerName);
            Console.WriteLine($"{_player.name}님, 입장하셨습니다.");
        }

        // 직업선택
        Console.WriteLine("직업을 선택하세요. [1:전사 | 2:법사 | 3:궁수]");
        int job = int.Parse(Console.ReadLine());

        if (job >= 1 && job <= 3)
        {
            _player.job = (Job)job;

            switch (_player.job)
            {
                case Job.Warrior:
                    Console.WriteLine($"{_player.job}를 선택했습니다.");
                    break;
                case Job.Wizzard:
                    Console.WriteLine($"{_player.job}를 선택했습니다.");
                    break;
                case Job.Archor:
                    Console.WriteLine($"{_player.job}를 선택했습니다.");
                    break;
            }
        }
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("1. 상태보기\n2. 인벤토리\n3. 상점");
            string information = Console.ReadLine();


            var ironArmor = new ItemData("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 5, ItemType.armor, 500);
            var spartanSpear = new ItemData("스파르타의 창", "스파르타 전사들이 사용했다는 전설의 창입니다.", 7, 0, ItemType.spear, 800);

            if (information == "1")
            {
                bool isInfo = true;
                while (isInfo)
                {

                    Console.WriteLine("상태보기 입니다.");
                    Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                    Console.WriteLine($"Lv : {_player.level}\n" + $"{_player.name}" + "(" + $"{_player.job}" + ")\n" +
                        $"공격력 : {_player.attack}\n" + $"방어력 : {_player.defense}\n" + $"체력 : {_player.stamina}\n" + $"Gold : {_player.money}\n");
                    Console.WriteLine("0. 나가기");
                    int infoOut = int.Parse(Console.ReadLine());

                    if (infoOut == 0)
                    {
                        isInfo = false;
                    }
                }
            }

            else if (information == "2")
            {
                bool isWearing = true;
                while (isWearing)
                {

                    Console.WriteLine("인벤토리 입니다.");
                    Console.WriteLine("1. 장착 관리");
                    Console.WriteLine("0. 나가기");
                    int wearing = int.Parse(Console.ReadLine());

                    if (wearing == 0)
                    {
                        isWearing = false;
                    }

                    if (wearing == 1)
                    {
                        if (_player.inventory.Count == 0)
                        {
                            Console.WriteLine("보유 중인 아이템이 없습니다.");
                        }
                        else
                        {
                            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                            Console.WriteLine("[아이템 목록]");
                            for (int i = 0; i < _player.inventory.Count; i++)
                            {
                                var item = _player.inventory[i];
                                bool isMark = (item == _player.equippedWeapon || item == _player.equippedArmor);
                                //삼항연산자 조건 ? 참일 때 값 : 거짓일 때 값;
                                string mark = isMark ? "[E]" : "   ";

                                Console.WriteLine($"{mark} {item.name} | 공격력 +{item.attack} | 방어력 +{item.defense} | {item.description}");
                            }
                            console.WriteLine("0. 나가기");
                            int state = int.parse(console.WriteLine());

                            if (state == 0)
                            {
                                isWearing = false;
                            }
                        }
                    }
                }
            }
            else if (information == "3")
            {
                bool isShopping = true;

                List<ItemData> shopItems = new List<ItemData>()
                {
                    new ItemData("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 5, ItemType.armor, 1000),
                    new ItemData("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, ItemType.armor, 1000),
                    new ItemData("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, ItemType.armor, 3500),
                    new ItemData("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, ItemType.spear, 600),
                    new ItemData("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 5, 0, ItemType.spear, 1500),
                    new ItemData("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, ItemType.spear, 1000)
                };
                while (isShopping)
                {
                    Console.WriteLine("상점 입니다.");
                    Console.WriteLine("[보유골드]\n" + $"{_player.money}G\n");
                    Console.WriteLine("아이템목록");
                    for (int i = 0; i < shopItems.Count; i++)
                    {
                        var item = shopItems[i];
                        bool already = _player.inventory.Exists(x => x.name == item.name);
                        string priceLabel = already ? "구매완료" : $"{item.money}G";

                        Console.WriteLine($"{i + 1}.{item.name} | 공격력 + {item.attack} | 방어력 +{item.defense} | {item.description} | {priceLabel}");
                    }
                    Console.WriteLine("\n1. 아이템 구매");
                    Console.WriteLine("0. 나가기");

                    string shopInput = Console.ReadLine();

                    if (shopInput == "0")
                    {
                        isShopping = false;
                    }
                    else if (shopInput == "1")
                    {
                        Console.WriteLine("구매할 아이템 번호를 입력하세요");
                        string storeNum = Console.ReadLine();
                        int buyIndex;
                        if (int.TryParse(storeNum, out buyIndex))
                        {
                            buyIndex -= 1;
                            if (buyIndex >= 0 && buyIndex < shopItems.Count)
                            {
                                var itemBuy = shopItems[buyIndex];

                                if (_player.inventory.Exists(x => x.name == itemBuy.name))
                                {
                                    Console.WriteLine("이미 구입한 물건입니다.");
                                }
                                else if (_player.money >= itemBuy.money)
                                {
                                    _player.money -= itemBuy.money;
                                    _player.inventory.Add(itemBuy);
                                    Console.WriteLine($"{itemBuy.name} 을 구매했습니다.");
                                }
                                else
                                {
                                    Console.WriteLine("골드가 부족합니다.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("번호를 다시 입력해주세요");
                            }
                        }
                        else
                        {
                            Console.WriteLine("숫자를 입력해주세요");
                        }
                    }
                    else
                    {
                        Console.WriteLine("번호를 다시 입력해주세요");
                    }
                }
            }
        }
    }
}

class Player
{
    public string name;
    public Job job;
    public int level;
    public int attack;
    public int defense;
    public int stamina;
    public int money;
    public ItemType type;

    public List<ItemData> inventory = new List<ItemData>();
    public ItemData equippedWeapon = null;
    public ItemData equippedArmor = null;

    public Player(string name)
    {
        this.name = name;
        this.level = 01;
        this.attack = 10;
        this.defense = 5;
        this.stamina = 100;
        this.money = 1500;
    }
}
class ItemData
{
    public string name;
    public string description;
    public int attack;
    public int defense;
    public ItemType type;
    public int money;

    public ItemData(string name, string description, int attack, int defense, ItemType type, int money)
    {
        this.name = name;
        this.description = description;
        this.attack = attack;
        this.defense = defense;
        this.type = type;
        this.money = money;
    }
}


public enum Job
{
    Warrior = 1,
    Wizzard,
    Archor
}

public enum ItemType
{
    armor,
    spear,
    knife
}
