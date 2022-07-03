using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Config
{
    [CreateAssetMenu(fileName = "SpriteAnimatorConfig", menuName = "Config/SpriteAnimatorConfig", order = 1)]
    public class SpriteAnimationConfig : ScriptableObject
    {
        [Serializable]
        public sealed class SpriteSequence
        {
            public AnimState Track;
            public List<Sprite> Sprites = new List<Sprite>();
        }

        public List<SpriteSequence> Sequence = new List<SpriteSequence>();
    }
}
