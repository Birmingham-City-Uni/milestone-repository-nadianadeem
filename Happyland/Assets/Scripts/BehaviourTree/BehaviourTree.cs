using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class BehaviourTree : ScriptableObject
{
    public BTNode rootNode;
    public BTNode.BTState treeState = BTNode.BTState.Running;
    public List<BTNode> nodes = new List<BTNode>();

    public BTNode.BTState Update()
    {
        if(rootNode.state == BTNode.BTState.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }

    public BTNode CreateNode(System.Type type)
    {
        BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(BTNode node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BTNode parent, BTNode child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = child;
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Add(child);
        }
    }

    public void RemoveChild(BTNode parent, BTNode child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            root.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);
        }
    }

    public List<BTNode> GetChildren(BTNode parent)
    {
        List<BTNode> children = new List<BTNode>();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }

        return children;
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        return tree;
    }
}
