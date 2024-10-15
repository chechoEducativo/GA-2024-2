using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InClass
{
    public class Character : MonoBehaviour
    {
        private Player player;

        private CharacterState state;

        private void Awake()
        {
            state = GetComponent<CharacterState>();
            Game.Instance.Player.GetComponent<Player>().StartControllingCharacter(this);
        }

        public Player Player
        {
            get => player;
            set => player = value;
        }

        public CharacterState State => state;
    }
}
