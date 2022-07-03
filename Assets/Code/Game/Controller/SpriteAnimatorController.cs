using System;
using System.Collections.Generic;
using Code.Game.Config;
using UnityEngine;

namespace Code.Game.Controller
{
    public class SpriteAnimatorController : IDisposable
    {
        private sealed class Animation
        {
            public AnimState Track;
            public List<Sprite> Sprites;
            public bool Loop;
            public float Speed = 10;
            public float Counter = 0;
            public bool Sleep;

            public void Update()
            {
                if (Sleep) return;

                Counter += Time.deltaTime * Speed;

                if (Loop)
                {
                    while (Counter > Sprites.Count)
                    {
                        Counter -= Sprites.Count;
                    }
                }
                else if (Counter > Sprites.Count)
                {
                    Counter = Sprites.Count;
                    Sleep = true;
                }
            }
        }

        private SpriteAnimationConfig _config;
        private Dictionary<SpriteRenderer, Animation> _activeAnimation = new Dictionary<SpriteRenderer, Animation>();

        public SpriteAnimatorController(SpriteAnimationConfig config)
        {
            _config = config;
        }

        public void StartAnimation(SpriteRenderer sprite, AnimState track, bool loop, float speed)
        {
            if (_activeAnimation.TryGetValue(sprite, out var animation))
            {
                animation.Loop = loop;
                animation.Sleep = false;
                animation.Speed = speed;

                if (animation.Track != track)
                {
                    animation.Track = track;
                    animation.Sprites = _config.Sequence.Find(sequence => sequence.Track == track).Sprites;
                    animation.Counter = 0;
                }
            }
            else
            {
                _activeAnimation.Add(sprite, new Animation()
                {
                    Track = track,
                    Sprites = _config.Sequence.Find(sequence => sequence.Track == track).Sprites,
                    Loop = loop,
                    Speed = speed
                });
            }
        }

        public void StopAnimation(SpriteRenderer sprite)
        {
            if (_activeAnimation.ContainsKey(sprite))
            {
                _activeAnimation.Remove(sprite);
            }
        }

        public void Update()
        {
            foreach (var animation in _activeAnimation)
            {
                animation.Value.Update();

                if (animation.Value.Counter < animation.Value.Sprites.Count)
                {
                    animation.Key.sprite = animation.Value.Sprites[(int)animation.Value.Counter];
                }
            }
        }
        
        
        public void Dispose()
        {
            _activeAnimation.Clear();
        }
    }
}