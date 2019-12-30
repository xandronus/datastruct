using System;
using Xunit;

namespace Graph
{
    public class SkipListTests
    {
        [Fact]
        public void CreateSkipListTest()
        {
            var skipList = this.CreateSkipList();
            Assert.NotNull(skipList);
        }

        [Fact]
        public void SearchInSkipListTest()
        {
            var skipList = this.CreateSkipList();
            var ed = skipList.FindByValue("Ed");
            Assert.NotNull(ed?.Value);
            Assert.Equal("Ed", ed.Value);
        }

        private string[] items = {"Alice", "Bob", "Cal", "Dave", "Ed", "Frank", "Gil", "Hank"};
        private SkipList<string> CreateSkipList()
        {
            var skipList = new SkipList<string>();
            foreach(var item in this.items)
            {
                skipList.Add(item);
            }

            return skipList;
        }
    }
}
