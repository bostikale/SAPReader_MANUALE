Imports System.Xml
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mail
Public Class Form1
    Dim settingsPath As String = currPath + "\settings.ini"
    Dim timerSAPIN As System.Timers.Timer = New System.Timers.Timer()
    Dim CommandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
    Dim IsAuto As Boolean = False
    Dim Cosa As String = ""
    Dim Orari As New DataTable 'tabella che contiene le schedulazioni

    Private Sub ScriviLog(FILE_NAME As String, ByVal cosa As String)

        Using w As StreamWriter = File.AppendText(FILE_NAME)

            w.Write(vbCrLf + "Log Entry : ")
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString())
            w.WriteLine("  :")
            w.WriteLine("  :{0}", cosa)
            w.WriteLine("-------------------------------")

        End Using

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        ScriviLog(LogFile, "Inizio")

        'For i As Integer = 0 To CommandLineArgs.Count - 1
        'FaiAzioni(CommandLineArgs(i))
        'IsAuto = False
        'Next

        Dim fileName As String = currPath & "\TIMER_SAP_READER.xml"

        Orari.ReadXml(fileName)

        For i As Integer = 0 To CommandLineArgs.Count - 1

            If CommandLineArgs(i) = "/TIMER" Then
                Timer1.Interval = 1000
                Timer1.Start()
                Button1.Enabled = False
            End If

        Next



    End Sub
    Private Sub FaiAzioni(currCommandLineArg As String)

        Cosa = currCommandLineArg

        Button1.Enabled = False

        If currCommandLineArg = "/M" Then       ' import massivo
            settingsPath = currPath + "\massivo.ini"
            Verso = "IN"
        ElseIf currCommandLineArg = "/A" Then   ' import aperti
            settingsPath = currPath + "\aperto.ini"
            Verso = "IN"
        ElseIf currCommandLineArg = "/P" Then   ' import priorità
            settingsPath = currPath + "\priorita.ini"
            Verso = "IN"
        ElseIf currCommandLineArg = "/D" Then   ' import delta
            settingsPath = currPath + "\delta.ini"
            Verso = "IN"
        ElseIf currCommandLineArg = "/V" Then   ' versamenti di avvisi e matricole
            settingsPath = currPath + "\inserisci.ini"
            Verso = "OUT"
        ElseIf currCommandLineArg = "/G" Then   ' nell'import del GOAF faccio tutto ed esco
            settingsPath = currPath + "\goaf.ini"

            StringConnection = IniRead(settingsPath, "CONNECTION", "STRING")


            ' IMPORTO I DATI
            ScriviLog(LogFile, "Caricamento dati in tabella: Solleciti_SAP")
            Destinazione = IniRead(settingsPath, "PERCORSI", "A")
            SettingsArray(1, Percorso) = IniRead(settingsPath, "PERCORSI", "DA")

            For Each file As String In Directory.GetFiles(SettingsArray(1, Percorso), "*.csv")

                ' CANCELLO I DATI DALLA TABELLA
                ScriviLog(LogFile, "Cancellazione Tabella: Solleciti_SAP")
                DeleteData(StringConnection, "DELETE FROM Solleciti_SAP")

                ScriviLog(LogFile, "Caricamento CSV: " & file)
                CaricaDatiGOAF(file)

                'SPOSTO I FILE
                Dim filename As String
                filename = Path.GetFileNameWithoutExtension(file)

                Dim myFileName As String = String.Format(Destinazione & filename & "_{0}.CSV", Now.ToString("yyyyMMddhhmmss.fff"))

                ScriviLog(LogFile, "Spostamento CSV: " & myFileName)
                My.Computer.FileSystem.MoveFile(file, myFileName)

            Next

            ' ELABORO I DATI
            ScriviLog(LogFile, "Lancio Stored Procedure")
            EXECSP(StringConnection, "ElaboraSAP", "")


            ' FINE ELABORAZIONE
            ScriviLog(LogFile, "Fine")
            'InviaMail("ELABORAZIONE X GOAF COMPLETATA.", "")
            GoTo fine
            'Button1.Enabled = True
            'Application.Exit()
            'End

        ElseIf currCommandLineArg = "/B" Then       ' nell'import delle timbrature da Board faccio tutto ed esco
            settingsPath = currPath + "\board.ini"

            StringConnection = IniRead(settingsPath, "CONNECTION", "STRING")

            ' IMPORTO I DATI
            ScriviLog(LogFile, "Caricamento dati in tabella: BOARD")
            'SettingsArray(1, Percorso) = IniRead(settingsPath, "PERCORSI", "DA")

            ' CANCELLO I DATI DALLA TABELLA
            'ScriviLog(LogFile, "Cancellazione Tabella: BOARD")
            'DeleteData(StringConnection, "DELETE FROM [TIMBRATURE]")


            'For Each file As String In Directory.GetFiles(SettingsArray(1, Percorso), "*.csv")

            'If Path.GetFileName(file) = "TIMBRATURE.CSV" Then
            'ScriviLog(LogFile, "Caricamento CSV: " & file)
            'CaricaDatiBoard(file)
            'End If

            'Next

            ' eseguo SP
            ScriviLog(LogFile, "Lancio Stored Procedure")

            EXECSP(StringConnection, "IMPORTA_TIMBRATURE", "")


            ' FINE ELABORAZIONE
            ScriviLog(LogFile, "Fine")
            'InviaMail("ELABORAZIONE X BOARD COMPLETATA.", "")
            GoTo fine
            'Button1.Enabled = True
            'Application.Exit()
            'End

        ElseIf currCommandLineArg = "/T" Then       ' versamento dei tempi consuntivi

            settingsPath = currPath + "\tempi.ini"

            InizializzaFileINI(False)

            Verso = "OUT"


            For Each file As String In Directory.GetFiles(SettingsArray(1, Percorso), "*.csv")

                If Not ChiamataASAPOUT(1) Then
                    ScriviLog(LogFile, "Chiamata a SAP abortita")
                    Trovato(1) = True
                End If

            Next

            ScriviLog(LogFile, "Fine")
            'InviaMail("ESPORATAZIONE TEMPI COMPLETATA.", "")
            GoTo fine
            'Button1.Enabled = True
            'Application.Exit()
            'End
        ElseIf currCommandLineArg = "/R" Then       ' aggiornamento singolo articolo
            Dim Articolo As String

            Articolo = InputBox("Quale articolo vuoi aggiornare?", "Articolo", "")

            If Articolo = "" Then
                MsgBox("L'articolo deve essere dichiarato.", MsgBoxStyle.Exclamation And MsgBoxStyle.OkOnly, "Attenzione!")
                GoTo fine
            End If

            ' carico i dati in tabella


            ' aggiorno i matchcode



        End If





        ' in base al verso inizializzo uno o l'altro file ini
        If Verso = "IN" Then

            InizializzaFileINI(True)

        ElseIf Verso = "OUT" Then

            InizializzaFileINI(False)

        End If


        ScriviLog(LogFile, "File Ini Inizializzato")



        ' mi setto i boleani a false per il timer
        InizializzaTrovato()

		'@@@@@@@ non li sposto più!!!!!
		'' mi sposto i file presenti nelle cartelle prima di fare le chiamate
		''        If Verso = "IN" Then
		'ScriviLog(LogFile, "Sposto i file che sono nelle cartelle")
		'Dim filenameWE As String

		'For x = 1 To Quanti

		'    For Each file As String In Directory.GetFiles(SettingsArray(x, Percorso), "*.csv")

		'        filenameWE = file

		'        'sposto il file
		'        filenameWE = Path.GetFileNameWithoutExtension(file)
		'        ScriviLog(LogFile, "Spostamento CSV: " & Destinazione & filenameWE & "_NI.CSV")
		'        My.Computer.FileSystem.MoveFile(file, Destinazione & filenameWE & "_NI.CSV")

		'    Next

		'Next
		'       End If




		'@@@@@@ non chiamo più SAP
		' chiamate in entrata/uscita a SAP
		'ScriviLog(LogFile, "Inizio chiamate a SAP")

		'      For x = 1 To Quanti

		'          If Verso = "IN" Then
		'		If SettingsArray(x, Nome) = "Z_SER_ODACONTOLAV" Then
		'			If Not ChiamataASAP2(x) Then
		'				ScriviLog(LogFile, "Chiamata a SAP abortita")
		'				Trovato(x) = True
		'			End If
		'		ElseIf SettingsArray(x, Nome) = "Z_ANAGR_ARTIC_PIAN" Then
		'			If Not ChiamataASAP4(x) Then
		'				ScriviLog(LogFile, "Chiamata a SAP abortita")
		'				Trovato(x) = True
		'			End If
		'		ElseIf SettingsArray(x, Nome) = "Z_DISRIC" Then
		'			If Not ChiamataASAP5(x) Then
		'				ScriviLog(LogFile, "Chiamata a SAP abortita")
		'				Trovato(x) = True
		'			End If
		'		Else
		'                  If Not ChiamataASAP(x) Then
		'                      ScriviLog(LogFile, "Chiamata a SAP abortita")
		'                      Trovato(x) = True
		'                  End If
		'              End If
		'          End If

		'          If Verso = "OUT" Then

		'              ExportCSV(x)
		'              ScriviLog(LogFile, "FIle CSV creato")

		'          End If

		'      Next

		' avvio il timer in attesa di elaborare i file
		If Verso = "IN" Then
            'timerSAPIN.Interval = 5000 ' 5 secondi
            'AddHandler timerSAPIN.Elapsed, AddressOf Me.OnTimer
            'timerSAPIN.Start()
            CaricaDatiTabelle2()
        End If

        ' altrimenti chiudo l'applicazione
        If Verso = "OUT" Then

            ScriviLog(LogFile, "Fine")
            'InviaMail("ESPORATAZIONE DATI COMPLETATA. (Versamenti)", "")
            GoTo fine
            'Button1.Enabled = True
            'Application.Exit()
            'End
        End If

        'Timer1.Start()

