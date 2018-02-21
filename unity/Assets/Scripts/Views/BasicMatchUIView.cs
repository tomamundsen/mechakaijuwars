
using System;
using UnityEngine;

public class BasicMatchUIView : MonoBehaviour
{
	public UnityEngine.UI.Text TurnBanner;
	public UnityEngine.UI.Image PlayerAvatar;
	public UnityEngine.UI.Text PlayerNameText;
	public UnityEngine.UI.Text TurnText;
	public UnityEngine.UI.Text TimeText;
	public UnityEngine.UI.Text ManaText;
	public Color ManaBlockEmptyColor;
	public Color ManaBlockFullColor;
	public UnityEngine.UI.Image ManaBlock1;
	public UnityEngine.UI.Image ManaBlock2;
	public UnityEngine.UI.Image ManaBlock3;
	public UnityEngine.UI.Image ManaBlock4;
	public UnityEngine.UI.Image ManaBlock5;
	public UnityEngine.UI.Image ManaBlock6;
	public UnityEngine.UI.Image ManaBlock7;
	public UnityEngine.UI.Image ManaBlock8;
	public UnityEngine.UI.Image ManaBlock9;
	public UnityEngine.UI.Image ManaBlock10;

	void ChangeAvatarImage(string url)
	{
		Sprite s = Resources.Load <Sprite> ("kaiju");
		PlayerAvatar.sprite = s;
	}

	public void SetSprite(Sprite s)
	{
		PlayerAvatar.sprite = s;
	}

	public void SetPlayerNameText(string s)
	{
		PlayerNameText.text = s;
	}

	public void SetTimeText(string s)
	{
		TimeText.text = s;
	}

	public void SetTurnText(string s)
	{
		TurnText.text = s;
	}

	public void SetManaText(string s)
	{
		ManaText.text = s;
	}

	public void SetManaMeter(int m)
	{
//		m %= 10;

		switch (m) {
		case 0:
			ManaBlock1.color = ManaBlockEmptyColor;
			ManaBlock2.color = ManaBlockEmptyColor;
			ManaBlock3.color = ManaBlockEmptyColor;
			ManaBlock4.color = ManaBlockEmptyColor;
			ManaBlock5.color = ManaBlockEmptyColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 1:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockEmptyColor;
			ManaBlock3.color = ManaBlockEmptyColor;
			ManaBlock4.color = ManaBlockEmptyColor;
			ManaBlock5.color = ManaBlockEmptyColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 2:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockEmptyColor;
			ManaBlock4.color = ManaBlockEmptyColor;
			ManaBlock5.color = ManaBlockEmptyColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 3:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockEmptyColor;
			ManaBlock5.color = ManaBlockEmptyColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 4:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockEmptyColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 5:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockEmptyColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 6:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockFullColor;
			ManaBlock7.color = ManaBlockEmptyColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 7:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockFullColor;
			ManaBlock7.color = ManaBlockFullColor;
			ManaBlock8.color = ManaBlockEmptyColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 8:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockFullColor;
			ManaBlock7.color = ManaBlockFullColor;
			ManaBlock8.color = ManaBlockFullColor;
			ManaBlock9.color = ManaBlockEmptyColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 9:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockFullColor;
			ManaBlock7.color = ManaBlockFullColor;
			ManaBlock8.color = ManaBlockFullColor;
			ManaBlock9.color = ManaBlockFullColor;
			ManaBlock10.color = ManaBlockEmptyColor;
			break;
		case 10:
			ManaBlock1.color = ManaBlockFullColor;
			ManaBlock2.color = ManaBlockFullColor;
			ManaBlock3.color = ManaBlockFullColor;
			ManaBlock4.color = ManaBlockFullColor;
			ManaBlock5.color = ManaBlockFullColor;
			ManaBlock6.color = ManaBlockFullColor;
			ManaBlock7.color = ManaBlockFullColor;
			ManaBlock8.color = ManaBlockFullColor;
			ManaBlock9.color = ManaBlockFullColor;
			ManaBlock10.color = ManaBlockFullColor;
			break;
		};
	}

	public void UpdateMana(int mana)
	{
		SetManaText(mana.ToString() + "/10");
		SetManaMeter(mana);
	}
}
