Dictionary dictionary = new Dictionary();

while (true)
{
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("1 - Показать имеющиеся словари");
    Console.WriteLine("2 - Создать словарь");
    Console.WriteLine("3 - Добавить слово и его переводы");
    Console.WriteLine("4 - Заменить слово или его перевод");
    Console.WriteLine("5 - Удалить слово или перевод");
    Console.WriteLine("6 - Искать перевод слова");
    Console.WriteLine("0 - Выход");

    string choice = Console.ReadLine();
    Console.Clear();

    switch (choice)
    {
        case "1":
            dictionary.ShowDictionaries();
            break;
        case "2":
            Console.WriteLine("Введите название словаря:");
            string dictionaryName = Console.ReadLine();
            dictionary.CreateDictionary(dictionaryName);
            break;
        case "3":
            dictionary.AddWord();
            break;
        case "4":
            dictionary.ReplaceWord();
            break;
        case "5":
            dictionary.DeleteWord();
            break;
        case "6":
            Console.WriteLine("Введите слово для поиска перевода:");
            string wordToSearchAll = Console.ReadLine();
            dictionary.SearchTranslation(wordToSearchAll);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Неверный выбор. Попробуйте ещё раз.");
            break;
    }

    Console.WriteLine();
}