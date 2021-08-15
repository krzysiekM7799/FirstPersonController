using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class BTNode
{
	protected NodeStates m_nodeState;

	public NodeStates nodeState
	{
		get { return m_nodeState; }
	}

	public BTNode() { }

	public abstract NodeStates Evaluate();
}