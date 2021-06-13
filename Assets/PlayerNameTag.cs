using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameTag : MonoBehaviour
{
    Text textElement;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponentInParent<PhotonView>();
        textElement = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(view.Owner.NickName);
        textElement.text = view.Owner.NickName;
    }
}
