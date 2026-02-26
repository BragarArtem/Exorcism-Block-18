using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class Control : Godot.Control
{
    [Export] public LineEdit nickname_inputField;

    private string[] firstNames = { "Толік","Васька","Петя","Масяня","Гріша","Мішаня","Коля","Саня","Вітьок","Андрюха",
        "Льоха","Паша","Жорик","Славік","Валерка","Дімон","Тьомка","Гєна","Тоха","Стасік",
        "Артур","Архіпчик","Деньчик" };
    private string[] lastNames = { "Драйвер","Агроном","Напівпровідник","Спічка","Каніфоль","Шифер","Транзистор","Процесор",
        "Паяльник","Болгарка","Роутер","Закодований","Соляра","Піксель","Фотон","Домушник",
        "Лінуксер","Задрот","Фотік","Тостер","Водолаз","Кругляк","Історік","Археолог",
        "Монстр","Дотер","Майнкрафтер" };

    private Random rng = new Random();

    private IEnumerator<int> _firstEnumerator;
    private IEnumerator<int> _lastEnumerator;

    public IEnumerable<int> IndexGenerator(int arrayLength)
    {
        while (true)
        {
            yield return rng.Next(0, arrayLength);
        }
    }

    public void ConsumeWithTimeout(IEnumerable<string> generator, double seconds)
    {
        Stopwatch sw = Stopwatch.StartNew();
        int count = 0;
		

        foreach (var nickname in generator)
        {
            if (sw.Elapsed.TotalSeconds >= seconds) break;
            GD.Print($"[{sw.Elapsed.TotalSeconds:F2}s],#{count} {nickname}");
			count++;
			System.Threading.Thread.Sleep(200);
        }
    }

    public IEnumerable<string> NicknameGenerator()
    {
        var first = IndexGenerator(firstNames.Length).GetEnumerator();
        var last = IndexGenerator(lastNames.Length).GetEnumerator();
        while (true)
        {
            first.MoveNext();
            last.MoveNext();
            yield return $"{firstNames[first.Current]} {lastNames[last.Current]}";
        }
    }

    public override void _Ready()
    {
        _firstEnumerator = IndexGenerator(firstNames.Length).GetEnumerator();
        _lastEnumerator = IndexGenerator(lastNames.Length).GetEnumerator();

        GetNode<Button>("nicknameRandomize_button").Pressed += _on_pressed;
        GetNode<Button>("nicknameRandomize_button").Pressed += _on_consume_pressed;
    }

    public void _on_pressed()
    {
        _firstEnumerator.MoveNext();
        _lastEnumerator.MoveNext();
        nickname_inputField.Text = $"{firstNames[_firstEnumerator.Current]} {lastNames[_lastEnumerator.Current]}";
    }

    public void _on_consume_pressed()
    {
        Task.Run(() => ConsumeWithTimeout(NicknameGenerator(), 2.0));
    }
}