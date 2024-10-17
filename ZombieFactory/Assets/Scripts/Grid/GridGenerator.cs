using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Node = Pathfinding.Node;

public class GridGenerator : MonoBehaviour
{
    Node[,,] _grid;

    public Node[,,] CreateGrid(float nodeSize, Vector3Int sizeOfGrid, LayerMask obstacleLayer, LayerMask nonPassLayer)
    {
        _grid = new Node[sizeOfGrid.x, sizeOfGrid.y, sizeOfGrid.z];
        Vector3 boxSize = new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2);

        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int y = 0; y < sizeOfGrid.y; y++)
            {
                for (int z = 0; z < sizeOfGrid.z; z++)
                {
                    Vector3 originPos = transform.position + boxSize; // pivot�� ���� �Ʒ��� ��ġ��Ų��.
                    Vector3 pos = originPos + new Vector3(x, y, z) * nodeSize;

                    Node node;

                    // �켱 nonPassCollider ����
                    Collider[] nonPassCollider = Physics.OverlapBox(pos, boxSize, Quaternion.identity, nonPassLayer);
                    if(nonPassCollider.Length > 0)
                    {
                        // Builder�� ����Ͽ� Node ��ü ����
                        node = new Node.Builder()
                            .SetPosition(pos)
                            .SetState(Node.State.NonPass)
                            .Build();
                    }
                    else // ���Ŀ� obstacleCollider ����
                    {
                        Collider[] obstacleCollider = Physics.OverlapBox(pos, boxSize, Quaternion.identity, obstacleLayer);
                        if (obstacleCollider.Length == 1)
                        {
                            // �����ɽ�Ʈ�� �Ʒ��� ���� Ȯ���غ���
                            RaycastHit hit;
                            Physics.BoxCast(pos + Vector3.up * nodeSize, boxSize, Vector3.down, out hit, Quaternion.identity, nodeSize, obstacleLayer);

                            if (hit.transform == null)
                            {
                                node = new Node.Builder()
                                    .SetPosition(pos)
                                    .SetState(Node.State.Block)
                                    .Build();
                            }
                            else
                            {
                                Vector3 surfacePos = new Vector3(pos.x, hit.point.y, pos.z);
                                node = new Node.Builder()
                                    .SetPosition(pos)
                                    .SetState(Node.State.Surface) // ǥ���� ������ �ִ� ���
                                    .SetSurfacePosition(surfacePos)
                                    .Build();
                            }
                        }
                        else if(obstacleCollider.Length > 1) // 1�� ���� ���� �ݶ��̴��� ����ȴٸ� Block���� ����
                        {
                            node = new Node.Builder()
                                .SetPosition(pos)
                                .SetState(Node.State.Block)
                                .Build();
                        }
                        else
                        {
                            node = new Node.Builder()
                                .SetPosition(pos)
                                .SetState(Node.State.Empty)
                                .Build();
                        }
                    }

                    _grid[x, y, z] = node;

                }
            }
        }

        return _grid;
    }
}
