using UnityEngine;
using DG.Tweening;

namespace Microlight.MicroBar {
    // ****************************************************************************************************
    // Creates DOTween sequence from AnimationCommands in MicroBarAnimation
    // ****************************************************************************************************
    internal static class AnimBuilder {
        /// <summary>
        /// Builds whole sequence for the animation from the list of commands
        /// </summary>
        /// <param name="animInfo">Data about animation</param>
        /// <returns>Sequence for the animation</returns>
        internal static Sequence BuildAnimation(AnimationInfo animInfo) {
            Sequence sequence = DOTween.Sequence();

            foreach (AnimCommand x in animInfo.Commands) {
                InterpretCommand(sequence, x, animInfo);
            }

            return sequence;
        }
        /// <summary>
        /// Interprets how tween should be added to the sequence (appended or joined etc)
        /// Then calls for creation of the tween
        /// </summary>
        /// <param name="sequence">Sequence to which tween will be appended</param>
        /// <param name="command">Comand that should create tween</param>
        /// <param name="animInfo">Data about animation</param>
        static void InterpretCommand(Sequence sequence, AnimCommand command, AnimationInfo animInfo) {
            Tween tween;
            switch(command.Execution) {
                case AnimExecution.Sequence:
                    tween = InterpretEffect(command, animInfo);
                    if(tween != null) {
                        sequence.Append(tween);
                    }
                    break;
                case AnimExecution.Parallel:
                    tween = InterpretEffect(command, animInfo);
                    if(tween != null) {
                        sequence.Join(tween);
                    }
                    break;
                case AnimExecution.Wait:
                    sequence.AppendInterval(command.Duration);
                    break;
                default:
                    Debugger.UnknownAnimExecution(command.Execution);
                    break;
            }
        }
        /// <summary>
        /// Creates tween specified in command
        /// </summary>
        /// <param name="command">Command from which tween should be created</param>
        /// <param name="animInfo">Data about animation</param>
        /// <returns>Created tween for the sequence</returns>
        static Tween InterpretEffect(AnimCommand command, AnimationInfo animInfo) {
            Tween tween;

            // Values to use in switches
            float newFloat;
            float newFloatX;
            float newFloatY;
            float baseFloatValue;
            float defaultFloatValue;
            Vector2 baseVector2Value;
            Vector2 defaultVector2Value;
            Vector2 newVector2;
            Color newColor;

            switch(command.Effect) {
                case AnimEffect.Color:
                    newColor = InterpretValueMode(animInfo.Target.color, animInfo.Animation.DefaultValues.Color, command);
                    tween = animInfo.Target.DOColor(newColor, command.Duration);
                    break;
                case AnimEffect.Fade:
                    baseFloatValue = animInfo.Target.color.a;
                    defaultFloatValue = animInfo.Animation.DefaultValues.Fade;
                    if(command.ValueMode == ValueMode.Additive || command.ValueMode == ValueMode.Multiplicative) {
                        newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                    }
                    else {
                        newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, true);
                    }
                    tween = animInfo.Target.DOFade(newFloat, command.Duration);
                    break;
                case AnimEffect.Fill:
                    if(command.BoolValue) {   // If fill will be custom value or fill to the bar value
                        baseFloatValue = animInfo.Target.fillAmount;
                        defaultFloatValue = animInfo.Animation.DefaultValues.Fill;
                        if(command.ValueMode == ValueMode.Additive || command.ValueMode == ValueMode.Multiplicative) {
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                        }
                        else {
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, true);
                        }
                        tween = animInfo.Target.DOFillAmount(newFloat, command.Duration);
                    }
                    else {
                        tween = animInfo.Target.DOFillAmount(animInfo.Bar.HPPercent, command.Duration);
                    }
                    break;
                case AnimEffect.Move:
                    switch(command.AnimAxis) {
                        case AnimAxis.Uniform:
                            baseFloatValue = animInfo.Target.rectTransform.localPosition.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Position.x;
                            newFloatX = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            baseFloatValue = animInfo.Target.rectTransform.localPosition.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Position.y;
                            newFloatY = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOLocalMove(new Vector2(newFloatX, newFloatY), command.Duration);
                            break;
                        case AnimAxis.XY:
                            baseVector2Value = animInfo.Target.rectTransform.localPosition;
                            defaultVector2Value = animInfo.Animation.DefaultValues.Position;
                            newVector2 = InterpretValueMode(baseVector2Value, defaultVector2Value, command);
                            tween = animInfo.Target.rectTransform.DOLocalMove(newVector2, command.Duration);
                            break;
                        case AnimAxis.X:
                            baseFloatValue = animInfo.Target.rectTransform.localPosition.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Position.x;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOLocalMoveX(newFloat, command.Duration);
                            break;
                        case AnimAxis.Y:
                            baseFloatValue = animInfo.Target.rectTransform.localPosition.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Position.y;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOLocalMoveY(newFloat, command.Duration);
                            break;
                        default:
                            Debugger.UnknownAnimAxis(command.AnimAxis);
                            return null;
                    }
                    break;
                case AnimEffect.Rotate:
                    baseFloatValue = animInfo.Target.rectTransform.localRotation.eulerAngles.z;
                    defaultFloatValue = animInfo.Animation.DefaultValues.Rotation;
                    newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                    tween = animInfo.Target.rectTransform.DOLocalRotate(new Vector3(0f, 0f, newFloat), command.Duration);
                    break;
                case AnimEffect.Scale:
                    switch(command.AnimAxis) {
                        case AnimAxis.Uniform:
                            baseFloatValue = animInfo.Target.rectTransform.localScale.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Scale.x;
                            newFloatX = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            baseFloatValue = animInfo.Target.rectTransform.localScale.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Scale.y;
                            newFloatY = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOScale(new Vector2(newFloatX, newFloatY), command.Duration);
                            break;
                        case AnimAxis.XY:
                            baseVector2Value = animInfo.Target.rectTransform.localScale;
                            defaultVector2Value = animInfo.Animation.DefaultValues.Scale;
                            newVector2 = InterpretValueMode(baseVector2Value, defaultVector2Value, command);
                            tween = animInfo.Target.rectTransform.DOScale(newVector2, command.Duration);
                            break;
                        case AnimAxis.X:
                            baseFloatValue = animInfo.Target.rectTransform.localScale.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Scale.x;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOScaleX(newFloat, command.Duration);
                            break;
                        case AnimAxis.Y:
                            baseFloatValue = animInfo.Target.rectTransform.localScale.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.Scale.y;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOScaleY(newFloat, command.Duration);
                            break;
                        default:
                            Debugger.UnknownAnimAxis(command.AnimAxis);
                            return null;
                    }
                    break;
                case AnimEffect.Punch:
                    switch(command.TransformProperty) {
                        case TransformProperties.Position:
                            tween = animInfo.Target.rectTransform.DOPunchPosition(command.Vector2Value, command.Duration, command.Frequency, command.PercentValue);
                            break;
                        case TransformProperties.Rotation:
                            tween = animInfo.Target.rectTransform.DOPunchRotation(new Vector3(0f, 0f, command.FloatValue), command.Duration, command.Frequency, command.PercentValue);
                            break;
                        case TransformProperties.Scale:
                            tween = animInfo.Target.rectTransform.DOPunchScale(command.Vector2Value, command.Duration, command.Frequency, command.PercentValue);
                            break;
                        case TransformProperties.AnchorPosition:
                            tween = animInfo.Target.rectTransform.DOPunchAnchorPos(command.Vector2Value, command.Duration, command.Frequency, command.PercentValue);
                            break;
                        default:
                            Debugger.UnknownTransformProperty(command.TransformProperty);
                            return null;
                    }
                    break;
                case AnimEffect.Shake:
                    switch(command.TransformProperty) {
                        case TransformProperties.Position:
                            tween = animInfo.Target.rectTransform.DOShakePosition(command.Duration, command.FloatValue, command.Frequency, 90f);
                            break;
                        case TransformProperties.Rotation:
                            tween = animInfo.Target.rectTransform.DOShakeRotation(command.Duration, new Vector3(0f, 0f, command.FloatValue), command.Frequency, 90f);
                            break;
                        case TransformProperties.Scale:
                            tween = animInfo.Target.rectTransform.DOShakeScale(command.Duration, command.FloatValue, command.Frequency, 90f);
                            break;
                        case TransformProperties.AnchorPosition:
                            tween = animInfo.Target.rectTransform.DOShakeAnchorPos(command.Duration, command.FloatValue, command.Frequency, 90f);
                            break;
                        default:
                            Debugger.UnknownTransformProperty(command.TransformProperty);
                            return null;
                    }
                    break;
                case AnimEffect.AnchorMove:
                    switch(command.AnimAxis) {
                        case AnimAxis.Uniform:
                            baseFloatValue = animInfo.Target.rectTransform.anchoredPosition.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.AnchorPosition.x;
                            newFloatX = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            baseFloatValue = animInfo.Target.rectTransform.anchoredPosition.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.AnchorPosition.y;
                            newFloatY = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOAnchorPos(new Vector2(newFloatX, newFloatY), command.Duration);
                            break;
                        case AnimAxis.XY:
                            baseVector2Value = animInfo.Target.rectTransform.anchoredPosition;
                            defaultVector2Value = animInfo.Animation.DefaultValues.AnchorPosition;
                            newVector2 = InterpretValueMode(baseVector2Value, defaultVector2Value, command);
                            tween = animInfo.Target.rectTransform.DOAnchorPos(newVector2, command.Duration);
                            break;
                        case AnimAxis.X:
                            baseFloatValue = animInfo.Target.rectTransform.anchoredPosition.x;
                            defaultFloatValue = animInfo.Animation.DefaultValues.AnchorPosition.x;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOAnchorPosX(newFloat, command.Duration);
                            break;
                        case AnimAxis.Y:
                            baseFloatValue = animInfo.Target.rectTransform.anchoredPosition.y;
                            defaultFloatValue = animInfo.Animation.DefaultValues.AnchorPosition.y;
                            newFloat = InterpretValueMode(baseFloatValue, defaultFloatValue, command, false);
                            tween = animInfo.Target.rectTransform.DOAnchorPosY(newFloat, command.Duration);
                            break;
                        default:
                            Debugger.UnknownAnimAxis(command.AnimAxis);
                            return null;
                    }
                    break;
                default:
                    Debugger.UnknownAnimEffect(command.Effect);
                    return null;
            }

