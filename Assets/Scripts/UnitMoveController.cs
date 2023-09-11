using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
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

    public Animator animator;


    private void Start()
    {
        fightController = GetComponent<UnitFightController>();
        beAim = GetComponentInChildren<BeAim>();
        animator = GetComponentInChildren<Animator>();

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
        float elapsedTime;
        float waitTime;
        Vector3 startPoint;
        Quaternion startRot;
        Vector3 relativePos;
        Vector3 relativePos2;
        Quaternion targRot;
        Quaternion afterTargRot;
        float smoothProgress;
        bool oneStep = currentPath.Count == 3 ? true : false;

        while (currentTile != destTile2 && currentPath.Count > 1)
        {
            step++;
            elapsedTime = 0;
            waitTime = 0.5f;
            currentTile = currentPath[0];
            nextTile = currentPath[1];

            startPoint = transform.position;
            startRot = transform.GetChild(0).transform.rotation;
            relativePos = new Vector3(nextTile.transform.position.x, 0, nextTile.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            targRot = Quaternion.LookRotation(relativePos, Vector3.up);
            afterTargRot = startRot;

            if (currentPath.Count > 2 && step > 1)
            {
                relativePos2 = new Vector3(currentPath[2].transform.position.x, 0, currentPath[2].transform.position.z) - new Vector3(nextTile.transform.position.x, 0, nextTile.transform.position.z);
                afterTargRot = Quaternion.LookRotation(relativePos2, Vector3.up);
            }

            animator.ResetTrigger("Stop");

            //ROTATE
            //while (elapsedTime < waitTime && step == 2 && Convert.ToInt32(startRot.eulerAngles.y) != Convert.ToInt32(targRot.eulerAngles.y))
            //{
            //    //animator.SetFloat("TurnBlend",1f);
            //    //transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, targRot, (elapsedTime / waitTime));
            //    elapsedTime += Time.deltaTime;

            //    yield return null;
            //}
            //animator.SetFloat("TurnBlend", 0.0f);

            //MOVE
            elapsedTime = 0;

            startRot = transform.GetChild(0).transform.rotation;

            while (elapsedTime < waitTime)
            {
                smoothProgress = elapsedTime / waitTime;
                waitTime = 0.4f;

                if (step == 2)
                {
                    var diff = Convert.ToInt32(targRot.eulerAngles.y) - Convert.ToInt32(startRot.eulerAngles.y);
                    animator.SetFloat("TurnBlend", diff / 10);
                }

                if (step > 1 && !oneStep)
                {
                    animator.SetTrigger("Run");
                }



                if (Math.Abs(Convert.ToInt32(startRot.eulerAngles.y) - Convert.ToInt32(afterTargRot.eulerAngles.y))>20 && currentPath.Count > 2 && step > 1)
                {
                    waitTime = 0.4f;
                    if (step == 2)
                    {
                        waitTime = 1f;
                        smoothProgress = SmoothStart(smoothProgress);
                        
                    }
          
                    transform.position = CubicSpline(startPoint, nextTile.transform.position + new Vector3(0, 1, 0), currentPath[2].transform.position + new Vector3(0, 1, 0), smoothProgress / 2);
                   // waitTime *= 2;
                    transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, afterTargRot, (smoothProgress));
                   // waitTime /= 2;
                    Debug.Log(waitTime + " " + step);
                }
                else
                {

                    if (step > 1)
                    {
                        transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, targRot, (smoothProgress));
                    }
                    if (step == 2)
                    {
                        waitTime = 1f;
                        smoothProgress = SmoothStart(smoothProgress);                      
                    }
                    if (oneStep && step == 2)
                    {
                        animator.SetTrigger("Step");
                        waitTime = 1f;
                        smoothProgress = SmoothEnd(smoothProgress);
                       
                    }
                    if (step != 2 && !oneStep && currentPath.Count == 2)
                    {
                        waitTime = 1f;
                        smoothProgress = SmoothEnd(smoothProgress);
                        animator.ResetTrigger("Run");
                        animator.SetTrigger("Stop");
                    }

                    transform.position = Vector3.Lerp(startPoint, nextTile.transform.position + new Vector3(0, 1, 0), (smoothProgress));
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //if (Convert.ToInt32(startRot.eulerAngles.y) != Convert.ToInt32(afterTargRot.eulerAngles.y) && currentPath.Count > 2 && step > 1)
            //currentPath.RemoveAt(0);
            currentPath.RemoveAt(0);
            animator.ResetTrigger("Step");
            animator.ResetTrigger("Run");
            animator.SetTrigger("Stop");
        }


        if (enemy != null)
        {
            animator.SetTrigger("Turn");
            elapsedTime = 0;
            waitTime = 0.5f;

            startRot = transform.GetChild(0).transform.rotation;

            relativePos = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);

            targRot = Quaternion.LookRotation(relativePos, Vector3.up);

            while (elapsedTime < waitTime && transform.GetChild(0).transform.rotation != targRot)
            {
                transform.GetChild(0).transform.rotation = Quaternion.Lerp(startRot, targRot, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            animator.ResetTrigger("Turn");
            animator.SetTrigger("Stop");

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

    private float SmoothStart(float progress)
    {
        progress = Mathf.Lerp(0, 1, progress);
        progress = progress * progress;
        return progress;
    }

    private float SmoothEnd(float progress)
    {
        progress = Mathf.Lerp(0, 1, progress);
        progress = -1 * (progress - 1) * (progress - 1)  + 1;

        return progress;
    }

    private float SmoothStep(float progress)
    {

        return progress;
    }
    private Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float progress)
    {
        var ab = Vector3.Lerp(a, b, progress);
        var bc = Vector3.Lerp(b, c, progress);
        var abc = Vector3.Lerp(ab, bc, progress);
        return abc;
    }

    private Vector3 CubicSpline(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        float oneMinusT = 1 - t;
        float oneMinusT2 = oneMinusT * oneMinusT;
        float oneMinusT3 = oneMinusT2 * oneMinusT;

        Vector3 interpolatedPoint =
            oneMinusT3 * p1 +
            (3 * oneMinusT2 * t) * p2 +
            (3 * oneMinusT * t2) * p3 +
            t3 * p2; // Здесь используется p2 в качестве четвертой точки

        return interpolatedPoint;
    }

}