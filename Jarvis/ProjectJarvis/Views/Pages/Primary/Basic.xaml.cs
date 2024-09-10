using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NAudio.Wave;
using System.Windows;
using Jarvis.Project.Settings.ConfigurationManager;
using System.Xml.Linq;
using log4net;

namespace Jarvis.Project.Views.Pages.Primary;

public partial class Basic : Page
{
    public static Basic Instance { get; private set; }
    private const string ApiKey = "e327b05bf5e93f0560363f8bc007f7e2";
    private readonly HttpClient http = new HttpClient();
    private WaveInEvent waveIn;
    private double baseScale = 0.9;
    private double maxScale = 2.3;
    private Storyboard[] waveAnimations;
    private ScaleTransform[] scaleTransforms;
    private RotateTransform[] rotateTransforms;
    private TranslateTransform[] translateTransforms;
    private Random random;
    private bool isQuietMode = false;
    private double quietModeScaleFactor = 0.09;
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public Basic()
    {
        InitializeComponent();
        InitializeMicrophone();
        InitializeWaveAnimation();
        random = new Random();
        Instance = this;
    }
    
    // Метод для добавления ответа Джарвиса
    public void AddJarvisAnswer(string answer)
    {
        Dispatcher.Invoke(() =>
        {
            var answerContainer = new StackPanel
                { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2.5, 0, 2.5) };

            // Добавляем фиолетовый кружок
            var purpleCircle = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Color.FromRgb(68, 102, 255)), // Фиолетовый цвет
                Margin = new Thickness(0, 0, 5, 0)
            };
            answerContainer.Children.Add(purpleCircle);

            // Добавляем текст ответа
            var answerTextBlock = new TextBlock
            {
                Text = answer,
                Foreground = Brushes.White,
                FontSize = 16
            };
            answerContainer.Children.Add(answerTextBlock);

