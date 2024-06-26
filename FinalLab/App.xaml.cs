﻿using System.Windows;
using FinalLab.Properties;

namespace FinalLab;

public partial class App : Application
{
    private static string _theme;

    public App()
    {
        InitializeComponent();
        Theme = Settings.Default.Theme;
    }

    public static string Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            var dick = new ResourceDictionary
                { Source = new Uri($"pack://application:,,,/Themes;component/{value}.xaml", UriKind.Absolute) };
            Current.Resources.MergedDictionaries.RemoveAt(0);
            Current.Resources.MergedDictionaries.Insert(0, dick);

            Settings.Default.Theme = value;
            Settings.Default.Save();
        }
    }
}