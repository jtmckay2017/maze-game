using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMaze : MonoBehaviour
{
    public Maze mazeRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mazeRef != null)
        {
            PlayerMazeManager cc = other.GetComponent<PlayerMazeManager>();
            if (cc)
            {
                cc.FinishCurrentMaze(mazeRef);
            }
        }
    }
}
