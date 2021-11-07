using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandlePlayerNameUpdated))] 
    [SerializeField]
    string playerName = "Missing name";

    [SyncVar(hook = nameof(HandlePlayerColorUpdated))]
    [SerializeField] 
    Color playerColor = Color.white;

    [SerializeField] TMP_Text playerNameText;
    [SerializeField] Renderer playerColorRenderer;

    void Awake()
    {
        playerNameText = GetComponentInChildren<TMP_Text>();
        playerColorRenderer = transform.Find("Player_Body").gameObject.GetComponent<Renderer>();

    }

    #region Server

    [Server]
    public void SetDisplayName(string newPlayerName)
    {
        playerName = newPlayerName;
        
    }

    [Server]
    public void SetRandomPlayerColor()
    {
        playerColor = GenerateRandomColor(); ;

    }
    
    [Command]
    void CmdSetDisplayName(string newPlayerName)
    {
        Debug.Log($"Player has requested to change their name to {newPlayerName} which is {newPlayerName.Length} characters long");
        if (IsPlayerNameLongEnough(newPlayerName))
        {
            RpcLogNewPlayerName(newPlayerName);
            SetDisplayName(newPlayerName);
        }
    }

    bool IsPlayerNameLongEnough(string newPlayerName)
    {
        if (newPlayerName.Length >= 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion


    #region Client
    Color GenerateRandomColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        Color generatedColor = new Color(red, green, blue);
        return generatedColor;
    }

    void HandlePlayerNameUpdated(string oldName, string newName)
    {
        playerNameText.text = newName;
    }

    void HandlePlayerColorUpdated(Color oldColor, Color newColor)
    {
        playerColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    [ContextMenu("SetPlayerName")]
    void SetMyName()
    {
        CmdSetDisplayName("Marku");
    }

    [ClientRpc]
    void RpcLogNewPlayerName(string newPlayerName)
    {
        Debug.Log($"Server updated the player name to {newPlayerName}");
    }

    #endregion
}
