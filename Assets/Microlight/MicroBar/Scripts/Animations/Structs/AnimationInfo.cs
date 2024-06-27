using System.Collections.Generic;
using UnityEngine.UI;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Stores data about animation for easier passing of the data
    // ****************************************************************************************************
    internal readonly struct AnimationInfo {
        readonly IReadOnlyList<AnimCommand> commands;
        readonly Image target;
        readonly MicroBar bar;
        readonly MicroBarAnimation animation;

        public readonly IReadOnlyList<AnimCommand> Commands => commands;
        public readonly Image Target => target;
        public readonly MicroBar Bar => bar;
        public readonly MicroBarAnimation Animation => animation;

        internal AnimationInfo(IReadOnlyList<AnimCommand> commands, Image target, MicroBar bar, MicroBarAnimation animation) {
            this.commands = commands;
            this.target = target;
            this.bar = bar;
            this.animation = animation;
        }
    }
}