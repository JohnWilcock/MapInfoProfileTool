Imports System
Imports System.Collections
Imports System.Diagnostics
Imports System.Windows.Forms
Imports System.IO
Imports System.Xml
Imports System.Threading
Imports MapInfo.MiPro.Interop


Namespace ProfileTool
    Public Class Controller

        '' strings for dock persistence xml file
        Private Const STR_DOCK_WINDOW_STATE As String = "DockWindowState"
        Private Const STR_TYPE As String = "Type"
        Private Const STR_FLOATING As String = "Floating"
        Private Const STR_FLOAT_STATE_RECT As String = "FloatStateRect"
        Private Const STR_RIGHT As String = "Right"
        Private Const STR_BOTTOM As String = "Bottom"
        Private Const STR_DOCKED As String = "Docked"
        Private Const STR_DOCK_STATE As String = "DockState"
        Private Const STR_POSITION As String = "Position"
        Private Const STR_CX As String = "CX"
        Private Const STR_CY As String = "CY"
        Private Const STR_PINNED As String = "Pinned"
        Private Const STR_LEFT As String = "Left"
        Private Const STR_TOP As String = "Top"
        Private Const STR_APP_DOCK_INFO_NODE As String = "ProfileToolDockPosInfo"
        '' file with path for docking persistence
        Private sDockInfoXMLFile As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MapInfo\\MapInfo\\Professional\\" + InteropHelper.GetAppVersion() + "\\profiletool_dockinfo.xml"

        Private Shared _Dlg As Dlg = Nothing     '' ProfileTool dialog
        Private Shared _winWrap As WindowWrapper = Nothing
        Private Shared _miApp As MapInfoApplication = Nothing
        Private Shared _dockWindow As DockWindow = Nothing
        Private _dockWindowState As DockWindowState = Nothing
        ''To prevent overwriting of xml storing the docking state Info, by different 
        ''running instance of MapInfo Professional Mutexes will be employed
        Private controllerMutex As Mutex = Nothing
        Private Const controllerMutexName As String = "ProfileTool.ProfileToolDockController" '' Name of the mutex

        ''' <summary>
        ''' Construction
        ''' </summary>
        Public Sub New()
            controllerMutex = New Mutex(False, controllerMutexName)
        End Sub
        ''' <summary>
        ''' This is called from MapBasic code
        ''' to get a named resource string
        ''' </summary>
        ''' <param name="itemName">Name of the string resource</param>
        ''' <returns>Value of the string resource</returns>
        Public Shared Function GetResItemStr(ByVal itemName As String) As String
            Return My.Resources.ResourceManager.GetString(itemName, My.Resources.Culture)
        End Function

        ''' <summary>
        ''' This function is called from MapBasic code
        ''' to display the About dialog.
        ''' </summary>
        ''' <param name="hMainWnd"></param>
        ''' <returns></returns>
        Public Shared Function ShowAboutDlg(ByVal hMainWnd As Integer) As Boolean

        End Function

        ''' <summary>
        ''' This function is called from MapBasic code
        ''' to display the ProfileTool dialog.
        ''' </summary>
        ''' <param name="hMainWnd"></param>
        ''' <returns></returns>
        Public Shared Function ShowDlg(ByVal hMainWnd As Integer) As Boolean
            If _Dlg Is Nothing Then
                _Dlg = New Dlg(New Controller())
                _miApp = InteropServices.MapInfoApplication
                '' Register the window with the docking system
                _dockWindow = _miApp.RegisterDockWindow(_Dlg.Handle)
                _Dlg.SetDockPosition()
                _dockWindow.Title = My.Resources.STR_MB_APP_DESC
            Else
                _dockWindow.Activate()
            End If
            Return True
        End Function
        ''' <summary>
        ''' This function is called from MapBasic code
        ''' to close theProfileTool dialog before exiting.
        ''' </summary>
        Public Shared Function CloseDlg() As Boolean
            If _Dlg IsNot Nothing Then
                _Dlg.CloseDockWindow()
            End If
            If _miApp IsNot Nothing And _dockWindow IsNot Nothing Then
                '' unregister the dock window
                _miApp.UnregisterDockWindow(_dockWindow)
            End If
            If _Dlg IsNot Nothing Then
                '' Clean up the ProfileTool dialog
                _Dlg.Dispose()
            End If

            If _miApp IsNot Nothing Then
                '' Clean up the MapInfoApplication object
                _miApp.Dispose()
                _miApp = Nothing
            End If
            Return True
        End Function
        ''' <summary>
        ''' Returns the window wrapper.
        ''' If window wrapper is null it correctly initializes the static member
        ''' </summary>
        ''' <param name="hMainWnd">Handle to a window</param>
        ''' <returns>Window wrapper for the given handle</returns>
        Private Shared Function GetWindowWrapper(ByVal hMainWnd As Integer) As WindowWrapper
            If _winWrap Is Nothing Then
                Dim hwnd As New IntPtr(hMainWnd)
                Dim winWrap As New WindowWrapper(hwnd)
            End If
            Return _winWrap
        End Function


        ''' <summary>
        ''' Close the dockable usercontrol(ProfileTool Dialog)
        ''' </summary>
        Public Sub DockWindowClose()
            ''store the docking related info in xml file
            WriteDockWindowStateToFile()
            _dockWindow.Close()
        End Sub

        ''' <summary>
        ''' Structure for storing left, right, top, bottom information of floating dialog
        ''' </summary>
        Public Structure RECT
            Public Left As Integer
            Public Top As Integer
            Public Right As Integer
            Public Bottom As Integer
            Public Sub RECT(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
                left = left
                top = top
                right = right
                bottom = bottom
            End Sub
        End Structure


        Public Structure FloatState
            Public Rect As RECT
        End Structure

        ''' <summary>
        ''' Structure for storing docking information
        ''' </summary>
        Public Structure DockState
            Public Position As DockPosition
            Public CX As Integer
            Public CY As Integer
            Public Pinned As Boolean
        End Structure

        ''' <summary>
        ''' Class for docking state related information of dockable window
        ''' </summary>
        Public Class DockWindowState
            Public Floating As Boolean
            Public Visible As Boolean
            Public FloatState As FloatState
            Public DockState As DockState
        End Class


        ''' <summary>
        ''' get the open/close status of dockable user controlProfileTool dialog)
        ''' </summary>
        Public Function GetDockWindowClosed() As Boolean
            Return _dockWindow.Closed
        End Function


        ''' <summary>
        ''' Method for getting the Docking state related information 
        ''' </summary>
        Private Function getDockWindowState() As DockWindowState
            If _dockWindowState Is Nothing Then
                _dockWindowState = New DockWindowState()
            End If

            '' store float state
            _dockWindowState.Floating = _dockWindow.Floating
            '' store float size/location
            _dockWindow.FloatSize(_dockWindowState.FloatState.Rect.Left, _dockWindowState.FloatState.Rect.Top, _dockWindowState.FloatState.Rect.Right, _dockWindowState.FloatState.Rect.Bottom)
            '' store dock position
            _dockWindowState.DockState.Position = _dockWindow.DockPosition
            '' store docked pinned state
            _dockWindowState.DockState.Pinned = _dockWindow.Pinned
            '' store docked size
            _dockWindowState.DockState.CX = _dockWindow.DockSizeCX
            _dockWindowState.DockState.CY = _dockWindow.DockSizeCY
            Return _dockWindowState
        End Function


        ''' <summary>
        ''' Method for Setting the Docking state from xml file containg
        ''' docking persistance information
        ''' </summary>
        Public Sub SetDockWindowPositionFromFile()
            LoadDockWindowStateFromFile()
            ApplyDockWindowState()
        End Sub


        ''' <summary>
        ''' Method for setting some resaonable position/ docked state
        ''' </summary>
        Private Sub SetDefaultDockingInfo()
            If _dockWindowState Is Nothing Then
                _dockWindowState = New DockWindowState()
            End If
            _dockWindowState.Floating = False
            _dockWindowState.DockState.Position = DockPosition.PositionBottom
            _dockWindowState.DockState.CX = 300
            _dockWindowState.DockState.CY = 225
            _dockWindowState.Visible = True
            _dockWindowState.DockState.Pinned = False
        End Sub



        ''' <summary>
        ''' Method for setting the docking and position of dockable window
        ''' </summary>
        Private Sub ApplyDockWindowState()
            If _dockWindowState Is Nothing Then
                '' This can be a case in which the persistence xml file is deleted/ xml
                '' corruption etc. Even if we docking state info does not exist, we will
                '' still like the dialog to open with some resaonable position/ docked state
                _dockWindowState = New DockWindowState()
                SetDefaultDockingInfo()
            End If
            If _dockWindowState.Floating = True Then
                _dockWindow.Float(_dockWindowState.FloatState.Rect.Left, _dockWindowState.FloatState.Rect.Top, _dockWindowState.FloatState.Rect.Right, _dockWindowState.FloatState.Rect.Bottom)
            Else
                _dockWindow.Dock(_dockWindowState.DockState.Position, _dockWindowState.DockState.CX, _dockWindowState.DockState.CY)
                If _dockWindowState.DockState.Pinned = True Then
                    _dockWindow.Pin()
                End If
            End If
        End Sub


        ''' <summary>
        ''' Load the dock window state from the xml file
        ''' It uses mutexes to synchronize the threads accessing
        ''' the xml file
        ''' </summary>
        Private Sub LoadDockWindowStateFromFile()
            Dim sErr As String = String.Empty

            '' Wait until safe to read from file
            controllerMutex.WaitOne()

            '' Try to read the xml file
            Dim xmlDoc As XmlDocument = New XmlDocument()
            Try
                '' Load the xml file
                xmlDoc.Load(sDockInfoXMLFile)

                ''Get the docking related information 
                _dockWindowState = New DockWindowState()

                ''All Docking related information is in "DockWindowState" node and its child nodes
                Dim XmlNodeList As XmlNodeList = xmlDoc.GetElementsByTagName(STR_DOCK_WINDOW_STATE)
                Dim dockWinStateNode As XmlNode = XmlNodeList(0)
                If (dockWinStateNode Is Nothing) Or (dockWinStateNode.Attributes(STR_TYPE) Is Nothing) Then
                    Throw New XmlException(My.Resources.ERR_INVALID_XML)
                End If

                If dockWinStateNode.Attributes(STR_TYPE).Value.CompareTo(STR_FLOATING) = 0 Then
                    '' The dialog was floating the last time the application was closed
                    _dockWindowState.Floating = True
                    Dim FloatStateRectNode As XmlNode = dockWinStateNode.ChildNodes(0)
                    If FloatStateRectNode Is Nothing Then
                        Throw New XmlException(My.Resources.ERR_INVALID_XML)
                    End If
                    _dockWindowState.FloatState.Rect.Top = Convert.ToInt32(FloatStateRectNode.Attributes(STR_TOP).Value)
                    _dockWindowState.FloatState.Rect.Left = Convert.ToInt32(FloatStateRectNode.Attributes(STR_LEFT).Value)
                    _dockWindowState.FloatState.Rect.Right = Convert.ToInt32(FloatStateRectNode.Attributes(STR_RIGHT).Value)
                    _dockWindowState.FloatState.Rect.Bottom = Convert.ToInt32(FloatStateRectNode.Attributes(STR_BOTTOM).Value)
                    _dockWindowState.DockState.Position = DockPosition.PositionFloat
                ElseIf dockWinStateNode.Attributes(STR_TYPE).Value.CompareTo(STR_DOCKED) = 0 Then
                    '' The dialog was docked the last time the application was closed
                    _dockWindowState.Floating = False
                    Dim dockStateNode As XmlNode = dockWinStateNode.ChildNodes(0)
                    If dockStateNode Is Nothing Then
                        Throw New XmlException(My.Resources.ERR_INVALID_XML)
                    End If
                    _dockWindowState.DockState.Position = ([Enum].Parse(GetType(DockPosition), dockStateNode.Attributes(STR_POSITION).Value))
                    _dockWindowState.DockState.Pinned = Convert.ToBoolean(dockStateNode.Attributes(STR_PINNED).Value)
                    _dockWindowState.DockState.CX = Convert.ToInt32(dockStateNode.Attributes(STR_CX).Value)
                    _dockWindowState.DockState.CY = Convert.ToInt32(dockStateNode.Attributes(STR_CY).Value)
                End If

            Catch ex As System.Xml.XPath.XPathException
                sErr = ex.Message
            Catch ex As XmlException
                sErr = ex.Message
            Catch ex As ArgumentException
                sErr = ex.Message
            Catch ex As FileNotFoundException
                sErr = String.Empty
            End Try


            If sErr.Equals(String.Empty) = False Then
                MessageBox.Show(sErr)
            End If

            ''release the mutex
            controllerMutex.ReleaseMutex()


        End Sub


        ''' <summary>
        ''' Write the dock window state to file
        ''' It uses mutexes to synchronize the threads accessing
        ''' the xml file
        ''' </summary>
        Private Sub WriteDockWindowStateToFile()
            Dim sErr As String = String.Empty

            ''wait until safe to read from file
            controllerMutex.WaitOne()
            Try
                Dim xw As XmlTextWriter = New XmlTextWriter(sDockInfoXMLFile, System.Text.Encoding.Unicode)

                '' Use indenting for readability.
                xw.Formatting = Formatting.Indented

                '' write the XML declaration
                xw.WriteStartDocument()

                '' write the Root node 
                xw.WriteStartElement(STR_APP_DOCK_INFO_NODE)
                getDockWindowState()

                ''Write the docking related information
                If (_dockWindowState.Floating = True) Then
                    ''DockWindowState node contains docking and position related info
                    xw.WriteStartElement(STR_DOCK_WINDOW_STATE)
                    xw.WriteAttributeString(STR_TYPE, STR_FLOATING)
                    xw.WriteStartElement(STR_FLOAT_STATE_RECT)
                    ''start the FloatStateRect node
                    xw.WriteAttributeString(STR_TOP, Convert.ToString(_dockWindowState.FloatState.Rect.Top))
                    xw.WriteAttributeString(STR_LEFT, Convert.ToString(_dockWindowState.FloatState.Rect.Left))
                    xw.WriteAttributeString(STR_RIGHT, Convert.ToString(_dockWindowState.FloatState.Rect.Right))
                    xw.WriteAttributeString(STR_BOTTOM, Convert.ToString(_dockWindowState.FloatState.Rect.Bottom))
                    '' end the FloatStateRect node 
                    xw.WriteEndElement()
                    '' end the DockWindowState node 
                    xw.WriteEndElement()
                Else
                    ''DockWindowState node contains docking and position related info
                    xw.WriteStartElement(STR_DOCK_WINDOW_STATE)
                    xw.WriteAttributeString(STR_TYPE, STR_DOCKED)
                    ''start the DockState node
                    xw.WriteStartElement(STR_DOCK_STATE)
                    xw.WriteAttributeString(STR_POSITION, Convert.ToString(_dockWindowState.DockState.Position))
                    xw.WriteAttributeString(STR_CX, Convert.ToString(_dockWindowState.DockState.CX))
                    xw.WriteAttributeString(STR_CY, Convert.ToString(_dockWindowState.DockState.CY))
                    xw.WriteAttributeString(STR_PINNED, Convert.ToString(_dockWindowState.DockState.Pinned))
                    '' end the DockState node
                    xw.WriteEndElement()
                    '' end the DockWindowState node
                    xw.WriteEndElement()
                End If


                '' end the root element (represent the tool)
                xw.WriteEndElement()

                '' finish the write operation
                xw.Flush()
                xw.Close()

            Catch ex As DirectoryNotFoundException
                sErr = ex.Message
            Catch ex As IOException
                sErr = ex.Message
            Catch ex As UnauthorizedAccessException
                sErr = ex.Message
            Catch ex As InvalidOperationException
                sErr = ex.Message
            Catch ex As ArgumentException
                sErr = ex.Message
            End Try

            If sErr.Equals(String.Empty) = False Then
                MessageBox.Show(sErr)
            End If
            ''release the mutex
            controllerMutex.ReleaseMutex()
        End Sub


        ''' <summary>
        ''' This class implements IWin32Window wrapping a handle to window.
        ''' It is used to wrap the handle to a running instance of 
        ''' MapInfo Professional application.
        ''' </summary>
        Public Class WindowWrapper
            Implements System.Windows.Forms.IWin32Window
            Public Sub New(ByVal handle As IntPtr)
                _hwnd = handle
            End Sub

            Public ReadOnly Property Handle() As IntPtr Implements System.Windows.Forms.IWin32Window.Handle
                Get
                    Return _hwnd
                End Get
            End Property

            Private _hwnd As IntPtr
        End Class
    End Class
End Namespace
