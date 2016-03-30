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
        private const int DEFAULT_PARTICLES = 5;
        private const float SIZE_PARTICLE_PERCENTAGE = .50f;                // 25% of Canvas size
        private const float FIRST_FRAME_XPROPERTY_PERCENTAGE = .40f;        // 40% of Width
        private const float SECOND_FRAME_XPROPERTY_PERCENTAGE = .20f;       // 20% of Width
        private const double FIRST_FRAME_SECONDS_DURATION = .3;
        private const double SECOND_FRAME_SECONDS_DURATION = 1;
        private const double THIRD_FRAME_SECONDS_DURATION = .3;
        private const double WAITING_TIME_FRAME_SECONDS_DURATION = .2;  // Time to wait between particles in seconds
        #endregion

        #region Properties
        protected bool _pending;

        protected Canvas Container
        {
            get
            {
                return this.GetTemplateChild("PART_Container") as Canvas;
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
                if (Particles == 0)
                    Particles = DEFAULT_PARTICLES;

                if (ParticleColor == null)
                    ParticleColor = Brushes.Black;

                for (int ParticlesCount = 0; ParticlesCount < Particles; ParticlesCount++)
                {
                    Ellipse EllipseParticle = new Ellipse();
                    EllipseParticle.Fill = ParticleColor;
                    EllipseParticle.Width = EllipseParticle.Height = (Container.Height * SIZE_PARTICLE_PERCENTAGE);
                    Container.Children.Add(EllipseParticle);
                }
            }
        }

        public void StartAnimation()
        {
            if (Container != null)
            {
                TimeSpan BegintTimeSpan = TimeSpan.FromSeconds(0);
                KeyTime FirstFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION));
                KeyTime SecondFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION));
                KeyTime ThirdFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION + THIRD_FRAME_SECONDS_DURATION));
                int FirstFrameXproperty = (int)(this.Width * FIRST_FRAME_XPROPERTY_PERCENTAGE);
                int SecondFrameXproperty = FirstFrameXproperty + (int)(this.Width * SECOND_FRAME_XPROPERTY_PERCENTAGE);
                int ThirdFrameXproperty = (int)(this.Width - (int)(Container.Height * SIZE_PARTICLE_PERCENTAGE));

                foreach (UIElement Element in Container.Children)
                {
                    /*Necessary instances*/
                    Ellipse Particle = (Ellipse)Element;
                    TransformGroup GroupT = new TransformGroup();
                    //RotateTransform RotateT = new RotateTransform();
                    TranslateTransform TranslateT = new TranslateTransform();
                    DoubleAnimationUsingKeyFrames DoubleAnimationKeyFrames = new DoubleAnimationUsingKeyFrames();

                    ///*Point of reference to animate the particle*/
                    Particle.RenderTransformOrigin = new Point(0, 0);

                    ///*The article ratates in three frames*/
                    DoubleAnimationKeyFrames.BeginTime = BegintTimeSpan;
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(FirstFrameXproperty, FirstFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(SecondFrameXproperty, SecondFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(ThirdFrameXproperty, ThirdFrame));

                    ///*Around this point the particles will rotate*/
                    //RotateT.CenterX = _container.ActualWidth / 2;
                    //RotateT.CenterY = _container.ActualHeight / 2;

                    /*This is initial point*/
                    TranslateT.X = 0;
                    TranslateT.Y = (Container.ActualHeight / 2) - (Particle.ActualHeight / 2);

                    ///*Group render transforms*/
                    GroupT.Children.Add(TranslateT);
                    //GroupT.Children.Add(RotateT);
                    Particle.RenderTransform = GroupT;

                    ///*Starts animation*/
                    DoubleAnimationKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
                    TranslateT.BeginAnimation(TranslateTransform.XProperty, DoubleAnimationKeyFrames);
                    //Particle.Visibility = System.Windows.Visibility.Visible;
                    /*Delay between particles*/
                    BegintTimeSpan = BegintTimeSpan.Add(TimeSpan.FromSeconds(WAITING_TIME_FRAME_SECONDS_DURATION));
                }

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
                Container.Children.Clear();
                OnApplyTemplate();
            }

            Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
