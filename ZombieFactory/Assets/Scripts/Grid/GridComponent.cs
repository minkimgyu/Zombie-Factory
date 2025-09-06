using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;


public class GridComponent : MonoBehaviour
{
    GridGenerator _gridGenerator;
    GroundPathfinder _groundPathfinder;
    GridDrawer _gridDrawer;

    [SerializeField] float _nodeSize = 0.5f;
    [SerializeField] Vector3Int _sizeOfGrid;
    [SerializeField] LayerMask _blockMask;
    [SerializeField] LayerMask _nonPassMask;

    Node[,,] _grid;

    PathSeekerCaptureComponent _pathSeekerCaptureComponent;

    // 그리드 생성
    public void CreateGrid()
    {
        _grid = _gridGenerator.CreateGrid(transform.position, _nodeSize, _sizeOfGrid, _blockMask, _nonPassMask);
    }

    // 주변 노드 초기화
    public void InitializeNodes()
    {
        for (int x = 0; x < _sizeOfGrid.x; x++)
        {
            for (int y = 0; y < _sizeOfGrid.y; y++)
            {
                for (int z = 0; z < _sizeOfGrid.z; z++)
                {
                    _grid[x, y, z].NearNodesInGround = GetNearNodesInGround(new Vector3Int(x, y, z));
                    _grid[x, y, z].NearNodes = ReturnNearNodes(new Vector3Int(x, y, z));
                }
            }
        }
    }

    // pathfinder에 FindPath 함수 등록
    void OnEnterGrid(IPathSeeker injectPathfind)
    {
        injectPathfind.InjectPathfinder(_groundPathfinder);
    }

    // 초기화
    public void Initialize()
    {
        _pathSeekerCaptureComponent = GetComponentInChildren<PathSeekerCaptureComponent>();
        _pathSeekerCaptureComponent.Initialize(OnEnterGrid);

        _gridGenerator = new GridGenerator();
        _groundPathfinder = new GroundPathfinder(this);

        _gridDrawer = GetComponent<GridDrawer>();
        _gridDrawer.Initialize(_grid, _nodeSize, _sizeOfGrid);
    }

    // y축 높낮이 차이가 있는 경우 -> 바로 아래, 바로 위는 없음 (이동 불가능)
    List<Vector3Int> _differentYNearPoints = new List<Vector3Int> 
    {
        new Vector3Int(-1, -1,  1), new Vector3Int(0, -1,  1), new Vector3Int(1, -1,  1),
        new Vector3Int(-1, -1, 0), new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1,  -1), new Vector3Int(0, -1,  - 1), new Vector3Int(1, -1,  -1),

