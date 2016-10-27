using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inlamningsuppgift1_Dbt
{
    public partial class Form1 : Form
    {
        List<Contact> ContactList = new List<Contact>();
        public Form1()
        {
            InitializeComponent();
            dataView.ClearSelection();
            UpdateDataGrid();  
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            

        }
        public void UpdateDataGrid()
        {
            ContactContext dc = new ContactContext();
            ContactList = new List<Contact>();
            foreach (var item in dc.Contacts)
            {
                ContactList.Add(item);
            }
            dataView.DataSource = ContactList;
        }
        public void ClearText()
        {
            tbxName.Text = "";
            tbxStreet.Text = "";
            tbxCity.Text = "";
            tbxZip.Text = "";
            tbxPhone.Text = "";
            tbxEmail.Text="";
            dtpBirthday.Value = DateTime.Now;
            }
        private void btnAddContact_Click(object sender, EventArgs e)
        {
            
            
            try
            {
                using (var db = new ContactContext())
                {
                    var contact = (new Contact
                    {
                        Name = tbxName.Text,
                        StreetAddress = tbxStreet.Text,
                        City = tbxCity.Text,
                        Zip = tbxZip.Text,
                        Phone = tbxPhone.Text,
                        Email = tbxEmail.Text,
                        Birthday = dtpBirthday.Value.Date
                    });
                    db.Contacts.Add(contact);
                    
                    db.SaveChanges();
                    
                    
                    
                }
                UpdateDataGrid();
                ClearText();
                MessageBox.Show("Contact added to the database");

            }

            catch (MissingFieldException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdateContact_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataView.Rows[dataView.SelectedCells[0].RowIndex];
            int selectedID = int.Parse(selectedRow.Cells[0].Value.ToString());
            Contact cont;
            //1. Get student from DB
            using (var ctext = new ContactContext())
            {
                cont= ctext.Contacts.Where(s => s.ContactId == selectedID).FirstOrDefault<Contact>();
            }

            //2. change student name in disconnected mode (out of ctx scope)
            if (cont!= null)
            {
                cont.Name = tbxName.Text;
                cont.StreetAddress = tbxStreet.Text;
                cont.City = tbxCity.Text;
                cont.Zip = tbxZip.Text;
                cont.Phone = tbxPhone.Text;
                cont.Email = tbxEmail.Text;
                cont.Birthday = Convert.ToDateTime(dtpBirthday);
            }

            //save modified entity using new Context
            using (var dbCtx = new ContactContext())
            {
                //3. Mark entity as modified
                dbCtx.Entry(cont).State = System.Data.Entity.EntityState.Modified;

                //4. call SaveChanges
                dbCtx.SaveChanges();
            }
            UpdateDataGrid();

        }

        private void btnDeleteContact_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dataView.Rows[dataView.SelectedCells[0].RowIndex];
                int selectedID = int.Parse(selectedRow.Cells[0].Value.ToString());
                Contact contactDelete;
                //1. Get student from DB
                using (var ctx = new ContactContext())
                {
                    contactDelete = ctx.Contacts.Where(s => s.ContactId == selectedID).FirstOrDefault<Contact>();
                }

                //Create new context for disconnected scenario
                using (var newContext = new ContactContext())
                {
                    newContext.Entry(contactDelete).State = System.Data.Entity.EntityState.Deleted;
                    newContext.SaveChanges();
                }
                UpdateDataGrid();
            }

            catch (MissingFieldException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbxSearch.Text=="")
            {
 //               dataView.ClearSelection();
                dataView.DataSource = SearchList();
            }
           
        }
        private void dataView_SelectionChanged(object sender, EventArgs e)
        {
            //hur ändrar man ordningen så att textchanged går före dv selectchanged

            if (dataView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataView.Rows[dataView.SelectedCells[0].RowIndex];
                tbxName.Text = selectedRow.Cells[1].Value.ToString();
                tbxStreet.Text = selectedRow.Cells[2].Value.ToString();
                tbxCity.Text = selectedRow.Cells[3].Value.ToString();
                tbxZip.Text = selectedRow.Cells[4].Value.ToString();
                tbxPhone.Text = selectedRow.Cells[5].Value.ToString();
                tbxEmail.Text = selectedRow.Cells[6].Value.ToString();
                dtpBirthday.Value = Convert.ToDateTime(selectedRow.Cells[7].Value.ToString());
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            dataView.DataSource= SearchList();
        }
        public List<Contact> SearchList()
        {
           
            List<Contact> returnlist = null;
            if (tbxSearch.Text == "")
            {
                
                foreach (var item in ContactList)
                {
                    returnlist = ContactList;
                }
                   
               
            }
            else
            {
                List<Contact> contactListTemp = ContactList.Where(x => x.Name.Contains(tbxSearch.Text)).ToList();
                returnlist = contactListTemp;

            }
            
            return returnlist;
        }

        private void dataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataView.DataSource = SearchList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearText();
            dataView.ClearSelection();
        }
    }
}
