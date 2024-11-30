using CommunityToolkit.Maui.Views;

namespace GameDatabaseEditor.Views;

public partial class GenericEditorPopup : Popup
{
    private EditorModes _editorMode;
    private EditorTypes _editorType;

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

    public EditorModes EditorMode
    {
        set
        {
            _editorMode = value;
            lblTitle.Text = BuildPopupHeader();
        }
    }
    public EditorTypes EditorType
    {
        set
        {
            _editorType = value;
            lblTitle.Text = BuildPopupHeader();
        }
    }

    public GenericEditorPopup()
    {
        InitializeComponent();
    }

    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Close(false);
    }
    private void btnSave_Clicked(object sender, EventArgs e)
    {
        Close(true);
    }

    private string BuildPopupHeader()
    {
        string editorType = string.Empty;
        string editorMode = string.Empty;

        switch (_editorType)
        {
            case EditorTypes.Room:
                editorType = _editorType.ToString();
                break;

            default:
                editorType = "Unset";
                break;
        }

        switch (_editorMode)
        {
            case EditorModes.Add:
                editorMode = _editorMode.ToString();
                break;

            case EditorModes.Edit:
                editorMode = _editorMode.ToString();
                break;

            default:
                editorMode = "Unset";
                break;
        }

        return $"{editorMode} {editorType}";
    }
}