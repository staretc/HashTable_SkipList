using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkipListLib;

namespace SkipListUnitTest
{
    [TestClass]
    public class SkipListUnitTest
    {
        [TestMethod]
        public void Initialization_Correct()
        {
            int size = 10;
            var skipList = new SkipList<int, int>(size);
            Assert.IsNotNull(skipList);
        }
        [TestMethod]
        public void Count_IncreasesAfterAdding()
        {
            int n = 10;
            var skipList = new SkipList<int, int>();

            for (int i = 0; i < n; i++)
            {
                skipList.Add(i, i);
            }
            Assert.AreEqual(n, skipList.Count);
        }
        [TestMethod]
        public void Add_StaticItems_ExistAfterAdding()
        {
            var skipList = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1 , 56, 3, 90, 31, 15, 26 });
            for (int i = 0; i < nums.Count; i++)
            {
                skipList.Add(nums[i], i);
            }
            foreach(var num in nums)
            {
                Assert.IsTrue(skipList.ContainsKey(num));
            }
            Assert.AreEqual(nums.Count, skipList.Count);
        }
        [TestMethod]
        public void Add_RandomItems_ExistAfterAdding()
        {
            var skipList = new SkipList<int, int>();
            var nums = new HashSet<int>();
            var rd = new Random();
            int n = 100;
            while (nums.Count < n)
            {
                nums.Add(rd.Next(1, n * 3));
            }
            foreach(var item in nums)
            {
                skipList.Add(item, 1);
            }

            var a = nums.ToList();
            a.Sort();
            int j = 0;
            foreach (var pair in skipList)
            {
                Assert.AreEqual(a[j], pair.Key);
                j++;
            }
            Assert.AreEqual(n, skipList.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_DuplicateKey_ThrowsException()
        {
            var skipList = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            for (int i = 0; i < nums.Count; i++)
            {
                skipList.Add(nums[i], i);
            }
            for (int i = 0; i < nums.Count; i++)
            {
                skipList.Add(nums[i], i);
            }
        }
        [TestMethod]
        public void Remove_ExistingKey_RemovedCorrectly()
        {
            var skipList = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            for (int i = 0; i<nums.Count; i++)
            {
                skipList.Add(nums[i], i);
            }
            for (int i = 0; i < nums.Count; i++)
            {
                skipList.Remove(nums[i]);
                Assert.IsFalse(skipList.ContainsKey(nums[i]));
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Remove_NonExistentKey_ThrowsException()
        {
            var skipList = new SkipList<int, int>();
            var nums = new HashSet<int>();
            var rd = new Random();
            int n = 100;
            while (nums.Count < n)
            {
                nums.Add(rd.Next(1, n * 3));
            }
            foreach (var item in nums)
            {
                skipList.Add(item, 1);
            }
            for (int i = 1; i <= n; i++)
            {
                var num = rd.Next(1, i * 3);
                if (!nums.Contains(num))
                {
                    skipList.Remove(num);
                }
            }
        }

    }
}
