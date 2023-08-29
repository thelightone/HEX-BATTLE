using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeAim : MonoBehaviour
{
    [HideInInspector]
    public UnitMoveController moveController;
    [HideInInspector]
    public HexTile curHex;

  
    public HexTile rightTop;
   
    public HexTile right;
   
    public HexTile rightBot;
   
    public HexTile leftBot;
   
    public HexTile left;
   
    public HexTile leftTop;

    public GameObject rightTopPart;
    public GameObject rightPart;
    public GameObject rightBotPart;
    public GameObject leftBotPart;
    public GameObject leftPart;
    public GameObject leftTopPart;

    public Dictionary<GameObject, HexTile> dict = new Dictionary<GameObject, HexTile>();

    private void Start()
    {
        moveController = GetComponentInParent<UnitMoveController>();

        dict.Add(rightTopPart, rightTop);
        dict.Add(rightPart, right);
        dict.Add(rightBotPart, rightBot);
        dict.Add(leftBotPart, leftBot);
        dict.Add(leftPart, left);
        dict.Add(leftTopPart, leftTop);
    }

    public void UpdateCoord()
    {
        DislightAim(0);

        curHex = moveController.currentTile;
        int x = curHex.offsetCoord.x;
        int y = curHex.offsetCoord.y;

        rightTop = null;
        right = null;
        rightBot = null;
        leftBot = null;
        left = null;
        leftTop = null;

        if (y % 2 == 1)
        {
            foreach (var tile in curHex.neigh)
            {
                if (tile.unitOn)
                {
                    if (tile.unitOn.player == BattleSystem.Instance.curPlayer)
                    {
                        continue;
                    }
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
                else if (tile.offsetCoord == new Vector2Int(x - 1, y + 1))
                {
                    leftBot = tile;
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y))
                {
                    left = tile;
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (tile.offsetCoord == new Vector2Int(x - 1, y - 1))
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
                if (tile.unitOn)
                {
                    if (tile.unitOn.player == BattleSystem.Instance.curPlayer)
                    {
                        continue;
                    }
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

        dict.Clear();

        dict.Add(rightTopPart, rightTop);
        dict.Add(rightPart, right);
        dict.Add(rightBotPart, rightBot);
        dict.Add(leftBotPart, leftBot);
        dict.Add(leftPart, left);
        dict.Add(leftTopPart, leftTop);

    }

    public void LightAims()
    {
        UpdateCoord();

        var range = TileManager.Instance.activeUnit.currentRange;

        if (rightTop != null)
        {
            rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
            rightTopPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (right != null)
        {
            rightPart.transform.GetChild(0).gameObject.SetActive(true);
            rightPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (rightBot != null)
        {
            rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
            rightBotPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (leftBot != null)
        {
            leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
            leftBotPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (left != null)
        {
            leftPart.transform.GetChild(0).gameObject.SetActive(true);
            leftPart.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (leftTop != null)
        {
            leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
            leftTopPart.transform.GetChild(1).gameObject.SetActive(true);
        }


        if (rightTop != null)
        {
            if (rightTop.unitOn!= null)
            {
                if (rightTop.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    rightTopPart.transform.GetChild(0).gameObject.SetActive(true);
                    rightTopPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        if (right != null)
        {
            if (right.unitOn != null)
            {
                if (right.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    rightPart.transform.GetChild(0).gameObject.SetActive(true);
                    rightPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }

        if (rightBot != null)
        {
            if (rightBot.unitOn != null)
            {
                if (rightBot.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    rightBotPart.transform.GetChild(0).gameObject.SetActive(true);
                    rightBotPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        if (leftBot != null)
        {
            if (leftBot.unitOn != null)
            {
                if (leftBot.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    leftBotPart.transform.GetChild(0).gameObject.SetActive(true);
                    leftBotPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        if (left != null)
        {
            if (left.unitOn != null)
            {
                if (left.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    leftPart.transform.GetChild(0).gameObject.SetActive(true);
                    leftPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        if (leftTop != null)
        {
            if (leftTop.unitOn != null)
            {
                if (leftTop.unitOn.player != BattleSystem.Instance.curPlayer)
                {
                    leftTopPart.transform.GetChild(0).gameObject.SetActive(true);
                    leftTopPart.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    public void DislightAim(int i)
    {
        rightTopPart.transform.GetChild(i).gameObject.SetActive(false);
        rightPart.transform.GetChild(i).gameObject.SetActive(false);
        rightBotPart.transform.GetChild(i).gameObject.SetActive(false);
        leftBotPart.transform.GetChild(i).gameObject.SetActive(false);
        leftPart.transform.GetChild(i).gameObject.SetActive(false);
        leftTopPart.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Attack(BeAim beAim,GameObject sector)
    {
        foreach (var key in beAim.dict.Keys)
        {
            if (sector == key)
            {
                TileManager.Instance.activeUnit.Move(beAim.dict[key]);
            }
        }
    }
}