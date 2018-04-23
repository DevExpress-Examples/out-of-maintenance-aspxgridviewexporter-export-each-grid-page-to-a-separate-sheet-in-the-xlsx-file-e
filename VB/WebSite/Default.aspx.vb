Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.XtraPrinting
Imports System.Collections
Imports DevExpress.Data.Filtering
Imports DevExpress.Web.ASPxGridView
Imports System.Data

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) AndAlso (Not IsCallback) Then
			pageSizeCombobox.Value = ASPxGridView1.SettingsPager.PageSize
		Else
			ASPxGridView1.SettingsPager.PageSize = Convert.ToInt32(pageSizeCombobox.Value)
		End If

	End Sub
	Protected Sub ASPxButton1_Click(ByVal sender As Object, ByVal e As EventArgs)
		Dim ps As New PrintingSystem()
		Dim clink As New Link(ps)
		AddHandler clink.CreateDetailArea, AddressOf clink_CreateDetailArea

				clink.CreateDocument()
				ps.PageSettings.Landscape = True
				Dim stream As New System.IO.MemoryStream()

				ps.ExportOptions.Xlsx.ExportMode = XlsxExportMode.SingleFilePageByPage
				ps.ExportOptions.Xlsx.SheetName = "Page"
				ps.ExportToXlsx(stream)

				WriteToResponse("export", True, "xlsx", stream)
				stream.Close()
	End Sub

	Protected Sub clink_CreateDetailArea(ByVal sender As Object, ByVal e As EventArgs)
		Dim self As Link = CType(sender, Link)
		For i As Integer = 0 To ASPxGridView1.VisibleRowCount - 1 Step ASPxGridView1.SettingsPager.PageSize

			Dim endIndex As Integer = i + ASPxGridView1.SettingsPager.PageSize
			Dim dt As DataTable = CreateRange(ASPxGridView1, i, endIndex)
			exportGrid.DataSource = dt
			exportGrid.DataBind()


			Dim linkdata As New DevExpress.Web.ASPxGridView.Export.Helper.GridViewLink(ASPxGridViewExporter1)
			linkdata.PrintingSystem = self.PrintingSystem
			self.PrintingSystem.InsertPageBreak(0)


			ASPxGridViewExporter1.DataBind()


			Dim skipArea As BrickModifier = linkdata.SkipArea
			linkdata.SkipArea = self.SkipArea
			linkdata.AddSubreport(System.Drawing.PointF.Empty)
			linkdata.SkipArea = skipArea


		Next i
	End Sub

		  Protected Sub WriteToResponse(ByVal fileName As String, ByVal saveAsFile As Boolean, ByVal fileFormat As String, ByVal stream As System.IO.MemoryStream)
			If Page Is Nothing OrElse Page.Response Is Nothing Then
				Return
			End If
			Dim disposition As String
			If saveAsFile Then
				disposition = "attachment"
			Else
				disposition = "inline"
			End If
			Page.Response.Clear()
			Page.Response.Buffer = False
			Page.Response.AppendHeader("Content-Type", String.Format("application/{0}", fileFormat))
			Page.Response.AppendHeader("Content-Transfer-Encoding", "binary")
			Page.Response.AppendHeader("Content-Disposition", String.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat))
			Page.Response.BinaryWrite(stream.ToArray())
			Page.Response.End()
		  End Sub

		  Private Function CreateRange(ByVal grid As ASPxGridView, ByVal startIndex As Integer, ByVal endIndex As Integer) As DataTable
			  grid.DataBind()

			  Dim dt As New DataTable()
			  Dim columnsCount As Integer = grid.Columns.Count
			  Dim fieldNames(columnsCount - 1) As String
			  For i As Integer = 0 To grid.Columns.Count - 1
				  If TypeOf grid.Columns(i) Is GridViewDataColumn Then
					  Dim fieldName As String = (TryCast(grid.Columns(i), GridViewDataColumn)).FieldName
					  dt.Columns.Add(fieldName)
					  fieldNames(i) = fieldName
				  End If
			  Next i


			  For i As Integer = startIndex To endIndex - 1


				  Dim val() As Object = TryCast(grid.GetRowValues(i, fieldNames), Object())
				  If val(0) IsNot Nothing Then
					  dt.Rows.Add(val.ToArray())
				  End If
			  Next i
			  Return dt
		  End Function

		  Protected Sub ASPxGridView1_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
			  CType(sender, ASPxGridView).SettingsPager.PageSize = Convert.ToInt32(e.Parameters)
			  CType(sender, ASPxGridView).DataBind()
		  End Sub
End Class

