Imports System.Drawing
Imports Microsoft.VisualBasic.FileIO

Public Class WriteToPrint
    Inherits System.Web.UI.Page

    Protected rseed As Random = New Random()

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
    End Sub

    Protected Sub Submittor_Click(sender As Object, e As EventArgs) Handles Submittor.Click
        Dim wordapp As Object = Nothing
        Dim wordex As Object = Nothing
        Dim wordsel As Object = Nothing
        Dim fq As String = Nothing
        Try
            wordapp = Server.CreateObject("Word.Application")
            wordapp.Visible = False
            If selprinters.SelectedIndex > 0 Then
                wordapp.ActivePrinter = selprinters.SelectedValue
            End If
            wordex = wordapp.Documents.Add()
            wordsel = wordapp.Selection
            wordsel.Font.Size = Val(textsizer.Text)
            wordsel.Font.Bold = isbold.Checked
            wordsel.TypeText(contents.Text)
            fq = FindFreefile("__temp_wrote")
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
        If (Not IsNothing(fq)) AndAlso FileSystem.FileExists(fq) Then
            FileSystem.DeleteFile(fq)
        End If
        If Not IsNothing(wordapp) Then
            wordapp.Quit()
        End If
    End Sub
End Class