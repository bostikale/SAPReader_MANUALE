﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Il codice è stato generato da uno strumento.
'     Versione runtime:4.0.30319.42000
'
'     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
'     il codice viene rigenerato.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "Funzionalità di salvataggio automatico My.Settings"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://it-sqr1e01.stulz.it:8001/sap/bc/srt/rfc/sap/zws_mes_outbound_1/300/zws_mes"& _ 
            "_outbound_1/zws_mes_outbound_1_b")>  _
        Public ReadOnly Property SAPReader_SapSOAPOut_ZWS_MES_OUTBOUND_1() As String
            Get
                Return CType(Me("SAPReader_SapSOAPOut_ZWS_MES_OUTBOUND_1"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://it-sqr1e01.stulz.it:8001/sap/bc/srt/rfc/sap/zws_mes_inbound_2/300/zws_mes_"& _ 
            "inbound_2/zws_mes_inbound_2_b")>  _
        Public ReadOnly Property SAPReader_SapSOAPin2_ZWS_MES_INBOUND_2() As String
            Get
                Return CType(Me("SAPReader_SapSOAPin2_ZWS_MES_INBOUND_2"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://it-sqr1e01.stulz.it:8001/sap/bc/srt/rfc/sap/zws_mes_inbound_4/300/zws_mes_"& _ 
            "inbound_4/zws_mes_inbound_4_b")>  _
        Public ReadOnly Property SAPReader_SapSOAPin4_ZWS_MES_INBOUND_4() As String
            Get
                Return CType(Me("SAPReader_SapSOAPin4_ZWS_MES_INBOUND_4"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://it-sqr1e01.stulz.it:8001/sap/bc/srt/rfc/sap/zws_mes_inbound_5/300/zws_mes_"& _ 
            "inbound_5/zws_mes_inbound_5_b")>  _
        Public ReadOnly Property SAPReader_SAPSOAPin5_ZWS_MES_INBOUND_5() As String
            Get
                Return CType(Me("SAPReader_SAPSOAPin5_ZWS_MES_INBOUND_5"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.SpecialSettingAttribute(Global.System.Configuration.SpecialSetting.WebServiceUrl),  _
         Global.System.Configuration.DefaultSettingValueAttribute("http://it-sqr1e01.stulz.it:8001/sap/bc/srt/rfc/sap/zws_mes_inbound_1/300/zws_mes_"& _ 
            "inbound_1/zws_mes_inbound_1_b")>  _
        Public ReadOnly Property SAPReader_SapSOAPin_ZWS_MES_INBOUND_1() As String
            Get
                Return CType(Me("SAPReader_SapSOAPin_ZWS_MES_INBOUND_1"),String)
            End Get
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.SAPReader.My.MySettings
            Get
                Return Global.SAPReader.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
