using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public HexTile startTile;
    public HexTile currentTile;
    public HexTile nextTile;
    public HexTile destTile2;

    public int goRange = 2;

    public GameObject choose;

    public Vector3 targetPosition;
    public bool gotPath;
    public bool isMoving = false;
    public List<HexTile> currentPath;
    public List<HexTile> currentRange;

    private Pathfinder pathfinder;

    private void Start()
    {
        gameObject.transform.position = startTile.transform.position + new Vector3(0, 1, 0);
        choose = GameObject.Find("Choose");
        choose.SetActive(false);

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

            destTile2 = destTile;
            currentPath = Pathfinder.FindPath(currentTile, destTile);
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
        isMoving = false;
        TileManager.Instance.activeUnit = null;
        TileManager.Instance.DisSelect();
    }

    public void Range(HexTile checkedHex)
    {
        foreach (var hex in checkedHex.neigh)
        {
            var range = Pathfinder.FindPath(currentTile, hex);
            if (range.Count > goRange + 1)
            {
                continue;
            }
            var rangeField = hex.transform.GetChild(0).GetChild(0).gameObject;
            if (!rangeField.activeSelf)
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

