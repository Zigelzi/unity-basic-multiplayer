using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn) {
        base.OnServerAddPlayer(conn);

        // Identity is reference to the client which is connected.
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        string playerName = $"Player {numPlayers}";

        player.SetDisplayName(playerName);
        player.SetRandomPlayerColor();
    }
}
