using HashTablesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataStructuresTestProject
{
    [TestClass]
    public class OpenAddressHashTableUnitTest
    {
        [TestMethod]
        public void Initialization_Correct()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            Assert.IsNotNull(hashTable);
        }
        [TestMethod]
        public void Count_EqualsZeroAfterInitialization()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var expectedCount = 0;
            Assert.AreEqual(expectedCount, hashTable.Count);
        }
        [TestMethod]
        public void Count_EqualsZeroAfterClearing()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            hashTable.Clear();
            var expectedCount = 0;
            Assert.AreEqual(expectedCount, hashTable.Count);
        }
        [TestMethod]
        public void Count_IncreasesAfterAdding()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            var expectedCount = 0;
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                    expectedCount++;
                    Assert.AreEqual(expectedCount, hashTable.Count);
                }
            }
        }
        [TestMethod]
        public void Count_DecreasesAfterRemoving()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            var expectedCount = array.Length;
            for (int i = 0; i < size / 2; i++)
            {
                hashTable.Remove(array[i]);
                Assert.AreEqual(--expectedCount, hashTable.Count);
            }
        }
        [TestMethod]
        public void Add_NonExistentKey_PairAdded()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                    Assert.IsTrue(array.Contains(item));
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_DuplicateKey_ThrowsException()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                hashTable.Add(array[i], array[i]);
            }
        }

        [TestMethod]
        public void Remove_ExistingKey_PairRemoved()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                hashTable.Remove(array[i]);
                Assert.IsFalse(hashTable.ContainsKey(array[i]));
            }
        }
        [TestMethod]
        public void Remove_NonExistentKey_ReturnsFalse()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    Assert.IsFalse(hashTable.Remove(item));
                }
            }
        }
        [TestMethod]
        public void Indexer_GetExistingKey_ValueTaken()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item + 1);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                Assert.AreEqual(array[i] + 1, hashTable[array[i]]);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Indexer_GetNonExistentKey_ThrowsException()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    var someKey = hashTable[item];
                }
            }
        }
        [TestMethod]
        public void Indexer_SetNonExistentKey_ItemAdded()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable[item] = item;
                    Assert.IsTrue(hashTable.ContainsKey(item));
                }
            }
        }
        [TestMethod]
        public void Indexer_SetExistingKey_ItemEdited()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                hashTable[array[i]] = array[i] + 1;
                Assert.AreEqual(array[i] + 1, hashTable[array[i]]);
            }
        }
        [TestMethod]
        public void TryGetValue_ExistingKey_ValueTakenAndCorrect()
        {

            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item + 1);
                }
            }
            int takenValue;
            for (int i = 0; i < size / 2; i++)
            {
                Assert.IsTrue(hashTable.TryGetValue(array[i], out takenValue));
                Assert.AreEqual(array[i] + 1, takenValue);
            }
        }
        [TestMethod]
        public void TryGetValue_NonExistentKey_ReturnsFalse()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item + 1);
                }
            }
            int takenValue;
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    Assert.IsFalse(hashTable.TryGetValue(item, out takenValue));
                }
            }
        }
        [TestMethod]
        public void CopyTo_ItemsCopied()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            KeyValuePair<int, int>[] newArray = new KeyValuePair<int, int>[size + 1];
            hashTable.CopyTo(newArray, 1);
            for (int i = 0; i < size / 2; i++)
            {
                Assert.IsTrue(newArray.Contains(new KeyValuePair<int, int>(array[i], array[i])));
            }
        }
        [TestMethod]
        public void Clear_ItemsDeleted()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            hashTable.Clear();
            for (int i = 0; i < size / 2; i++)
            {
                Assert.IsFalse(hashTable.ContainsKey(array[i]));
            }
        }
        [TestMethod]
        public void Keys_ShouldReturnAllKeys()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                Assert.IsTrue(hashTable.Keys.Contains(array[i]));
            }
        }
        [TestMethod]
        public void Values_ShouldReturnAllValues()
        {
            int size = 10103;
            var hashTable = new OpenAddressHashTable<int, int>(size);
            var array = new int[size / 2];
            var rnd = new Random();
            for (int i = 0; i < size / 2; i++)
            {
                var item = rnd.Next();
                if (!array.Contains(item))
                {
                    array[i] = item;
                    hashTable.Add(item, item * 2);
                }
            }
            for (int i = 0; i < size / 2; i++)
            {
                Assert.IsTrue(hashTable.Values.Contains(array[i] * 2));
            }
        }
    }
}
