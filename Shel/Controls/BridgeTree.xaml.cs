using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shel.Controls
{
    /// <summary>
    /// Interaction logic for BridgeTree.xaml
    /// </summary>
    public partial class BridgeTree : UserControl
    {
        static BridgeTree()
        {
            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(BridgeTree),
                new FrameworkPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnColorChanged)));
            RedProperty = DependencyProperty.Register("Red", typeof(Byte), typeof(BridgeTree),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRgbChanged)));
            GreenProperty = DependencyProperty.Register("Green", typeof(Byte), typeof(BridgeTree),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRgbChanged)));
            BlueProperty = DependencyProperty.Register("Blue", typeof(Byte), typeof(BridgeTree),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRgbChanged)));
            ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(BridgeTree));
        }

        public static DependencyProperty ColorProperty;
        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Byte Red
        {
            get { return (Byte) GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public Byte Green
        {
            get { return (Byte) GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public Byte Blue
        {
            get { return (Byte) GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        private static void OnColorRgbChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (BridgeTree) sender;
            var color = colorPicker.Color;
            if (e.Property == RedProperty)
                color.R = (Byte) e.NewValue;
            else if (e.Property == GreenProperty)
                color.G = (Byte) e.NewValue;
            else if (e.Property == BlueProperty)
                color.B = (Byte) e.NewValue;

            colorPicker.Color = color;
        }

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (Color) e.NewValue;
            var colorpicker = (BridgeTree) sender;
            colorpicker.Red = newColor.R;
            colorpicker.Green = newColor.G;
            colorpicker.Blue = newColor.B;
        }

        public static readonly RoutedEvent ColorChangedEvent;

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }
        public BridgeTree()
        {
            InitializeComponent();
        }
    }
}
