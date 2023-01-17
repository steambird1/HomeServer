Imports System.IO
Imports System.IO.Compression

Public Class DocumentPreview
    Inherits System.Web.UI.Page

    Public Const FileRoot As String = "F:\DiskEntry"
    Public ViewMode As Hashtable = New Hashtable
    Public AcceptablePictureTypes As HashSet(Of String) = New HashSet(Of String)
    Public AcceptablePrintTypes As HashSet(Of String) = New HashSet(Of String)

    Public Const wdFormatHTML As Integer = 8
    Public Const xlHtml As Integer = 44

    Public CurrentPath As String = ""
    Protected rseed As Random = New Random

    Public Function GetExt(filename As String) As String
        Dim d As List(Of String) = New List(Of String)(Split(filename, "."))
        Return d(d.Count - 1)
    End Function

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

    Protected Sub LoadHash()
        AcceptablePictureTypes.Add(".jpg")
        AcceptablePictureTypes.Add(".jpeg")
        AcceptablePictureTypes.Add(".png")
        AcceptablePictureTypes.Add(".bmp")
        AcceptablePictureTypes.Add(".gif")
        AcceptablePrintTypes.Add(".doc")
        AcceptablePrintTypes.Add(".docx")
        AcceptablePrintTypes.Add(".xls")
        AcceptablePrintTypes.Add(".xlsx")
        AcceptablePrintTypes.Add(".txt")
        AcceptablePrintTypes.Add(".pdf")
        For Each i In AcceptablePictureTypes
            AcceptablePrintTypes.Add(i)
        Next
        ViewMode.Add("txt", 0)
        ViewMode.Add("log", 0)
        ViewMode.Add("html", 1)
        ViewMode.Add("htm", 1)
        ViewMode.Add("jpg", 2)
        ViewMode.Add("png", 2)
        ViewMode.Add("gif", 2)
        ViewMode.Add("jpeg", 2)
        ViewMode.Add("doc", 3)
        ViewMode.Add("docx", 3)
        ViewMode.Add("xls", 4)
        ViewMode.Add("xlsx", 4)
        ViewMode.Add("csv", 4)
        ViewMode.Add("zip", 5)
    End Sub

    Protected Function IsUTFChar(Judge As Byte) As Boolean
        If (Judge And &HC0) = &H80 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Const TryReadFor As Integer = 100

    Protected Sub LoadTextPreview(ByVal Filename As String, ByVal GotMode As Integer)
        HTMLPreviewPanel.Visible = True
        Dim CurrentEncoder As Encoding = Encoding.UTF8
        Select Case SelectEncoding.SelectedIndex
            Case 0
                ' Auto:
                Dim tryReader As IO.BinaryReader = New BinaryReader(File.Open(Filename, FileMode.Open))
                Dim firsts() As Byte = tryReader.ReadBytes(3)
                tryReader.Close()
                If firsts(0) >= 239 OrElse IsUTFChar(firsts(1)) OrElse IsUTFChar(firsts(2)) Then
                    ' EF Header of file.
                    ' Do nothing!
                    TextEncodingInfo.Text = "[UTF-8 BOM]"
                    Exit Select
                ElseIf firsts(0) = 254 AndAlso firsts(1) = 255 Then
                    CurrentEncoder = System.Text.Encoding.BigEndianUnicode
                    TextEncodingInfo.Text = "[Unicode BE]"
                    Exit Select
                ElseIf firsts(0) = 255 AndAlso firsts(1) = 254 Then
                    CurrentEncoder = System.Text.Encoding.Unicode
                    TextEncodingInfo.Text = "[Unicode]"
                    Exit Select
                Else
                    ' ANSI Here!
                    CurrentEncoder = System.Text.Encoding.Default
                    TextEncodingInfo.Text = "[" & System.Text.Encoding.Default.EncodingName & "]"
                End If

            Case 1
                CurrentEncoder = System.Text.Encoding.Default
                TextEncodingInfo.Text = "[" & System.Text.Encoding.Default.EncodingName & "]"
            Case 2
                CurrentEncoder = Encoding.UTF8
                TextEncodingInfo.Text = "[UTF-8]"
            Case 3
                CurrentEncoder = System.Text.Encoding.Unicode
                TextEncodingInfo.Text = "[Unicode]"
            Case 4
                CurrentEncoder = System.Text.Encoding.BigEndianUnicode
                TextEncodingInfo.Text = "[Unicode BE]"
        End Select
        Dim DataReader As IO.StreamReader = My.Computer.FileSystem.OpenTextFileReader(Filename, CurrentEncoder)
        'RawDat.Text = DataReader.ReadToEnd()
        RawDat.Text = ""
        Do Until DataReader.EndOfStream
            RawDat.Text &= DataReader.ReadLine()
            RawDat.Text &= vbCrLf
            If GotMode = 0 Then
                RawDat.Text &= "<br />"
            End If
        Loop
        DataReader.Close()
    End Sub

    Protected Function GenerateLayer(Path As String) As String
        Dim tmp As String = ""
        For Each i In Path
            If i = "/" OrElse i = "\" Then
                tmp &= "&nbsp;&nbsp;"
            End If
        Next
        Return tmp
    End Function

    Protected Function CountLayer(ByVal Path As String) As Integer
        Dim Sum As Integer = 0
        For Each i In Path
            If i = "/" Then
                Sum += 1
            End If
        Next
        Return Sum
    End Function

    Protected Sub GenerateArchiveTree(ByVal Path As String, ByVal Display As String, ByRef CurrentNode As TreeNode)
        ' Tag 为文件夹/文件名
        While Path(Path.Length - 1) = "/"c
            Path = Path.Substring(0, Path.Length - 1)
        End While
        Dim CurNode As TreeNode
        If CountLayer(Path) = 0 Then
            CurNode = New TreeNode(Display, Path)
            CurrentNode.ChildNodes.Add(CurNode)
            CurNode.CollapseAll()
            Exit Sub
        End If
        Dim Pather() As String = Split(Path, "/", 2)
        For i = 0 To CurrentNode.ChildNodes.Count - 1
            If CurrentNode.ChildNodes(i).Value = Pather(0) Then
                GenerateArchiveTree(Pather(1), Display, CurrentNode.ChildNodes(i))
                Exit Sub
            End If
        Next
        CurNode = New TreeNode(Pather(0), Pather(0))    ' 需要特殊处理？
        CurrentNode.ChildNodes.Add(CurNode)
        GenerateArchiveTree(Pather(1), Display, CurNode)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadHash()
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
                    CurrentPath = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(Para(1)))
            End Select
        Next
        If CurrentPath = "" Then
            Exit Sub
        End If
        PreviewPath.Text = CurrentPath
        Dim GotExt = GetExt(CurrentPath)
        Dim Filename As String = FileRoot & CurrentPath
        If ViewMode.Contains(GotExt) Then
            DefaultDisplay.Visible = False
            Dim GotMode = ViewMode(GotExt)
            Select Case GotMode
                Case 0, 1
                    LoadTextPreview(Filename, GotMode)
                Case 2
                    PicturePreviewPanel.Visible = True
                    Dim ToCopy As String = FindFreefile("_viewer." & GotExt)
                    My.Computer.FileSystem.CopyFile(FileRoot & CurrentPath, ToCopy)
                    PreviewImage.ImageUrl = "/" & My.Computer.FileSystem.GetName(ToCopy)
                Case 3
                    Try
                        Dim wordapp, wordex As Object
                        wordapp = Server.CreateObject("Word.Application")
                        wordapp.Visible = False
                        wordex = wordapp.Documents.Open(Filename)
                        Dim fq As String = FindFreefile("_preview.html")
                        wordex.SaveAs(fq, FileFormat:=wdFormatHTML)
                        wordex.Close()
                        wordapp.Quit()
                        LoadTextPreview(fq, 1)

                    Catch ex As Exception
                        RawDat.Visible = True
                        RawDat.ForeColor = Drawing.Color.Red
                        RawDat.Text = "打开 Word 文档时出现错误：" & ex.Message
                    End Try
                Case 4
                    HTMLPreviewPanel.Visible = True
                    RawDat.Visible = True
                    Dim fq As String = FindFreefile("_preview.html")
                    Try
                        Dim wordapp, wordex As Object
                        wordapp = Server.CreateObject("Excel.Application")
                        wordapp.Visible = False
                        wordex = wordapp.Workbooks.Open(Filename)
                        wordex.SaveAs(fq, FileFormat:=xlHtml)
                        wordex.Close()
                        wordapp.Quit()
                        ' xml 
                    Catch ex As Exception
                        RawDat.ForeColor = Drawing.Color.Red
                        RawDat.Text = "打开 Excel 文档时出现错误：" & ex.Message
                        GoTo CheckPrint
                    End Try
                    RawDat.Text = "请点击 <a href=""/" & My.Computer.FileSystem.GetName(fq) & """>此处</a>预览。"
                    Response.Redirect("/" & My.Computer.FileSystem.GetName(fq))
                Case 5
                    ' Zip file:
                    If Not IsPostBack Then
                        Dim arc = ZipFile.Open(Filename, ZipArchiveMode.Read)
                        'Dim res As StringBuilder = New StringBuilder("zip 存档中的内容：<br /><table><tr><th>文件名</th><th>修改日期</th><th>大小</th><th>压缩后大小</th></tr>")
                        ArchiveTree.Visible = True
                        ArchiveTree.Nodes.Add(New TreeNode("(ZIP 存档根目录)", ""))
                        For Each i In arc.Entries
                            If i.Name = "" Then
                                Dim GotDirectoryName As String = i.FullName
                                While GotDirectoryName(GotDirectoryName.Length - 1) = "/"c
                                    GotDirectoryName = GotDirectoryName.Substring(0, GotDirectoryName.Length - 1)
                                End While
                                Dim GotIndex = GotDirectoryName.LastIndexOf("/"c)
                                If GotIndex > 0 Then
                                    GotDirectoryName = GotDirectoryName.Substring(GotDirectoryName.LastIndexOf("/"c) + 1)
                                End If
                                GenerateArchiveTree(i.FullName, GotDirectoryName, ArchiveTree.Nodes(0))
                            End If
                        Next
                        For Each i In arc.Entries
                            If i.Name <> "" Then
                                GenerateArchiveTree(i.FullName, i.Name, ArchiveTree.Nodes(0))
                            End If
                        Next
                        'res.Append("</table>")
                        'RawDat.Text = res.ToString()
                        'HTMLPreviewPanel.Visible = True
                        arc.Dispose()
                    End If
            End Select
        End If
CheckPrint: If AcceptablePrintTypes.Contains("." & GotExt) Then
            ToPrint.Enabled = True
        End If
    End Sub

    Protected Sub ToPrint_Click(sender As Object, e As EventArgs) Handles ToPrint.Click
        Response.Redirect("/WebFormPrinter.aspx?path=" & HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(CurrentPath)))
    End Sub

End Class