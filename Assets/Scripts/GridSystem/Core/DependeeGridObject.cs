using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GridSystem.Core
{
    /// <summary>
    /// <para>
    /// A struct that is used for representing a dependee GridObject.
    /// </para>
    /// 
    /// <para>
    /// This is just a <see cref="NumberedGridObject"/> with a coordinate respect to the dependent GridObject position.
    /// </para>
    /// </summary>
    public struct DependeeGridObject
    {
        public Type GridObjectType;
        public int Amount;
        public Vector2 DependentSpaceCoordinate;

        public DependeeGridObject(Type type, int amount) : this(type, amount, Vector2.zero)
        {
        }

        public DependeeGridObject(Type type, int amount, Vector2 dependentSpaceCoordinate)
        {
            GridObjectType = type;
            Amount = amount;
            DependentSpaceCoordinate = dependentSpaceCoordinate;
        }
    }
}