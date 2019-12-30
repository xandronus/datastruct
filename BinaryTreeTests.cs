using Xunit;

namespace Graph
{
    public class BinaryTreeTests
    {
        [Fact]
        public void CreateBinaryTreeTest()
        {
            var btree = this.CreateSampleTree();
            Assert.NotNull(btree);
        }

        // https://docs.microsoft.com/en-us/previous-versions/ms379572%28v%3dvs.80%29
        // ie. sort ascending by value
        [Fact]
        public void InorderTransversalOfBinarySearchTreeTest()
        {
            var tree = this.GetSampleBST();
            int[] inorder = new int[tree.Count];
            tree.CopyTo(inorder, 0);
            
            int index = 0;
            foreach(var node in inorder)
            {
                Assert.Equal(node, inorderExpected[index]);
                index++;
            }
        }

        // https://docs.microsoft.com/en-us/previous-versions/ms379572%28v%3dvs.80%29
        // ie- Reverse of how it was created
        // Postorder traversal is used to delete the tree. Please see the question for deletion of tree for details. Postorder traversal is also useful to get the postfix expression of an expression tree. Please see http://en.wikipedia.org/wiki/Reverse_Polish_notation to for the usage of postfix expression.
        // Useful for pruning the graph of irrelevant nodes
        [Fact]
        public void PostorderTransversalOfBinarySearchTreeTest()
        {
            var tree = this.GetSampleBST();
            int[] postorder = new int[tree.Count];
            tree.CopyTo(postorder, 0, TraversalMethod.Postorder);
            
            int index = 0;
            foreach(var node in postorder)
            {
                Assert.Equal(node, postorderExpected[index]);
                index++;
            }
        }

        // https://docs.microsoft.com/en-us/previous-versions/ms379572%28v%3dvs.80%29
        // ie- By branch from root
        // Preorder traversal is used to create a copy of the tree. Preorder traversal is also used to get prefix expression on of an expression tree. Please see http://en.wikipedia.org/wiki/Polish_notation to know why prefix expressions are useful.
        [Fact]
        public void PreorderTransversalOfBinarySearchTreeTest()
        {
            var tree = this.GetSampleBST();
            int[] preorder = new int[tree.Count];
            tree.CopyTo(preorder, 0, TraversalMethod.Preorder);
            
            int index = 0;
            foreach(var node in preorder)
            {
                Assert.Equal(node, preorderExpected[index]);
                index++;
            }
        }        

        private BinaryTree<int> CreateSampleTree()
        {
            BinaryTree<int> btree = new BinaryTree<int>();
            btree.Root = new BinaryTreeNode<int>(1);
            btree.Root.Left = new BinaryTreeNode<int>(2);
            btree.Root.Right = new BinaryTreeNode<int>(3);

            btree.Root.Left.Left = new BinaryTreeNode<int>(4);
            btree.Root.Right.Right = new BinaryTreeNode<int>(5);

            btree.Root.Left.Left.Right = new BinaryTreeNode<int>(6);
            btree.Root.Right.Right.Right = new BinaryTreeNode<int>(7);

            btree.Root.Right.Right.Right.Right = new BinaryTreeNode<int>(8);
            return btree;
        }

        private int[] preorderExpected = {90, 50, 20, 5, 25, 75, 66, 80, 150, 95, 92, 111, 175, 166, 200};
        private int[] postorderExpected = {5, 25, 20, 66, 80, 75, 50, 92, 111, 95, 166, 200, 175, 150, 90};
        private int[] inorderExpected = {5, 20, 25, 50, 66, 75, 80, 90, 92, 95, 111, 150, 166, 175, 200};        

        private BinarySearchTree<int> GetSampleBST()
        {
            BinarySearchTree<int> tree = new BinarySearchTree<int>();
            foreach (var node in preorderExpected)
            {
                tree.Add(node);
            }

            return tree;
        }
    }
}