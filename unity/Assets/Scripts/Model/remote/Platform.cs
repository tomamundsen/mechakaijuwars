
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class Platform : MonoBehaviour
{
	private static Platform mInstance = null;

	private readonly static string PLATFORM_HOST = "http://127.0.0.1:10008/";

	public static Platform Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = (new GameObject("PlatformContainer")).AddComponent<Platform>();
			}
			return mInstance;
		}
	}

	public IEnumerator Login(string username, string password, Action<bool, string, string, string> callback)
	{
		Debug.Log("Platform:Login()");

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers["Authorization"] = "Basic " + System.Convert.ToBase64String(
			System.Text.Encoding.ASCII.GetBytes(username + ":" + password));

		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				var json_results = JSON.Parse(results);
				callback(true, json_results["access_token"].Value, json_results["player_id"].Value, "");
			}
			else
			{
				var json_errors = JSON.Parse(errors);
				callback(false, null, null, json_errors["message"].Value);
			}
		};

		yield return StartCoroutine(Network.Instance.Post(PLATFORM_HOST, "auth", "login", headers, null, cb));
	}

	public IEnumerator Logout(Action<bool> callback)
	{
		Debug.Log("Platform:Logout()");

		Action<bool, string, string> cb = 
			(success, results, errors) =>
		{
			if (success)
			{
				var json_results = JSON.Parse(results);
				if (json_results["success"].Value == "true")
				{
					callback(true);
				}
				else
				{
					callback(false);
				}
			}
			else
			{
				var json_errors = JSON.Parse(errors);
				callback(false);
			}
		};

		yield return StartCoroutine(Network.Instance.Post(PLATFORM_HOST, "auth", "logout", null, null, cb));
	}

	public IEnumerator ListMatches(Action<bool, List<string>, string> callback)
	{
		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				var dict = MiniJSON.Json.Deserialize(results) as Dictionary<string,System.Object>;
				List<System.Object> list = ((dict["matches"]) as List<System.Object>);
				List<string> matches = new List<string>();
				foreach (System.Object i in list)
				{
					Dictionary<string, System.Object> match = (Dictionary<string, System.Object>) i;
					System.Object obj;
					match.TryGetValue("matchId", out obj);
					string matchId = (string) obj;
					matches.Add(matchId);
				}
				callback(true, matches, "");
			}
			else
			{
				var json_errors = JSON.Parse (errors);
				callback(false, null, json_errors["message"]);
			}
		};

		yield return StartCoroutine(Network.Instance.Get (PLATFORM_HOST, "turnbasedmatches", "", null, cb));
	}

	public IEnumerator ListDecks(string playerID, Action<bool, Dictionary<string, string>, string> callback)
	{
		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				var list = MiniJSON.Json.Deserialize(results) as List<System.Object>;
				Dictionary<string, string> decks = new Dictionary<string, string>();
				foreach (System.Object i in list)
				{
					Dictionary<string, System.Object> deck = ((i) as Dictionary<string, System.Object>);

					System.Object obj;
					deck.TryGetValue("deckId", out obj);
					string deckId = (string) obj;
					deck.TryGetValue ("displayName", out obj);
					string displayName = (string) obj;

					decks.Add(deckId, displayName);
				}
				callback(true, decks, "");
			}
			else
			{
				var json_errors = JSON.Parse (errors);
				callback(false, null, json_errors["message"]);
			}
		};
		
		yield return StartCoroutine(Network.Instance.Get (PLATFORM_HOST, "players/" + playerID + "/categories/decks", "", null, cb));
	}

	private TurnBasedMatch ParseMatch(string results)
	{
		string matchKind = "";
		string id = "";
		string applicationId = "";
		string variant = "";
		string status = "";
		string userMatchStatus = "";
		List<TurnBasedMatchParticipant> ppp = new List<TurnBasedMatchParticipant>();
		
		string creationDetailsKind = "";
		string creationDetailsParticipantId = "";
		long creationDetailsModifiedTimestampMillis = 0;
		TurnBasedMatchModification creationDetails = new TurnBasedMatchModification(creationDetailsKind, creationDetailsParticipantId, 
		                                                                            creationDetailsModifiedTimestampMillis);
		string lastUpdateDetailsKind = "";
		string lastUpdateParticipantId = "";
		long lastUpdateModifiedTimestampMillis = 0;
		TurnBasedMatchModification lastUpdateDetails = new TurnBasedMatchModification(lastUpdateDetailsKind, lastUpdateParticipantId,
		                                                                              lastUpdateModifiedTimestampMillis);
		string autoMatchingCriteriaKind = "";
		int autoMatchingCriteriaMinAutoMatchingPlayers = 0;
		int autoMatchingCriteriaMaxAutoMatchingPlayers = 0;
		int autoMatchingCriteriaExclusiveBitmask = 0;
		TurnBasedAutoMatchingCriteria autoMatchingCriteria = new TurnBasedAutoMatchingCriteria(autoMatchingCriteriaKind, 
		                                                                                       autoMatchingCriteriaMinAutoMatchingPlayers,
		                                                                                       autoMatchingCriteriaMaxAutoMatchingPlayers,
		                                                                                       autoMatchingCriteriaExclusiveBitmask);
		string dataKind = "";
		bool dataDataAvailable = false;
		TurnBasedMatchDataStructure dataData = new TurnBasedMatchDataStructure(null);
		TurnBasedMatchData data = new TurnBasedMatchData(dataKind, dataDataAvailable, dataData);
		
		string myResultsKind = "";
		string myResultsParticipantId = "";
		string myResultsResult = "";
		string myResultsPlacing = "";
		ParticipantResult myResults = new ParticipantResult(myResultsKind, myResultsParticipantId, myResultsResult, myResultsPlacing);
		string inviterId = "";
		string withParticipantId = "";
		string description = "";
		string pendingParticipantId = "";
		string matchVersion = "";
		string rematchId = "";
		string matchNumber = "";
		
		string previousMatchDataKind = "";
		bool previousMatchDataDataAvailable = false;
		TurnBasedMatchDataStructure previousMatchDataData = new TurnBasedMatchDataStructure(null);
		TurnBasedMatchData previousMatchData = new TurnBasedMatchData(previousMatchDataKind, previousMatchDataDataAvailable,
		                                                              previousMatchDataData);
		
		Dictionary<string, System.Object> resp = MiniJSON.Json.Deserialize(results) as Dictionary<string, System.Object>;
		System.Object ooo;
		resp.TryGetValue("match", out ooo);
		Dictionary<string, System.Object> ddd = (Dictionary<string, System.Object>) ooo;
		Debug.Log(ddd.ToString());
		foreach (KeyValuePair<string, System.Object> matchData in ddd)
		{
			if (matchData.Key == "kind")
			{
				if (matchData.Value != null)
				{
					matchKind = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "matchId")
			{
				if (matchData.Value != null)
				{
					id = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "applicationId")
			{
				if (matchData.Value != null)
				{
					applicationId = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "variant")
			{
				if (matchData.Value != null)
				{
					variant = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "status")
			{
				if (matchData.Value != null)
				{
					status = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "userMatchStatus")
			{
				if (matchData.Value != null)
				{
					userMatchStatus = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "participants")
			{
				ppp = new List<TurnBasedMatchParticipant>();
				
				List<System.Object> participants = new List<System.Object>();// = matchData.Value as List<System.Object>;
				if (matchData.Value != null)
				{
					participants = matchData.Value as List<System.Object>;
				}
				foreach (System.Object o in participants)
				{
					Dictionary<string, System.Object> participantData = (Dictionary<string, System.Object>) o;
					System.Object v;
					participantData.TryGetValue("id", out v);
					string playerId = "";
					if (v != null)
					{
						playerId = (string) v;
					}
					
					participantData.TryGetValue("kind", out v);
					string k = "";
					if (v != null)
					{
						k = (string) v;
					}
					
					participantData.TryGetValue("autoMatched", out v);
					bool autoMatched = false;
					if (v != null)
					{
						autoMatched = bool.Parse((string)v);
					}
					
					participantData.TryGetValue("status", out v);
					string playerStatus = "";
					if (v != null)
					{
						playerStatus = (string) v;
					}
					
					participantData.TryGetValue("player", out v);
					Dictionary<string, System.Object> playerD = new Dictionary<string, System.Object>();// = (Dictionary<string, System.Object>) v;
					if (v != null)
					{
						playerD = (Dictionary<string, System.Object>) v;
					}
					
					string kind = "";
					string participantId = "";
					string avatarImageUrl = "";
					foreach (KeyValuePair<string, System.Object> playerData in playerD)
					{
						if (playerData.Key == "kind")
						{
							if (playerData.Value != null)
							{
								kind = (string) playerData.Value;
							}
						}
						else if (playerData.Key == "participantId")
						{
							if (playerData.Value != null)
							{
								participantId = (string) playerData.Value;
							}
						}
						else if (playerData.Key == "avatarImageUrl")
						{
							if (playerData.Value != null)
							{
								avatarImageUrl = (string) playerData.Value;
							}
						}
					}
					Player player = new Player("player", participantId, avatarImageUrl);
					TurnBasedMatchParticipant p = new TurnBasedMatchParticipant(k,playerId, player, autoMatched, playerStatus);
					ppp.Add(p);
				}
			}
			else if (matchData.Key == "creationDetails")
			{
				string kind = "";
				string participantId = "";
				long modifiedTimestampMillis = 0;
				
				Dictionary<string, System.Object> details = new Dictionary<string, System.Object>();// = matchData.Value as Dictionary<string, System.Object>;
				
				if (matchData.Value != null)
				{
					details = matchData.Value as Dictionary<string, System.Object>;
				}
				
				foreach (KeyValuePair<string, System.Object> p in details)
				{
					if (p.Key == "kind")
					{
						if (p.Value != null)
						{
							kind = (string) p.Value;
						}
					}
					else if (p.Key == "participantId")
					{
						if (p.Value != null)
						{
							participantId = (string) p.Value;
						}
					}
					else if (p.Key == "modifiedTimeStampMillis")
					{
						if (p.Value != null)
						{
							modifiedTimestampMillis = (long) p.Value;
						}
					}
				}						
				creationDetails = new TurnBasedMatchModification(kind, participantId, modifiedTimestampMillis);
			}
			else if (matchData.Key == "lastUpdateDetails")
			{
				string kind = "";
				string participantId = "";
				long modifiedTimestampMillis = 0;
				
				Dictionary<string, System.Object> details = new Dictionary<string, System.Object>();// = matchData.Value as Dictionary<string, System.Object>;
				if (matchData.Value != null)
				{
					details = matchData.Value as Dictionary<string, System.Object>;
				}
				
				foreach (KeyValuePair<string, System.Object> p in details)
				{
					if (p.Key == "kind")
					{
						if (p.Value != null)
						{
							kind = (string) p.Value;
						}
					}
					else if (p.Key == "participantId")
					{
						if (p.Value != null)
						{
							participantId = (string) p.Value;
						}
					}
					else if (p.Key == "modifiedTimeStampMillis")
					{
						if (p.Value != null)
						{
							modifiedTimestampMillis = (long) p.Value;
						}
					}
				}
				lastUpdateDetails = new TurnBasedMatchModification(kind, participantId, modifiedTimestampMillis);
			}
			else if (matchData.Key == "autoMatchingCriteria")
			{
				string kind = "";
				int minAutoMatchingPlayers = 0;
				int maxAutoMatchingPlayers = 0;
				int exclusiveBitmask = 0;
				
				Dictionary<string, System.Object> criteria = new Dictionary<string, System.Object>();// = matchData.Value as Dictionary<string, System.Object>;
				if (matchData.Value != null)
				{
					criteria = matchData.Value as Dictionary<string, System.Object>;
				}
				foreach (KeyValuePair<string, System.Object> p in criteria)
				{
					if (p.Key == "kind")
					{
						if (String.Compare((string) p.Value, "\"null\"") == 0)
						{
							kind = (string) p.Value;
						}
					}
					else if (p.Key == "minAutoMatchingPlayers")
					{
						if (String.Compare((string) p.Value, "\"null\"") == 0)
						{
							minAutoMatchingPlayers = int.Parse((string)p.Value);
						}
					}
					else if (p.Key == "maxAutoMatchingPlayers")
					{
						if (String.Compare((string) p.Value, "\"null\"") == 0)
						{
							maxAutoMatchingPlayers = int.Parse((string)p.Value);
						}
					}
					else if (p.Key == "exclusiveBitmask")
					{
						if (String.Compare((string) p.Value, "\"null\"") == 0)
						{
							exclusiveBitmask = int.Parse((string)p.Value);
						}
					}
				}
				autoMatchingCriteria = new TurnBasedAutoMatchingCriteria(kind, minAutoMatchingPlayers, maxAutoMatchingPlayers,
				                                                         exclusiveBitmask);
			}
			else if (matchData.Key == "data")
			{
				string kind = "";
				bool dataAvailable = false;
				TurnBasedMatchDataStructure theData = new TurnBasedMatchDataStructure(null);

				if (matchData.Value != null)
				{
					Dictionary<string, System.Object> d = matchData.Value as Dictionary<string, System.Object>;

					foreach (KeyValuePair<string, System.Object> p in d)
					{
						if (p.Key == "kind")
						{
							if (String.Compare((string) p.Value, "\"null\"") == 0)
							{
								kind = (string) p.Value;
							}
						}
						else if (p.Key == "dataAvailable")
						{
							if (String.Compare((string) p.Value, "\"null\"") == 0)
							{
								dataAvailable = bool.Parse((string) p.Value);
							}
						}
						else if (p.Key == "data")
						{
							if (p.Value != null)
							{
								theData = new TurnBasedMatchDataStructure((Dictionary<string, System.Object>) p.Value);
							}
						}
					}
				}
				
				data = new TurnBasedMatchData(kind, dataAvailable, theData);
			}
			else if (matchData.Key == "results")
			{
				string kind = "";
				string participantId = "";
				string result = "";
				string placing = "";
				
				if (matchData.Value != null)
				{
					List<System.Object> l = matchData.Value as List<System.Object>;
					if (l != null)
					{
						foreach (System.Object o in l)
						{
							Dictionary<string, System.Object> d = o as Dictionary<string, System.Object>;
							if (d != null)
							{
								foreach (KeyValuePair<string, System.Object> p in d)
								{
									if (p.Key == "kind")
									{
										if (p.Value != null)
										{
											kind = (string) p.Value;
										}
									}
									else if (p.Key == "participantId")
									{
										if (p.Value != null)
										{
											participantId = (string) p.Value;
										}
									}
									else if (p.Key == "result")
									{
										if (p.Value != null)
										{
											result = (string) p.Value;
										}
									}
									else if (p.Key == "placing")
									{
										if (p.Value != null)
										{
											placing = (string) p.Value;
										}
									}
								}
							}
						}
					}
				}
				
				myResults = new ParticipantResult(kind, participantId, result, placing);
			}
			else if (matchData.Key == "inviterId")
			{
				if (matchData.Value != null)
				{
					inviterId = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "withParticipantId")
			{
				if (matchData.Value != null)
				{
					withParticipantId = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "description")
			{
				if (matchData.Value != null)
				{
					description = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "pendingParticipantId")
			{
				if (matchData.Value != null)
				{
					pendingParticipantId = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "matchVersion")
			{
				if (matchData.Value != null)
				{
					matchVersion = matchData.Value.ToString();
				}
			}
			else if (matchData.Key == "rematchId")
			{
				if (matchData.Value != null)
				{
					rematchId = matchData.Key.ToString();
				}
			}
			else if (matchData.Key == "matchNumber")
			{
				if (matchData.Value != null)
				{
					matchNumber = matchData.Key.ToString();
				}
			}
			else if (matchData.Key == "previousMatchData")
			{
				string kind = "";
				bool dataAvailable = false;
				TurnBasedMatchDataStructure theData = new TurnBasedMatchDataStructure(null);
				
				Dictionary<string, System.Object> d = new Dictionary<string, System.Object>();// = matchData.Value as Dictionary<string, System.Object>;
				if (matchData.Value != null)
				{
					d = matchData.Value as Dictionary<string, System.Object>;
				}
				foreach (KeyValuePair<string, System.Object> p in d)
				{
					if (p.Key == "kind")
					{
						kind = (string) p.Value;
					}
					else if (p.Key == "dataAvailable")
					{
						dataAvailable = bool.Parse((string) p.Value);
					}
					else if (p.Key == "data")
					{
						theData = new TurnBasedMatchDataStructure((Dictionary<string, System.Object>) p.Value);
					}
				}
				
				previousMatchData = new TurnBasedMatchData(kind, dataAvailable, theData);
			}
		}
		
		return new TurnBasedMatch(matchKind, id, applicationId, variant, status, userMatchStatus,
		                                      ppp,	creationDetails, lastUpdateDetails,	autoMatchingCriteria,
		                                      data, myResults, inviterId, withParticipantId, description, pendingParticipantId,
		                                      matchVersion, rematchId, matchNumber, previousMatchData);
	}

	public IEnumerator CreateMatch(MatchCriteria c, Action<bool, TurnBasedMatch, string> callback)
	{
		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				callback(true, ParseMatch(results), "");
			}
			else
			{
				Debug.Log("WARN!!! Platform:CreateMatch failed");
				callback(false, null, errors);
			}
		};

		Hashtable data = new Hashtable();
		data.Add("deck_id", GameManager.Instance.CurrentDeckId);
		yield return StartCoroutine(Network.Instance.Post(PLATFORM_HOST, "turnbasedmatches", "create", null, data, cb));
	}

	public IEnumerator GetMatchInfo(string matchId, Action<bool, TurnBasedMatch, string> callback)
	{
		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				callback(true, ParseMatch(results), "");
			}
			else
			{
				Debug.Log("WARN!!! Platform:GetMatchInfo() failed ");
				callback(success, null, errors);
			}
		};

		yield return StartCoroutine(Network.Instance.Get(PLATFORM_HOST, "turnbasedmatches/" + matchId, "", null, cb));
	}

	public IEnumerator TakeTurn(string matchId, Dictionary<string, System.Object> unitData, Action<bool, TurnBasedMatch, string> callback)
	{
		Hashtable postData = new Hashtable();
		postData["data"] = unitData;

		Action<bool, string, string> cb =
			(success, results, errors) =>
		{
			if (success)
			{
				callback(true, ParseMatch(results), "");
			}
			else
			{
				Debug.Log("Platform:TakeTurn failure");
				callback(false, null, errors);
			}
		};

		yield return StartCoroutine(Network.Instance.Post(PLATFORM_HOST, "turnbasedmatches", matchId + "/turn",
		                                                  null, new Hashtable(postData), cb));
	}

    public IEnumerator TakeTurnPlayerAction(string matchId, List<PlayerAction> l, Action<bool, TurnBasedMatch, string> callback)
    {
        Action<bool, string, string> cb =
            (success, results, errors) =>
        {
            if (success)
            {
                callback(true, ParseMatch(results), "");
            }
            else
            {
                Debug.Log("Platform:TakeTurnPlayerAction failure");
                callback(false, null, errors);
            }
        };

        Dictionary<string, System.Object> data = new Dictionary<string, System.Object>();
        List<System.Object> actions = new List<System.Object>();
        foreach (PlayerAction a in l)
        {
            actions.Add(a.GetActionDictionary());
        }
        data.Add("Actions", actions);

        yield return StartCoroutine(Network.Instance.Post(PLATFORM_HOST, "turnbasedmatches", matchId + "/turn",
                                                          null, new Hashtable(data), cb));
    }
}
