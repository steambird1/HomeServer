Imports System.Drawing
Imports Microsoft.VisualBasic.FileIO

Public Class AdvancedPicturePrinter
    Inherits System.Web.UI.Page

    Public Const FileRoot As String = "F:\DiskEntry"

    Protected rseed As Random = New Random()
    Protected submits As List(Of WebControls.Button) = New List(Of WebControls.Button)

    Public ReadOnly wdStory As Integer = 6
    Public InternalGiven As Boolean = False
    Public InternalGivenFiles As List(Of String) = New List(Of String)

    Public Structure MyRange
        Public Property Begin As Integer
        Public Property [End] As Integer
        Public Sub New(Begin As Integer, [End] As Integer)
            Me.End = [End]
            Me.Begin = Begin
        End Sub
    End Structure

    Protected Function FindFreefile(fix As String) As String
        Dim r As String = ""
        Do
            r = Server.MapPath(rseed.Next() & "_" & fix)
            If Not FileSystem.FileExists(r) Then Exit Do
        Loop
        Return r
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
        submits.Add(Submittor)
        submits.Add(LayoutSub)
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
            For Each i In submits
                i.Enabled = True
            Next
        Catch ex As Exception
            printerr.Visible = True
            'Submittor.Enabled = False
            ' Must Gray All submits:
            For Each i In submits
                i.Enabled = False
            Next
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
            If Para(0) = "deliver" Then
                Continue For
            End If
            ' For each file procees it!
            InternalGiven = True
            InternalGivenFiles.Add(Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(Para(1))))
        Next
        InternalGivenInfo.Text = "来自服务器的 " & InternalGivenFiles.Count & " 已选择"
        InternalGivenInfo.Visible = InternalGiven
        pictureloads.Visible = Not InternalGiven
    End Sub

    Protected Sub LoadFilenames(ByRef frs As List(Of String), ByRef folds As List(Of String))
        If InternalGiven Then
            For Each i In InternalGivenFiles
                Dim RealCurrent As String = FileRoot & i
                Dim fr As String = FindFreefile(My.Computer.FileSystem.GetName(RealCurrent))
                My.Computer.FileSystem.CopyFile(RealCurrent, fr)
                frs.Add(fr)
                folds.Add(i)
            Next
        ElseIf pictureloads.HasFile Then                ' FUCK
            'pictureloads.SaveAs(fr)
            For Each i In pictureloads.PostedFiles
                Dim fr As String = FindFreefile(pictureloads.FileName)
                frs.Add(fr)
                folds.Add(i.FileName)
                i.SaveAs(fr)
            Next
        Else
            Response.Write("<script>alert('请选择文件!')</script>")
            Exit Sub
        End If
    End Sub

    Protected Sub Submittor_Click(sender As Object, e As EventArgs) Handles Submittor.Click
        ' Almost used the same.
        ' 1. Save file
        '
        Dim frs As List(Of String) = New List(Of String)()
        Dim folds As List(Of String) = New List(Of String)()
        Dim fq As String = Nothing
        LoadFilenames(frs, folds)
        If frs.Count <= 0 Then
            Exit Sub
        End If
        ' 2. Progress
        Dim wordapp As Object = Nothing
        Dim wrange As Object
        Dim wordex, wordpic As Object
        Try
            wordapp = Server.CreateObject("Word.Application")
            wordapp.Visible = False
            If selprinters.SelectedIndex > 0 Then
                wordapp.ActivePrinter = selprinters.SelectedValue
            End If

            wordex = wordapp.Documents.Add()
            Dim cnt As Integer = 0
            For Each i In frs

                If checktoaddname.Checked Then
                    cnt += 1
                    wordex.Content.InsertAfter(Text:="[" & cnt & "] " & folds(cnt - 1))
                End If
                wordex.Content.InsertAfter(Text:=vbCrLf)                    ' Preventing bad text
                'wordapp.Selection.EndKey(Unit:=wdStory)                     ' Not working correctly. Bad insertation
                'wrange = New MyRange(wordex.Content.End, wordex.Content.End)
                wrange = wordex.Range(Start:=wordex.Content.End - 1, End:=wordex.Content.End - 1)
                wordpic = wordex.InlineShapes.AddPicture(i, Range:=wrange)
                For j = 0 To Val(linecount.Text)
                    wordex.Content.InsertAfter(Text:=vbCrLf)
                Next
            Next
            Dim p As Double = Val(Zoomer.Text) / 100.0#
            ' Inline shape only, so just do this
            For i = 1 To wordex.InlineShapes.Count
                wordex.InlineShapes(i).Width = wordex.InlineShapes(i).Width * p
                wordex.InlineShapes(i).Height = wordex.InlineShapes(i).Height * p
            Next
            fq = FindFreefile("__temp__from_app")
            wordex.PageSetup.Orientation = PageOrd.SelectedIndex
            wordex.SaveAs(fq)
            wordex.PrintOut(Copies:=Val(DocCopies.Text))
            wordex.Close()
        Catch ex As Exception
            Response.Write("<script>alert('内部服务器错误：调用 word 应用程序时出现 " & ex.Message & " 错误。请尝试稍后再试。')</script>")
            Try
                If Not IsNothing(wordex) Then
                    wordex.Close(False)
                End If
            Catch
                ' Do nothing
            End Try
        End Try
        For Each i In frs
            If FileSystem.FileExists(i) Then
                FileSystem.DeleteFile(i)
            End If
        Next
        If (Not IsNothing(fq)) AndAlso FileSystem.FileExists(fq) Then
            FileSystem.DeleteFile(fq)
        End If
        If Not IsNothing(wordapp) Then
            wordapp.Quit()
        End If
    End Sub

    Protected Function GetGreatSize(nowWidth As Double, nowHeight As Double, etaWidth As Double, etaHeight As Double, Optional stretch As Boolean = True, Optional stepper As Double = 0.001) As SizeF
        ' Try zooming nowWidth
        Dim ratio As Double = etaWidth / nowWidth
        Dim newWidth As Double = etaWidth
        Dim newHeight As Double = nowHeight * ratio
        If newHeight > etaHeight Then
            If stretch Then
                ratio = etaHeight / nowHeight
                newHeight = etaHeight
                newWidth = nowWidth * ratio
            Else
                Do Until newHeight <= etaHeight
                    ratio -= stepper
                    newWidth = nowWidth * ratio
                    newHeight = nowHeight * ratio
                Loop
            End If
        End If
        Return New SizeF(newWidth, newHeight)
    End Function

    Protected Sub LayoutSub_Click(sender As Object, e As EventArgs) Handles LayoutSub.Click
        Const wdWrapNone As Integer = 3
        Const msoFront As Integer = 4

        Dim x As Double = Val(HorzN.Text)
        Dim y As Double = Val(VertN.Text)

        Dim wordapp, wordex, wrange, wordpic As Object
        Try
            wordapp = Server.CreateObject("Word.Application")
            wordapp.Visible = False
            If selprinters.SelectedIndex > 0 Then
                wordapp.ActivePrinter = selprinters.SelectedValue
            End If

            wordex = wordapp.Documents.Add()
        Catch ex As Exception
            Response.Write("<script>alert('内部服务器错误：调用 word 应用程序时出现 " & ex.Message & " 错误。请尝试稍后再试。')</script>")
            Try
                If Not IsNothing(wordex) Then
                    wordex.Close(False)
                End If
            Catch
                ' Do nothing
            End Try
        End Try

        wordex.PageSetup.Orientation = PageOrd.SelectedIndex

        ' Try to get page
        Dim h As Double = wordex.PageSetup.PageWidth - 150
        Dim w As Double = wordex.PageSetup.PageHeight - 150

        Dim a As Double = Val(HorzMargin.Text)
        Dim b As Double = Val(VertMargin.Text)

        Dim s As Double = (w - (x + 1) * b) / x
        Dim t As Double = (h - (y + 1) * a) / y

        Dim frs As List(Of String) = New List(Of String)()
        Dim folds As List(Of String) = New List(Of String)()
        Dim fq As String = Nothing
        LoadFilenames(frs, folds)
        If frs.Count <= 0 Then
            Exit Sub
        End If

        Dim horz_ordered As Double = 0
        Dim vert_ordered As Double = 0
        Dim counter As Integer = 0

        For Each i In frs
            counter += 1
            wrange = wordex.Range(Start:=wordex.Content.End - 1, End:=wordex.Content.End - 1)
            wordpic = wordex.InlineShapes.AddPicture(i, Range:=wrange)
            wordpic.ConvertToShape()
            ' Must be last one!
            wordpic = wordex.Shapes.Item(counter)
            wordpic.WrapFormat.Type = wdWrapNone
            wordpic.ZOrder(ZOrderCmd:=msoFront)

            Dim d As SizeF
            If StrOption.Checked Then
                d.Width = t
                d.Height = s
            Else
                d = GetGreatSize(wordpic.Width, wordpic.Height, t, s)
            End If

            wordpic.Width = d.Width
            wordpic.Height = d.Height

            wordpic.Left = a + horz_ordered * (t + a)
            wordpic.Top = b + vert_ordered * (s + b)

            horz_ordered += 1
            If horz_ordered >= Val(HorzN.Text) Then
                horz_ordered = 0
                vert_ordered += 1
            End If
            If vert_ordered >= Val(VertN.Text) Then
                vert_ordered = 0
                wrange = wordex.Range(Start:=wordex.Content.End - 1, End:=wordex.Content.End - 1)
                wrange.InsertBreak()
            End If
        Next

        fq = FindFreefile("__temp__from_app")

        wordex.SaveAs(fq)
        wordex.PrintOut(Copies:=Val(DocCopies.Text))
        wordex.Close()

        For Each i In frs
            If FileSystem.FileExists(i) Then
                FileSystem.DeleteFile(i)
            End If
        Next
        If (Not IsNothing(fq)) AndAlso FileSystem.FileExists(fq) Then
            FileSystem.DeleteFile(fq)
        End If
        If Not IsNothing(wordapp) Then
            wordapp.Quit()
        End If
    End Sub

    Protected Sub PrintModeSelector_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PrintModeSelector.SelectedIndexChanged
        PrintEach.Visible = False
        PrintLayout.Visible = False
        Select Case PrintModeSelector.SelectedIndex
            Case 0
                PrintEach.Visible = True
            Case 1
                PrintLayout.Visible = True
            Case Else

        End Select
    End Sub
End Class