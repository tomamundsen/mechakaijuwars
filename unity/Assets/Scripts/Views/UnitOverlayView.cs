using UnityEngine;
using System.Collections;

public class UnitOverlayView : MonoBehaviour {

	public delegate IEnumerator AsyncDelegate();
	public delegate void SyncDelegate();
	public SyncDelegate MagicDel;
	public SyncDelegate AttackDel;
	public SyncDelegate MoveDel;

	public void Attack()
	{
		AttackDel();
	}

	public void Magic()
	{
		MagicDel();
	}

	public void Move()
	{
		MoveDel();
	}
}
