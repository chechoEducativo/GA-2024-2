using UnityEngine;

namespace InClass
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerCameraManager2 cameraManager;

        private Character controlledCharacter;

        public void StartControllingCharacter(Character character)
        {
            controlledCharacter = character;
            controlledCharacter.Player = this;
        }
        
        public PlayerCameraManager2 CameraManager => cameraManager;
    }
}
