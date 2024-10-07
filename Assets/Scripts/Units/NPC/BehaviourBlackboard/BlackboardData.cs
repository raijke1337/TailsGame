using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcatech.BlackboardSystem
{
    [CreateAssetMenu(fileName = "Blackboard data file", menuName = "NPC Behaviour / Blackboard Data") ]
    public class BlackboardData : ScriptableObjectID
    {
        // we can have a ton of data typesfor key T so we use a wrapper class
        public List<BlackboardEntryData> entries = new();
        public void SetupBlackboard (Blackboard bb)
        {
            foreach (var e in entries)
            {
                e.SetValueOnBlackboard (bb);
            }
        }


    }


    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver
    {
        public string Key;
        public AnyValue.ValueType ValueType;
        public AnyValue Value;

        //this is called a dispatch table
        // basically this is a check to make sure that when a value is set on blackborad,
        // it is actually a valuetype and not an object
        static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> setTable =
            new Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>>()
            {
                {  AnyValue.ValueType.Bool,(bb,key,val) => bb.SetValue<bool>(key,val) },
                {  AnyValue.ValueType.Int,(bb,key,val) => bb.SetValue<int>(key,val) },
                {  AnyValue.ValueType.String,(bb,key,val) => bb.SetValue<string>(key,val) },
                {  AnyValue.ValueType.Float,(bb,key,val) => bb.SetValue<float>(key,val) },
                {  AnyValue.ValueType.Vector3,(bb,key,val) => bb.SetValue<Vector3>(key,val) },
            };

        public void SetValueOnBlackboard (Blackboard bb)
        {
            var key = bb.GetOrRegisterKey(Key); // new key
            setTable[Value.Type](bb, key, Value);  // set values
        }

        public void OnAfterDeserialize()
        {
            Value.Type = ValueType;
        }

        public void OnBeforeSerialize() { }
    }




    [Serializable]
    public struct AnyValue
    {
        //supported data types for blackboard
        public enum ValueType { Int, Float, Bool, String, Vector3 };
        public bool boolVal;
        public float floatVal;
        public int intVal;
        public string stringVal;
        public Vector3 vectorVal;

        public ValueType Type;

        // implicit conversions to get values
        public static implicit operator bool(AnyValue v) => v.ConvertValue<bool>();
        public static implicit operator float(AnyValue v) => v.ConvertValue<float>();
        public static implicit operator int(AnyValue v) => v.ConvertValue<int>();
        public static implicit operator string(AnyValue v) => v.ConvertValue<string>();
        public static implicit operator Vector3(AnyValue v) => v.ConvertValue<Vector3>();

        T AsBool<T>(bool value) =>
            typeof(T) == typeof(bool)
            && value is T correctType ? correctType : default;

        T AsInt<T>(int value) =>
            typeof(T) == typeof(int)
            && value is T correctType ? correctType : default;

        T AsFloat<T>(float value) =>
            typeof(T) == typeof(float)
            && value is T correctType ? correctType : default;



        T ConvertValue <T> ()
        {
            return Type switch
            {
                ValueType.Bool => AsBool<T>(boolVal),
                ValueType.Int => AsInt<T>(intVal),  
                ValueType.Float => AsFloat<T>(floatVal),
                ValueType.String => (T)(object)stringVal,
                ValueType.Vector3 => (T)(object)vectorVal,
                _ => throw new NotImplementedException()
            };
        }
    }
}