            if(tween == null) {
                return null;   // Don't go further if tween is null, but this should not be true in any case
            }

            if(command.Delay > 0f) {
                tween.SetDelay(command.Delay);
            }
            tween.SetEase(command.Ease);
            return tween;
        }

        #region Value interpreter
        // ****************************************************************************************************
        static Color InterpretValueMode(Color baseValue, Color defaultValue, AnimCommand command) {
            if(command.ValueMode == ValueMode.Absolute) {
                return command.ColorValue;
            }
            else if(command.ValueMode == ValueMode.Additive) {
                return baseValue + command.ColorValue;
            }
            else if(command.ValueMode == ValueMode.Multiplicative) {
                return baseValue * command.ColorValue;
            }
            else if(command.ValueMode == ValueMode.StartingValue) {
                return baseValue;
            }
            else if(command.ValueMode == ValueMode.DefaultValue) {
                return defaultValue;
            }
            else {
                Debugger.UnknownValueMode(command.ValueMode);
                return baseValue;
            }
        }
        static float InterpretValueMode(float baseValue, float defaultValue, AnimCommand command, bool percent) {
            if(command.ValueMode == ValueMode.Absolute) {
                if(percent) {
                    return Mathf.Clamp01(command.PercentValue);
                }
                else {
                    return command.FloatValue;
                }
            }
            else if(command.ValueMode == ValueMode.Additive) {
                if(percent) {
                    return Mathf.Clamp01(baseValue + command.PercentValue);
                }
                else {
                    return baseValue + command.FloatValue;
                }
            }
            else if(command.ValueMode == ValueMode.Multiplicative) {
                if(percent) {
                    return Mathf.Clamp01(baseValue * command.PercentValue);
                }
                else {
                    return baseValue * command.FloatValue;
                }
            }
            else if(command.ValueMode == ValueMode.StartingValue) {
                if(percent) {
                    return Mathf.Clamp01(baseValue);
                }
                else {
                    return baseValue;
                }
            }
            else if(command.ValueMode == ValueMode.DefaultValue) {
                return defaultValue;
            }
            else {
                Debugger.UnknownValueMode(command.ValueMode);
                return baseValue;
            }
        }
        static int InterpretValueMode(int baseValue, int defaultValue, AnimCommand command) {
            if(command.ValueMode == ValueMode.Absolute) {
                return command.IntValue;
            }
            else if(command.ValueMode == ValueMode.Additive) {
                return baseValue + command.IntValue;
            }
            else if(command.ValueMode == ValueMode.Multiplicative) {
                return baseValue * command.IntValue;
            }
            else if(command.ValueMode == ValueMode.StartingValue) {
                return baseValue;
            }
            else if(command.ValueMode == ValueMode.DefaultValue) {
                return defaultValue;
            }
            else {
                Debugger.UnknownValueMode(command.ValueMode);
                return baseValue;
            }
        }
        static Vector3 InterpretValueMode(Vector3 baseValue, Vector3 defaultValue, AnimCommand command) {
            if(command.ValueMode == ValueMode.Absolute) {
                return command.Vector3Value;
            }
            else if(command.ValueMode == ValueMode.Additive) {
                return baseValue + command.Vector3Value;
            }
            else if(command.ValueMode == ValueMode.Multiplicative) {
                return new Vector3(baseValue.x * command.Vector3Value.x, baseValue.y * command.Vector3Value.y, baseValue.z * command.Vector3Value.z);
            }
            else if(command.ValueMode == ValueMode.StartingValue) {
                return baseValue;
            }
            else if(command.ValueMode == ValueMode.DefaultValue) {
                return defaultValue;
            }
            else {
                Debugger.UnknownValueMode(command.ValueMode);
                return baseValue;
            }
        }
        static Vector2 InterpretValueMode(Vector2 baseValue, Vector2 defaultValue, AnimCommand command) {
            if(command.ValueMode == ValueMode.Absolute) {
                return command.Vector2Value;
            }
            else if(command.ValueMode == ValueMode.Additive) {
                return baseValue + command.Vector2Value;
            }
            else if(command.ValueMode == ValueMode.Multiplicative) {
                return new Vector2(baseValue.x * command.Vector3Value.x, baseValue.y * command.Vector3Value.y);
            }
            else if(command.ValueMode == ValueMode.StartingValue) {
                return baseValue;
            }
            else if(command.ValueMode == ValueMode.DefaultValue) {
                return defaultValue;
            }
            else {
                Debugger.UnknownValueMode(command.ValueMode);
                return baseValue;
            }
        }
        #endregion
    }
}