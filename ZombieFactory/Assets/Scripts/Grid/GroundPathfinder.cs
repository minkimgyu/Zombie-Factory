using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;

public class GroundPathfinder : MonoBehaviour
{
    GridComponent _gridComponent;
    const int maxSize = 1000;

    Heap<Node> _openList = new Heap<Node>(maxSize);
    HashSet<Node> _closedList = new HashSet<Node>();

    Vector3 _startNodePos;
    Vector3 _endNodePos;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(_startNodePos, Vector3.one * 0.5f);
        Gizmos.DrawCube(_endNodePos, Vector3.one * 0.5f);
    }

    public void Initialize(GridComponent gridComponent)
    {
        _gridComponent = gridComponent;
    }

    List<Vector3> ConvertNodeToV3(Stack<Node> stackNode)
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

    bool HaveAlternativeNode(Node node, out Node alternativeNode)
    {
        // �̹� ã�Ƴ��� ��尡 �����Ѵٸ�
        if (node.AlternativeNode != null && node.AlternativeNode.CanStep == true)
        {
            alternativeNode = node.AlternativeNode;
            return true;
        }

        // �ƴ϶�� false ��ȯ
        alternativeNode = null;
        return false;
    }

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
                nodeQueue.Enqueue(nearNodes[i]); // ������ ���� �ʴٸ� �ִ´�.
            }
        }

        return null;
    }

    // BFS�� ���� �̵� ������ ���� ����� ��带 ã�� ���� �������ʹ� ������ ����� �ش� ��带 ��ȯ���ش�.

    // ���� ���� �ݿø��� ���� ���� ����� ��带 ã�´�.
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //// ����Ʈ �ʱ�ȭ
        _openList.Clear();
        _closedList.Clear();

        Vector3Int startIndex = _gridComponent.ReturnNodeIndex(startPos);
        Vector3Int endIndex = _gridComponent.ReturnNodeIndex(targetPos);

        Node startNode = _gridComponent.ReturnNode(startIndex);
        Node endNode = _gridComponent.ReturnNode(endIndex);

        if (startNode == null || endNode == null) { return null; }

        if (startNode.CanStep == false)
        {
            Node startAlternativeNode;

            if (HaveAlternativeNode(startNode, out startAlternativeNode))
            {
                startNode = startAlternativeNode; // ���� ���� ���̶�� �ش� ���� ��ü
            }
            else
            {
                startAlternativeNode = FindAlternativeNode(startNode);
                startNode.AlternativeNode = startAlternativeNode; // ��ü ���� �������ֱ�
                startNode = startNode.AlternativeNode; // �ش� ���� �ٲ��ֱ�
            }
        }

        if (endNode.CanStep == false)
        {
            Node endAlternativeNode;

            if (HaveAlternativeNode(endNode, out endAlternativeNode))
            {
                endNode = endAlternativeNode; // ���� ���� ���̶�� �ش� ���� ��ü
            }
            else
            {
                endAlternativeNode = FindAlternativeNode(endNode);
                endNode.AlternativeNode = endAlternativeNode; // ��ü ���� �������ֱ�
                endNode = endNode.AlternativeNode; // �ش� ���� �ٲ��ֱ�
            }
        }

        if (startNode == null || endNode == null) { return null; }

        _startNodePos = startNode.SurfacePos;
        _endNodePos = endNode.SurfacePos;

        _openList.Insert(startNode);

        while (_openList.Count > 0)
        {
            Node targetNode = _openList.ReturnMin();
            if (targetNode == endNode) // �������� Ÿ���� ������ ��
            {
                Stack<Node> finalList = new Stack<Node>();

                Node TargetCurNode = targetNode;
                while (TargetCurNode != startNode)
                {
                    finalList.Push(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                //finalList.Push(startNode);

                return ConvertNodeToV3(finalList);
            }

            _openList.DeleteMin(); // �ش� �׸��� ������
            _closedList.Add(targetNode); // �ش� �׸��� �߰�����
            AddNearGridInList(targetNode, endNode.SurfacePos);
        }

        // �� ���� ��θ� ã�� ���� ��Ȳ��
        return null;
    }

    void AddNearGridInList(Node targetNode, Vector3 targetGridPos)
    {
        for (int i = 0; i < targetNode.NearNodesInGround.Count; i++)
        {
            Node nearNode = targetNode.NearNodesInGround[i];
            if (nearNode.CanStep == false || _closedList.Contains(nearNode)) continue; // �������� �ʰų� ���� ����Ʈ�� �ִ� ��� ���� �׸��� Ž�� --> Ground�� ��� �����־�� Ž�� ������

            // ���߿� �ִ� ���� Pos, ���� �ִ� ���� SurfacePos�� ó���Ѵ�.
            float moveCost = Vector3.Distance(targetNode.SurfacePos, nearNode.SurfacePos);
            // �� �κ� �߿�! --> �Ÿ��� �����ؼ� ������Ʈ ���� �ʰ� ��� �����ִ� ������� �����ؾ���
            moveCost += targetNode.G;

            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // ���� ����Ʈ�� �ִ��� G ���� ����ȴٸ� �ٽ� �������ֱ�
            if (isOpenListContainNearGrid == false || moveCost < nearNode.G)
            {
                // ���⼭ grid �� �Ҵ� �ʿ�
                nearNode.G = moveCost;
                nearNode.H = Vector3.Distance(nearNode.SurfacePos, targetGridPos);
                nearNode.ParentNode = targetNode;
            }

            if (isOpenListContainNearGrid == false) _openList.Insert(nearNode);
        }
    }
}
