using System;
using System.Collections.Generic;

namespace starterCode
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        private sealed class Node { public T Data; public Node? L, R; public Node(T d){Data=d;} }

        private Node? _root;
        public int Count { get; private set; }

        public void Insert(T item){ Insert(ref _root,item); Count++; }

        private static void Insert(ref Node? n, T item)
        {
            if(n==null) n=new Node(item);
            else if(item.CompareTo(n.Data)<0) Insert(ref n.L,item);
            else if(item.CompareTo(n.Data)>0) Insert(ref n.R,item);
        }

        public bool TryGet(T probe, out T found)
        {
            var cur=_root;
            while(cur!=null)
            {
                int c=probe.CompareTo(cur.Data);
                if(c==0){found=cur.Data;return true;}
                cur=c<0?cur.L:cur.R;
            }
            found=default!; return false;
        }

        public IEnumerable<T> InOrder()=>WalkIn(_root);
        private IEnumerable<T> WalkIn(Node? n)
        { if(n!=null){foreach(var x in WalkIn(n.L))yield return x; yield return n.Data; foreach(var x in WalkIn(n.R))yield return x;} }

        public IEnumerable<T> PreOrder()=>WalkPre(_root);
        private IEnumerable<T> WalkPre(Node? n)
        { if(n!=null){yield return n.Data; foreach(var x in WalkPre(n.L))yield return x; foreach(var x in WalkPre(n.R))yield return x;} }
    }
}
