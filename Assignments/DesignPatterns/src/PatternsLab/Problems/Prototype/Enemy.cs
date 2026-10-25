namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
}

public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public List<string> Abilities { get; set; } = new();
    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);
        _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6];
    }
}

public class Orc : Enemy,IEnemyProtoType
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;
        Weapon = new Weapon { Name = "Axe", Damage = 25 };
        Abilities.Add("Rage");
    }

    public IEnemyProtoType Clone()
	{
        var Clone = this.MemberwiseClone() as Orc;
		Clone.Weapon = new Weapon { Name = this.Weapon.Name, Damage = this.Weapon.Damage };
		Clone.Abilities = new List<string>(this.Abilities);
        return Clone;
	}
}

public class Elf : Enemy,IEnemyProtoType
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;
        Weapon = new Weapon { Name = "Bow", Damage = 18 };
        Abilities.Add("Stealth");
    }

	public IEnemyProtoType Clone()
	{
        var Clone = this.MemberwiseClone() as Elf;
        Clone.Weapon = new Weapon { Name = this.Weapon.Name, Damage = this.Weapon.Damage };
        Clone.Abilities = new List<string>(this.Abilities);
        return Clone;
	}
}

public static class EnemyRegister
{
    private readonly static Dictionary<string, Enemy> _Enemies = new Dictionary<string, Enemy>();

    public static void Register(string Key,Enemy Value)
    {
        _Enemies[Key] = Value;
    }
    public static Enemy Create(string Enemy)
    {

		if (!_Enemies.TryGetValue(Enemy,out Enemy e))
		{

		    throw new NotSupportedException("Unknown enemy type");
		}

		if (e is Orc orc)
        {
            return (Orc) orc.Clone();
        }
        else if (e is Elf elf)
        {
            return (Elf)elf.Clone();
        }
		throw new NotSupportedException("Unknown enemy type");
	}
}