using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int TilePos;
    public GameObject DisplayNo;
    public TMP_Text text;
    public MeshRenderer RenderObj;
    public bool blocked;
    public bool explored;
    public bool path;
    public Tile ConnectedTile;
}
