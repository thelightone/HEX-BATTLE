using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventsListener : MonoBehaviour
{
    private UnitFightController unit;

    private void Start()
    {
        unit = GetComponentInParent<UnitFightController>();
    }
    public void Hit()
    {
        unit.Hit();
    }
    public void PreHit()
    {
        unit.PreHit();
    }
}
