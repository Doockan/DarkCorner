using Code.Game.Config;
using Code.Game.Utils;
using Code.Game.View;
using UnityEngine;

namespace Code.Game.Controller
{
    public class PlayerController
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        private const float _movingThresh = 0.3f;
        private const float _walkSpeed = 3f;

        private const float _animationSpeed = 15f;

        private readonly PlayerView _view;
        private readonly SpriteAnimatorController _spriteAnimator;

        private float _xAxisInput;
        private float _yAxisInput;
        
        private readonly ContactPoller _contactPoller;


        public PlayerController(PlayerView view, SpriteAnimatorController spriteAnimator)
        {
            _view = view;
            _spriteAnimator = spriteAnimator;

            _contactPoller = new ContactPoller(_view.Collider2D);
            _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimState.MoveDown, true, _animationSpeed);
        }

        public void Update()
        {
            _xAxisInput = Input.GetAxis(HorizontalAxisName);
            _yAxisInput = Input.GetAxis(VerticalAxisName);
            _spriteAnimator.Update();
            _contactPoller.Update();
            
            var horizontalWalks = Mathf.Abs(_xAxisInput) > _movingThresh;
            var verticalWalks = Mathf.Abs(_yAxisInput) > _movingThresh;

            if (horizontalWalks)
            {
                var track = _xAxisInput < 0 ? AnimState.MoveLeft : AnimState.MoveRight;
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, track, true, _animationSpeed);

                if ((_xAxisInput > 0 || !_contactPoller.HasLeftContacts) 
                    && (_xAxisInput < 0 || !_contactPoller.HasRightContacts))
                {
                    _view.Rigidbody2D.transform.position += Vector3.right * (Time.deltaTime * _walkSpeed * (_xAxisInput < 0 ? -1 : 1));
                }
            }

            if (verticalWalks)
            {
                var track = _yAxisInput < 0 ? AnimState.MoveDown : AnimState.MoveUp;
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, track, true, _animationSpeed);
                
                if ((_yAxisInput > 0 || !_contactPoller.HasUpContacts) 
                    && (_yAxisInput < 0 || !_contactPoller.HasDownContacts))
                {
                    _view.Rigidbody2D.transform.position += Vector3.up * (Time.deltaTime * _walkSpeed * (_yAxisInput < 0 ? -1 : 1));
                }
            }

            if (!horizontalWalks && !verticalWalks)
            {
                _spriteAnimator.StartAnimation(_view.SpriteRenderer, AnimState.Idle, true, _animationSpeed);
            }
        }
    }
}