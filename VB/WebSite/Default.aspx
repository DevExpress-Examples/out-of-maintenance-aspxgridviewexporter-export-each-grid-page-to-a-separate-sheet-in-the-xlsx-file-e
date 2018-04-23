<%@ Page Language="vb" AutoEventWireup="true"  CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ register Assembly="DevExpress.Web.v11.1, Version=11.1.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>

<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1, Version=11.1.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v11.1.Export, Version=11.1.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView.Export" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.9.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">

	<dx:aspxroundpanel ID="ASPxRoundPanel1" runat="server" Width="253px" ShowHeader="False">
	  <PanelCollection>
		<dx:PanelContent runat="server">
		   <table cellpadding="3px">
			  <tr>
				<td>
					Select a PageSize for the ASPxGridView:
				</td>
			  </tr>
			  <tr>
				<td>
					 <dx:aspxcombobox ID="pageSizeCombobox" runat="server" ValueType="System.Int32">
						<items>
						  <dx:listedititem Text="5 rows per Page" Value="5" />
						  <dx:listedititem Text="10 rows per Page" Value="10" />
						  <dx:listedititem Text="15 rows per Page" Value="15" />
						  <dx:listedititem Text="20 rows per Page" Value="20" />
					   </items>
					   <clientsideevents SelectedIndexChanged = "function(s,e){grid.PerformCallback(s.GetValue().toString());}" />
					</dx:aspxcombobox>
				</td>
			 </tr>
			 <tr>
				<td>
				  Click the button to export:
				</td>
			 </tr>
			 <tr>
				<td>
				  <dx:aspxbutton ID="ASPxButton1" runat="server" AutoPostBack="False" 
					 onclick="ASPxButton1_Click" Text="Export to XLSX">
				  </dx:aspxbutton>
				</td>
			 </tr>
		 </table>
	 </dx:PanelContent>
   </PanelCollection>

</dx:aspxroundpanel>


	<dx:aspxgridview ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" 
		DataSourceID="AccessDataSource1" KeyFieldName="ProductID" 
		ClientInstanceName="grid" oncustomcallback="ASPxGridView1_CustomCallback">
		<columns>
			<dx:gridviewdatatextcolumn FieldName="ProductID" ReadOnly="True" 
				VisibleIndex="0">
				<editformsettings Visible="False" />
			</dx:gridviewdatatextcolumn>
			<dx:gridviewdatatextcolumn FieldName="ProductName" VisibleIndex="1">
			</dx:gridviewdatatextcolumn>
			<dx:gridviewdatatextcolumn FieldName="UnitPrice" VisibleIndex="2">
			</dx:gridviewdatatextcolumn>
		</columns>
	</dx:aspxgridview>
	<asp:accessdatasource ID="AccessDataSource1" runat="server" 
		DataFile="~/App_Data/nwind.mdb" 
		SelectCommand="SELECT [ProductID], [ProductName], [UnitPrice] FROM [Products]">
	</asp:accessdatasource>


	<dx:aspxgridview ID="exportGrid" runat="server" AutoGenerateColumns="true" ClientVisible="false">
	</dx:aspxgridview>
	<dx:aspxgridviewexporter ID="ASPxGridViewExporter1" runat="server" 
		GridViewID="exportGrid">
	</dx:aspxgridviewexporter>



	</form>
</body>
</html>