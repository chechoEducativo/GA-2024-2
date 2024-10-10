using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGame : MonoBehaviour
{
    private static GameGame instance;

    private const string PLAYER_PREFAB = "Prefabs/Player";

    private GameObject player;
    public static GameGame Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject g = new GameObject("GameInstance");
                DontDestroyOnLoad(g);
                instance = g.AddComponent<GameGame>();

                instance.player = GameObject.Instantiate(Resources.Load<GameObject>(PLAYER_PREFAB));
            }

            return instance;
        }
    }
}
