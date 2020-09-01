using System;

using System.IO;

using System.Windows.Forms;
using System.Data.SQLite;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {



        public string newPathsong; //путь где сохраняется наш файл
        public string test;  //берет из название файла song_id
       public string endPath; //конечный путь к переименнованному файлу


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] text = File.ReadAllLines(Application.StartupPath + @"\" + "text.txt");
                string[] textSplit = text[0].Split(' ');
                string[] textSplit1 = text[1].Split(' ');
                string[] textSplit2 = text[2].Split(' ');

                if (textSplit[0] == "db")
                {
                    textBox6.Text = textSplit[2];
                    textBox1.Text = textSplit1[2];
                    textBox5.Text = textSplit2[2];
                    
                }
                
                else
                {
                    textBox6.Text = textSplit[0];
                    textBox1.Text = textSplit1[0];
                    textBox5.Text = textSplit2[0];
                }
            }
            catch(Exception)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(Application.StartupPath + @"\" + "text.txt"))
                {
                    sw.WriteLine("db = " + textBox6.Text);
                    sw.WriteLine("temp = " + textBox1.Text);
                    sw.WriteLine("save = " + textBox5.Text);
                }
            }
            catch(Exception)
            {

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {


        try
        {
                
                    label1.Text = "";
                string pathdb = textBox6.Text; //путь к папке где располагается db
                string pathh = textBox1.Text; //папка от куда брать файл
                string newPath = textBox5.Text; //перенос в новую папку

                //  string test= "'2103790728'";// берет из названия файла song_id




                

               

                var dr = new System.IO.DirectoryInfo(pathh);
                foreach (System.IO.FileInfo fi in dr.GetFiles("*.mp3"))
                {





                    // if (!(Path.GetFileName(endPath) == textBox2.Text))


                   

                    test = Path.GetFileName(pathh+@"\"+fi.Name);
                    test = test.Substring(test.IndexOf('-') + 1); //
                    test = test.Substring(0, test.LastIndexOf('-')); //

                    string cs = @"URI=file:" + pathdb + @"\" + "Xiami.db"; //путь к db

                    var con = new SQLiteConnection(cs);
                    con.Open();

                    string stm = $"SELECT * FROM song_info WHERE song_id=={test}";

                    var cmd = new SQLiteCommand(stm, con);
                    SQLiteDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        //Console.OutputEncoding = System.Text.Encoding.GetEncoding("GB2312");
                        // Console.WriteLine($"{rdr.GetString(11)} ");
                        textBox2.Text = rdr.GetString(11);
                        textBox3.Text = rdr.GetString(3);
                        textBox4.Text = rdr.GetString(6);
                    }

                    endPath = newPath + textBox2.Text + ".mp3"; //путь к переименнованому файлу
                    newPathsong = newPath + fi.Name; //место где сохранили наш файл

                    if (!(File.Exists(endPath)))
                    {
                        fi.CopyTo(newPath + fi.Name, true);  //переносим файл в нужную нам папку
                        File.Move(newPathsong, newPath + textBox2.Text + ".mp3");  //присваиваем название нашему файлу
                        label1.Text = "Скопировано";
                    }
                    else
                    {
                        label1.Text = "Не удалось скопировать т.к. уже есть";
                    }
                }
                   
          
        }
        catch(Exception)
        {
                label1.Text = "Не удалось скопировать";
        }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                label1.Text = "";
                var audioFile = TagLib.File.Create(endPath);
                audioFile.Tag.Album = textBox3.Text;  //название альбома
                audioFile.Tag.Title = textBox2.Text;   //Название трека
                var ee = new String[] { textBox4.Text }; // Исполнитель
                audioFile.Tag.Artists = ee;
                audioFile.Save();
                label1.Text = "Теги заданы";
            }
            catch(Exception)
            {
                label1.Text = "Нету файла";
            }

        }

       
    }

    public class song_info
    {
       public double album_id { get; set; }
        public string album_logo { get; set; }
        public string album_logoS { get; set; }
        public string album_name { get; set; }
        public string artistLogo { get; set; }
        public double artist_id { get; set; }

        public string artist_name { get; set; }
        public double length { get; set; }

        public string qualitys { get; set; }
        public string singers { get; set; }

        public double song_id { get; set; }

        public string song_name { get; set; }

    }
}
