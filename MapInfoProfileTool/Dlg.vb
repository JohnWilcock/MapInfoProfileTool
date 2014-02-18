'/*****************************************************************************
'*       Author JWilcock 2014
'*      Profile Tool for native mapinfo Grids v1.0
'*****************************************************************************


Imports System
Imports System.Windows.Forms
Imports System.Threading
Imports System.Xml
Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports MapInfo.MiPro.Interop
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.DataVisualization.Charting



Namespace ProfileTool
    Partial Public Class Dlg

        Inherits UserControl
        ' some string in xml file 
        'Private Const STR_NAME As String = "Name"
        Private Const STR_ROOT As String = "root"
        Private Const STR_DIALOG As String = "Dialog"
        Private Const STR_NAMEDVIEWS As String = "ProfileTool"
        'Private Const STR_VIEWS As String = "Views"
        Private Const STR_PATH_DIALOG As String = "/ProfileTool/Dialog"
        Private Const STR_PATH_ROOT_FOLDER As String = "/ProfileTool/Views"
        Private Const STR_LEFT As String = "Left"
        Private Const STR_TOP As String = "Top"
        Private Const STR_WIDTH As String = "Width"
        Private Const STR_HEIGHT As String = "Height"

        ' The controller class which uses this dialog class ensures 
        ' * a single instance of this dialog class. However different 
        ' * running instance of MapInfo Professional will have their 
        ' * own copy of dll. To make sure that read/write from/to xml 
        ' * file which is going to be a single file on the disk, is 
        ' * smooth and we have the synchronized access to the xml file, 
        ' * the Mutexes will be used. 
        ' 

        ' Name of the mutex 
        Private sXMLFile As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\MapInfo\MapInfo\nviews.xml"
        Private dialogLeft As Integer, dialogTop As Integer, dialogWidth As Integer, dialogHeight As Integer
        ' flag indicating whether it is the first time the form is being loaded
        Dim firstLoad As Boolean = True
        Private _controller As Controller  ' represents the window that owns this dialog (main MI Pro window)

        'min/max x/y
        Dim minX, minY, maxX, maxY As Double
        Dim isMapper As Integer
        Dim coordsys As String = "CoordSys Earth Projection 8, 79, " & Chr(34) & "m" & Chr(34) & ", -2, 49, 0.9996012717, 400000, -100000 Bounds (-7845061.1011, -15524202.1641) (8645061.1011, 4470074.53373)"

        ''' <summary> 
        ''' Construction 
        ''' </summary> 
        ''' 
        Public Shared fPathList As New List(Of String)
        Public gridNameList As New List(Of String)

        'lists for node xy
        Public Shared xList As New List(Of Double)
        Public Shared yList As New List(Of Double)

        'lists for interval xy
        Public Shared xIntervalList As New List(Of Double)
        Public Shared yIntervalList As New List(Of Double)
        Public Shared zIntervalList As New List(Of Double)
        Public Shared ELEVarray As New List(Of Double)

        Public Shared nSamples As Integer
        Public Shared xsec_length As Double
        Public Shared tableName As String


        Public Sub New()
            InitializeComponent()
            'mut = New Mutex(False, mutexName)

            'check for blank file used in search operations - this is used as a flag to see if OSTools has been installed, if not it is autoregestered by the MBX
            CreateBlank()
        End Sub

        ''' <summary>
        ''' Parameterised Construction
        ''' <param name="controller"></param>
        ''' </summary>
        Public Sub New(ByVal controller As Controller)
            Me.New()
            _controller = controller
        End Sub




