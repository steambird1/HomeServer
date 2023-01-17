Imports Microsoft.VisualBasic.FileIO
Imports System.Drawing

Public Class WebFormMain
    Inherits System.Web.UI.Page

    Public Const FileRoot As String = "F:\DiskEntry"
    Public LocalFile As String = ""     ' 空：没有本地文件可用

    Protected rseed As Random = New Random()
    Protected ReadOnly ftsearch As Hashtable = New Hashtable()

    Private Function Rounder(Value As Double) As Double
        Return Int(Value * 100) / 100
    End Function

    Public Function GetExt(filename As String) As String
        Dim d As List(Of String) = New List(Of String)(Split(filename, "."))
        Return d(d.Count - 1)
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        printerr.Visible = False
        Submittor.Enabled = True
        printstate.ForeColor = Color.Black
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
        ' Load possible extensions
        ftsearch.Add("doc", 0)
        ftsearch.Add("docx", 0)
        ftsearch.Add("txt", 0)
        ftsearch.Add("jpg", 1)
        ftsearch.Add("jpeg", 1)
        ftsearch.Add("png", 1)
        ftsearch.Add("bmp", 1)
        ftsearch.Add("gif", 1)
        ftsearch.Add("xls", 3)
        ftsearch.Add("xlsx", 3)
        ftsearch.Add("csv", 3)
        ftsearch.Add("pdf", 4)
        ' Add upper-case conditions
        Dim ftsearch_old As Hashtable = New Hashtable(ftsearch)
        For Each i In ftsearch_old
            ftsearch.Add(UCase(i.Key), ftsearch(i.Value))
        Next
        ' Load state
        With My.Computer.FileSystem
            For Each i In .Drives
                If i.Name = "D:\" Then
                    FreeDSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                End If
            Next
        End With
        Dim wordapp As Object = Nothing
        Try
            wordapp = Server.CreateObject("Word.Application")
            wordapp.Visible = False
            printstate.Text = wordapp.ActivePrinter
            wordapp.Quit()
            ' Load other printers
            If Not IsPostBack Then
                ' It'll always set to default if we load it on PostBack (an ASP execution
                ' to get its values).
                selprinters.Items.Clear()
                selprinters.Items.Add("(使用默认打印机)")
                Dim i As String
                For Each i In Printing.PrinterSettings.InstalledPrinters
                    selprinters.Items.Add(i)
                Next
            End If
        Catch ex As Exception
            printerr.Visible = True
            Submittor.Enabled = False
            printstate.Text = "无法获取打印机类型"
            printstate.ForeColor = Color.Red
        End Try
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
                    ' Get Direct-Print Path (Manually add DiskEntry)
                    LocalFile = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(Para(1)))
                    FileUpload.Visible = False
                    SelectedServerFile.Text = "来自服务器的文件: " & LocalFile
                    SelectedServerFile.Visible = True
            End Select
        Next
    End Sub

    Protected Function FindFreefile(fix As String) As String
        Dim r As String = ""
        Do
            r = Server.MapPath(rseed.Next() & "_" & fix)
            If Not FileSystem.FileExists(r) Then Exit Do
        Loop
        Return r
    End Function

    Protected Sub Submittor_Click(sender As Object, e As EventArgs) Handles Submittor.Click
        ' 1. Save file
        Dim RealFilename As String
        If LocalFile <> "" Then
            RealFilename = My.Computer.FileSystem.GetName(FileRoot & LocalFile)
        Else
            RealFilename = FileUpload.FileName
        End If
        Dim fr As String = FindFreefile(RealFilename)
        If FileUpload.HasFile Then                ' FUCK
            FileUpload.SaveAs(fr)
        ElseIf LocalFile = "" Then
            Response.Write("<script>alert('请选择文件!')</script>")
            Exit Sub
        Else
            My.Computer.FileSystem.CopyFile(FileRoot & LocalFile, fr)
        End If
        ' 2. Progress
        Dim wordapp As Object = Nothing
        Dim si As Integer = FileTypes.SelectedIndex
        If si = 2 Then
            ' Auto confirm
            Dim ext As String = GetExt(RealFilename)
            If ftsearch.Contains(ext) Then
                si = ftsearch(ext)
            Else
                Response.Write("<script>alert('无效的文档类型: " & ext & "！请在下拉列表中选择。')</script>")
                Exit Sub
            End If
        End If
        Select Case si
            Case 3
                Try
                    wordapp = Server.CreateObject("Excel.Application")
                    wordapp.Visible = False
                    If selprinters.SelectedIndex > 0 Then
                        wordapp.ActivePrinter = selprinters.SelectedValue
                    End If
                    Dim wordex As Object = wordapp.Workbooks.Open(fr)
                    'wordex.ActiveSheet.PageSetup.Orientation = PageOrd.SelectedIndex
                    wordex.PrintOut(Copies:=Val(DocCopies.Text))
                    wordex.Close()
                    wordapp.Quit()
                    Response.Write("<script>alert('打印成功！');</script>")
                Catch ex As Exception
                    Response.Write("<script>alert('内部服务器错误：调用 excel 应用程序时出现 " & ex.Message & " 错误。请尝试稍后再试。')</script>")
                End Try
            Case 4
                Try
                    For i = 0 To Val(DocCopies.Text) - 1
                        Shell(Server.MapPath("Sumatra.exe") & " -print-to-default """ & fr & """", AppWinStyle.MinimizedNoFocus, True)
                    Next
                    Response.Write("<script>alert('打印成功！');</script>")
                Catch ex As Exception
                    Response.Write("<script>alert('内部服务器错误：调用 Sumatra 应用程序时出现 " & ex.Message & " 错误。请尝试稍后再试。')</script>")
                End Try
            Case Else
                Try
                    wordapp = Server.CreateObject("Word.Application")
                    wordapp.Visible = False
                    If selprinters.SelectedIndex > 0 Then
                        wordapp.ActivePrinter = selprinters.SelectedValue
                    End If
                    Dim fq As String = Nothing
                    Dim wordex As Object = Nothing
                    Select Case si
                        Case 0
                            wordex = wordapp.Documents.Open(fr)
                        Case 1
                            wordex = wordapp.Documents.Add()
                            wordex.InlineShapes.AddPicture(fr)
                            ' It always requires saving.
                            fq = FindFreefile("__temp__.docx")
                            wordex.SaveAs(fq)
                        Case Else
                            Response.Write("<script>alert('无效的文档类型！请在下拉列表中选择。')</script>")
                            wordapp.Quit()
                            Exit Sub
                    End Select
                    wordex.PageSetup.Orientation = PageOrd.SelectedIndex
                    wordex.PrintOut(Copies:=Val(DocCopies.Text))
                    wordex.Close()
                    wordapp.Quit()
                    FileSystem.DeleteFile(fr)
                    If Not IsNothing(fq) Then
                        FileSystem.DeleteFile(fq)
                    End If
                    Response.Write("<script>alert('打印成功！');</script>")
                Catch ex As Exception
                    Response.Write("<script>alert('内部服务器错误：调用 word 应用程序时出现 " & ex.Message & " 错误。请尝试稍后再试。')</script>")
                End Try
        End Select
        
    End Sub

    Protected Sub FileTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FileTypes.SelectedIndexChanged
        PageOrd.Enabled = True
        DocCopies.Enabled = True
        selprinters.Enabled = True
        If FileTypes.SelectedIndex = 3 Then
            PageOrd.SelectedIndex = 0
            PageOrd.Enabled = False
        ElseIf FileTypes.SelectedIndex = 4 Then
            PageOrd.Enabled = False
            'DocCopies.Text = "1"
            'DocCopies.Enabled = False
            selprinters.SelectedIndex = 0
            selprinters.Enabled = False
        End If
    End Sub
End Class