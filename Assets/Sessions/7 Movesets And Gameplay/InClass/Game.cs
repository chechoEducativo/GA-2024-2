using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InClass
{
    public class Game : MonoBehaviour
    {
        private static Game instance;

        private const string PLAYER_PREFAB = "Prefabs/Player";

        private GameObject player;

        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject g = new GameObject("GameInstance");
                    DontDestroyOnLoad(g);
                    instance = g.AddComponent<Game>();
                    instance.player = GameObject.Instantiate(Resources.Load<GameObject>(PLAYER_PREFAB));
                    DontDestroyOnLoad(instance.player);
                }

                return instance;
            }
        }

        public GameObject Player => player;
    }
}
