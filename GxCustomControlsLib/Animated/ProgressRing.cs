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
    public class ProgressRing : Control
    {
        private const int DEFAULT_PARTICLES = 5;
        private const int SIZE_PARTICLE_PERCENTAGE = 13;                // 10% of Canvas size
        private const int HUNDRED_PERCENTAGE = 100;
        private const int FIRST_FRAME_DEGREES = 70;                  
        private const int SECOND_FRAME_DEGREES = 150;
        private const int THIRD_FRAME_DEGREES = 360;                   
        private const int FRAMES_BY_PARTICLES = 3;                      // First frame goes to 0° - 70°, second frame goes to 70° - 150°, third goes to 150° - 360°
        private const double FIRST_FRAME_SECONDS_DURATION = .3;
        private const double SECOND_FRAME_SECONDS_DURATION = .7;
        private const double THIRD_FRAME_SECONDS_DURATION = .4;
        private const double WAITING_TIME_FRAME_SECONDS_DURATION = .18;  // Time to wait between particles in seconds

        private Canvas _container;
        private bool _pending;

        public static readonly DependencyProperty ParticleColorProperty = DependencyProperty.Register("ParticleColor", typeof(Brush), typeof(ProgressRing), new PropertyMetadata(null));
        public static readonly DependencyProperty ParticlesProperty = DependencyProperty.Register("Particles", typeof(int), typeof(ProgressRing), new PropertyMetadata(null));
        public static readonly DependencyProperty IsActivatedProperty = DependencyProperty.Register("IsActivated", typeof(bool), typeof(ProgressRing), new FrameworkPropertyMetadata(false, OnIsActivatePropertyChanged));

        [Description("Color particle")]
        public Brush ParticleColor
        {
            get { return (Brush)GetValue(ParticleColorProperty); }
            set { SetValue(ParticleColorProperty, value); }
        }

        [Description("Particles numerber")]
        public int Particles
        {
            get { return (int)GetValue(ParticlesProperty); }
            set { SetValue(ParticlesProperty, value); }
        }

        [Description("Activate progress ring")]
        public bool IsActivated
        {
            get { return (bool)GetValue(IsActivatedProperty); }
            set { SetValue(IsActivatedProperty, value); }
        }

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing), new FrameworkPropertyMetadata(typeof(ProgressRing)));            
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _container = this.GetTemplateChild("PART_Container") as Canvas;
            _container.Loaded += _container_Loaded;
            InitializeParticles();
        }

        private void _container_Loaded(object sender, RoutedEventArgs e)
        {
            if (_pending)
            {
                _pending = false;
                IsActivated = true;
            }
            else Visibility = Visibility.Hidden;
        }
        
        private static void OnIsActivatePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (Boolean.Parse(e.NewValue.ToString()))
                ((ProgressRing)source).StartAnimation();
            else if (!Boolean.Parse(e.NewValue.ToString()))
                ((ProgressRing)source).StopAnimation();
        }
        
        private void InitializeParticles()
        {
            if (_container != null)
            {
                if (Particles == 0)
                    Particles = DEFAULT_PARTICLES;

                if (ParticleColor == null)
                    ParticleColor = Brushes.Black;

                for (int ParticlesCount = 0; ParticlesCount < Particles; ParticlesCount++)
                {
                    Ellipse EllipseParticle = new Ellipse();
                    EllipseParticle.Fill = ParticleColor;
                    EllipseParticle.Width = EllipseParticle.Height = (_container.Width * SIZE_PARTICLE_PERCENTAGE) / HUNDRED_PERCENTAGE;
                    _container.Children.Add(EllipseParticle);
                }
            }
        }

        public void StartAnimation()
        {
            if (_container != null)
            {
                TimeSpan BegintTimeSpan = TimeSpan.FromSeconds(0);
                KeyTime FirstFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION));
                KeyTime SecondFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION));
                KeyTime ThirdFrame = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(FIRST_FRAME_SECONDS_DURATION + SECOND_FRAME_SECONDS_DURATION + THIRD_FRAME_SECONDS_DURATION));

                foreach (UIElement Element in _container.Children)
                {
                    /*Necessary instances*/
                    Ellipse Particle = (Ellipse)Element;
                    TransformGroup GroupT = new TransformGroup();
                    RotateTransform RotateT = new RotateTransform();
                    TranslateTransform TranslateT = new TranslateTransform();
                    DoubleAnimationUsingKeyFrames DoubleAnimationKeyFrames = new DoubleAnimationUsingKeyFrames();

                    /*Point of reference to animate the particle*/
                    Particle.RenderTransformOrigin = new Point(0, 0);

                    /*The article ratates in three frames*/
                    DoubleAnimationKeyFrames.BeginTime = BegintTimeSpan;
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(FIRST_FRAME_DEGREES, FirstFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(SECOND_FRAME_DEGREES, SecondFrame));
                    DoubleAnimationKeyFrames.KeyFrames.Add(new LinearDoubleKeyFrame(THIRD_FRAME_DEGREES, ThirdFrame));
                    
                    /*Around this point the particles will rotate*/
                    RotateT.CenterX = _container.ActualWidth / 2;
                    RotateT.CenterY = _container.ActualHeight / 2;

                    /*This is initial point*/
                    TranslateT.X = 0;
                    TranslateT.Y = _container.ActualHeight / 2;

                    /*Group render transforms*/
                    GroupT.Children.Add(TranslateT);
                    GroupT.Children.Add(RotateT);
                    Particle.RenderTransform = GroupT;                    

                    /*Starts animation*/
                    DoubleAnimationKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
                    RotateT.BeginAnimation(RotateTransform.AngleProperty, DoubleAnimationKeyFrames);

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
            if (_container != null)
            {
                _container.Children.Clear();
                OnApplyTemplate();                
            }

            Visibility = Visibility.Hidden;
        }
    }
}
