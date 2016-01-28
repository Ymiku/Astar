using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	private Node[,] grid;
	public Vector2 gridSize;
	private float nodeRadius = 0.2f;
	private float nodeDiameter;
	public LayerMask WhatLayer;
	//public Transform player;
	public int gridCntX, gridCntY;


	public void Initialize() {
		nodeDiameter = nodeRadius * 2;
		gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
		gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
		grid = new Node[gridCntX, gridCntY];
		CreatGrid();
	}
	public void Leave()
	{
		grid = new Node[0,0];
	}

	private void CreatGrid()
	{
		Vector3 startPoint = transform.position - gridSize.x/2*Vector3.right-Vector3.up*gridSize.y/2;
		for(int i = 0;i<gridCntX;i++)
		{
			for(int j = 0;j<gridCntY;j++)
			{
				Vector3 worldPoint = startPoint + Vector3.right*(i*nodeDiameter + nodeRadius)+
					Vector3.up*(j*nodeDiameter + nodeRadius);
				//bool walkable = !Physics.CheckSphere(worldPoint,nodeRadius,WhatLayer);
				bool walkable = !Physics2D.CircleCast(worldPoint,nodeRadius*1.2f,Vector3.zero,WhatLayer);
			
				grid[i,j] = new Node(walkable,worldPoint,i,j);
			}
		}
	}
	void Update () {
	}
	
	
	public Node GetFromPosition(Vector3 position)
	{
		float percentX = (position.x - this.transform.position.x + gridSize.x/2)/gridSize.x;
		float percentY = (position.y - this.transform.position.y + gridSize.y/2)/gridSize.y;
		
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x = Mathf.RoundToInt((gridCntX-1)*percentX);
		int y = Mathf.RoundToInt((gridCntY-1)*percentY);
		return grid[x,y];
	}
	
	
	//在编辑器中将将节点以立方体的形式展示出来以方便测试真机运行时此函数不执行
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridSize.x,gridSize.y,1));
		if(grid==null)return;
		foreach(var node in grid)
		{
			Gizmos.color = node._canWalk?Color.white:Color.red;
			Gizmos.DrawCube (node._worldPos,Vector3.one*(nodeDiameter/2));
		}


	}
	public List<Node> GetNeibourhood(Node node)
		//获取周围邻居节点
	{
		List<Node> neibourhood=new List<Node>();
		for(int i =-1;i<=1;i++)
		{
			for(int j=-1;j<=1;j++)
			{
				if(i==0&&j==0)
				{
					continue;
				}
				int tempX = node._gridX+i;
				int tempY = node._gridY+j;
				if(tempX < gridCntX&&tempX > 0&&tempY > 0&&tempY<gridCntY)
				{
					neibourhood.Add(grid[tempX,tempY]);
				}
			}
		}
		return neibourhood;
	}
}
