using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Maui.Ideas.App.Presentation.Views.Controls;

public partial class LazyImage : ContentView
{

    public static BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(LazyImage));

    public static BindableProperty AspectProperty = BindableProperty.Create(
        nameof(Aspect),
        typeof(Aspect),
        typeof(LazyImage));

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public Aspect Aspect
    {
        get => (Aspect)GetValue(AspectProperty);
        set => SetValue(AspectProperty, value);
    }

    public LazyImage()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (nameof(Source).Equals(propertyName))
        {
            _ = Task.Run(() => StartDownloadImageAsync(Source));
        }
    }

    private async Task StartDownloadImageAsync(string source)
    {
        try
        {
            var hash = GenerateHash(source);
            var cacheDir = Path.Combine(FileSystem.Current.CacheDirectory, "images");
            var finalPath = Path.Combine(cacheDir, hash);

            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            if (!File.Exists(finalPath))
            {
                using (var client = new HttpClient())
                {
                    var resultStream = await client.GetStreamAsync(source).ConfigureAwait(false);

                    using (var outputFileStream = new FileStream(finalPath, FileMode.CreateNew))
                    {
                        await resultStream.CopyToAsync(outputFileStream).ConfigureAwait(false);
                    }
                }
            }

            var imageSource = ImageSource.FromFile(finalPath);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                imageView.Source = imageSource;
                imageView.WidthRequest = WidthRequest;
                imageView.HeightRequest = HeightRequest;
                imageView.Aspect = Aspect;

                _ = loader.FadeTo(0);
                _ = imageView.FadeTo(1);
            });
        }
        catch (Exception ex)
        {
            var teste = ex;
        }

    }

    public string GenerateHash(string text)
    {
        using (var sha = SHA256.Create())
        {
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

            var hexChars = bytes
                .Select(b => b.ToString("X2"));

            return string.Concat(hexChars);
        }
    }
}