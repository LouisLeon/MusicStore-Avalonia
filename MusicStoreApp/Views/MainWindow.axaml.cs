using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using MusicStoreApp.Messages;
using MusicStoreApp.ViewModels;

namespace MusicStoreApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            return;
        }
        
        WeakReferenceMessenger.Default.Register<MainWindow, PurchaseAlbumMessage>(this, static (w, m) =>
        {
            MusicStoreWindow dialog = new MusicStoreWindow()
            {
                DataContext = new MusicStoreViewModel()
            };
            
            m.Reply(dialog.ShowDialog<AlbumViewModel?>(w));
        });
    }
}