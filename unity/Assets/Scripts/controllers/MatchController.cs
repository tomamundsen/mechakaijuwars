
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MatchController: MonoBehaviour
{
	private static readonly int MATCH_REFRESH_RATE = 5;
	private static readonly Color ORANGE = new Color(255f / 255f, 127f / 255f, 0, 127f/255f);

	private bool isPlayerTurn;
	private bool unitOverlayVisible;
	private bool largeCardViewVisible;
	private float mouseDownTime;
	private float longClickLength = 0.5f;
	private int level = 1;
	private Dictionary<Unit, UnitView> unitViews = new Dictionary<Unit, UnitView>();
	private Dictionary<UnitView, Unit> units;
	private List<CardView> cards;
	private Unit player1;
	private Unit player2;
	private Unit currentPlayer;
	private Map map;
	private tk2dTileMap tilemap;
	private MapView mapView;
	private UnityEngine.UI.Image resultsImage;
	private UnityEngine.UI.Text resultsText;
	private BasicMatchUIView basicMatchUIView;
	private LargeCardView largeCardView;
	private UnitOverlayView unitOverlayView;
	private UnitView selectedUnit;
	private GameObject largeCardViewObj;
	private GameObject unitOverlayViewObj;
	private GameObject basicMatchUIViewObj;
	private GameObject mediumCardObject;
	private CardView mediumCardView;
	private Dictionary<CardView, CardLevelSheet> cardLevelSheets;
	private bool isDisplayingConfirm;
	private TileView selectedTileView;
    private List<PlayerAction> playerActions = new List<PlayerAction>();

    public int Player1StartX;
	public int Player1StartY;
	public int Player2StartX;
	public int Player2StartY;
	public Canvas Canvas;
	public GameObject Tilemap;
	public GameObject Player1Prefab;
	public GameObject Player2Prefab;
	public GameObject UnitPrefab;
	public GameObject CardPrefab;
	public GameObject ResultsImage;
	public GameObject ResultsText;
	public GameObject UnitOverlayViewPrefab;
	public GameObject LargeCardViewPrefab;
	public GameObject BasicMatchUIViewPrefab;
	public GameObject ConfirmPrefab;

//	public static Texture2D LoadPNG(string filePath) {
//		
//		Texture2D tex = null;
//		byte[] fileData;
//		
//		if (File.Exists(filePath))     {
//			fileData = File.ReadAllBytes(filePath);
//			tex = new Texture2D(2, 2);
//			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
//		}
//		return tex;
//	}

//	Vector3 GetScreenPosition(float x, float y)
//	{
//		return new Vector3(Player1StartX, Player1StartY);
//	}

	void UnitViewEnter(UnitView v)
	{
		TileOnMouseEnter(mapView.GetTileViewAt(units[v].Location));
	}

	void UnitViewExit(UnitView v)
	{
		TileOnMouseExit(mapView.GetTileViewAt(units[v].Location));
	}

	void SetLUVText(LargeCardView v, int level)
	{
		Card c = GameManager.Instance.Cards[v.Id];
		CardLevelSheet cs = c.GetLevelSheet(level);
		v.SetLevel(cs.Level);
		v.SetLevel(cs.Level);
		v.SetText(cs.Text);
	}

	void LUVPrevious(LargeCardView v)
	{
		if (level > 1)
		{
			level--;
		}
		
		SetLUVText(v, level);
	}
	
	void LUVNext(LargeCardView v)
	{
		if (level < 3)
		{
			level++;
		}
		SetLUVText(v, level);
	}

	public void LUVBack(LargeCardView v)
	{
		Debug.Log("LargeUnitController:Back");
		GameObject.Destroy(largeCardViewObj);
		largeCardView = null;
		largeCardViewVisible = false;
	}
	
	void UnitOverlayViewAttack()
	{
		Debug.Log("MatchController:UnitOverlayPanelViewAttack " + units[selectedUnit].Name);
		// TOOD: perform attack sequence
		// need to start a new UI state where you select a unit to attack
	}
	
	void UnitOverlayViewMagic()
	{
		Debug.Log("MatchController:UnitOverlayPanelViewMagic " + units[selectedUnit].Name);
		// TODO: perform magic sequence
		// need to start a new UI state where you select magic to perform, which may have targets
	}
	
	void UnitOverlayViewMove()
	{
		Debug.Log("MatchController;UnitOverlayPanelViewMove " + units[selectedUnit].Name);
	}

	IEnumerator UnitViewOver(UnitView v)
	{
		Unit u = units[v];
		TileOnMouseOver(mapView.GetTileViewAt(units[v].Location));
//		Debug.Log("MatchController:UnitViewOver " + v.ToString());
//		v.SetColor(Color.yellow);
		if (Input.GetMouseButtonDown(0))
		{
			mouseDownTime = Time.time;
		}
		if (Input.GetMouseButtonUp(0))
		{
			//Tile t = map.GetTileAt(u.Location);
			//TileView tv = mapView.GetTileViewAt(u.Location);
			//if (map.OriginTile == null)
			//{
			//	if (t.Location.Equals(currentPlayer.Location) && !IsUnitMoving())
			//	{
			//		map.OriginTile = t;
			//		mapView.OriginTileView = tv;
			//	}
			//	else
			//	{
			//		Debug.Log ("you clicked somewhere that doesn't work");
			//	}
			//}
			//else if (!map.DestinationSelected)
			//{
			//	map.SelectedTile = t;
			//	map.DestinationSelected = true;
				
			//	mapView.SelectedTileView = tv;
			//	mapView.Clear();
				
			//	Path<Tile> path = map.FindPath();
			//	List<TileView> pathViews = new List<TileView>();
			//	foreach (Tile tile in path)
			//	{
			//		pathViews.Add (mapView.GetTileViewAt(tile.Location));
			//	}
				
			//	mapView.SetPathViews(pathViews);
			//	unitViews[currentPlayer].SetPathViews(pathViews);
			//	mapView.DrawSelectedPath();
			//}
			//else if (map.DestinationSelected)
			//{
			//	if (t == map.SelectedTile)
			//	{
			//		List<Tile> thePath = new List<Tile>();
			//		Path<Tile> myPath = map.FindPath();
			//		List<TileView> myPathViews  = new List<TileView>();
					
			//		foreach (Tile tile in myPath)
			//		{
			//			thePath.Add(tile);
			//			myPathViews.Add (mapView.GetTileViewAt(tile.Location));
			//		}
					
			//		currentPlayer.Location = thePath[0].Location;
			//		mapView.DrawSelectedPath();
					
			//		yield return StartCoroutine(unitViews[currentPlayer].Walk());
					
			//		yield return StartCoroutine(TakeTurn());
					
			//		map.Reset();
			//		map.OriginTile = null;
			//		map.SelectedTile = null;
			//		map.DestinationSelected = false;
			//		mapView.Clear();
			//	}
			//	else
			//	{
			//		Debug.Log("didn't confirm selection");
			//	}
			//}

			selectedUnit = v;
			if (!unitOverlayVisible && !largeCardViewVisible)
			{
				if ((Time.time - mouseDownTime) > longClickLength)
				{
					largeCardViewObj = GameObject.Instantiate(LargeCardViewPrefab);
					largeCardViewObj.transform.SetParent(Canvas.transform, false);
					largeCardView = (LargeCardView) largeCardViewObj.GetComponent("LargeCardView");
					// todo: get CardLevelSheet for selected unit
					largeCardView.Id = "10";// todo: get id of selected unit 
					largeCardView.SetName(u.Name);
					largeCardView.SetType(u.Type);
					largeCardView.SetSubtype(u.Subtype);
					largeCardView.SetText(u.LevelOneText);
					largeCardView.PreviousDel = LUVPrevious;
					largeCardView.NextDel = LUVNext;
					largeCardView.BackDel = LUVBack;
					largeCardView.SetRarity("Common");
//					largeCardView.Image.sprite = Resources.Load(u.a
					largeCardViewVisible = true;
				}
			}
		}
		if (Input.GetMouseButtonUp(1))
		{
			Debug.Log("right click");
			if (!unitOverlayVisible)
			{
				unitOverlayViewObj = GameObject.Instantiate(UnitOverlayViewPrefab);
				unitOverlayViewObj.transform.SetParent(Canvas.transform, false);
				unitOverlayView = (UnitOverlayView) unitOverlayViewObj.GetComponent("UnitOverlayView");
				unitOverlayView.MagicDel = UnitOverlayViewMagic;
				unitOverlayView.AttackDel = UnitOverlayViewAttack;
				unitOverlayView.MoveDel = UnitOverlayViewMove;

				unitOverlayVisible = true;
			}
			else
			{
				unitOverlayVisible = false;
				
				if (unitOverlayViewObj != null)
				{
					GameObject.Destroy(unitOverlayViewObj);
					unitOverlayView = null;
					unitOverlayVisible = false;
				}
			}
		}

		yield return new WaitForSeconds(0.0f);
	}

	void Awake()
	{
		cardLevelSheets = new Dictionary<CardView, CardLevelSheet>();
		basicMatchUIViewObj = (GameObject)Instantiate(BasicMatchUIViewPrefab);
		basicMatchUIView = (BasicMatchUIView)(basicMatchUIViewObj.GetComponent("BasicMatchUIView"));

		basicMatchUIView.transform.SetParent(Canvas.transform, false);
		map = new Map();
		units = new Dictionary<UnitView, Unit>();
		resultsText = (UnityEngine.UI.Text)ResultsText.GetComponent("Text");
		resultsImage = (UnityEngine.UI.Image)ResultsImage.GetComponent("Image");
		tilemap = (tk2dTileMap)Tilemap.GetComponent("tk2dTileMap");
		mapView = (MapView)Tilemap.GetComponent("MapView");
		mapView.TileViewEnter = TileOnMouseEnter;
		mapView.TileViewExit = TileOnMouseExit;
		mapView.TileViewOver = TileOnMouseOver;
		mapView.Initialize();
		GameManager.Instance.CurrentMatchOpponentPlayerId = "ERROR_NO_PLAYER";

//		player1 = new Unit("Kaiju1",
//		                   "Set1.1",
//		                   "Kaiju",
//		                   "Lizard",
//		                   "This is the level one lizard Kaiju.",
//		                   "This is the level two lizard Kaiju.",
//		                   "This is the level three lizard Kaiju.",
//		                   new Point(Player1StartX, Player1StartY));
//		Vector3 player1Location = mapView.GetTileViewAt(player1.Location).GetPosition();
//		UnitView player1UnitView = (UnitView)((GameObject)Instantiate (Player1Prefab)).GetComponent("UnitView");
//		player1UnitView.MouseEnter = UnitViewEnter;
//		player1UnitView.MouseExit = UnitViewExit;
//		player1UnitView.MouseOver = UnitViewOver;
//		player1UnitView.SetPosition(player1Location);
//		player1UnitView.SetColor(Color.white);
//		unitViews[player1] = player1UnitView;
//		units[player1UnitView] = player1;
//		currentPlayer = player1;
		// code for UnityEngine.UI
		//		Vector3 player1LocationNew = GetScreenPosition(Player1StartX, Player1StartY); // TODO: make Vector2D
		//		player1UnitView.SetPosition(player1LocationNew);
		//		player1UnitView.transform.SetParent(Canvas.transform, false);

//		player2 = new Unit("Mecha1",
//		                   "Set1.2",
//		                   "Mecha",
//		                   "Gundam",
//		                   "This is the level one Gundam Mecha.",
//		                   "This is the level two Gundam Mecha.",
//		                   "This is the level three Gundam Mecha.",
//		                   new Point(Player2StartX, Player2StartY));
//		Vector3 player2Location = mapView.GetTileViewAt(player2.Location).GetPosition();
//		UnitView player2UnitView = (UnitView)((GameObject)Instantiate (Player2Prefab)).GetComponent("UnitView");
//		player2UnitView.MouseEnter = UnitViewEnter;
//		player2UnitView.MouseExit = UnitViewExit;
//		player2UnitView.MouseOver = UnitViewOver;
//		player2UnitView.SetPosition(player2Location);
//		player2UnitView.SetColor(Color.white);
//		unitViews[player2] = player2UnitView;
//		units[player2UnitView] = player2;
		// code for UnityEngine.UI
		//		player2UnitView.transform.SetParent(Canvas.transform, false);

		if (GameManager.Instance.CurrentPlayerId == "thomas")
		{
			basicMatchUIView.SetSprite(Resources.Load <Sprite> ("thomas_selfie"));
		}
		else
		{
			basicMatchUIView.SetSprite(Resources.Load <Sprite> ("spongebob_selfie"));
		}

		basicMatchUIView.SetPlayerNameText(GameManager.Instance.CurrentPlayerId);
		basicMatchUIView.SetTimeText("Time: 90");
		basicMatchUIView.SetManaText("0/10");
		basicMatchUIView.SetManaMeter(0);

		Action<bool, TurnBasedMatch, string> cb =
			(success, match, errors) =>
		{
			if (success)
			{
				string matchId = match.MatchId;
				Debug.Log("GridManager:Start() starting match: " + matchId);
				GameManager.Instance.CurrentMatchId = matchId;

				string status = match.Status;
				Debug.Log("GridManager:Start() status: " + status);
				GameManager.Instance.CurrentMatchStatus = status;
				string pendingParticipantId = match.PendingParticipantId;
				Debug.Log("GridManager:Awake() pendingParticipantId: " + pendingParticipantId);
				GameManager.Instance.CurrentPendingParticipantId = pendingParticipantId;

				foreach (TurnBasedMatchParticipant p in match.Participants)
				{
					string participantId = p.Player.ParticipantId;
					Debug.Log("GridManager:Start() participantId: " + participantId);
					if (participantId == GameManager.Instance.CurrentPlayerId)
					{
						GameManager.Instance.CurrentMatchPlayerId = p.Id;
					}
					else
					{
						// TODO: refactor for more than 2 players
						GameManager.Instance.CurrentMatchOpponentPlayerId = p.Player.ParticipantId;

					}
				}

				isPlayerTurn = (GameManager.Instance.CurrentMatchPlayerId == GameManager.Instance.CurrentPendingParticipantId);
				if (isPlayerTurn)
				{
					Debug.Log("GridManager:Awake() it's my turn!");
				}
				else
				{
					Debug.Log("GridManager:Awake() it's the other player's turn!");
				}

				GameManager.Instance.Hand = match.Data.Data.GetCardPile("hand");
				GameManager.Instance.DrawPile = match.Data.Data.GetCardPile("draw_pile");
				GameManager.Instance.DiscardPile = match.Data.Data.GetCardPile("discard_pile");
                GameManager.Instance.Units = match.Data.Data.GetUnits("units");

                // todo: update displayed units based on GameManager.Instance.Units data
                // create new UnitViews for any newly added Units
                // update the position of previously existing Units
                // index Units locally by MatchId

                cards = new List<CardView>();
				cards.Clear();

				List<Vector2> cardLocations = new List<Vector2>();
				int y = -260;
				cardLocations.Add(new Vector2(-250, y));
				cardLocations.Add(new Vector2(-140, y));
				cardLocations.Add(new Vector2(0, y));
				cardLocations.Add(new Vector2(120, y));
				cardLocations.Add(new Vector2(250, y));

				for (int i = 0; i < 5; i++)
				{
					GameObject cardGameObject = (GameObject)Instantiate(CardPrefab);
					CardLevelSheet card1 = new CardLevelSheet(GameManager.Instance.Hand.Get(i));
					CardView cardView = (CardView)(cardGameObject.GetComponent("CardView"));
					cardView.Id = card1.ID;
					cardView.transform.SetParent(Canvas.transform, false);
					cardView.BeginDragDel = CardBeginDrag;
					cardView.EndDragDel = CardEndDrag;
					cardView.DragDel = CardDrag;
					cardView.PointerClickDel = CardPointerClick;
					cardView.PointerEnterDel = CardPointerEnter;
					cardView.PointerExitDel = CardPointerExit;
					cardView.SetCost(card1.Cost);
					cardView.SetLevel(card1.Level);
					cardView.SetType(card1.Type);
					cardView.SetSubtype(card1.Subtype);
					cardView.SetName(card1.Name);
					cardView.SetText(card1.Text);
					cardView.SetImage(Resources.Load <Sprite> (card1.Art));
					cardView.SetRarity(card1.Rarity);
					cardView.SetFaction(card1.Faction);
					RectTransform cardRT = (RectTransform)cardGameObject.GetComponent("RectTransform");
					cardRT.anchoredPosition = cardLocations[i];
					cards.Add(cardView);
					cardLevelSheets[cardView] = card1;
				}
			}
			else
			{
				Debug.Log("WARN!!! match info null");
			}
		};
		StartCoroutine(Platform.Instance.GetMatchInfo(GameManager.Instance.CurrentMatchId, cb));
		StartCoroutine(CheckMatchStatusLoop());	
	}

	private bool isInsideCardsPanel = false;

	public void CardsPanelEnter()
	{
		isInsideCardsPanel = true;
	}

	public void CardsPanelExit()
	{
		if (mediumCardView != null)
		{
			mediumCardView.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
		}
		isInsideCardsPanel = false;
		foreach (CardView v in cards)
		{
			v.gameObject.SetActive(true);
		}
	}

	private Vector3 Delta;
	void CardBeginDrag(CardView v)
	{
        if (isPlayerTurn)
        {
            Debug.Log("MatchController:CardBeginDrag");
            Delta = Input.mousePosition - v.transform.position;
        }
	}

	void CardEndDrag(CardView v)
	{
        if (isPlayerTurn)
        {
            Debug.Log("MatchController:CardEndDrag");
            isMediumCardDragging = false;
        }
    }
    //bool isSmallCardDragging = false;
	void CardDrag(CardView v)
	{
        if (isPlayerTurn)
        {
            Debug.Log("MatchController:CardDrag");
            v.transform.position = Input.mousePosition - Delta;
            //isMediumCardDragging = false;
        }
    }

	private static readonly int ANCHORED_POSITION_WIDTH = 1900;
	private static readonly int ANCHORED_POSITION_HEIGHT = 1400;
	private static readonly int MOUSE_POSITION_WIDTH = 607;
	private static readonly int MOUSE_POSITION_HEIGHT = 455;

	Point MousePositionToAnchoredPosition(Vector3 m)
	{
		return new Point((int) ((m.x * ANCHORED_POSITION_WIDTH) / MOUSE_POSITION_WIDTH), (int) ((m.y * ANCHORED_POSITION_HEIGHT) / MOUSE_POSITION_HEIGHT));
	}

	private bool isMediumCardDragging = false;

	void Update()
	{
        if (isMediumCardDragging && mediumCardView != null)
		{
			mediumCardView.transform.position = Input.mousePosition - Delta;
			mediumCardView.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		}

	}

	void MediumCardBeginDrag(CardView v)
	{
        if (isPlayerTurn && !isDisplayingConfirm)
        {
            Delta = Input.mousePosition - v.transform.position;
            Delta.y += 30;
            v.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            isMediumCardDragging = true;
        }
	}

	void DoAccept(ConfirmView v)
	{
		GameObject.Destroy(v.gameObject);
		isDisplayingConfirm = false;
        CardLevelSheet c = cardLevelSheets[mediumCardView];
        Point p = selectedTileView.Location;
        Unit u = new Unit(c.Name, c.ID, c.Type, c.Subtype, c.Text, c.Text, c.Text, p);
		UnitView unitView = (UnitView)((GameObject)Instantiate (UnitPrefab)).GetComponent("UnitView");
		unitView.MouseEnter = UnitViewEnter;
		unitView.MouseExit = UnitViewExit;
		unitView.MouseOver = UnitViewOver;
		unitView.SetPosition(mapView.GetTileViewAt(p).GetPosition());
		unitView.SetColor(Color.white);
		unitViews[u] = unitView;
		units[unitView] = u;

        List<string> toRemove = new List<string>();

		foreach (CardView cv in cards)
		{
			if (cv.Id == c.ID)
			{
                toRemove.Add(cv.Id);
			}
		}

        foreach (string s in toRemove)
        {
            foreach (CardView cv in cards)
            {
                if (cv.Id == s)
                {
                    cards.Remove(cv);
                    GameObject.Destroy(cv.gameObject);
                    break;
                }
            }
        }

		mapView.Clear();
        cardLevelSheets.Remove(mediumCardView);
        playerActions.Add(new SummonAction(c, u.Location));
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnAsync());
    }
    public IEnumerator EndTurnAsync()
    {
        Action<bool, TurnBasedMatch, string> callback =
            (success, match, errors) =>
        {
            if (success)
            {
                Debug.Log("MatchController:EndTurn pendingParticipantId: " + match.PendingParticipantId);
                isPlayerTurn = match.PendingParticipantId == GameManager.Instance.CurrentMatchPlayerId;
                //isPlayerTurn = false;
            }
            else
            {
                Debug.Log("GridManager:TileOnMouseOver TakeTurn unsuccessful");
            }
        };
        yield return StartCoroutine(Platform.Instance.TakeTurnPlayerAction(GameManager.Instance.CurrentMatchId, playerActions, callback));
    }

	void DoDecline(ConfirmView v)
	{
		GameObject.Destroy(v.gameObject);
		isDisplayingConfirm = false;
        CardLevelSheet s = cardLevelSheets[mediumCardView];
        cardLevelSheets.Remove(mediumCardView);
        foreach (CardView cv in cards)
        {
            if (cv.Id == s.ID)
            {
                cv.gameObject.SetActive(true);
            }
        }
        mapView.Clear();
    }
	
	void DisplayConfirm()
	{
		if (!isDisplayingConfirm)
		{
			GameObject confirmObject = (GameObject) Instantiate(ConfirmPrefab);
			confirmObject.transform.SetParent(Canvas.transform, false);
			ConfirmView confirmView = (ConfirmView)(confirmObject.GetComponent("ConfirmView"));
			CardLevelSheet c = cardLevelSheets[mediumCardView];
			confirmView.SetMessage("Are you sure you want to summon '" + c.ID + "'?");
			confirmView.AcceptDel = DoAccept;
			confirmView.DeclineDel = DoDecline;
			isDisplayingConfirm = true;
			selectedTileView = currentTile;

            foreach (CardView cv in cards)
            {
                if (cv.Id == c.ID)
                {
                    cv.gameObject.SetActive(false);
                }
            }

            GameObject.Destroy(mediumCardObject);
            GameObject.Destroy(mediumCardView);
            isMediumCardDragging = false;
        }
	}

	void MediumCardEndDrag(CardView v)
	{
        if (isPlayerTurn)
        {
            Debug.Log("current tile location: " + currentTile.Location);
            isMediumCardDragging = false;
            DisplayConfirm();
        }
	}

	void MediumCardDrag(CardView v)
	{
        if (isPlayerTurn)
        {
            v.transform.position = Input.mousePosition - Delta;
            v.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //isMediumCardDragging = true;
        }
    }

	void MediumCardPointerClick(CardView v)
	{
        if (isPlayerTurn && !isDisplayingConfirm)
        {
            if (Input.GetMouseButtonUp(1))
            {
                DisplayLargeCardView(v.Id);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (isMediumCardDragging)
                {
                    DisplayConfirm();
                }
                if (!isDisplayingConfirm)
                {
                    //isMediumCardDragging = !isMediumCardDragging;
                    isMediumCardDragging = true;
                    v.BeginDrag();
                }
            }
        }
	}

	void MediumCardPointerEnter(CardView v)
	{
		
	}

	void MediumCardPointerExit(CardView v)
	{
		GameObject.Destroy(mediumCardObject);
		GameObject.Destroy(mediumCardView);
		GameObject.Destroy(v);
		isMediumCardDragging = false;

        if (!isDisplayingConfirm)
        {
            foreach (CardView cv in cards)
            {
                cv.gameObject.SetActive(true);
            }
        }
    }

    void CardPointerEnter(CardView v)
	{
		if (mediumCardObject == null && mediumCardView == null)
		{
			Debug.Log("MatchController:CardPointerEnter()");
			v.gameObject.SetActive(false);
			mediumCardObject = (GameObject)Instantiate(CardPrefab);
			CardLevelSheet card = new CardLevelSheet(cardLevelSheets[v]);
			mediumCardView = (CardView)(mediumCardObject.GetComponent("CardView"));
			mediumCardView.Id = card.ID;
			mediumCardView.transform.SetParent(Canvas.transform, false);
			mediumCardView.BeginDragDel = MediumCardBeginDrag;
			mediumCardView.EndDragDel = MediumCardEndDrag;
			mediumCardView.DragDel = MediumCardDrag;
			mediumCardView.PointerClickDel = MediumCardPointerClick;
			mediumCardView.PointerEnterDel = MediumCardPointerEnter;
			mediumCardView.PointerExitDel = MediumCardPointerExit;
			mediumCardView.SetCost(card.Cost);
			mediumCardView.SetLevel(card.Level);
			mediumCardView.SetType(card.Type);
			mediumCardView.SetSubtype(card.Subtype);
			mediumCardView.SetName(card.Name);
			mediumCardView.SetText(card.Text);
			mediumCardView.SetImage(Resources.Load <Sprite> (card.Art));
			mediumCardView.SetRarity(card.Rarity);
			mediumCardView.SetFaction(card.Faction);
			mediumCardView.transform.localScale = new Vector3(2, 2, 1);
			mediumCardView.transform.position = Input.mousePosition;
			cardLevelSheets[mediumCardView] = card;
		}
	}

	void CardPointerExit(CardView v)
	{
		Debug.Log("MatchController:CardPointerExit()");
		v.gameObject.SetActive(true);
	}

	void DisplayLargeCardView(string id)
	{
		// todo: don't replace "'" with "" here. replace that when reading data from Google Drive
		Debug.Log("MatchController:CardPointerClick()");
		largeCardViewObj = GameObject.Instantiate(LargeCardViewPrefab);
		largeCardViewObj.transform.SetParent(Canvas.transform, false);
		largeCardView = (LargeCardView) largeCardViewObj.GetComponent("LargeCardView");
		CardLevelSheet c = GameManager.Instance.CardData[id];
		largeCardView.Id = c.ID;
		largeCardView.SetCost(c.Cost);
		largeCardView.SetLevel(c.Level);
		largeCardView.SetName(c.Name);
		largeCardView.SetType(c.Type);
		largeCardView.SetSubtype(c.Subtype);
		largeCardView.SetText(c.Text);
		largeCardView.PreviousDel = LUVPrevious;
		largeCardView.NextDel = LUVNext;
		largeCardView.BackDel = LUVBack;
		largeCardView.Image.sprite = Resources.Load <Sprite> (c.Art); // todo: SetImage and make Image not settable publicly
		largeCardView.SetRarity(c.Rarity);
		largeCardView.SetFaction(c.Faction);
		
		largeCardViewVisible = true;
	}

	void CardPointerClick(CardView v)
	{
		if (Input.GetMouseButtonUp(1))
		{
			if (!largeCardViewVisible)
			{
				DisplayLargeCardView(v.Id);
			}
		}
	}

	IEnumerator CheckMatchStatus()
	{
		Action<bool, TurnBasedMatch, string> cb =
			(success, match, errors) =>
		{
			if (success)
			{
				Debug.Log("GridManager:CheckMatchStatus success");

				int turn = 0;
				int time = 90;
				int mana = 0;

				basicMatchUIView.SetTurnText("Turn: " + turn.ToString());
				basicMatchUIView.SetTimeText("Time: "  + time.ToString());
				basicMatchUIView.SetManaText(mana.ToString() + "/10");
				basicMatchUIView.SetManaMeter(mana);
								
				string pendingParticipantId = match.PendingParticipantId;
				if (String.Compare(pendingParticipantId, GameManager.Instance.CurrentMatchPlayerId) == 0)
				{
					Debug.Log("GridManager:Start() it's my turn: " + pendingParticipantId);
					if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player1") == 0)
					{
						currentPlayer = player1;
//						unitViews[player2].SetColor(Color.white);
					}
					else if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player2") == 0)
					{
						currentPlayer = player2;
//						unitViews[player1].SetColor(Color.white);
					}
					else
					{
						Debug.Log("GridManager:Awake unknown player ID");
					}

					isPlayerTurn = true;
//					unitViews[currentPlayer].SetColor(Color.red);
				}
				else
				{
					if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player1") == 0)
					{
//						unitViews[player2].SetColor(Color.red);
					}
					else if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player2") == 0)
					{
//						unitViews[player1].SetColor(Color.red);
					}
					else
					{
						Debug.Log("GridManager:Awake unknown player ID");
					}
//					unitViews[currentPlayer].SetColor(Color.white);
				}

				string currentPlayerId = GameManager.Instance.CurrentMatchPlayerId;
				
				TurnBasedMatchData d = match.Data;
				TurnBasedMatchDataStructure ds = d.Data;
				
				try
				{
					GameManager.Instance.CurrentMatchTurnNumber = ds.GetInt("turn");
					basicMatchUIView.SetTurnText("Turn " + GameManager.Instance.CurrentMatchTurnNumber.ToString());

					int manaPoints = ds.GetInt("mana");
					basicMatchUIView.SetManaText(manaPoints.ToString() + "/10");
					basicMatchUIView.SetManaMeter(manaPoints);

					if (GameManager.Instance.CurrentMatchTurnNumber >= 5)
					{
						ResultsImage.SetActive(true);
						
						if (GameManager.Instance.CurrentMatchPlayerId == "player2")
						{
							resultsText.text = "You Lose!";
						}
						else
						{
							resultsImage.color = Color.yellow;
						}
	 					
						// TODO: clone results image (you win/you lose) and attach to Canvas
					}
				}
				catch (Exception e)
				{
					Debug.Log("GridManager:CheckMatchStatus TurnNumber failure: " + e.ToString());
				}
				
				// update opponent position
				// TODO: update your own position as long as the player is not moving or selecting
				if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player1") == 0 && !IsUnitMoving() && !map.DestinationSelected && map.OriginTile == null)
				{
					Point p1;
					try {
						p1 = ds.GetPoint("position", GameManager.Instance.CurrentPlayerId);
					}
					catch (Exception e)
					{
						p1 = new Point(Player1StartX, Player1StartY);
					}
					
					Vector3 v = mapView.GetTileViewAt(p1).GetPosition();
//					unitViews[player1].SetPosition(v);
//					player1.Location = p1;

					Point p2;
					try {
						p2 = ds.GetPoint("position", GameManager.Instance.CurrentMatchOpponentPlayerId);
					}
					catch (Exception e)
					{
						p2 = new Point(Player2StartX, Player2StartY);
					}
					Vector3 v2 = mapView.GetTileViewAt(p2).GetPosition();
//					unitViews[player2].SetPosition(v2);
//					player2.Location = p2;
				}
				else if (String.Compare(GameManager.Instance.CurrentMatchPlayerId, "player2") == 0 && !IsUnitMoving() && !map.DestinationSelected && map.OriginTile == null)
				{
					Point point1;
					try
					{
						point1 = ds.GetPoint("position", GameManager.Instance.CurrentMatchOpponentPlayerId);
					}
					catch (Exception e)
					{
						point1 = new Point(Player1StartX, Player1StartY);
					}
					Vector3 v = mapView.GetTileViewAt(point1).GetPosition();
//					unitViews[player1].SetPosition(v);
//					player1.Location = point1;

					Point point2;

					try
					{
						point2 = ds.GetPoint("position", GameManager.Instance.CurrentPlayerId);
					}
					catch (Exception e)
					{
						point2 = new Point(Player2StartX, Player2StartY);
					}
					Vector3 v2 = mapView.GetTileViewAt(point2).GetPosition();
//					unitViews[player2].SetPosition(v2);
//					player2.Location = point2;
				}
			}
			else
			{
				Debug.Log("GridManager:CheckMatchStatus failure");
			}
		};
		yield return StartCoroutine(Platform.Instance.GetMatchInfo(GameManager.Instance.CurrentMatchId, cb));
	}

	IEnumerator CheckMatchStatusLoop()
	{
		while (true)
		{
			yield return StartCoroutine(CheckMatchStatus());
			yield return new WaitForSeconds(MATCH_REFRESH_RATE);
		}
	}

	void TileOnMouseEnter(TileView tv)
	{
		if (isPlayerTurn && isMediumCardDragging && !isDisplayingConfirm)
		{
			if (map.DestinationSelected)
			{
				return;
			}

			Tile t = map.GetTileAt(tv.Location);
			map.SelectedTile = t;
			mapView.SelectedTileView = tv;

			if (map.OriginTile != null)
			{
				Path<Tile> path = map.FindPath();
				List<TileView> pathViews = new List<TileView>();

				foreach (Tile tile in path)
				{
					pathViews.Add (mapView.GetTileViewAt(tile.Location));
				}
				
				mapView.SetPathViews(pathViews);
				mapView.DrawSelectionPath();
			}
		
			//when mouse is over some tile, the tile is passable and the current tile is neither destination nor origin tile, change color to orange
			if (!IsUnitMoving() && !t.IsOnPath)
			{
				tv.ChangeColor(ORANGE);
			}
		}
	}

	void TileOnMouseExit(TileView tv)
	{
        if (isPlayerTurn && !isDisplayingConfirm)
            {
			Tile t = map.GetTileAt (tv.Location);
			if (!t.IsOnPath)
			{
				tv.Clear();
			}
		}
	}

	private TileView currentTile;

	IEnumerator TileOnMouseOver(TileView tv)
	{
		currentTile = tv;
		if (isPlayerTurn && isMediumCardDragging)
		{
			Tile t = map.GetTileAt(tv.Location);

			// left click
			if (Input.GetMouseButtonUp(0))
			{
				if (map.OriginTile == null)
				{
//					if (t.Location.Equals(currentPlayer.Location) && !IsUnitMoving())
//					{
//						map.OriginTile = t;
//						mapView.OriginTileView = tv;
//					}
//					else
//					{
//						Debug.Log ("you clicked somewhere that doesn't work");
//					}
				}
				else if (!map.DestinationSelected)
				{
					map.SelectedTile = t;
					map.DestinationSelected = true;

					mapView.SelectedTileView = tv;
					mapView.Clear();

					Path<Tile> path = map.FindPath();
					List<TileView> pathViews = new List<TileView>();
					foreach (Tile tile in path)
					{
						pathViews.Add (mapView.GetTileViewAt(tile.Location));
					}

					mapView.SetPathViews(pathViews);
//					unitViews[currentPlayer].SetPathViews(pathViews);
					mapView.DrawSelectedPath();
				}
				else if (map.DestinationSelected)
				{
					if (t == map.SelectedTile)
					{
						List<Tile> thePath = new List<Tile>();
						Path<Tile> myPath = map.FindPath();
						List<TileView> myPathViews  = new List<TileView>();

						foreach (Tile tile in myPath)
						{
							thePath.Add(tile);
							myPathViews.Add (mapView.GetTileViewAt(tile.Location));
						}

						currentPlayer.Location = thePath[0].Location;
						mapView.DrawSelectedPath();

						yield return StartCoroutine(unitViews[currentPlayer].Walk());

						yield return StartCoroutine(TakeTurn());

						map.Reset();
						map.OriginTile = null;
						map.SelectedTile = null;
						map.DestinationSelected = false;
						mapView.Clear();
					}
					else
					{
						Debug.Log("didn't confirm selection");
					}
				}
			}
		}
	}

	IEnumerator TakeTurn()
	{
		Action<bool, TurnBasedMatch, string> callback = 
			(success, match, errors) =>
		{
			if (success)
			{
				Debug.Log("GridManager:TileOnMouseOver TakeTurn successful");
				Debug.Log("GridManager:TakeTurn pendingParticipantId: " + match.PendingParticipantId);
				isPlayerTurn = match.PendingParticipantId == GameManager.Instance.CurrentMatchPlayerId;
				Debug.Log("GridManager:TileOnMouseOver data: " + match.Data.ToString());
			}
			else
			{
				Debug.Log("GridManager:TileOnMouseOver TakeTurn unsuccessful");
			}
		};

		Dictionary<string, System.Object> newData = new Dictionary<string, System.Object>();
		if (GameManager.Instance.CurrentMatchPlayerId == "player1")
		{
			Dictionary<string, int> l = new Dictionary<string, int>();
			l.Add("x", player1.Location.X);
			l.Add("y", player1.Location.Y);
			newData.Add(GameManager.Instance.CurrentPlayerId, l);
		}
		else if (GameManager.Instance.CurrentMatchPlayerId == "player2")
		{
			Dictionary<string, int> l = new Dictionary<string, int>();
			l.Clear();
			l.Add("x", player2.Location.X);
			l.Add ("y", player2.Location.Y);
			newData.Add(GameManager.Instance.CurrentPlayerId, l);
		}
		
		newData.Add ("turn", GameManager.Instance.CurrentMatchTurnNumber);
		
		yield return StartCoroutine(Platform.Instance.TakeTurn(GameManager.Instance.CurrentMatchId,
		                                                       newData, callback));
		yield return StartCoroutine(CheckMatchStatus());
	}

	public bool IsUnitMoving()
	{
		foreach (UnitView v in unitViews.Values)
		{
			if (v.IsMoving)
			{
				return true;
			}
		}
		return false;
	}

	public Point GetLocation(Vector3 v)
	{
		int x;
		int y;
		tilemap.GetTileAtPosition(v, out x, out y);
		return new Point(x, y);
	}

	public void GoBack()
	{
		Application.LoadLevel("Arena");
	}
}