#Region "[DIALOG EVENT HANDLERS]"
        ''' <summary> 
        ''' Named View dialog Load event handler 
        ''' </summary> 
        ''' <param name="sender"></param> 
        ''' <param name="e"></param> 
        Private Sub NViewDlg_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ComboBox1.Text = "Distance"
            CreateBlank()
            If firstLoad = True Then
                firstLoad = False

                If dialogWidth >= Me.MinimumSize.Width AndAlso dialogWidth <= Screen.PrimaryScreen.WorkingArea.Width Then
                    Me.Width = dialogWidth
                End If
                If dialogHeight >= Me.MinimumSize.Height AndAlso dialogHeight <= Screen.PrimaryScreen.WorkingArea.Height Then
                    Me.Height = dialogHeight
                End If
                If dialogLeft > -Me.Width AndAlso dialogLeft < Screen.PrimaryScreen.WorkingArea.Width Then
                    Me.Left = dialogLeft
                End If
                If dialogTop > -Me.Top AndAlso dialogTop < Screen.PrimaryScreen.WorkingArea.Height Then
                    Me.Top = dialogTop
                End If
            End If
        End Sub
        ' This call to the WIN32 API function SetFocus is used in NViewDlg_FormClosing below
        <DllImport("User32.dll")> _
        Private Shared Function SetFocus(ByVal hWnd As IntPtr)
        End Function




#End Region


        Public Sub CloseDockWindow()
            ''Write out the XML file that stores the Named Views info
            _controller.DockWindowClose()
        End Sub
        ''' <summary>
        ''' Set the dialog position and docking state 
        ''' </summary>
        Public Sub SetDockPosition()
            _controller.SetDockWindowPositionFromFile()
        End Sub




#Region "[HELPERS]"

        Sub CreateBlank()
            'create blank tab is it does not exist
            Dim MIfolderDir As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-3)")
            If Not System.IO.File.Exists(MIfolderDir & "\ProfileTool.tab") Then

                InteropServices.MapInfoApplication.Do("Create Table blank(blank Char(30)) file " & Chr(34) & MIfolderDir & "\blank.tab" & Chr(34))
                InteropServices.MapInfoApplication.Do("open table " & Chr(34) & MIfolderDir & "\blank.tab" & Chr(34) & " as blank hide readonly")
                InteropServices.MapInfoApplication.Do("create map for blank CoordSys Earth Projection 8, 79, " & Chr(34) & "m" & Chr(34) & ", -2, 49, 0.9996012717, 400000, -100000 Bounds (-7845061.1011, -15524202.1641) (8645061.1011, 4470074.53373)")
                InteropServices.MapInfoApplication.Do("close table blank")
            End If
        End Sub


