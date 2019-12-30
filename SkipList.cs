using System;
using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    /// <summary>
    /// Represents a SkipList.  A SkipList is a combination of a BST and a sorted link list, providing
    /// sub-linear access, insert, and deletion running times.  It is a randomized data structure, randomly
    /// choosing the heights of the nodes in the SkipList.
    /// </summary>
    /// <typeparam name="T">Type type of elements contained within the SkipList.</typeparam>
    public class SkipList<T> : ICollection<T>, IEnumerable<T>
    {
        SkipListNode<T> head;      // a reference to the head of the SkipList
        protected readonly double probability = 0.5;  // the probability used in determining the heights of the SkipListNodes
        private Random randomNum;
        private IComparer<T> comparer = Comparer<T>.Default;

        /// <summary>
        /// Returns the height of the tallest SkipListNode in the SkipList.
        /// </summary>
        public int Height => this.head.Height;

        /// <summary>
        /// Returns the number of total comparisons made - used for perf. testing.
        /// </summary>
        /// <value></value>
        public long ComparisonCount { get; private set; }

        /// <summary>
        /// Returns the number of elements in the SkipList
        /// </summary>
        public int Count {get; private set;}

        public bool IsReadOnly => throw new NotImplementedException();

        public SkipList() : this(-1, null) 
        {            
        }

        public SkipList(int randomSeed) : this(randomSeed, null) 
        {            
        }

        public SkipList(IComparer<T> comparer) : this(-1, comparer) 
        {            
        }

        public SkipList(int randomSeed, IComparer<T> comparer)
        {
            this.head = new SkipListNode<T>(1);
            this.ComparisonCount = 0;
            this.Count = 0;
            this.Count++;
            if (randomSeed < 0)
                randomNum = new Random();
            else
                randomNum = new Random(randomSeed);

            if (comparer != null)
                this.comparer = comparer;
        }

        /// <summary>
        /// Selects a height for a new SkipListNode using the "loaded dice" technique.
        /// The value selected is between 1 and maxLevel.
        /// </summary>
        /// <param name="maxLevel">The maximum value ChooseRandomHeight can return.</param>
        /// <returns>A randomly chosen integer value between 1 and maxLevel.</returns>
        protected virtual int ChooseRandomHeight(int maxLevel)
        {
            int level = 1;

            while (this.randomNum.NextDouble() < this.probability && level < maxLevel)
                level++;

            return level;
        }

        // Clears out the contents of the SkipList and creates a new head, with height 1.
        public void Clear()
        {
            // create a new head
            this.head = null;
            this.head = new SkipListNode<T>(1);
            this.Count = 0;
        }

        /// <summary>
        /// Determines if a particular element is contained within the SkipList.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if value is found in the SkipList; false otherwise.</returns>
        public bool Contains(T value)
        {
            return this.FindByValue(value) != null;
        }

        public SkipListNode<T> FindByValue(T value)
        {
            SkipListNode<T> current = this.head;

            // first, determine the nodes that need to be updated at each level
            for (int i = this.head.Height - 1; i >= 0; i--)
            {
                while (current[i] != null)
                {
                    this.ComparisonCount++;
                    int results = this.comparer.Compare(current[i].Value, value);
                    if (results == 0)
                        return current[i];        // we found the item
                    else if (results < 0)
                        current = current[i];   // move on to the next neighbor
                    else
                        break;	// exit while loop, we need to move down the height of the current node
                }
            }

            // if we reach here, we searched to the end of the list without finding the element
            return null;
        }

        /// <summary>
        /// Adds a new element to the SkipList.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <remarks>This SkipList implementation does not allow for duplicates.  Attempting to add a
        /// duplicate value will not raise an exception, it will simply exit the method without
        /// changing the SkipList.</remarks>
        public void Add(T value)
        {
            SkipListNode<T>[] updates = this.BuildUpdateTable(value);
            SkipListNode<T> current = updates[0];

            // see if a duplicate is being inserted
            if (current[0] != null && this.comparer.Compare(current[0].Value, value) == 0)
                // cannot enter a duplicate, handle this case by either just returning or by throwing an exception
                return;

            // create a new node
            SkipListNode<T> n = new SkipListNode<T>(value, this.ChooseRandomHeight(head.Height + 1));
            this.Count++;

            // if the node's level is greater than the head's level, increase the head's level
            if (n.Height > this.head.Height)
            {
                this.head.IncrementHeight();
                this.head[this.head.Height - 1] = n;
            }

            // splice the new node into the list
            for (int i = 0; i < n.Height; i++)
            {
                if (i < updates.Length)
                {
                    n[i] = updates[i][i];
                    updates[i][i] = n;
                }
            }
        }
        
        /// <summary>
        /// Attempts to remove a value from the SkipList.
        /// </summary>
        /// <param name="value">The value to remove from the SkipList.</param>
        /// <returns>True if the value is found and removed; false if the value is not found
        /// in the SkipList.</returns>
        public bool Remove(T value)
        {
            SkipListNode<T>[] updates = this.BuildUpdateTable(value);
            SkipListNode<T> current = updates[0][0];

            if (current != null && this.comparer.Compare(current.Value, value) == 0)
            {
                this.Count--;

                // We found the data to delete
                for (int i = 0; i < this.head.Height; i++)
                {
                    if (updates[i][i] != current)
                        break;
                    else
                        updates[i][i] = current[i];
                }

                // finally, see if we need to trim the height of the list
                if (this.head[this.head.Height - 1] == null)
                    // we removed the single, tallest item... reduce the list height
                    this.head.DecrementHeight();

                current = null;

                return true;        // the item was successfully removed
            }
            else
                // the data to delete wasn't found.
                return false;
        }

        /// <summary>
        /// Creates a table of the SkipListNode instances that will need to be updated when an item is
        /// added or removed from the SkipList.
        /// </summary>
        /// <param name="value">The value to be added or removed.</param>
        /// <returns>An array of SkipListNode instances, as many as the height of the head node.
        /// A SkipListNode instance in array index k represents the SkipListNode at height k that must
        /// be updated following the addition/deletion.</returns>
        protected SkipListNode<T>[] BuildUpdateTable(T value)
        {
            SkipListNode<T>[] updates = new SkipListNode<T>[this.head.Height];
            SkipListNode<T> current = this.head;

            // determine the nodes that need to be updated at each level
            for (int i = this.head.Height - 1; i >= 0; i--)
            {
                if (!(current[i] != null && this.comparer.Compare(current[i].Value, value) < 0))
                    this.ComparisonCount++;

                while (current[i] != null && this.comparer.Compare(current[i].Value, value) < 0)
                {
                    current = current[i];
                    this.ComparisonCount++;
                }

                updates[i] = current;
            }

            return updates;
        }

        /// <summary>
        /// Copies the contents of the SkipList to the passed-in array.
        /// </summary>
        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        /// <summary>
        /// Copies the contents of the SkipList to the passed-in array.
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            // copy the values from the skip list to array
            if (array == null)
                throw new ArgumentNullException("array is null");

            if (index < 0)
                throw new ArgumentOutOfRangeException("index is less than 0");

            if (index >= array.Length)
                throw new ArithmeticException("index is greater than the length of array");

            if (array.Length - index <= this.Count)
                throw new ArgumentException("insufficient space in array to store skip list starting at specified index");

            SkipListNode<T> current = this.head[0];
            int i = 0;
            while (current != null)
            {
                array[i + index] = current.Value;
                i++;
            }
        }

        /// <summary>
        /// Returns an enumerator to access the contents of the SkipList.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            // enumerate through the skip list one element at a time
            SkipListNode<T> current = this.head[0];
            while (current != null)
            {
                yield return current.Value;
                current = current[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// This overridden form of ToString() is simply for displaying detailed information
        /// about the contents of the SkipList, used by SkipListTester - feel free to remove it.
        /// </summary>
        public override string ToString()
        {
            SkipListNode<T> current = this.head[0];
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            while (current != null)
            {
                stringBuilder.Append(current.Value.ToString());
                stringBuilder.Append(" [ H=").Append(current.Height);
                for (int i = current.Height - 1; i >= 0; i--)
                {
                    stringBuilder.Append(" | ");
                    if (current[i] == null)
                        stringBuilder.Append("NULL");
                    else
                        stringBuilder.Append(current[i].Value.ToString());
                }
                stringBuilder.Append(" ] ; ");

                current = current[0];
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Resets the internal comparison counter back to zero.  Used for performance testing (can be removed).
        /// </summary>
        public void ResetComparisons()
        {
            this.ComparisonCount = 0;
        }
    }
}