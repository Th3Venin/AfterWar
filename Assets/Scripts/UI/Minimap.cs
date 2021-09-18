using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        try
        {
            Vector3 newPosition = GameManager.players[Client.instance.gameId].transform.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, GameManager.players[Client.instance.gameId].transform.eulerAngles.y, 0);

        } catch (Exception)
        {
            //Debug.LogError("Player not spawned yet");
        }
    }
}
