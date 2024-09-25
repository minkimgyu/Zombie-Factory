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

        Vector3 _pos; // �׸��� ���� ��ġ
        public Vector3 Pos { get { return _pos; } set { _pos = value; } }

        //bool _haveSurface; // ǥ���� ������ �ִ��� ����
        //public bool HaveSurface { get { return _haveSurface; } set { _haveSurface = value; } }

        Vector3 _surfacePos; // ���� ���� �� �ִ� ǥ�� ��ġ
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        // ���� �ش� ��ġ�� �̵��� �� ��尡 Block�� ��� �� ��� ���
        // ���� �� ��嵵 ��� �Ұ����ϴٸ� ���ο� ��带 ã�ƾ� �Ѵ�.
        public Node AlternativeNode { get; set; } // ��ü�� ���

        public List<Node> NearNodesInAir { get; set; }
        public List<Node> NearNodesInGround { get; set; }

        public List<Node> NearNodes { get; set; }
        public List<Node> NearBottomNodes { get; set; }
        // �� ���� Empty�� Surface�� ��常 ���� �����ϱ� ������ �ֺ� ��带 ��� �����ϴ� ������ BFS�� �������
        // ��� ��带 ã�� �� ����


        //bool _nonPass; // ����ġ�� ���ϴ� ���
        //public bool NonPass { get { return _nonPass; } set { _nonPass = value; } }

        //bool _block; // ������Ʈ�� �����ϴ� ���
        //public bool Block { get { return _block; } set { _block = value; } }

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


        public class Builder
        {
            private Vector3 _pos;
            private Vector3 _surfacePos;
            private State _state;

            public Builder()
            {
                // �⺻ �� ����
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