using System.Net.WebSockets;
using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
class Response
{
    public string type;
    public string id;
    public ScorePayload payload;
}

[System.Serializable]
public class ScorePayload
{
    public ScoreData data;
}

[System.Serializable]
public class ScoreData
{
    public ScoreDef[] Scores;
}

[System.Serializable]
public class ScoreDef
{
    public int id;
    public int maze_id;
    public string name;
    public float time;
    public DateTime created_at;
}


public class Score : MonoBehaviour
{
	public GameObject loading;

	[Header("API")]
	public GraphApi scoreApi;

	[Header("Query")]
	public Text queryDisplay;

	[Header("Mutation")]
	public InputField id;
	public InputField _name;
    public InputField time;

    public Text mutationDisplay;

    private ScoreDef[] scores;


	[Header("Subscription")]
	public Text subscriptionDisplay;
    public Maze maze;

	private ClientWebSocket cws;

	private void OnEnable()
	{
		OnSubscriptionDataReceived.RegisterListener(DisplayData);
        this.Subscribe();
	}

	private void OnDisable()
	{
		OnSubscriptionDataReceived.UnregisterListener(DisplayData);
	}

	//public async void GetQuery()
	//{
	//	loading.SetActive(true);
	//	UnityWebRequest request = await scoreApi.Post("GetScores", GraphApi.Query.Type.Query);
	//	loading.SetActive(false);
	//	queryDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
	//}

	//public async void CreateNewScore()
	//{
	//	loading.SetActive(true);
	//	GraphApi.Query query = scoreApi.GetQueryByName("CreateNewScore", GraphApi.Query.Type.Mutation);
	//	query.SetArgs(new { objects = new { name = _name.text, time = time.text } });
	//	UnityWebRequest request = await scoreApi.Post(query);
	//	loading.SetActive(false);
	//	mutationDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
	//}

	public async void Subscribe()
	{
		loading.SetActive(true);
		cws = await scoreApi.Subscribe("SubscribeToScores", GraphApi.Query.Type.Subscription, "default");
		loading.SetActive(false);
	}

	public void DisplayData(OnSubscriptionDataReceived subscriptionDataReceived)
	{
		Debug.Log("I was called");
        Response payload = JsonUtility.FromJson<Response>(HttpHandler.FormatJson(subscriptionDataReceived.data));
        scores = payload.payload.data.Scores;
        subscriptionDisplay.text = HttpHandler.FormatJson(subscriptionDataReceived.data);
        Array.Sort(scores, (a, b) => a.time.CompareTo(b.time));

        // Only show scores for hard maze for now
        ScoreDef[] filteredScores = Array.FindAll(scores, c => c.maze_id == (int)maze.mazeId);
        subscriptionDisplay.text = "";

        int rank = 1;
        foreach (ScoreDef row in filteredScores)
        {
            if (rank == 11) {
                subscriptionDisplay.text += "Ranks past 10 aren't good enough to be displayed sorry ):";
                return;
            }
            subscriptionDisplay.text += string.Format("#{0} | {1} | {2}\n\n", rank, PlayerMazeManager.FormatTime(row.time), row.name);
            rank += 1;
        }


    }

	public void CancelSubscribe()
	{
		scoreApi.CancelSubscription(cws);
	}
}
