using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;

public interface IPathfinder
{
    List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos);
}

// A* �˰����� ����� ���� ��� Ž����
public class GroundPathfinder : IPathfinder
{
    GridComponent _gridComponent;

    // �ִ� �� ũ��
    const int maxSize = 1000;
    Heap<Node> _openList;
    HashSet<Node> _closedList;

    // �޸���ƽ ����ġ -> �������� �޸���ƽ�� ������� Ŀ�� -> ��ǥ �������� �� ���������� ���ϵ��� ������ Ž�� ������ ����
    // �׷��� �ⱸ�� �ϳ� �ۿ� ���� �̷ο� ���� �ʿ����� ������ ��ȿ������ �� ����
    [SerializeField] float _weight = 1.0f;

    // �ʱ�ȭ �Լ�
    public GroundPathfinder(GridComponent gridComponent)
    {
        _gridComponent = gridComponent;
        _openList = new Heap<Node>(maxSize);
        _closedList = new HashSet<Node>();
    }

    // Node ������ Vector3 ����Ʈ�� ��ȯ�Ͽ� ���� ��� ��ȯ
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

    // BFS�� ���� �̵� ������ ���� ����� ��带 ã��
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

    // ��ü ��� ��ȯ �Լ�
    Node GetAlternativeNode(Node node)
    {
        Node alternativeNode = null;

        // ���� ��尡 CanStep�� false�� ��� ��ü ��带 ã�Ƽ� �������ش�.
        if (node.CanStep == false)
        {
            Node startAlternativeNode;

            if (node.AlternativeNode != null && node.AlternativeNode.CanStep == true)
            {
                alternativeNode = node.AlternativeNode; // ���� ���� ���̶�� �ش� ���� ��ü
            }
            else
            {
                startAlternativeNode = FindAlternativeNode(node); // ��ü ��� ã��
                node.AlternativeNode = startAlternativeNode; // ��ü ���� �������ֱ�
                alternativeNode = node.AlternativeNode; // �ش� ���� �ٲ��ֱ�
            }
        }
        else
        {
            alternativeNode = node;
        }

        return alternativeNode;
    }

    // ��� Ž�� �Լ�
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // ����Ʈ �ʱ�ȭ
        _openList.Clear();
        _closedList.Clear();

        // ���� ���� �ݿø��� ���� ���� ����� ��带 ã�´�.
        Vector3Int startIndex = _gridComponent.GetNodeIndex(startPos);
        Vector3Int endIndex = _gridComponent.GetNodeIndex(targetPos);

        // �ε����� �´� ��� ��ȯ
        Node startNode = _gridComponent.ReturnNode(startIndex);
        Node endNode = _gridComponent.ReturnNode(endIndex);

        // ��尡 ���� ��� null ��ȯ
        if (startNode == null || endNode == null) { return null; }

        // ���� ���� ��峪 �� ��尡 CanStep�� �ƴ� ��� ��ü ��带 ã�Ƽ� �������ش�.
        if (startNode.CanStep == false) startNode = GetAlternativeNode(startNode);

        if (endNode.CanStep == false) endNode = GetAlternativeNode(endNode);

        if (startNode == null || endNode == null) { return null; }

        _openList.Insert(startNode);

        while (_openList.Count > 0)
        {
            Node targetNode = _openList.ReturnMin();
            if (targetNode == endNode) // �������� Ÿ���� ����
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

            _openList.DeleteMin(); // Min��� ����
            _closedList.Add(targetNode); // closedList�� �߰�
            AddNearGridInList(targetNode, endNode.SurfacePos);
        }

        // �� ���� ��θ� ã�� ���� ��Ȳ��
        return null;
    }

    // ��Ŭ���� �Ÿ� ��� �Լ�
    float GetDistance(Vector3 nearNodeSurfacePos, Vector3 endSurfacePos)
    {
        return Vector3.Distance(nearNodeSurfacePos, endSurfacePos);
    }

    // ���� ��带 ���� ����Ʈ�� �߰��ϴ� �Լ�
    void AddNearGridInList(Node targetNode, Vector3 endSurfacePos)
    {
        for (int i = 0; i < targetNode.NearNodesInGround.Count; i++)
        {
            // �̵��� �� ���ų� ���� ����Ʈ�� �ִ� ��� ���� ��� Ž��
            Node nearNode = targetNode.NearNodesInGround[i];
            if (nearNode.CanStep == false || _closedList.Contains(nearNode)) continue;

            float moveCost = GetDistance(targetNode.SurfacePos, nearNode.SurfacePos);
            moveCost += targetNode.G;

            bool isOpenListContainNearGrid = _openList.Contain(nearNode);

            // ���� ����Ʈ�� �ִ��� G���� �� �۴ٸ� �ٽ� �������ֱ�
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