fine:

    End Sub

    Private Sub CaricaDatiTabelle2()

        ' verifico  il giro di tutti i file coinvolti
        Dim filename As String
        Dim stpw As Stopwatch
        Dim timeout As TimeSpan = New TimeSpan(0, 5, 0) 'ten minutes

        Dim x As Integer
        For x = 1 To Quanti

            stpw = Stopwatch.StartNew
            Do


                For Each file As String In Directory.GetFiles(SettingsArray(x, Percorso), "*.csv")

                    filename = file

                    ' pulisco la tabella corrispondente
                    If SettingsArray(x, Nome) <> "Z_SER_ODACONTOLAV" And SettingsArray(x, Nome) <> "Z_ODA_CONTO_LAVOR" And SettingsArray(x, Nome) <> "Z_MATERIALI_ODA_CONTO_LAV" Then
                        ScriviLog(LogFile, "Cancellazione Tabella: " & SettingsArray(x, Tabella))
                        DeleteData(StringConnection, "delete From " & SettingsArray(x, Tabella))
                    End If

                    'carico i dati nel db
                    ScriviLog(LogFile, "Caricamento CSV: " & filename)
                    CaricaDati(x, filename)
                    'sposto il file
                    filename = Path.GetFileName(file)
                    ScriviLog(LogFile, "Spostamento CSV: " & Destinazione & filename)
                    My.Computer.FileSystem.MoveFile(file, Destinazione & filename)

                    Trovato(x) = True
                    Exit Do

                Next

            Loop While stpw.Elapsed < timeout

        Next

        For x = 1 To Quanti
            If Trovato(x) = False Then
                InviaMail("NON HO TROVATO IL FILE -->" & SettingsArray(x, Percorso), "")
                Exit Sub
            End If
        Next


		'@@@@@@@ faccio tutto a mano
		'      ' importo i dati in produzione
		'      ScriviLog(LogFile, "Lancio Stored Procedure")
		'If Cosa <> "/P" Then
		'	EXECSP(StringConnection, "CARICAMENTO_MASSIVO", "'" & Cosa & "'")
		'Else
		'	EXECSP(StringConnection, "ASSEGNA_PRIORITA", "")
		'End If

		ScriviLog(LogFile, "Fine")
        'InviaMail("IMPORTAZIONE DATI COMPLETATA. " & Cosa, "")



    End Sub


    Private Sub CaricaDati(x As Integer, fileN As String)

        Dim dt As New DataTable()
        Dim line As String = Nothing
        Dim i As Integer = 0
        Dim trovato As Boolean = False

        Try

            Using sr As StreamReader = File.OpenText(fileN)
                line = sr.ReadLine()
                Do While line IsNot Nothing
                    Dim data() As String = line.Split(";"c)
                    If data.Length > 0 Then
                        If i = 0 Then
                            For Each item In data
                                dt.Columns.Add(New DataColumn())
                            Next item
                            i += 1
                        End If

                        If SettingsArray(x, Tabella) = "AVANZAMENTO_LISTE_2" Then
                            dt.Columns(4).DataType = System.Type.GetType("System.DateTime")
                        End If

                        If SettingsArray(x, Tabella) = "FATTIBILITA_PRODUZIONE_2" Then
                            dt.Columns(1).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(2).DataType = System.Type.GetType("System.DateTime")
                        End If

                        If SettingsArray(x, Tabella) = "MATERIALI_OP_2" Then
                            dt.Columns(3).DataType = System.Type.GetType("System.Double")
                        End If

                        If SettingsArray(x, Tabella) = "TESTATE_OP_2" Then
                            dt.Columns(8).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(9).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(10).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(11).DataType = System.Type.GetType("System.DateTime")
                        End If

                        If SettingsArray(x, Tabella) = "CICLI_OP_2" Then
                            dt.Columns(5).DataType = System.Type.GetType("System.Double")
                            dt.Columns(6).DataType = System.Type.GetType("System.Double")
                            dt.Columns(7).DataType = System.Type.GetType("System.Double")
                        End If

                        If SettingsArray(x, Tabella) = "BOLLE_DI_LAVORO_2" Then
                            dt.Columns(3).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(4).DataType = System.Type.GetType("System.DateTime")
                            dt.Columns(5).DataType = System.Type.GetType("System.Double")
                            dt.Columns(6).DataType = System.Type.GetType("System.Double")
                            dt.Columns(7).DataType = System.Type.GetType("System.Double")
                        End If


                        Dim row As DataRow = dt.NewRow()

                        ' in caso di date setto il campo correttamente
                        If SettingsArray(x, Tabella) = "AVANZAMENTO_LISTE_2" Then
                            If Mid(data(4).ToString(), 1, 1) = "0" Then
                                data(4) = Nothing
                            ElseIf Mid(data(4).ToString, 1, 1) = "2" Then
                                data(4) = Mid(data(4).ToString, 7, 2) & "/" & Mid(data(4).ToString, 5, 2) & "/" & Mid(data(4).ToString, 1, 4)
                            End If
                        End If

                        If SettingsArray(x, Tabella) = "TESTATE_OP_2" Then

                            data(5) = Replace(data(5), ".", ",")
                            data(6) = Replace(data(6), ".", ",")


                            If Mid(data(8).ToString(), 1, 1) = "0" Then
                                data(8) = Nothing
                            ElseIf Mid(data(8).ToString, 1, 1) = "2" Then
                                data(8) = Mid(data(8).ToString, 7, 2) & "/" & Mid(data(8).ToString, 5, 2) & "/" & Mid(data(8).ToString, 1, 4)
                            End If

                            If Mid(data(9).ToString(), 1, 1) = "0" Then
                                data(9) = Nothing
                            ElseIf Mid(data(9).ToString, 1, 1) = "2" Then
                                data(9) = Mid(data(9).ToString, 7, 2) & "/" & Mid(data(9).ToString, 5, 2) & "/" & Mid(data(9).ToString, 1, 4)
                            End If

                            If Mid(data(10).ToString(), 1, 1) = "0" Then
                                data(10) = Nothing
                            ElseIf Mid(data(10).ToString, 1, 1) = "2" Then
                                data(10) = Mid(data(10).ToString, 7, 2) & "/" & Mid(data(10).ToString, 5, 2) & "/" & Mid(data(10).ToString, 1, 4)
                            End If

                            If Mid(data(11).ToString(), 1, 1) = "0" Then
                                data(11) = Nothing
                            ElseIf Mid(data(11).ToString, 1, 1) = "2" Then
                                data(11) = Mid(data(11).ToString, 7, 2) & "/" & Mid(data(11).ToString, 5, 2) & "/" & Mid(data(11).ToString, 1, 4)
                            End If

                        End If

                        If SettingsArray(x, Tabella) = "BOLLE_DI_LAVORO_2" Then

                            data(5) = Replace(data(5), ".", ",")
                            data(6) = Replace(data(6), ".", ",")
                            data(7) = Replace(data(7), ".", ",")

                            If Mid(data(3).ToString(), 1, 1) = "0" Then
                                data(3) = Nothing
                            ElseIf Mid(data(3).ToString, 1, 1) = "2" Then
                                data(3) = Mid(data(3).ToString, 7, 2) & "/" & Mid(data(3).ToString, 5, 2) & "/" & Mid(data(3).ToString, 1, 4)
                            End If

                            If Mid(data(4).ToString(), 1, 1) = "0" Then
                                data(4) = Nothing
                            ElseIf Mid(data(4).ToString, 1, 1) = "2" Then
                                data(4) = Mid(data(4).ToString, 7, 2) & "/" & Mid(data(4).ToString, 5, 2) & "/" & Mid(data(4).ToString, 1, 4)
                            End If

                        End If

                        If SettingsArray(x, Tabella) = "FATTIBILITA_PRODUZIONE_2" Then
                            If Mid(data(1).ToString(), 1, 1) = "0" Then
                                data(1) = Nothing
                            ElseIf Mid(data(1).ToString, 1, 1) = "2" Then
                                data(1) = Mid(data(1).ToString, 7, 2) & "/" & Mid(data(1).ToString, 5, 2) & "/" & Mid(data(1).ToString, 1, 4)
                            End If

                            If Mid(data(2).ToString(), 1, 1) = "0" Then
                                data(2) = Nothing
                            ElseIf Mid(data(2).ToString, 1, 1) = "2" Then
                                data(2) = Mid(data(2).ToString, 7, 2) & "/" & Mid(data(2).ToString, 5, 2) & "/" & Mid(data(2).ToString, 1, 4)
                            End If
                        End If

                        ' in caso di numeri setto il campo correttamente
                        If SettingsArray(x, Tabella) = "RIGHE_DOCUMENTO_MOVIMENTAZIONE_2" Then
                            data(5) = Replace(data(5), ".", ",")
                        End If

                        If SettingsArray(x, Tabella) = "MATERIALI_OP_2" Then
                            data(3) = Replace(data(3), ".", ",")
                        End If

                        If SettingsArray(x, Tabella) = "CICLI_OP_2" Then
                            data(5) = Replace(data(5), ".", ",")
                            data(6) = Replace(data(6), ".", ",")
                            data(7) = Replace(data(7), ".", ",")
                        End If

                        If SettingsArray(x, Tabella) = "ANAGRAFICA_ARTICOLI_DATI_PIAN_2" Then
                            data(3) = Replace(data(3), ".", ",")
                        End If

                        If SettingsArray(x, Tabella) = "ANAGRAFICA_ARTICOLI_2" Then
                            data(5) = Replace(data(5), "ST", "PZ")
                        End If

                        If SettingsArray(x, Tabella) = "ANAGRAFICA_ARTICOLI_DATI_ACQ_2" Then
                            If data(2) = "ST" Then
                                data(2) = "8"
                            ElseIf data(2) = "AL" Then
                                data(2) = "7"
                            ElseIf data(2) = "BE" Then
                                data(2) = "6"
                            ElseIf data(2) = "GA" Then
                                data(2) = "5"
                            End If
                        End If


                        If SettingsArray(x, Tabella) = "MATRICOLE_REALI_2" Then

                            data(0) = Mid(data(0), 9, 10)

                            If data(1).ToString() = "DISP" Then
                                data(1) = "0"
                            ElseIf data(1).ToString() = "CLIE" Then
                                data(1) = "4"
                            ElseIf data(1).ToString() = "MAG" Then
                                data(1) = "1"
                            Else
                                data(1) = data(1)
                            End If

                        End If

                        row.ItemArray = data

                        trovato = False

                        'accorpa i testi delle descrizioni

                        If (SettingsArray(x, Tabella) = "ANAGRAFICA_ARTICOLI_2") Or (SettingsArray(x, Tabella) = "TESTATA_OP_2") Or (SettingsArray(x, Tabella) = "MATRICOLE_REALI_2") Then

                            Dim ia As Integer = (dt.Rows.Count - 1)
                            Do While (ia >= 0)
                                If dt.Rows(ia)("Column1") = row.Item(0).ToString Then
                                    trovato = True

                                    If SettingsArray(x, Tabella) = "ANAGRAFICA_ARTICOLI_2" Then
                                        dt.Rows(ia)("Column3") = Mid(dt.Rows(ia)("Column3") & Chr(13) & row.Item(2).ToString, 1, 256)
                                    End If

                                    If SettingsArray(x, Tabella) = "MATRICOLE_REALI_2" Then
                                        dt.Rows(ia)("Column2") = Mid(dt.Rows(ia)("Column2") & "/" & row.Item(1).ToString, 1, 30)
                                    End If

                                    If SettingsArray(x, Tabella) = "TESTATE_OP_2" Then
                                        dt.Rows(ia)("Column2") = Mid(dt.Rows(ia)("Column2") & "/" & row.Item(1).ToString, 1, 30)
                                    End If

                                End If
                                ia = ia - 1
                            Loop
                            'fine accorpamento
                        End If

                        If (SettingsArray(x, Tabella) = "DESCRIZIONI_LINGUA_ARTICOLI_2") Then

                            Dim ia As Integer = (dt.Rows.Count - 1)
                            Do While (ia >= 0)
                                If dt.Rows(ia)("Column1") = row.Item(0).ToString And dt.Rows(ia)("Column3") = row.Item(2).ToString Then
                                    trovato = True

                                    dt.Rows(ia)("Column4") = Mid(dt.Rows(ia)("Column4") & Chr(13) & row.Item(3).ToString, 1, 256)


                                End If
                                ia = ia - 1
                            Loop
                            'fine accorpamento
                        End If

                        If Not trovato Then
                            dt.Rows.Add(row)
                        End If


                    End If
                    line = sr.ReadLine()
                Loop
            End Using



            Using cn As New SqlConnection(StringConnection)
                cn.Open()
                Using copy As New SqlBulkCopy(cn)

                    For i = 0 To dt.Columns.Count - 1
                        copy.ColumnMappings.Add(i, i)
                    Next

                    copy.DestinationTableName = SettingsArray(x, Tabella)
                    copy.WriteToServer(dt)
                End Using
            End Using

        Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
        End Try



    End Sub
    Private Sub InizializzaFileINI(come As Boolean)



        ' il file settings esiste?
        If Not My.Computer.FileSystem.FileExists(settingsPath) Then
            MsgBox("Il file INI non esiste.")
            Exit Sub
        End If


        StringConnection = IniRead(settingsPath, "CONNECTION", "STRING")
        Quanti = CInt(IniRead(settingsPath, "ELABORAZIONI", "QUANTE"))
        Destinazione = IniRead(settingsPath, "ELABORAZIONI", "DESTINAZIONE")
        LogFile = IniRead(settingsPath, "LOG", "DOVE")

        ReDim SettingsArray(Quanti, 4)


        Dim x As Integer

        For x = 1 To Quanti

            SettingsArray(x, Nome) = IniRead(settingsPath, "ELABORAZIONI", "NOME" & x)
            If come Then SettingsArray(x, Tipo) = IniRead(settingsPath, "ELABORAZIONI", "TIPO" & x)
            SettingsArray(x, Percorso) = IniRead(settingsPath, "ELABORAZIONI", "PERCORSO" & x)
            SettingsArray(x, Tabella) = IniRead(settingsPath, "ELABORAZIONI", "TABELLA" & x)

        Next

    End Sub
    Private Sub InizializzaTrovato()
        ReDim Trovato(Quanti)
        Dim x As Integer

        For x = 1 To Quanti
            Trovato(x) = False
        Next
    End Sub

    Private Function ChiamataASAP(position As Integer) As Boolean

        ScriviLog(LogFile, "Chiamata: " & SettingsArray(position, Nome) & " Tipo:" & SettingsArray(position, Tipo))

        Try


			Dim WS As New SapSOAPin.ZWS_MES_INBOUND_1
			Dim WS_Input As New SapSOAPin.ZMES_WS_IN
			Dim WS_Output As New SapSOAPin.ZMES_WS_INResponse



			WS.Credentials = New System.Net.NetworkCredential("mes_user", "$tUlz2019!")

			WS_Input.I_ARTICOLO = ""

			WS_Input.I_ID_EVENTO = SettingsArray(position, Nome)

			WS_Input.I_TIPO_ELABORAZIONE = SettingsArray(position, Tipo)

			WS_Output = WS.ZMES_WS_IN(WS_Input)

			ScriviLog(LogFile, WS_Output.E_RETCODE.ToString)
			ScriviLog(LogFile, WS_Output.E_MESSAGE.ToString)


			If WS_Output.E_RETCODE <> 0 Then
				Return False
				Exit Function
			End If



			'MsgBox(WS_Output.EMessage)

			'If WS_Output.EMessage.Contains("OK") = False Then
			'Return False
			'Exit Function
			'End If

		Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
            Return False
            Exit Function
        End Try

        Return True

    End Function
    Private Function ChiamataASAP2(position As Integer) As Boolean


        ScriviLog(LogFile, "Chiamata: " & SettingsArray(position, Nome) & " Tipo:" & SettingsArray(position, Tipo))

        Try

            Dim WS As New SapSOAPin2.ZWS_MES_INBOUND_2
            Dim WS_Input As New SapSOAPin2.ZmesWsIn2
            Dim WS_Output As New SapSOAPin2.ZmesWsIn2Response



            WS.Credentials = New System.Net.NetworkCredential("mes_user", "12345678")


            WS_Input.IIdEvento = SettingsArray(position, Nome)

            WS_Input.ITipoElaborazione = SettingsArray(position, Tipo)

            WS_Output = WS.ZmesWsIn2(WS_Input)

            ScriviLog(LogFile, WS_Output.ERetcode.ToString)
            ScriviLog(LogFile, WS_Output.EMessage.ToString)


            If WS_Output.ERetcode <> 0 Then
                Return False
                Exit Function
            End If

            'MsgBox(WS_Output.EMessage)

            'If WS_Output.EMessage.Contains("OK") = False Then
            'Return False
            'Exit Function
            'End If

        Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
            Return False
            Exit Function
        End Try

        Return True

    End Function
	Private Function ChiamataASAP4(position As Integer) As Boolean


		ScriviLog(LogFile, "Chiamata: " & SettingsArray(position, Nome) & " Tipo:" & SettingsArray(position, Tipo))

		Try

			Dim WS As New SAPSOAPin4.ZWS_MES_INBOUND_4
			Dim WS_Input As New SAPSOAPin4.ZmesWsIn4
			Dim WS_Output As New SAPSOAPin4.ZmesWsIn4Response



			WS.Credentials = New System.Net.NetworkCredential("mes_user", "12345678")


			WS_Input.IIdEvento = SettingsArray(position, Nome)

			WS_Input.ITipoElaborazione = SettingsArray(position, Tipo)

			WS_Output = WS.ZmesWsIn4(WS_Input)

			ScriviLog(LogFile, WS_Output.ERetcode.ToString)
			ScriviLog(LogFile, WS_Output.EMessage.ToString)


			If WS_Output.ERetcode <> 0 Then
				Return False
				Exit Function
			End If

			'MsgBox(WS_Output.EMessage)

			'If WS_Output.EMessage.Contains("OK") = False Then
			'Return False
			'Exit Function
			'End If

		Catch ex As Exception
			ScriviLog(LogFile, ex.Message.ToString)
			InviaMail(ex.Message.ToString, "")
			Return False
			Exit Function
		End Try

		Return True

	End Function
	Private Function ChiamataASAP5(position As Integer) As Boolean


		ScriviLog(LogFile, "Chiamata: " & SettingsArray(position, Nome) & " Tipo:" & SettingsArray(position, Tipo))

		Try

			Dim WS As New SAPSOAPin5.ZWS_MES_INBOUND_5
			Dim WS_Input As New SAPSOAPin5.ZmesWsIn5
			Dim WS_Output As New SAPSOAPin5.ZmesWsIn5Response



			WS.Credentials = New System.Net.NetworkCredential("mes_user", "12345678")


			WS_Input.I_ID_EVENTO = SettingsArray(position, Nome)

			WS_Input.I_TIPO_ELABORAZIONE = SettingsArray(position, Tipo)

			WS_Output = WS.ZmesWsIn5(WS_Input)

			ScriviLog(LogFile, WS_Output.E_RETCODE.ToString)
			ScriviLog(LogFile, WS_Output.E_MESSAGE.ToString)


			If WS_Output.E_RETCODE <> 0 Then
				Return False
				Exit Function
			End If

			'MsgBox(WS_Output.EMessage)

			'If WS_Output.EMessage.Contains("OK") = False Then
			'Return False
			'Exit Function
			'End If

		Catch ex As Exception
			ScriviLog(LogFile, ex.Message.ToString)
			InviaMail(ex.Message.ToString, "")
			Return False
			Exit Function
		End Try

		Return True

	End Function
	Public Sub DeleteData(ByVal connectionString As String, query As String)

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Try
            con.ConnectionString = connectionString
            con.Open()
            cmd.Connection = con
            cmd.CommandText = query

            cmd.ExecuteNonQuery()

        Catch ex As Exception
            'MessageBox.Show("Error while deleting record on table..." & ex.Message, "Delete Records")
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")

        Finally

            con.Close()
        End Try
    End Sub

    Public Sub EXECQUERY(ByVal connectionString As String, query As String)

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Try
            con.ConnectionString = connectionString
            con.Open()
            cmd.Connection = con
            cmd.CommandText = query

            cmd.ExecuteNonQuery()

        Catch ex As Exception

            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")

        Finally

            con.Close()
        End Try
    End Sub

    Public Sub EXECSP(ByVal connectionString As String, NomeSP As String, Parametro As String)

        Try

            Using sql As New SqlClient.SqlConnection(connectionString)
                sql.Open()
                Using cmd As New SqlClient.SqlCommand(NomeSP, sql)

                    cmd.CommandType = CommandType.StoredProcedure
                    If Len(Parametro) > 0 Then
                        Dim PP As New SqlClient.SqlParameter
                        PP.ParameterName = "@come"
                        PP.Value = Parametro
                        cmd.Parameters.Add(PP)
                    End If
                    cmd.CommandTimeout = 900
                    Dim myReturnValue As String = cmd.ExecuteScalar
                End Using
            End Using

        Catch ex As Exception

            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")

        Finally


        End Try

    End Sub
    Protected Sub ExportCSV(x As Integer)

        Dim qcolonne As Integer = 0
        Dim ccolonne As Integer = 0
        Dim clavoro As String = ""
        Dim op As String = ""
        Dim id As Integer = 0

        Using con As New SqlConnection(StringConnection)
            Using cmd As New SqlCommand("SELECT * FROM " & SettingsArray(x, Tabella))
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)

                        'Build the CSV file data as a Comma separated string.
                        Dim csv As String = String.Empty
                        For Each column As DataColumn In dt.Columns
                            qcolonne = qcolonne + 1
                        Next

                        If dt.Rows.Count = 0 Then
                            ScriviLog(LogFile, "La tabella " & SettingsArray(x, Tabella) & " è vuota. Non esporto niente")
                            Exit Sub
                        End If

                        For Each row As DataRow In dt.Rows



                            ccolonne = 0


                            For Each column As DataColumn In dt.Columns

                                If x = 2 Then
                                    If column.ColumnName <> "C_LAVORO" Then
                                        If column.ColumnName <> "INCID" Then
                                            'Add the Data rows.
                                            ccolonne = ccolonne + 1
                                            If ccolonne <> (qcolonne - 2) Then

                                                If column.ColumnName = "Ordine di produzione" Then
                                                    op = row(column.ColumnName).ToString
                                                End If

                                                If column.ColumnName = "Operazione" Then

                                                    If clavoro = "assembl" Then



                                                        csv += SelectDati("SELECT TOP 1 [operation] From [CICLI_OP] Where N_ORDER_MFG ='" & op & "'") + ";"c

                                                    Else

                                                        csv += SelectDati("SELECT operation FROM (SELECT TOP 2 operation, Row_Number() OVER (ORDER BY operation) AS rownum FROM CICLI_OP where N_ORDER_MFG='" & op & "') AS tbl WHERE rownum = 2;") + ";"c

                                                    End If





                                                Else
                                                    csv += row(column.ColumnName).ToString().Replace(",", ".") + ";"c

                                                End If



                                            Else
                                                csv += row(column.ColumnName).ToString().Replace(",", ".")
                                            End If
                                        Else
                                            id = row(column.ColumnName).ToString
                                        End If
                                    Else
                                        clavoro = row(column.ColumnName).ToString

                                    End If

                                Else
                                    'Add the Data rows.
                                    ccolonne = ccolonne + 1
                                    If ccolonne <> qcolonne Then
                                        csv += row(column.ColumnName).ToString().Replace(",", ".") + ";"c
                                    Else
                                        csv += row(column.ColumnName).ToString().Replace(",", ".")
                                    End If
                                End If




                            Next
                            clavoro = ""
                            op = ""


                            ' AGGIORNO LA RIGA!!!!
                            id = 0

                            'Add new line.
                            csv += vbLf 'vbCr & vbLf
                        Next


                        'Download the CSV file.
                        Dim myFileName As String = ""

                        If x = 1 Then myFileName = String.Format(SettingsArray(x, Percorso) & "VERSA_{0}.CSV", Now.ToString("yyyyMMddhhmmss.fff"))
                        If x = 2 Then myFileName = String.Format(SettingsArray(x, Percorso) & "AVVISI_{0}.CSV", Now.ToString("yyyyMMddhhmmss.fff"))

                        File.WriteAllText(myFileName, csv)

                        ' chiamo SAP per importare i dati
                        If Not ChiamataASAPOUT(x) Then
                            ScriviLog(LogFile, "Chiamata a SAP abortita")
                        Else
                            ' segno in star che ho esportato la riga

                            For Each row As DataRow In dt.Rows

                                If x = 1 Then EXECQUERY(StringConnection, "UPDATE LOG SET I_STAMPA=13 where S_SERIALE='" & row(2).ToString & "'  and I_STAMPA is null and I_AZIONE=11")
                                If x = 2 Then EXECQUERY(StringConnection, "UPDATE RICHIESTE_MATERIALE_PER_SAP SET B_INVIATO=-1 where INCID=" & row(1).ToString)

                            Next
                        End If




                    End Using
                End Using
            End Using
        End Using
    End Sub

    Protected Sub ExportCSV_new(x As Integer)

        If x = 1 Then Exit Sub

        Dim qcolonne As Integer = 0
        Dim ccolonne As Integer = 0
        Dim clavoro As String = ""
        Dim op As String = ""
        Dim id As Integer = 0

        Using con As New SqlConnection(StringConnection)
            Using cmd As New SqlCommand("SELECT * FROM " & SettingsArray(x, Tabella))
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)

                        'Build the CSV file data as a Comma separated string.
                        Dim csv As String = String.Empty
                        For Each column As DataColumn In dt.Columns
                            qcolonne = qcolonne + 1
                        Next

                        If dt.Rows.Count = 0 Then
                            ScriviLog(LogFile, "La tabella " & SettingsArray(x, Tabella) & " è vuota. Non esporto niente")
                            Exit Sub
                        End If

                        For Each row As DataRow In dt.Rows



                            ccolonne = 0


                            For Each column As DataColumn In dt.Columns


                                'Add the Data rows.
                                ccolonne = ccolonne + 1
                                If ccolonne <> qcolonne Then
                                    csv += row(column.ColumnName).ToString().Replace(",", ".").Replace(vbCr, " ").Replace(vbLf, "") + ";"c
                                Else
                                    csv += row(column.ColumnName).ToString().Replace(",", ".").Replace(vbCr, " ").Replace(vbLf, "")
                                End If





                            Next
                            clavoro = ""
                            op = ""


                            ' AGGIORNO LA RIGA!!!!
                            id = 0

                            'Add new line.
                            csv += vbLf 'vbCr & vbLf
                        Next


                        'Download the CSV file.
                        Dim myFileName As String = ""

                        'If x = 1 Then myFileName = String.Format(SettingsArray(x, Percorso) & "VERSA_{0}.CSV", Now.ToString("yyyyMMddhhmmss.fff"))
                        If x = 2 Then myFileName = String.Format(SettingsArray(x, Percorso) & "AVVISI_{0}.CSV", Now.ToString("yyyyMMddhhmmss.fff"))

                        File.WriteAllText(myFileName, csv)

                        ' chiamo SAP per importare i dati
                        If Not ChiamataASAPOUT(x) Then
                            ScriviLog(LogFile, "Chiamata a SAP abortita")
                        Else
                            ' segno in star che ho esportato la riga

                            For Each row As DataRow In dt.Rows

                                'If x = 1 Then EXECQUERY(StringConnection, "UPDATE LOG SET I_STAMPA=13 where S_SERIALE='" & row(2).ToString & "'  and I_STAMPA is null and I_AZIONE=11")
                                'If x = 2 Then EXECQUERY(StringConnection, "UPDATE RICHIESTE_MATERIALE_PER_SAP SET B_INVIATO=-1 where INCID=" & row(1).ToString)
                                If x = 2 Then EXECQUERY(StringConnection, "UPDATE PROBLEMI SET B_INVIATO=1,DT_INVIO=GETDATE() where INCID=cast(substring('" & row(10).ToString & "',2,len('" & row(10).ToString & "')-1) as int)")

                            Next
                        End If




                    End Using
                End Using
            End Using
        End Using
    End Sub
    Private Function SelectDati(query As String) As String

        SelectDati = ""

        Dim sqlConnection1 As New SqlConnection(StringConnection)
        Dim cmd As New SqlCommand
        Dim reader As SqlDataReader

        cmd.CommandText = query
        cmd.CommandType = CommandType.Text
        cmd.Connection = sqlConnection1

        sqlConnection1.Open()

        reader = cmd.ExecuteReader()

        While reader.Read

            SelectDati = reader(0).ToString

        End While


        sqlConnection1.Close()

    End Function

    Private Sub InviaMail(Cosa As String, Chi As String)

        If Chi = "" Then Chi = "service.it@stulz.it"

        Dim MyMailMessage As New MailMessage()
        MyMailMessage.From = New MailAddress("sapreader@stulz.it", "Stulz SpA - SAP READER")
        MyMailMessage.To.Add(Chi)
        MyMailMessage.CC.Add("alessandro.soave@stulz.it")
        MyMailMessage.Subject = "elaborazione"
        MyMailMessage.Body = Cosa
        MyMailMessage.ReplyToList.Add("noreply@stulz.it")
        MyMailMessage.Priority = MailPriority.High

        MyMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess
        MyMailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8")


        ' contatto il server di posta
        Dim SMTPServer As New SmtpClient("172.21.1.25", 25)

        SMTPServer.UseDefaultCredentials = False

        Try
            SMTPServer.Send(MyMailMessage)
        Catch ex As SmtpException
            ScriviLog(LogFile, ex.Message.ToString)
        End Try
    End Sub
    Private Function ChiamataASAPOUT(position As Integer) As Boolean


        ScriviLog(LogFile, "Chiamata: " & SettingsArray(position, Nome) & " Tipo:" & SettingsArray(position, Tipo))

        Try

            'Dim WS As New SapSOAPOut.ZWS_MES_OUTBOUND1

            Dim WS As New SapSOAPOut.ZWS_MES_OUTBOUND_1

            Dim WS_Input As New SapSOAPOut.ZmesWsOut
            Dim WS_Output As New SapSOAPOut.ZmesWsOutResponse


            WS.Credentials = New System.Net.NetworkCredential("mes_user", "12345678")


            WS_Input.IIdEvento = SettingsArray(position, Nome)

            WS_Output = WS.ZmesWsOut(WS_Input)


            ScriviLog(LogFile, WS_Output.ERetcode.ToString)
            ScriviLog(LogFile, WS_Output.EMessage.ToString)


            If WS_Output.ERetcode <> 0 Then
                Return False
                Exit Function
            End If

            'MsgBox(WS_Output.EMessage)

            'If WS_Output.EMessage.Contains("OK") = False Then
            'Return False
            'Exit Function
            'End If

        Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
            Return False
            Exit Function
        End Try

        Return True

    End Function
    Private Sub CaricaDatiGOAF(fileN As String)

        Dim dt As New DataTable()
        Dim line As String = Nothing
        Dim i As Integer = 0
        Dim trovato As Boolean = False

        Try

            Using sr As StreamReader = File.OpenText(fileN)
                line = sr.ReadLine()
                Do While line IsNot Nothing
                    Dim data() As String = line.Split(";"c)
                    If data.Length > 0 Then
                        If i = 0 Then
                            For Each item In data
                                dt.Columns.Add(New DataColumn())
                            Next item
                            i += 1
                        End If


                        ' salto l'intestazione

                        If data(0).ToString <> "Fornitore" Then
                            Dim row As DataRow = dt.NewRow()

                            If InStr(data(13).ToString(), "-") > 0 Then GoTo prossimo

                            row.ItemArray = data

                            dt.Rows.Add(row)
                        End If

                    End If
