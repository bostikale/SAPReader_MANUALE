Module Settings

    Public currPath As String = My.Application.Info.DirectoryPath

    Public StringConnection As String

    Public Destinazione As String

    Public SettingsArray(1, 4) As String

    Public Trovato(1) As Boolean

    Public Quanti As Integer

    Public Const Nome As Integer = 1
    Public Const Tipo As Integer = 2
    Public Const Percorso As Integer = 3
    Public Const Tabella As Integer = 4

    Public LogFile As String = "C:\temp\log.log"

    Public Verso As String = "IN"


    Private Declare Auto Function GetPrivateProfileString Lib “kernel32.dll” (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Public Function IniRead(ByVal Filename As String, ByVal Section As String, ByVal Key As String, Optional ByVal lpDefault As String = “”, Optional ByVal bRaiseError As Boolean = False) As String

        Dim RetVal As String = New String(” “, 255)

        Dim LenResult As Integer

        Dim ErrString As String

        LenResult = GetPrivateProfileString(Section, Key, lpDefault, RetVal, RetVal.Length, Filename)

        If LenResult = 0 AndAlso bRaiseError Then

            If Not (System.IO.File.Exists(Filename)) Then

                ErrString = “Impossibile aprire il file INI” & Filename

            Else

                ErrString = “La sezione o la chiave sono errate oppure l’accesso al file non è consentito”

            End If

            Throw New Exception(ErrString)

        End If

        Return RetVal.Substring(0, LenResult)

    End Function
End Module
