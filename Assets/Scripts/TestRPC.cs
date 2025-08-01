using UnityEngine;
using Unity.NetCode;
public struct TestRPC : IRpcCommand 
{
    public int value;

    public TestRPC(int value)
    {
        this.value = value;
    }
}