prossimo:
                    line = sr.ReadLine()
                Loop
            End Using



            Using cn As New SqlConnection(StringConnection)
                cn.Open()
                Using copy As New SqlBulkCopy(cn)

                    For i = 0 To dt.Columns.Count - 1
                        copy.ColumnMappings.Add(i, i)
                    Next

                    'msgbox(dt.Rows.Count)

                    copy.DestinationTableName = "Solleciti_SAP"
                    copy.WriteToServer(dt)
                End Using
            End Using

        Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
        End Try



    End Sub
    Private Sub CaricaDatiBoard(fileN As String)

        Dim dt As New DataTable()
        Dim line As String = Nothing
        Dim i As Integer = 0
        Dim x As Integer = 0
        Dim trovato As Boolean = False
        Dim giorno As String = Nothing
        Dim ora As String = Nothing
        Dim verso As String = ""

        Try

            Using sr As StreamReader = File.OpenText(fileN)
                line = sr.ReadLine()
                Do While line IsNot Nothing
                    Dim data() As String = line.Split(vbTab)
                    If data.Length > 0 Then
                        If i = 1 Then
                            For Each item In data
                                x += 1
                                ' una colonna in meno
                                If x < data.Length Then dt.Columns.Add(New DataColumn())
                            Next item
                            i += 1
                        End If


                        ' salto l'intestazione

                        If i > 1 Then

                            ora = data(1).ToString
                            verso = data(2).ToString
                            giorno = Mid(data(3).ToString, 7, 2) & "/" & Mid(data(3).ToString, 5, 2) & "/" & Mid(data(3).ToString, 1, 4)

                            Dim row As DataRow = dt.NewRow()

                            row(0) = data(0)
                            row(1) = giorno
                            row(2) = ora
                            row(3) = verso

                            'row.ItemArray = data

                            dt.Rows.Add(row)
                        End If

                        i += 1
                    End If

                    line = sr.ReadLine()
                    x = 0
                Loop
            End Using



            Using cn As New SqlConnection(StringConnection)
                cn.Open()
                Using copy As New SqlBulkCopy(cn)

                    For i = 0 To dt.Columns.Count - 1
                        copy.ColumnMappings.Add(i, i)
                    Next

                    copy.DestinationTableName = "TIMBRATURE"
                    copy.WriteToServer(dt)
                End Using
            End Using

        Catch ex As Exception
            ScriviLog(LogFile, ex.Message.ToString)
            InviaMail(ex.Message.ToString, "")
        End Try



    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        If RadioButton1.Checked Then
            FaiAzioni("/M")
        End If
        If RadioButton2.Checked Then
            FaiAzioni("/G")
        End If
        If RadioButton3.Checked Then
            FaiAzioni("/A")
        End If
        If RadioButton4.Checked Then
            FaiAzioni("/D")
        End If
        If RadioButton5.Checked Then
            FaiAzioni("/V")
        End If
        If RadioButton6.Checked Then
            FaiAzioni("/B")
        End If
        If RadioButton7.Checked Then
            FaiAzioni("/T")
        End If
        If RadioButton8.Checked Then
            FaiAzioni("/P")
        End If
        If RadioButton9.Checked Then
            FaiAzioni("/R")
        End If

        Application.Exit()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick


        If Now.DayOfWeek = DayOfWeek.Sunday Then Exit Sub
        'If Now.DayOfWeek = DayOfWeek.Saturday Then Exit Sub


        For Each row As DataRow In Orari.Rows


            If Hour(Now) = row.Item("ORA") And Minute(Now) = row.Item("MINUTI") And Second(Now) = row.Item("Secondi") Then


                FaiAzioni(row.Item("COSA"))


            End If

        Next row



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DemonstrateReadWriteXMLDocumentWithString()
    End Sub
    Private Sub DemonstrateReadWriteXMLDocumentWithString()
        Dim table As DataTable = CreateTestTable("SAPREADER")

        ' Write the schema and data to XML in a file.
        Dim fileName As String = currPath & "\TIMER_SAP_READER.xml"
        table.WriteXml(fileName, XmlWriteMode.WriteSchema)


        Orari.ReadXml(fileName)


    End Sub

    Private Function CreateTestTable(ByVal tableName As String) _
      As DataTable

        ' Create a test DataTable with two columns and a few rows.
        Dim table As New DataTable(tableName)
        Dim column As New DataColumn("ORA", GetType(System.String))
        table.Columns.Add(column)

        column = New DataColumn("MINUTI", GetType(System.String))
        table.Columns.Add(column)

        column = New DataColumn("COSA", GetType(System.String))
        table.Columns.Add(column)




        ' Add ten rows.
        Dim row As DataRow
        For i As Integer = 0 To 20
            row = table.NewRow()
            row("ORA") = "10"
            row("minuti") = "00"
            row("cosa") = "/P"
            table.Rows.Add(row)
        Next i

        table.AcceptChanges()
        Return table
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        InviaMail("pippo", "serviceit@stulz.it")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
		settingsPath = currPath + "\goaf.ini"

		StringConnection = IniRead(settingsPath, "CONNECTION", "STRING")


		Using cn As New SqlConnection(StringConnection)
			cn.Open()

		End Using





	End Sub
End Class

