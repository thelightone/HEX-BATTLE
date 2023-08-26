using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public Dictionary<Vector3Int, HexTile> tiles;

    [SerializeField]
    private GameObject highlight;
    [SerializeField]
    private GameObject select;
    [SerializeField]
    private GameObject busy;
    [SerializeField]
    private GameObject selectUnit;
    [SerializeField]
    private GameObject lightUnit;

    public UnitController activeUnit;


    private void Awake()
    {
        Instance = this;
        tiles = new Dictionary<Vector3Int, HexTile>();

        HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
        foreach (var hextile in hexTiles)
        {
            tiles.Add(hextile.cubeCoord, hextile);
        }

        foreach (var hextile in hexTiles)
        {
            List<HexTile> neighs = GetNeigh(hextile);
            hextile.neigh = neighs;
        }
    }

    private List <HexTile> GetNeigh (HexTile tile)
    {
        List<HexTile> neighs = new List<HexTile>();
        Vector3Int[] neighCoord = new Vector3Int[]
        {
            new Vector3Int (1,-1,0),
            new Vector3Int (1,0,-1),
            new Vector3Int (0, 1,-1),
            new Vector3Int (-1,1,0),
            new Vector3Int (-1,0,1),
            new Vector3Int (0, -1,1),
        };
        foreach (var coord in neighCoord)
        {
            Vector3Int tileCoord = tile.cubeCoord;
            if (tiles.TryGetValue(tileCoord+coord, out HexTile neighbour))
            {
                neighs.Add(neighbour);
            }
          
        }
        return neighs;        
    }

    public void Highlight(HexTile tile)
    {
        highlight.transform.GetChild(0).gameObject.SetActive(true);
        highlight.transform.GetChild(1).gameObject.SetActive(false);

        highlight.transform.position = tile.transform.position;
    }

    public void Busy(HexTile tile)
    {
        highlight.transform.GetChild(0).gameObject.SetActive(false);
        highlight.transform.GetChild(1).gameObject.SetActive(true);

        highlight.transform.position = tile.transform.position;
    }

    public void Select(HexTile tile)
    {
        if (!tile.busy)
        {
            select.SetActive(true);
            select.transform.position = tile.transform.position;
            activeUnit.Move(tile);
        }
    }

    public void DisSelect()
    {
        highlight.SetActive(false);    
        select.SetActive(false);
    }

    public void ChooseUnit(UnitController unit)
    {   if (activeUnit != null)
        {
            DisChooseUnit();
        }
        activeUnit = unit;
        activeUnit.choose.SetActive(true);
        highlight.SetActive(true);
        activeUnit.Range(activeUnit.currentTile);
    }

    public void DisChooseUnit()
    {
        activeUnit.choose.SetActive(false);
        activeUnit.UnRange();
        activeUnit = null;
    }

    public void LightUnit(UnitController unit)
    {
        if (unit != activeUnit)
        {
            lightUnit.SetActive(true);
            lightUnit.transform.position = unit.transform.position - new Vector3 (0,0.9f,0);
        }
    }
    public void DisLightUnit()
    {
            lightUnit.SetActive(false);
    }
}
