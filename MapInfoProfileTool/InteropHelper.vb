Imports System
Imports System.Windows.Forms
Imports MapInfo.MiPro.Interop

Namespace ProfileTool
    Friend NotInheritable Class InteropHelper

        Public Shared theDlg As Dlg

#Region "[GET APP VERSION]"

        Private Const SYS_INFO_APPVERSION As Integer = 2 ' Used with SystemInfo to get appversion

        ''' <summary>
        ''' Gets the MapInfo Professional version number
        ''' </summary>
        ''' <returns>Version number (multiplied by 100) as string</returns>
        Public Shared Function GetAppVersion() As String
            Dim expr As String = String.Format("SystemInfo({0})", SYS_INFO_APPVERSION)
            Return InteropServices.MapInfoApplication.Eval(expr)
        End Function

#End Region

#Region "[GET FRONT WINDOW]"

        ''' <summary>
        ''' Get front window (child window) from the running instance of MapInfo Professional
        ''' </summary>
        ''' <returns>Id of the front window</returns>
        Private Shared Function GetFrontWindow() As Integer
            Dim evalResult As String = InteropServices.MapInfoApplication.Eval("FrontWindow()")
            Return Int32.Parse(evalResult)

        End Function

#End Region

#Region "[GET WINDOW INFORMATION]"

        Private Const WIN_INFO_TYPE As Integer = 3 ' Used with WindowInfo to get win type

        ''' <summary>
        ''' Returns window type for given window id.
        ''' </summary>
        ''' <param name="windowId"></param>
        ''' <returns>Window type</returns>
        Private Shared Function GetWindowType(ByVal windowId As Integer) As Integer
            ' make sure the front window is a mapper
            Dim expr As String = String.Format("WindowInfo({0}, {1})", windowId, WIN_INFO_TYPE)
            Dim evalResult As String = InteropServices.MapInfoApplication.Eval(expr)
            Return Int32.Parse(evalResult)
        End Function

#End Region

#Region "[GET MAPPER INFORMATION]"

        Private Const MAPPER_INFO_ZOOM As Integer = 1 ' Used with MapperInfo to get zoom
        Private Const MAPPER_INFO_CENTERX As Integer = 3 ' Used with MapperInfo to get center X
        Private Const MAPPER_INFO_CENTERY As Integer = 4 ' Used with MapperInfo to get center Y
        Private Const MAPPER_INFO_DISTUNITS As Integer = 12  ' Used with MapperInfo to get distance units e.g. "mi" 
        Private Const MAPPER_INFO_COORDSYS_CLAUSE_WITH_BOUNDS As Integer = 22  ' Used with MapperInfo to get coordinate system

        ''' <summary>
        ''' Gets the view information from a mapper window in
        ''' MapInfo Professional application
        ''' </summary>
        ''' <remarks>
        ''' MapBasic's MapperInfo function can return numeric information
        ''' such as Zoom width.  However, MapInfoApplication.Eval returns 
        ''' results as strings, so if you request numeric information such
        ''' as MAPPER_INFO_ZOOM, Eval will return a string such as "1234.5"
        ''' (with a period as the decimal separator, regardless of 
        ''' the user's regional settings).  
        ''' 
        ''' Instead of parsing such String results into Double values, we 
        ''' will return the String results.  The string representation
        ''' of numeric values is ideal for this application, because the 
        ''' string formatting returned by the Eval method (i.e. always using 
        ''' the period as the decimal separator) is appropriate for use  
        ''' in the Set Map statement we will be constructing later on.   
        ''' 
        ''' </remarks>
        ''' <param name="windowId">identification number of mapper window</param>
        ''' <param name="infoType">The type of information</param>
        ''' <returns>The requested information</returns>
        Private Shared Function GetMapperInfo(ByVal windowId As Integer, ByVal infoType As Integer) As String
            Dim expr, evalResult As String

            expr = String.Format("MapperInfo({0}, {1})", windowId, infoType)
            evalResult = InteropServices.MapInfoApplication.Eval(expr)
            Return evalResult

        End Function

        ''' <summary>
        ''' Get a string representing the coordinate system of the map window 
        ''' </summary>
        ''' <param name="windowId">identification number of mapper window</param>
        ''' <returns>a CoordSys clause string</returns>
        ''' <remarks></remarks>
        Public Shared Function GetMapperCoordSys(ByVal windowId As Integer) As String
            Dim expr As String

            expr = String.Format("MapperInfo({0}, {1})", windowId, MAPPER_INFO_COORDSYS_CLAUSE_WITH_BOUNDS)
            Return InteropServices.MapInfoApplication.Eval(expr)
        End Function

        ''' <summary>
        ''' Get a string representing the distance unit in use in a specific map window
        ''' </summary>
        ''' <param name="windowId">identification number of mapper window</param>
        ''' <returns>a string representing a distance unit, such as mi or km </returns>
        ''' <remarks></remarks>
        Public Shared Function GetMapperDistanceUnit(ByVal windowId As Integer) As String
            Dim expr As String

            expr = String.Format("MapperInfo({0}, {1})", windowId, MAPPER_INFO_DISTUNITS)
            Return InteropServices.MapInfoApplication.Eval(expr)
        End Function

        ''' <summary>
        ''' Gets mapper window zoom value
        ''' </summary>
        ''' <param name="windowId">identification number of mapper window</param>
        ''' <returns>Zoom value of mapper window's current view</returns>
        Public Shared Function GetMapperZoom(ByVal windowId As Integer) As String
            Return GetMapperInfo(windowId, MAPPER_INFO_ZOOM)
        End Function

        ''' <summary>
        ''' Gets mapper window center X value
        ''' </summary>
        ''' <param name="windowId">window identification number of mapper window</param>
        ''' <returns>Center Y of mapper window's current view</returns>
        Public Shared Function GetMapperCenterX(ByVal windowId As Integer) As String
            Return GetMapperInfo(windowId, MAPPER_INFO_CENTERX)
        End Function

        ''' <summary>
        ''' Gets mapper window center Y value
        ''' </summary>
        ''' <param name="windowId">window identification number of mapper window</param>
        ''' <returns>Center X of mapper window's current view</returns>
        Public Shared Function GetMapperCenterY(ByVal windowId As Integer) As String
            Return GetMapperInfo(windowId, MAPPER_INFO_CENTERY)
        End Function

        ''' <summary>
        ''' Gets a string representing MapInfo's current distance units, such as mi or km. 
        ''' Defaults to "mi" but can be reset through the Set Distance Units statement. 
        ''' </summary>
        ''' <returns>string such as "mi"</returns>
        ''' <remarks></remarks>
        Public Shared Function GetSessionDistanceUnit() As String

            ' Use SessionInfo(SESSION_INFO_DISTANCE_UNITS) to get the unit string
            Return InteropServices.MapInfoApplication.Eval("SessionInfo(2)")
        End Function

        Public Shared Sub SetSessionDistanceUnit(ByVal unit As String)
            Dim expr As String

            expr = String.Format("Set Distance Units ""{0}""", unit)
            InteropServices.MapInfoApplication.Do(expr)
        End Sub

        ''' <summary>
        ''' Get a string representing the CoordSys clause, of the coordinate system
        ''' that is currently in effect. 
        ''' </summary>
        ''' <returns>string such as "CoordSys Earth"</returns>
        ''' <remarks></remarks>
        Public Shared Function GetSessionCoordSys() As String

            ' Make note of the current MapBasic SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            Return InteropServices.MapInfoApplication.Eval("SessionInfo(1)")
        End Function

        ''' <summary>
        ''' Sets the current coordinate system. 
        ''' </summary>
        ''' <param name="csys">string such as "CoordSys Earth"</param>
        ''' <remarks></remarks>
        Public Shared Sub SetSessionCoordSys(ByVal csys As String)
            ' Change the current coordsys by issuing a Set CoordSys statement. 
            InteropServices.MapInfoApplication.[Do](String.Format("Set {0}", csys))
        End Sub

        ''' <summary>
        ''' Given a string representation of a number, in invariant formatting 
        ''' (always using the period as the decimal separator), return a  
        ''' string formatted according to the user's current system settings. 
        ''' </summary>
        ''' <param name="numericString">a number string with period as the decimal separator, if any</param>
        ''' <returns>a number string formatted with the current system settings</returns>
        ''' <remarks>
        ''' The resulting number string is appropriate for displaying numbers 
        ''' in the user interface, but not appropriate for constructing MapBasic 
        ''' statements.  When you construct a MapBasic statement string (to 
        ''' be executed through a call to the Do method), any numeric literals
        ''' in the string must use period as the decimal separator, even if 
        ''' the user's system's regional settings use some other character 
        ''' as the decimal separator. 
        ''' </remarks>
        Public Shared Function GetFormattedString(ByVal numericString As String) As String
            Dim expr As String
            expr = String.Format("FormatNumber$({0})", numericString)
            Return InteropServices.MapInfoApplication.Eval(expr)
        End Function

