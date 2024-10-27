using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Node = Pathfinding.Node;

public class GridGenerator : MonoBehaviour
{
    public Node[,,] CreateGrid(float nodeSize, Vector3Int sizeOfGrid, LayerMask obstacleLayer, LayerMask nonPassLayer)
    {
        Node[,,] grid = new Node[sizeOfGrid.x, sizeOfGrid.y, sizeOfGrid.z];
        Vector3 halfSize = new Vector3(nodeSize / 2, nodeSize / 2, nodeSize / 2); // halfExtents

        // 1. �ݶ��̴��� �����ؼ� Non-Pass, Block, Empty�� �����ش�.

        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int z = 0; z < sizeOfGrid.z; z++)
            {
                for (int y = 0; y < sizeOfGrid.y; y++)
                {
                    Vector3 originPos = transform.position + halfSize; // pivot�� ���� �Ʒ��� ��ġ��Ų��.
                    Vector3 pos = originPos + new Vector3(x, y, z) * nodeSize;

                    // �켱 nonPassCollider ����
                    Collider[] nonPassCollider = Physics.OverlapBox(pos, halfSize, Quaternion.identity, nonPassLayer);
                    if(nonPassCollider.Length > 0)
                    {
                        Node nonPassNode = new Node(pos, Node.State.NonPass);
                        grid[x, y, z] = nonPassNode;
                        continue;
                    }

                    Collider[] obstacleCollider = Physics.OverlapBox(pos, halfSize, Quaternion.identity, obstacleLayer);
                    if (obstacleCollider.Length > 0)
                    {
                        Node bloackNode = new Node(pos, Node.State.Block);
                        grid[x, y, z] = bloackNode;
                        continue;
                    }

                    Node emptyNode = new Node(pos, Node.State.Empty);
                    grid[x, y, z] = emptyNode;
                }
            }
        }

        // 2. Pass��� �߿��� �Ʒ��� Block�� ��常 �����ؼ� Raycast�� ���ְ� ��絵�� üũ���ش�.

        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int z = 0; z < sizeOfGrid.z; z++)
            {
                for (int y = 1; y < sizeOfGrid.y; y++)
                {
                    // ���� ĭ�� Empty�̰� �ٷ� �Ʒ� ĭ�� Block�� ��� Raycast�� �߻��ؼ� Surface�� üũ���ش�.
                    if (grid[x, y, z].CurrentState == Node.State.Empty && grid[x, y - 1, z].CurrentState == Node.State.Block)
                    {
                        Vector3 startPos = grid[x, y, z].Pos;

                        // �����ɽ�Ʈ�� �Ʒ��� ���� Ȯ���غ���
                        RaycastHit hit;
                        Physics.BoxCast(startPos, halfSize, Vector3.down, out hit, Quaternion.identity, nodeSize, obstacleLayer);
                        if (hit.transform != null)
                        {
                            Vector3 blockNodePos = grid[x, y - 1, z].Pos;
                            Vector3 surfacePos = new Vector3(blockNodePos.x, hit.point.y, blockNodePos.z);

                            grid[x, y - 1, z].HaveSurface = true;
                            grid[x, y - 1, z].SurfacePos = surfacePos;
                        }
                    }
                }
            }
        }

        return grid;
    }
}
