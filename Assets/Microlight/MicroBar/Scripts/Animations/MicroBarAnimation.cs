using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Animation which holds commands on how should animation behave
    // Triggered by UpdateAnim definition in the bar update
    // *Class must be public because it needs to be used in editor scripts
    // ****************************************************************************************************
    [System.Serializable]
    public class MicroBarAnimation {
        [SerializeField] UpdateAnim animationType;
        [SerializeField] Image targetImage;
        [SerializeField] bool notBar;   // If turned on, when animation is skipped, will not fill image fill
        [SerializeField] List<AnimCommand> commands;

        MicroBar parentBar;
        ImageDefaultValues defaultValues;
        internal ImageDefaultValues DefaultValues => defaultValues;

        Sequence sequence;   // Sequence for animations

        internal bool Initialize(MicroBar bar) {
            if(targetImage == null) {
                return false;
            }

            parentBar = bar;
            bar.OnBarUpdate += Update;
            bar.BarDestroyed += Destroy;
            bar.OnDefaultValuesSnapshot += DefaultValuesSnapshot;

            DefaultValuesSnapshot();

            return true;
        }
        // When bar is updated, decide what this animation should do
        void Update(bool skipAnimation, UpdateAnim animationType) {
            // Always kill when bar is updating, because we dont want to have for example active damage animation if heal animation is active
            if(sequence.IsActive()) {
                sequence.Kill();
                sequence = null;
            }
            if(animationType != this.animationType) return;
            if(skipAnimation) {
                if(!notBar) {
                    SilentUpdate();
                }
                return;
            }

            if(sequence.IsActive()) {
                sequence.Kill();
            }
            AnimationInfo animInfo = new AnimationInfo(commands, targetImage, parentBar, this);
            sequence = AnimBuilder.BuildAnimation(animInfo);
        }
        internal void DefaultValuesSnapshot() {
            if(targetImage != null) {
                defaultValues = new ImageDefaultValues(targetImage);
            }
        }
        // When animation is skipped, bar can be updated silently
        void SilentUpdate() {
            if(parentBar == null) {
                Debugger.MissingBarReference();
                return;
            }
            if(targetImage != null) {
                targetImage.fillAmount = parentBar.HPPercent;
            }
            Debugger.SilentUpdate();
        }
        // When health bar is being destroyed
        void Destroy() {
            if(sequence.IsActive()) {
                sequence.Kill();
                sequence = null;
            }
        }
    }
}