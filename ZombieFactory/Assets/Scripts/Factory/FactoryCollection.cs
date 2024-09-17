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
        Weapon,
        Life,

        ArmedCharacter
    }

    public Dictionary<Type, BaseFactory> Factories { get; private set; }

    public FactoryCollection(AddressableHandler addressableHandler)
    {
        Factories = new Dictionary<Type, BaseFactory>();

        // DI�� ���� ������ ����

        BaseFactory effectFactory = new EffectFactory(addressableHandler);
        BaseFactory lifeFactory = new LifeFactory(addressableHandler, effectFactory);
        BaseFactory itemFactory = new ItemFactory(addressableHandler, effectFactory);
        BaseFactory armedCharacterFactory = new ArmedCharacterFactory(itemFactory, effectFactory, lifeFactory);

        Factories.Add(Type.Effect, effectFactory);
        Factories.Add(Type.Life, lifeFactory);
        Factories.Add(Type.Weapon, itemFactory);

        Factories.Add(Type.ArmedCharacter, armedCharacterFactory);
    }
}