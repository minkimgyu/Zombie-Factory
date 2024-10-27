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

        Vector3 _pos; // �׸��� ���� ��ġ
        public Vector3 Pos { get { return _pos; }}


        bool _haveSurface;
        public bool HaveSurface { set { _haveSurface = value; } }

        Vector3 _surfacePos; // ���� ���� �� �ִ� ǥ�� ��ġ
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        public bool CanStep { get { return _state == State.Block && _haveSurface == true; } }


        float _surfaceSlope; // ǥ���� ����
        public float SurfaceSlope { set { _surfaceSlope = value; } get { return _surfaceSlope; } }


        // ���� �ش� ��ġ�� �̵��� �� ��尡 Block�� ��� �� ��� ���
        // ���� �� ��嵵 ��� �Ұ����ϴٸ� ���ο� ��带 ã�ƾ� �Ѵ�.
        public Node AlternativeNode { get; set; } // ��ü�� ���
        public List<Node> NearNodesInGround { get; set; }
        public List<Node> NearNodes { get; set; }

       

        // g�� ���� �������� �Ÿ�
        // h�� �� �������� �Ÿ�
        // f�� �� ���� ��ģ ��
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