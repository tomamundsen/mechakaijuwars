
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmView : MonoBehaviour
{
	public Text Message;

	public delegate void SyncMouseDelegate(ConfirmView v);
	public SyncMouseDelegate AcceptDel;
	public SyncMouseDelegate DeclineDel;

	public void SetMessage(string s)
	{
		Message.text = s;
	}

	public void Accept()
	{
		AcceptDel(this);
	}

	public void Decline()
	{
		DeclineDel(this);
	}
}
