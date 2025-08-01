using UnityEngine;
using Unity.NetCode;

[UnityEngine.Scripting.Preserve]
public class NewMonoBehaviourScript : ClientServerBootstrap
{
    public override bool Initialize(string defaultWorldName)
    {
        Application.runInBackground = true;
        AutoConnectPort = 7979;
        return base.Initialize(defaultWorldName);
    }
}
