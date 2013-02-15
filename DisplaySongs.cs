using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Microsoft.VisualBasic;

namespace YouTube_Views_CSharp
{
    public partial class DisplaySongs : Form
    {
        string[] columnNames = { "Name", "Views", "Views per day", "Date Added", "Status" };
        ArrayList songs;
        SortOrder dateAddedDirection = SortOrder.None;
        DataGridViewTextBoxColumn oldColumn;
        public DisplaySongs(ArrayList songs)
        {
            InitializeComponent();
            this.songs = songs;
            tableSetup(false);
            dataGridView1.Columns["Date Added"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dataGridView1.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                ListSortDirection direction;
                //If nothing has been sorted yet
                if (oldColumn == null)
                {
                    direction = ListSortDirection.Ascending;
                    oldColumn = (DataGridViewTextBoxColumn)dataGridView1.Columns[3];
                }
                else
                {
                    //If we have sorted this column ascending previously
                    if (oldColumn.Name.Equals("Date Added") && dateAddedDirection == SortOrder.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else //Either this column wasn't sorted before, or it was sorted descending
                    {
                        direction = ListSortDirection.Ascending;
                        //oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    }
                }
                // Sort the selected column.                
                if (direction == ListSortDirection.Ascending)
                {
                    songs.Sort(new Program.sortByDateAdded(false));
                    dateAddedDirection = SortOrder.Ascending;
                }
                else
                {
                    songs.Sort(new Program.sortByDateAdded(true));
                    dateAddedDirection = SortOrder.Descending;
                }
                tableSetup(true);
            }
            else
            {
                if (oldColumn.Name.Equals("Date Added"))
                {
                    dateAddedDirection = SortOrder.None;
                }
            }
        }


        private void tableSetup(bool sort)
        {
            DataTable d = new DataTable();
            foreach (string s in columnNames)
            {
                d.Columns.Add(s);
            }
            while (d.Rows.Count < songs.Count)
            {
                d.Rows.Add();
            }
            Song currentSong;
            for (int i = 0; i < d.Rows.Count; i++)
            {
                currentSong = (Song)songs[i];
                d.Rows[i][0] = currentSong.Name;
                d.Rows[i][1] = String.Format("{0,6:N0}", currentSong.GetTotalViews());
                d.Rows[i][2] = String.Format("{0,6:N0}", currentSong.ViewsPerDay);
                d.Rows[i][3] = currentSong.DateAdded.ToShortDateString();
                d.Rows[i][4] = currentSong.getStatus();
            }
            dataGridView1.DataSource = d;
            if (sort)
            {
                if (dateAddedDirection == SortOrder.Ascending || dateAddedDirection == SortOrder.None)
                {
                    dataGridView1.Columns[3].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                else
                {
                    dataGridView1.Columns[3].HeaderCell.SortGlyphDirection = SortOrder.Descending;                    
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddEditVideo(songs).ShowDialog();
            songs.Sort(new Program.sortByTotalViews());
            songs = Program.calculateAllViewsPerDay(songs);
            tableSetup(false);
        }

        private void updateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            songs.Sort(new Program.sortByViews());
            for (int i = 0; i < songs.Count; i++)
            {             
                ((Song)songs[i]).Views = int.Parse(Interaction.InputBox("Enter number of views for " + ((Song)songs[i]).Name, "Enter views", "" + ((Song)songs[i]).Views));
            }
            songs.Sort(new Program.sortByTotalViews());
            tableSetup(false);
            Program.WriteToFile(songs);
        }

        private void totalViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int total = 0;
            for (int i = 0; i < songs.Count; i++)
            {
                total += ((Song)songs[i]).Views;
            }            
            MessageBox.Show("Total number of views: " + String.Format("{0,6:N0}", total), "Total Views" , MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        private void averageViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int total = 0;
            for (int i = 0; i < songs.Count; i++)
            {
                total += ((Song)songs[i]).Views;
            }
            int average = total / songs.Count;
            MessageBox.Show("Average number of views: " + String.Format("{0,6:N0}", average), "Average Views", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells["Name"].FormattedValue.ToString();
            DialogResult result = MessageBox.Show("Are you sure that you want to delete " + name + "?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);                        
            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < songs.Count; i++)
                {
                    if (((Song)songs[i]).Name.Equals(name))
                    {
                        songs.RemoveAt(i);
                        Program.WriteToFile(songs);
                        tableSetup(false);
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String name = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index].Cells["Name"].FormattedValue.ToString();
            Song song = new Song();
            for (int i = 0; i < songs.Count; i++)
            {
                if (((Song)songs[i]).Name.Equals(name))
                {
                    song = (Song)songs[i];
                }
            }

            new AddEditVideo(songs, song).ShowDialog();
            songs.Sort(new Program.sortByTotalViews());
            songs = Program.calculateAllViewsPerDay(songs);
            tableSetup(false);
        }

    }
}
