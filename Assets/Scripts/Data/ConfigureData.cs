using UnityEngine;

namespace Math.Data
{
    public abstract class ConfigureData : ScriptableObject
    {
        public abstract ContentData[] ContentDatas { get; }

        public int ItemPoolSize;
    }

    public abstract class ContentData
    {}

}