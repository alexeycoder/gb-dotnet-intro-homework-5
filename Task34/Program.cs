// Задача 34: Задайте массив заполненный случайными положительными трёхзначными числами.
// Напишите программу, которая покажет количество чётных чисел в массиве.
// [345, 897, 568, 234] -> 2

do
{
	Console.Clear();
	PrintTitle("Определение количества чётных элементов в массиве,"
				+ " заполненном случайными положительными трёхзначными числами", ConsoleColor.Cyan);

	int qty = GetUserInput("Введите число элементов массива (не менее одного): ", 1);
	int[] arr = CreateArrayRandomInt(qty, 100, 999);
	int numOfEvenItems = CountEvenItems(arr);

	PrintArray(arr, ConsoleColor.DarkGray);
	PrintColored($" -> {numOfEvenItems}", ConsoleColor.Yellow);

} while (AskForRepeat());

// Methods

static int CountEvenItems(int[] array)
{
	int count = 0;
	for (int i = 0; i < array.Length; ++i)
	{
		if (array[i] % 2 == 0)
			++count;
	}
	return count;
}

static int[] CreateArrayRandomInt(int size, int min, int max)
{
	if (size <= 0)
		return new int[] { };

	int[] array = new int[size];
	Random rnd = new Random();
	max = max + 1; // to include to random range
	for (int i = 0; i < size; ++i)
	{
		array[i] = rnd.Next(min, max);
	}

	return array;
}

void PrintArray(int[] array, ConsoleColor? foreColor = null)
{
	if (array.Length <= 0)
	{
		Console.WriteLine("Массив пуст.");
		return;
	}

	var bkpColor = Console.ForegroundColor;
	if (foreColor.HasValue)
		Console.ForegroundColor = foreColor.Value;

	Console.Write("[");
	for (int i = 0; i < array.Length - 1; i++)
	{
		Console.Write($"{array[i]}, ");
	}
	Console.Write($"{array[array.Length - 1]}]");
	Console.ForegroundColor = bkpColor;
}

static int GetUserInput(string inputMessage, int minAllowed = int.MinValue, int maxAllowed = int.MaxValue)
{
	const string errorMessageWrongType = "Некорректный ввод! Пожалуйста повторите";
	string errorMessageOutOfRange = string.Empty;
	if (minAllowed != int.MinValue && maxAllowed != int.MaxValue)
		errorMessageOutOfRange = $"Число должно быть в интервале от {minAllowed} до {maxAllowed}! Пожалуйста повторите";
	else if (minAllowed != int.MinValue)
		errorMessageOutOfRange = $"Число не должно быть меньше {minAllowed}! Пожалуйста повторите";
	else
		errorMessageOutOfRange = $"Число не должно быть больше {maxAllowed}! Пожалуйста повторите";

	int input;
	bool notANumber = false;
	bool outOfRange = false;
	do
	{
		if (notANumber)
		{
			PrintError(errorMessageWrongType, ConsoleColor.Magenta);
		}
		if (outOfRange)
		{
			PrintError(errorMessageOutOfRange, ConsoleColor.Magenta);
		}
		Console.Write(inputMessage);
		notANumber = !int.TryParse(Console.ReadLine(), out input);
		outOfRange = !notANumber && (input < minAllowed || input > maxAllowed);

	} while (notANumber || outOfRange);

	return input;
}

static void PrintTitle(string title, ConsoleColor foreColor)
{
	int feasibleWidth = Math.Min(title.Length, Console.BufferWidth);
	string titleDelimiter = new string('\u2550', feasibleWidth);
	PrintColored(titleDelimiter + Environment.NewLine + title + Environment.NewLine + titleDelimiter, foreColor);
}

static void PrintError(string errorMessage, ConsoleColor foreColor)
{
	PrintColored("\u2757 Ошибка: " + errorMessage, foreColor);
}

static void PrintColored(string message, ConsoleColor foreColor)
{
	var bkpColor = Console.ForegroundColor;
	Console.ForegroundColor = foreColor;
	Console.WriteLine(message);
	Console.ForegroundColor = bkpColor;
}

static bool AskForRepeat()
{
	Console.WriteLine();
	Console.WriteLine("Нажмите Enter, чтобы повторить или Esc, чтобы завершить...");
	ConsoleKeyInfo key = Console.ReadKey(true);
	return key.Key != ConsoleKey.Escape;
}
