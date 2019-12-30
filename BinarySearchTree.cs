using System;
using System.Collections;
using System.Collections.Generic;

namespace Graph
{
    public enum TraversalMethod
    {
        Preorder,
        Inorder,
        Postorder
    }

    /// <summary>
    /// Represents a binary search tree.  A binary search tree is a binary tree whose nodes are arranged
    /// such that for any given node k, all nodes in k's left subtree have a value less than k, and all
    /// nodes in k's right subtree have a value greater than k.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the binary tree nodes.</typeparam>
    public class BinarySearchTree<T> : BinaryTree<T>, IEnumerable<T>
    {
        private IComparer<T> comparer = Comparer<T>.Default;    // used to compare node values when percolating down the tree

        public int Count {get; private set;} = 0;
        
        public BinarySearchTree() 
        {             
        }

        public BinarySearchTree(IComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public override void Clear()
        {
            this.Root = null;
            this.Count = 0;
        }

        public bool Contains(T data)
        {
            // search the tree for a node that contains data
            BinaryTreeNode<T> current = this.Root;
            while (current != null)
            {
                int result = this.comparer.Compare(current.Value, data);
                if (result > 0)
                {
                     // current.Value > data, search current's left subtree                    
                    current = current.Left;
                }
                else if (result < 0)
                { // current.Value < data, search current's right subtree
                    current = current.Right;
                }
                else // result == 0
                {
                    // we found data
                    return true;
                }
            }

            return false; // didn't find data
        }

        public virtual void Add(T data)
        {
            // trace down the tree until we hit a NULL
            var parent = this.FindInsertParent(data); 

            // Add new node to parent
            var newNode = new BinaryTreeNode<T>(data);
            if (parent == null)
            {
                // the tree was empty, make n the root
                this.Root = newNode; 
            }
            else
            {
                int result = comparer.Compare(parent.Value, data);
                if (result > 0)
                {
                    // parent.Value > data, therefore n must be added to the left subtree
                    parent.Left = newNode;
                }
                else
                {
                    // parent.Value < data, therefore n must be added to the right subtree
                    parent.Right = newNode;
                }
            }
            this.Count++;
        }

        public bool Remove(T data)
        {
            // first make sure there exist some items in this tree
            if (this.Root == null)
                return false;       // no items to remove

            // Now, try to find data in the tree
            BinaryTreeNode<T> current = this.Root;
            BinaryTreeNode<T> parent = null;
            int result = comparer.Compare(current.Value, data);
            while (result != 0)
            {
                if (result > 0)
                {
                    // current.Value > data, if data exists it's in the left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, if data exists it's in the right subtree
                    parent = current;
                    current = current.Right;
                }

                // If current == null, then we didn't find the item to remove
                if (current == null)
                    return false;
                else
                    result = comparer.Compare(current.Value, data);
            }

            // We now need to "rethread" the tree
            // CASE 1: If current has no right child, then current's left child becomes
            //         the node pointed to by the parent
            if (current.Right == null)
            {
                if (parent == null)
                {
                    this.Root = current.Left;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        // parent.Value > current.Value, so make current's left child a left child of parent
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        // parent.Value < current.Value, so make current's left child a right child of parent
                        parent.Right = current.Left;
                    }
                }                
            }
            // CASE 2: If current's right child has no left child, then current's right child
            //         replaces current in the tree
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    this.Root = current.Right;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        // parent.Value > current.Value, so make current's right child a left child of parent
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        // parent.Value < current.Value, so make current's right child a right child of parent
                        parent.Right = current.Right;
                    }
                }
            }
            // CASE 3: If current's right child has a left child, replace current with current's
            //          right child's left-most descendent
            else
            {
                // We first need to find the right node's left-most child
                var leftmost = current.Right.Left;
                var leftmostParent = current.Right;
                while (leftmost.Left != null)
                {
                    leftmostParent = leftmost;
                    leftmost = leftmost.Left;
                }

                // the parent's left subtree becomes the leftmost's right subtree
                leftmostParent.Left = leftmost.Right;

                // assign leftmost's left and right to current's left and right children
                leftmost.Left = current.Left;
                leftmost.Right = current.Right;

                if (parent == null)
                {
                    this.Root = leftmost;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        // parent.Value > current.Value, so make leftmost a left child of parent
                        parent.Left = leftmost;
                    }
                    else if (result < 0)
                    {
                        // parent.Value < current.Value, so make leftmost a right child of parent
                        parent.Right = leftmost;
                    }
                }
            }

            // Update count for removed node
            this.Count--;

            //TODO: Don't think I need this - the node should be gonezo
            // Clear out the values from current
            //current.Left = current.Right = null; 
            //current = null;

            return true;
        }

        public void CopyTo(T[] array, int index)
        {
            CopyTo(array, index, TraversalMethod.Inorder);
        }

        /// <summary>
        /// Copies the contents of the BST to an appropriately-sized array of type T, using a specified
        /// traversal method.
        /// </summary>
        public void CopyTo(T[] array, int index, TraversalMethod TraversalMethod)
        {
            IEnumerable<T> enumProp = null;

            // Determine which Enumerator-returning property to use, based on the TraversalMethod input parameter
            switch (TraversalMethod)
            {
                case TraversalMethod.Preorder:
                    enumProp = PreorderEnumerable;
                    break;

                case TraversalMethod.Inorder:
                    enumProp = InorderEnumerable;
                    break;

                case TraversalMethod.Postorder:
                default:
                    enumProp = PostorderEnumerable;
                    break;
            }

            // dump the contents of the tree into the passed-in array
            int i = 0;
            foreach (T value in enumProp)
            {
                array[i + index] = value;
                i++;
            }
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(TraversalMethod.Inorder);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(TraversalMethod.Inorder);
        }

        /// <summary>
        /// Enumerates the BST's contents using a specified traversal method.
        /// </summary>
        /// <param name="TraversalMethod">The type of traversal to perform.</param>
        /// <returns>An enumerator that provides access to the BST's elements using a specified traversal technique.</returns>
        public virtual IEnumerator<T> GetEnumerator(TraversalMethod TraversalMethod)
        {
            // The traversal approaches are defined as public properties in the BST class...
            // This method simply returns the appropriate property.
            switch (TraversalMethod)
            {
                case TraversalMethod.Preorder:
                    return PreorderEnumerable.GetEnumerator();

                case TraversalMethod.Inorder:
                    return InorderEnumerable.GetEnumerator();

                case TraversalMethod.Postorder:
                default:
                    return PostorderEnumerable.GetEnumerator();
            }
        }

        /// <summary>
        /// Provides enumeration through the BST using preorder traversal.
        /// </summary>
        public IEnumerable<T> PreorderEnumerable
        {
            get
            {
                // A single stack is sufficient here - it simply maintains the correct
                // order with which to process the children.
                Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>(this.Count);
                BinaryTreeNode<T> current = this.Root;
                if (current != null) 
                    toVisit.Push(current);

                while (toVisit.Count != 0)
                {
                    // take the top item from the stack
                    current = toVisit.Pop();

                    // add the right and left children, if not null
                    if (current.Right != null) 
                        toVisit.Push(current.Right);
                    if (current.Left != null) 
                        toVisit.Push(current.Left);

                    // return the current node
                    yield return current.Value;
                }
            }
        }

        /// <summary>
        /// Provides enumeration through the BST using inorder traversal.
        /// </summary>
        public IEnumerable<T> InorderEnumerable
        {
            get
            {
                // A single stack is sufficient - this code was made available by Grant Richins:
                // http://blogs.msdn.com/grantri/archive/2004/04/08/110165.aspx
                var toVisit = new Stack<BinaryTreeNode<T>>(Count);
                for (BinaryTreeNode<T> current = this.Root; current != null || toVisit.Count != 0; current = current.Right)
                {
                    // Get the left-most item in the subtree, remembering the path taken
                    while (current != null)
                    {
                        toVisit.Push(current);
                        current = current.Left;
                    }

                    current = toVisit.Pop();
                    yield return current.Value;
                }
            }
        }

        /// <summary>
        /// Provides enumeration through the BST using postorder traversal.
        /// </summary>
        public IEnumerable<T> PostorderEnumerable
        {
            get
            {
                // maintain two stacks, one of a list of nodes to visit,
                // and one of booleans, indicating if the note has been processed
                // or not.
                var toVisit = new Stack<BinaryTreeNode<T>>(this.Count);
                var hasBeenProcessed = new Stack<bool>(this.Count);
                var current = this.Root;
                if (current != null)
                {
                    toVisit.Push(current);
                    hasBeenProcessed.Push(false);
                    current = current.Left;
                }

                while (toVisit.Count != 0)
                {
                    if (current != null)
                    {
                        // add this node to the stack with a false processed value
                        toVisit.Push(current);
                        hasBeenProcessed.Push(false);
                        current = current.Left;
                    }
                    else
                    {
                        // see if the node on the stack has been processed
                        bool processed = hasBeenProcessed.Pop();
                        var node = toVisit.Pop();
                        if (!processed)
                        {
                            // if it's not been processed, "recurse" down the right subtree
                            toVisit.Push(node);
                            hasBeenProcessed.Push(true);    // it's now been processed
                            current = node.Right;
                        }
                        else
                            yield return node.Value;
                    }
                }
            }
        }
   
        private BinaryTreeNode<T> FindInsertParent(T data)
        {
            BinaryTreeNode<T> current = this.Root;
            BinaryTreeNode<T> parent = null;
            while (current != null)
            {
                int result = comparer.Compare(current.Value, data);
                if (result == 0)
                {
                    // they are equal - attempting to enter a duplicate
                    throw new ArgumentException($"Value {data} already exists.");
                }
                else if (result > 0)
                {
                    // current.Value > data, must add n to current's left subtree
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // current.Value < data, must add n to current's right subtree
                    parent = current;
                    current = current.Right;
                }
            }

            return parent;
        }
    }
}