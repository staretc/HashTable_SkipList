using HashTablesLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;


namespace ExperimentsConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
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
            var watch = new Stopwatch();
            #region Starting up
            hashTable.Add("word", 0);
            foreach(var pair in hashTable) { }
            hashTable.Remove("word");
            #endregion

            #region HashTable

            Console.WriteLine("HashTable Insert");

            watch.Restart();
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
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            Console.WriteLine("HashTable Taking Most Frequent Words");

            var mostFrequentWords = new List<string>();
            var frequency = 27;
            watch.Restart();
            // Получение выборки слов которые встречаются чаще 27 раз
            foreach (var pair in hashTable)
            {
                if (pair.Value > frequency)
                {
                    mostFrequentWords.Add(pair.Key);
                }
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            Console.WriteLine("HashTable Deleting Most Frequent Words");

            watch.Restart();
            // Удаление слов которые встречаются чаще 27 раз
            foreach (var word in mostFrequentWords)
            {
                hashTable.Remove(word);
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            #endregion
        }
        static void Work_Dictionary(string[] words)
        {
            var dictionary = new System.Collections.Generic.Dictionary<string, int>();
            var watch = new Stopwatch();

            #region Dictionary

            Console.WriteLine("Dictionary Insert");

            watch.Restart();
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
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            Console.WriteLine("Dictionary Taking Most Frequent Words");

            var mostFrequentWords = new List<string>();
            var frequency = 27;
            watch.Restart();
            // Получение выборки слов которые встречаются чаще 27 раз
            foreach (var pair in dictionary)
            {
                if (pair.Value > frequency)
                {
                    mostFrequentWords.Add(pair.Key);
                }
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            Console.WriteLine("Dictionary Deleting Most Frequent Words");

            watch.Restart();
            // Удаление слов которые встречаются чаще 27 раз
            foreach (var word in mostFrequentWords)
            {
                dictionary.Remove(word);
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            Console.WriteLine("\n============\n");

            #endregion
        }
    }
}
