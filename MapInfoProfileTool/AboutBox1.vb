Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports MapInfo.MiPro.Interop

Public NotInheritable Class AboutBox1

    Private Sub AboutBox1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        getHelp()
    End Sub

    Sub getHelp()
        'check help file exists
        'save help to MI directory to prevent Microsoft Security Updates 896358 & 840315  from blocking it 
        'http://stackoverflow.com/questions/11438634/opening-a-chm-file-produces-navigation-to-the-webpage-was-canceled

        Dim cur_dir As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-4 )")
        If File.Exists(cur_dir & "ProfileToolHelp.chm") Then
        Else
            My.Computer.FileSystem.WriteAllBytes(cur_dir & "ProfileToolHelp.chm", My.Resources.ProfileToolHelp, False)
        End If
        ' MsgBox(cur_dir & "ProfileToolHelp.chm")
        'run help file
        'System.Windows.Forms.Help.ShowHelp(Me, cur_dir & "ProfileToolHelp.chm", HelpNavigator.TableOfContents)
        System.Diagnostics.Process.Start(cur_dir & "ProfileToolHelp.chm")

    End Sub
End Class
