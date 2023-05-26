namespace FlexFieldTextBoxWFTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(iPv4AddressTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(iPv4AddressPortTextBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(iPv6AddressTextBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(macAddressTextBox1.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(phoneNumberTextBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(phoneNumberExtTextBox1.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ssnTextBox1.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(flexFieldControl1.Text);
        }
    }
}