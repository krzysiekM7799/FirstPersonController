using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : BTNode
{
	protected List<BTNode> m_nodes = new List<BTNode>();

	public Selector()
    {

    }

	public Selector(List<BTNode> nodes, bool replace = true)
	{
		if (replace)
		{
			m_nodes = nodes;
		}
		else
		{
			foreach (BTNode node in nodes)
			{
				m_nodes.Add(node);
			}
		}

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
		foreach (BTNode node in m_nodes)
		{
			switch (node.Evaluate())
			{
				case NodeStates.FAILURE:
					continue;
				case NodeStates.SUCCESS:
					m_nodeState = NodeStates.SUCCESS;
					return m_nodeState;
				case NodeStates.RUNNING:
					m_nodeState = NodeStates.RUNNING;
					return m_nodeState;
				default:
					continue;
			}
		}

		m_nodeState = NodeStates.FAILURE;
		return m_nodeState;
	}
}