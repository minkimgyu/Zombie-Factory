using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node : INode<Node>
    {
        // 노드 상태
        public enum State
        {
            Empty, // 통과 가능한 지역
            Block, // 밟을 수 있는 지역 (바닥, 플랫폼 등)
            NonPass, // 이동 불가능한 지역 (벽, 장애물 등)
        }

        // 생성자
        public Node(Vector3 pos, State state)
        {
            _pos = pos;
            _state = state;

            _haveSurface = false;
            _surfacePos = Vector3.zero;
        }

        // 노드 상태
        State _state;
        public State CurrentState { get { return _state; }}

        Vector3 _pos; // 노드 위치
        public Vector3 Pos { get { return _pos; }}

        // 밟을 수 있는 표면이 있는지 여부
        bool _haveSurface;
        public bool HaveSurface { set { _haveSurface = value; } }

        // 노드의 실제 표면 위치
        Vector3 _surfacePos;
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        // 밟을 수 있는지 여부 확인
        public bool CanStep { get { return _state == State.Block && _haveSurface == true; } }


        // 만약 해당 위치로 이동할 때 노드가 CanStep이 아닌 경우 AlternativeNode 노드 사용
        // 다음 사용을 위해 캐싱해놓는다.
        // 만약 이 노드도 사용 불가능하다면 새로운 노드를 찾아야 한다.
        public Node AlternativeNode { get; set; }

        // 땅에 닿아있는 이동 가능한 인접 노드
        public List<Node> NearNodesInGround { get; set; }

        // 모든 인접 노드
        // 대체 노드 탐색에 사용
        public List<Node> NearNodes { get; set; }

        // g는 시작 노드부터의 거리
        // h는 끝 노드부터의 거리
        float g, h = 0;
        public float G { get { return g; } set { g = value; } }
        public float H { get { return h; } set { h = value; } }

        // f는 이 둘을 합친 값
        public float F { get { return g + h; } }

        // 경로 탐색에 사용되는 부모 노드
        public Node ParentNode { get; set; }

        // 힙에서 Contain 함수를 사용하기 위한 인덱스
        public int StoredIndex { get; set; }

        // 힙에서 제거될 때 호출
        public void Release()
        {
            StoredIndex = -1;
            ParentNode = null;
        }

        // f값 비교하는 함수 (같다면 h값 비교)
        public int CompareTo(Node other)
        {
            int compareValue = F.CompareTo(other.F);
            if (compareValue == 0) compareValue = H.CompareTo(other.H);
            return compareValue;
        }
    }
}