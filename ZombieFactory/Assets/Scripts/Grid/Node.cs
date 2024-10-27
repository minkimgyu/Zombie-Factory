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
            NonPass,
        }

        public Node(Vector3 pos, State state)
        {
            _pos = pos;
            _state = state;

            _haveSurface = false;
            _surfacePos = Vector3.zero;
        }

        State _state;
        public State CurrentState { get { return _state; }}

        Vector3 _pos; // 그리드 실제 위치
        public Vector3 Pos { get { return _pos; }}


        bool _haveSurface;
        public bool HaveSurface { set { _haveSurface = value; } }

        Vector3 _surfacePos; // 발을 딛을 수 있는 표면 위치
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        public bool CanStep { get { return _state == State.Block && _haveSurface == true; } }


        float _surfaceSlope; // 표면의 각도
        public float SurfaceSlope { set { _surfaceSlope = value; } get { return _surfaceSlope; } }


        // 만약 해당 위치로 이동할 때 노드가 Block인 경우 이 노드 사용
        // 만약 이 노드도 사용 불가능하다면 새로운 노드를 찾아야 한다.
        public Node AlternativeNode { get; set; } // 대체할 노드
        public List<Node> NearNodesInGround { get; set; }
        public List<Node> NearNodes { get; set; }

       

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
    }
}