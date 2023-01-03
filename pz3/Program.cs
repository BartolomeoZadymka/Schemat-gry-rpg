using System;
using System.Collections.Generic;

namespace pz3
{
    class Item
    {
        // Pola klasy
        public string Name { get; set; }
        public int StrengthBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int IntelligenceBonus { get; set; }

        // Konstruktor klasy
        public Item(string name, int strengthBonus, int dexterityBonus, int intelligenceBonus)
        {
            Name = name;
            StrengthBonus = strengthBonus;
            DexterityBonus = dexterityBonus;
            IntelligenceBonus = intelligenceBonus;
        }
    }

    class Character
    {
        // Pola klasy
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public List<Item> Inventory { get; set; }
        public int Experience { get; set; }
        public int ExperienceToLevelUp { get; set; } = 100;

        // Konstruktor klasy
        public Character(string name, int level, int strength, int dexterity, int intelligence, int experience)
        {
            Name = name;
            Level = level;
            Health = 100;
            Mana = 100;
            Strength = strength;
            Dexterity = dexterity;
            Intelligence = intelligence;
            Inventory = new List<Item>();
            Experience = experience;

        }
        public void GainExperience(int amount)
        {
            Experience += amount;
        }

        // Metoda levelowania
        public void LevelUp()
        {
            if (Experience >= ExperienceToLevelUp)
            {
                Level++;
                Experience -= ExperienceToLevelUp;
                ExperienceToLevelUp += 50;
                Strength += 10;
                Dexterity += 10;
                Intelligence += 10;
                Health += 50;
                Mana += 50;
                Console.WriteLine(Name + " awansował na poziom " + Level + "!");
            }
        }
    

    // Metoda klasy
    public void PrintStats()
        {
            Console.WriteLine(" Nazwa: " + Name);
            Console.WriteLine(" Poziom: " + Level);
            Console.WriteLine(" Zdrowie: " + Health);
            Console.WriteLine(" Mana: " + Mana);
            Console.WriteLine(" Siła: " + Strength);
            Console.WriteLine(" Zręczność: " + Dexterity);
            Console.WriteLine(" Inteligencja: " + Intelligence);
            Console.WriteLine(" Ekwipunek: ");
            foreach (Item item in Inventory)
            {
                Console.WriteLine("  - " + item.Name + " (Siła +" + item.StrengthBonus + ", Zręczność +" + item.DexterityBonus + ", Inteligencja +" + item.IntelligenceBonus + ")");
            }
        }

        // Metoda klasy
        public void Attack(Character enemy)
        {
            int damage = Strength * 3 - enemy.Dexterity;
            if (damage < 0)
            {
                damage = 0;
            }
            enemy.Health -= damage;
            Console.WriteLine(Name + " zadał " + damage + " obrażeń " + enemy.Name + " i pozostało mu " + enemy.Health + " punktów zdrowia.");
            
        }

        // Metoda klasy
        public void Defend()
        {
            Console.WriteLine(Name + " broni się i otrzymuje bonus do obrony na kolejny atak.");
            IncreaseDefense();
        }

        private void IncreaseDefense()
        {
            Dexterity += 10;
                
        }
    

        // Metoda klasy
        public void CastSpell(Character enemy)
        {
            if (Mana < 10)
            {
                Console.WriteLine(Name + " nie ma wystarczająco many, by rzucić zaklęcie.");
                return;
            }

            int damage = Intelligence * 5;
            enemy.Health -= damage;
            Mana -= 10;
            Console.WriteLine(Name + " rzucił zaklęcie i zadał " + damage + " obrażeń " + enemy.Name + " i pozostało mu " + enemy.Health + " punktów zdrowia oraz " + Mana + " punktów many.");
        }

