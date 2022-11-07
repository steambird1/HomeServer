'------------------------------------------------------------------------------
' <自动生成>
'     此代码由工具生成。
'
'     对此文件的更改可能会导致不正确的行为，并且如果
'     重新生成代码，这些更改将会丢失。 
' </自动生成>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class FileExplore

    '''<summary>
    '''form1 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents form1 As Global.System.Web.UI.HtmlControls.HtmlForm

    '''<summary>
    '''ScriptManagerMain 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ScriptManagerMain As Global.System.Web.UI.ScriptManager

    '''<summary>
    '''FileModifiers 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FileModifiers As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''ErrorDisplay 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ErrorDisplay As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''FreeFSpace 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FreeFSpace As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''FreeDSpace 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FreeDSpace As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''PrevDirs 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents PrevDirs As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''CurrentPosition 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents CurrentPosition As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''GotoPosition 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents GotoPosition As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''EnterDirectory 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnterDirectory As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''CreateFileHelper 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents CreateFileHelper As Global.System.Web.UI.WebControls.FileUpload

    '''<summary>
    '''CreateFile 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents CreateFile As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''DeleteFile 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents DeleteFile As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''CreateCopy 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents CreateCopy As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''RenameFile 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents RenameFile As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''DownloadFiles 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents DownloadFiles As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''CreateDirectory 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents CreateDirectory As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''UpdateProgressPanel 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents UpdateProgressPanel As Global.System.Web.UI.UpdateProgress

    '''<summary>
    '''FileRenamePanel 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FileRenamePanel As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''OriginalFilename 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents OriginalFilename As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''NewFilename 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents NewFilename As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''ConfirmRename 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ConfirmRename As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''AbandonRename 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents AbandonRename As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''DirectoryCreater 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents DirectoryCreater As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''NewDirectoryName 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents NewDirectoryName As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''ConfirmCreater 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ConfirmCreater As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''AbandonCreater 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents AbandonCreater As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''ConfirmDeletion 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ConfirmDeletion As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''ConfirmDelete 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ConfirmDelete As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''AbandonDelete 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents AbandonDelete As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''SelectAll 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SelectAll As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''SelectAllVert 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SelectAllVert As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''FileViewers 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FileViewers As Global.System.Web.UI.WebControls.CheckBoxList

    '''<summary>
    '''FileProperty 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents FileProperty As Global.System.Web.UI.WebControls.Label
End Class
