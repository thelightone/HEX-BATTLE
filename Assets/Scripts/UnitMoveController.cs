using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerController;

public class UnitMoveController : MonoBehaviour
{
    public UnitFightController fightController;
    public BeAim beAim;

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
    public int step = 0;


    

    private void Start()
    {
        fightController = GetComponent<UnitFightController>();
        beAim = GetComponentInChildren<BeAim>();

        gameObject.transform.position = startTile.transform.position + new Vector3(0, 1, 0);

        choose.SetActive(false);
        active.SetActive(false);

        currentTile = startTile;
        currentTile.busy = true;
        currentTile.unitOn = this;
        beAim.UpdateCoord();

        if (player.player == Player.left)
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public void Move(HexTile destTile, HexTile enemy)
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
            StartCoroutine(Step(enemy));
        }
    }

    private IEnumerator Step(HexTile enemy)
    {
        while (currentTile != destTile2 && currentPath.Count > 1)
        {
            step++;
            float elapsedTime = 0;
            float waitTime = 0.25f;
            currentTile = currentPath[0];
            nextTile = currentPath[1];
            
            var startPoint = transform.position;
            var startRot = transform.GetChild(0).transform.rotation;

            Vector3 relativePos = new Vector3(nextTile.transform.position.x, 0, nextTile.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            var targRot = Quaternion.LookRotation(relativePos, Vector3.up);

          
            while (elapsedTime < waitTime && step>1)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, targRot, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }


            elapsedTime = 0;
            waitTime = 0.5f;

            while (elapsedTime < waitTime)
            {              
                transform.position = Vector3.Lerp(startPoint, nextTile.transform.position + new Vector3(0, 1, 0), (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }


            currentPath.RemoveAt(0);
        }

        if (enemy!=null)
        {
            float elapsedTime = 0;
            float waitTime = 0.25f;

            var startRot = transform.GetChild(0).transform.rotation;

            Vector3 relativePos = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);

            var targRot = Quaternion.LookRotation(relativePos, Vector3.up);

            while (elapsedTime < waitTime && transform.GetChild(0).transform.rotation != targRot)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, targRot, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            fightController.DoDamage(enemy);
        }

        choose.SetActive(false);
        currentTile = destTile2;
        destTile2.busy = true;
        destTile2.unitOn = this;
        TileManager.Instance.DisSelect();
        TileManager.Instance.DisChooseUnit(this);
        isMoving = false;
        beAim.UpdateCoord();
        BattleSystem.Instance.OnAct();
        step = 0;
    }

    public void Range(HexTile checkedHex)
    {
        if (!currentRange.Contains(currentTile))
        {
            currentRange.Add(currentTile);
        }

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