            // Добавляем контейнер в StackPanel
            answerStackPanel.Children.Add(answerContainer);
        });
    }

    // Метод для добавления запроса пользователя
    public void AddUserRequest(string request)
    {
        Dispatcher.Invoke(() =>
        {
            var requestContainer = new StackPanel
                { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2.5, 0, 2.5) };

            // Добавляем белый кружок
            var whiteCircle = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.White,
                Margin = new Thickness(0, 0, 5, 0)
            };
            requestContainer.Children.Add(whiteCircle);

            // Добавляем текст запроса
            var requestTextBlock = new TextBlock
            {
                Text = request,
                Foreground = Brushes.White,
                FontSize = 16
            };
            requestContainer.Children.Add(requestTextBlock);

            // Добавляем контейнер в StackPanel
            answerStackPanel.Children.Add(requestContainer);
        });
    }

    private void InitializeMicrophone()
    {
        waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(44100, 1);
        waveIn.DataAvailable += OnDataAvailable;
        waveIn.StartRecording();
    }

    private void InitializeWaveAnimation()
    {
        waveAnimations = new Storyboard[5];
        scaleTransforms = new ScaleTransform[5];
        rotateTransforms = new RotateTransform[5];
        translateTransforms = new TranslateTransform[5];

        for (int i = 0; i < 5; i++)
        {
            Ellipse circle = (Ellipse)FindName($"circle{i + 1}");

            RadialGradientBrush gradientBrush = new RadialGradientBrush();
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 40, 215), 0.1));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 100, 255), 0.5));


            circle.Fill = gradientBrush;

            ScaleTransform scaleTransform = new ScaleTransform();
            RotateTransform rotateTransform = new RotateTransform();
            TranslateTransform translateTransform = new TranslateTransform();

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(translateTransform);

            circle.RenderTransform = transformGroup;
            circle.RenderTransformOrigin = new Point(0.7, 0.7);
            scaleTransforms[i] = scaleTransform;
            rotateTransforms[i] = rotateTransform;
            translateTransforms[i] = translateTransform;

            // Основная анимация масштабирования
            DoubleAnimation scaleAnimationX = new DoubleAnimation
            {
                From = baseScale,
                To = baseScale + 0.5,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(scaleAnimationX, scaleTransform);
            Storyboard.SetTargetProperty(scaleAnimationX, new PropertyPath(ScaleTransform.ScaleXProperty));

            DoubleAnimation scaleAnimationY = new DoubleAnimation
            {
                From = baseScale,
                To = baseScale + 0.5,
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(scaleAnimationY, scaleTransform);
            Storyboard.SetTargetProperty(scaleAnimationY, new PropertyPath(ScaleTransform.ScaleYProperty));

            // Анимация вращения
            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(6),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(rotateAnimation, rotateTransform);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath(RotateTransform.AngleProperty));

            // Анимация перемещения
            DoubleAnimation translateAnimationX = new DoubleAnimation
            {
                From = -50,
                To = 50,
                Duration = TimeSpan.FromSeconds(4),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(translateAnimationX, translateTransform);
            Storyboard.SetTargetProperty(translateAnimationX, new PropertyPath(TranslateTransform.XProperty));

            DoubleAnimation translateAnimationY = new DoubleAnimation
            {
                From = -50,
                To = 50,
                Duration = TimeSpan.FromSeconds(4),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(translateAnimationY, translateTransform);
            Storyboard.SetTargetProperty(translateAnimationY, new PropertyPath(TranslateTransform.YProperty));

            // Дополнительная анимация масштабирования "волноподобная"
            DoubleAnimation waveScaleAnimationX = new DoubleAnimation
            {
                From = baseScale,
                To = baseScale + 0.3,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(waveScaleAnimationX, scaleTransform);
            Storyboard.SetTargetProperty(waveScaleAnimationX, new PropertyPath(ScaleTransform.ScaleXProperty));

            DoubleAnimation waveScaleAnimationY = new DoubleAnimation
            {
                From = baseScale,
                To = baseScale + 0.3,
                Duration = TimeSpan.FromSeconds(3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(waveScaleAnimationY, scaleTransform);
            Storyboard.SetTargetProperty(waveScaleAnimationY, new PropertyPath(ScaleTransform.ScaleYProperty));

            // Комбинирование анимаций в сториборд
            waveAnimations[i] = new Storyboard();
            waveAnimations[i].Children.Add(scaleAnimationX);
            waveAnimations[i].Children.Add(scaleAnimationY);
            waveAnimations[i].Children.Add(rotateAnimation);
            waveAnimations[i].Children.Add(translateAnimationX);
            waveAnimations[i].Children.Add(translateAnimationY);
            waveAnimations[i].Children.Add(waveScaleAnimationX);
            waveAnimations[i].Children.Add(waveScaleAnimationY);

            waveAnimations[i].Begin();
        }
    }

    private void OnDataAvailable(object sender, WaveInEventArgs e)
    {
        float maxVolume = 0;
        for (int index = 0; index < e.BytesRecorded; index += 2)
        {
            short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
            var volume = Math.Abs(sample / 32768f);
            if (volume > maxVolume)
                maxVolume = volume;
        }

        Dispatcher.BeginInvoke(() => AdjustCircleScales(maxVolume));
    }

    private void AdjustCircleScales(float volume)
    {
        isQuietMode = volume < 0.001;

        double scale = baseScale + (volume * (maxScale - baseScale));

        for (int i = 0; i < 5; i++)
        {
            double minScaleFactor = isQuietMode ? 0.1 : 0.5;
            double randomScaleX = baseScale + random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor;
            double randomScaleY = baseScale + random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor;

            DoubleAnimation scaleAnimationX = new DoubleAnimation
            {
                To = randomScaleX * scale,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            scaleTransforms[i].BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimationX);

            DoubleAnimation scaleAnimationY = new DoubleAnimation
            {
                To = randomScaleY * scale,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            scaleTransforms[i].BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimationY);

            DoubleAnimation translateAnimationX = new DoubleAnimation
            {
                To = random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor * 100,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            translateTransforms[i].BeginAnimation(TranslateTransform.XProperty, translateAnimationX);

            DoubleAnimation translateAnimationY = new DoubleAnimation
            {
                To = random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor * 100,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            translateTransforms[i].BeginAnimation(TranslateTransform.YProperty, translateAnimationY);
        }
    }
}
