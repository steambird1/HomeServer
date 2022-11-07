<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WriteToPrint.aspx.vb" Inherits="EasyPrinter.WriteToPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>文本打印</h1>
        <p>在手机上输入文本，即可直接打印！</p>
        <p>
            当前默认打印机：<asp:Label ID="printstate" runat="server" Text="---"></asp:Label>
        </p>
        <p>
            选择打印机：<asp:DropDownList ID="selprinters" runat="server" Width="192px">
            </asp:DropDownList>
        </p>
    </div>
        <p>
            字体大小：<asp:TextBox ID="textsizer" runat="server" TextMode="Number"></asp:TextBox>
        </p>
        <asp:CheckBox ID="isbold" runat="server" Text="加粗文字" />
        <br />
        <br />
                        打印的份数:
                        <asp:TextBox ID="DocCopies" runat="server" TextMode="Number">1</asp:TextBox>
                    <br />
        <br />
        页面方向：<asp:DropDownList ID="PageOrd" runat="server" Height="16px" Width="113px">
            <asp:ListItem>纵向</asp:ListItem>
            <asp:ListItem>横向</asp:ListItem>
        </asp:DropDownList>
        <p>
            <asp:TextBox ID="contents" runat="server" Height="215px" TextMode="MultiLine" Width="661px"></asp:TextBox>
        </p>
        <asp:Button ID="Submittor" runat="server" Text="打印" style="height: 21px" />
        <p>
    <asp:Label ID="printerr" runat="server" ForeColor="#FF0066" Text="打印机异常，无法打印！" Visible="False"></asp:Label>
        </p>
    </form>
</body>
</html>
