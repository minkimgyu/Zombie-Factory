using System;

// 노드 인터페이스
public interface INode<T> : IComparable<T>
{
    // Heap에서 Index를 저장하기 위한 변수
    public int StoredIndex { get; set; }

    // 초기화 함수
    public void Release();
}

// 추후 재사용성을 고려해 제네릭으로 구현
public class Heap<T> where T : INode<T>
{
    // 생성자
    public Heap(int maxItemCount)
    {
        _items = new T[maxItemCount];
        _count = 0;
    }

    // 마지막으로 자식 노드를 가진 노드의 인덱스 반환
    int GetLastIndexHavingChild()
    {
        return (_count - 2) / 2;
    }

    // 부모 노드 인덱스 반환
    int GetParentIndex(int index)
    {
        return (index - 1) / 2;
    }

    // 자식 노드 인덱스 반환
    int GetChildIndex(int index, bool isLeft = true)
    {
        if (isLeft) return 2 * index + 1;
        else return 2 * index + 2;
    }

    // 두 노드의 배열 상 위치를 바꿔줌
    void Swap(int index1, int index2)
    {
        T tmp = _items[index2];
        _items[index2] = _items[index1];
        _items[index1] = tmp;

        _items[index1].StoredIndex = index1;
        _items[index2].StoredIndex = index2;
    }

    // 위로 올라가면서 부모와 비교
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

    // 아래로 내려가면서 자식과 비교
    void PercolateDown(int index)
    {
        int currentIndex = index;
        int childIndex = GetMinIndexInChildren(index);

        while (childIndex < _count && _items[childIndex].CompareTo(_items[currentIndex]) < 0)
        {
            Swap(childIndex, currentIndex);

            if (childIndex > GetLastIndexHavingChild()) break; // 자식이 존재하지 않는 경우 탈출

            currentIndex = childIndex;
            childIndex = GetMinIndexInChildren(childIndex); // 조건 봐주기
        }
    }

    // 자식 중 더 작은 값을 가진 노드의 인덱스 반환
    int GetMinIndexInChildren(int index)
    {
        int leftChildIndex = GetChildIndex(index);
        int rightChildIndex = GetChildIndex(index, false);

        T leftChild = _items[leftChildIndex];
        T rightChild = _items[rightChildIndex];

        // right가 더 작은 경우
        if (rightChild != null && rightChild.CompareTo(leftChild) < 0) return rightChildIndex;
        return leftChildIndex;
    }

    // 노드 초기화
    public void Clear()
    {
        for (int i = 0; i < _count; i++)
        {
            _items[i].Release();
        }
        Array.Clear(_items, 0, Count);
        _count = 0;
    }

    // 해당 노드가 힙에 포함되어있는지 확인
    public bool Contain(T item)
    {
        if (item.StoredIndex == -1) return false; // 초기화가 안 되어있을 경우
        return Equals(_items[item.StoredIndex], item);
    }

    // 노드 삽입
    public void Insert(T item)
    {
        item.StoredIndex = _count;
        _items[_count] = item;
        PercolateUp(_count);
        _count++;
    }

    // 최소값 노드 제거
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

        _items[0] = lastItem; // 첫번째 아이템으로 바꿔줌

        PercolateDown(0);
    }

    // 최소값 노드 반환
    public T ReturnMin() { return _items[0]; }

    // 노드 배열
    T[] _items;

    // 노드 개수
    int _count = 0;
    public int Count { get { return _count; } }
}