using System;
using System.Collections;
using System.Collections.Generic;
using GraphQlClient.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//[System.Serializable]
//class Response
//{
//    public string type;
//    public string id;
//    public Payload payload;
//}

[System.Serializable]
class Payload
{
    public Data data;
}

[System.Serializable]
class Data
{
    public MazeDef[] Mazes;
}

[System.Serializable]
class MazeDef
{
    public int id;
    public string maze_name;
    public string difficulty;
    public DateTime created_at;
}

public class Maze : MonoBehaviour
{

    [Header("API")]
    public GraphApi mazeApi;

    [Header("Setup")]
    public int mazeId;

    public Text mazeDisplay;



    private MazeDef mazeData;

    // Start is called before the first frame update
    void Start()
    {
        GetMaze();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public async void GetMaze()
    {
        //Debug.Log("test");
        UnityWebRequest request = await mazeApi.Post("GetMazes", GraphApi.Query.Type.Query);
        Payload payload = JsonUtility.FromJson<Payload>(HttpHandler.FormatJson(request.downloadHandler.text));
        Debug.Log(payload.data.Mazes);
        foreach (MazeDef mz in payload.data.Mazes)
        {
            if (mz.id == mazeId)
            {
                mazeData = mz;
            }
        };
        if (mazeData != null)
        {
            // Do some setup
            mazeDisplay.text = mazeData.maze_name;
        }

    }
}
    /**
     {"payload":
        {"data":
            {"Mazes": [
                {
                    "id":1,
                    "maze_name":"First Maze",
                    "difficulty":"Easy",
                    "created_at":"2021-06-11T22:07:22.767244+00:00"
                }
            ]}
         }
       }

    **/