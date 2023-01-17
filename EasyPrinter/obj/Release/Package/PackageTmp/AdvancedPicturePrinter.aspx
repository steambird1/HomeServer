<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdvancedPicturePrinter.aspx.vb" Inherits="EasyPrinter.AdvancedPicturePrinter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>高级照片打印</title>
    <style>
        .printModes {
            padding-left: 65px;
            padding-right: 65px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>高级照片打印</h1>
        <a href="WebFormMain.aspx">返回主页</a>
    </div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/WebFormMain.aspx">返回</asp:HyperLink>
        <br />
        <br />
        <strong>Step 1: 选择文件</strong><p>
            选择照片：</p>
        <p>
            <asp:FileUpload ID="pictureloads" runat="server" AllowMultiple="True" />
        </p>
        <p>
            <asp:Label ID="InternalGivenInfo" runat="server" ForeColor="#009933" Visible="False"></asp:Label>
        </p>
        <p>
            <strong>Step 2: 配置通用设置</strong></p>
        <p>
            当前默认打印机：<asp:Label ID="printstate" runat="server" Text="---"></asp:Label>
        </p>
        <p>
            选择打印机：<asp:DropDownList ID="selprinters" runat="server" Width="192px">
            </asp:DropDownList>
        </p>
        <p>
                        打印的份数:
                        <asp:TextBox ID="DocCopies" runat="server" TextMode="Number">1</asp:TextBox>
                    <br />
            <br />
            页面方向：<asp:DropDownList ID="PageOrd" runat="server" Height="16px" Width="113px">
                <asp:ListItem>纵向</asp:ListItem>
                <asp:ListItem>横向</asp:ListItem>
            </asp:DropDownList>
        </p>
        <p style="font-weight: 700">
            Step 3: 选择打印模式</p>
        <p style="color: #0000CC">
            更多模式更新中...</p>
        <p style="font-weight: 700">
    <asp:Label ID="printerr" runat="server" ForeColor="#FF0066" Text="打印机异常，无法打印！" Visible="False"></asp:Label>
        </p>
        <p>
            <asp:RadioButtonList ID="PrintModeSelector" runat="server" AutoPostBack="True" Height="16px" Width="110px">
                <asp:ListItem Selected="True">逐个打印</asp:ListItem>
                <asp:ListItem>排版打印</asp:ListItem>
            </asp:RadioButtonList>
        </p>
            <asp:Panel ID="PrintEach" runat="server">
                <p>
                    <strong>逐个打印</strong></p>
                <p>
                    选择间隔的换行符数量：<asp:TextBox ID="linecount" runat="server" TextMode="Number">1</asp:TextBox>
                </p>
                <p>
                    选择每张图片缩放：<asp:TextBox ID="Zoomer" runat="server" TextMode="Number">100</asp:TextBox>
                    %</p>
                <p>
                    <asp:CheckBox ID="checktoaddname" runat="server" Text="在图片前添加图片文件名称" />
                </p>
                <asp:Button ID="Submittor" runat="server" Text="打印" />
                <br />
                <br />
        </asp:Panel>
        <asp:Panel ID="PrintLayout" runat="server" Visible="False">
            <strong>排版打印<br /> 
            <br />
            </strong>
            <br />
            对于一张纸：<br /> 
            <br />
            横向个数：<asp:TextBox ID="HorzN" runat="server" TextMode="Number">2</asp:TextBox>
            <br />
            <br />
            纵向个数：<asp:TextBox ID="VertN" runat="server" TextMode="Number">2</asp:TextBox>
            <br />
            <br />
            横向间隔：<asp:TextBox ID="HorzMargin" runat="server" TextMode="Number">25</asp:TextBox>
            <br />
            <br />
            纵向间隔：<asp:TextBox ID="VertMargin" runat="server" TextMode="Number">25</asp:TextBox>
            <br />
            <br />
            <asp:CheckBox ID="StrOption" runat="server" Text="拉伸" />
            <br />
            <br />
            比例尺 
            <asp:Label ID="Label1" runat="server" BackColor="Black" ForeColor="White" Text="0" Width="100px"></asp:Label>
            <asp:Label ID="Label2" runat="server" BackColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" Text="100" Width="100px"></asp:Label>
            <asp:Label ID="Label3" runat="server" BackColor="Black" ForeColor="White" Text="200" Width="100px"></asp:Label>
            <asp:Label ID="Label4" runat="server" BackColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" Text="300" Width="100px"></asp:Label>
            <asp:Label ID="Label5" runat="server" BackColor="Black" ForeColor="White" Text="400" Width="100px"></asp:Label>
            <asp:Label ID="Label6" runat="server" BackColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" Text="500" Width="100px"></asp:Label>
            <br />
            <br />
            <asp:Button ID="LayoutSub" runat="server" Text="打印" />
        </asp:Panel>
                <br />
        <br />
                <br />
        <br />
       
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
