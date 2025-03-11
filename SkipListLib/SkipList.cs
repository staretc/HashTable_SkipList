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
        private Node<TKey, TValue>[] _head;
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
            _head = new Node<TKey, TValue>[_maxLevel];
            _head[0] = new Node<TKey, TValue>();
            for (int i = 1; i < maxLevel; i++)
            {
                _head[i] = new Node<TKey, TValue>();
                _head[i - 1].Up = _head[i];
                _head[i].Down = _head[i - 1];
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
            var prevNode = new Node<TKey, TValue>[_maxLevel]; // для записи узлов каждого уровня, на которых остановились
            var currentNode = _head[_currentLevel];
            // проходим по уровням
            for (int i = _currentLevel; i >= 0; i--)
            {
                // двигаемся по уровню пока следующий элемент < текущего элемента
                while (currentNode.Right != null && currentNode.Right.Key.CompareTo(key) < 0)
                {
                    currentNode = currentNode.Right;
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
            int level = 0;
            while (_random.NextDouble() < _probability && level < _maxLevel - 1)
            {
                level++;
            }
            while (_currentLevel < level)
            {
                // обновляем текущий самый нижний уровень 
                _currentLevel++;
                prevNode[_currentLevel] = _head[_currentLevel];
            }
            for (int i = 0; i <= level; i++)
            {
                var node = new Node<TKey, TValue>(key, value) { Right = prevNode[i].Right };
                prevNode[i].Right = node;
                if (i == 0) continue;
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
        public bool Contains(TKey key)
        {
            return true;
        }
        /// <summary>
        /// Удаление узла из Списка по ключу
        /// </summary>
        /// <param name="key">Ключ, по которому необходимо удалить узел</param>
        public void Remove(TKey key)
        {

        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (var node = _head[0].Right; node != null; node = node.Right)
            {
                yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