        new Vector3Int(-1, 1,  1), new Vector3Int(0, 1, 1), new Vector3Int(1, 1,  1),
        new Vector3Int(-1, 1, 0), new Vector3Int(1, 1, 0),
        new Vector3Int(-1, 1,  -1), new Vector3Int(0, 1,  -1), new Vector3Int(1, 1,  -1)
    };

    // y축 높이가 같은 경우 수직, 수평 이동
    List<Vector3Int> _sameYDiagonalPoints = new List<Vector3Int> 
    {
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1), new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 0),
    };

    // y축 높이가 같은 경우 대각선 이동
    List<Vector3Int> _sameYCrossPoints = new List<Vector3Int> 
    {
        new Vector3Int(-1, 0, -1), new Vector3Int(- 1, 0, 1),
        new Vector3Int(1, 0, -1), new Vector3Int(1, 0, 1),
    };

    // 주변 노드 반환 - 지상용
    public List<Node> GetNearNodesInGround(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        Node currentNode = ReturnNode(index);
        if (currentNode.CurrentState != Node.State.Block) return nearNodes;

        for (int i = 0; i < _differentYNearPoints.Count; i++)
        {
            Vector3Int nearPos = index + _differentYNearPoints[i];

            bool isOutOfRange = IsOutOfRange(nearPos);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(nearPos);
            if (node.CurrentState != Node.State.Block) continue;

            nearNodes.Add(node);
        }


        // y축 높이가 같고 주변 그리드 ↑ ↓ ← → 의 경우
        //       (0)
        //        ↑ 
        // (1) ←  ※ → (2)
        //        ↓ 
        //       (3)

        for (int i = 0; i < _sameYDiagonalPoints.Count; i++)
        {
            Vector3Int diagonalPos = index + _sameYDiagonalPoints[i];

            bool isOutOfRange = IsOutOfRange(diagonalPos);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(diagonalPos);
            if(node.CurrentState != Node.State.Block) continue;

            nearNodes.Add(node);
        }

        // y축 높이가 같고 주변 그리드 ↗ ↘ ↙ ↖ 의 경우
        // (0)      (1)
        //   ↖    ↗
        //      ※
        //   ↙    ↘ 
        // (2)      (3)

        for (int i = 0; i < _sameYCrossPoints.Count; i++)
        {
            Vector3Int crossPos = index + _sameYCrossPoints[i];

            bool isOutOfRange = IsOutOfRange(crossPos);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(crossPos);
            if (node.CurrentState != Node.State.Block) continue;

            // 갈 수 있는 코너인지 체크
            Node node1, node2;
            switch (i)
            {
                case 0:
                    if(IsOutOfRange(index + _sameYCrossPoints[0]) == true || IsOutOfRange(index + _sameYCrossPoints[1]) == true) continue;

                    node1 = ReturnNode(index + _sameYCrossPoints[0]);
                    node2 = ReturnNode(index + _sameYCrossPoints[1]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 1:
                    if (IsOutOfRange(index + _sameYCrossPoints[0]) == true || IsOutOfRange(index + _sameYCrossPoints[2]) == true) continue;

                    node1 = ReturnNode(index + _sameYCrossPoints[0]);
                    node2 = ReturnNode(index + _sameYCrossPoints[2]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 2:
                    if (IsOutOfRange(index + _sameYCrossPoints[1]) == true || IsOutOfRange(index + _sameYCrossPoints[3]) == true) continue;

                    node1 = ReturnNode(index + _sameYCrossPoints[1]);
                    node2 = ReturnNode(index + _sameYCrossPoints[3]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
                case 3:
                    if (IsOutOfRange(index + _sameYCrossPoints[2]) == true || IsOutOfRange(index + _sameYCrossPoints[3]) == true) continue;

                    node1 = ReturnNode(index + _sameYCrossPoints[2]);
                    node2 = ReturnNode(index + _sameYCrossPoints[3]);
                    if (node1.CanStep == false || node2.CanStep == false) continue;
                    break;
            }

            nearNodes.Add(ReturnNode(crossPos));
        }

        return nearNodes;
    }

    // 주변 노드 인덱스
    List<Vector3Int> _nearPoints = new List<Vector3Int>
    {
        new Vector3Int(-1, -1, 1), new Vector3Int(0, -1, 1), new Vector3Int(1, -1, 1),
        new Vector3Int(-1, -1, 0), new Vector3Int(0, -1, 0), new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1, -1), new Vector3Int(0, -1, -1), new Vector3Int(1, -1, -1),

        new Vector3Int(-1, 0, 1), new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 1),
        new Vector3Int(-1, 0, 0), new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, -1), new Vector3Int(0, 0, -1), new Vector3Int(1, 0, -1),

        new Vector3Int(-1, 1, 1), new Vector3Int(0, 1, 1), new Vector3Int(1, 1, 1),
        new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, 0), new Vector3Int(1, 1, 0),
        new Vector3Int(-1, 1, -1), new Vector3Int(0, 1, -1), new Vector3Int(1, 1, -1)
    };


    // 주변 노드 반환
    public List<Node> ReturnNearNodes(Vector3Int index)
    {
        List<Node> nearNodes = new List<Node>();

        for (int i = 0; i < _nearPoints.Count; i++)
        {
            Vector3Int nearPos = index + _nearPoints[i];

            bool isOutOfRange = IsOutOfRange(nearPos);
            if (isOutOfRange == true) continue;

            Node node = ReturnNode(nearPos);
            nearNodes.Add(node);
        }

        return nearNodes;
    }

    // 그리드 범위 체크
    bool IsOutOfRange(Vector3Int index)
    {
        bool isOutOfRange = index.x < 0 || index.y < 0 || index.z < 0 || index.x >= _sizeOfGrid.x || index.y >= _sizeOfGrid.y || index.z >= _sizeOfGrid.z;
        if (isOutOfRange == true) return true;

        return false;
    }

    // 인덱스로 노드 반환
    public Node ReturnNode(Vector3Int index) { return _grid[index.x, index.y, index.z]; }

    // 월드 좌표를 그리드 범위 안에 맞춰서 반환
    public Vector3 GetClampedRange(Vector3 pos)
    {
        Vector3 bottomLeftPos = ReturnNode(Vector3Int.zero).Pos;
        Vector3 topRightPos = ReturnNode(_sizeOfGrid - Vector3Int.one).Pos;

        // 반올림하고 범위 안에 맞춰줌
        float xPos = Mathf.Clamp(pos.x, bottomLeftPos.x, topRightPos.x);
        float yPos = Mathf.Clamp(pos.y, bottomLeftPos.y, topRightPos.y);
        float zPos = Mathf.Clamp(pos.z, bottomLeftPos.z, topRightPos.z);

        return new Vector3(xPos, yPos, zPos);
    }

    // 노드 인덱스 반환
    public Vector3Int GetNodeIndex(Vector3 worldPos)
    {
        Vector3 clampedPos = GetClampedRange(worldPos);
        Vector3 bottomLeftPos = ReturnNode(Vector3Int.zero).Pos;

        float xRelativePos = (clampedPos.x - bottomLeftPos.x) / _nodeSize;
        float yRelativePos = (clampedPos.y - bottomLeftPos.y) / _nodeSize;
        float zRelativePos = (clampedPos.z - bottomLeftPos.z) / _nodeSize;

        int xIndex = (int)Mathf.Clamp(xRelativePos, 0, _sizeOfGrid.x - 1);
        int yIndex = (int)Mathf.Clamp(yRelativePos, 0, _sizeOfGrid.y - 1);
        int zIndex = (int)Mathf.Clamp(zRelativePos, 0, _sizeOfGrid.z - 1);

        return new Vector3Int(xIndex, yIndex, zIndex);
    }
}
