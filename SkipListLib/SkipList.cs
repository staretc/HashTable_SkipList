using SkipListLib;
using System;
using System.Collections;
using System.Collections.Generic;

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
            // определяем, на какой уровень опустим новый элемент
            // это может быть как существующий уровень, так и новый (если можно добавить новые уровни)
            // всё зависит от рандома и вероятности перехода на новый уровень
            int level = 0;
            while (_random.NextDouble() < _probability && level < _maxLevel - 1)
            {
                level++;
            }

            var prevNode = new Node<TKey, TValue>[level + 1]; // для записи узлов на которых остановились при поиске места вставки на каждом уровне
            while (_currentLevel < level)
            {
                // обновляем текущее количество уровней
                _currentLevel++;
                // заполняем ячейку массива головным элементом данного уровня (до этого она была пустая)
                // на новых уровнях мы вставим элемент сразу после головного
                prevNode[_currentLevel] = _heads[_currentLevel];
            }
            
            var currentNode = _heads[level];
            // проходим по уровням и определяем на какое место необходимо вставить узел на каждом уровне
            for (int i = level; i >= 0; i--)
            {
                // двигаемся по уровню пока следующий ключ < вставляемого ключа
                while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
                }
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
        /// Добавление первого элемента в Список
        /// </summary>
        /// <param name="key">Ключ узла</param>
        /// <param name="value">Значение узла по ключу</param>
        private void AddFirstItem(TKey key, TValue value)
        {
            foreach (var head in _heads)
            {
                head.Right = new Node<TKey, TValue>(key, value);
            }
        }
        /// <summary>
        /// Проверка наличия узла в Списке по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому ищем узел</param>
        /// <returns>True если ключ содержится в списке, иначе False</returns>
        public bool Contains(TKey key)
        {
            return Find(key) != null;
        }
        /// <summary>
        /// Удаление узла из Списка по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому необходимо удалить узел</param>
        public void Remove(TKey key)
        {
            var deletingNode = Find(key);
            // удаляем со всех уровней где есть этот элемент
        }


        /// <summary>
        /// Поиск узла по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому ищем элемент</param>
        /// <returns>Узел с искомым ключом, если существует, иначе null</returns>
        private Node<TKey, TValue> Find(TKey key)
        {
            var currentNode = _heads[_currentLevel];
            int compareResult;
            while (currentNode != null)
            {
                if (currentNode.Key.Equals(default))
                {
                    compareResult = currentNode.Right.Key.CompareTo(key);
                    if (compareResult == 0)
                    {
                        return currentNode.Right;
                    }
                    if (compareResult < 0)
                    {
                        currentNode = currentNode.Right;
                        continue;
                    }
                    currentNode = currentNode.Down;
                    continue;
                }
                if (currentNode.Right != null)
                {
                    compareResult = currentNode.Right.Key.CompareTo(key);
                    if (compareResult == 0)
                    {
                        return currentNode.Right;
                    }
                    if (compareResult < 0)
                    {
                        while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                        {
                            currentNode = currentNode.Right;
                        }
                        continue;
                    }
                }
                currentNode = currentNode.Down;
            }
            return null;
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
