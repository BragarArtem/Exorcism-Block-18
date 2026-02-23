using Godot;
using System;

public partial class Control : Godot.Control
{
	[Export] public LineEdit nickname_inputField;
	private string[] firstNames = { 
    "Толік", "Васька", "Петя", "Масяня", "Гріша", 
    "Мішаня", "Коля", "Саня", "Вітьок", "Андрюха", 
    "Льоха", "Паша", "Жорик", "Славік", "Валерка", 
    "Дімон", "Тьомка", "Гєна", "Тоха", "Стасік",
	"Артур", "Архіпчик", "Деньчик"
	};
	private string[] lastNames = { 
    "Драйвер", "Агроном", "Напівпровідник", "Спічка", "Каніфоль", 
    "Шифер", "Транзистор", "Процесор", "Паяльник", "Болгарка", 
    "Роутер", "Закодований", "Соляра", "Піксель", "Фотон", 
    "Домушник", "Лінуксер", "Задрот", "Фотік", "Тостер",
	"Водолаз","Кругляк","Історік","Археолог", "Монстр",
	"Дотер", "Майнкрафтер"
	};
	public override void _Ready()
	{
    Button rndButton = GetNode<Button>("nicknameRandomize_button"); 
    rndButton.Pressed += _on_pressed;
	}
	public override void _Process(double delta)
	{
	}
	public void _on_pressed()
	{
		string n = firstNames[Generator.Get(firstNames.Length)];
		string s = lastNames[Generator.Get(lastNames.Length)];
		string fullName = "";
		foreach(string part in new string[] { n, s })
		{
			fullName += part + " ";
		}
        nickname_inputField.Text = fullName.Trim();
	}
}
public static class Generator
{
    private static ulong _state = (ulong)System.Environment.TickCount;

    public static int Get(int max) 
    {
        _state = (214013 * _state + 2531011) % 2147483648;
        int result = (int)(_state >> 16) % max;
        return result;
    }
}
