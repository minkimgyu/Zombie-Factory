using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerCreater
{
    protected BaseViewer _prefab;

    public ViewerCreater(BaseViewer prefab)
    {
        _prefab = prefab;
    }

    public BaseViewer Create()
    {
        BaseViewer viewer = UnityEngine.Object.Instantiate(_prefab);
        return viewer;
    }
}

public class ViewerFactory : BaseFactory
{
    Dictionary<BaseViewer.Name, ViewerCreater> _effectCreaters;

    public ViewerFactory(AddressableHandler addressableHandler)
    {
        _effectCreaters = new Dictionary<BaseViewer.Name, ViewerCreater>();
        _effectCreaters[BaseViewer.Name.MoveableHpViewer] = new ViewerCreater(addressableHandler.ViewerPrefabs[BaseViewer.Name.MoveableHpViewer]);
    }

    public override BaseViewer Create(BaseViewer.Name name)
    {
        return _effectCreaters[name].Create();
    }
}
