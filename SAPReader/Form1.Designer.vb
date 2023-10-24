<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Me.RadioButton1 = New System.Windows.Forms.RadioButton()
		Me.GroupBox1 = New System.Windows.Forms.GroupBox()
		Me.RadioButton9 = New System.Windows.Forms.RadioButton()
		Me.RadioButton8 = New System.Windows.Forms.RadioButton()
		Me.RadioButton7 = New System.Windows.Forms.RadioButton()
		Me.RadioButton6 = New System.Windows.Forms.RadioButton()
		Me.RadioButton5 = New System.Windows.Forms.RadioButton()
		Me.RadioButton4 = New System.Windows.Forms.RadioButton()
		Me.RadioButton3 = New System.Windows.Forms.RadioButton()
		Me.RadioButton2 = New System.Windows.Forms.RadioButton()
		Me.Button1 = New System.Windows.Forms.Button()
		Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
		Me.Button2 = New System.Windows.Forms.Button()
		Me.Button3 = New System.Windows.Forms.Button()
		Me.Button4 = New System.Windows.Forms.Button()
		Me.GroupBox1.SuspendLayout()
		Me.SuspendLayout()
		'
		'RadioButton1
		'
		Me.RadioButton1.AutoSize = True
		Me.RadioButton1.Location = New System.Drawing.Point(27, 33)
		Me.RadioButton1.Name = "RadioButton1"
		Me.RadioButton1.Size = New System.Drawing.Size(180, 18)
		Me.RadioButton1.TabIndex = 0
		Me.RadioButton1.TabStop = True
		Me.RadioButton1.Text = "Aggiornamento Massivo da SAP"
		Me.RadioButton1.UseVisualStyleBackColor = True
		'
		'GroupBox1
		'
		Me.GroupBox1.Controls.Add(Me.RadioButton9)
		Me.GroupBox1.Controls.Add(Me.RadioButton8)
		Me.GroupBox1.Controls.Add(Me.RadioButton7)
		Me.GroupBox1.Controls.Add(Me.RadioButton6)
		Me.GroupBox1.Controls.Add(Me.RadioButton5)
		Me.GroupBox1.Controls.Add(Me.RadioButton4)
		Me.GroupBox1.Controls.Add(Me.RadioButton3)
		Me.GroupBox1.Controls.Add(Me.RadioButton2)
		Me.GroupBox1.Controls.Add(Me.RadioButton1)
		Me.GroupBox1.Location = New System.Drawing.Point(13, 13)
		Me.GroupBox1.Name = "GroupBox1"
		Me.GroupBox1.Size = New System.Drawing.Size(256, 264)
		Me.GroupBox1.TabIndex = 1
		Me.GroupBox1.TabStop = False
		Me.GroupBox1.Text = "   Azioni..."
		'
		'RadioButton9
		'
		Me.RadioButton9.AutoSize = True
		Me.RadioButton9.Location = New System.Drawing.Point(27, 229)
		Me.RadioButton9.Name = "RadioButton9"
		Me.RadioButton9.Size = New System.Drawing.Size(108, 18)
		Me.RadioButton9.TabIndex = 8
		Me.RadioButton9.TabStop = True
		Me.RadioButton9.Text = "Aggiorna Articolo"
		Me.RadioButton9.UseVisualStyleBackColor = True
		'
		'RadioButton8
		'
		Me.RadioButton8.AutoSize = True
		Me.RadioButton8.Location = New System.Drawing.Point(27, 205)
		Me.RadioButton8.Name = "RadioButton8"
		Me.RadioButton8.Size = New System.Drawing.Size(105, 18)
		Me.RadioButton8.TabIndex = 7
		Me.RadioButton8.TabStop = True
		Me.RadioButton8.Text = "Aggiorna Priorità"
		Me.RadioButton8.UseVisualStyleBackColor = True
		'
		'RadioButton7
		'
		Me.RadioButton7.AutoSize = True
		Me.RadioButton7.Location = New System.Drawing.Point(27, 181)
		Me.RadioButton7.Name = "RadioButton7"
		Me.RadioButton7.Size = New System.Drawing.Size(162, 18)
		Me.RadioButton7.TabIndex = 6
		Me.RadioButton7.TabStop = True
		Me.RadioButton7.Text = "Invia Tempi consuntivi a SAP"
		Me.RadioButton7.UseVisualStyleBackColor = True
		'
		'RadioButton6
		'
		Me.RadioButton6.AutoSize = True
		Me.RadioButton6.Location = New System.Drawing.Point(27, 156)
		Me.RadioButton6.Name = "RadioButton6"
		Me.RadioButton6.Size = New System.Drawing.Size(158, 18)
		Me.RadioButton6.TabIndex = 5
		Me.RadioButton6.TabStop = True
		Me.RadioButton6.Text = "Importa timbrature da Board"
		Me.RadioButton6.UseVisualStyleBackColor = True
		'
		'RadioButton5
		'
		Me.RadioButton5.AutoSize = True
		Me.RadioButton5.Location = New System.Drawing.Point(27, 108)
		Me.RadioButton5.Name = "RadioButton5"
		Me.RadioButton5.Size = New System.Drawing.Size(154, 18)
		Me.RadioButton5.TabIndex = 4
		Me.RadioButton5.TabStop = True
		Me.RadioButton5.Text = "Invia Versamenti/NC a SAP"
		Me.RadioButton5.UseVisualStyleBackColor = True
		'
		'RadioButton4
		'
		Me.RadioButton4.AutoSize = True
		Me.RadioButton4.Location = New System.Drawing.Point(27, 83)
		Me.RadioButton4.Name = "RadioButton4"
		Me.RadioButton4.Size = New System.Drawing.Size(164, 18)
		Me.RadioButton4.TabIndex = 3
		Me.RadioButton4.TabStop = True
		Me.RadioButton4.Text = "Aggiornamento Delta da SAP"
		Me.RadioButton4.UseVisualStyleBackColor = True
		'
		'RadioButton3
		'
		Me.RadioButton3.AutoSize = True
		Me.RadioButton3.Location = New System.Drawing.Point(27, 58)
		Me.RadioButton3.Name = "RadioButton3"
		Me.RadioButton3.Size = New System.Drawing.Size(168, 18)
		Me.RadioButton3.TabIndex = 2
		Me.RadioButton3.TabStop = True
		Me.RadioButton3.Text = "Aggiornamento Aperti da SAP"
		Me.RadioButton3.UseVisualStyleBackColor = True
		'
		'RadioButton2
		'
		Me.RadioButton2.AutoSize = True
		Me.RadioButton2.Location = New System.Drawing.Point(27, 132)
		Me.RadioButton2.Name = "RadioButton2"
		Me.RadioButton2.Size = New System.Drawing.Size(102, 18)
		Me.RadioButton2.TabIndex = 1
		Me.RadioButton2.TabStop = True
		Me.RadioButton2.Text = "Aggiorna GOAF"
		Me.RadioButton2.UseVisualStyleBackColor = True
		'
		'Button1
		'
		Me.Button1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Button1.Location = New System.Drawing.Point(162, 283)
		Me.Button1.Name = "Button1"
		Me.Button1.Size = New System.Drawing.Size(110, 69)
		Me.Button1.TabIndex = 2
		Me.Button1.Text = "PROCEDI..."
		Me.Button1.UseVisualStyleBackColor = True
		'
		'Timer1
		'
		'
		'Button2
		'
		Me.Button2.Location = New System.Drawing.Point(13, 316)
		Me.Button2.Name = "Button2"
		Me.Button2.Size = New System.Drawing.Size(48, 35)
		Me.Button2.TabIndex = 3
		Me.Button2.Text = "Button2"
		Me.Button2.UseVisualStyleBackColor = True
		Me.Button2.Visible = False
		'
		'Button3
		'
		Me.Button3.Location = New System.Drawing.Point(67, 283)
		Me.Button3.Name = "Button3"
		Me.Button3.Size = New System.Drawing.Size(75, 56)
		Me.Button3.TabIndex = 4
		Me.Button3.Text = "Test Invio Mail"
		Me.Button3.UseVisualStyleBackColor = True
		'
		'Button4
		'
		Me.Button4.Location = New System.Drawing.Point(13, 275)
		Me.Button4.Name = "Button4"
		Me.Button4.Size = New System.Drawing.Size(48, 35)
		Me.Button4.TabIndex = 5
		Me.Button4.Text = "Button4"
		Me.Button4.UseVisualStyleBackColor = True
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(280, 363)
		Me.Controls.Add(Me.Button4)
		Me.Controls.Add(Me.Button3)
		Me.Controls.Add(Me.Button2)
		Me.Controls.Add(Me.Button1)
		Me.Controls.Add(Me.GroupBox1)
		Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "Form1"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "SAP READER/WRITER v.2.00 DR1"
		Me.GroupBox1.ResumeLayout(False)
		Me.GroupBox1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Button1 As Button
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton4 As RadioButton
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton5 As RadioButton
    Friend WithEvents RadioButton6 As RadioButton
    Friend WithEvents RadioButton7 As RadioButton
    Friend WithEvents RadioButton8 As RadioButton
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Button2 As Button
    Friend WithEvents RadioButton9 As RadioButton
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
End Class
