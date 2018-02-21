
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
	public bool IsMoving { get; set; }
	public List<TileView> Path;
	// speed in meters per second
	private readonly float speed = 0.0025F;
	private readonly float rotationSpeed = 0.004F;
	//distance between character and tile position when we assume we reached it and start looking for the next. Explained in detail later on
	private readonly float MinNextTileDist = 0.002f;

	public delegate IEnumerator AsyncMouseDelegate(UnitView tv);
	public delegate void SyncMouseDelegate(UnitView tv);
	public SyncMouseDelegate MouseEnter;
	public SyncMouseDelegate MouseExit;
	public AsyncMouseDelegate MouseOver;

	void Awake()
	{
		IsMoving = false;
	}

	public void SetPathViews(List<TileView> pathViews)
	{
		Path = pathViews;
	}

	public IEnumerator Walk() // TODO: play appropriate animations
	{
		if (Path.Count < 2)
		{
			Debug.Log("WARN!!!! UnitView:StartMmoving Path null");
            yield return new WaitForSeconds(0.0f);
		}
		
		int i = Path.Count-1;
		IsMoving = true;
		Vector3 tilePos;
		
		while (i > 0)
		{
			TileView curTV = Path[i];
			TileView nextTV = Path[i-1];
			int numSteps = 10;
			Vector3 delta = nextTV.GetPosition() - curTV.GetPosition();
			int count = 0;
			float xDiff = 0;
			float yDiff = 0;
			
			if (Mathf.Abs(delta.x) > 0)
			{
				xDiff = delta.x/numSteps;
			}
			else if (Mathf.Abs(delta.y) > 0)
			{
				yDiff = delta.y/numSteps;
			}
			else
			{
				Debug.Log("WARN!!! UnitView:StartMoving impossible state");
			}
			
			while (count++ < numSteps)
			{
				SetPosition(new Vector3(gameObject.transform.position.x + xDiff,
				                           gameObject.transform.position.y + yDiff,
				                           0));
				yield return new WaitForSeconds(0.001f);
				
			}
			count = 0;
			i--;
		}
		
		IsMoving = false;
	}

	public void SetPosition(Vector3 v)
	{
		Vector3 vector = new Vector3(v.x, v.y, -10);
		gameObject.transform.position = vector;
	}

	public float GetWidth()
	{
		return ((SpriteRenderer)GetComponent<Renderer>()).bounds.size.x;
	}

	public float GetHeight()
	{
		return ((SpriteRenderer)GetComponent<Renderer>()).bounds.size.y;
	}

//	public Vector3 GetPosition()
//	{
//		Vector3 result = ((SpriteRenderer)renderer).transform.position;
//		result.z = -3;
//		return result;
//	}

	public void SetColor(Color c)
	{
		((SpriteRenderer)GetComponent<Renderer>()).color = c;
	}

	void OnMouseEnter()
	{
		if (MouseEnter == null)
		{
			Debug.Log("TileView:OnMouseEnterCallback null");
		}
		else
		{
			MouseEnter(this);
		}
	}
	
	void OnMouseExit()
	{
		if (MouseExit == null)
		{
			Debug.Log("TileView:OnMouseExitCallback null");
		}
		else
		{
			MouseExit(this);
		}
	}
	
	void OnMouseOver()
	{
		if (MouseOver == null)
		{
			Debug.Log("TileView:OnMouseOverCallback null");
		}
		else
		{
			StartCoroutine(MouseOver(this));
		}
	}
}
