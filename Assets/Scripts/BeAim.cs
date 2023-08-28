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


        if (y % 2 == 1)
        {
            foreach (var tile in curHex.neigh)
            {
                if (tile.busy)
                {
                    continue;
                }
                if (tile.offsetCoord == new Vector2Int(x, y - 1))
                {
                    rightTop = tile;
                    rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y))
                {
                    right = tile;
                    rightPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y + 1))
                {
                    rightBot = tile;
                    rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x-1, y + 1))
                {
                    leftBot = tile;
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y))
                {
                    left = tile;
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x-1, y - 1))
                {
                    leftTop = tile;
                    leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var tile in curHex.neigh)
            {
                if (tile.busy)
                {
                    continue;
                }
                if (tile.offsetCoord == new Vector2Int(x + 1, y - 1))
                {
                    rightTop = tile;
                    rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y))
                {
                    right = tile;
                    rightPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x + 1, y + 1))
                {
                    rightBot = tile;
                    rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y + 1))
                {
                    leftBot = tile;
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y))
                {
                    left = tile;
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x, y - 1))
                {
                    leftTop = tile;
                    leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    public void LightAims()
    {
        var range = TileManager.Instance.activeUnit.currentRange;

        if (range.Contains(rightTop))
        {
            rightTopPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (range.Contains(right))
        {
            rightPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (range.Contains(rightBot))
        {
            rightBotPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (range.Contains(leftBot))
        {
            leftBotPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (range.Contains(left))
        {
            leftPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (range.Contains(leftTop))
        {
            leftTopPart.transform.GetChild(1).gameObject.SetActive(true);
        }


        if (rightTop.unitOn != null)
        {
            if (rightTop.unitOn != BattleSystem.Instance.curPlayer)
            {
                rightTopPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (right.unitOn != null)
        {
            if (right.unitOn != BattleSystem.Instance.curPlayer)
            {
                rightPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (rightBot.unitOn != null)
        {
            if (rightBot.unitOn != BattleSystem.Instance.curPlayer)
            {
                rightBotPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (leftBot.unitOn != null)
        {
            if (leftBot.unitOn != BattleSystem.Instance.curPlayer)
            {
                leftBotPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (left.unitOn != null)
        {
            if (left.unitOn != BattleSystem.Instance.curPlayer)
            {
                leftPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (leftTop.unitOn != null)
        {
            if (leftTop.unitOn != BattleSystem.Instance.curPlayer)
            {
                leftTopPart.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void DislightAim()
    {
        rightTopPart.transform.GetChild(1).gameObject.SetActive(false);
        rightPart.transform.GetChild(1).gameObject.SetActive(false); 
        rightBotPart.transform.GetChild(1).gameObject.SetActive(false);
        leftBotPart.transform.GetChild(1).gameObject.SetActive(false);
        leftPart.transform.GetChild(1).gameObject.SetActive(false);
        leftTopPart.transform.GetChild(1).gameObject.SetActive(false);
}
}