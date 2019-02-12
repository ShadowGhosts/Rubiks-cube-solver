// Rubik's cube solver
// Christian Martynov

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using System.Drawing.Imaging;
using AForge.Controls;
using AForge.Imaging.Filters;
using AForge;
using AForge.Imaging.ColorReduction;

namespace Rubiks_cube_solver_app
{
    public partial class MainForm : Form
    {

        //veriable for used port numbers
        private string[] ports = SerialPort.GetPortNames();
        
        // used for the camera camera and picture resolution
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice Camera1;
        private VideoCaptureDevice Camera2;
        private VideoCapabilities[] snapshotCapabilities;

        //Create port class
       // private Serial_Com Serial_Com_Ardu;

        public MainForm()
        {
            InitializeComponent();
        }

        // Main form is loaded
        private void MainForm_Load(object sender, EventArgs e)
        {
            FaceL5.BackColor = Color.Green;
            FaceU5.BackColor = Color.White;
            FaceF5.BackColor = Color.Red;
            FaceR5.BackColor = Color.Blue;
            FaceD5.BackColor = Color.Yellow;
            FaceB5.BackColor = Color.Orange;

            // enumerate video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count != 0)
            {
                // add all devices to combo
                foreach (FilterInfo device in videoDevices)
                {
                    Camera1Combo.Items.Add(device.Name);
                    Camera2Combo.Items.Add(device.Name);
                }
            }
            else
            {
                Camera1Combo.Items.Add("No DirectShow devices found");
                Camera2Combo.Items.Add("No DirectShow devices found");
            }

            Camera1Combo.SelectedIndex = 0;
            Camera2Combo.SelectedIndex = 0;

            EnableConnectionControlsCamera1(true);
            EnableConnectionControlsCamera2(true);
            EnableConnectionControlsPort(true);

            foreach (string port in ports)
            {
                PortSelect.Items.Add(port);
            }
        }

        // Closing the main form
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectCamera1();
            DisconnectCamera2();
            DisconnectArduinoPort();
        }

        // Enable/disable connection related controls for camera 1
        private void EnableConnectionControlsCamera1(bool enable)
        {
            Camera1Combo.Enabled = enable;
            Camera1ResolutionsCombo.Enabled = enable;
            Camera1ConnectButton.Enabled = enable;
            Camera1DisconnectButton.Enabled = !enable;
            Camera1TriggerButton.Enabled = (!enable) && (snapshotCapabilities.Length != 0);
        }

        // Enable/disable connection related controls for camera 2
        private void EnableConnectionControlsCamera2(bool enable)
        {
            Camera2Combo.Enabled = enable;
            Camera2ResolutionsCombo.Enabled = enable;
            Camera2ConnectButton.Enabled = enable;
            Camera2DisconnectButton.Enabled = !enable;
            Camera2TriggerButton.Enabled = (!enable) && (snapshotCapabilities.Length != 0);
        }

        // Enable/diasble connection related controls for Arduino port
        private void EnableConnectionControlsPort(bool enable)
        {
            PortSelect.Enabled = enable;
            ConnectPort.Enabled = enable;
            SolveButton.Enabled = !enable;
            Scramble.Enabled = !enable;
            DisconnectPort.Enabled = !enable;
        }

