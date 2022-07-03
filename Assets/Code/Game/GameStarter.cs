using Code.Game.Config;
using Code.Game.Controller;
using Code.Game.View;
using UnityEngine;

namespace Code.Game
{
    public class GameStarter : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerView _playerView;

        private SpriteAnimationConfig _playerConfig;
        private SpriteAnimatorController _playerAnimator;
        private PlayerController _playerController;

        void Awake()
        {
            _playerConfig = Resources.Load<SpriteAnimationConfig>("PlayerAnimatorConfigs");
            _playerAnimator = new SpriteAnimatorController(_playerConfig);
            _playerController = new PlayerController(_playerView, _playerAnimator);
        }
        void Update()
        {
            _playerController.Update();
        }
    }
}
