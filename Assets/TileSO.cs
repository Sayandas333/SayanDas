using UnityEditor;
using UnityEngine;
[ CreateAssetMenu()]
public class TileSO : ScriptableObject 
{
    public bool[,] Tile = new bool[10,10];
}
