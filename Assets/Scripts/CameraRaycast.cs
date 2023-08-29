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
            Transform objectHit = hit.transform;
            var parent = objectHit.parent;

            if (parent.GetComponent<UnitMoveController>())
            {
                HighlightUnit(parent);
            }

            else if (objectHit.GetComponent<AttackSector>())
            {
                _activeAim = objectHit.GetComponentInParent<BeAim>();

                if (TileManager.Instance.activeUnit != null && _activeAim.moveController.player!= BattleSystem.Instance.curPlayer)
                {                    
                    _activeAim.LightAims();

                    if (Input.GetMouseButtonUp(0))
                    {
                        _activeAim.Attack(_activeAim,parent.gameObject);
                    }
                }
            }
            else
            {
                TileManager.Instance.DisLightUnit();
                _activeAim?.UpdateCoord();
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

    private void HighlightUnit(Transform objectHit)
    {

        _targetUnit = objectHit.GetComponent<UnitMoveController>();

        if (_targetUnit.player == BattleSystem.Instance.curPlayer)
        {
            TileManager.Instance.LightUnit(_targetUnit);

            if (Input.GetMouseButtonUp(0))
            {
                TileManager.Instance.ChooseUnit(_targetUnit);
            }
        }
        //else 
        //{
        //    if (TileManager.Instance.activeUnit != null)
        //    {
        //        _activeAim = objectHit.GetComponentInChildren<BeAim>();
        //        _activeAim.LightAims();
        //    }
        //}
    }
}
