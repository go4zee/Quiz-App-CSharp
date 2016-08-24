﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using QuizApp.Core.Authentication;
using QuizApp.Core.Database;

namespace QuizApp
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeQuizList();
        }


        public void InitializeQuizList()
        {

            QuizDBContext db = new QuizDBContext();
            string QueryString = @"Select QuizName FROM QuizList";

            DataTable datatable = new DataTable();
            datatable = QueryExecute(QueryString, db.QuizConnectionString);

            if (datatable != null)
            {
                if (datatable.Rows.Count > 0)
                {
                    foreach (DataRow dr in datatable.Rows)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerText = dr["QuizName"].ToString();
                        li.Attributes["class"] = "list-group-item";

                        HtmlButton editButton = new HtmlButton();
                        editButton.InnerText = "Edit";
                        editButton.Attributes["class"] = "btn btn-primary";
                        editButton.Attributes["value"] = SecurityClass.EncryptString(dr["QuizName"].ToString(), "ButtonType");
                        editButton.Attributes["action"] = SecurityClass.EncryptString("Edit", "ActionPhrase");
                        editButton.Attributes["runat"] = "server";
                        editButton.ServerClick += EditQuiz;
                        editButton.Attributes["id"] = "editButton";
                        

                        HtmlButton DeleteButton = new HtmlButton();
                        DeleteButton.InnerText = "Delete";
                        DeleteButton.Attributes["class"] = "btn btn-danger";
                        DeleteButton.Attributes["value"] = SecurityClass.EncryptString(dr["QuizName"].ToString(), "ButtonTypePhrase");
                        DeleteButton.Attributes["action"] = SecurityClass.EncryptString("Delete", "ActionPhrase");
                        DeleteButton.Attributes["runat"] = "server";
                        DeleteButton.Attributes["id"] = "deleteButton";

                        QuizListDiv.Controls.Add(li);
                        li.Controls.Add(editButton);
                        li.Controls.Add(DeleteButton);
                    }
                }
            }
            



        }
        public DataTable QueryExecute(string QueryString, string Connection)
        {
            DataTable retVal = new DataTable();
            using (SqlConnection SqlConnection1 = new SqlConnection(Connection))
            {
                try
                {
                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(QueryString, SqlConnection1);
                    adapter.Fill(dataset, "QuizList");
                    retVal = dataset.Tables["QuizList"];
                }
                catch
                {
                    retVal = null;
                }
            }

            return retVal;
        }

        protected void EditQuiz(object sender, EventArgs e)
        {
            Response.Redirect("QuizSetup.aspx");
        }

        protected void createNewQuiz(object sender, EventArgs e)
        {

        }
    }
}