using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Node : INode<Node>
    {
        // ��� ����
        public enum State
        {
            Empty, // ��� ������ ����
            Block, // ���� �� �ִ� ���� (�ٴ�, �÷��� ��)
            NonPass, // �̵� �Ұ����� ���� (��, ��ֹ� ��)
        }

        // ������
        public Node(Vector3 pos, State state)
        {
            _pos = pos;
            _state = state;

            _haveSurface = false;
            _surfacePos = Vector3.zero;
        }

        // ��� ����
        State _state;
        public State CurrentState { get { return _state; }}

        Vector3 _pos; // ��� ��ġ
        public Vector3 Pos { get { return _pos; }}

        // ���� �� �ִ� ǥ���� �ִ��� ����
        bool _haveSurface;
        public bool HaveSurface { set { _haveSurface = value; } }

        // ����� ���� ǥ�� ��ġ
        Vector3 _surfacePos;
        public Vector3 SurfacePos { set { _surfacePos = value; } get { return _surfacePos; } }

        // ���� �� �ִ��� ���� Ȯ��
        public bool CanStep { get { return _state == State.Block && _haveSurface == true; } }


        // ���� �ش� ��ġ�� �̵��� �� ��尡 CanStep�� �ƴ� ��� AlternativeNode ��� ���
        // ���� ����� ���� ĳ���س��´�.
        // ���� �� ��嵵 ��� �Ұ����ϴٸ� ���ο� ��带 ã�ƾ� �Ѵ�.
        public Node AlternativeNode { get; set; }

        // ���� ����ִ� �̵� ������ ���� ���
        public List<Node> NearNodesInGround { get; set; }

        // ��� ���� ���
        // ��ü ��� Ž���� ���
        public List<Node> NearNodes { get; set; }

        // g�� ���� �������� �Ÿ�
        // h�� �� �������� �Ÿ�
        float g, h = 0;
        public float G { get { return g; } set { g = value; } }
        public float H { get { return h; } set { h = value; } }

        // f�� �� ���� ��ģ ��
        public float F { get { return g + h; } }

        // ��� Ž���� ���Ǵ� �θ� ���
        public Node ParentNode { get; set; }

        // ������ Contain �Լ��� ����ϱ� ���� �ε���
        public int StoredIndex { get; set; }

        // ������ ���ŵ� �� ȣ��
        public void Release()
        {
            StoredIndex = -1;
            ParentNode = null;
        }

        // f�� ���ϴ� �Լ� (���ٸ� h�� ��)
        public int CompareTo(Node other)
        {
            int compareValue = F.CompareTo(other.F);
            if (compareValue == 0) compareValue = H.CompareTo(other.H);
            return compareValue;
        }
    }
}