using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Swat;

public class SwatSpawner : BaseSpawner
{
    BaseFactory _lifeFactory;
    BaseFactory _viewerFactory;
    BaseFactory _itemFactory;

    [SerializeField] Color _rangeColor;
    [SerializeField] int _spawnCount = 1;
    [SerializeField] float _range = 3f;
    [SerializeField] List<BaseItem.Name> _weaponNames;
    [SerializeField] BaseLife.Name[] _lifeNames;

    public override void Initialize(BaseFactory lifeFactory, BaseFactory viewerFactory) 
    {
        _lifeFactory = lifeFactory;
        _viewerFactory = viewerFactory;
    }

    protected Vector3 ReturnRandomPos()
    {
        Vector2 randomPos = Random.insideUnitCircle * _range;
        return transform.position + new Vector3(randomPos.x, 0, randomPos.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _rangeColor;
        Gizmos.DrawSphere(transform.position, _range);
    }

    public override void Spawn()
    {
        for (int i = 0; i < _lifeNames.Length; i++)
        {
            BaseLife swat = _lifeFactory.Create(_lifeNames[i], _weaponNames);

            TrackableHpViewer hpViewer = (TrackableHpViewer)_viewerFactory.Create(BaseViewer.Name.MoveableHpViewer);
            hpViewer.transform.SetParent(swat.transform);
            hpViewer.transform.localPosition = new Vector3(0, 1.8f, 0);

            hpViewer.ResetName(_lifeNames[i].ToString());
            swat.AddObserverEvent(hpViewer.UpdateHp);

            Vector3 pos = ReturnRandomPos();
            swat.ResetPosition(pos);
        }
    }
}