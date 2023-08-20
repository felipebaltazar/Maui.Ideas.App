namespace Maui.Ideas.App.Resources.Styles;

public partial class ApplicationStyles : ResourceDictionary
{
    public ApplicationStyles(params ResourceDictionary[] merged)
    {
        foreach(var inner in merged)
            MergedDictionaries.Add(inner);

        InitializeComponent();
    }

    public ApplicationStyles()
    {
        InitializeComponent();
    }
}
