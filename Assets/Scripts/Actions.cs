using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All of the actions able in a behaviour are here
/// </summary>
public abstract class Actions : MonoBehaviour
{
    public Dictionary<string, bool> commands = new Dictionary<string, bool>();

    /// <summary>
    /// Run the action
    /// </summary>
    /// <param name="active">Action can go two ways</param>
    public virtual void Run(bool active) { }

    /// <summary>
    /// Stop the action from running
    /// </summary>
    public virtual void Stop() { }
}