<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FileExplore.aspx.vb" Inherits="EasyPrinter.FileExplore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            color: #FF5050;
        }
        .auto-style2 {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            <asp:ScriptManager ID="ScriptManagerMain" runat="server">
            </asp:ScriptManager>
            局域网盘</h1>
        <a href="WebFormMain.aspx">返回主页</a>
        <br />
        <asp:UpdatePanel ID="FileModifiers" runat="server">
            <ContentTemplate>
                <br />
                
                <br />
                <br />
                (如果没有足够的空间，请联系网站管理者。)<br />
                <br />
                文件存储可用空间：<asp:Label ID="FreeFSpace" runat="server"></asp:Label>
                <br />
                <br />
                可用下载空间：<asp:Label ID="FreeDSpace" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Button ID="PrevDirs" runat="server" Height="21px" Text="&lt;- 上层目录" Width="109px" />
                &nbsp;当前位置：<asp:TextBox ID="CurrentPosition" runat="server" Width="219px">/</asp:TextBox>
                <asp:Button ID="GotoPosition" runat="server" Text="转到/刷新" />
                <br />
<br />
                <asp:Button ID="EnterDirectory" runat="server" Text="进入目录" />
                &nbsp;<br /> <asp:FileUpload ID="CreateFileHelper" runat="server" AllowMultiple="True" ClientIDMode="Static" />
                &nbsp;<asp:Button ID="CreateFile" runat="server" Text="上传文件" />
                <br />
                <span class="auto-style2">注意：不能上传超过 2 GB 的文件, 尽量不要上传超过 512 MB 的文件。<br /> 上传文件可能会花费较长的时间，其间可能没有任何提示！</span><br />&nbsp;<asp:Button ID="DeleteFile" runat="server" Text="删除文件" />
                &nbsp;<asp:Button ID="CreateCopy" runat="server" Text="创建副本" />
                &nbsp;<asp:Button ID="FileMover" runat="server" Text="移动文件" Width="71px" />
                &nbsp;<asp:Button ID="RenameFile" runat="server" Text="重命名" />
                &nbsp;<br /> <asp:Button ID="DownloadFiles" runat="server" Text="下载文件" />
                &nbsp;<asp:Button ID="CreateDirectory" runat="server" Text="新建文件夹" />
                <br />
                <br />
                <asp:UpdateProgress ID="UpdateProgressPanel" runat="server">
                    <ProgressTemplate>
                        <span class="auto-style1">请稍候... 正在执行操作。请勿进行其它操作！</span>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Label ID="ErrorDisplay" runat="server" ForeColor="#FF5050" Visible="False"></asp:Label>
                <br />
                <asp:Panel ID="FileRenamePanel" runat="server" Height="102px" Visible="False" BorderStyle="Solid">
                    原文件名：<asp:Label ID="OriginalFilename" runat="server"></asp:Label>
                    <br />
                    <br />
                    新文件名：<asp:TextBox ID="NewFilename" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="ConfirmRename" runat="server" Text="确定" Width="57px" />
                    &nbsp;<asp:Button ID="AbandonRename" runat="server" Text="取消" Width="59px" />
                    <br />
                </asp:Panel>
                <asp:Panel ID="DirectoryCreater" runat="server" Visible="False" BorderStyle="Solid">
                    新文件夹名称：<asp:TextBox ID="NewDirectoryName" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="ConfirmCreater" runat="server" style="height: 21px" Text="确定" Width="57px" />
                    &nbsp;<asp:Button ID="AbandonCreater" runat="server" Text="取消" Width="59px" />
                </asp:Panel>
                <br />
                <asp:Panel ID="ConfirmDeletion" runat="server" Visible="False" BorderStyle="Solid">
                    是否确认删除？<br /> 
                    <br />
                    <asp:Button ID="ConfirmDelete" runat="server" style="height: 21px" Text="确定" Width="57px" />
                    &nbsp;<asp:Button ID="AbandonDelete" runat="server" Text="取消" Width="59px" />
                </asp:Panel>
                <br />
                <asp:Panel ID="SetMoving" runat="server" BorderStyle="Solid" Visible="False">
                    移动到：<asp:TextBox ID="MovePosition" runat="server" Width="219px">/</asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="ConfirmMoving" runat="server" style="height: 21px" Text="确定" Width="57px" />
                    &nbsp;<asp:Button ID="AbandonMoving" runat="server" Text="取消" Width="59px" />
                </asp:Panel>
                <br />
                <br />
                <br />
                <asp:Button ID="SelectAll" runat="server" Text="全选" Width="52px" />
                &nbsp;<asp:Button ID="SelectAllVert" runat="server" Text="全不选" Width="52px" />
<br />
<br />
                <asp:CheckBoxList ID="FileViewers" runat="server" Height="307px" RepeatLayout="Flow" Width="885px" AutoPostBack="True">
                </asp:CheckBoxList>
                <br />
                <asp:Label ID="FileProperty" runat="server" Text="选择一个文件，查看其具体属性。"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="CreateFile" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
