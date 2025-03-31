using SkipListLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SkipListLib
{
    public class SkipList<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        #region Fields

        /// <summary>
        /// Массив узлов, с которых начинается каждый уровень
        /// </summary>
        private Node<TKey, TValue>[] _heads;
        /// <summary>
        /// Вероятность для перевода элемента на следующий уровень
        /// </summary>
        private readonly double _probability;
        /// <summary>
        /// Максимально возможный уровень
        /// </summary>
        private readonly int _maxLevel;
        /// <summary>
        /// Самый нижний занятый уровень
        /// </summary>
        private int _currentLevel;
        /// <summary>
        /// Для генерации случайных вероятностей
        /// </summary>
        private Random _random;

        #endregion

        #region Properties

        /// <summary>
        /// Количество узлов в Списке
        /// </summary>
        public int Count { get; private set; }

        #endregion

        #region Constructors

        public SkipList(int maxLevel = 10, double probability = 0.5)
        {
            _maxLevel = maxLevel;
            _probability = probability;
            _heads = new Node<TKey, TValue>[_maxLevel];
            // заполняем головные элементы каждого уровня пустыми узлами
            _heads[0] = new Node<TKey, TValue>();
            for (int i = 1; i < maxLevel; i++)
            {
                _heads[i] = new Node<TKey, TValue>();
                _heads[i - 1].Up = _heads[i];
                _heads[i].Down = _heads[i - 1];
            }
            _currentLevel = 0;
            _random = new Random(DateTime.Now.Millisecond);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Добавление нового узла в Список
        /// </summary>
        /// <param name="key">Ключ узла</param>
        /// <param name="value">Значение узла по ключу</param>
        public void Add(TKey key, TValue value)
        {
            var prevNode = new Node<TKey, TValue>[_maxLevel]; // для записи узлов на которых остановились при поиске места вставки на каждом уровне
            var currentNode = _heads[_currentLevel];
            // проходим по уровням и определяем на какое место необходимо вставить узел на каждом уровне
            for (int i = _currentLevel; i >= 0; i--)
            {
                // двигаемся по уровню пока следующий ключ < вставляемого ключа
                while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
                }
                // если нашли узел с таким же ключом, бросаем исключение
                if (currentNode.Right != null && currentNode.Right.Key.Equals(key))
                {
                    throw new ArgumentException();
                }
                // записываем узал, на котором остановились
                prevNode[i] = currentNode;
                // если на следующем уровне нет узла, останавливаемся
                if (currentNode.Down == null)
                {
                    break;
                }
                // иначе идём на следующий уровень
                currentNode = currentNode.Down;
            }
            // определяем, на какой уровень опустим новый элемент
            // это может быть как существующий уровень, так и новый (если можно добавить новые уровни)
            // всё зависит от рандома и вероятности перехода на новый уровень
            int level = 0;
            while (_random.NextDouble() < _probability && level < _maxLevel - 1)
            {
                level++;
            }
            while (_currentLevel < level)
            {
                // обновляем текущее количество уровней
                _currentLevel++;
                // заполняем ячейку массива головным элементом данного уровня (до этого она была пустая)
                // на новых уровнях мы вставим элемент сразу после головного
                prevNode[_currentLevel] = _heads[_currentLevel];
            }
            for (int i = 0; i <= level; i++)
            {
                // объявляем новый узел и вставляем его между бОльшим и меньшим
                var node = new Node<TKey, TValue>(key, value) { Right = prevNode[i].Right };
                prevNode[i].Right = node;
                if (i == 0) continue;
                // делаем связку между нижним элементом нового узла
                // и правым элементом узла прошлого уровня на котором остановились при поиске места для элемента
                // это будет один и тот же узел на разных уровнях
                node.Down = prevNode[i - 1].Right;
                prevNode[i - 1].Right.Up = node;
            }
            Count++;
        }
        /// <summary>
        /// Проверка наличия узла в Списке по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому ищем узел</param>
        /// <returns>True если ключ содержится в списке, иначе False</returns>
        public bool ContainsKey(TKey key)
        {
            if (Count == 0) return false;
            return Find(key) != null;
        }
        /// <summary>
        /// Удаление узла из Списка по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому необходимо удалить узел</param>
        public void Remove(TKey key)
        {
            if (Count == 0)
            {
                throw new ArgumentException();
            }

            var prevNodes = new Node<TKey, TValue>[_currentLevel + 1];
            var level = _currentLevel;

            // пропускаем заголовки
            // идём пока следующий после заголовка узел пустой или ключ узла < данного ключа
            var currentNode = _heads[_currentLevel];
            int compareResult;
            while (currentNode != null)
            {
                if (currentNode.Key.Equals(default(TKey)) && currentNode.Right != null)
                {
                    compareResult = currentNode.Right.Key.CompareTo(key);
                    if (compareResult <= 0)
                    {
                        break;
                    }
                }
                currentNode = currentNode.Down;
                level--;
            }
            if (currentNode == null)
            {
                throw new ArgumentException();
            }

            // собираем узлы, которые предшествуют удаляемому узлу
            while (currentNode != null)
            {
                if (currentNode.Right != null)
                {
                    compareResult = currentNode.Right.Key.CompareTo(key);
                    // если нашли узел с таким же ключом, записываем предшествующий ему узел
                    if (compareResult == 0)
                    {
                        prevNodes[level] = currentNode;
                        currentNode = currentNode.Down;
                        level--;
                        continue;
                    }
                    if (compareResult < 0) // иначе идём вправо по текущему уровню и пропускаем элементы с ключами < keyы
                    {
                        while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                        {
                            currentNode = currentNode.Right;
                        }
                        continue;
                    }
                }
                currentNode = currentNode.Down;
                level--;
            }

            // если ни одна из ячеек не заполнена (достаточно проверить самую первую, отвечающую за нижний уровень)
            // значит элемент не нашли
            if (prevNodes[0] == null)
            {
                throw new ArgumentException();
            }

            // перезаписываем связи узлов, исключая из Списка узел с данным ключом
            for (int i = 0; i < prevNodes.Length && prevNodes[i] != null; i++)
            {
                if (prevNodes[i].Right.Right != null)
                {
                    prevNodes[i].Right = prevNodes[i].Right.Right;
                    continue;
                }
                prevNodes[i].Right = null;
            }
            // проходим по заголовкам начиная с _currentLevel и проверяем, находятся ли справа какие то узлы
            // если не находятся, значит мы удалили единственный узел с этого уровня
            // в таком случае уменьшаем _currentLevel
            while (_currentLevel > 0 && _heads[_currentLevel].Right == null)
            {
                _currentLevel--;
            }
            Count--;
        }
        /// <summary>
        /// Поиск узла по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому ищем элемент</param>
        /// <returns>Узел с искомым ключом, если существует, иначе null</returns>
        private Node<TKey, TValue> Find(TKey key)
        {
            // пропускаем заголовки
            var currentNode = SkipDefaultNodes(key);
            if (currentNode == null)
            {
                return null;
            }
            // ищем узел с нужным ключом
            int compareResult;
            while (currentNode != null)
            {
                if (currentNode.Right != null)
                {
                    // сравниваем ключ правого узла с данным ключом
                    compareResult = currentNode.Right.Key.CompareTo(key);
                    if (compareResult == 0) // если нашли, возвращаем
                    {
                        return currentNode.Right;
                    }
                    if (compareResult < 0) // иначе идём вправо по текущему уровню и пропускаем элементы с ключами < key
                    {
                        while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                        {
                            currentNode = currentNode.Right;
                        }
                        continue;
                    }
                }
                // если на текущем уровне не нашли, идём ниже
                currentNode = currentNode.Down;
            }
            return null;
        }
        /// <summary>
        /// Пропускаем узлы-заголовки при поиске и удалении узлов
        /// </summary>
        /// <param name="key">Ключ, который мы ищем в Списке</param>
        /// <returns>Узел, полученный после пропуска заголовных узлов, null в случае если не нашли ключа < key</returns>
        private Node<TKey, TValue> SkipDefaultNodes(TKey key)
        {
            var node = _heads[_currentLevel];
            int compareResult;
            while (node != null)
            {
                if (node.Key.Equals(default(TKey)) && node.Right != null)
                {
                    compareResult = node.Right.Key.CompareTo(key);
                    if (compareResult <= 0)
                    {
                        return node;
                    }
                }
                node = node.Down;
            }
            return node;
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var node = _heads[0].Right; node != null; node = node.Right)
            {
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
