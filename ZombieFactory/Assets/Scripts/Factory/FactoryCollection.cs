using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class FactoryRegistry
//{
//    Dictionary<FactoryCollection.Type, BaseFactory> _factories = new Dictionary<FactoryCollection.Type, BaseFactory>();

//    public void RegisterFactory(BaseFactory factory)
//    {
//        _factories[typeof(T)] = factory;
//    }

//    public T GetFactory<T>()
//    {
//        return (T)_factories[typeof(T)];
//    }
//}

// --> �߻� ���丮 ������ Ȱ���ؼ� ���� �� ���� �� ����.

public class FactoryCollection
{
    public enum Type
    {
        Effect,
        Item,
        Life,
        Ragdoll,
        SoundPlayer,
    }

    public Dictionary<Type, BaseFactory> Factories { get; private set; }

    public FactoryCollection(AddressableHandler addressableHandler)
    {
        Factories = new Dictionary<Type, BaseFactory>();

        // DI�� ���� ������ ����

        BaseFactory effectFactory = new EffectFactory(addressableHandler);
        BaseFactory ragdollFactory = new RagdollFactory(addressableHandler);

        BaseFactory lifeFactory = new LifeFactory(addressableHandler, effectFactory, ragdollFactory);
        BaseFactory itemFactory = new ItemFactory(addressableHandler, effectFactory);
        BaseFactory soundFactory = new SoundPlayerFactory(addressableHandler);

        Factories.Add(Type.Effect, effectFactory);
        Factories.Add(Type.Life, lifeFactory);
        Factories.Add(Type.Item, itemFactory);
        Factories.Add(Type.SoundPlayer, soundFactory);
    }
}