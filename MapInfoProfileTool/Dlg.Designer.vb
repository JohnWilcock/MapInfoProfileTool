Imports System.io
Imports System.Reflection

Namespace ProfileTool
    Partial Class Dlg
        ''' <summary> 
        ''' Required designer variable. 
        ''' </summary> 
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary> 
        ''' Clean up any resources being used. 
        ''' </summary> 
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param> 
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary> 
        ''' Required method for Designer support - do not modify 
        ''' the contents of this method with the code editor. 
        ''' </summary> 
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dlg))
            Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
            Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
            Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
            Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.ToolStripContainer2 = New System.Windows.Forms.ToolStripContainer()
            Me.DataGridView1 = New System.Windows.Forms.DataGridView()
            Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
            Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.ComboBox1 = New System.Windows.Forms.ComboBox()
            Me.CheckBox1 = New System.Windows.Forms.CheckBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.ToolStripContainer3 = New System.Windows.Forms.ToolStripContainer()
            Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
            Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
            Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.Button4 = New System.Windows.Forms.Button()
            Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
            Me.ToolStripContainer1.ContentPanel.SuspendLayout()
            Me.ToolStripContainer1.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TabControl1.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.ToolStripContainer2.ContentPanel.SuspendLayout()
            Me.ToolStripContainer2.LeftToolStripPanel.SuspendLayout()
            Me.ToolStripContainer2.SuspendLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ToolStrip1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage3.SuspendLayout()
            Me.ToolStripContainer3.ContentPanel.SuspendLayout()
            Me.ToolStripContainer3.LeftToolStripPanel.SuspendLayout()
            Me.ToolStripContainer3.SuspendLayout()
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ToolStrip2.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            Me.SuspendLayout()
            '
            'ToolStripContainer1
            '
            Me.ToolStripContainer1.BottomToolStripPanelVisible = False
            '
            'ToolStripContainer1.ContentPanel
            '
            Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.TableLayoutPanel1)
            resources.ApplyResources(Me.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
            resources.ApplyResources(Me.ToolStripContainer1, "ToolStripContainer1")
            Me.ToolStripContainer1.LeftToolStripPanelVisible = False
            Me.ToolStripContainer1.Name = "ToolStripContainer1"
            Me.ToolStripContainer1.RightToolStripPanelVisible = False
            Me.ToolStripContainer1.TopToolStripPanelVisible = False
            '
            'TableLayoutPanel1
            '
            resources.ApplyResources(Me.TableLayoutPanel1, "TableLayoutPanel1")
            Me.TableLayoutPanel1.Controls.Add(Me.TabControl1, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.TabPage1)
            Me.TabControl1.Controls.Add(Me.TabPage2)
            Me.TabControl1.Controls.Add(Me.TabPage3)
            resources.ApplyResources(Me.TabControl1, "TabControl1")
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.ToolStripContainer2)
            resources.ApplyResources(Me.TabPage1, "TabPage1")
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'ToolStripContainer2
            '
            Me.ToolStripContainer2.BottomToolStripPanelVisible = False
            '
            'ToolStripContainer2.ContentPanel
            '
            Me.ToolStripContainer2.ContentPanel.Controls.Add(Me.DataGridView1)
            resources.ApplyResources(Me.ToolStripContainer2.ContentPanel, "ToolStripContainer2.ContentPanel")
            resources.ApplyResources(Me.ToolStripContainer2, "ToolStripContainer2")
            '
            'ToolStripContainer2.LeftToolStripPanel
            '
            Me.ToolStripContainer2.LeftToolStripPanel.Controls.Add(Me.ToolStrip1)
            Me.ToolStripContainer2.Name = "ToolStripContainer2"
            Me.ToolStripContainer2.TopToolStripPanelVisible = False
            '
            'DataGridView1
            '
            Me.DataGridView1.AllowUserToAddRows = False
            Me.DataGridView1.AllowUserToDeleteRows = False
            Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            resources.ApplyResources(Me.DataGridView1, "DataGridView1")
            Me.DataGridView1.Name = "DataGridView1"
            Me.DataGridView1.ReadOnly = True
            '
            'ToolStrip1
            '
            resources.ApplyResources(Me.ToolStrip1, "ToolStrip1")
            Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton3})
            Me.ToolStrip1.Name = "ToolStrip1"
            '
            'ToolStripButton1
            '
            Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            resources.ApplyResources(Me.ToolStripButton1, "ToolStripButton1")
            Me.ToolStripButton1.Name = "ToolStripButton1"
            '
            'ToolStripButton3
            '
            Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            resources.ApplyResources(Me.ToolStripButton3, "ToolStripButton3")
            Me.ToolStripButton3.Name = "ToolStripButton3"
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.Label2)
            Me.TabPage2.Controls.Add(Me.ComboBox1)
            Me.TabPage2.Controls.Add(Me.CheckBox1)
            Me.TabPage2.Controls.Add(Me.Label1)
            Me.TabPage2.Controls.Add(Me.NumericUpDown1)
            resources.ApplyResources(Me.TabPage2, "TabPage2")
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'ComboBox1
            '
            Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.ComboBox1.FormattingEnabled = True
            Me.ComboBox1.Items.AddRange(New Object() {resources.GetString("ComboBox1.Items"), resources.GetString("ComboBox1.Items1")})
            resources.ApplyResources(Me.ComboBox1, "ComboBox1")
            Me.ComboBox1.Name = "ComboBox1"
            '
            'CheckBox1
            '
            resources.ApplyResources(Me.CheckBox1, "CheckBox1")
            Me.CheckBox1.Name = "CheckBox1"
            Me.CheckBox1.UseVisualStyleBackColor = True
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'NumericUpDown1
            '
            resources.ApplyResources(Me.NumericUpDown1, "NumericUpDown1")
            Me.NumericUpDown1.Name = "NumericUpDown1"
            Me.NumericUpDown1.Value = New Decimal(New Integer() {20, 0, 0, 0})
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.ToolStripContainer3)
            resources.ApplyResources(Me.TabPage3, "TabPage3")
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'ToolStripContainer3
            '
            Me.ToolStripContainer3.BottomToolStripPanelVisible = False
            '
            'ToolStripContainer3.ContentPanel
            '
            Me.ToolStripContainer3.ContentPanel.Controls.Add(Me.Chart1)
            resources.ApplyResources(Me.ToolStripContainer3.ContentPanel, "ToolStripContainer3.ContentPanel")
            resources.ApplyResources(Me.ToolStripContainer3, "ToolStripContainer3")
            '
            'ToolStripContainer3.LeftToolStripPanel
            '
            Me.ToolStripContainer3.LeftToolStripPanel.Controls.Add(Me.ToolStrip2)
            Me.ToolStripContainer3.Name = "ToolStripContainer3"
            Me.ToolStripContainer3.TopToolStripPanelVisible = False
            '
            'Chart1
            '
            ChartArea1.Name = "ChartArea1"
            Me.Chart1.ChartAreas.Add(ChartArea1)
            resources.ApplyResources(Me.Chart1, "Chart1")
            Legend1.Name = "Legend1"
            Me.Chart1.Legends.Add(Legend1)
            Me.Chart1.Name = "Chart1"
            Series1.ChartArea = "ChartArea1"
            Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            Series1.Legend = "Legend1"
            Series1.Name = "Series1"
            Me.Chart1.Series.Add(Series1)
            '
            'ToolStrip2
            '
            resources.ApplyResources(Me.ToolStrip2, "ToolStrip2")
            Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2, Me.ToolStripButton4})
            Me.ToolStrip2.Name = "ToolStrip2"
            '
            'ToolStripButton2
            '
            Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            resources.ApplyResources(Me.ToolStripButton2, "ToolStripButton2")
            Me.ToolStripButton2.Name = "ToolStripButton2"
            '
            'ToolStripButton4
            '
            Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ToolStripButton4.Image = Global.My.Resources.Resources.MapInfow_100
            resources.ApplyResources(Me.ToolStripButton4, "ToolStripButton4")
            Me.ToolStripButton4.Name = "ToolStripButton4"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.TableLayoutPanel2)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'TableLayoutPanel2
            '
            resources.ApplyResources(Me.TableLayoutPanel2, "TableLayoutPanel2")
            Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 1)
            Me.TableLayoutPanel2.Controls.Add(Me.CheckedListBox1, 0, 0)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            '
            'TableLayoutPanel3
            '
            resources.ApplyResources(Me.TableLayoutPanel3, "TableLayoutPanel3")
            Me.TableLayoutPanel3.Controls.Add(Me.Button2, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Button1, 0, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Button4, 2, 0)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            '
            'Button2
            '
            resources.ApplyResources(Me.Button2, "Button2")
            Me.Button2.Image = Global.My.Resources.Resources.complex1
            Me.Button2.Name = "Button2"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'Button1
            '
            resources.ApplyResources(Me.Button1, "Button1")
            Me.Button1.Name = "Button1"
            Me.Button1.UseVisualStyleBackColor = True
            '
            'Button4
            '
            resources.ApplyResources(Me.Button4, "Button4")
            Me.Button4.Name = "Button4"
            Me.Button4.UseVisualStyleBackColor = True
            '
            'CheckedListBox1
            '
            resources.ApplyResources(Me.CheckedListBox1, "CheckedListBox1")
            Me.CheckedListBox1.FormattingEnabled = True
            Me.CheckedListBox1.Name = "CheckedListBox1"
            '
            'Dlg
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.ToolStripContainer1)
            Me.MinimumSize = New System.Drawing.Size(300, 200)
            Me.Name = "Dlg"
            Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
            Me.ToolStripContainer1.ResumeLayout(False)
            Me.ToolStripContainer1.PerformLayout()
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.ToolStripContainer2.ContentPanel.ResumeLayout(False)
            Me.ToolStripContainer2.LeftToolStripPanel.ResumeLayout(False)
            Me.ToolStripContainer2.LeftToolStripPanel.PerformLayout()
            Me.ToolStripContainer2.ResumeLayout(False)
            Me.ToolStripContainer2.PerformLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ToolStrip1.ResumeLayout(False)
            Me.ToolStrip1.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.TabPage2.PerformLayout()
            CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage3.ResumeLayout(False)
            Me.ToolStripContainer3.ContentPanel.ResumeLayout(False)
            Me.ToolStripContainer3.LeftToolStripPanel.ResumeLayout(False)
            Me.ToolStripContainer3.LeftToolStripPanel.PerformLayout()
            Me.ToolStripContainer3.ResumeLayout(False)
            Me.ToolStripContainer3.PerformLayout()
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ToolStrip2.ResumeLayout(False)
            Me.ToolStrip2.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

#End Region




        Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Friend WithEvents CheckedListBox1 As System.Windows.Forms.CheckedListBox
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Button2 As System.Windows.Forms.Button
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
        Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
        Friend WithEvents ToolStripContainer2 As System.Windows.Forms.ToolStripContainer
        Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
        Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripContainer3 As System.Windows.Forms.ToolStripContainer
        Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
        Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
        Friend WithEvents Button4 As System.Windows.Forms.Button
    End Class
End Namespace