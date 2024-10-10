using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSubSystem : MonoBehaviour
{
    protected GameSystem gameSystem;

    public GameSystem GameSystem
    {
        set => gameSystem = value;
    }
}
