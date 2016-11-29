using System;
using System.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

//Добавить рефреш!
//Доделать закачку 
//Логины и мульти акк
namespace Vk
{
    public partial class Form1 : Form
    {
        public ReadOnlyCollection<Audio> audio;
        VkApi api = new VkApi();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            listView1.Columns.Add("Title", 200);
            listView1.Columns.Add("Artist", 150);
            listView1.View = View.Details;

        }

        private void T_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                listView1.Items.Clear();
                User user;
                var agp = new AudioGetParams(true);
                audio = api.Audio.Get(api.UserId.Value, out user);
                foreach (var item in audio)
                {
                    ListViewItem lv = new ListViewItem(item.Title);

                    lv.SubItems.Add(item.Artist.ToString());
                    listView1.Items.Add(lv);

                }
            }
            catch (Exception)
            {
                
                
            }
           
        }
        private void loginBut_Click(object sender, EventArgs e)
        {
            try
            {
                ApiAuthParams a = new ApiAuthParams();
                a.Login = login.Text;
                a.Password = password.Text;
                a.Settings = Settings.All;
                a.ApplicationId = 5691421;
                api.Authorize(a);
                User user;
                var agp = new AudioGetParams(true);
                audio = api.Audio.Get(api.UserId.Value, out user);
                foreach (var item in audio)
                {
                    ListViewItem lv = new ListViewItem(item.Title);

                    lv.SubItems.Add(item.Artist.ToString());
                    listView1.Items.Add(lv);

                }
                login.Enabled = false;
                password.Enabled = false;
                loginBut.Enabled = false;
               
                



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in audio)
                {
                    if (item.Title == listView1.SelectedItems[0].SubItems[0].Text &&
                        item.Artist == listView1.SelectedItems[0].SubItems[1].Text)
                    {
                        label3.Text = listView1.SelectedItems[0].SubItems[1].Text + " - " +
                                      listView1.SelectedItems[0].SubItems[0].Text;
                        MediaPlayer.URL = item.Url.AbsoluteUri;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in audio)
                {
                    if (item.Title == listView1.SelectedItems[0].SubItems[0].Text &&
                        item.Artist == listView1.SelectedItems[0].SubItems[1].Text)
                    {
                        string fileName = listView1.SelectedItems[0].SubItems[1].Text + " - " +
                                      listView1.SelectedItems[0].SubItems[0].Text + ".mp3";
                        String myStringWebResource = null;
                        using (WebClient myWebClient = new WebClient())
                        {
                            
                            // Download the Web resource and save it into the current filesystem folder.
                            myWebClient.DownloadFile(item.Url, fileName);
                            MessageBox.Show("Download completed, file in exe directory \n\r" + Directory.GetCurrentDirectory());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Refresh();
        }

        private void MediaPlayer_EndOfStream(object sender, AxWMPLib._WMPOCXEvents_EndOfStreamEvent e)
        {
            try
            {
                int selectedIndex = listView1.SelectedIndices[0];
                selectedIndex++;
                // Prevents exception on the last element:      
                if (selectedIndex < listView1.Items.Count)
                {
                    listView1.Items[selectedIndex].Selected = true;
                    listView1.Items[selectedIndex].Focused = true;
                }
                try
                {
                    foreach (var item in audio)
                    {
                        if (item.Title == listView1.SelectedItems[0].SubItems[0].Text &&
                            item.Artist == listView1.SelectedItems[0].SubItems[1].Text)
                        {
                            label3.Text = listView1.SelectedItems[0].SubItems[1].Text + " - " +
                                          listView1.SelectedItems[0].SubItems[0].Text;
                            MediaPlayer.URL = item.Url.AbsoluteUri;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        
        }
    }
}