        // Metoda klasy
        public void TakeTurn(Character enemy)
        {
            Console.WriteLine("Co chcesz zrobić?");
            Console.WriteLine("1. Atak");
            Console.WriteLine("2. Obrona");
            Console.WriteLine("3. Rzucenie zaklęcia");
            Console.WriteLine("Wybierz opcję (wpisz cyfrę):");
            string input = Console.ReadLine();
            int choice;
            if (!int.TryParse(input, out choice))
            {
                Console.WriteLine("Nieprawidłowa opcja.");
                return;
            }
            switch (choice)
            {
                case 1:
                    Attack(enemy);
                    break;
                case 2:
                    Defend();
                    break;
                case 3:
                    CastSpell(enemy);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }
            // Zdobycie doświadczenia po zabiciu wroga
            if (enemy.Health <= 0)
            {
                GainExperience(enemy.Experience);
                LevelUp();
                PrintStats();
                Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void EnemyTakeTurn(Character player)
        {
            Random random = new Random();
            int action = random.Next(1, 4); // losujemy liczbę od 1 do 3
            if (action == 1)
            {
                Attack(player); // atak
            }
            else if (action == 2)
            {
                Defend(); // obrona
            }
            else if (action == 3)
            {
                CastSpell(player); // rzucenie zaklęcia
            }
        }

        // Metoda klasy
        public void Fight(Character enemy)
        {
            int dexterityplayer, dexterityenemy, i;
            dexterityplayer = this.Dexterity;
            dexterityenemy = enemy.Dexterity;
            i = 0;
            while (Health > 0 && enemy.Health > 0)
            {
                TakeTurn(enemy);
                if (enemy.Health <= 0) break;
                enemy.EnemyTakeTurn(this);
                Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
                Console.ReadKey();
                Console.Clear();
                i++;
                if(i%2==0)
                {
                    this.Dexterity = dexterityplayer;
                    enemy.Dexterity = dexterityenemy;
                }
                
            }

            if (Health > 0)
            {
                Console.WriteLine(Name + " pokonał " + enemy.Name + "!");
            }
            else
            {
                Console.WriteLine(enemy.Name + " pokonał " + Name + ".");
            }
        }
    }

    


    class Program
    {
        static void Main(string[] args)
        {
            // Wstęp
            string name ,decyzja;
            Console.WriteLine("Podaj swą godność");
            name = Console.ReadLine();

            // Tworzenie nowej postaci
            Character player = new Character(name, 1, 10, 10, 10 ,0);
            Character enemy = new Character("Lalka", 1, 15, 5, 5 ,100);

            // Tworzenie przedmiotów
            Item sword = new Item("Miecz", 5, 0, 0);
            Item shield = new Item("Tarcza", 0, 5, 0);
            Item scroll = new Item("Pierścień many", 0, 0, 5);

            // Dodawanie przedmiotów do ekwipunku
            player.Inventory.Add(sword);
            player.Inventory.Add(shield);
            player.Inventory.Add(scroll);

            Console.WriteLine("Oto ty i twoje statystyki");
            player.PrintStats();
            Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
            Console.ReadKey();
            Console.WriteLine("Rozpoczynasz trening twoim przeciwnikiem będzie lalka");
            enemy.PrintStats();
            Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
            Console.ReadKey();


            while (player.Health>0 && enemy.Health > 0)
            {
               
                // Aktualizacja statystyk postaci na podstawie przedmiotów w ekwipunku
                foreach (Item item in player.Inventory)
                {
                    player.Strength += item.StrengthBonus;
                    player.Dexterity += item.DexterityBonus;
                    player.Intelligence += item.IntelligenceBonus;
                }
               
                // Walka - tura gracza
                player.Fight(enemy);

            }
            

            if (player.Health>0)
            {
                Console.WriteLine("Gratulacje, wygrałeś walkę!");
            }
            else
            {
                Console.WriteLine("Niestety, przegrałeś walkę.");
            }
            Console.WriteLine("Udało ci się przejść szkolenie teraz czeka na ciebie przygoda. Wyleczymy cię zanim wyruszysz");
            player.Health = 100;
            enemy = new Character("Rabuś", 1, 15, 5, 5, 50);
            Console.WriteLine("Z początku udałeś sie do karczy aby znaleźć swoje pierwsze indywidualne zlecenie , lecz napadł na ciebie Rabuś");
            enemy.PrintStats();
            Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
            Console.ReadKey();
            Console.Clear();
            // Walka - tura gracza
            player.Fight(enemy);
            if (enemy.Health <= 0)
            { Item sword2 = new Item("Miecz Rabusia", 8, 0, 2);
                Console.WriteLine("Z przeciwnika wypadł Miecz Rabusia z statystykami  8, 0, 2. Czy chcesz go zamienić z swoim aktualnym mieczem? tak/nie");
                decyzja = Console.ReadLine();
                if (decyzja == "tak")
                {
                    player.Inventory.Add(sword2);
                    player.Inventory.Remove(sword);
                    player.PrintStats();
                }
                else
                {
                    Console.WriteLine("Skoro go nie chciałeś to podróż twa dalej");
                }
                Console.WriteLine("Wciśnij dowolny klawisz aby kontynuować");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("W karczmie było jedno zlecenie więc niestety byłeś na nie skazany");
                Console.WriteLine("Udałeś się aby zabić potężnego smoka");
                enemy = new Character("Smok", 99, 40, 40, 40, 100);
                enemy.Health = +400;
                // Walka - tura gracza
                player.Fight(enemy);
                if (player.Health <= 0) Console.WriteLine("Jeżeli pokonał cię smok taki był zamysł niestety twoja przygoda dobiega końca nie ma dalszej części gry jeżeli przegrałeś przed smokiem to graj dalej");
                else Console.WriteLine("Nie wiem jak ci się to udało ale brawo, twoja przygoda dobiega końca nie ma dalszej części gry");

            } 
            
        }
    }
}