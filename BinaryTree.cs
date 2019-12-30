namespace Graph
{
    public class BinaryTree<T>
        {
        public BinaryTree()
        {
        }

        public virtual void Clear()
        {
            Root = null;
        }

        public BinaryTreeNode<T> Root {get; set;}
    }
}