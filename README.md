# ASP.NET 家庭服务器项目 HomeServer
[Chinese] 这是支持打印服务与文件存储/共享服务的家庭服务器。页面语言为中文。
[English] This is a Home Server which shares printers and storages in a web page. It's in Chinese.

## 服务器功能 Functions
1. 打印及图片智能排版 Printing and intelligent typesetting of pictures
2. 文件存储与共享 File sharing

## 搭建 Building
(To be translated ...)
1. 使用 Visual Studio 2012 及以上版本编译并发布该项目。该项目要求 .Net Framework 4.5 和 IIS 8.0 及以上版本。
推荐的配置是：
* Windows Server 2012 R2 (版本 6.2)
* IIS 8.0 (对应上述服务器)
* .Net Framework 4.5 (与相关 IIS 支持)
测试可用的另一组配置：
* Windows 11
* IIS Express 8.0
(与 Visual Studio 2017 编译)
* .Net Framework 4.5 (与相关 IIS 支持)
3. 如果要正常运行任何打印服务，需要至少 Microsoft Word 2007 及以上版本。如果要正常运行 Excel 打印服务，需要 Microsoft Excel 2007。
（因此，本软件中的打印服务仅作为学习参考使用。）

注意：使用时要更改如下几处:
1. 根据临时文件盘符的不同，修改 WebFormPrinter.aspx.vb 的第 59 行（如实际盘符为 `D:`，
```vb.net
        With My.Computer.FileSystem
            For Each i In .Drives
                ' 修改了这里
                If i.Name = "D:\" Then
                    FreeDSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                End If
            Next
        End With
```
与 FileExplore.aspx.vb 的第 64 行附近（如实际盘符均为 `D:`，
```vb.net
  Protected Sub UpdateDiskInfo()
        With My.Computer.FileSystem
            For Each i In .Drives
                If i.Name = "D:\" Then
                    FreeDSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                    FreeFSpace.Text = GetSpaceInfo(i.AvailableFreeSpace / 1024 / 1024)
                End If
            Next
        End With
    End Sub
```
2. 根据共享文件位置的不同，修改 FileExplore.aspx.vb 第 7 行（如
```vb.net
Public Const FileRoot As String = "D:\example"
```

更多内容正在补充...
To be written...