        // New video device is selected
        private void Camera1Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (videoDevices.Count != 0)
            {
                Camera1 = new VideoCaptureDevice(videoDevices[Camera1Combo.SelectedIndex].MonikerString);
                EnumeratedSupportedFrameSizes(Camera1, 1);
            }
        }

        private void Camera2Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Camera2 = new VideoCaptureDevice(videoDevices[Camera2Combo.SelectedIndex].MonikerString);
            EnumeratedSupportedFrameSizes(Camera2, 2);
        }

        //new port selected
        private void PortSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = PortSelect.GetItemText(PortSelect.SelectedItem);
            ArduinoSerial.PortName = selected;
        }

        // Collect supported video and snapshot sizes
        private void EnumeratedSupportedFrameSizes(VideoCaptureDevice videoDevice, int camera)
        {
            this.Cursor = Cursors.WaitCursor;
            if (camera == 1)
            {
                Camera1ResolutionsCombo.Items.Clear();

                try
                {
                    snapshotCapabilities = videoDevice.SnapshotCapabilities;

                    foreach (VideoCapabilities capabilty in snapshotCapabilities)
                    {
                        Camera1ResolutionsCombo.Items.Add(string.Format("{0} x {1}",
                            capabilty.FrameSize.Width, capabilty.FrameSize.Height));
                    }

                    if (snapshotCapabilities.Length == 0)
                    {
                        Camera1ResolutionsCombo.Items.Add("Not supported");
                    }

                    Camera1ResolutionsCombo.SelectedIndex = 0;
                }

                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            if (camera == 2)
            {
                Camera2ResolutionsCombo.Items.Clear();

                try
                {
                    snapshotCapabilities = videoDevice.SnapshotCapabilities;

                    foreach (VideoCapabilities capabilty in snapshotCapabilities)
                    {
                        Camera2ResolutionsCombo.Items.Add(string.Format("{0} x {1}",
                            capabilty.FrameSize.Width, capabilty.FrameSize.Height));
                    }

                    if (snapshotCapabilities.Length == 0)
                    {
                        Camera2ResolutionsCombo.Items.Add("Not supported");
                    }

                    Camera2ResolutionsCombo.SelectedIndex = 0;
                }

                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        // On "Connect" button clicked for camera 1
        private void Camera1ConnectButton_Click(object sender, EventArgs e)
        {
            if (Camera1 != null)
            {
                if ((snapshotCapabilities != null) && (snapshotCapabilities.Length != 0))
                {

                    Camera1.VideoResolution = snapshotCapabilities[Camera1ResolutionsCombo.SelectedIndex];
                    Camera1.ProvideSnapshots = true;
                    Camera1.SnapshotResolution = snapshotCapabilities[Camera1ResolutionsCombo.SelectedIndex];
                    Camera1.SnapshotFrame += new NewFrameEventHandler(Camera1_SnapshotFrame);
                }

                EnableConnectionControlsCamera1(false);

                videoSourceCamera1.VideoSource = Camera1;
                videoSourceCamera1.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.VideoSourceCamera1_NewFrame);
                videoSourceCamera1.Start();
            }

        }

        // On "Connect" button clicked for camera 2
        private void Camera2ConnectButton_Click(object sender, EventArgs e)
        {
            if (Camera2 != null)
            {
                if ((snapshotCapabilities != null) && (snapshotCapabilities.Length != 0))
                {
                    Camera2.VideoResolution = snapshotCapabilities[Camera2ResolutionsCombo.SelectedIndex];
                    Camera2.ProvideSnapshots = true;
                    Camera2.SnapshotResolution = snapshotCapabilities[Camera2ResolutionsCombo.SelectedIndex];
                    Camera2.SnapshotFrame += new NewFrameEventHandler(Camera2_SnapshotFrame);
                }

                EnableConnectionControlsCamera2(false);

                videoSourceCamera2.VideoSource = Camera2;
                videoSourceCamera2.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.VideoSourceCamera2_NewFrame);
                videoSourceCamera2.Start();
            }

        }

        // On "Connect" button clicked for serialport
        private void ConnectPort_Click(object sender, EventArgs e)
        {
            if(PortSelect.GetItemText(PortSelect.SelectedItem) == "")
            {
                //error
                MessageBox.Show("No Port Selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ArduinoSerial.Open();
                EnableConnectionControlsPort(false);
            }
        }

        // On "Disconnect" button clicked for Camera1
        private void Camera1DisconnectButton_Click(object sender, EventArgs e)
        {
            DisconnectCamera1();
        }

        // On "Disconnect" button clicked for Caamer2
        private void Camera2DisconnectButton_Click(object sender, EventArgs e)
        {
            DisconnectCamera2();
        }

        //on "disconect" button clicked for serialport
        private void DisconnectPort_Click(object sender, EventArgs e)
        {
            DisconnectArduinoPort();
        }

        // Disconnect from Camera1
        private void DisconnectCamera1()
        {
            if (videoSourceCamera1.VideoSource != null)
            {
                // stop video device
                videoSourceCamera1.SignalToStop();
                videoSourceCamera1.WaitForStop();
                videoSourceCamera1.VideoSource = null;

                if (Camera1.ProvideSnapshots)
                {
                    Camera1.SnapshotFrame -= new NewFrameEventHandler(Camera1_SnapshotFrame);
                }
                EnableConnectionControlsCamera1(true);
            }
        }

        // Disconnect from Camera2
        private void DisconnectCamera2()
        {
            if (videoSourceCamera2.VideoSource != null)
            {
                //stop Camera 2
                videoSourceCamera2.SignalToStop();
                videoSourceCamera2.WaitForStop();
                videoSourceCamera2.VideoSource = null;

                if (Camera2.ProvideSnapshots)
                {
                    Camera2.SnapshotFrame -= new NewFrameEventHandler(Camera2_SnapshotFrame);
                }
                EnableConnectionControlsCamera2(true);
            }
        }

        // Disconnect from serialport
        private void DisconnectArduinoPort()
        {
            if (ArduinoSerial.IsOpen)
            {
                ArduinoSerial.Close();
                EnableConnectionControlsPort(true);
            }
        }

        // Simulate snapshot trigger for Camera1
        private void Camera1TriggerButton_Click(object sender, EventArgs e)
        {
            if ((Camera1 != null) && (Camera1.ProvideSnapshots))
            {
                Camera1.SimulateTrigger();
            }
        }

        // Simulate snapshot trigger for Camera2
        private void Camera2TriggerButton_Click(object sender, EventArgs e)
        {
            if ((Camera2 != null) && (Camera2.ProvideSnapshots))
            {
                Camera2.SimulateTrigger();
            }
        }

        // New snapshot frame is available
        private void Camera1_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Frame.Size);

            ShowSnapshotCamera1((Bitmap)eventArgs.Frame.Clone());
        }

        private void Camera2_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Frame.Size);

            ShowSnapshotCamera2((Bitmap)eventArgs.Frame.Clone());
        }

        private void ShowSnapshotCamera1(Bitmap snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Bitmap>(ShowSnapshotCamera1), snapshot);
            }
            else
            {
                SetImageCamera1(snapshot);
            }
        }

        private void ShowSnapshotCamera2(Bitmap snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Bitmap>(ShowSnapshotCamera2), snapshot);
            }
            else
            {
                SetImageCamera2(snapshot);
            }
        }

        public void SetImageCamera1(Bitmap bitmap)
        {


            lock (this)
            {
                Bitmap test = new Bitmap(@"C:\Users\cmart\source\repos\Raspberry win app\Raspberry win app\rubiks-cube.jpg");
                Bitmap old = (Bitmap)pictureBoxCamera1.Image;
                pictureBoxCamera1.Image = test;

                //GetColorLayoutCamera1(test);


                if (old != null)
                {
                    old.Dispose();
                }
            }
        }

        public void SetImageCamera2(Bitmap bitmap)
        {


            lock (this)
            {
                Bitmap old = (Bitmap)pictureBoxCamera2.Image;
                pictureBoxCamera2.Image = bitmap;

                // create filter
                GammaCorrection filter = new GammaCorrection(0.29999);
                BrightnessCorrection BCfilter = new BrightnessCorrection(-40);
                // apply the filter
                //filter.ApplyInPlace(bitmap);
                BCfilter.ApplyInPlace(bitmap);

                GetColorLayoutCamera2(bitmap);


                if (old != null)
                {
                    old.Dispose();
                }
            }
        }



        private void GetColorLayoutCamera1(Bitmap test, char FilterUsed)
        {
            RGBColorFilter colorFilter = new RGBColorFilter();

            //display filtered color LEFT side
            FaceL1.BackColor = colorFilter.ColorFilter(test, "fL1");
            FaceL2.BackColor = colorFilter.ColorFilter(test, "fL2");
            FaceL3.BackColor = colorFilter.ColorFilter(test, "fL3");
            FaceL4.BackColor = colorFilter.ColorFilter(test, "fL4");
            //  FaceL5.BackColor = Color.Green;
            FaceL6.BackColor = colorFilter.ColorFilter(test, "fL6");
            FaceL7.BackColor = colorFilter.ColorFilter(test, "fL7");
            FaceL8.BackColor = colorFilter.ColorFilter(test, "fL8");
            FaceL9.BackColor = colorFilter.ColorFilter(test, "fL9");

            //display filtered color UP side
            FaceU1.BackColor = colorFilter.ColorFilter(test, "fU1");
            FaceU2.BackColor = colorFilter.ColorFilter(test, "fU2");
            FaceU3.BackColor = colorFilter.ColorFilter(test, "fU3");
            FaceU4.BackColor = colorFilter.ColorFilter(test, "fU4");
            //FaceU5.BackColor = Color.White;
            FaceU6.BackColor = colorFilter.ColorFilter(test, "fU6");
            FaceU7.BackColor = colorFilter.ColorFilter(test, "fU7");
            FaceU8.BackColor = colorFilter.ColorFilter(test, "fU8");
            FaceU9.BackColor = colorFilter.ColorFilter(test, "fU9");

            //display filtered color blue side
            FaceF1.BackColor = colorFilter.ColorFilter(test, "fF1");
            FaceF2.BackColor = colorFilter.ColorFilter(test, "fF2");
            FaceF3.BackColor = colorFilter.ColorFilter(test, "fF3");
            FaceF4.BackColor = colorFilter.ColorFilter(test, "fF4");
            //FaceF5.BackColor = Color.Red;
            FaceF6.BackColor = colorFilter.ColorFilter(test, "fF6");
            FaceF7.BackColor = colorFilter.ColorFilter(test, "fF7");
            FaceF8.BackColor = colorFilter.ColorFilter(test, "fF8");
            FaceF9.BackColor = colorFilter.ColorFilter(test, "fF9");

            
        }

        private void GetColorLayoutCamera2(Bitmap test)
        {
            RGBColorFilter colorFilter = new RGBColorFilter();

            //display filtered color RIGHT side
            FaceR1.BackColor = colorFilter.ColorFilter(test, "fR1");
            FaceR2.BackColor = colorFilter.ColorFilter(test, "fR2");
            FaceR3.BackColor = colorFilter.ColorFilter(test, "fR3");
            FaceR4.BackColor = colorFilter.ColorFilter(test, "fR4");
            //FaceR5.BackColor = Color.Blue;
            FaceR6.BackColor = colorFilter.ColorFilter(test, "fR6");
            FaceR7.BackColor = colorFilter.ColorFilter(test, "fR7");
            FaceR8.BackColor = colorFilter.ColorFilter(test, "fR8");
            FaceR9.BackColor = colorFilter.ColorFilter(test, "fR9");

            //display filtered color DOWN side
            FaceD1.BackColor = colorFilter.ColorFilter(test, "fD1");
            FaceD2.BackColor = colorFilter.ColorFilter(test, "fD2");
            FaceD3.BackColor = colorFilter.ColorFilter(test, "fD3");
            FaceD4.BackColor = colorFilter.ColorFilter(test, "fD4");
            //FaceD5.BackColor = Color.Yellow;
            FaceD6.BackColor = colorFilter.ColorFilter(test, "fD6");
            FaceD7.BackColor = colorFilter.ColorFilter(test, "fD7");
            FaceD8.BackColor = colorFilter.ColorFilter(test, "fD8");
            FaceD9.BackColor = colorFilter.ColorFilter(test, "fD9");

            //display filtered color BACK side
            FaceB1.BackColor = colorFilter.ColorFilter(test, "fB1");
            FaceB2.BackColor = colorFilter.ColorFilter(test, "fB2");
            FaceB3.BackColor = colorFilter.ColorFilter(test, "fB3");
            FaceB4.BackColor = colorFilter.ColorFilter(test, "fB4");
            //FaceB5.BackColor = Color.Orange;
            FaceB6.BackColor = colorFilter.ColorFilter(test, "fB6");
            FaceB7.BackColor = colorFilter.ColorFilter(test, "fB7");
            FaceB8.BackColor = colorFilter.ColorFilter(test, "fB8");
            FaceB9.BackColor = colorFilter.ColorFilter(test, "fB9");

        }


        private Rectangle[] GenerateCallibrBox(int width, int hight)
        {
            Rectangle[] CallibrBox;

            int[,] boxLocation = { { 255, 55 }, { 195, 78 }, { 130, 109 }, { 180, 133 }, { 243, 104 }, { 307, 74 }, { 363, 99 }, { 304, 132 }, { 232, 170 }, { 100, 153 }, { 111, 213 }, { 125, 278 }, { 167, 310 }, { 158, 250 }, { 147, 185 }, { 202, 219 }, { 213, 293 }, { 217, 354 }, { 268, 231 }, { 336, 189 }, { 401, 151 }, { 399, 223 }, { 341, 263 }, { 271, 304 }, { 275, 370 }, { 339, 323 }, { 397, 287 } };

            CallibrBox = new Rectangle[boxLocation.GetLength(0)];
            for (int i = 0; i < boxLocation.GetLength(0); i++)
            {
                CallibrBox[i] = new Rectangle(boxLocation[i, 0], boxLocation[i, 1], width, hight);
            }
            return CallibrBox;
        }


        private void VideoSourceCamera1_NewFrame(object sender, ref Bitmap image)
        {
            Pen Black = new Pen(Color.Black, 3);
            Graphics g1 = Graphics.FromImage(image);

            g1.DrawRectangles(Black, GenerateCallibrBox(30, 30));
            g1.DrawLine(Black, 93, 128, 253, 233);
            g1.DrawLine(Black, 443, 117, 253, 233);
            g1.DrawLine(Black, 271, 420, 253, 233);
            g1.DrawLine(Black, 132, 302, 93, 128);
            g1.DrawLine(Black, 132, 302, 271, 420);
            g1.DrawLine(Black, 271, 41, 93, 128);
            g1.DrawLine(Black, 271, 41, 443, 117);
            g1.DrawLine(Black, 440, 308, 443, 117);
            g1.DrawLine(Black, 440, 308, 271, 420);


        }

        private void VideoSourceCamera2_NewFrame(object sender, ref Bitmap image)
        {
            Pen Black = new Pen(Color.Black, 3);
            Graphics g1 = Graphics.FromImage(image);
            
            g1.DrawRectangles(Black, GenerateCallibrBox(30, 30));
            g1.DrawLine(Black, 93, 128, 253, 233);
            g1.DrawLine(Black, 443, 117, 253, 233);
            g1.DrawLine(Black, 271, 420, 253, 233);
            g1.DrawLine(Black, 132, 302, 93, 128);
            g1.DrawLine(Black, 132, 302, 271, 420);
            g1.DrawLine(Black, 271, 41, 93, 128);
            g1.DrawLine(Black, 271, 41, 443, 117);
            g1.DrawLine(Black, 440, 308, 443, 117);
            g1.DrawLine(Black, 440, 308, 271, 420);

            BrightnessCorrection BCfilter = new BrightnessCorrection(-40);
            GammaCorrection filter = new GammaCorrection(0.23999);
            // apply the filter
            //filter.ApplyInPlace(image);
            BCfilter.ApplyInPlace(image);
        }


        private void FaceU1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }

        }

        private void FaceU2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceU3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceU4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

       /* private void FaceU5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceU6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceU7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceU8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceU9_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceU9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        /*private void FaceL5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceL6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceL9_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceL9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        /*private void FaceF5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceF6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceF9_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceF9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        /*private void FaceD5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceD6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceD9_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceD9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        /*private void FaceR5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceR6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceR9_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceR9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB1_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB1.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB2_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB2.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB3_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB3.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB4_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB4.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

       /* private void FaceB5_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB5.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }*/

        private void FaceB6_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB6.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB7_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB7.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB8_Click(object sender, EventArgs e)
        {
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point(System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB8.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private void FaceB9_Click(object sender, EventArgs e)
        {
            
            ColorSelect colorSelectform = new ColorSelect
            {
                StartPosition = FormStartPosition.Manual,
                Location = new Point (System.Windows.Forms.Cursor.Position.X - 200, System.Windows.Forms.Cursor.Position.Y - 160)
            };
            if (colorSelectform.ShowDialog(this) == DialogResult.OK)
            {
                FaceB9.BackColor = colorSelectform.ColorSelected;
                colorSelectform.Close();
            }
        }

        private int GreenBox;
        private int BlueBox;
        private int RedBox;
        private int YellowBox;
        private int WhiteBox;
        private int OrangeBox;
        private int OtherBox;


        private void SolveButton_Click(object sender, EventArgs e)
        {
            GreenBox = 0;
            BlueBox = 0;
            RedBox = 0;
            YellowBox = 0;
            WhiteBox = 0;
            OrangeBox = 0;
            OtherBox = 0;

            CheckBoxColor();

            if( false) //GreenBox != 9 || BlueBox != 9 || RedBox != 9 || YellowBox != 9 || WhiteBox != 9 || OrangeBox != 9 || OtherBox > 0)
            {
                MessageBox.Show("Some Colors are wrong! \nCheck the colors.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string Solution;
                Connect httpClient = new Connect();

                httpClient.type = ServerMessage;

                Solution = httpClient.HttpGet(httpClient.Join());

                // regex which match tags

                System.Text.RegularExpressions.Regex Httptags = new System.Text.RegularExpressions.Regex("<[^>]*>");

                // replace all matches with empty strin

                Solution = Httptags.Replace(Solution, "");
                Solution = Solution.TrimStart('\r','\n');
                SolutionBox.Text = Solution;
                TurnSequence = Solution;
                TurnSequence = TurnSequence.TrimEnd('\r', '\n', '\r', '\n');
                ServerMessage = null;

                count_message_array = 0;
                scramble = false;
                Arduino_Com(count_message_array);

            }

        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>.
        //Serial Comunication
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>.
        private static readonly string MESSAGE_CONFIRM = "Z";
        private static readonly string MESSAGE_TIMER_STOP = "Y";
        private string TurnSequence ="";
        private static readonly string[] Turn_Com = { "U", "F", "L", "R", "D", "B", "U'", "F'", "L'", "R'", "D'", "B'", "U2", "F2", "L2", "R2", "D2", "B2" };
        private static readonly string[] ConvertedArray = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S" };
        private int Array_size = 0;
        private int count_message_array = 0;
        private string messageSend = "";
        private bool scramble;

        private void Arduino_Com(int count)
        {
            if (ArduinoSerial.IsOpen)
            {
                string[] ConvertedMessage = ArrayConvert();
                    
                    

                if (Array_size != 0 && count < Array_size && scramble == false)
                {
                    ArduinoSerial.Write(ConvertedMessage[count]);
                    messageSend = ConvertedMessage[count];
                }

                if(scramble == true && count < 30)
                {
                    ArduinoSerial.Write(ConvertedMessage[count]);
                    messageSend = ConvertedMessage[count];
                }
             
            }
            else
            {
                MessageBox.Show("Port not open!!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string[] ArrayConvert()
        {
            string[] TurnSequenceArray = TurnSequence.Split(' ');
            string[] ConvertArray = new string[TurnSequenceArray.Length];
            Array_size = TurnSequenceArray.Length;
            int count = 0;

            foreach( string word in TurnSequenceArray)
            {
                for(int i = 0; i <= 17; i++)
                {
                    if (word == Turn_Com[i])
                    {
                        ConvertArray[count] = ConvertedArray[i];
                    }
                   
                }
                count++;
            }

            return ConvertArray;
        }
        

        private void ArduinoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string message = ArduinoSerial.ReadExisting();

            //send confirmation message
            if(message == messageSend)
            {
                ArduinoSerial.Write(MESSAGE_CONFIRM);      //confirmation char
                //MessageBox.Show("Confirm message send", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //recive confirmation message
            else if(message == MESSAGE_CONFIRM)            
            {
                if(count_message_array <= Array_size)
                {
                    count_message_array++;
                    Arduino_Com(count_message_array);
                    //MessageBox.Show("Confirm message recived.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if(count_message_array == Array_size)
                {
                    //Sending turn sequance finished
                    count_message_array = 0;
                    Array_size = 0;
                    //MessageBox.Show("Message string send finished", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ArduinoSerial.Write(MESSAGE_TIMER_STOP);
                }
                if(count_message_array == 30 && scramble == true)
                {
                    //Sending turn sequance finished
                    count_message_array = 0;
                    Array_size = 0;
                    //MessageBox.Show("Message string send finished", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ArduinoSerial.Write(MESSAGE_TIMER_STOP);
                }
            }
            else if(message == MESSAGE_TIMER_STOP)
            {
                MessageBox.Show("Message string send finished and timer stoped", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("ERROR \nRecived unknowen message\nRetry sending message.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Arduino_Com(count_message_array);
            }

        }



        private int[] CubeCode = { 6,4,2,3,1,1,5,1,5,1,5,1,5,2,4,4,2,1,6,6,3,5,3,6,4,6,6,3,4,2,2,4,6,4,1,2,5,2,1,4,5,3,3,1,2,3,2,4,3,6,5,6,3,5};
        private int[] arrayposition = {0,9,18,27,36,45};
        private string ServerMessage = null;


        private void CheckBoxColor()
        {
            Color Green = Color.Green;
            Color Blue = Color.Blue;
            Color Red = Color.Red;
            Color Yellow = Color.Yellow;
            Color White = Color.White;
            Color Orange = Color.Orange;

            Color[] FaceB = { FaceB1.BackColor, FaceB2.BackColor, FaceB3.BackColor, FaceB4.BackColor, FaceB5.BackColor, FaceB6.BackColor, FaceB7.BackColor, FaceB8.BackColor, FaceB9.BackColor};
            Color[] FaceR = { FaceR1.BackColor, FaceR2.BackColor, FaceR3.BackColor, FaceR4.BackColor, FaceR5.BackColor, FaceR6.BackColor, FaceR7.BackColor, FaceR8.BackColor, FaceR9.BackColor };
            Color[] FaceD = { FaceD1.BackColor, FaceD2.BackColor, FaceD3.BackColor, FaceD4.BackColor, FaceD5.BackColor, FaceD6.BackColor, FaceD7.BackColor, FaceD8.BackColor, FaceD9.BackColor };
            Color[] FaceU = { FaceU1.BackColor, FaceU2.BackColor, FaceU3.BackColor, FaceU4.BackColor, FaceU5.BackColor, FaceU6.BackColor, FaceU7.BackColor, FaceU8.BackColor, FaceU9.BackColor };
            Color[] FaceL = { FaceL1.BackColor, FaceL2.BackColor, FaceL3.BackColor, FaceL4.BackColor, FaceL5.BackColor, FaceL6.BackColor, FaceL7.BackColor, FaceL8.BackColor, FaceL9.BackColor };
            Color[] FaceF = { FaceF1.BackColor, FaceF2.BackColor, FaceF3.BackColor, FaceF4.BackColor, FaceF5.BackColor, FaceF6.BackColor, FaceF7.BackColor, FaceF8.BackColor, FaceF9.BackColor };
            
            //white = 1 blue = 2 red = 3 yellow = 4 green = 5 orange = 6

            for(int i = 0; i <= 8; i++)
            {
                if (FaceB[i] == Green)
                {
                    CubeCode[(arrayposition[5] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceB[i] == Blue)
                {
                    CubeCode[(arrayposition[5] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceB[i] == Red)
                {
                    CubeCode[(arrayposition[5] + i)] = 3;
                    RedBox++;
                }
                else if (FaceB[i] == Yellow)
                {
                    CubeCode[(arrayposition[5] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceB[i] == White)
                {
                    CubeCode[(arrayposition[5] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceB[i] == Orange)
                {
                    CubeCode[(arrayposition[5] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;

                if (FaceR[i] == Green)
                {
                    CubeCode[(arrayposition[1] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceR[i] == Blue)
                {
                    CubeCode[(arrayposition[1] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceR[i] == Red)
                {
                    CubeCode[(arrayposition[1] + i)] = 3;
                    RedBox++;
                }
                else if (FaceR[i] == Yellow)
                {
                    CubeCode[(arrayposition[1] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceR[i] == White)
                {
                    CubeCode[(arrayposition[1] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceR[i] == Orange)
                {
                    CubeCode[(arrayposition[1] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;

                if (FaceD[i] == Green)
                {
                    CubeCode[(arrayposition[3] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceD[i] == Blue)
                {
                    CubeCode[(arrayposition[3] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceD[i] == Red)
                {
                    CubeCode[(arrayposition[3] + i)] = 3;
                    RedBox++;
                }
                else if (FaceD[i] == Yellow)
                {
                    CubeCode[(arrayposition[3] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceD[i] == White)
                {
                    CubeCode[(arrayposition[3] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceD[i] == Orange)
                {
                    CubeCode[(arrayposition[3] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;

                if (FaceU[i] == Green)
                {
                    CubeCode[(arrayposition[0] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceU[i] == Blue)
                {
                    CubeCode[(arrayposition[0] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceU[i] == Red)
                {
                    CubeCode[(arrayposition[0] + i)] = 3;
                    RedBox++;
                }
                else if (FaceU[i] == Yellow)
                {
                    CubeCode[(arrayposition[0] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceU[i] == White)
                {
                    CubeCode[(arrayposition[0] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceU[i] == Orange)
                {
                    CubeCode[(arrayposition[0] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;

                if (FaceL[i] == Green)
                {
                    CubeCode[(arrayposition[4] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceL[i] == Blue)
                {
                    CubeCode[(arrayposition[4] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceL[i] == Red)
                {
                    CubeCode[(arrayposition[4] + i)] = 3;
                    RedBox++;
                }
                else if (FaceL[i] == Yellow)
                {
                    CubeCode[(arrayposition[4] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceL[i] == White)
                {
                    CubeCode[(arrayposition[4] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceL[i] == Orange)
                {
                    CubeCode[(arrayposition[4] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;

                if (FaceF[i] == Green)
                {
                    CubeCode[(arrayposition[2] + i)] = 5;
                    GreenBox++;
                }
                else if (FaceF[i] == Blue)
                {
                    CubeCode[(arrayposition[2] + i)] = 2;
                    BlueBox++;
                }
                else if (FaceF[i] == Red)
                {
                    CubeCode[(arrayposition[2] + i)] = 3;
                    RedBox++;
                }
                else if (FaceF[i] == Yellow)
                {
                    CubeCode[(arrayposition[2] + i)] = 4;
                    YellowBox++;
                }
                else if (FaceF[i] == White)
                {
                    CubeCode[(arrayposition[2] + i)] = 1;
                    WhiteBox++;
                }
                else if (FaceF[i] == Orange)
                {
                    CubeCode[(arrayposition[2] + i)] = 6;
                    OrangeBox++;
                }
                else
                    OtherBox++;
            }

            for(int k = 0; k <= 53; k++)
            {
                switch(CubeCode[k])
                {
                    case 1:
                        ServerMessage = ServerMessage + 'u';
                        break;
                    case 2:
                        ServerMessage = ServerMessage + 'r';
                        break;
                    case 3:
                        ServerMessage = ServerMessage + 'f';
                        break;
                    case 4:
                        ServerMessage = ServerMessage + 'd';
                        break;
                    case 5:
                        ServerMessage = ServerMessage + 'l';
                        break;
                    case 6:
                        ServerMessage = ServerMessage + 'b';
                        break;

                }
            }
           
        }



        private void Scramble_Click(object sender, EventArgs e)
        {
            Random_Scrambler scrambler = new Random_Scrambler();

            string ScrambleCube = scrambler.Return_Combo();

            SolutionBox.Text = ScrambleCube;

            TurnSequence = ScrambleCube;

            count_message_array = 0;
            scramble = true;
            Arduino_Com(count_message_array);

        }

        
        private void SolutionBox_TextChanged(object sender, EventArgs e)
        {
            //label3.Text +=  "! ";
        }

       

    }
}

