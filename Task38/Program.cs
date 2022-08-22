// Задача 38: Задайте массив вещественных чисел.
// Найдите разницу между максимальным и минимальным элементами массива.
// [3.5, 7.1, 22.9, 2.3, 78.5] -> 76.2

const string DblNumFormat = "F1";
const double DblEpsilon = 1E-5;

do
{
	Console.Clear();
	PrintTitle("Вычисление разницы между максимальным и минимальным элементами массива вещественных чисел", ConsoleColor.Cyan);
	double minimum = GetUserInputDbl("Введите нижний предел диапазона случайных чисел: ");
	double maximum = GetUserInputDbl("Введите верхний предел диапазона случайных чисел: ", minimum);
	int qty = GetUserInputInt("Введите число элементов массива (не менее 2-х): ", 2);

	double[] arr = CreateRandomArrayDbl(qty, minimum, maximum);
	double delta = GetDeltaMinMax(arr, out double minVal, out double maxVal);

	PrintArrayDbl(arr, minVal, maxVal, ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkGray);
	PrintColored($" -> {delta.ToString(DblNumFormat)}", ConsoleColor.Yellow);

} while (AskForRepeat());

// Methods

/// <summary>
/// Находит макс и мин элементы массива и вычисляет разницу между ними.
/// </summary>
/// <param name="array">Массив элементов типа double.</param>
/// <param name="min">Возвращаемый аргумент - значение минимального элемента массива.</param>
/// <param name="max">Возвращаемый аргумент - значение максимального элемента массива.</param>
/// <returns>Разница между макс и мин элементами массива.</returns>
/// <exception cref="array">Передан недопустимый пустой массив.</exception>
static double GetDeltaMinMax(double[] array, out double min, out double max)
{
	if (array.Length <= 0)
		throw new ArgumentException("Требуется непустой массив!", nameof(array));

	min = max = array[0];
	for (int i = 1; i < array.Length; ++i)
	{
		double item = array[i];
		if (item < min)
			min = item;
		else if (item > max)
			max = item;
	}
	return max - min;
}

static double[] CreateRandomArrayDbl(int size, double min, double max)
{
	if (size <= 0)
		return new double[] { };

	double[] array = new double[size];
	Random rnd = new Random();
	for (int i = 0; i < size; ++i)
	{
		array[i] = GetPseudoRandomDbl(rnd, min, max);
	}

	return array;
}

static double GetPseudoRandomDbl(Random rnd, double min, double max)
{
	double rndDbl = rnd.NextDouble();
	return min + (max - min) * rndDbl;
}

void PrintArrayDbl(double[] array, double minValue, double maxValue, ConsoleColor? highlightColorMin, ConsoleColor? highlightColorMax, ConsoleColor? foreColor)
{
	if (array.Length <= 0)
	{
		Console.WriteLine("Массив пуст.");
		return;
	}

	var bkpForeColor = Console.ForegroundColor;
	var bkpBackColor = Console.BackgroundColor;
	if (foreColor.HasValue)
		Console.ForegroundColor = foreColor.Value;
	else
		foreColor = bkpForeColor;

	Console.Write("[");
	int lastIndex = array.Length - 1;
	for (int i = 0; i <= lastIndex; i++)
	{
		double itemValue = array[i];

		// highlight Max (higher priority) or Min (less priority)
		bool restoreBackColor = DoHighlightIfNecessary(highlightColorMax, maxValue, itemValue)
							|| DoHighlightIfNecessary(highlightColorMin, minValue, itemValue);

		Console.Write(itemValue.ToString(DblNumFormat));

		if (restoreBackColor)
		{
			Console.BackgroundColor = bkpBackColor;
			Console.ForegroundColor = foreColor.Value;
		}

		Console.Write(i == lastIndex ? "]" : ", ");
	}

	// double lastItemValue = array[array.Length - 1];
	// bool _ = DoHighlightIfNecessary(highlightColorMax, maxValue, lastItemValue) || DoHighlightIfNecessary(highlightColorMin, minValue, lastItemValue);
	// Console.Write($"{lastItemValue.ToString(DblNumFormat)}]");

	Console.ForegroundColor = bkpForeColor;
	Console.BackgroundColor = bkpBackColor;
}

static bool DoHighlightIfNecessary(ConsoleColor? highlightColor, double referenceValue, double itemValue)
{
	if (highlightColor.HasValue && Math.Abs(itemValue - referenceValue) < DblEpsilon)
	{
		var backColor = highlightColor.Value;
		var oppositeForeColor = backColor < ConsoleColor.DarkGray ? ConsoleColor.White : ConsoleColor.Black;
		Console.BackgroundColor = backColor;
		Console.ForegroundColor = oppositeForeColor;
		return true;
	}

	return false;
}

static int GetUserInputInt(string inputMessage, int minAllowed = int.MinValue, int maxAllowed = int.MaxValue)
{
	const string errorMessageWrongType = "Некорректный ввод! Требуется целое число. Пожалуйста повторите";
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

static double GetUserInputDbl(string inputMessage, double minAllowed = double.MinValue, double maxAllowed = double.MaxValue)
{
	const string errorMessageWrongType = "Некорректный ввод! Пожалуйста повторите";
	string errorMessageOutOfRange = string.Empty;
	if (minAllowed != double.MinValue && maxAllowed != double.MaxValue)
		errorMessageOutOfRange = $"Число должно быть в интервале от {minAllowed} до {maxAllowed}! Пожалуйста повторите";
	else if (minAllowed != double.MinValue)
		errorMessageOutOfRange = $"Число не должно быть меньше {minAllowed}! Пожалуйста повторите";
	else
		errorMessageOutOfRange = $"Число не должно быть больше {maxAllowed}! Пожалуйста повторите";

	double input = 0;
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

		string? inputStr = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(inputStr))
		{
			notANumber = true;
			continue;
		}
		notANumber = !double.TryParse(MakeInvariantToSeparator(inputStr), out input);
		outOfRange = !notANumber && (input < minAllowed || input > maxAllowed);

	} while (notANumber || outOfRange);

	return input;
}

static string MakeInvariantToSeparator(string input)
{
	char decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
	char wrongSeparator = decimalSeparator.Equals('.') ? ',' : '.';
	return input.Trim().Replace(wrongSeparator, decimalSeparator);
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
