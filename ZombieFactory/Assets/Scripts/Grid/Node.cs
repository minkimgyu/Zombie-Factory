using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node : INode<Node>
    {
        public enum State
        {
            Empty,
            Block,
            Surface,
            NonPass,
        }

        State _state;
        public State CurrentState { get { return _state; } set { _state = value; } }

        Vector3 _pos; // 그리드 실제 위치
        public Vector3 Pos { get { return _pos; } set { _pos = value; } }

        //bool _haveSurface; // 표면을 가지고 있는지 여부
        //public bool HaveSurface { get { return _haveSurface; } set { _haveSurface = value; } }

        Vector3 _surfacePos; // 발을 딛을 수 있는 표면 위치
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        // 만약 해당 위치로 이동할 때 노드가 Block인 경우 이 노드 사용
        // 만약 이 노드도 사용 불가능하다면 새로운 노드를 찾아야 한다.
        public Node AlternativeNode { get; set; } // 대체할 노드

        public List<Node> NearNodesInAir { get; set; }
        public List<Node> NearNodesInGround { get; set; }

        public List<Node> NearNodes { get; set; }
        public List<Node> NearBottomNodes { get; set; }
        // 이 둘은 Empty나 Surface의 노드만 보유 가능하기 때문에 주변 노드를 모두 보유하는 변수로 BFS를 돌려줘야
        // 대안 노드를 찾을 수 있음


        //bool _nonPass; // 지나치지 못하는 경우
        //public bool NonPass { get { return _nonPass; } set { _nonPass = value; } }

        //bool _block; // 오브젝트가 존재하는 경우
        //public bool Block { get { return _block; } set { _block = value; } }

        // g는 시작 노드부터의 거리
        // h는 끝 노드부터의 거리
        // f는 이 둘을 합친 값
        float g, h = 0;
        public float G { get { return g; } set { g = value; } }
        public float H { get { return h; } set { h = value; } }
        public float F { get { return g + h; } }

        public Node ParentNode { get; set; }
        public int StoredIndex { get; set; }



        public void Dispose()
        {
            StoredIndex = -1;
            ParentNode = null;
        }

        public int CompareTo(Node other)
        {
            int compareValue = F.CompareTo(other.F);
            if (compareValue == 0) compareValue = H.CompareTo(other.H);
            return compareValue;
        }


        public class Builder
        {
            private Vector3 _pos;
            private Vector3 _surfacePos;
            private State _state;

            public Builder()
            {
                // 기본 값 설정
                _pos = Vector3.zero;
                _surfacePos = Vector3.zero;
                _state = State.Empty;
            }

            public Builder SetPosition(Vector3 pos)
            {
                _pos = pos;
                return this;
            }

            public Builder SetState(State state)
            {
                _state = state;
                return this;
            }

            public Builder SetSurfacePosition(Vector3 surfacePos)
            {
                _surfacePos = surfacePos;
                return this;
            }

            public Node Build()
            {
                Node node = new Node();
                node.Pos = _pos;
                node.SurfacePos = _surfacePos;
                node.CurrentState = _state;
                return node;
            }
        }
    }
}