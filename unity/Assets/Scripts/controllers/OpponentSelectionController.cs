using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpponentSelectionController : MonoBehaviour
{
	public Text errorLabel;

	public void SelectRandomOnline()
	{
		GameManager.Instance.CurrentOpponentType = "Random Online";
		Application.LoadLevel("FormatSelection");
	}

	public void GoBack()
	{
		Application.LoadLevel("DeckSelection");
	}
}
