using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitMoveController : MonoBehaviour
{
    public UnitFightController fightController;

    public HexTile startTile;
    public HexTile currentTile;
    public HexTile nextTile;
    public HexTile destTile2;

    public List<HexTile> currentPath;
    public List<HexTile> currentRange;

    public GameObject choose;
    public GameObject active;
    public Vector3 targetPosition;
    public PlayerController player;

    public bool gotPath;
    public bool isMoving = false;


    

    private void Start()
    {
        fightController = GetComponent<UnitFightController>();

        gameObject.transform.position = startTile.transform.position + new Vector3(0, 1, 0);

        choose.SetActive(false);
        active.SetActive(false);

        currentTile = startTile;
        currentTile.busy = true;     
    }

    public void Move(HexTile destTile)
    {
        if (!isMoving && currentRange.Contains(destTile))
        {
            UnRange();

            isMoving = true;
            currentTile.busy = false;
            currentTile.unitOn = null;

            destTile2 = destTile;
            currentPath = Pathfinder.FindPath(currentTile, destTile, true);
            currentPath.Add(currentTile);
            currentPath.Reverse();
            StartCoroutine("Step");
        }
    }

    private IEnumerator Step()
    {
        while (currentTile != destTile2 && currentPath.Count > 1)
        {

            currentTile = currentPath[0];
            nextTile = currentPath[1];

            float elapsedTime = 0;
            float waitTime = 0.5f;
            var startPoint = transform.position;

            while (elapsedTime < waitTime)
            {
                transform.position = Vector3.Lerp(startPoint, nextTile.transform.position + new Vector3(0, 1, 0), (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }


            currentPath.RemoveAt(0);
        }

        choose.SetActive(false);
        currentTile = destTile2;
        destTile2.busy = true;
        destTile2.unitOn = this;
        TileManager.Instance.DisSelect();
        TileManager.Instance.DisChooseUnit(this);
        isMoving = false;
        BattleSystem.Instance.OnAct();
    }

    public void Range(HexTile checkedHex)
    {
        foreach (var hex in checkedHex.neigh)
        {
            var range = Pathfinder.FindPath(currentTile, hex, false);
            if (range.Count > fightController.goRange + 1)
            {
                continue;
            }
            var rangeField = hex.transform.GetChild(0).GetChild(0).gameObject;
            if (!rangeField.activeSelf && !hex.busy)
            {
                currentRange.Add(hex);
                rangeField.SetActive(true);
                Range(hex);
            }
            else
            {
                continue;
            }
        }
        return;
    }

    public void UnRange()
    {
        foreach (var hex in currentRange)
        {
            hex.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        currentRange.Clear();
    }

}

