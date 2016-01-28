using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath : UnitySingleton<FindPath> {
	public Grid _grid;
	private void SetGrid(Grid grid) {
		_grid = grid;
		_grid.Initialize();
	}
	public Vector2 FindingPath(Vector3 StartPos,Vector3 EndPos)
	{
		Node startNode = _grid.GetFromPosition(StartPos);
		Node EndNode = _grid.GetFromPosition(EndPos);
		if (startNode._canWalk == false || EndNode._canWalk == false) {
			Debug.Log("cantwalk");
			return Vector2.zero;
		}
		if (startNode == EndNode)
			return Vector2.zero;
		List<Node> openSet=new List<Node>();
		HashSet<Node>closeSet=new HashSet<Node>();
		openSet.Add(startNode);
		openSet.Add(startNode);
		while(openSet.Count>1)
		{
			Node currentNode = openSet[1];
			int DeletePos = 1;
			int LeftChild = DeletePos*2;
			int RightChild = DeletePos*2+1;
			Node DelTemp;
			DelTemp = openSet[DeletePos];
			openSet[DeletePos] = openSet[openSet.Count-1];
			openSet[openSet.Count-1] = DelTemp;
			openSet.Remove(DelTemp);
			while(RightChild <= openSet.Count-1)
			{
				if((openSet[DeletePos].fCost<openSet[LeftChild].fCost||
				    openSet[DeletePos].fCost==openSet[LeftChild].fCost&&openSet[DeletePos].hCost<openSet[LeftChild].hCost)
				   &&(openSet[DeletePos].fCost<openSet[RightChild].fCost||
				   openSet[DeletePos].fCost==openSet[RightChild].fCost&&openSet[DeletePos].hCost<openSet[RightChild].hCost))
					
				{
					break;
				}
				else
				{
					if(openSet[LeftChild].fCost<openSet[RightChild].fCost||
					   openSet[LeftChild].fCost==openSet[RightChild].fCost&&openSet[LeftChild].hCost<openSet[RightChild].hCost)
					{
						DelTemp = openSet[DeletePos];
						openSet[DeletePos] = openSet[LeftChild];
						openSet[LeftChild] = DelTemp;
						DeletePos = LeftChild;
						LeftChild = DeletePos*2;
						RightChild = DeletePos*2+1;
					}
					else
					{
						DelTemp = openSet[DeletePos];
						openSet[DeletePos] = openSet[RightChild];
						openSet[RightChild] = DelTemp;
						DeletePos = RightChild;
						LeftChild = DeletePos*2;
						RightChild = DeletePos*2+1;
					}
				}
			}
			if(LeftChild == openSet.Count-1)
			{
				if(!(openSet[DeletePos].fCost<openSet[LeftChild].fCost||
				     openSet[DeletePos].fCost==openSet[LeftChild].fCost&&openSet[DeletePos].hCost<openSet[LeftChild].hCost))
				{
					DelTemp = openSet[DeletePos];
					openSet[DeletePos] = openSet[LeftChild];
					openSet[LeftChild] = DelTemp;
				}
			}
			//
			closeSet.Add(currentNode);
			if(currentNode == EndNode)
			{
				Vector3 target = GeneratePath(startNode,EndNode);
				return new Vector2(target.x-StartPos.x,target.y-StartPos.y);
			}
			foreach(var node in _grid.GetNeibourhood(currentNode) )
			{
				
				if(!node._canWalk||closeSet.Contains(node))continue;
				int newCost = currentNode.gCost + GetDistanceNodes(currentNode,node);
				if(newCost < node.gCost||!openSet.Contains(node))
				{
					node.gCost = newCost;
					node.hCost = GetDistanceNodes (node,EndNode);
					node.parent = currentNode;
					if(newCost < node.gCost)
					{
						int AddPos = 0;
						for(int m = 1;m <= openSet.Count-1;m++)
						{
							if((openSet[m].gCost == node.gCost)&&(openSet[m].hCost == node.hCost))
							{
								AddPos = m;
								break;
							}
						}
						
						int FatherPos = AddPos/2;
						Node temp;
						while(FatherPos != 0)
						{
							
							if(openSet[AddPos].fCost<openSet[FatherPos].fCost||
							   openSet[AddPos].fCost==openSet[FatherPos].fCost&&openSet[AddPos].hCost<openSet[FatherPos].hCost)
							{
								temp = openSet[FatherPos];
								openSet[FatherPos] = openSet[AddPos];
								openSet[AddPos] = temp;
								AddPos = FatherPos;
								FatherPos /= 2;
								
							}
							else
							{
								break;
							}
						}
						
					}
					
					if(!openSet.Contains(node))
					{
						int AddPos = openSet.Count;
						int FatherPos = openSet.Count/2;
						Node temp;
						openSet.Add(node);
						while(FatherPos != 0)
						{
							
							if(openSet[AddPos].fCost<openSet[FatherPos].fCost||
							   openSet[AddPos].fCost==openSet[FatherPos].fCost&&openSet[AddPos].hCost<openSet[FatherPos].hCost)
							{
								temp = openSet[FatherPos];
								openSet[FatherPos] = openSet[AddPos];
								openSet[AddPos] = temp;
								AddPos = FatherPos;
								FatherPos /= 2;
								
							}
							else
							{
								break;
							}
						}
						//
					}
				}
			}
		}
		return Vector2.zero;
	}
	
	private Vector3 GeneratePath(Node startNode,Node endNode)
	{
		Node temp = endNode;
		while(temp.parent!=startNode)
		{
			temp = temp.parent;
		}
		return temp._worldPos;
	}

	
	int GetDistanceNodes(Node a,Node b)
	{
		int cntX = Mathf.Abs (a._gridX - b._gridX);
		int cntY = Mathf.Abs (a._gridY - b._gridY);
		if(cntX > cntY)
		{
			return 14*cntY + 10*(cntX-cntY);
		}
		else
		{
			return 14*cntX + 10*(cntY-cntX);
		}
	}


}
