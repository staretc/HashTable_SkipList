using HashTablesLib;
using SkipListLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace ExperimentsConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Experiment_HashTable();
            //Experiment_SkipList();
        }
        #region Experiment HashTable

        /// <summary>
        /// Запускаемый эксперимент для Хеш-таблицы
        /// </summary>
        static void Experiment_HashTable()
        {
            var inputText = File.ReadAllText("WarAndWorld.txt");
            string pattern = @"\b\w+\b";
            var matches = Regex.Matches(inputText, pattern);
            var words = new List<string>();
            foreach (Match match in matches)
            {
                words.Add(match.Value.ToLower());
            }
            Work_HashTable(words.ToArray());
            Work_Dictionary(words.ToArray());
        }
        static void Work_HashTable(string[] words)
        {
            var hashTable = new OpenAddressHashTable<string, int>();
            var insertWatch = new Stopwatch();
            var deleteWatch = new Stopwatch();
            var totalWatch = new Stopwatch();
            #region Starting up
            hashTable.Add("word", 0);
            foreach (var pair in hashTable) { }
            hashTable.Remove("word");
            #endregion

            #region HashTable

            totalWatch.Restart();
            insertWatch.Restart();
            // Вставка элементов в HashTable
            foreach (var word in words)
            {
                if (hashTable.ContainsKey(word))
                {
                    hashTable[word]++;
                    continue;
                }
                hashTable[word] = 1;
            }
            insertWatch.Stop();

            var mostFrequentWords = new List<string>();
            var frequency = 27;
            // Получение выборки слов которые встречаются чаще 27 раз
            foreach (var pair in hashTable)
            {
                if (pair.Value > frequency)
                {
                    mostFrequentWords.Add(pair.Key);
                }
            }

            deleteWatch.Restart();
            // Удаление слов которые встречаются чаще 27 раз
            foreach (var word in mostFrequentWords)
            {
                hashTable.Remove(word);
            }
            deleteWatch.Stop();

            Console.WriteLine("\n============\n");
            Console.WriteLine("Hash Table");
            Console.WriteLine($"Insert time: {insertWatch.ElapsedTicks}");
            Console.WriteLine($"Delete time: {deleteWatch.ElapsedTicks}");
            Console.WriteLine($"Total  time: {totalWatch.ElapsedTicks}");
            Console.WriteLine("\n============\n");

            #endregion
        }
        static void Work_Dictionary(string[] words)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, int>();
            var insertWatch = new Stopwatch();
            var deleteWatch = new Stopwatch();
            var totalWatch = new Stopwatch();

            #region Dictionary

            totalWatch.Restart();
            insertWatch.Restart();
            // Вставка элементов в Dictionary
            foreach (var word in words)
            {
                if (dictionary.ContainsKey(word))
                {
                    dictionary[word]++;
                    continue;
                }
                dictionary[word] = 1;
            }
            insertWatch.Stop();

            var mostFrequentWords = new List<string>();
            var frequency = 27;
            // Получение выборки слов которые встречаются чаще 27 раз
            foreach (var pair in dictionary)
            {
                if (pair.Value > frequency)
                {
                    mostFrequentWords.Add(pair.Key);
                }
            }

            deleteWatch.Restart();
            // Удаление слов которые встречаются чаще 27 раз
            foreach (var word in mostFrequentWords)
            {
                dictionary.Remove(word);
            }
            deleteWatch.Stop();
            totalWatch.Stop();

            Console.WriteLine("\n============\n");
            Console.WriteLine("Dictionary");
            Console.WriteLine($"Insert time: {insertWatch.ElapsedTicks}");
            Console.WriteLine($"Delete time: {deleteWatch.ElapsedTicks}");
            Console.WriteLine($"Total  time: {totalWatch.ElapsedTicks}");
            Console.WriteLine("\n============\n");

            #endregion
        }

        #endregion

        #region Experiment SkipList

        static void Experiment_SkipList()
        {
            var random = new Random();
            var count = 10000;
            var numbers = new HashSet<int>();
            while (numbers.Count < count)
            {
                numbers.Add(random.Next(1, count * 5));
            }
            Work_SkipList(numbers.ToArray());
            Work_SortedList(numbers.ToArray());
        }
        static void Work_SkipList(int[] numbers)
        {
            var skipList = new SkipList<int, int>();
            var totalWatch = new Stopwatch();

            #region Starting up
            skipList.Add(1, 1);
            skipList.ContainsKey(1);
            skipList.Remove(1);
            #endregion

            totalWatch.Start();
            foreach (var num in numbers)
            {
                skipList.Add(num, num);
            }
            var low = numbers.Length / 2;
            var high = numbers.Length / 4 * 3;
            for (int i = low; i < high; i++)
            {
                skipList.Remove(numbers[i]);
            }
            foreach (var num in numbers)
            {
                skipList.ContainsKey(num);
            }
            totalWatch.Stop();

            Console.WriteLine("\n============\n");
            Console.WriteLine("SkipList");
            Console.WriteLine($"Total time: {totalWatch.ElapsedTicks}");
            Console.WriteLine("\n============\n");
        }
        static void Work_SortedList(int[] numbers)
        {
            var sortedList = new SortedList<int, int>();
            var totalWatch = new Stopwatch();

            totalWatch.Start();
            foreach (var num in numbers)
            {
                sortedList.Add(num, num);
            }
            var low = numbers.Length / 2;
            var high = numbers.Length / 4 * 3;
            for (int i = low; i < high; i++)
            {
                sortedList.Remove(numbers[i]);
            }
            foreach (var num in numbers)
            {
                sortedList.ContainsKey(num);
            }
            totalWatch.Stop();

            Console.WriteLine("\n============\n");
            Console.WriteLine("SortedList");
            Console.WriteLine($"Total time: {totalWatch.ElapsedTicks}");
            Console.WriteLine("\n============\n");
        }

        #endregion
    }
}
