<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DocumentPreview.aspx.vb" Inherits="EasyPrinter.DocumentPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        span.up {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <strong>文件预览</strong> <asp:Label ID="PreviewPath" runat="server"></asp:Label>
&nbsp;<asp:Button ID="ToPrint" runat="server" Text="转到打印" Width="116px" Enabled="False" />
        &nbsp;<asp:Label ID="TextEncodingInfo" runat="server"></asp:Label>
        &nbsp;<asp:DropDownList ID="SelectEncoding" runat="server" AutoPostBack="True" Height="18px" Width="180px">
            <asp:ListItem>(自动选择编码)</asp:ListItem>
            <asp:ListItem>ANSI (GB2312)</asp:ListItem>
            <asp:ListItem>UTF-8</asp:ListItem>
            <asp:ListItem>Unicode</asp:ListItem>
            <asp:ListItem>Unicode Big Endian</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="DefaultDisplay" runat="server" Text="暂不支持该文件的预览。"></asp:Label>
        <br />
        <br />
    </div>
        <asp:Panel ID="PicturePreviewPanel" runat="server" Visible="False">
            <asp:Image ID="PreviewImage" runat="server" AlternateText="预览加载失败！" />
        </asp:Panel>
        <br />

        <asp:Panel ID="HTMLPreviewPanel" runat="server" Visible="False">
            <asp:Label ID="RawDat" runat="server"></asp:Label>
            <br />

            </asp:Panel>
        <asp:TreeView ID="ArchiveTree" runat="server" Visible="False">
        </asp:TreeView>
    </form>
</body>
</html>
