using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using System.Collections;
using DevExpress.Data.Filtering;
using DevExpress.Web.ASPxGridView;
using System.Data;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && !IsCallback)
        {
            pageSizeCombobox.Value = ASPxGridView1.SettingsPager.PageSize;
        }
        else
        {
            ASPxGridView1.SettingsPager.PageSize = Convert.ToInt32(pageSizeCombobox.Value);
        }
        
    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        PrintingSystem ps = new PrintingSystem();
        Link clink = new Link(ps);
        clink.CreateDetailArea += new CreateAreaEventHandler(clink_CreateDetailArea);
           
                clink.CreateDocument();
                ps.PageSettings.Landscape = true;
                System.IO.MemoryStream stream = new System.IO.MemoryStream();

                ps.ExportOptions.Xlsx.ExportMode = XlsxExportMode.SingleFilePageByPage;
                ps.ExportOptions.Xlsx.SheetName = "Page";
                ps.ExportToXlsx(stream);
               
                WriteToResponse("export", true, "xlsx", stream);
                stream.Close();
            }

    protected void clink_CreateDetailArea(object sender, EventArgs e)
    {
        Link self = (Link)sender;
        for (int i = 0; i < ASPxGridView1.VisibleRowCount; i += ASPxGridView1.SettingsPager.PageSize)
        {

            int endIndex = i + ASPxGridView1.SettingsPager.PageSize;
            DataTable dt = CreateRange(ASPxGridView1, i, endIndex);
            exportGrid.DataSource = dt;
            exportGrid.DataBind();


            DevExpress.Web.ASPxGridView.Export.Helper.GridViewLink linkdata = new DevExpress.Web.ASPxGridView.Export.Helper.GridViewLink(ASPxGridViewExporter1);
            linkdata.PrintingSystem = self.PrintingSystem;
            self.PrintingSystem.InsertPageBreak(0);


            ASPxGridViewExporter1.DataBind();


            BrickModifier skipArea = linkdata.SkipArea;
            linkdata.SkipArea = self.SkipArea;
            linkdata.AddSubreport(System.Drawing.PointF.Empty);
            linkdata.SkipArea = skipArea;


        }
    }

          protected void WriteToResponse(string fileName, bool saveAsFile, string fileFormat, System.IO.MemoryStream stream)
        {
            if (Page == null || Page.Response == null) return;
            string disposition = saveAsFile ? "attachment" : "inline";
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.AppendHeader("Content-Type", string.Format("application/{0}", fileFormat));
            Page.Response.AppendHeader("Content-Transfer-Encoding", "binary");
            Page.Response.AppendHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat));
            Page.Response.BinaryWrite(stream.ToArray());
            Page.Response.End();
        }

          private DataTable CreateRange(ASPxGridView grid, int startIndex, int endIndex)
          {
              grid.DataBind();

              DataTable dt = new DataTable();
              int columnsCount = grid.Columns.Count;
              string[] fieldNames = new string[columnsCount] ;
              for (int i = 0; i < grid.Columns.Count; i++)
              {
                  if (grid.Columns[i] is GridViewDataColumn)
                  {
                      string fieldName = (grid.Columns[i] as GridViewDataColumn).FieldName;
                      dt.Columns.Add(fieldName);
                      fieldNames[i] = fieldName;
                  }
              }
            
             
              for (int i = startIndex; i < endIndex; i++)
              {

                 
                  object[] val = grid.GetRowValues(i, fieldNames) as object[];
                  if (val[0] != null)
                  {
                      dt.Rows.Add(val.ToArray());
                  }
              }
              return dt;
          }

          protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
          {
              ((ASPxGridView)sender).SettingsPager.PageSize = Convert.ToInt32(e.Parameters);
              ((ASPxGridView)sender).DataBind();
          }
}

