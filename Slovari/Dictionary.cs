class Dictionary
{
    private string[] dictionaryFiles;

    public Dictionary()
    {
        dictionaryFiles = GetDictionaryFiles();
    }

    public void CreateDictionary(string dictionaryName)
    {
        string filePath = $"{dictionaryName}.txt";
        if (File.Exists(filePath))
        {
            Console.WriteLine($"Словарь {dictionaryName} уже существует.");
        }
        else
        {
            File.Create(filePath).Close();
            Console.WriteLine($"Словарь {dictionaryName} успешно создан.");
            UpdateDictionaryFiles();
        }
    }

    private void UpdateDictionaryFiles()
    {
        dictionaryFiles = GetDictionaryFiles();
    }

    public void AddWord()
    {
        Console.WriteLine("Введите слово:");
        string word = Console.ReadLine();
        Console.WriteLine("Введите переводы (через запятую):");
        string translationsInput = Console.ReadLine();
        List<string> translations = new List<string>(translationsInput.Split(','));

        Console.WriteLine("Выберите словарь, в который хотите добавить перевод:");
        string[] dictionaryNames = GetDictionaryNames();
        int selectedDictionaryIndex = GetUserChoice(dictionaryNames);
        if (selectedDictionaryIndex != -1)
        {
            string dictionaryName = dictionaryNames[selectedDictionaryIndex];
            string filePath = $"{dictionaryName}.txt";
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine($"{word}: {string.Join(", ", translations)}");
            }
            Console.WriteLine($"Слово {word} успешно добавлено в словарь {dictionaryName}.");
        }
        else
        {
            Console.WriteLine("Неверный выбор словаря.");
        }
    }

    public void ReplaceWord()
    {
        Console.WriteLine("Введите слово для замены:");
        string wordToReplace = Console.ReadLine();

        Console.WriteLine("Введите новые переводы (через запятую):");
        string newTranslationsInput = Console.ReadLine();
        List<string> newTranslations = new List<string>(newTranslationsInput.Split(','));

        List<string> dictionariesWithWord = new List<string>();
        List<string> dictionariesToReplaceIn = new List<string>();

        foreach (string filePath in dictionaryFiles)
        {
            string dictionaryName = Path.GetFileNameWithoutExtension(filePath);
            string[] lines = File.ReadAllLines(filePath);
            bool wordFound = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith($"{wordToReplace}:", StringComparison.OrdinalIgnoreCase))
                {
                    wordFound = true;
                    dictionariesWithWord.Add(dictionaryName);
                }
            }
            if (wordFound)
            {
                dictionariesToReplaceIn.Add(dictionaryName);
            }
        }

        if (dictionariesWithWord.Count == 0)
        {
            Console.WriteLine($"Слово {wordToReplace} не найдено в словарях.");
            return;
        }

        Console.WriteLine($"Слово {wordToReplace} найдено в следующих словарях: {string.Join(", ", dictionariesWithWord)}");

        if (dictionariesToReplaceIn.Count == 0)
        {
            Console.WriteLine($"Слово {wordToReplace} не может быть заменено, так как отсутствует в каком-либо словаре.");
            return;
        }

        Console.WriteLine($"Выберите словарь для замены слова {wordToReplace}:");
        int selectedDictionaryIndex = GetUserChoice(dictionariesToReplaceIn.ToArray());
        if (selectedDictionaryIndex != -1)
        {
            string dictionaryName = dictionariesToReplaceIn[selectedDictionaryIndex];
            string filePath = $"{dictionaryName}.txt";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith($"{wordToReplace}:", StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = $"{wordToReplace}: {string.Join(", ", newTranslations)}";
                    File.WriteAllLines(filePath, lines);
                    Console.WriteLine($"Слово {wordToReplace} успешно заменено в словаре {dictionaryName}.");
                    return;
                }
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор словаря.");
        }
    }


    public void DeleteWord()
    {
        Console.WriteLine("Введите слово или перевод для удаления:");
        string wordOrTranslationToDelete = Console.ReadLine();

        List<string> dictionariesWithWord = new List<string>();
        List<string> dictionariesToDeleteFrom = new List<string>();

        foreach (string filePath in dictionaryFiles)
        {
            string dictionaryName = Path.GetFileNameWithoutExtension(filePath);
            string[] lines = File.ReadAllLines(filePath);
            bool wordFound = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith($"{wordOrTranslationToDelete}:", StringComparison.OrdinalIgnoreCase))
                {
                    wordFound = true;
                    dictionariesWithWord.Add(dictionaryName);
                }
            }
            if (wordFound)
            {
                dictionariesToDeleteFrom.Add(dictionaryName);
            }
        }

        if (dictionariesWithWord.Count == 0)
        {
            Console.WriteLine($"Слово {wordOrTranslationToDelete} не найдено в словарях.");
            return;
        }

        Console.WriteLine($"Слово {wordOrTranslationToDelete} найдено в следующих словарях: {string.Join(", ", dictionariesWithWord)}");

        if (dictionariesToDeleteFrom.Count == 0)
        {
            Console.WriteLine($"Слово {wordOrTranslationToDelete} не может быть удалено, так как отсутствует в каком-либо словаре.");
            return;
        }

        Console.WriteLine($"Выберите словарь для удаления слова {wordOrTranslationToDelete}:");
        int selectedDictionaryIndex = GetUserChoice(dictionariesToDeleteFrom.ToArray());
        if (selectedDictionaryIndex != -1)
        {
            string dictionaryName = dictionariesToDeleteFrom[selectedDictionaryIndex];
            string filePath = $"{dictionaryName}.txt";
            string[] lines = File.ReadAllLines(filePath);
            List<string> newLines = new List<string>();
            bool wordDeleted = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.StartsWith($"{wordOrTranslationToDelete}:", StringComparison.OrdinalIgnoreCase))
                {
                    newLines.Add(line);
                }
                else
                {
                    wordDeleted = true;
                }
            }
            if (wordDeleted)
            {
                File.WriteAllLines(filePath, newLines);
                Console.WriteLine($"Слово {wordOrTranslationToDelete} успешно удалено из словаря {dictionaryName}.");
            }
            else
            {
                Console.WriteLine($"Слово {wordOrTranslationToDelete} не найдено в словаре {dictionaryName}.");
            }
        }
        else
        {
            Console.WriteLine("Неверный выбор словаря.");
        }
    }

    public void SearchTranslation(string word)
    {
        word = word.ToLower();
        bool translationFound = false;
        foreach (string filePath in dictionaryFiles)
        {
            string dictionaryName = Path.GetFileNameWithoutExtension(filePath);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                string entryWord = parts[0].Trim().ToLower();
                string translation = parts[1].Trim();
                if (entryWord == word)
                {
                    Console.WriteLine($"Перевод слова {entryWord} из словаря {dictionaryName}: {translation}");
                    translationFound = true;
                }
            }
        }
        if (!translationFound)
        {
            Console.WriteLine($"Перевод слова {word} не найден в словарях.");
        }
        else
        {
            bool saveResult = GetYesOrNoInput("Хотите сохранить результат в отдельный файл? (да/нет)");
            if (saveResult)
            {
                SaveSearchResultToFile(word);
            }
        }
    }

    private bool GetYesOrNoInput(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine().ToLower();
            if (input == "да" || input == "д")
            {
                return true;
            }
            else if (input == "нет" || input == "н")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз.");
            }
        }
    }

    public void ShowDictionaries()
    {
        string[] dictionaryNames = GetDictionaryNames();
        if (dictionaryNames.Length == 0)
        {
            Console.WriteLine("Нет доступных словарей.");
        }
        else
        {
            Console.WriteLine("Доступные словари:");
            foreach (string dictionaryName in dictionaryNames)
            {
                Console.WriteLine(dictionaryName);
            }
        }
    }

    private void SaveSearchResultToFile(string word)
    {
        string fileName = $"search/{word}.txt";
        List<string> searchResultLines = new List<string>();

        foreach (string filePath in dictionaryFiles)
        {
            string dictionaryName = Path.GetFileNameWithoutExtension(filePath);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(':');
                string entryWord = parts[0].Trim().ToLower();
                string translation = parts[1].Trim();
                if (entryWord == word)
                {
                    searchResultLines.Add($"Перевод слова {entryWord} из словаря {dictionaryName}: {translation}");
                }
            }
        }

        if (searchResultLines.Count > 0)
        {
            Directory.CreateDirectory("search");
            File.WriteAllLines(fileName, searchResultLines);
            Console.WriteLine($"Результат поиска сохранен в файл {fileName}.");
        }
    }

    private string[] GetDictionaryFiles()
    {
        return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
    }

    private string[] GetDictionaryNames()
    {
        List<string> dictionaryNames = new List<string>();
        foreach (string filePath in dictionaryFiles)
        {
            dictionaryNames.Add(Path.GetFileNameWithoutExtension(filePath));
        }
        return dictionaryNames.ToArray();
    }

    private int GetUserChoice(string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.WriteLine("0 - Отмена");

        int choice;
        bool validChoice = int.TryParse(Console.ReadLine(), out choice);
        if (validChoice && choice >= 0 && choice <= options.Length)
        {
            return choice - 1;
        }
        return -1;
    }
}