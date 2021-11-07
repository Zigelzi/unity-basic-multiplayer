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

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        playerName = newDisplayName;
    }

    [Server]
    public void SetRandomPlayerColor()
    {
        playerColor = GenerateRandomColor(); ;

    }

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

}
