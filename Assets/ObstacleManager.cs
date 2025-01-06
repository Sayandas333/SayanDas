using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject tile_Obj;
     public Pathfinder PlayerPathfinder;
    [SerializeField] private EnemyAI ai;
     public GameObject Player;
     public Dictionary<Vector2Int , Tile> Tilelist;
    public TileSO tileSO;
    public Material BlockedMat;
    public Material Valid;
    private const int TILEX = 10;
    private const int TILEY = 10;
    public bool editor  = false;
    public static ObstacleManager Instance;
    private void Start()
    {
        Instance  = this;   
        GenarateTile();
        ai.pathfinder.Init();
    }
    private void Update()
    {
        Tile Hovertile = null;
        RaycastHit Hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                Hit.transform.gameObject.TryGetComponent(out Tile tile);
                if (tile != null)
                {
                    if (!PlayerPathfinder.isPath)
                    {
                        PlayerPathfinder.SetNewDestination(PlayerPathfinder.StartPos, tile.TilePos);
                        RecalculatePath(false, tile.TilePos);
                    }
                }
            }
            return;
        }
        Ray rayss = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayss, out Hit, Mathf.Infinity))
        {
            Hit.transform.gameObject.TryGetComponent(out Hovertile);
            if (Hovertile != null)
            {
                Hovertile.DisplayNo.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Tilelist.TryGetValue(new Vector2Int(j, i), out Tile s);
                if (s != Hovertile)
                {
                    s.DisplayNo.gameObject.SetActive(false);
                }
            }
        }
    }
    void RecalculatePath(bool resetPath , Vector2Int d )
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = PlayerPathfinder.StartPos;
        }
        else
        {
            coordinates = new Vector2Int((int)PlayerPathfinder.Move.transform.position.x,
                        (int)PlayerPathfinder.Move.transform.position.y);
            PlayerPathfinder.SetNewDestination(PlayerPathfinder.StartPos, d);
        }
        StopAllCoroutines();
        PlayerPathfinder.path.Clear();
        ResetNode();
        PlayerPathfinder.path = PlayerPathfinder.GetNewPath();
        StartCoroutine(PlayerPathfinder.PlayerMv(0));
    }
    public void ResetNode()
    {
        foreach (KeyValuePair<Vector2Int  , Tile> tile in Tilelist)
        {
            tile.Value.path = false;
            tile.Value.explored =  false;
            tile.Value.DisplayNo.gameObject.SetActive(false);
            tile.Value.ConnectedTile = null;
        }
    }
    private void GenarateTile()
    {
        Tilelist= new Dictionary<Vector2Int , Tile>(TILEX * TILEY);
        for (int Y = 0; Y < TILEY; Y++)
        {
            for (int X = 0; X < TILEY; X++)
            {
               GameObject SpwnTile =  Instantiate(tile_Obj, new Vector3(X,0,Y), Quaternion.identity);
               SpwnTile.name  = new Vector2(X, Y).ToString();
                SpwnTile.TryGetComponent(out Tile tile);
                tile.text.text = (X, Y).ToString();
                tile.TilePos = new Vector2Int(X, Y);
                Tilelist.Add(new Vector2Int(X, Y), tile);
                tile.blocked = tileSO.Tile[X, Y];
            }
        }
    }
}
