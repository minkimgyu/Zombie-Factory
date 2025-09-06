using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;

public interface IPathfinder
{
    List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos);
}

// A* 알고리즘을 사용한 지상 경로 탐색기
public class GroundPathfinder : IPathfinder
{
    GridComponent _gridComponent;

    // 최대 힙 크기
    const int maxSize = 1000;
    Heap<Node> _openList;
    HashSet<Node> _closedList;

    // 휴리스틱 가중치 -> 높을수록 휴리스틱의 영향력이 커짐 -> 목표 지점으로 더 공격적으로 향하도록 유도해 탐색 구간을 줄임
    // 그러나 출구가 하나 밖에 없는 미로와 같은 맵에서는 오히려 비효율적일 수 있음
    [SerializeField] float _weight = 1.0f;

    // 초기화 함수
    public GroundPathfinder(GridComponent gridComponent)
    {
        _gridComponent = gridComponent;
        _openList = new Heap<Node>(maxSize);
        _closedList = new HashSet<Node>();
    }

    // Node 스택을 Vector3 리스트로 변환하여 실제 경로 반환
    List<Vector3> ConvertToPath(Stack<Node> stackNode)
    {
        List<Vector3> points = new List<Vector3>();
        while (stackNode.Count > 0)
        {
            Node node = stackNode.Peek();
            points.Add(node.SurfacePos);
            stackNode.Pop();
        }

        return points;
    }

    // BFS를 통해 이동 가능한 가장 가까운 노드를 찾음
    Node FindAlternativeNode(Node node)
    {
        HashSet<Node> closeHash = new HashSet<Node>();
        Queue<Node> nodeQueue = new Queue<Node>();
        nodeQueue.Enqueue(node);

        while (nodeQueue.Count > 0)
        {
            Node frontNode = nodeQueue.Dequeue();

            List<Node> nearNodes = frontNode.NearNodes;
            for (int i = 0; i < nearNodes.Count; i++)
            {
                Node closeNode = nearNodes[i];

                bool nowHave = closeHash.Contains(nearNodes[i]);
                if (nowHave == true) continue;

                if (closeNode.CanStep == true) return closeNode;

                closeHash.Add(nearNodes[i]);
                nodeQueue.Enqueue(nearNodes[i]); // 가지고 있지 않다면 넣는다.
            }
        }

        return null;
    }

    // 대체 노드 반환 함수
    Node GetAlternativeNode(Node node)
    {
        Node alternativeNode = null;

        // 만약 노드가 CanStep이 false인 경우 대체 노드를 찾아서 적용해준다.
        if (node.CanStep == false)
        {
            Node startAlternativeNode;

            if (node.AlternativeNode != null && node.AlternativeNode.CanStep == true)
            {
                alternativeNode = node.AlternativeNode; // 만약 보유 중이라면 해당 노드로 대체
            }
            else
            {
                startAlternativeNode = FindAlternativeNode(node); // 대체 노드 찾기
                node.AlternativeNode = startAlternativeNode; // 대체 노드로 적용해주기
                alternativeNode = node.AlternativeNode; // 해당 노드로 바꿔주기
            }
        }
        else
        {
            alternativeNode = node;
        }

        return alternativeNode;
    }

    // 경로 탐색 함수
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // 리스트 초기화
        _openList.Clear();
        _closedList.Clear();

        // 가장 먼저 반올림을 통해 가장 가까운 노드를 찾는다.
        Vector3Int startIndex = _gridComponent.GetNodeIndex(startPos);
        Vector3Int endIndex = _gridComponent.GetNodeIndex(targetPos);

        // 인덱스에 맞는 노드 반환
        Node startNode = _gridComponent.ReturnNode(startIndex);
        Node endNode = _gridComponent.ReturnNode(endIndex);

        // 노드가 없는 경우 null 반환
        if (startNode == null || endNode == null) { return null; }

        // 만약 시작 노드나 끝 노드가 CanStep이 아닌 경우 대체 노드를 찾아서 적용해준다.
        if (startNode.CanStep == false) startNode = GetAlternativeNode(startNode);

        if (endNode.CanStep == false) endNode = GetAlternativeNode(endNode);

        if (startNode == null || endNode == null) { return null; }

        _openList.Insert(startNode);

        while (_openList.Count > 0)
        {
            Node targetNode = _openList.ReturnMin();
            if (targetNode == endNode) // 목적지와 타겟이 종료
            {
                Stack<Node> finalList = new Stack<Node>();

                Node TargetCurNode = targetNode;
                while (TargetCurNode != startNode)
                {
                    finalList.Push(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }

                return ConvertToPath(finalList);
            }

            _openList.DeleteMin(); // Min노드 제거
            _closedList.Add(targetNode); // closedList에 추가
            AddNearGridInList(targetNode, endNode.SurfacePos);
        }

        // 이 경우는 경로를 찾지 못한 상황임
        return null;
    }

    // 유클리드 거리 계산 함수
    float GetDistance(Vector3 nearNodeSurfacePos, Vector3 endSurfacePos)
    {
        return Vector3.Distance(nearNodeSurfacePos, endSurfacePos);
    }

    // 인접 노드를 오픈 리스트에 추가하는 함수
    void AddNearGridInList(Node targetNode, Vector3 endSurfacePos)
    {
        for (int i = 0; i < targetNode.NearNodesInGround.Count; i++)
        {
            // 이동할 수 없거나 닫힌 리스트에 있는 경우 다음 노드 탐색
            Node nearNode = targetNode.NearNodesInGround[i];
            if (nearNode.CanStep == false || _closedList.Contains(nearNode)) continue;

            float moveCost = GetDistance(targetNode.SurfacePos, nearNode.SurfacePos);
            moveCost += targetNode.G;

            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // 오픈 리스트에 있더라도 G값이 더 작다면 다시 설정해주기
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                nearNode.G = moveCost;
                nearNode.H = GetDistance(nearNode.SurfacePos, endSurfacePos) * _weight;
                nearNode.ParentNode = targetNode;
            }

            if (isOpenListContainNearGrid == false) _openList.Insert(nearNode);
        }
    }
}
