using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    public ExamplePlayer player;
    public Text playerNameText;

    public void SetPlayerNameAndStart()
    {
        if (playerNameText.text.Length != 0 && playerNameText.text.Length < 20)
        {
            player.UpdatePlayerNameTag(playerNameText.text);
            this.gameObject.SetActive(false);
        }
    }
}
