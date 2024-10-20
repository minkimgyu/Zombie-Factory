using System;
using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;

public class GridComponent : MonoBehaviour
{
    // 주변 노드를 반환하는 코드를 작성한다.

    // 대상에 따라서 빈 공간을 이동할 수 있는 드론
    // 막힌 공간의 표면을 따라 이동할 수 있는 좀비, 조력자로 나뉜다.
    // 모두 상하좌우 1칸씩 이동가능하게끔 해보자
    GridGenerator _gridGenerator;
    GroundPathfinder _groundPathfinder;

    [SerializeField] float _nodeSize = 0.5f;
    [SerializeField] Vector3Int _sizeOfGrid;
    [SerializeField] LayerMask _blockMask;
    [SerializeField] LayerMask _nonPassMask;

    [SerializeField] float _surfaceHeight = 0.2f;

    [SerializeField] Color _wireColor = new Color();
    [SerializeField] Color _passNodeColor = new Color();
    [SerializeField] Color _nonPassNodeColor = new Color();
    [SerializeField] Color _blockNodeColor = new Color();

    [SerializeField] Color _surfaceNodeColor;

    [SerializeField] bool _showRect;
    [SerializeField] bool _showNonPass;
    [SerializeField] bool _showBlockNode;
    [SerializeField] bool _showSurface;
    [SerializeField] bool _showNavigationRect;

    Node[,,] _grid;

    PathSeekerCaptureComponent _pathSeekerCaptureComponent;

    void OnEnter(IInjectPathfind injectPathfind)
    {
        injectPathfind.AddPathfind(_groundPathfinder.FindPath);
    }

    public void Initialize()
    {
        _pathSeekerCaptureComponent = GetComponentInChildren<PathSeekerCaptureComponent>();
        _pathSeekerCaptureComponent.Initialize(OnEnter);

        _gridGenerator = GetComponent<GridGenerator>();
        _groundPathfinder = GetComponent<GroundPathfinder>();

        _grid = _gridGenerator.CreateGrid(_nodeSize, _sizeOfGrid, _blockMask, _nonPassMask);
        for (int x = 0; x < _sizeOfGrid.x; x++)
        {
            for (int y = 0; y < _sizeOfGrid.y; y++)
            {
                for (int z = 0; z < _sizeOfGrid.z; z++)
                {
                    _grid[x, y, z].NearNodesInAir = ReturnNearNodesInAir(new Vector3Int(x, y, z));
                    _grid[x, y, z].NearNodesInGround = ReturnNearNodesInGround(new Vector3Int(x, y, z));
                    _grid[x, y, z].NearNodes = ReturnNearNodes(new Vector3Int(x, y, z));
                }
            }
        }

        _groundPathfinder.Initialize(this);
    }

    public List<Node> ReturnNearNodesInGround(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        // 주변 그리드
        List<Vector3Int> closeIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y - 1, index.z + 1), new Vector3Int(index.x, index.y - 1, index.z + 1), new Vector3Int(index.x + 1, index.y - 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y - 1, index.z), new Vector3Int(index.x + 1, index.y - 1, index.z),
            new Vector3Int(index.x - 1, index.y - 1, index.z - 1), new Vector3Int(index.x, index.y - 1, index.z - 1), new Vector3Int(index.x + 1, index.y - 1, index.z - 1),

            new Vector3Int(index.x - 1, index.y, index.z + 1), new Vector3Int(index.x, index.y, index.z + 1), new Vector3Int(index.x + 1, index.y, index.z + 1),
            new Vector3Int(index.x - 1, index.y, index.z), new Vector3Int(index.x + 1, index.y, index.z),
            new Vector3Int(index.x - 1, index.y, index.z - 1), new Vector3Int(index.x, index.y, index.z - 1), new Vector3Int(index.x + 1, index.y, index.z - 1),

