
using System;
using System.Collections;
using UnityEngine;
	
public class TileView : MonoBehaviour
{
	private Color initialColor;

	public Sprite TransparentSprite;
	public Sprite OpaqueSprite;
	public Point Location { get; set; }
	public delegate IEnumerator AsyncMouseDelegate(TileView tv);
	public delegate void SyncMouseDelegate(TileView tv);
	public SyncMouseDelegate MouseEnter;
	public SyncMouseDelegate MouseExit;
	public AsyncMouseDelegate MouseOver;

	void Start()
	{
		((SpriteRenderer)GetComponent<Renderer>()).sprite = TransparentSprite;
		Color c = initialColor;
		c.a = 1.0f;
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

	public float GetWidth()
	{
		return ((SpriteRenderer)GetComponent<Renderer>()).sprite.bounds.extents.x;
	}
	public float GetHeight()
	{
		return ((SpriteRenderer)GetComponent<Renderer>()).sprite.bounds.extents.y;
	}

	public void Clear()
	{
		((SpriteRenderer) GetComponent<Renderer>()).sprite = TransparentSprite;
		Color c = new Color(1.0f, 1.0f, 1.0f);
//		Color c = initialColor;
		c.a = 130f / 255f;
//		c.a = 1.0f;
		((SpriteRenderer) GetComponent<Renderer>()).color = c;
	}

	public Vector3 GetPosition()
	{
		return gameObject.transform.position;
	}
	
	public void ChangeColor(Color color)
	{
		//If transparency is not set already, set it to default value
		if (color.a == 1)
		{
			color.a = 130f / 255f;
		}
		
		((SpriteRenderer)GetComponent<Renderer>()).sprite = OpaqueSprite;
		((SpriteRenderer)GetComponent<Renderer>()).color = color;
	}
}
