using System.Linq;

namespace Graph
{
    public class BinaryTreeNode<T> : Node<T>
    {
        public BinaryTreeNode() : base() 
        {            
        }

        public BinaryTreeNode(T data) : base(data, null) 
        {            
        }

        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            base.Value = data;
            base.Neighbors = new NodeList<T>() {left, right}; // set link to children
        }

        public BinaryTreeNode<T> Left
        {
            get
            {
                return (BinaryTreeNode<T>) base.Neighbors?.FirstOrDefault();
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                return (BinaryTreeNode<T>) base.Neighbors?.Skip(1)?.FirstOrDefault();
            }
            set
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>(2);

                base.Neighbors[1] = value;
            }
        }
    }
}