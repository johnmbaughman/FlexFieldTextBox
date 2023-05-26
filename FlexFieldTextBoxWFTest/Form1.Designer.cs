using FlexFieldTextBoxWF.Controls;

namespace FlexFieldTextBoxWFTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flexFieldControl1 = new FlexFieldTextBoxWF.FlexFieldTextBox();
            iPv4AddressTextBox1 = new FlexFieldTextBoxWF.Controls.IPv4AddressTextBox();
            button1 = new Button();
            iPv4AddressPortTextBox1 = new FlexFieldTextBoxWF.Controls.IPv4AddressPortTextBox();
            button2 = new Button();
            iPv6AddressTextBox1 = new FlexFieldTextBoxWF.Controls.IPv6AddressTextBox();
            button3 = new Button();
            macAddressTextBox1 = new FlexFieldTextBoxWF.Controls.MACAddressTextBox();
            button4 = new Button();
            phoneNumberTextBox1 = new FlexFieldTextBoxWF.Controls.PhoneNumberTextBox();
            button5 = new Button();
            phoneNumberExtTextBox1 = new FlexFieldTextBoxWF.Controls.PhoneNumberExtTextBox();
            button6 = new Button();
            ssnTextBox1 = new FlexFieldTextBoxWF.Controls.SSNTextBox();
            button7 = new Button();
            button8 = new Button();
            SuspendLayout();
            // 
            // iPv4AddressTextBox1
            // 
            iPv4AddressTextBox1.AllowInternalTab = false;
            iPv4AddressTextBox1.AutoHeight = true;
            iPv4AddressTextBox1.BackColor = SystemColors.Window;
            iPv4AddressTextBox1.BorderStyle = BorderStyle.Fixed3D;
            iPv4AddressTextBox1.FieldCount = 4;
            iPv4AddressTextBox1.Location = new Point(12, 12);
            iPv4AddressTextBox1.Name = "iPv4AddressTextBox1";
            iPv4AddressTextBox1.ReadOnly = false;
            iPv4AddressTextBox1.Size = new Size(143, 23);
            iPv4AddressTextBox1.TabIndex = 0;
            iPv4AddressTextBox1.Text = "...";
            // 
            // button1
            // 
            button1.Location = new Point(161, 12);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "IPv4";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // iPv4AddressPortTextBox1
            // 
            iPv4AddressPortTextBox1.AllowInternalTab = false;
            iPv4AddressPortTextBox1.AutoHeight = true;
            iPv4AddressPortTextBox1.BackColor = SystemColors.Window;
            iPv4AddressPortTextBox1.BorderStyle = BorderStyle.Fixed3D;
            iPv4AddressPortTextBox1.FieldCount = 5;
            iPv4AddressPortTextBox1.Location = new Point(12, 41);
            iPv4AddressPortTextBox1.Name = "iPv4AddressPortTextBox1";
            iPv4AddressPortTextBox1.ReadOnly = false;
            iPv4AddressPortTextBox1.Size = new Size(143, 23);
            iPv4AddressPortTextBox1.TabIndex = 2;
            iPv4AddressPortTextBox1.Text = "... : ";
            // 
            // button2
            // 
            button2.Location = new Point(161, 41);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "IPv4+Port";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // iPv6AddressTextBox1
            // 
            iPv6AddressTextBox1.AllowInternalTab = false;
            iPv6AddressTextBox1.AutoHeight = true;
            iPv6AddressTextBox1.BackColor = SystemColors.Window;
            iPv6AddressTextBox1.BorderStyle = BorderStyle.Fixed3D;
            iPv6AddressTextBox1.FieldCount = 8;
            iPv6AddressTextBox1.Location = new Point(12, 70);
            iPv6AddressTextBox1.Name = "iPv6AddressTextBox1";
            iPv6AddressTextBox1.ReadOnly = false;
            iPv6AddressTextBox1.Size = new Size(251, 23);
            iPv6AddressTextBox1.TabIndex = 4;
            iPv6AddressTextBox1.Text = ":::::::";
            // 
            // button3
            // 
            button3.Location = new Point(269, 70);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 5;
            button3.Text = "IPv6";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // macAddressTextBox1
            // 
            macAddressTextBox1.AllowInternalTab = false;
            macAddressTextBox1.AutoHeight = true;
            macAddressTextBox1.BackColor = SystemColors.Window;
            macAddressTextBox1.BorderStyle = BorderStyle.Fixed3D;
            macAddressTextBox1.FieldCount = 6;
            macAddressTextBox1.Location = new Point(12, 99);
            macAddressTextBox1.Name = "macAddressTextBox1";
            macAddressTextBox1.ReadOnly = false;
            macAddressTextBox1.Size = new Size(117, 23);
            macAddressTextBox1.TabIndex = 6;
            macAddressTextBox1.Text = ":::::";
            // 
            // button4
            // 
            button4.Location = new Point(135, 99);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 7;
            button4.Text = "MAC";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // phoneNumberTextBox1
            // 
            phoneNumberTextBox1.AllowInternalTab = false;
            phoneNumberTextBox1.AutoHeight = true;
            phoneNumberTextBox1.BackColor = SystemColors.Window;
            phoneNumberTextBox1.BorderStyle = BorderStyle.Fixed3D;
            phoneNumberTextBox1.FieldCount = 3;
            phoneNumberTextBox1.Location = new Point(12, 128);
            phoneNumberTextBox1.Name = "phoneNumberTextBox1";
            phoneNumberTextBox1.ReadOnly = false;
            phoneNumberTextBox1.Size = new Size(82, 23);
            phoneNumberTextBox1.TabIndex = 8;
            phoneNumberTextBox1.Text = "() -";
            // 
            // button5
            // 
            button5.Location = new Point(100, 128);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 9;
            button5.Text = "Phone";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // phoneNumberExtTextBox1
            // 
            phoneNumberExtTextBox1.AllowInternalTab = false;
            phoneNumberExtTextBox1.AutoHeight = true;
            phoneNumberExtTextBox1.BackColor = SystemColors.Window;
            phoneNumberExtTextBox1.BorderStyle = BorderStyle.Fixed3D;
            phoneNumberExtTextBox1.FieldCount = 4;
            phoneNumberExtTextBox1.Location = new Point(12, 157);
            phoneNumberExtTextBox1.Name = "phoneNumberExtTextBox1";
            phoneNumberExtTextBox1.ReadOnly = false;
            phoneNumberExtTextBox1.Size = new Size(131, 23);
            phoneNumberExtTextBox1.TabIndex = 10;
            phoneNumberExtTextBox1.Text = "() -  ext:";
            // 
            // button6
            // 
            button6.Location = new Point(149, 157);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 11;
            button6.Text = "Phone+Ext";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // ssnTextBox1
            // 
            ssnTextBox1.AllowInternalTab = false;
            ssnTextBox1.AutoHeight = true;
            ssnTextBox1.BackColor = SystemColors.Window;
            ssnTextBox1.BorderStyle = BorderStyle.Fixed3D;
            ssnTextBox1.FieldCount = 3;
            ssnTextBox1.Location = new Point(12, 186);
            ssnTextBox1.Name = "ssnTextBox1";
            ssnTextBox1.ReadOnly = false;
            ssnTextBox1.Size = new Size(70, 23);
            ssnTextBox1.TabIndex = 12;
            ssnTextBox1.Text = "--";
            // 
            // button7
            // 
            button7.Location = new Point(88, 186);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 13;
            button7.Text = "SSN";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // flexFieldControl1
            // 
            flexFieldControl1.AllowInternalTab = false;
            flexFieldControl1.AutoHeight = true;
            flexFieldControl1.BackColor = SystemColors.Window;
            flexFieldControl1.BorderStyle = BorderStyle.Fixed3D;
            flexFieldControl1.FieldCount = 3;
            flexFieldControl1.Location = new Point(12, 215);
            flexFieldControl1.Name = "flexFieldControl1";
            flexFieldControl1.ReadOnly = false;
            flexFieldControl1.Size = new Size(160, 23);
            flexFieldControl1.TabIndex = 14;
            flexFieldControl1.Text = "<><><>";
            // 
            // button8
            // 
            button8.Location = new Point(178, 215);
            button8.Name = "button8";
            button8.Size = new Size(75, 23);
            button8.TabIndex = 15;
            button8.Text = "Default";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(361, 253);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(ssnTextBox1);
            Controls.Add(button6);
            Controls.Add(phoneNumberExtTextBox1);
            Controls.Add(button5);
            Controls.Add(phoneNumberTextBox1);
            Controls.Add(button4);
            Controls.Add(macAddressTextBox1);
            Controls.Add(button3);
            Controls.Add(iPv6AddressTextBox1);
            Controls.Add(button2);
            Controls.Add(iPv4AddressPortTextBox1);
            Controls.Add(button1);
            Controls.Add(iPv4AddressTextBox1);
            Controls.Add(flexFieldControl1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private FlexFieldTextBoxWF.FlexFieldTextBox flexFieldControl1;
        private IPv4AddressTextBox iPv4AddressTextBox1;
        private Button button1;
        private IPv4AddressPortTextBox iPv4AddressPortTextBox1;
        private Button button2;
        private IPv6AddressTextBox iPv6AddressTextBox1;
        private Button button3;
        private MACAddressTextBox macAddressTextBox1;
        private Button button4;
        private PhoneNumberTextBox phoneNumberTextBox1;
        private Button button5;
        private PhoneNumberExtTextBox phoneNumberExtTextBox1;
        private Button button6;
        private SSNTextBox ssnTextBox1;
        private Button button7;
        private Button button8;
    }
}