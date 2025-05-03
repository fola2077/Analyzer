// TreeNode.cs
namespace starterCode
{
    // A single node in the binary tree
    public class TreeNode
    {
        public WordInfo Data;
        public TreeNode? Left, Right;

        public TreeNode(WordInfo data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
}