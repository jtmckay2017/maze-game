using System.Collections;
using System.Collections.Generic;
using GraphQlClient.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMazeManager : MonoBehaviour
{

    public Text timerText;
    public Text currentMazeText;
    private MazeDef currentMaze;

    private float timeElasped = 0;

    [Header("API")]
    public GraphApi scoreApi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMaze != null)
        {
            timerText.text = FormatTime(timeElasped);
            timeElasped += Time.deltaTime;
        }
        
    }

    public void StartMaze(MazeDef maze)
    {
        if (currentMaze == null)
        {
            currentMaze = maze;
        }
        timeElasped = 0;
        currentMazeText.text = maze.maze_name;

    }

    public void FinishCurrentMaze(Maze mazeRef)
    {
        if (currentMaze != null && mazeRef.mazeId == currentMaze.id)
        {
            int mazeIdCompleted = currentMaze.id;
            float mazeFinalTime = timeElasped;
            currentMaze = null;
            timeElasped = 0;
            timerText.text = FormatTime(mazeFinalTime);
            CreateNewScore("Test User", mazeFinalTime, mazeIdCompleted);
        }
    }

    public async void CreateNewScore(string name, float time, int maze_id)
    {
        GraphApi.Query query = scoreApi.GetQueryByName("CreateNewScore", GraphApi.Query.Type.Mutation);
        query.SetArgs(new { objects = new { name, time, maze_id } });
        UnityWebRequest request = await scoreApi.Post(query);
        Debug.Log("SCORE POSTED");
    }

    private string FormatTime(float time)
    {
        float totalTime = time;
        //int hours = (int)(totalTime / 3600);
        int minutes = (int)(totalTime / 60) % 60;
        int seconds = (int)totalTime % 60;
        int milliseconds = (int)(totalTime * 1000f) % 1000;


        string answer = minutes.ToString("00") + ":" + seconds.ToString("00") + "." + milliseconds.ToString("00");
        return answer;
    }
}
