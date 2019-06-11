using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ji.WPFHelper.AminationHelper
{
    public static class AminationMoveHelpers
    {
        /// <summary> 移动动画 </summary>
        /// <param name="usercontrol"> 增加动画的控件 </param>
        /// <param name="start"> 起点 </param>
        /// <param name="end"> 终点 </param>
        /// <param name="nextaction"> 完成动画时回调 </param>
        public static void SetMoveAnimation(this FrameworkElement usercontrol, Point start, Point end, Action nextaction = null)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);

            usercontrol.RenderTransform = new TransformGroup() { Children = new TransformCollection() { new TranslateTransform() } };

            var storyVertical = new Storyboard() { AutoReverse = false };

            var df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(start.Y, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(end.Y, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 1000))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            storyVertical.Children.Add(df);

            df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(start.X, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(end.X, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 1000))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            storyVertical.Children.Add(df);

            if (nextaction != null)
            {
                Action act = null;
                var eh = new EventHandler(delegate { nextaction(); if (act != null) act(); });
                act = () => { storyVertical.Completed -= eh; };
                storyVertical.Completed += eh;
            }

            storyVertical.Begin(usercontrol);
        }

        /// <summary>
        ///收放动画
        /// </summary>
        /// <param name="usercontrol"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetMoveAnimationLefttopToRightBottom(this FrameworkElement usercontrol, double width, double height)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);

            var tfs = new TransformGroup();
            tfs.Children.Add(new ScaleTransform());
            tfs.Children.Add(new TranslateTransform());
            usercontrol.RenderTransform = tfs;

            Storyboard storyVertical = new Storyboard() { AutoReverse = false };

            var df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(0d, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(1d, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
            storyVertical.Children.Add(df);

            df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(1, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
            storyVertical.Children.Add(df);

            df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(-height, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
            storyVertical.Children.Add(df);

            df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(width, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
            storyVertical.Children.Add(df);

            storyVertical.Begin(usercontrol);
        }

        /// <summary>
        /// usercontrol 必须在xaml中标识了 x:Name
        /// </summary>
        /// <param name="usercontrol"></param>
        public static void SetAnimation(this FrameworkElement usercontrol, PropertyPath pp, double from, double to)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            DoubleAnimation xAnimation = new DoubleAnimation();
            Storyboard story = new Storyboard();

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);
            xAnimation.From = from;
            xAnimation.To = to;
            xAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            story.AutoReverse = false;
            story.RepeatBehavior = new RepeatBehavior(1);
            story.Children.Add(xAnimation);
            Storyboard.SetTargetName(xAnimation, usercontrol.Name);
            Storyboard.SetTargetProperty(xAnimation, pp);
            story.Begin(usercontrol);
        }

        /// <summary>
        /// 上移动动画 必须在xaml中标识了 x:Name
        /// </summary>
        /// <param name="usercontrol"></param>
        /// <param name="height"></param>
        public static void SetVerticalAnimation(this FrameworkElement usercontrol, double height)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            DoubleAnimation xAnimation = new DoubleAnimation();
            Storyboard story = new Storyboard();

            usercontrol.Height = 0;

            var pp = new PropertyPath(Control.HeightProperty);
            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);
            xAnimation.From = 0;
            xAnimation.To = height;
            xAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            story.AutoReverse = false;
            story.RepeatBehavior = new RepeatBehavior(1);
            story.Children.Add(xAnimation);
            Storyboard.SetTargetName(xAnimation, usercontrol.Name);
            Storyboard.SetTargetProperty(xAnimation, pp);
            story.Begin(usercontrol);
        }

        /// <summary>
        /// 默认为上移动的动画 必须在xaml中标识了 x:Name
        /// </summary>
        /// <param name="usercontrol"></param>
        public static void SetMoveAnimation(this FrameworkElement usercontrol)
        {
            SetMoveAnimation(usercontrol, new Point(0.5, 0), new Point(0.5, 1));
        }

        ///// <summary>
        ///// 移动显示动画 必须在xaml中标识了 x:Name
        ///// </summary>
        ///// <param name="usercontrol"></param>
        //public static void SetMoveAnimation(this FrameworkElement usercontrol, Point startpoint, Point endpoint, Action nextaction = null)
        //{
        //    if (string.IsNullOrWhiteSpace(usercontrol.Name))
        //    {
        //        usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //    }

        //    NameScope.SetNameScope(usercontrol, new NameScope());
        //    usercontrol.RegisterName(usercontrol.Name, usercontrol);

        //    var lb = new LinearGradientBrush();
        //    lb.StartPoint = startpoint;
        //    lb.EndPoint = endpoint;
        //    var gs0 = new GradientStop(Colors.Black, 0);
        //    var gs1 = new GradientStop(Colors.White, 1);
        //    lb.GradientStops.Add(gs0);
        //    lb.GradientStops.Add(gs1);
        //    usercontrol.OpacityMask = lb;
        //    Storyboard storyVertical = new Storyboard() { AutoReverse = false };

        //    var cf = new ColorAnimationUsingKeyFrames();
        //    cf.KeyFrames.Add(new EasingColorKeyFrame(Colors.Transparent, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
        //    cf.KeyFrames.Add(new EasingColorKeyFrame(Colors.Transparent, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500))));

        //    Storyboard.SetTarget(cf, usercontrol);
        //    Storyboard.SetTargetProperty(cf, new PropertyPath("(UIElement.OpacityMask).(GradientBrush.GradientStops)[1].(GradientStop.Color)"));
        //    storyVertical.Children.Add(cf);

        //    var df = new DoubleAnimationUsingKeyFrames();
        //    df.KeyFrames.Add(new EasingDoubleKeyFrame(1d, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
        //    df.KeyFrames.Add(new EasingDoubleKeyFrame(0.001d, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500))));
        //    Storyboard.SetTarget(df, usercontrol);
        //    Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.OpacityMask).(GradientBrush.GradientStops)[0].(GradientStop.Offset)"));
        //    storyVertical.Children.Add(df);

        //    df = new DoubleAnimationUsingKeyFrames();
        //    df.KeyFrames.Add(new EasingDoubleKeyFrame(0.999, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
        //    df.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 500))));
        //    Storyboard.SetTarget(df, usercontrol);
        //    Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.OpacityMask).(GradientBrush.GradientStops)[1].(GradientStop.Offset)"));
        //    storyVertical.Children.Add(df);

        //    if (nextaction != null)
        //    {
        //        Action act = null;
        //        var eh = new EventHandler(delegate { nextaction(); if (act != null) act(); });
        //        act = () => { storyVertical.Completed -= eh; };
        //        storyVertical.Completed += eh;
        //    }

        //    storyVertical.Begin(usercontrol);
        //}

        /// <summary>
        /// 设置上下滑动动画，由起点滑动到终点
        /// </summary>
        /// <param name="usercontrol"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void SetMoveAnimationTopToBottom(this FrameworkElement usercontrol, double start, double end, int misecond = 300, Action nextaction = null)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);
            var tfs = new TransformGroup();
            tfs.Children.Add(new TranslateTransform());
            usercontrol.RenderTransform = tfs;

            Storyboard storyVertical = new Storyboard() { AutoReverse = false };

            var df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(start, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(end, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, misecond))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
            storyVertical.Children.Add(df);

            if (nextaction != null)
            {
                Action act = null;
                var eh = new EventHandler(delegate { nextaction(); if (act != null) act(); });
                act = () => { storyVertical.Completed -= eh; };
                storyVertical.Completed += eh;
            }
            storyVertical.Begin(usercontrol);
        }

        public static void SetMoveAnimationLeftToRight(this FrameworkElement usercontrol, double start, double end)
        {
            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);
            var tfs = new TransformGroup();
            tfs.Children.Add(new TranslateTransform());
            usercontrol.RenderTransform = tfs;

            Storyboard storyVertical = new Storyboard() { AutoReverse = false };

            var df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(start, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(end, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
            storyVertical.Children.Add(df);

            storyVertical.Begin(usercontrol);
        }

        /// <summary>
        /// 设置往中间消失动画 ishide false 时消失变可见，true 时为可见变为消失
        /// </summary>
        /// <param name="usercontrol"></param>
        /// <param name="ishide"></param>
        public static void SetMoveAnimationBorderToCenter(this FrameworkElement usercontrol, bool ishide = false, Action nextaction = null)
        {
            double startxy = ishide ? 1 : 0;
            double endxy = ishide ? 0 : 1;

            if (string.IsNullOrWhiteSpace(usercontrol.Name))
            {
                usercontrol.Name = "name" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            NameScope.SetNameScope(usercontrol, new NameScope());
            usercontrol.RegisterName(usercontrol.Name, usercontrol);

            var tfs = new TransformGroup();
            tfs.Children.Add(new ScaleTransform());
            usercontrol.RenderTransform = tfs;

            Storyboard storyVertical = new Storyboard() { AutoReverse = false };

            var df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(startxy, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(endxy, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 800))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
            storyVertical.Children.Add(df);

            df = new DoubleAnimationUsingKeyFrames();
            df.KeyFrames.Add(new EasingDoubleKeyFrame(startxy, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0))));
            df.KeyFrames.Add(new EasingDoubleKeyFrame(endxy, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 800))));
            Storyboard.SetTarget(df, usercontrol);
            Storyboard.SetTargetProperty(df, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
            storyVertical.Children.Add(df);

            if (nextaction != null)
            {
                Action act = null;
                var eh = new EventHandler(delegate { nextaction(); if (act != null) act(); });
                act = () => { storyVertical.Completed -= eh; };
                storyVertical.Completed += eh;
            }

            storyVertical.Begin(usercontrol);
        }
    }
}