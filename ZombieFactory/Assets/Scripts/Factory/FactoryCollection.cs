using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryCollection
{
    public enum Type
    {
        Effect,
        Item,
        Life,
        Ragdoll,
    }

    public Dictionary<Type, BaseFactory> Factories { get; private set; }

    // DI를 통해 의존성 주입
    public FactoryCollection(AddressableHandler addressableHandler, HelperMediator mediator, Transform effectParent)
    {
        Factories = new Dictionary<Type, BaseFactory>();

        BaseFactory effectFactory = new EffectFactory(addressableHandler, effectParent);
        BaseFactory ragdollFactory = new RagdollFactory(addressableHandler);
        BaseFactory itemFactory = new ItemFactory(addressableHandler, effectFactory);
        BaseFactory lifeFactory = new LifeFactory(addressableHandler, mediator, effectFactory, itemFactory, ragdollFactory);
        
        Factories.Add(Type.Effect, effectFactory);
        Factories.Add(Type.Life, lifeFactory);
        Factories.Add(Type.Item, itemFactory);
    }
}