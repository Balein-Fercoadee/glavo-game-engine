using CommunityToolkit.Maui.Views;
namespace GameDatabaseEditor.Views;

public partial class GenericEditorPopup : Popup
{
	/// <summary>
	/// Sets the editor view for the popup
	/// </summary>
	public ContentView EditorView
	{
		set
		{
			// Add the view to the second row, across all columns
			homeGrid.AddWithSpan(value, 1, 0, 1, 3);
		}
	}

	public GenericEditorPopup()
	{
		InitializeComponent();
	}

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}