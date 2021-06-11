using System.Net.WebSockets;
using GraphQlClient.Core;
using GraphQlClient.EventCallbacks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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


	[Header("Subscription")]
	public Text subscriptionDisplay;

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

	public async void GetQuery()
	{
		loading.SetActive(true);
		UnityWebRequest request = await scoreApi.Post("GetScores", GraphApi.Query.Type.Query);
		loading.SetActive(false);
		queryDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
	}

	public async void CreateNewScore()
	{
		loading.SetActive(true);
		GraphApi.Query query = scoreApi.GetQueryByName("CreateNewScore", GraphApi.Query.Type.Mutation);
		query.SetArgs(new { objects = new { name = _name.text, time = time.text } });
		UnityWebRequest request = await scoreApi.Post(query);
		loading.SetActive(false);
		mutationDisplay.text = HttpHandler.FormatJson(request.downloadHandler.text);
	}

	public async void Subscribe()
	{
		loading.SetActive(true);
		cws = await scoreApi.Subscribe("SubscribeToScores", GraphApi.Query.Type.Subscription, "default");
		loading.SetActive(false);
	}

	public void DisplayData(OnSubscriptionDataReceived subscriptionDataReceived)
	{
		Debug.Log("I was called");
		subscriptionDisplay.text = HttpHandler.FormatJson(subscriptionDataReceived.data);
	}

	public void CancelSubscribe()
	{
		scoreApi.CancelSubscription(cws);
	}
}
