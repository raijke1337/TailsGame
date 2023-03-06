using UnityEngine;

public abstract class LoadedManagerBase : MonoBehaviour
{
    public abstract void Initiate();
    public abstract void RunUpdate(float delta);
    public abstract void Stop();

}
