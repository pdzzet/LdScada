using System.Collections.Generic;
using System.Linq;

namespace KHBC.Core.Extend
{
    /// <summary>
    /// 树集合（多层）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> where T : new()
    {
        /// <summary>
        /// 对象
        /// </summary>
        public T TreeNode { get; set; }

        /// <summary>
        /// 子对象集合
        /// </summary>
        public List<Tree<T>> ChildTrees { get; set; } = new List<Tree<T>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public Tree(T node)
        {
            TreeNode = node;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childTrees"></param>
        public Tree(T node, List<Tree<T>> childTrees)
        {
            TreeNode = node;
            ChildTrees = childTrees;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childNodes"></param>
        public Tree(T node, List<T> childNodes)
        {
            TreeNode = node;
            ChildTrees = childNodes.Select(x => new Tree<T>(x)).ToList();
        }
    }

    /// <summary>
    /// 树集合（多层）
    /// </summary>
    /// <typeparam name="T">主干类型（可以支持多层）</typeparam>
    /// <typeparam name="T2">次类型（不支持多层）</typeparam>
    public class Tree<T, T2> where T : new() where T2 : new()
    {
        /// <summary>
        /// 对象
        /// </summary>
        public T TreeNode { get; set; }
        /// <summary>
        /// 节点名
        /// </summary>
        public string TreeName { get; set; }
        /// <summary>
        /// 预留
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// 子分支
        /// </summary>
        public List<Tree<T, T2>> ChildTrees { get; set; } = new List<Tree<T, T2>>();

        /// <summary>
        /// 第二分支
        /// </summary>
        public List<T2> SecondNodes { get; set; } = new List<T2>();
        public Tree()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public Tree(T node)
        {
            TreeNode = node;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childTrees"></param>
        public Tree(T node, List<Tree<T, T2>> childTrees)
        {
            TreeNode = node;
            ChildTrees = childTrees;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childNodes"></param>
        public Tree(T node, List<T> childNodes)
        {
            TreeNode = node;
            ChildTrees = childNodes.Select(x => new Tree<T, T2>(x)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="secondNodes"></param>
        public Tree(T node, List<T2> secondNodes)
        {
            TreeNode = node;
            SecondNodes = secondNodes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childNodes"></param>
        /// <param name="secondNodes"></param>
        public Tree(T node, List<T> childNodes, List<T2> secondNodes)
        {
            TreeNode = node;
            SecondNodes = secondNodes;
            ChildTrees = childNodes.Select(x => new Tree<T, T2>(x)).ToList();
        }

    }

    /// <summary>
    /// 简单树集合（1层）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OneTree<T> where T : new()
    {
        /// <summary>
        /// 对象
        /// </summary>
        public T TreeNode { get; set; }

        /// <summary>
        /// 子对象集合
        /// </summary>
        public List<T> ChildNodes { get; set; } = new List<T>();

        public OneTree(T node)
        {
            TreeNode = node;
        }
        public OneTree(T node, List<T> childNodes)
        {
            TreeNode = node;
            ChildNodes = childNodes;
        }
    }

    /// <summary>
    /// 简单树集合（1层）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class OneTree<T, T2> where T : new() where T2 : new()
    {
        /// <summary>
        /// 对象
        /// </summary>
        public T TreeNode { get; set; }

        /// <summary>
        /// 子对象集合
        /// </summary>
        public List<T> ChildNodes { get; set; } = new List<T>();
        public List<T2> SecondNodes { get; set; } = new List<T2>();

        public OneTree(T node)
        {
            TreeNode = node;
        }
        public OneTree(T node, List<T> childNodes, List<T2> sencondNodes)
        {
            TreeNode = node;
            ChildNodes = childNodes;
            SecondNodes = sencondNodes;
        }
    }
}
