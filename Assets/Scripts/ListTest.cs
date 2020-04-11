using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tests
{
    public class ListTest : MonoBehaviour
    {
        public int[] AwesomeInts;
        public float[] GoodFloats;
        public List<string> NiceStrings;
        public List<Transform> CoolTransforms;
        public List<TestClass> OkClasses;
        public List<TestStruct> OkStructs;
        public List<SomeType> SomeEnums;
        public Vector3[] FancyVectors;
        public bool[] ExcitingBools;
    }

    [Serializable]
    public class TestClass
    {
        public Vector3 Vector;
        public float Value;
        public GameObject GameObject;
    }

    [Serializable]
    public struct TestStruct
    {
        public SomeType Type;
        public int Value;
        //Nested lists are not supported yet :(
        public int[] BadInts;
    }

    public enum SomeType
    {
        Value1,
        Value2,
        Value3
    }
}