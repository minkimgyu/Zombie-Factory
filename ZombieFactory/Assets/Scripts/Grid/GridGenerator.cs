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
                    Vector3 originPos = transform.position + boxSize; // pivot을 왼쪽 아래로 위치시킨다.
                    Vector3 pos = originPos + new Vector3(x, y, z) * nodeSize;

                    Node node;

                    // 우선 nonPassCollider 검출
                    Collider[] nonPassCollider = Physics.OverlapBox(pos, boxSize, Quaternion.identity, nonPassLayer);
                    if(nonPassCollider.Length > 0)
                    {
                        // Builder를 사용하여 Node 객체 생성
                        node = new Node.Builder()
                            .SetPosition(pos)
                            .SetState(Node.State.NonPass)
                            .Build();
                    }
                    else // 이후에 obstacleCollider 검출
                    {
                        Collider[] obstacleCollider = Physics.OverlapBox(pos, boxSize, Quaternion.identity, obstacleLayer);
                        if (obstacleCollider.Length == 1)
                        {
                            // 레이케스트를 아래로 쏴서 확인해보기
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
                                    .SetState(Node.State.Surface) // 표면을 가지고 있는 경우
                                    .SetSurfacePosition(surfacePos)
                                    .Build();
                            }
                        }
                        else if(obstacleCollider.Length > 1) // 1개 보다 많은 콜라이더가 검출된다면 Block으로 적용
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
