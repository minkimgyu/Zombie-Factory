using System;

// ��� �������̽�
public interface INode<T> : IComparable<T>
{
    // Heap���� Index�� �����ϱ� ���� ����
    public int StoredIndex { get; set; }

    // �ʱ�ȭ �Լ�
    public void Release();
}

// ���� ���뼺�� ����� ���׸����� ����
public class Heap<T> where T : INode<T>
{
    // ������
    public Heap(int maxItemCount)
    {
        _items = new T[maxItemCount];
        _count = 0;
    }

    // ���������� �ڽ� ��带 ���� ����� �ε��� ��ȯ
    int GetLastIndexHavingChild()
    {
        return (_count - 2) / 2;
    }

    // �θ� ��� �ε��� ��ȯ
    int GetParentIndex(int index)
    {
        return (index - 1) / 2;
    }

    // �ڽ� ��� �ε��� ��ȯ
    int GetChildIndex(int index, bool isLeft = true)
    {
        if (isLeft) return 2 * index + 1;
        else return 2 * index + 2;
    }

    // �� ����� �迭 �� ��ġ�� �ٲ���
    void Swap(int index1, int index2)
    {
        T tmp = _items[index2];
        _items[index2] = _items[index1];
        _items[index1] = tmp;

        _items[index1].StoredIndex = index1;
        _items[index2].StoredIndex = index2;
    }

    // ���� �ö󰡸鼭 �θ�� ��
    void PercolateUp(int index)
    {
        int currentIndex = index;
        int parentIndex = GetParentIndex(index);

        while (currentIndex > 0 && _items[parentIndex].CompareTo(_items[currentIndex]) > 0)
        {
            Swap(parentIndex, currentIndex);

            currentIndex = parentIndex;
            parentIndex = GetParentIndex(parentIndex);
        }
    }

    // �Ʒ��� �������鼭 �ڽİ� ��
    void PercolateDown(int index)
    {
        int currentIndex = index;
        int childIndex = GetMinIndexInChildren(index);

        while (childIndex < _count && _items[childIndex].CompareTo(_items[currentIndex]) < 0)
        {
            Swap(childIndex, currentIndex);

            if (childIndex > GetLastIndexHavingChild()) break; // �ڽ��� �������� �ʴ� ��� Ż��

            currentIndex = childIndex;
            childIndex = GetMinIndexInChildren(childIndex); // ���� ���ֱ�
        }
    }

    // �ڽ� �� �� ���� ���� ���� ����� �ε��� ��ȯ
    int GetMinIndexInChildren(int index)
    {
        int leftChildIndex = GetChildIndex(index);
        int rightChildIndex = GetChildIndex(index, false);

        T leftChild = _items[leftChildIndex];
        T rightChild = _items[rightChildIndex];

        // right�� �� ���� ���
        if (rightChild != null && rightChild.CompareTo(leftChild) < 0) return rightChildIndex;
        return leftChildIndex;
    }

    // ��� �ʱ�ȭ
    public void Clear()
    {
        for (int i = 0; i < _count; i++)
        {
            _items[i].Release();
        }
        Array.Clear(_items, 0, Count);
        _count = 0;
    }

    // �ش� ��尡 ���� ���ԵǾ��ִ��� Ȯ��
    public bool Contain(T item)
    {
        if (item.StoredIndex == -1) return false; // �ʱ�ȭ�� �� �Ǿ����� ���
        return Equals(_items[item.StoredIndex], item);
    }

    // ��� ����
    public void Insert(T item)
    {
        item.StoredIndex = _count;
        _items[_count] = item;
        PercolateUp(_count);
        _count++;
    }

    // �ּҰ� ��� ����
    public void DeleteMin()
    {
        if (_count == 0) return;
        else if (_count == 1)
        {
            _items[0].StoredIndex = -1;
            _count--;
            return;
        }

        T lastItem = _items[_count - 1];
        lastItem.StoredIndex = 0;

        _items[0].StoredIndex = -1;
        _count--;

        _items[0] = lastItem; // ù��° ���������� �ٲ���

        PercolateDown(0);
    }

    // �ּҰ� ��� ��ȯ
    public T ReturnMin() { return _items[0]; }

    // ��� �迭
    T[] _items;

    // ��� ����
    int _count = 0;
    public int Count { get { return _count; } }
}