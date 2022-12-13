Imports System.Web.UI.WebControls
Imports System.IO.Compression

Public Class FileExplore
    Inherits System.Web.UI.Page

    Public Const FileRoot As String = "F:\DiskEntry"
    Public rseed As Random = New Random

    ''' <summary>
    ''' 目录结尾包含斜杠。
    ''' </summary>
    ''' <value>当前环境目录。</value>
    Public ReadOnly Property CurrentPath As String
        Get
            Return FileRoot & CurrentPosition.Text
        End Get
    End Property

    ''' <summary>
    ''' 部分服务依赖于 Server.MapPath，请勿更改。
    ''' 在服务器根目录下找到下一个有效文件
    ''' </summary>
    ''' <param name="fix">文件的后缀名。</param>
    ''' <returns>文件路径。</returns>
    Protected Function FindFreefile(fix As String) As String
        Dim r As String = ""
        Do
            r = Server.MapPath(rseed.Next() & "_" & fix)
            If Not My.Computer.FileSystem.FileExists(r) Then Exit Do
        Loop
        Return r
    End Function

    Protected Function FindFreeUnder(Prefix As String, Suffix As String, Path As String) As String
        Dim r As Integer = 1
        With My.Computer.FileSystem
            While .FileExists(Path & Prefix & r & Suffix) OrElse .DirectoryExists(Path & Prefix & r & Suffix)
                r += 1
            End While
        End With
        Return Path & Prefix & r & Suffix
    End Function


    Protected Function FindFreeDirectory(fix As String) As String
        Dim r As String = ""
        Do
            r = Server.MapPath(rseed.Next() & "_" & fix)
            If Not My.Computer.FileSystem.DirectoryExists(r) Then Exit Do
        Loop
        Return r
    End Function

    Private Sub ReportError(Info As String)
        'Response.Write("<script>alert('" & Info & "');</script>")
        ErrorDisplay.Visible = True
        ErrorDisplay.Text = Format(Now, "hh:mm:ss") & " " & Info
    End Sub

    Protected Sub UpdateDiskInfo()
        With My.Computer.FileSystem
            For Each i In .Drives
                If i.Name = "D:\" Then
                    FreeDSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                ElseIf i.Name = "F:\" Then
                    FreeFSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                End If
            Next
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' 第一次显示!

            Dim Queryer As String = HttpContext.Current.Request.Url.Query
            If Queryer.Length > 0 AndAlso Queryer(0) = "?"c Then
                Queryer = Queryer.Substring(1)
            End If
            Dim Queries() As String = Split(Queryer, "&")
            For Each i In Queries
                Dim Para() As String = Split(i, "=", 2)
                If Para.Count() < 2 Then
                    Continue For
                End If
                Select Case Para(0)
                    Case "path"
                        CurrentPosition.Text = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(Para(1)))
                End Select
            Next
            RefreshPosition()
        End If
        ' 无论如何都加载大小信息
        
    End Sub

    Protected Sub PrevDirs_Click(sender As Object, e As EventArgs) Handles PrevDirs.Click
        Dim SplitPosition = CurrentPosition.Text.LastIndexOf("/"c)
        SplitPosition = CurrentPosition.Text.Substring(0, SplitPosition).LastIndexOf("/"c)
        If SplitPosition < 0 Then
            ReportError("已经在根目录!")
        Else
            CurrentPosition.Text = CurrentPosition.Text.Substring(0, SplitPosition) & "/"
            RefreshPosition()
        End If
    End Sub

    Private Function GenerateDownloadName(ShownData As String, Fullpath As String) As String
        Return ShownData
    End Function

    Private Function GetExtension(Path As String) As String
        Dim SplitPosition = Path.LastIndexOf(".")
        If SplitPosition < 0 OrElse SplitPosition >= Path.Length Then
            Return ""
        Else
            Return Path.Substring(SplitPosition)
        End If
    End Function

    ''' <summary>
    ''' 从提供的路径中得到文件名 (可以作为 <code>My.Computer.FileSystem.GetName</code> 别名)。
    ''' </summary>
    ''' <param name="Path">文件的路径。</param>
    ''' <returns>文件名。</returns>
    Private Function GetFilename(Path As String) As String
        Return My.Computer.FileSystem.GetName(Path)
    End Function

    Private Sub RefreshPosition()
        If Not My.Computer.FileSystem.DirectoryExists(CurrentPath) Then
            ReportError("路径 " & CurrentPath & " 不存在！")
            CurrentPosition.Text = "/"
        End If
        FileViewers.Items.Clear()
        With My.Computer.FileSystem
            For Each it In .GetDirectories(CurrentPath)
                Dim i As String = GetFilename(it)
                Dim GeneratedDirectoryName As String = GenerateDownloadName("[目录] " & i, it)
                Dim Targets As String = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(CurrentPosition.Text & i & "/"))
                FileViewers.Items.Add(New ListItem("<a href=""" & Request.Path & "?path=" & Targets & """>" & GeneratedDirectoryName & "</a>", i))
            Next
            For Each it In .GetFiles(CurrentPath)
                Dim i As String = GetFilename(it)
                FileViewers.Items.Add(New ListItem(GenerateDownloadName(i, CurrentPath & it), i))
            Next
        End With
        FileProperty.Text = "选择一个文件，查看其具体属性。"
        UpdateDiskInfo()
    End Sub

    Protected Sub GotoPosition_Click(sender As Object, e As EventArgs) Handles GotoPosition.Click
        RefreshPosition()
    End Sub

    Private Function Rounder(Value As Double) As Double
        Return Int(Value * 100) / 100
    End Function

    ''' <summary>
    ''' 获取文件大小的合适表示方式。
    ''' </summary>
    ''' <param name="Value">MB 为大小的文件大小。</param>
    ''' <returns>合适的字符串表示。</returns>
    ''' 
    Private Function GetSpaceInfo(Value As Double) As String
        If Value < 1 Then
            If Value * 1024 < 1 Then
                Return "少于 1 KB"
            Else
                Return Rounder(Value * 1024) & " KB"
            End If
        ElseIf Value > 1 AndAlso Value < 1024 Then
            Return Rounder(Value) & " MB"
        Else
            Return Rounder(Value / 1024) & " GB"
        End If
    End Function

    Protected Sub UpdateFileInfo(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing) Handles FileViewers.SelectedIndexChanged
        Dim SumSize As Double = 0         ' KB
        Dim Visited As Boolean = False
        FileProperty.Text = ""
        With My.Computer.FileSystem
            For Each it In FileViewers.Items
                Dim i As ListItem = it
                If i.Selected Then
                    Visited = True
                    Dim Selects As String = CurrentPath & i.Value
                    If .DirectoryExists(Selects) Then
                        Dim Desc = .GetDirectoryInfo(Selects)
                        FileProperty.Text &= "文件夹: " & i.Value & "<br />" & "创建日期: " & Desc.CreationTime.ToString() & "<br />" & "<br />"
                    ElseIf .FileExists(Selects) Then
                        Dim Desc = .GetFileInfo(Selects)
                        SumSize += Desc.Length / 1024 / 1024
                        FileProperty.Text &= "文件: " & i.Value & "<br />" & "创建日期: " & Desc.CreationTime.ToString() & "<br />" & "大小: " & GetSpaceInfo(Desc.Length / 1024 / 1024) & "<br />" & "<br />"
                    End If
                End If
            Next
        End With
        FileProperty.Text &= "所有文件总大小: " & GetSpaceInfo(SumSize)
        If Not Visited Then
            FileProperty.Text = "选择一个文件，查看其具体属性。"
        End If
    End Sub

    Protected Sub EnterDirectory_Click(sender As Object, e As EventArgs) Handles EnterDirectory.Click
        If FileViewers.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim Selects As String = CurrentPath & FileViewers.SelectedItem.Value
        With My.Computer.FileSystem
            If .DirectoryExists(Selects) Then
                CurrentPosition.Text &= FileViewers.SelectedItem.Value & "/"
                RefreshPosition()
            End If
        End With
    End Sub

    Protected Sub CreateFile_Click(sender As Object, e As EventArgs) Handles CreateFile.Click
        If Not CreateFileHelper.HasFile Then
            Exit Sub
        End If
        For Each i In CreateFileHelper.PostedFiles
            i.SaveAs(CurrentPath & i.FileName)
            RefreshPosition()
        Next
    End Sub

    Protected Sub DeleteFile_Click(sender As Object, e As EventArgs) Handles DeleteFile.Click
        ConfirmDeletion.Visible = True
    End Sub

    Protected Sub CreateCopy_Click(sender As Object, e As EventArgs) Handles CreateCopy.Click
        If FileViewers.SelectedIndex < 0 Then
            Exit Sub
        End If
        For Each it In FileViewers.Items
            Dim i As ListItem = it
            If i.Selected Then
                Dim Selects As String = CurrentPath & i.Value
                With My.Computer.FileSystem
                    If .DirectoryExists(Selects) Then
                        .CopyDirectory(Selects, FindFreeUnder("复件 ", GetExtension(i.Value), CurrentPath))
                    ElseIf .FileExists(Selects) Then
                        .CopyFile(Selects, FindFreeUnder("复件 ", GetExtension(i.Value), CurrentPath))
                    Else
                        ReportError("无法复制文件:指定的文件不存在!")
                        Exit Sub
                    End If
                End With
            End If
        Next
        RefreshPosition()
    End Sub

    Protected Sub AbandonRename_Click(sender As Object, e As EventArgs) Handles AbandonRename.Click
        FileRenamePanel.Visible = False
    End Sub

    Protected Sub ConfirmRename_Click(sender As Object, e As EventArgs) Handles ConfirmRename.Click
        Dim Selects As String = CurrentPath & OriginalFilename.Text
        With My.Computer.FileSystem
            If .DirectoryExists(Selects) Then
                .RenameDirectory(Selects, NewFilename.Text)
            ElseIf .FileExists(Selects) Then
                .RenameFile(Selects, NewFilename.Text)
            Else
                ReportError("无法重命名文件:指定的文件不存在!")
                Exit Sub
            End If
        End With
        RefreshPosition()
        FileRenamePanel.Visible = False
    End Sub

    Protected Sub RenameFile_Click(sender As Object, e As EventArgs) Handles RenameFile.Click
        If FileViewers.SelectedIndex < 0 Then
            Exit Sub
        End If
        OriginalFilename.Text = FileViewers.SelectedItem.Value
        FileRenamePanel.Visible = True
    End Sub

    Protected Sub DownloadFiles_Click(sender As Object, e As EventArgs) Handles DownloadFiles.Click
        ' 为所有下载文件创建 zip。
        Dim ZIPTarget As String = FindFreefile(".zip")
        Dim SourceTarget As String = FindFreeDirectory("_filexplore") & "\"
        For Each it In FileViewers.Items
            Dim i As ListItem = it
            If i.Selected Then
                Dim Selects As String = CurrentPath & i.Value
                With My.Computer.FileSystem
                    If .DirectoryExists(Selects) Then
                        .CopyDirectory(Selects, SourceTarget & i.Value)
                    ElseIf .FileExists(Selects) Then
                        .CopyFile(Selects, SourceTarget & i.Value)
                    End If
                End With
            End If
        Next
        ZipFile.CreateFromDirectory(SourceTarget, ZIPTarget)
        My.Computer.FileSystem.DeleteDirectory(SourceTarget, FileIO.DeleteDirectoryOption.DeleteAllContents)
        'Response.ContentType = "application/x-zip-compressed"
        'Response.AddHeader("Content-Disposition", "attachment;filename=Download.zip")
        'Response.TransmitFile(ZIPTarget)
        Dim Visitable As String = My.Computer.FileSystem.GetName(ZIPTarget)
        UpdateDiskInfo()
        ReportError("创建成功，点击<a href=""" & Visitable & """ download=""" & Visitable & """>此处</a>下载")
    End Sub

    Protected Sub CreateDirectory_Click(sender As Object, e As EventArgs) Handles CreateDirectory.Click
        DirectoryCreater.Visible = True
    End Sub

    Protected Sub AbandonCreater_Click(sender As Object, e As EventArgs) Handles AbandonCreater.Click
        DirectoryCreater.Visible = False
    End Sub

    Protected Sub ConfirmCreater_Click(sender As Object, e As EventArgs) Handles ConfirmCreater.Click
        With My.Computer.FileSystem
            .CreateDirectory(CurrentPath & NewDirectoryName.Text)
        End With
        DirectoryCreater.Visible = False
        RefreshPosition()
    End Sub

    Protected Sub SelectAll_Click(sender As Object, e As EventArgs) Handles SelectAll.Click
        For Each i In FileViewers.Items
            Dim it As ListItem = i
            it.Selected = True
        Next
        UpdateFileInfo()
    End Sub

    Protected Sub SelectAllVert_Click(sender As Object, e As EventArgs) Handles SelectAllVert.Click
        For Each i In FileViewers.Items
            Dim it As ListItem = i
            it.Selected = False
        Next
        FileProperty.Text = "选择一个文件，查看其具体属性。"
    End Sub

    Protected Sub AbandonDelete_Click(sender As Object, e As EventArgs) Handles AbandonDelete.Click
        ConfirmDeletion.Visible = False
    End Sub

    Protected Sub ConfirmDelete_Click(sender As Object, e As EventArgs) Handles ConfirmDelete.Click
        If FileViewers.SelectedIndex < 0 Then
            Exit Sub
        End If
        For Each it In FileViewers.Items
            Dim i As ListItem = it
            If i.Selected Then
                Dim Selects As String = CurrentPath & i.Value
                With My.Computer.FileSystem
                    If .DirectoryExists(Selects) Then
                        .DeleteDirectory(Selects, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    ElseIf .FileExists(Selects) Then
                        .DeleteFile(Selects)
                    Else
                        ReportError("无法删除文件:文件" & i.Value & "不存在!")
                        Exit Sub
                    End If
                End With
            End If
        Next
        ReportError("删除成功!")
        RefreshPosition()
        ConfirmDeletion.Visible = False
    End Sub

    Protected Sub FileMover_Click(sender As Object, e As EventArgs) Handles FileMover.Click
        MovePosition.Text = CurrentPosition.Text
        SetMoving.Visible = True
    End Sub

    Protected Sub AbandonMoving_Click(sender As Object, e As EventArgs) Handles AbandonMoving.Click
        SetMoving.Visible = False
    End Sub

    Protected Sub ConfirmMoving_Click(sender As Object, e As EventArgs) Handles ConfirmMoving.Click
        If FileViewers.SelectedIndex < 0 Then
            Exit Sub
        End If
        If MovePosition.Text.Length <= 0 OrElse MovePosition.Text(MovePosition.Text.Length - 1) <> "/"c OrElse (Not My.Computer.FileSystem.DirectoryExists(FileRoot & MovePosition.Text)) Then
            ReportError("不是有效的目录名: " & MovePosition.Text & "。目录名必须以 ""/"" 结尾并存在！")
            Exit Sub
        End If
        Dim succeed As Boolean = True
        Try
            For Each it In FileViewers.Items
                Dim i As ListItem = it
                If i.Selected Then
                    Dim Selects As String = CurrentPath & i.Value
                    With My.Computer.FileSystem
                        If .DirectoryExists(Selects) Then
                            .MoveDirectory(Selects, FileRoot & MovePosition.Text & i.Value)
                        ElseIf .FileExists(Selects) Then
                            .MoveFile(Selects, FileRoot & MovePosition.Text & i.Value)
                        Else
                            ReportError("无法移动文件:文件" & i.Value & "不存在!")
                            Exit Sub
                        End If
                    End With
                End If
            Next
        Catch ex As Exception
            ReportError("无法移动文件：" & ex.Message)
            succeed = False
        End Try
        If succeed Then
            ReportError("移动成功!")
        End If
        RefreshPosition()
        SetMoving.Visible = False
    End Sub
End Class