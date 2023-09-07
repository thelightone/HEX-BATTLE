using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    private Camera _mainCamera;
    private HexTile _targetHex;
    private UnitMoveController _targetUnit;
    private BeAim _activeAim;
    private Vector3 _mousePos;
    private Transform objectHit;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    void Update()
    {

        RaycastHit hit;
        var plane = new Plane(Vector3.up, Vector3.zero);
        _mousePos = Input.mousePosition;

        var ray = _mainCamera.ScreenPointToRay(_mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;
            var parent = objectHit.parent;

            if (objectHit.GetComponentInParent<UnitMoveController>())
            {
                HighlightUnit(parent);
            }
            else
            {
                TileManager.Instance.DisLightUnit();
                _activeAim?.DislightAim(1);

                if (TileManager.Instance.activeUnit != null && parent.GetComponent<HexTile>())
                {
                    _targetHex = parent.GetComponent<HexTile>();
                    _targetHex.Highlight();
                    if (Input.GetMouseButtonUp(0))
                    {
                        _targetHex.Select();
                    }
                }
            }
        }
    }

    private void HighlightUnit(Transform parent)
    {

        _targetUnit = objectHit.GetComponentInParent<UnitMoveController>();

        if (_targetUnit.player == BattleSystem.Instance.curPlayer)
        {
            TileManager.Instance.LightUnit(_targetUnit);

            if (Input.GetMouseButtonUp(0))
            {
                TileManager.Instance.ChooseUnit(_targetUnit);
            }
        }
        else if (_targetUnit.player != BattleSystem.Instance.curPlayer
                && TileManager.Instance.activeUnit != null
                && objectHit.GetComponent<AttackSector>())
        {
            _activeAim = _targetUnit.GetComponentInChildren<BeAim>();
            _activeAim.LightAim(objectHit.parent.gameObject);

            if (Input.GetMouseButtonUp(0))
            {
                _activeAim.Attack(_activeAim, parent.gameObject);
            }
        }
    }
}

