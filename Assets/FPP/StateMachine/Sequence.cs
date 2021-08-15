using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequence : BTNode
{
    private List<BTNode> m_nodes = new List<BTNode>();

    public Sequence(List<BTNode> nodes, bool replace = true)
    {
        if (replace)
        {
            m_nodes = nodes;
        }
        else
        {
            foreach(BTNode node in nodes)
            {
                m_nodes.Add(node);
            }
        }
    }

    public Sequence()
    {
       
    }

    public Sequence(BTNode node)
    {
        m_nodes.Add(node);
    }

    public void SetChildrenOfNode(List<BTNode> nodes)
    {
        foreach (var item in nodes)
        {
            m_nodes.Add(item);
        }      
    }

    public void SetChildrenOfNode(BTNode node)
    {      
            m_nodes.Add(node);
    }


    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        foreach (BTNode node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_nodeState = NodeStates.FAILURE;
                    return m_nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    m_nodeState = NodeStates.SUCCESS;
                    return m_nodeState;
            }
        }

        m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return m_nodeState;
    }
}