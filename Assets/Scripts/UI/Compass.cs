using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage compassImage;
    Transform player;
    public TMPro.TextMeshProUGUI compassDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		try
        {
			player = GameManager.players[Client.instance.gameId].transform;
        } 
		catch (Exception)
        {
			//Debug.LogError("Player Not Spawned Yet");
			return;
        }

        compassImage.uvRect = new Rect(player.localEulerAngles.y / 360, 0, 1, 1);
        Vector3 forward = player.transform.forward;

        forward.y = 0;

        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

        int displayAngle = Mathf.RoundToInt(headingAngle);

		switch (displayAngle)
		{
			case 0:
				compassDirection.text = "N";
				break;
			case 360:
				compassDirection.text = "N";
				break;
			case 45:
				compassDirection.text = "NE";
				break;
			case 90:
				compassDirection.text = "E";
				break;
			case 130:
				compassDirection.text = "SE";
				break;
			case 180:
				compassDirection.text = "S";
				break;
			case 225:
				compassDirection.text = "SW";
				break;
			case 270:
				compassDirection.text = "W";
				break;
			default:
				compassDirection.text = headingAngle.ToString();
				break;
		}
	}
}
