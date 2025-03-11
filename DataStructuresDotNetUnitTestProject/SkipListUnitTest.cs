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
        public void Count_IncreasesAfterAdding()
        {
            int n = 10;
            var lib = new SkipList<int, int>();

            for (int i = 0; i < n; i++)
            {
                lib.Add(i, i);
            }
            Assert.AreEqual(n, lib.Count);
        }
        [TestMethod]
        public void Add_StaticItems_ExistAfterAdding()
        {
            var lib = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1 , 56, 3, 90, 31, 15, 26 });
            for (int i = 0; i < nums.Count; i++)
            {
                lib.Add(nums[i], i);
            }
            foreach(var num in nums)
            {
                Assert.IsTrue(lib.Contains(num));
            }
            Assert.AreEqual(nums.Count, lib.Count);
        }
        [TestMethod]
        public void Add_RandomItems_ExistAfterAdding()
        {
            var lib = new SkipList<int, int>();
            var nums = new HashSet<int>();
            var rd = new Random();
            int n = 100;
            while (nums.Count < n)
            {
                nums.Add(rd.Next(1, n * 3));
            }
            foreach(var item in nums)
            {
                lib.Add(item, 1);
            }

            var a = nums.ToList();
            a.Sort();
            int j = 0;
            foreach (var pair in lib)
            {
                Assert.AreEqual(a[j], pair.Key);
                j++;
            }
            Assert.AreEqual(n, lib.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_DuplicateKey_ThrowsException()
        {
            var lib = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            for (int i = 0; i < nums.Count; i++)
            {
                lib.Add(nums[i], i);
            }
            for (int i = 0; i < nums.Count; i++)
            {
                lib.Add(nums[i], i);
            }
        }
    }
}