            new Vector3Int(index.x - 1, index.y + 1, index.z + 1), new Vector3Int(index.x, index.y + 1, index.z + 1), new Vector3Int(index.x + 1, index.y + 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y + 1, index.z), new Vector3Int(index.x + 1, index.y + 1, index.z),
            new Vector3Int(index.x - 1, index.y + 1, index.z - 1), new Vector3Int(index.x, index.y + 1, index.z - 1), new Vector3Int(index.x + 1, index.y + 1, index.z - 1)
        };

        for (int i = 0; i < closeIndex.Count; i++)
        {
            bool isOutOfRange = closeIndex[i].x < 0 || closeIndex[i].z < 0 || closeIndex[i].y < 0 || closeIndex[i].x >= _sizeOfGrid.x || closeIndex[i].y >= _sizeOfGrid.y || closeIndex[i].z >= _sizeOfGrid.z;
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(closeIndex[i]);
            if(node.CurrentState != Node.State.Surface) continue;

            nearNodes.Add(node);
        }

        return nearNodes;
    }

    public List<Node> ReturnNearNodesInAir(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        // 주변 그리드
        List<Vector3Int> closeIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y - 1, index.z + 1), new Vector3Int(index.x, index.y - 1, index.z + 1), new Vector3Int(index.x + 1, index.y - 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y - 1, index.z), new Vector3Int(index.x, index.y - 1, index.z), new Vector3Int(index.x + 1, index.y - 1, index.z),
            new Vector3Int(index.x - 1, index.y - 1, index.z - 1), new Vector3Int(index.x, index.y - 1, index.z - 1), new Vector3Int(index.x + 1, index.y - 1, index.z - 1),

            new Vector3Int(index.x - 1, index.y, index.z + 1), new Vector3Int(index.x, index.y, index.z + 1), new Vector3Int(index.x + 1, index.y, index.z + 1),
            new Vector3Int(index.x - 1, index.y, index.z), new Vector3Int(index.x + 1, index.y, index.z),
            new Vector3Int(index.x - 1, index.y, index.z - 1), new Vector3Int(index.x, index.y, index.z - 1), new Vector3Int(index.x + 1, index.y, index.z - 1),

            new Vector3Int(index.x - 1, index.y + 1, index.z + 1), new Vector3Int(index.x, index.y + 1, index.z + 1), new Vector3Int(index.x + 1, index.y + 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y + 1, index.z), new Vector3Int(index.x, index.y + 1, index.z), new Vector3Int(index.x + 1, index.y + 1, index.z),
            new Vector3Int(index.x - 1, index.y + 1, index.z - 1), new Vector3Int(index.x, index.y + 1, index.z - 1), new Vector3Int(index.x + 1, index.y + 1, index.z - 1)
        };

        for (int i = 0; i < closeIndex.Count; i++)
        {
            bool isOutOfRange = closeIndex[i].x < 0 || closeIndex[i].y < 0 || closeIndex[i].z < 0 || closeIndex[i].x >= _sizeOfGrid.x || closeIndex[i].y >= _sizeOfGrid.y || closeIndex[i].z >= _sizeOfGrid.z;
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(closeIndex[i]);
            if (node.CurrentState != Node.State.Empty) continue;

            nearNodes.Add(node);
        }

        return nearNodes;
    }

    public List<Node> ReturnNearNodes(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        // 주변 그리드
        List<Vector3Int> closeIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y - 1, index.z + 1), new Vector3Int(index.x, index.y - 1, index.z + 1), new Vector3Int(index.x + 1, index.y - 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y - 1, index.z), new Vector3Int(index.x, index.y - 1, index.z), new Vector3Int(index.x + 1, index.y - 1, index.z),
            new Vector3Int(index.x - 1, index.y - 1, index.z - 1), new Vector3Int(index.x, index.y - 1, index.z - 1), new Vector3Int(index.x + 1, index.y - 1, index.z - 1),

            new Vector3Int(index.x - 1, index.y, index.z + 1), new Vector3Int(index.x, index.y, index.z + 1), new Vector3Int(index.x + 1, index.y, index.z + 1),
            new Vector3Int(index.x - 1, index.y, index.z), new Vector3Int(index.x + 1, index.y, index.z),
            new Vector3Int(index.x - 1, index.y, index.z - 1), new Vector3Int(index.x, index.y, index.z - 1), new Vector3Int(index.x + 1, index.y, index.z - 1),

            new Vector3Int(index.x - 1, index.y + 1, index.z + 1), new Vector3Int(index.x, index.y + 1, index.z + 1), new Vector3Int(index.x + 1, index.y + 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y + 1, index.z), new Vector3Int(index.x, index.y + 1, index.z), new Vector3Int(index.x + 1, index.y + 1, index.z),
            new Vector3Int(index.x - 1, index.y + 1, index.z - 1), new Vector3Int(index.x, index.y + 1, index.z - 1), new Vector3Int(index.x + 1, index.y + 1, index.z - 1)
        };

        for (int i = 0; i < closeIndex.Count; i++)
        {
            bool isOutOfRange = closeIndex[i].x < 0 || closeIndex[i].y < 0 || closeIndex[i].z < 0 || closeIndex[i].x >= _sizeOfGrid.x || closeIndex[i].y >= _sizeOfGrid.y || closeIndex[i].z >= _sizeOfGrid.z;
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(closeIndex[i]);
            nearNodes.Add(node);
        }

        return nearNodes;
    }

    public Node ReturnNode(Vector3Int index) { return _grid[index.x, index.y, index.z]; }
    public Node ReturnNode(int x, int y, int z) { return _grid[x, y, z]; }

    public Vector3 ReturnClampedRange(Vector3 pos)
    {
        Vector3 bottomLeftPos = ReturnNode(Vector3Int.zero).Pos;
        Vector3 topRightPos = ReturnNode(_sizeOfGrid.x - 1, _sizeOfGrid.y - 1, _sizeOfGrid.z - 1).Pos; // --> 실질적 위치는 노드의 크기를 곱해줘야 한다.

        // 반올림하고 범위 안에 맞춰줌
        // 이 부분은 GridSize 바뀌면 수정해야함
        float xPos = Mathf.Clamp(pos.x, bottomLeftPos.x, topRightPos.x);
        float yPos = Mathf.Clamp(pos.y, bottomLeftPos.y, topRightPos.y);
        float zPos = Mathf.Clamp(pos.z, bottomLeftPos.z, topRightPos.z);

        return new Vector3(xPos, yPos, zPos);
    }

    public Vector3 ReturnNodePos(Vector3 worldPos)
    {
        Vector3Int index = ReturnNodeIndex(worldPos);
        return ReturnNode(index).Pos;
    }

    public Vector3Int ReturnNodeIndex(Vector3 worldPos)
    {
        Vector3 clampedPos = ReturnClampedRange(worldPos);
        Vector3 bottomLeftPos = ReturnNode(Vector3Int.zero).Pos;

        float xRelativePos = (clampedPos.x - bottomLeftPos.x) / _nodeSize;
        float yRelativePos = (clampedPos.y - bottomLeftPos.y) / _nodeSize;
        float zRelativePos = (clampedPos.z - bottomLeftPos.z) / _nodeSize;

        int xIndex = (int)Mathf.Clamp(xRelativePos, 0, _sizeOfGrid.x - 1);
        int yIndex = (int)Mathf.Clamp(yRelativePos, 0, _sizeOfGrid.y - 1);
        int zIndex = (int)Mathf.Clamp(zRelativePos, 0, _sizeOfGrid.z - 1);

        return new Vector3Int(xIndex, yIndex, zIndex);
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
            for (int y = 0; y < _sizeOfGrid.y; y++)
            {
                for (int z = 0; z < _sizeOfGrid.z; z++)
                {
                    if (_showNavigationRect)
                    {
                        Vector3 originPos = transform.position + new Vector3(_nodeSize / 2, _nodeSize / 2, _nodeSize / 2);
                        DrawGizmoCube(originPos + new Vector3(x, y, z) * _nodeSize, _wireColor, _nodeSize, true);
                    }

                    if (_grid == null) continue;
                    Node node = _grid[x, y, z];

                    if (_showNonPass && node.CurrentState == Node.State.NonPass)
                    {
                        DrawGizmoCube(node.Pos, _nonPassNodeColor, _nodeSize);
                        continue;
                    }

                    if (_showBlockNode && _grid != null)
                    {
                        if (node.CurrentState == Node.State.Empty)
                        {
                            DrawGizmoCube(node.Pos, _passNodeColor, _nodeSize);
                        }
                        else
                        {
                            if (_showSurface == true)
                            {
                                DrawGizmoCube(node.SurfacePos, _surfaceNodeColor, new Vector3(_nodeSize, _nodeSize * _surfaceHeight, _nodeSize));
                            }
                            DrawGizmoCube(node.Pos, _blockNodeColor, _nodeSize);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawGrid();
    }
}
