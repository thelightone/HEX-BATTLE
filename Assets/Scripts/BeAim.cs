using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeAim : MonoBehaviour
{
    [HideInInspector]
    public UnitMoveController moveController;
    [HideInInspector]
    public HexTile curHex;

    [HideInInspector]
    public HexTile rightTop;
    [HideInInspector]
    public HexTile right;
    [HideInInspector]
    public HexTile rightBot;
    [HideInInspector]
    public HexTile leftBot;
    [HideInInspector]
    public HexTile left;
    [HideInInspector]
    public HexTile leftTop;

    public GameObject rightTopPart;
    public GameObject rightPart;
    public GameObject rightBotPart;
    public GameObject leftBotPart;
    public GameObject leftPart;
    public GameObject leftTopPart;

    private void Start()
    {
        moveController = GetComponentInParent<UnitMoveController>();
       
    }

    public void UpdateCoord()
    {
        rightTopPart.transform.GetChild(0).gameObject.SetActive(false);
        rightPart.transform.GetChild(0).gameObject.SetActive(false);
        rightBotPart.transform.GetChild(0).gameObject.SetActive(false);
        leftBotPart.transform.GetChild(0).gameObject.SetActive(false);
        leftPart.transform.GetChild(0).gameObject.SetActive(false);
        leftTopPart.transform.GetChild(0).gameObject.SetActive(false);

        curHex = moveController.currentTile;
        int x = curHex.offsetCoord.x;
        int y = curHex.offsetCoord.y;

        foreach (var tile in curHex.neigh)
        {
            if (tile.offsetCoord == new Vector2Int(x+1,y-1))
            {
                rightTop = tile;
                rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (tile.offsetCoord == new Vector2Int(x+1, y))
            {
                right = tile;
                rightPart.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (tile.offsetCoord == new Vector2Int(x+1, y+1))
            {
                rightBot = tile;
                rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (tile.offsetCoord == new Vector2Int(x, y+1))
            {
                leftBot = tile;
                leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (tile.offsetCoord == new Vector2Int(x-1, y))
            {
                left = tile;
                leftPart.transform.GetChild(0).gameObject.SetActive(true);
            }
            if (tile.offsetCoord == new Vector2Int(x, y-1))
            {
                leftTop = tile;
                leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}