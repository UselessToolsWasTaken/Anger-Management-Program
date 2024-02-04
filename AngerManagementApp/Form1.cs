using Microsoft.Toolkit.Uwp.Notifications;
using NAudio.Wave;
using System;
using System.Windows.Forms;

namespace AngerManagementApp
{
    public partial class AngerApp : Form
    {
        bool appActive;
        private WaveInEvent waveIn;
        private float threshold = 0.7f;


        public AngerApp()
        {
            InitializeComponent();
            InitializeAudio();

            appActive = false;
        }

        private void InitializeAudio()
        {
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(44100, 1);
            waveIn.DataAvailable += WaveInDataAvailable;
        }

        private void WaveInDataAvailable(object sender, WaveInEventArgs e)
        {
            float max = 0;
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                var sample32 = sample / 32768f;
                if (sample32 < 0) sample32 = -sample32;
                if (sample32 > max) max = sample32;
            }

            if(max > threshold)
            {
                new ToastContentBuilder()
                    .AddText("STFU")
                    .AddText("Calm the FUCK down")
                    .Show();
            }
            else
            {
                return;
            }
        }

        private void zenButton_Click(object sender, EventArgs e)
        {
            appActive = !appActive;

            if (appActive)
            {
                waveIn.StartRecording();
                label1.Text = "I'm listening to your voice!";
            }
            else
            {
                waveIn.StopRecording();
                label1.Text = "I'm no longer\nlistening to your voice!";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
