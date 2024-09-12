using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;

[Serializable]
public struct SerializableVector2
{
    public SerializableVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public float x;
    public float y;

    [JsonIgnore]
    public Vector3 V2 { get { return new Vector3(x, y); } }
}

[System.Serializable]
public struct SerializableVector3
{
    public SerializableVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float x;
    public float y;
    public float z;

    [JsonIgnore]
    public Vector3 V3 { get { return new Vector3(x, y, z); } }
}

public class JsonParser
{
    public T JsonToObject<T>(string json)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.DeserializeObject<T>(json, setting);
    }

    public T JsonToObject<T>(TextAsset tmpAsset)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.DeserializeObject<T>(tmpAsset.text, setting);
    }

    public string ObjectToJson(object objectToParse)
    {
        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        return JsonConvert.SerializeObject(objectToParse, setting);
    }
}
