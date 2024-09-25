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

// --> 추상 팩토리 패턴을 활용해서 묶을 수 있을 것 같다.

public class FactoryCollection
{
    public enum Type
    {
        Effect,
        Weapon,
        Life,
        Ragdoll,

        ArmedCharacter
    }

    public Dictionary<Type, BaseFactory> Factories { get; private set; }

    public FactoryCollection(AddressableHandler addressableHandler)
    {
        Factories = new Dictionary<Type, BaseFactory>();

        // DI를 통해 의존성 주입

        BaseFactory effectFactory = new EffectFactory(addressableHandler);
        BaseFactory ragdollFactory = new RagdollFactory(addressableHandler);

        BaseFactory lifeFactory = new LifeFactory(addressableHandler, effectFactory, ragdollFactory);
        BaseFactory itemFactory = new ItemFactory(addressableHandler, effectFactory);

        Factories.Add(Type.Effect, effectFactory);
        Factories.Add(Type.Life, lifeFactory);
        Factories.Add(Type.Weapon, itemFactory);
    }
}