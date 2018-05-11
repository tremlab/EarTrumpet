﻿using EarTrumpet.Services;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EarTrumpet.Extensions
{
    internal static class WindowExtensions
    {
        private static bool shouldAnimate()
        {
            return SystemParameters.MenuAnimation;
        }

        public static void HideWithAnimation(this Window window, Action completed)
        {
            var onCompleted = new EventHandler((s, e) =>
             {
                 window.Visibility = Visibility.Hidden;
                 completed();
             });

            var hideAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                FillBehavior = FillBehavior.Stop,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn }
            };
            var taskbarPosition = TaskbarService.GetWinTaskbarState().TaskbarPosition;
            switch (taskbarPosition)
            {
                case TaskbarPosition.Left:
                case TaskbarPosition.Right:
                    hideAnimation.From = window.Left;
                    break;
                default:
                    hideAnimation.From = window.Top;
                    break;
            }
            hideAnimation.To = (taskbarPosition == TaskbarPosition.Top || taskbarPosition == TaskbarPosition.Left) ? hideAnimation.From - 10 : hideAnimation.From + 10;
            hideAnimation.Completed += onCompleted;

            switch (taskbarPosition)
            {
                case TaskbarPosition.Left:
                case TaskbarPosition.Right:
                    if (shouldAnimate())
                    {
                        window.ApplyAnimationClock(Window.LeftProperty, hideAnimation.CreateClock());
                    }
                    else
                    {
                        window.Left = (double)hideAnimation.To;
                    }
                    break;
                default:
                    if (shouldAnimate())
                    {
                        window.ApplyAnimationClock(Window.TopProperty, hideAnimation.CreateClock());
                    }
                    else
                    {
                        window.Top = (double)hideAnimation.To;
                    }
                    break;
            }

            if (!shouldAnimate())
            {
                onCompleted(null, null);
            }
        }

        public static void ShowwithAnimation(this Window window, Action completed)
        {
            var onCompleted = new EventHandler((s, e) =>
            {
                window.Topmost = true;
                window.Focus();
                completed();
            });

            window.Visibility = Visibility.Visible;
            window.Topmost = false;
            window.Activate();
            var showAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                FillBehavior = FillBehavior.Stop,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut }
            };
            var taskbarPosition = TaskbarService.GetWinTaskbarState().TaskbarPosition;
            switch (taskbarPosition)
            {
                case TaskbarPosition.Left:
                case TaskbarPosition.Right:
                    showAnimation.To = window.Left;
                    break;
                default:
                    showAnimation.To = window.Top;
                    break;
            }
            showAnimation.From = (taskbarPosition == TaskbarPosition.Top || taskbarPosition == TaskbarPosition.Left) ? showAnimation.To - 25 : showAnimation.To + 25;
            showAnimation.Completed += onCompleted;

            switch (taskbarPosition)
            {
                case TaskbarPosition.Left:
                case TaskbarPosition.Right:
                    if (shouldAnimate())
                    {
                        window.ApplyAnimationClock(Window.LeftProperty, showAnimation.CreateClock());
                    }
                    else
                    {
                        window.Left = (double)showAnimation.To;
                    }
                    break;
                default:
                    if (shouldAnimate())
                    {
                        window.ApplyAnimationClock(Window.TopProperty, showAnimation.CreateClock());
                    }
                    else
                    {
                        window.Top = (double)showAnimation.To;
                    }
                    break;
            }

            if (!shouldAnimate())
            {
                onCompleted(null, null);
            }
        }

        public static Matrix CalculateDpiFactors(this Window window)
        {
            var mainWindowPresentationSource = PresentationSource.FromVisual(window);
            return mainWindowPresentationSource == null ? new Matrix() { M11 = 1, M22 = 1 } : mainWindowPresentationSource.CompositionTarget.TransformToDevice;
        }

        public static double DpiHeightFactor(this Window window)
        {
            var m = CalculateDpiFactors(window);
            return m.M22;
        }

        public static double DpiWidthFactor(this Window window)
        {
            var m = CalculateDpiFactors(window);
            return m.M11;
        }
    }
}
