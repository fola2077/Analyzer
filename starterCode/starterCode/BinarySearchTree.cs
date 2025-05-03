// BinaryTree.cs
using System;
using System.Collections.Generic;

namespace starterCode
{
    // Binary Search Tree that holds WordInfo objects
    public class BinaryTree
    {
        private TreeNode? root;
        private int count = 0;

        public int Count => count;

        public void Insert(string word, int line)
        {
            root = InsertRecursive(root, word, line);
        }

        private TreeNode InsertRecursive(TreeNode? node, string word, int line)
        {
            if (node == null)
            {
                count++;
                return new TreeNode(new WordInfo(word, line));
            }

            int cmp = string.Compare(word, node.Data.Word, StringComparison.OrdinalIgnoreCase);
            if (cmp < 0)
            {
                node.Left = InsertRecursive(node.Left, word, line);
            }
            else if (cmp > 0)
            {
                node.Right = InsertRecursive(node.Right, word, line);
            }
            else
            {
                node.Data.AddOccurrence(line);
            }

            return node;
        }

        public WordInfo? Search(string word)
        {
            TreeNode? current = root;
            while (current != null)
            {
                int cmp = string.Compare(word, current.Data.Word, StringComparison.OrdinalIgnoreCase);
                if (cmp == 0) return current.Data;
                current = (cmp < 0) ? current.Left : current.Right;
            }
            return null;
        }

        public IEnumerable<WordInfo> InOrder()
        {
            return InOrderTraversal(root);
        }

        private IEnumerable<WordInfo> InOrderTraversal(TreeNode? node)
        {
            if (node != null)
            {
                foreach (var left in InOrderTraversal(node.Left)) yield return left;
                yield return node.Data;
                foreach (var right in InOrderTraversal(node.Right)) yield return right;
            }
        }

        public IEnumerable<WordInfo> PreOrder()
        {
            return PreOrderTraversal(root);
        }

        private IEnumerable<WordInfo> PreOrderTraversal(TreeNode? node)
        {
            if (node != null)
            {
                yield return node.Data;
                foreach (var left in PreOrderTraversal(node.Left)) yield return left;
                foreach (var right in PreOrderTraversal(node.Right)) yield return right;
            }
        }

        public WordInfo? FindMostFrequent()
        {
            WordInfo? mostFrequent = null;
            int max = 0;
            foreach (var info in InOrder())
            {
                if (info.Frequency > max)
                {
                    mostFrequent = info;
                    max = info.Frequency;
                }
            }
            return mostFrequent;
        }

        public WordInfo? FindLongestWord()
        {
            WordInfo? longest = null;
            foreach (var info in InOrder())
            {
                if (longest == null || info.Word.Length > longest.Word.Length)
                {
                    longest = info;
                }
            }
            return longest;
        }
    }
}
