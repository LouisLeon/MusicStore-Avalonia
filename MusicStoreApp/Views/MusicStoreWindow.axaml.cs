using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using MusicStoreApp.Messages;

namespace MusicStoreApp.Views;

public partial class MusicStoreWindow : Window
{
    public MusicStoreWindow()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<MusicStoreWindow, MusicStoreClosedMessage>
            (this, static (w, m) => w.Close(m.SelectedAlbum));

        WeakReferenceMessenger.Default.Register<MusicStoreWindow, NotificationMessage>(this, static (w, m) =>
        {
            w.NotificationManager.CloseAll();
            w.NotificationManager.Show(m.Message, NotificationType.Warning, TimeSpan.FromSeconds(3));
        });
    }
}