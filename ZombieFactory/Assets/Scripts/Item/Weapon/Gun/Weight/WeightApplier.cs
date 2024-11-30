using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class WeightApplier
{
    public float storedWeight = 0;
    [JsonIgnore] public float StoredWeight { get { return storedWeight; } }

    float minWeight = 0;
    public float maxWeight = 0.03f;
    public float weightMultiplier = 0.005f;
    public float weightDecreation = 0.01f;

    public WeightApplier(float maxWeight, float weightMultiplier, float weightDecreation)
    {
        this.maxWeight = maxWeight;
        this.weightMultiplier = weightMultiplier;
        this.weightDecreation = weightDecreation;
    }

    public void MultiplyWeight()
    {
        storedWeight += weightMultiplier;
        if (maxWeight < storedWeight) storedWeight = maxWeight;
    }

    void DecreaseWeight()
    {
        storedWeight -= weightDecreation;
        if (storedWeight < minWeight)
        {
            storedWeight = minWeight;
        }
    }

    public void OnUpdate()
    {
        DecreaseWeight();
    }
}
