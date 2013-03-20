using System;
using System.Collections.Generic;

// # class Node

// Holds a node in a BSP tree. A BSP tree is built from a collection of polygons
// by picking a polygon to split along. That polygon (and all other coplanar
// polygons) are added directly to that node and the other polygons are added to
// the front and/or back subtrees. This is not a leafy BSP tree since there is
// no distinction between internal and leaf nodes.
public class CSGNode
{
	public CSGPlane plane;
	public CSGNode front;
	public CSGNode back;
	public List<CSGPolygon> polygons = new List<CSGPolygon>();

	private bool validPlane;
	
	public CSGNode()
	{
	}
	
	public CSGNode(List<CSGPolygon> polygons)
	{
		addPolygons(polygons);
	}
	
	public CSGNode clone()
	{
		CSGNode node = new CSGNode();
		
		node.plane = plane;
		node.validPlane = validPlane;
		
		if (front != null)
			node.front = front.clone();
		
		if (back != null)
			node.back = back.clone();

		for (int i = 0; i < polygons.Count; i++)
			node.polygons.Add(polygons[i].clone());

		return node;
	}
	
	// Convert solid space to empty space and empty space to solid space.
	public void invert()
	{
		for (int i = 0; i < polygons.Count; i++)
			polygons[i].flip();
		
		plane.flip();
		
		if (front != null) 
			front.invert();
		
		if (back != null) 
			back.invert();
		
		CSGNode temp = front;
		front = back;
		back = temp;
	}
	
	// Recursively remove all polygons in `polygons` that are inside this BSP
	// tree.
	public List<CSGPolygon> clipPolygons(List<CSGPolygon> polygons) 
	{
		if (!validPlane) 
			return new List<CSGPolygon>(polygons);
		
		List<CSGPolygon> front = new List<CSGPolygon>();
		List<CSGPolygon> back = new List<CSGPolygon>();
		
		for (int i = 0; i < polygons.Count; i++)
			plane.splitPolygon(polygons[i], front, back, front, back);
		
		if (this.front != null) 
			front = this.front.clipPolygons(front);
		
		if (this.back != null) 
			back = this.back.clipPolygons(back);
		else
			back = new List<CSGPolygon>();
		
		front.AddRange(back);
		
		return front;
	}
	
	// Remove all polygons in this BSP tree that are inside the other BSP tree
	// `bsp`.
	public void clipTo(CSGNode bsp) 
	{
		this.polygons = bsp.clipPolygons(this.polygons);
		
		if (this.front != null) 
			this.front.clipTo(bsp);
		
		if (this.back != null) 
			this.back.clipTo(bsp);
	}
	
	// Return a list of all polygons in this BSP tree.
	public List<CSGPolygon> allPolygons() 
	{
		List<CSGPolygon> target = new List<CSGPolygon>();

		allPolygons(target);

		return target;
	}
	
	private void allPolygons(List<CSGPolygon> target) 
	{
		target.AddRange(this.polygons);
		
		if (this.front != null) 
			this.front.allPolygons(target);

		if (this.back != null) 
			this.back.allPolygons(target);
	}

	// Build a BSP tree out of `polygons`. When called on an existing tree, the
	// new polygons are filtered down to the bottom of the tree and become new
	// nodes there. Each set of polygons is partitioned using the first polygon
	// (no heuristic is used to pick a good split).
	public void addPolygons(List<CSGPolygon> polygons) 
	{
		if (polygons.Count == 0) 
			return;
		
		if (!validPlane)
		{
			this.plane = polygons[0].plane;
			validPlane = true;
		}
		
		List<CSGPolygon> front = new List<CSGPolygon>();
		List<CSGPolygon> back = new List<CSGPolygon>();
		
		for (int i = 0; i < polygons.Count; i++) {
			this.plane.splitPolygon(polygons[i], this.polygons, this.polygons, front, back);
		}
		
		if (front.Count > 0) 
		{
			if (this.front == null) 
				this.front = new CSGNode();
			
			this.front.addPolygons(front);
		}
		
		if (back.Count > 0) 
		{
			if (this.back == null) 
				this.back = new CSGNode();
			
			this.back.addPolygons(back);
		}
	}
}


