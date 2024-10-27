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

        // 1. 콜라이더를 검출해서 Non-Pass, Block, Empty를 나눠준다.

        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int z = 0; z < sizeOfGrid.z; z++)
            {
                for (int y = 0; y < sizeOfGrid.y; y++)
                {
                    Vector3 originPos = transform.position + halfSize; // pivot을 왼쪽 아래로 위치시킨다.
                    Vector3 pos = originPos + new Vector3(x, y, z) * nodeSize;

                    // 우선 nonPassCollider 검출
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

        // 2. Pass노드 중에서 아래가 Block인 노드만 선별해서 Raycast를 쏴주고 경사도를 체크해준다.

        for (int x = 0; x < sizeOfGrid.x; x++)
        {
            for (int z = 0; z < sizeOfGrid.z; z++)
            {
                for (int y = 1; y < sizeOfGrid.y; y++)
                {
                    // 현재 칸은 Empty이고 바로 아래 칸은 Block인 경우 Raycast를 발사해서 Surface를 체크해준다.
                    if (grid[x, y, z].CurrentState == Node.State.Empty && grid[x, y - 1, z].CurrentState == Node.State.Block)
                    {
                        Vector3 startPos = grid[x, y, z].Pos;

                        // 레이케스트를 아래로 쏴서 확인해보기
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
