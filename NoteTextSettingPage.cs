using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nature_prhysm_launcher
{
    internal class NoteTextSettingPage : TabPage
    {
        ComboBox noteImageComboBox = new ComboBox();
        DirectoryInfo[] noteImageList;

        PictureBox[] noteImage = new PictureBox[9];

        private void initNoteImageList()
        {
            DirectoryInfo notesDirectory = new DirectoryInfo(@"img/notes");
            noteImageList = notesDirectory.GetDirectories();
            foreach (DirectoryInfo d in noteImageList)
            {
                noteImageComboBox.Items.Add(d.Name);
            }

            noteImageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            noteImageComboBox.Location = new Point(70, 70);
            noteImageComboBox.Size = new Size(350, 30);
            noteImageComboBox.SelectedIndexChanged += new EventHandler(onNoteImageChanged);
            noteImageComboBox.SelectedIndex = 0;
            this.Controls.Add(noteImageComboBox);
        }

        private void onNoteImageChanged(object? sender, EventArgs e)
        {
            setNoteImage(noteImageList[noteImageComboBox.SelectedIndex].FullName);
        }

        private void setNoteImage(String path)
        {
            noteImage[0].ImageLocation = path + "/r.png";
            noteImage[1].ImageLocation = path + "/g.png";
            noteImage[2].ImageLocation = path + "/b.png";
            noteImage[3].ImageLocation = path + "/c.png";
            noteImage[4].ImageLocation = path + "/m.png";
            noteImage[5].ImageLocation = path + "/y.png";
            noteImage[6].ImageLocation = path + "/w.png";
            noteImage[7].ImageLocation = path + "/d.png";
            noteImage[8].ImageLocation = path + "/f.png";
        }

        Point[] imagePos = { 
            new Point(5, 100),
            new Point(5, 140),
            new Point(5, 180),
            new Point(250, 100),
            new Point(250, 140),
            new Point(250, 180),
            new Point(250, 220),
            new Point(5, 220),
            new Point(5, 260),
        };

        String[] colorName =
        {
            "赤色(R)",
            "緑色(G)",
            "青色(B)",
            "水色(C)",
            "紫色(M)",
            "黄色(Y)",
            "白色(W)",
            "黒色(K)",
            "虹色(F)",
        };

        public CheckBox[] colorNameCheckBox = new CheckBox[9];


        private void initNoteImage()
        {

            for (int i = 0; i < noteImage.Length; i++) 
            {
                noteImage[i] = new PictureBox();
                noteImage[i].Location = imagePos[i];
                noteImage[i].Size = new Size(50, 50);
                noteImage[i].SizeMode = PictureBoxSizeMode.Zoom;
                this.Controls.Add(noteImage[i]);


                colorNameCheckBox[i] = new CheckBox();
                colorNameCheckBox[i].Text = colorName[i];
                colorNameCheckBox[i].Location = new Point(imagePos[i].X + 50, imagePos[i].Y + 15);
                colorNameCheckBox[i].AutoSize = true;
                this.Controls.Add(colorNameCheckBox[i]);
            }
        }


        public NoteTextSettingPage()
        {
            initNoteImage();
            initNoteImageList();

            this.Name = "tab2";
            this.Text = "音符文字設定";

            Label note = new Label();
            note.Text = "区別しにくい色の音符に文字を表示できます。";
            note.Location = new Point(5, 10);
            note.AutoSize = true;
            this.Controls.Add(note);

            Label note2 = new Label();
            note2.Text = "チェックを付けた色の音符のみ文字が表示されます。";
            note2.Location = new Point(5, 40);
            note2.AutoSize = true;
            this.Controls.Add(note2);

            Label note3 = new Label();
            note3.Text = "音符種類";
            note3.Location = new Point(5, 70);
            note3.AutoSize = true;
            this.Controls.Add(note3);

        }


    }
}
