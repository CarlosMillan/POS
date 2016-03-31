using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gestionix.POS
{
    public class WaitingBar : Control
    {
        #region Const
        private const int DEFAULT_PARTICLES = 1;
        private const float SIZE_PARTICLE_PERCENTAGE = .50f;                // 25% of Canvas size
        private const float FIRST_FRAME_XPROPERTY_PERCENTAGE = .40f;        // 40% of Width
        private const float SECOND_FRAME_XPROPERTY_PERCENTAGE = .20f;       // 20% of Width
        private const double FIRST_FRAME_SECONDS_DURATION = .3;
        private const double SECOND_FRAME_SECONDS_DURATION = 1;
        private const double THIRD_FRAME_SECONDS_DURATION = .3;
        private const double WAITING_TIME_FRAME_SECONDS_DURATION = .2;      // Time to wait between particles in seconds
        private const double WAITING_TIME_MOVEMENT_SECONDS_DURATION = 1.8;  // Time to wait between movement through x axis in seconds
        #endregion

        #region Properties
        protected bool _pending;
        protected double _particleheight;
        protected Storyboard _stBoard;

        protected Canvas Container
        {
            get
            {
                return this.GetTemplateChild("PART_Container") as Canvas;
            }
        }

        protected Border BorderContainer
        {
            get
            {
                return this.GetTemplateChild("PART_BorderContainer") as Border;
            }
        }

        #endregion

        #region Dependency properties
        public static readonly DependencyProperty ParticleColorProperty = DependencyProperty.Register("ParticleColor", typeof(Brush), typeof(WaitingBar), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        [Description("Color particle")]
        public Brush ParticleColor
        {
            get { return (Brush)GetValue(ParticleColorProperty); }
            set { SetValue(ParticleColorProperty, value); }
        }

        public static readonly DependencyProperty ParticlesProperty = DependencyProperty.Register("Particles", typeof(int), typeof(WaitingBar), new PropertyMetadata(DEFAULT_PARTICLES));
        [Description("Particles numerber")]
        public int Particles
        {
            get { return (int)GetValue(ParticlesProperty); }
            set { SetValue(ParticlesProperty, value); }
        }

        public static readonly DependencyProperty IsActivatedProperty = DependencyProperty.Register("IsActivated", typeof(bool), typeof(WaitingBar), new FrameworkPropertyMetadata(false, OnIsActivatePropertyChanged));
        [Description("Activate progress ring")]
        public bool IsActivated
        {
            get { return (bool)GetValue(IsActivatedProperty); }
            set { SetValue(IsActivatedProperty, value); }
        }
        #endregion

        static WaitingBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitingBar), new FrameworkPropertyMetadata(typeof(WaitingBar)));
        }

        #region Events
        private static void OnIsActivatePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (Boolean.Parse(e.NewValue.ToString()))
                ((WaitingBar)source).StartAnimation();
            else if (!Boolean.Parse(e.NewValue.ToString()))
                ((WaitingBar)source).StopAnimation();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Container.Loaded += Container_Loaded;
            InitializeParticles();
        }

        void Container_Loaded(object sender, RoutedEventArgs e)
        {
            if (_pending)
            {
                _pending = false;
                IsActivated = true;
            }
            else Visibility = Visibility.Hidden;
        }
        #endregion

        #region Mehods
        private void InitializeParticles()
        {
            if (Container != null)
            {
                _particleheight = (BorderContainer.Height * SIZE_PARTICLE_PERCENTAGE);
                Container.Height = _particleheight;

                if (Particles == 0)
                    Particles = DEFAULT_PARTICLES;

                if (ParticleColor == null)
                    ParticleColor = Brushes.Black;

                for (int ParticlesCount = 0; ParticlesCount < Particles; ParticlesCount++)
                {
                    Ellipse EllipseParticle = new Ellipse();
                    EllipseParticle.Fill = ParticleColor;
                    
                    EllipseParticle.Width = EllipseParticle.Height = _particleheight;
                    Container.Children.Add(EllipseParticle);
                }
            }
        }

        public void StartAnimation()
        {
            if (Container != null)
            {
                _stBoard = new Storyboard();
                TimeSpan BegintTimeSpan = TimeSpan.FromSeconds(0);
                KeyTime FirstFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION));
                KeyTime SecondFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION));
                KeyTime ThirdFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION + THIRD_FRAME_SECONDS_DURATION));
                int FirstFrameXproperty = (int)(this.Width * FIRST_FRAME_XPROPERTY_PERCENTAGE);
                int SecondFrameXproperty = FirstFrameXproperty + (int)(this.Width * SECOND_FRAME_XPROPERTY_PERCENTAGE);
                int ThirdFrameXproperty = (int)(this.Width - (int)(Container.Height * SIZE_PARTICLE_PERCENTAGE));

                foreach (UIElement Element in Container.Children)
                {
                    ///*Necessary instances*/
                    Ellipse Particle = (Ellipse)Element;
                    TranslateTransform TranslateT = new TranslateTransform();
                    DoubleAnimationUsingKeyFrames DoubleAnimationKeyFrames = new DoubleAnimationUsingKeyFrames();

                    /////*The article ratates in three frames*/
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(FirstFrameXproperty, FirstFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(SecondFrameXproperty, SecondFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(ThirdFrameXproperty, ThirdFrame));

                    Particle.RenderTransform = TranslateT;
          
                    DoubleAnimation VisibilityAnimationIn = new DoubleAnimation(0,1, TimeSpan.FromSeconds(0));
                    DoubleAnimation VisibilityAnimationOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0));

                    Storyboard.SetTarget(VisibilityAnimationIn, Particle);
                    Storyboard.SetTargetProperty(VisibilityAnimationIn, new PropertyPath("Opacity"));
                    VisibilityAnimationIn.BeginTime = BegintTimeSpan.Add(TimeSpan.FromSeconds(0.1));

                    Storyboard.SetTarget(DoubleAnimationKeyFrames, Particle);
                    Storyboard.SetTargetProperty(DoubleAnimationKeyFrames, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                    DoubleAnimationKeyFrames.BeginTime = BegintTimeSpan;

                    Storyboard.SetTarget(VisibilityAnimationOut, Particle);
                    Storyboard.SetTargetProperty(VisibilityAnimationOut, new PropertyPath("Opacity"));
                    VisibilityAnimationOut.BeginTime = BegintTimeSpan.Add(TimeSpan.FromSeconds(WAITING_TIME_MOVEMENT_SECONDS_DURATION));

                    _stBoard.Children.Add(VisibilityAnimationIn);
                    _stBoard.Children.Add(DoubleAnimationKeyFrames);
                    _stBoard.Children.Add(VisibilityAnimationOut);

                    BegintTimeSpan = BegintTimeSpan.Add(TimeSpan.FromSeconds(WAITING_TIME_FRAME_SECONDS_DURATION));
                }

                _stBoard.RepeatBehavior = RepeatBehavior.Forever;
                _stBoard.Begin();
                Visibility = Visibility.Visible;
            }
            else
            {
                if (IsActivated) _pending = true;
                else _pending = false;

                IsActivated = false;
            }
        }

        public void StopAnimation()
        {
            if (Container != null)
            {
                _stBoard.Stop();
                Container.Children.Clear();
                OnApplyTemplate();
            }

            Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
