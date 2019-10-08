using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All of the actions able in a behavior are here
/// </summary>
public abstract class Actions : MonoBehaviour
{
    internal PigIdle idle;

    public Dictionary<string, dynamic> commands = new Dictionary<string, dynamic>();

    /// <summary>
    /// Run the action
    /// </summary>
    /// <param name="param">Action can go two ways</param>
    public virtual void Run(dynamic param)
    {
        idle.isActive = true;
    }

    /// <summary>
    /// Stop the action from running
    /// </summary>
    public virtual void Stop()
    {
        idle.isActive = false;
    }
}