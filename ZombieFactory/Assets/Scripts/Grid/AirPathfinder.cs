//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Node = Pathfinding.Node;

//public class AirPathfinder : MonoBehaviour
//{
//    GridComponent _gridComponent;
//    const int maxSize = 1000;

//    Heap<Node> _openList = new Heap<Node>(maxSize);
//    HashSet<Node> _closedList = new HashSet<Node>();

//    Vector3 _startNodePos;
//    Vector3 _endNodePos;

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawCube(_startNodePos, Vector3.one * 0.5f);
//        Gizmos.DrawCube(_endNodePos, Vector3.one * 0.5f);
//    }

//    public void Initialize(GridComponent gridComponent)
//    {
//        _gridComponent = gridComponent;
//    }

//    List<Vector3> ConvertNodeToV3(Stack<Node> stackNode)
//    {
//        List<Vector3> points = new List<Vector3>();
//        while (stackNode.Count > 0)
//        {
//            Node node = stackNode.Peek();
//            points.Add(node.Pos);
//            stackNode.Pop();
//        }

//        return points;
//    }

//    bool HaveAlternativeNode(Node node, out Node alternativeNode)
//    {
//        // 이미 찾아놓은 노드가 존재한다면
//        if (node.AlternativeNode != null && node.AlternativeNode.CurrentState == Node.State.Empty)
//        {
//            alternativeNode = node.AlternativeNode;
//            return true;
//        }

//        // 아니라면 false 반환
//        alternativeNode = null;
//        return false;
//    }

//    Node FindAlternativeNode(Node node)
//    {
//        HashSet<Node> closeHash = new HashSet<Node>();
//        Queue<Node> nodeQueue = new Queue<Node>();
//        nodeQueue.Enqueue(node);

//        while (nodeQueue.Count > 0)
//        {
//            Node frontNode = nodeQueue.Dequeue();
//            List<Node> nearNodes = frontNode.NearNodes;
//            for (int i = 0; i < nearNodes.Count; i++)
//            {
//                Node closeNode = nearNodes[i];

//                bool nowHave = closeHash.Contains(nearNodes[i]);
//                if (nowHave == true) continue;

//                if(closeNode.CurrentState == Node.State.Empty) return closeNode;

//                closeHash.Add(nearNodes[i]);
//                nodeQueue.Enqueue(nearNodes[i]); // 가지고 있지 않다면 넣는다.
//            }
//        }

//        return null;
//    }

//    // BFS를 통해 이동 가능한 가장 가까운 노드를 찾고 다음 루프부터는 문제가 생기면 해당 노드를 반환해준다.

//    // 가장 먼저 반올림을 통해 가장 가까운 노드를 찾는다.
//    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
//    {
//        //// 리스트 초기화
//        _openList.Clear();
//        _closedList.Clear();

//        Vector3Int startIndex = _gridComponent.ReturnNodeIndex(startPos);
//        Vector3Int endIndex = _gridComponent.ReturnNodeIndex(targetPos);

//        Node startNode = _gridComponent.ReturnNode(startIndex);
//        Node endNode = _gridComponent.ReturnNode(endIndex);

//        if (startNode == null || endNode == null) { return null; }

//        if (startNode.CurrentState != Node.State.Empty)
//        {
//            Node startAlternativeNode;

//            if (HaveAlternativeNode(startNode, out startAlternativeNode))
//            {
//                startNode = startAlternativeNode; // 만약 보유 중이라면 해당 노드로 대체
//            }
//            else
//            {
//                startAlternativeNode = FindAlternativeNode(startNode);
//                startNode.AlternativeNode = startAlternativeNode; // 대체 노드로 적용해주기
//                startNode = startNode.AlternativeNode; // 해당 노드로 바꿔주기
//            }
//        }

//        if (endNode.CurrentState != Node.State.Empty)
//        {
//            Node endAlternativeNode;

//            if (HaveAlternativeNode(endNode, out endAlternativeNode))
//            {
//                endNode = endAlternativeNode; // 만약 보유 중이라면 해당 노드로 대체
//            }
//            else
//            {
//                endAlternativeNode = FindAlternativeNode(endNode);
//                endNode.AlternativeNode = endAlternativeNode; // 대체 노드로 적용해주기
//                endNode = endNode.AlternativeNode; // 해당 노드로 바꿔주기
//            }
//        }

//        if (startNode == null || endNode == null) { return null; }

//        _startNodePos = startNode.Pos;
//        _endNodePos = endNode.Pos;

//        _openList.Insert(startNode);

//        while (_openList.Count > 0)
//        {
//            Node targetNode = _openList.ReturnMin();
//            if (targetNode == endNode) // 목적지와 타겟이 같으면 끝
//            {
//                Stack<Node> finalList = new Stack<Node>();

//                Node TargetCurNode = targetNode;
//                while (TargetCurNode != startNode)
//                {
//                    finalList.Push(TargetCurNode);
//                    TargetCurNode = TargetCurNode.ParentNode;
//                }
//                //finalList.Push(startNode);

//                return ConvertNodeToV3(finalList);
//            }

//            _openList.DeleteMin(); // 해당 그리드 지워줌
//            _closedList.Add(targetNode); // 해당 그리드 추가해줌
//            AddNearGridInList(targetNode, endNode.Pos);
//        }

//        // 이 경우는 경로를 찾지 못한 상황임
//        return null;
//    }

//    void AddNearGridInList(Node targetNode, Vector3 targetGridPos)
//    {
//        for (int i = 0; i < targetNode.NearNodesInAir.Count; i++)
//        {
//            Node nearNode = targetNode.NearNodesInAir[i];
//            if (_closedList.Contains(nearNode)) continue; // 통과하지 못하거나 닫힌 리스트에 있는 경우 다음 그리드 탐색

//                // 공중에 있는 경우는 Pos, 땅에 있는 경우는 SurfacePos로 처리한다.
//            float moveCost = Vector3.Distance(targetNode.Pos, nearNode.Pos);
//            // 이 부분 중요! --> 거리를 측정해서 업데이트 하지 않고 계속 더해주는 방식으로 진행해야함
//            moveCost += targetNode.G;

//            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

//            // 오픈 리스트에 있더라도 G 값이 변경된다면 다시 리셋해주기
//            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
//            {
//                // 여기서 grid 값 할당 필요
//                nearNode.G = moveCost;
//                nearNode.H = Vector3.Distance(nearNode.Pos, targetGridPos);
//                nearNode.ParentNode = targetNode;
//            }

//            if (isOpenListContainNearGrid == false) _openList.Insert(nearNode);
//        }
//    }
//}