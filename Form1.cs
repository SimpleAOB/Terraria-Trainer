using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProcessMemoryReaderLib;
using System.Globalization;
using System.Diagnostics;

namespace Terraria_Trainer
{
    public partial class Form1 : Form
    {
        Process[] MyProcess;
        ProcessModule mainModule;
        ProcessMemoryReader Mem = new ProcessMemoryReader();
        PlayerInfo.PlayerData player = new PlayerInfo.PlayerData();

        #region Addresses
        int PlayerBase;
        int PlayerBaseHM = 0x03D29AA8;
        int PlayerbaseMA = 0x193B6C6C;
        int[] multiLevel = new int[] { 0x04 };
        #endregion

        bool GameFound = false;

        string gameProc;
        int gameProcId;
        int playerBase;

        string labelActive;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void conGame_Click(object sender, EventArgs e)
        {
            MyProcess = Process.GetProcesses();
            for (int i = 0; i < MyProcess.Length; i++)
            {
                if (MyProcess[i].ProcessName == "Terraria")
                {
                    gameProc = MyProcess[i].ProcessName + "-" + MyProcess[i].Id;
                    gameProcId = MyProcess[i].Id;
                    label2.Text = gameProc;
                    break;
                }
            }
            try
            {
                MyProcess[0] = Process.GetProcessById(gameProcId);
                mainModule = MyProcess[0].MainModule;
                Mem.ReadProcess = MyProcess[0];
                Mem.OpenProcess();
                GameFound = true;
                player.offsets = new PlayerInfo.playerDataAddr(0x308, 0x310, 0x314, 0x318, 0x80);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect: " + ex.Message + ex.StackTrace);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                PlayerBase = PlayerBaseHM;
                multiLevel = new int[] { 0x04 };
                labelActive = "health";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                PlayerBase = PlayerBaseHM;
                multiLevel = new int[] { 0x04 };
                labelActive = "mana";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                PlayerBase = PlayerbaseMA;
                multiLevel = new int[] { 0x00 };
                labelActive = "mslot1";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                PlayerBase = PlayerbaseMA + 0x4;
                multiLevel = new int[] { 0x00 };
                labelActive = "mslot2";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                PlayerBase = PlayerbaseMA + 0x8;
                multiLevel = new int[] { 0x00 };
                labelActive = "mslot3";
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                PlayerBase = PlayerbaseMA + 0xC;
                multiLevel = new int[] { 0x00 };
                labelActive = "mslot4";
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                PlayerBase = PlayerbaseMA + 0x1;
                multiLevel = new int[] { 0x00 };
                labelActive = "aslot1";
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                PlayerBase = PlayerbaseMA + 0x14;
                multiLevel = new int[] { 0x00 };
                labelActive = "aslot2";
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                PlayerBase = PlayerbaseMA + 0x18;
                multiLevel = new int[] { 0x00 };
                labelActive = "aslot3";
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                PlayerBase = PlayerbaseMA + 0x1C;
                multiLevel = new int[] { 0x00 };
                labelActive = "aslot4";
            }
            player.baseAddress = PlayerBase;
            player.multilevel = multiLevel;
            playerBase = Mem.ReadMultiLevelPointer(PlayerBase, 4, player.multilevel);
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (GameFound == false)
            {
                MessageBox.Show("Fuck you, click the button to connect to the game");
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("Health");
                comboBox1.Items.Add("Mana");
                comboBox1.Items.Add("Money Slot 1");
                comboBox1.Items.Add("Money Slot 2");
                comboBox1.Items.Add("Money Slot 3");
                comboBox1.Items.Add("Money Slot 4");
                comboBox1.Items.Add("Ammo Slot 1");
                comboBox1.Items.Add("Ammo Slot 2");
                comboBox1.Items.Add("Ammo Slot 3");
                comboBox1.Items.Add("Ammo Slot 4");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(GameFound)
            {
                int PlayerBase2 = PlayerBaseHM;
                int[] multiLevel2 = new int[] { 0x04 };
                int playerBase2 = Mem.ReadMultiLevelPointer(PlayerBase2, 4, multiLevel2);
                label1.Text = "HP: " + Mem.ReadInt(playerBase2 + player.offsets.Health);
                label3.Text = "Mana: " + Mem.ReadInt(playerBase2 + player.offsets.Mana);
                label4.Text = Convert.ToString(Mem.ReadInt(playerBase + player.offsets.amslot));
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GameFound)
            {
                if (textBox1.Text != null)
                {
                    int setTo = Convert.ToInt32(textBox1.Text);
                    switch (labelActive)
                    {
                        case "health":
                            Mem.WriteInt(playerBase + player.offsets.Health, setTo);
                            Mem.WriteInt(playerBase + player.offsets.MaxHealth, setTo);
                            break;
                        case "mana":
                            Mem.WriteInt(playerBase + player.offsets.Mana, setTo);
                            Mem.WriteInt(playerBase + player.offsets.MaxMana, setTo);
                            break;
                        case "mslot1":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "mslot2":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "mslot3":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "mslot4":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "aslot1":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "aslot2":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "aslot3":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                        case "aslot4":
                            Mem.WriteInt(playerBase + player.offsets.amslot, setTo);
                            break;
                    }
                }
            }
        }
    }
}
