using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.Ideas.App.Presentation.Views.Controls;

public partial class CustomTitleView : StackLayout
{
    public static BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(CustomTitleView));

    public static BindableProperty MenuIconCommandProperty = BindableProperty.Create(
        nameof(MenuIconCommand),
        typeof(ICommand),
        typeof(CustomTitleView));

    public ICommand MenuIconCommand
    {
        get => (ICommand)GetValue(MenuIconCommandProperty);
        set => SetValue(MenuIconCommandProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public CustomTitleView()
    {
        InitializeComponent();
        titleLabel.Text = Title;
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (nameof(Title).Equals(propertyName))
        {
            titleLabel.Text = Title;
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        MenuIconCommand?.Execute(this);
    }
}