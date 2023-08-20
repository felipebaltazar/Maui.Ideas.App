using Maui.Ideas.App.Presentation.Views.Effects;
using System.Collections;
using System.Windows.Input;

namespace Maui.Ideas.App.Presentation.Views.Controls;

/// <summary>
/// A basic idea for a segmented control
/// </summary>
public partial class SegmentedControl : Frame
{
    #region Bindable Properties

    public static BindableProperty SegmentSelectedCommandProperty = BindableProperty.Create(
   nameof(SegmentSelectedCommand),
   typeof(ICommand),
   typeof(SegmentedControl));

    public static BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(SegmentedControl));

    #endregion

    #region Properties

    public ICommand SegmentSelectedCommand
    {
        get => (ICommand)GetValue(SegmentSelectedCommandProperty);
        set => SetValue(SegmentSelectedCommandProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty) ?? Enumerable.Empty<string>();
        set
        {
            SetValue(ItemsSourceProperty, value);
            BindableLayout.SetItemsSource(container, value);
        }
    }

    #endregion

    #region Constructor

    public SegmentedControl()
    {
        InitializeComponent();
        container.ChildAdded += Container_ChildAdded;
    }

    #endregion

    #region Private Methods

    private void Container_ChildAdded(object sender, ElementEventArgs e)
    {
        container.ChildAdded -= Container_ChildAdded;

        var firstElement = container.FirstOrDefault();
        if (firstElement is Grid grid)
            SetSelectionStyle(grid, true);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var canExecute = SegmentSelectedCommand?.CanExecute("") ?? true;

        if (!canExecute)
            return;

        var selectedInfo = string.Empty;
        foreach (var item in container)
        {
            if (item is Grid grid)
            {
                var isSelected = sender == item;
                if (isSelected)
                {
                    var child = grid.Children.FirstOrDefault(c => c is Label);
                    if (child is Label label)
                    {
                        selectedInfo = label.Text;
                    }
                }

                SetSelectionStyle(grid, isSelected);
            }
        }

        SegmentSelectedCommand?.Execute(selectedInfo);
    }

    private static void SetSelectionStyle(Grid grid, bool isSelected = false)
    {
        if (isSelected)
            grid.Effects.Add(new ShadowRoutingEffect());
        else
            grid.Effects.Clear();

        var background = grid.Children.FirstOrDefault(c => c is BoxView) as BoxView;
        if (background is BoxView backgroundBoxView)
            backgroundBoxView.IsVisible = isSelected;
    }
    
    #endregion
}