#End Region

#Region "[SET CURRENT VIEW OF MAPPER WINDOW]"

        ''' <summary>
        ''' Sets the current view of mapper window represented by windowId
        ''' </summary>
        ''' <param name="windowId">Window identification number of mapper window</param>
        ''' <param name="centerX">New center X of the mapper window</param>
        ''' <param name="centerY">New center Y of the mapper window</param>
        ''' <param name="mapperZoom">New zoom of the mapper window</param>
        ''' <param name="unit">Distance unit string that applies to mapperZoom, such as mi or km </param>
        Public Shared Sub SetView(ByVal windowId As Integer, ByVal centerX As String, ByVal centerY As String, ByVal mapperZoom As String, ByVal unit As String, ByVal csys As String)

            Dim oldCSys, setMapStatement As String

            ' Before we do any work involving the map's X/Y coordinates, we
            ' will set the current coordinate system; that way, we will guarantee 
            ' that the coordinates will be processed correctly, regardless of which
            ' coordinate system is in use in the current map. 
            ' But, before we set the coorindate system, make note of the current 
            ' coordinate system, so that we can restore it later.  This way, in
            ' the unlikely event that the user typed a Set CoordSys statement into 
            ' the MapBasic window, we will preserve the coordsys typed in by the user. 

            ' Make note of the current MapBasic Coordinate System, equivalent
            ' to calling:  SessionInfo(SESSION_INFO_COORDSYS_CLAUSE) 
            oldCSys = GetSessionCoordSys()

            ' Set the coordsys clause to the csys that was saved with the named view
            SetSessionCoordSys(csys)

            ' Set the map view 
            setMapStatement = String.Format("Set Map Window {0} Center ( {1}, {2} ) Zoom {3} Units ""{4}""", windowId, centerX, centerY, mapperZoom, unit)
            InteropServices.MapInfoApplication.Do(setMapStatement)

            ' Restore the session coordsys to its previous state. 
            SetSessionCoordSys(oldCSys)

        End Sub

#End Region

#Region "[GET MAPPER WINDOW IDENTIFICATION NUMBER]"

        ''' <summary>
        ''' Get the ID of the front window.  Displays a message
        ''' and returns 0 if there is no window open, or the 
        ''' front window is not a mapper
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetMapWindowId() As Integer
            Dim windowId As Integer = GetFrontWindow()
            ' Make sure we have a window
            If windowId = 0 Then
                MessageBox.Show("Error no window open")
                Return 0
            End If

            Dim windowType As Integer = GetWindowType(windowId)

            ' Make sure if front window is a mapper window
            If windowType <> 1 Then
                MessageBox.Show(My.Resources.ERR_FRONT_WIN_NOT_MAPPER)
                Return 0
            End If

            Return windowId
        End Function

#End Region

    End Class
End Namespace