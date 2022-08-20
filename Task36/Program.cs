// Задача 36: Задайте одномерный массив, заполненный случайными числами.
// Найдите сумму элементов, стоящих на нечётных позициях (индексах).
// [3, 7, 23, 12] -> 19
// [-4, 6-, 89, 6] -> 0

do
{
	Console.Clear();
	PrintTitle("Подсчёт суммы элементов массива случайных чисел c нечётными индексами", ConsoleColor.Cyan);
	int minimum = GetUserInput("Введите нижний предел диапазона случайных чисел (целое число): ");
	int maximum = GetUserInput("Введите верхний предел диапазона случайных чисел (целое число): ", minimum);
	int qty = GetUserInput("Введите число элементов массива (не менее 2-х): ", 2);

	int[] arr = CreateArrayRandomInt(qty, minimum, maximum);
	int numOfEvenItems = CountEvenItems(arr);

	PrintArray(arr, ConsoleColor.DarkGray);
	PrintColored($" -> {numOfEvenItems}", ConsoleColor.Yellow);

} while (AskForRepeat());

// Methods

static int CountEvenItems(int[] array)
{
	int sum = 0;
	for (int i = 1; i < array.Length; i = i + 2)
	{
		sum += array[i];
	}
	return sum;
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
