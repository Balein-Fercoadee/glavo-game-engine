using CommunityToolkit.Maui.Views;

namespace GameDatabaseEditor.Views;

/// <summary>
/// Hosts an editor view with Save and Cancel actions.
/// </summary>
public partial class GenericEditorPopup : Popup
{
    private EditorModes _editorMode;
    private EditorTypes _editorType;

    public Func<bool>? CanSave { get; set; }

    /// <summary>
    /// Adds an editor view to the popup.
    /// </summary>
    public ContentView EditorView
    {
        set
        {
            homeGrid.AddWithSpan(value, 1, 0, 1, 3);
        }
    }

    /// <summary>
    /// Sets the editing mode and updates the popup title.
    /// </summary>
    public EditorModes EditorMode
    {
        set
        {
            _editorMode = value;
            lblTitle.Text = BuildPopupHeader();
        }
    }
    /// <summary>
    /// Sets the type of object being edited and updates the popup title.
    /// </summary>
    public EditorTypes EditorType
    {
        set
        {
            _editorType = value;
            lblTitle.Text = BuildPopupHeader();
        }
    }

    /// <summary>
    /// Initializes a new editor popup.
    /// </summary>
    public GenericEditorPopup()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Closes the popup with a false result to discard the edit.
    /// </summary>
    private void btnCancel_Clicked(object sender, EventArgs e)
    {
        Close(false);
    }
    /// <summary>
    /// Closes the popup with a true result to accept the edit.
    /// </summary>
    private void btnSave_Clicked(object sender, EventArgs e)
    {
        if (CanSave is not null && !CanSave())
        {
            return;
        }

        Close(true);
    }

    /// <summary>
    /// Builds the title from the editor mode and type, using "Unset" for unsupported values.
    /// </summary>
    /// <returns>The popup title.</returns>
    private string BuildPopupHeader()
    {
        string editorType = string.Empty;
        string editorMode = string.Empty;

        switch (_editorType)
        {
            case EditorTypes.Room:
            case EditorTypes.Item:
            case EditorTypes.Message:
            case EditorTypes.Word:
            case EditorTypes.Action:
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