using UnityEngine;
using Node = Pathfinding.Node;

public class GridDrawer : MonoBehaviour
{
    Node[,,] _grid;
    float _nodeSize = 0.5f;
    Vector3Int _sizeOfGrid;

    [SerializeField] float _surfaceHeight = 0.2f;

    [SerializeField] Color _wireColor;
    [SerializeField] Color _passNodeColor;
    [SerializeField] Color _nonPassNodeColor;
    [SerializeField] Color _blockNodeColor;
    [SerializeField] Color _surfaceNodeColor;

    [SerializeField] bool _showRect;
    [SerializeField] bool _showNonPass;
    [SerializeField] bool _showBlockNode;
    [SerializeField] bool _showSurface;
    [SerializeField] bool _showNavigationRect;

    public void Initialize(Node[,,] grid, float nodeSize, Vector3Int sizeOfGrid)
    {
        _grid = grid;
        _nodeSize = nodeSize;
        _sizeOfGrid = sizeOfGrid;
    }

    private void OnDrawGizmos()
    {
        DrawGrid();
    }

    void DrawGizmoCube(Vector3 pos, Color color, Vector3 size)
    {
        Gizmos.color = color;
        Gizmos.DrawCube(pos, size);
    }

    void DrawGizmoCube(Vector3 pos, Color color, float size, bool isWire = false)
    {
        Gizmos.color = color;

        if (isWire) Gizmos.DrawWireCube(pos, new Vector3(size, size, size));
        else Gizmos.DrawCube(pos, new Vector3(size, size, size));
    }

    void DrawGrid()
    {
        if (_showRect == false) return;

        for (int x = 0; x < _sizeOfGrid.x; x++)
        {
            for (int z = 0; z < _sizeOfGrid.z; z++)
            {
                for (int y = 0; y < _sizeOfGrid.y; y++)
                {
                    if (_showNavigationRect)
                    {
                        Vector3 originPos = transform.position + new Vector3(_nodeSize / 2, _nodeSize / 2, _nodeSize / 2);
                        DrawGizmoCube(originPos + new Vector3(x, y, z) * _nodeSize, _wireColor, _nodeSize);
                    }

                    if (_grid == null) continue;

                    Node node = _grid[x, y, z];

                    if (node.CurrentState == Node.State.Empty)
                    {
                        DrawGizmoCube(node.Pos, _passNodeColor, _nodeSize);
                        continue;
                    }

                    if (_showNonPass && node.CurrentState == Node.State.NonPass)
                    {
                        DrawGizmoCube(node.Pos, _nonPassNodeColor, _nodeSize);
                        continue;
                    }

                    if (_showBlockNode && node.CurrentState == Node.State.Block)
                    {
                        DrawGizmoCube(node.Pos, _blockNodeColor, _nodeSize);
                    }

                    if (_showSurface == true && node.CanStep == true)
                    {
                        DrawGizmoCube(node.SurfacePos, _surfaceNodeColor, new Vector3(_nodeSize, _nodeSize * _surfaceHeight, _nodeSize));
                    }
                }
            }
        }
    }
}
