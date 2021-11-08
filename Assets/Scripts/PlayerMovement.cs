using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    Camera playerCamera;
    NavMeshAgent playerNavAgent;

    public override void OnStartServer()
    {
        base.OnStartServer();

        playerNavAgent = GetComponent<NavMeshAgent>();
    }
    [Command]
    void CmdMove(Ray destination)
    {
        if (Physics.Raycast(destination, out RaycastHit hit, Mathf.Infinity))
        {
            playerNavAgent.SetDestination(hit.point);
        }
    }

    #region Client

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        playerCamera = Camera.main;
        
    }

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) { return; }

        HandleMovementCommand();
    }

    void HandleMovementCommand()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray destination = playerCamera.ScreenPointToRay(Input.mousePosition);
            CmdMove(destination);
        }

    }

    

    #endregion


}
