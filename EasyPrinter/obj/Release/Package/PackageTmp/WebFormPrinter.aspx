<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebFormPrinter.aspx.vb" Inherits="EasyPrinter.WebFormMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        p.note1 {
            color: red;
        }
        .auto-style1 {
            color: #FF3300;
        }
        td.elem {
            padding-left: 20px;
            padding-right: 20px;
        }
    </style>
</head>
<body>
    <div>
    
        <h1>
            <asp:ScriptManager ID="scm" runat="server" EnablePartialRendering="False">
            </asp:ScriptManager>
            快速打印</h1>
        <a href="WebFormMain.aspx">返回主页</a>
        <strong>只需几步，快速打印文档！</strong>
        <hr />
        <form runat="server">
                可用打印空间：<asp:Label ID="FreeDSpace" runat="server"></asp:Label>
                <br />
                <br />
                当前打印机：<asp:Label ID="printstate" runat="server" Text="(正在加载...)"></asp:Label>
                <br />
                <br />
                Step 1: 设置<br /> 
                <br />
            <span>
                文件类型：
                <asp:DropDownList ID="FileTypes" runat="server" AutoPostBack="True">
                    <asp:ListItem>Word 文档文件</asp:ListItem>
                    <asp:ListItem>图片文件</asp:ListItem>
                    <asp:ListItem Selected="True" Value="自动选择"></asp:ListItem>
                    <asp:ListItem>Excel 文档文件</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                打印机：<asp:DropDownList ID="selprinters" runat="server" Height="69px" Width="328px">
                    <asp:ListItem Selected="True">(使用默认打印机)</asp:ListItem>
                </asp:DropDownList>
                </span>
                <br />
                <br />
                <span class="auto-style1">注意：图片打印将以原图大小打印，超出部分将被忽略！<br />
                同时，务必确认文件类型选择正确，否则将导致不可预料的后果！<br />
                如果需要更多图片打印功能，请选择下方的“高级打印功能”！<br />
                <br />
                Excel 文件将不能选择页面方向，锁定为纵向，且将打印所有工作表！</span><br />
                <asp:Panel ID="DOCPanel" runat="server" Visible="False">
                    
                </asp:Panel>
                <table>
                    <tr>
                        <td class="elem">
                            打印的份数:
                        <asp:TextBox ID="DocCopies" runat="server" TextMode="Number">1</asp:TextBox>
                        </td>
                        <td class="elem">
                            页面方向：<asp:DropDownList ID="PageOrd" runat="server" Height="16px" Width="113px">
                    <asp:ListItem>纵向</asp:ListItem>
                    <asp:ListItem>横向</asp:ListItem>
                </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                        
                    <br />
                <br />
                <br />
                Step 2: 选择文件<br /> 
                <br />
                <asp:FileUpload ID="FileUpload" runat="server" ClientIDMode="Static" />
                <br />
                <br />
                Step 3: 点击打印<br /> 
                <br />
                <asp:Button ID="Submittor" runat="server" Text="打印" />
                <br />
        </form>
        
        
    </div>
    <asp:Label ID="printerr" runat="server" ForeColor="#FF0066" Text="打印机异常，无法打印！" Visible="False"></asp:Label>
    <h2>高级打印功能</h2>
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/AdvancedPicturePrinter.aspx">多张照片打印</asp:HyperLink>
    <p>
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/WriteToPrint.aspx">输入文字打印</asp:HyperLink>
    </p>
</body>
</html>
