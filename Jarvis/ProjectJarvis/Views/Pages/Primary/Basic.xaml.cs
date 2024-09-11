using System;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using log4net;
using NAudio.Wave;

namespace Jarvis.Project.Views.Pages.Primary;

public partial class Basic : Page
{
    private const string ApiKey = "e327b05bf5e93f0560363f8bc007f7e2";
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private readonly HttpClient http = new();
    private readonly double baseScale = 0.9;
    private bool isQuietMode;
    private readonly double maxScale = 2.3;
    private readonly double quietModeScaleFactor = 0.09;
    private readonly Random random;
    private RotateTransform[] rotateTransforms;
    private ScaleTransform[] scaleTransforms;
    private TranslateTransform[] translateTransforms;
    private Storyboard[] waveAnimations;
    private WaveInEvent waveIn;

    public Basic()
    {
        InitializeComponent();
        InitializeMicrophone();
        InitializeWaveAnimation();
        random = new Random();
        Instance = this;
    }

    public static Basic Instance { get; private set; }

    // Метод для добавления ответа Джарвиса
    public void AddJarvisAnswer(string answer)
    {
        Dispatcher.Invoke(() =>
        {
            var answerContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 10, 0, 10) // Увеличенные вертикальные отступы
            };

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
            // Создаем контейнер для запроса с выравниванием справа
            var requestContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            // Добавляем текст запроса
            var requestTextBlock = new TextBlock
            {
                Text = request,
                Foreground = Brushes.White,
                FontSize = 16
            };
            requestContainer.Children.Add(requestTextBlock);

            // Добавляем белый кружок
            var whiteCircle = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.White,
                Margin = new Thickness(5, 0, 0, 0) // Отступ слева для расстояния между текстом и кружком
            };
            requestContainer.Children.Add(whiteCircle);

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

        for (var i = 0; i < 5; i++)
        {
            var circle = (Ellipse)FindName($"circle{i + 1}");

            var gradientBrush = new RadialGradientBrush();
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 40, 215), 0.1));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 100, 255), 0.5));


            circle.Fill = gradientBrush;

            var scaleTransform = new ScaleTransform();
            var rotateTransform = new RotateTransform();
            var translateTransform = new TranslateTransform();

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(translateTransform);

            circle.RenderTransform = transformGroup;
            circle.RenderTransformOrigin = new Point(0.7, 0.7);
            scaleTransforms[i] = scaleTransform;
            rotateTransforms[i] = rotateTransform;
            translateTransforms[i] = translateTransform;

            // Основная анимация масштабирования
            var scaleAnimationX = new DoubleAnimation
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

            var scaleAnimationY = new DoubleAnimation
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
            var rotateAnimation = new DoubleAnimation
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
            var translateAnimationX = new DoubleAnimation
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

            var translateAnimationY = new DoubleAnimation
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
            var waveScaleAnimationX = new DoubleAnimation
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

            var waveScaleAnimationY = new DoubleAnimation
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
        for (var index = 0; index < e.BytesRecorded; index += 2)
        {
            var sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
            var volume = Math.Abs(sample / 32768f);
            if (volume > maxVolume)
                maxVolume = volume;
        }

        Dispatcher.BeginInvoke(() => AdjustCircleScales(maxVolume));
    }

    private void AdjustCircleScales(float volume)
    {
        isQuietMode = volume < 0.001;

        var scale = baseScale + volume * (maxScale - baseScale);

        for (var i = 0; i < 5; i++)
        {
            var minScaleFactor = isQuietMode ? 0.1 : 0.5;
            var randomScaleX =
                baseScale + random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor;
            var randomScaleY =
                baseScale + random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor;

            var scaleAnimationX = new DoubleAnimation
            {
                To = randomScaleX * scale,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            scaleTransforms[i].BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimationX);

            var scaleAnimationY = new DoubleAnimation
            {
                To = randomScaleY * scale,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            scaleTransforms[i].BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimationY);

            var translateAnimationX = new DoubleAnimation
            {
                To = random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor * 100,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            translateTransforms[i].BeginAnimation(TranslateTransform.XProperty, translateAnimationX);

            var translateAnimationY = new DoubleAnimation
            {
                To = random.NextDouble() * (isQuietMode ? quietModeScaleFactor : 0.4) * minScaleFactor * 100,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            translateTransforms[i].BeginAnimation(TranslateTransform.YProperty, translateAnimationY);
        }
    }
}