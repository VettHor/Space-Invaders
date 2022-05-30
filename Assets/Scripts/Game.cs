using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public sealed class Game : MonoBehaviour
    {
        public GameFacade gameFacade;
        private void Awake()
        {
            gameFacade.AwakeGame();
        }

        private void Start()
        {
            gameFacade.StartGame();
        }

        private void Update()
        {
            gameFacade.UpdateGame();
        }
    }
}
