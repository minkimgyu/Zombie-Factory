using System;
using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;
using System.Diagnostics;

public class GridComponent : MonoBehaviour
{
    // �ֺ� ��带 ��ȯ�ϴ� �ڵ带 �ۼ��Ѵ�.

    // ��� ���� �� ������ �̵��� �� �ִ� ���
    // ���� ������ ǥ���� ���� �̵��� �� �ִ� ����, �����ڷ� ������.
    // ��� �����¿� 1ĭ�� �̵������ϰԲ� �غ���
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

    public void CreateGrid()
    {
        _grid = _gridGenerator.CreateGrid(_nodeSize, _sizeOfGrid, _blockMask, _nonPassMask);
    }

    public void InitializeNodes()
    {
        //Stopwatch stopwatch = new Stopwatch();
        //// �ð� ���� ����
        //stopwatch.Start();

        for (int x = 0; x < _sizeOfGrid.x; x++)
        {
            for (int y = 0; y < _sizeOfGrid.y; y++)
            {
                for (int z = 0; z < _sizeOfGrid.z; z++)
                {
                    _grid[x, y, z].NearNodesInGround = ReturnNearNodesInGround(new Vector3Int(x, y, z));
                    _grid[x, y, z].NearNodes = ReturnNearNodes(new Vector3Int(x, y, z));
                }
            }
        }

        //// �ð� ���� ����
        //stopwatch.Stop();

        //// �ɸ� �ð� ���
        //UnityEngine.Debug.Log($"�ڵ� ���� �ð�: {stopwatch.ElapsedMilliseconds} ms");
    }

    public void Initialize()
    {
        _pathSeekerCaptureComponent = GetComponentInChildren<PathSeekerCaptureComponent>();
        _pathSeekerCaptureComponent.Initialize(OnEnter);

        _gridGenerator = GetComponent<GridGenerator>();
        _groundPathfinder = GetComponent<GroundPathfinder>();
        _groundPathfinder.Initialize(this);
    }

    public List<Node> ReturnNearNodesInGround(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        // y�� ������ ���̰� �ִ� ���
        List<Vector3Int> closeIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y - 1, index.z + 1), new Vector3Int(index.x, index.y - 1, index.z + 1), new Vector3Int(index.x + 1, index.y - 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y - 1, index.z), new Vector3Int(index.x + 1, index.y - 1, index.z),
            new Vector3Int(index.x - 1, index.y - 1, index.z - 1), new Vector3Int(index.x, index.y - 1, index.z - 1), new Vector3Int(index.x + 1, index.y - 1, index.z - 1),

            new Vector3Int(index.x - 1, index.y + 1, index.z + 1), new Vector3Int(index.x, index.y + 1, index.z + 1), new Vector3Int(index.x + 1, index.y + 1, index.z + 1),
            new Vector3Int(index.x - 1, index.y + 1, index.z), new Vector3Int(index.x + 1, index.y + 1, index.z),
            new Vector3Int(index.x - 1, index.y + 1, index.z - 1), new Vector3Int(index.x, index.y + 1, index.z - 1), new Vector3Int(index.x + 1, index.y + 1, index.z - 1)
        };

        for (int i = 0; i < closeIndex.Count; i++)
        {
            bool isOutOfRange = IsOutOfRange(closeIndex[i]);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(closeIndex[i]);
            if (node.CurrentState != Node.State.Block) continue;

            nearNodes.Add(node);
        }


        // y�� ���̰� ���� �ֺ� �׸��� �� �� �� �� �� ���
        //       (0)
        //        �� 
        // (1) ��  �� �� (2)
        //        �� 
        //       (3)

        //Tuple<Vector3Int, bool>

        List<Vector3Int> nearIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y, index.z),
            new Vector3Int(index.x, index.y, index.z - 1), new Vector3Int(index.x, index.y, index.z + 1),
            new Vector3Int(index.x + 1, index.y, index.z),
        };

        for (int i = 0; i < nearIndex.Count; i++)
        {
            bool isOutOfRange = IsOutOfRange(nearIndex[i]);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(nearIndex[i]);
            if(node.CurrentState != Node.State.Block) continue;

            nearNodes.Add(node);
        }


        // y�� ���̰� ���� �ֺ� �׸��� �� �� �� �� �� ���
        // (0)      (1)
        //   ��    ��
        //      ��
        //   ��    �� 
        // (2)      (3)

        List<Vector3Int> crossIndex = new List<Vector3Int> {
            new Vector3Int(index.x - 1, index.y, index.z - 1), new Vector3Int(index.x - 1, index.y, index.z + 1),
            new Vector3Int(index.x + 1, index.y, index.z - 1), new Vector3Int(index.x + 1, index.y, index.z + 1),
        };

        for (int i = 0; i < crossIndex.Count; i++)
        {
            bool isOutOfRange = IsOutOfRange(crossIndex[i]);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(crossIndex[i]);
            if (node.CurrentState != Node.State.Block) continue;

            // �� �� �ִ� �ڳ����� üũ
            Node node1, node2;
            switch (i)
            {
                case 0:
                    if(IsOutOfRange(nearIndex[0]) == true || IsOutOfRange(nearIndex[1]) == true) continue;

                    node1 = ReturnNode(nearIndex[0]);
                    node2 = ReturnNode(nearIndex[1]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 1:
                    if (IsOutOfRange(nearIndex[0]) == true || IsOutOfRange(nearIndex[2]) == true) continue;

                    node1 = ReturnNode(nearIndex[0]);
                    node2 = ReturnNode(nearIndex[2]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 2:
                    if (IsOutOfRange(nearIndex[1]) == true || IsOutOfRange(nearIndex[3]) == true) continue;

                    node1 = ReturnNode(nearIndex[1]);
                    node2 = ReturnNode(nearIndex[3]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 3:
                    if (IsOutOfRange(nearIndex[2]) == true || IsOutOfRange(nearIndex[3]) == true) continue;

                    node1 = ReturnNode(nearIndex[2]);
                    node2 = ReturnNode(nearIndex[3]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
            }

            nearNodes.Add(ReturnNode(crossIndex[i]));
        }

        return nearNodes;
    }

    public List<Node> ReturnNearNodes(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        // �ֺ� �׸���
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
            bool isOutOfRange = IsOutOfRange(closeIndex[i]);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(closeIndex[i]);
            nearNodes.Add(node);
        }

        return nearNodes;
    }

    bool IsOutOfRange(Vector3Int index)
    {
        bool isOutOfRange = index.x < 0 || index.y < 0 || index.z < 0 || index.x >= _sizeOfGrid.x || index.y >= _sizeOfGrid.y || index.z >= _sizeOfGrid.z;
        if (isOutOfRange == true) return true;

        return false;
    }


    public Node ReturnNode(Vector3Int index) { return _grid[index.x, index.y, index.z]; }
    public Node ReturnNode(int x, int y, int z) { return _grid[x, y, z]; }

    public Vector3 ReturnClampedRange(Vector3 pos)
    {
        Vector3 bottomLeftPos = ReturnNode(Vector3Int.zero).Pos;
        Vector3 topRightPos = ReturnNode(_sizeOfGrid.x - 1, _sizeOfGrid.y - 1, _sizeOfGrid.z - 1).Pos; // --> ������ ��ġ�� ����� ũ�⸦ ������� �Ѵ�.

        // �ݿø��ϰ� ���� �ȿ� ������
        // �� �κ��� GridSize �ٲ�� �����ؾ���
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

    private void OnDrawGizmos()
    {
        DrawGrid();
    }
}
