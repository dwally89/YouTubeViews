using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace YouTube_Views_CSharp
{
    public partial class AddEditVideo : Form
    {
        private bool edit;        
        private ArrayList songs;
        private Song song;
        public AddEditVideo(ArrayList songs)
        {
            this.songs = songs;
            InitializeComponent();
            monthCalendar1.MaxDate = DateTime.Today;
            edit = false;
        }
        public AddEditVideo(ArrayList songs, Song song)
        {
            this.songs = songs;
            InitializeComponent();
            monthCalendar1.MaxDate = DateTime.Today;
            edit = true;
            this.song = song;
            txtName.Text = song.Name;            
            txtViews.Text = song.Views.ToString();
            txtHiddenNumbers.Text = song.HiddenNumbers.ToString();
            monthCalendar1.SetDate(song.DateAdded);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Creating new song");
            if (edit)
            {
                for(int i=0; i<songs.Count;i++)
                {
                    if (((Song)songs[i]).Name.Equals(song.Name))
                    {
                        songs.RemoveAt(i);
                        i = songs.Count;
                    }
                }
            }
            Song toAdd = new Song(txtName.Text, int.Parse(txtViews.Text), monthCalendar1.SelectionRange.Start, int.Parse(txtHiddenNumbers.Text));
            songs.Add(toAdd);
            Console.WriteLine("Writing song to file");            
            Program.WriteToFile(songs);
            Console.WriteLine("Finished");
            this.Close();            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}