#End Region



        'functions called from MapBasic ***************************************

        Public Shared Function nextGrid(ByVal x As Integer) As Boolean
            Return InteropHelper.theDlg.CheckedListBox1.GetItemChecked(x)
        End Function

        Public Shared Function gridPath(ByVal x As Integer) As String

            'MI native grid handler excepts
            'asc
            'mig
            'dem
            'adf
            'flt
            'img
            'grd

            Return fPathList(x)
        End Function

        Public Shared Function numGrids() As Integer
            Return InteropHelper.theDlg.CheckedListBox1.Items.Count
        End Function

        Public Shared Function numSamplePoints() As Integer
            nSamples = InteropHelper.theDlg.NumericUpDown1.Value
            Return InteropHelper.theDlg.NumericUpDown1.Value
        End Function

        Public Shared Function isVerbose() As Boolean
            Return InteropHelper.theDlg.CheckBox1.Checked
        End Function

        '**********************************************************************


        'draws the line and populates the dat grid in the .net usercontrol
        Public Shared Function XsecLine(ByVal ReturnString As String) As Boolean
            Dim arrayGrid(,) As Double
            Dim gridStrings() As String
            Dim currentArray() As String
            Dim allHeaders As New List(Of String)
            Dim allSeries As New List(Of Series)
            Dim graphMIN As Double
            Dim graphMAX As Double

            'clear graph
            InteropHelper.theDlg.Chart1.Series.Clear()
            InteropHelper.theDlg.Chart1.Titles.Clear()
            InteropHelper.theDlg.Chart1.Titles.Add("Profile")


            'split string into grids
            gridStrings = ReturnString.Split("|")
            ReDim arrayGrid(gridStrings.Length, InteropHelper.theDlg.NumericUpDown1.Value)

            'setup datagrid
            InteropHelper.theDlg.DataGridView1.Columns.Clear()
            InteropHelper.theDlg.DataGridView1.Columns.Add("ID", "ID")
            InteropHelper.theDlg.DataGridView1.Rows.Clear()
            InteropHelper.theDlg.DataGridView1.Rows.Add(nSamples)

            'x values 
            currentArray = gridStrings(gridStrings.Length - 3).Split(",")
            allHeaders.Add(currentArray(0))
            InteropHelper.theDlg.DataGridView1.Columns.Add("Distance", currentArray(0))
            For x As Integer = 1 To nSamples
                arrayGrid(0, x) = currentArray(x)
                'add to rows
                InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("ID").Value = x
                InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("Distance").Value = currentArray(x)
            Next

            'xvalues 
            currentArray = gridStrings(gridStrings.Length - 2).Split(",")
            InteropHelper.theDlg.DataGridView1.Columns.Add("X", currentArray(0))
            allHeaders.Add(currentArray(0))
            For x As Integer = 1 To nSamples
                arrayGrid(1, x) = currentArray(x)
                'add to rows
                InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("X").Value = currentArray(x)
            Next

            'y values 
            currentArray = gridStrings(gridStrings.Length - 1).Split(",")
            InteropHelper.theDlg.DataGridView1.Columns.Add("Y", currentArray(0))
            allHeaders.Add(currentArray(0))
            For x As Integer = 1 To nSamples
                arrayGrid(2, x) = currentArray(x)
                'add to rows
                InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("Y").Value = currentArray(x)
            Next

            'for each z grid
            For z As Integer = 0 To InteropHelper.theDlg.CheckedListBox1.CheckedItems.Count - 1
                currentArray = gridStrings(z).Split(",")
                InteropHelper.theDlg.DataGridView1.Columns.Add("Z" & CStr(z), currentArray(0))
                allHeaders.Add(currentArray(0))

                'add graph series
                allSeries.Add(New Series)
                allSeries(z).Name = currentArray(0)
                allSeries(z).ChartType = SeriesChartType.Line

                For x As Integer = 1 To nSamples
                    arrayGrid(z + 3, x) = currentArray(x)
                    'add to rows
                    InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("Z" & CStr(z)).Value = currentArray(x)
                    If InteropHelper.theDlg.ComboBox1.Text = "Samples" Then
                        'label with sample points
                        allSeries(z).Points.AddXY(x, currentArray(x))
                    Else
                        'label with distance
                        allSeries(z).Points.AddXY(InteropHelper.theDlg.DataGridView1.Rows(x - 1).Cells("Distance").Value, currentArray(x))
                    End If


                    'get min /max
                    If z + x = 1 Then
                        graphMIN = currentArray(x)
                        graphMAX = currentArray(x)
                    Else
                        graphMIN = Math.Min(graphMIN, CDbl(currentArray(x)))
                        graphMAX = Math.Max(graphMAX, CDbl(currentArray(x)))
                    End If
                Next

                'add the series to the chart
                InteropHelper.theDlg.Chart1.Series.Add(allSeries(z))
            Next

            InteropHelper.theDlg.Chart1.ChartAreas(0).AxisY.Minimum = graphMIN
            InteropHelper.theDlg.Chart1.ChartAreas(0).AxisY.Maximum = graphMAX

            'show graph
            InteropHelper.theDlg.TabControl1.SelectTab(2)

        End Function


        Public Sub starDrawLine()
            'tool is in mapbasic - triggers XsecLine function when line is drawn
            InteropServices.MapInfoApplication.Do("run menu command ID 881")
        End Sub


        Public Sub getGridsLayers()
            'clear layer list
            CheckedListBox1.Items.Clear()
            If fPathList.Count > 0 Then
                fPathList.Clear()
                gridNameList.Clear()
            End If

            'get front mapper
            Dim FrontWinID As Integer = InteropServices.MapInfoApplication.Eval("FrontWindow()")

            'check its a mapper
            If InteropServices.MapInfoApplication.Eval("WindowInfo(" & FrontWinID & ",3 )") <> 1 Then
                MsgBox("must be a map window. Type:" & InteropServices.MapInfoApplication.Eval("WindowInfo(" & FrontWinID & ",1)"))
                Exit Sub
            End If
            'get number of layers
            Dim numLayers As Integer = InteropServices.MapInfoApplication.Eval("MapperInfo(" & FrontWinID & ", 9 )")

            'cycle through layers - if grid add to checked list box AND array
            For i As Integer = 1 To numLayers
                If InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 24)") = 4 Then '4 is grid layer
                    fPathList.Add(InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 8 )"))
                    gridNameList.Add(InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 1 )"))
                ElseIf InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 24)") = 2 Then
                    'is image - may be vm grid with tab created from vm.
                    If InteropServices.MapInfoApplication.Eval("RasterTableInfo(" & InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 1 )") & ", 1)").EndsWith(".grd") Then
                        MsgBox(InteropServices.MapInfoApplication.Eval("LayerInfo(" & FrontWinID & ", " & i & ", 1 )") & " is a VM Grid" & vbNewLine & "this can only be recognised by the Profile tool if the grd file (NOT tab file) is re-opened through file>open>(files of type = grid image)")
                    End If

                End If
            Next

            CheckedListBox1.Items.AddRange(gridNameList.ToArray)

        End Sub














        Private Sub ToolStripContainer1_ContentPanel_Load(sender As Object, e As EventArgs) Handles ToolStripContainer1.ContentPanel.Load

        End Sub

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
            'refresh list
            getGridsLayers()
        End Sub

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
            'draw new line
            InteropHelper.theDlg = Me
            starDrawLine()

        End Sub

    

        Sub exportCSV()
            Dim FSA As New SaveFileDialog
            FSA.Filter = "Comma Separated values (*.csv)|*.csv"
            FSA.ShowDialog()
            'validate outname
            If FSA.FileName.EndsWith("\") Then Exit Sub
            If Not FSA.FileName.EndsWith(".csv") Then
                FSA.FileName = FSA.FileName & ".csv"
            End If
            Dim tempString As String = ""

            Dim csvFile As New System.IO.StreamWriter(FSA.FileName)
            'headers
            For h As Integer = 0 To DataGridView1.Columns.Count - 1
                tempString = tempString & "," & DataGridView1.Columns(h).HeaderText
            Next
            csvFile.WriteLine(tempString.Substring(1))

            'for each row
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                'for each column
                tempString = ""
                For j As Integer = 0 To DataGridView1.Columns.Count - 1
                    tempString = tempString & "," & DataGridView1.Rows(i).Cells(j).Value
                Next
                csvFile.WriteLine(tempString.Substring(1))
            Next

            csvFile.Close()


        End Sub

        Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

            saveTAB()
        End Sub

        Sub saveTAB(Optional withPoints As Boolean = 1)
            tableName = ""
            Dim FSA As New SaveFileDialog
            FSA.Filter = "MapInfo Tab (*.tab)|*.tab"
            FSA.ShowDialog()
            'validate outname
            If FSA.FileName.EndsWith("\") Then Exit Sub
            If Not FSA.FileName.EndsWith(".tab") Then
                FSA.FileName = FSA.FileName & ".tab"
            End If

            'create table structure string
            Dim structureString As String
            structureString = "ID integer,Distance float, X float, Y float "
            For h As Integer = 4 To DataGridView1.Columns.Count - 1
                structureString = structureString & "," & DataGridView1.Columns(h).HeaderText & " float"
            Next

            InteropServices.MapInfoApplication.Do("Create Table " & Path.GetFileNameWithoutExtension(FSA.FileName) & " (" & structureString & ")File " & Chr(34) & FSA.FileName & Chr(34) & " Type DBF")
            'open table
            InteropServices.MapInfoApplication.Do("Open Table" & Chr(34) & Path.GetFileNameWithoutExtension(FSA.FileName) & Chr(34))
            'get tab name - may be different if multiple tables opened with same name
            Dim tabName As String = InteropServices.MapInfoApplication.Eval("TableInfo(NumTables(), 1)") 'TODO: what happens when the table is already open
            InteropServices.MapInfoApplication.Do("Create Map For " & tabName)
            InteropServices.MapInfoApplication.Do("Add Map Auto Layer " & tabName)
            'get front win id
            Dim winID As Integer = InteropServices.MapInfoApplication.Eval("FrontWindow()")
            'add points to table
            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                structureString = ""
                For h As Integer = 0 To DataGridView1.Columns.Count - 1
                    structureString = structureString & "," & DataGridView1.Rows(i).Cells(h).Value
                Next

                InteropServices.MapInfoApplication.Do("Insert Into " & tabName & " Values ( " & structureString.Substring(1) & "  )")
            Next

            'set tab name for use with grapher
            tableName = tabName

            If withPoints Then
                createPoints(tabName)
            End If

        End Sub

        Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
            exportCSV()
        End Sub

        Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
            saveChartasimage()
        End Sub


        Sub createPoints(ByVal tableName As String)
            'TODO: set coord system first

            InteropServices.MapInfoApplication.Do("Update " & tableName & " Set obj = CreatePoint(X, Y)")
        End Sub


        Sub saveChartasImage()
            Dim FSA As New SaveFileDialog
            FSA.Filter = "PNG (*.png)|*.png"
            FSA.ShowDialog()
            'validate outname
            If FSA.FileName.EndsWith("\") Then Exit Sub
            If Not FSA.FileName.EndsWith(".png") Then
                FSA.FileName = FSA.FileName & ".png"
            End If

            Chart1.SaveImage(FSA.FileName, Imaging.ImageFormat.Png)
        End Sub

        Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
            saveTAB(False)
            MIGraph()
        End Sub

        Sub MIGraph()
            'validate 
            If tableName = "" Then Exit Sub

            'get app directory with graph templates
            'GetFolderPath$( folder_id ) -4 = FOLDER_MI_COMMON_APPDATA (has slash on end already)
            Dim MIGraphTemplates As String = InteropServices.MapInfoApplication.Eval("GetFolderPath$(-4)") & "GraphSupport\Templates\Line\Clustered.3tf"

            'eg:Graph ID, weirclip2, b3lidar2 From out4 Using "C:\ProgramData\MapInfo\MapInfo\Professional\1150\GraphSupport\Templates\Line\Clustered.3tf" Series In Columns
            'eg v5.5 graph Graph  ID, weirclip2, b3lidar2  From out4 ----->>> set graph statements only work with these
            'get z grid names from datagridview
            Dim zString As String = ""
            For h As Integer = 3 To DataGridView1.Columns.Count - 1
                zString = zString & "," & DataGridView1.Columns(h).HeaderText
            Next

            'build new grapher string
            Dim graphString As String = ""
            'V6  + graph 'graphString = "Graph ID," & zString.Substring(1) & " From " & tableName & " Using " & MIGraphTemplates & " Series In Columns"
            graphString = "Graph  ID, " & zString.Substring(1) & "  From " & tableName & ""
            InteropServices.MapInfoApplication.Do(graphString)

            'get front window
            Dim winID As Integer = InteropServices.MapInfoApplication.Eval("FrontWindow()")
            'check its a grapher

            'change graph options ' B for v5.5 graphs - this will do for this purpose, they look neater.6+ can't be customised through MB or if they can missing documentation at v11.5. Time MI pulled thier finger out and draged this heap into the 21st century
            InteropServices.MapInfoApplication.Do("Set graph window " & winID & "show3d off value axis max " & Chart1.ChartAreas(0).AxisY.Maximum & " min " & Chart1.ChartAreas(0).AxisY.Minimum & "  rotated off type line")


        End Sub

        Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
            Dim AB As New AboutBox1
            AB.ShowDialog()
        End Sub
    End Class
End Namespace