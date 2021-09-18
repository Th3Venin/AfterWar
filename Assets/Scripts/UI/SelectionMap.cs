using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionMap : MonoBehaviour, IPointerClickHandler
{
    private bool clicked = false;

    public GameObject markPrefab;
    public GameObject remoteMarkPrefab;
    public float mapScale = 3.85f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clicked)
            return;

        clicked = true;
        Vector2 localCursor;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        SpawnLocalMark(localCursor);
        ClientSend.ChooseSpawn(localCursor);
        localCursor /= mapScale;

        Debug.Log("Spawn Position:" + localCursor);
        GameManager.players[Client.instance.gameId].transform.position = new Vector3(localCursor.x, 20f, localCursor.y);
    }

    public void SpawnLocalMark(Vector2 location)
    {
        var mark = Instantiate(markPrefab) as GameObject;
        mark.GetComponent<RectTransform>().anchoredPosition = location;
        mark.transform.SetParent(transform, false);
    }

    public void SpawnRemotemark(Vector2 location)
    {
        var mark = Instantiate(remoteMarkPrefab) as GameObject;
        mark.GetComponent<RectTransform>().anchoredPosition = location;
        mark.transform.SetParent(transform, false);
    }
}
