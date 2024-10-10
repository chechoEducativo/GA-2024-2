using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameSystem : MonoBehaviour
{
    protected List<GameSubSystem> gameSubSystems = new List<GameSubSystem>();
    protected virtual void Awake()
    {
        gameSubSystems.AddRange(GetComponentsInChildren<GameSubSystem>());
        foreach (GameSubSystem subSystem in gameSubSystems)
        {
            subSystem.GameSystem = this;
        }
    }

    public virtual TSubSystem GetSubSystem<TSubSystem>() where TSubSystem : GameSubSystem
    {
        return (TSubSystem)gameSubSystems.FirstOrDefault(s => s is TSubSystem);
    }
}
