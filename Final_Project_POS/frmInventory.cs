using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Final_Project_POS
{
    public partial class frmInventory : Form
    {
        private Dictionary<string, (int quantity, decimal price, decimal total)> itemStats;

        public frmInventory()
        {
            InitializeComponent();
            itemStats = new Dictionary<string, (int quantity, decimal price, decimal total)>(); 
            InitializeFlowLayoutPanel();
            LoadItems();

            // Initialize DataGridView columns
            dataGridView1.Columns.Add("Column1", "ID");
            dataGridView1.Columns.Add("Column2", "Item");
            dataGridView1.Columns.Add("Column3", "Quantity");
            dataGridView1.Columns.Add("Column4", "Price");
            dataGridView1.Columns.Add("Column5", "Total");

            // Hide the ID column
            dataGridView1.Columns["Column1"].Visible = false;
        }

        private void InitializeFlowLayoutPanel()
        {
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            this.Controls.Add(flowLayoutPanel);
        }

        private void LoadItems()
        {
            // Sample data
            var items = new[]
            {
        new { Id = "1", ImagePath = "apple.png", Name = "Apple", Price = 30m },
        new { Id = "2", ImagePath = "banana.png", Name = "Banana", Price = 160m },
        new { Id = "3", ImagePath = "grapes.png", Name = "Grapes", Price = 80m },
        new { Id = "4", ImagePath = "mango.png", Name = "Mango", Price = 60m },
        new { Id = "5", ImagePath = "orange.png", Name = "Orange", Price = 30m },
        new { Id = "6", ImagePath = "pineapple.png", Name = "Pineapple", Price = 40m },
        new { Id = "7", ImagePath = "strawberry.png", Name = "Strawberry", Price = 200m },
        new { Id = "8", ImagePath = "sugarapple.png", Name = "Sugar Apple", Price = 60m },
        new { Id = "9", ImagePath = "watermelon.png", Name = "Watermelon", Price = 180m }
    };

            FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)this.Controls[0];
            if (flowLayoutPanel == null)
            {
                MessageBox.Show("FlowLayoutPanel is null.");
                return;
            }

            if (itemStats == null)
            {
                MessageBox.Show("itemStats is null.");
                return;
            }

            foreach (var item in items)
            {
                AddItemToPanel(flowLayoutPanel, item.Id, item.ImagePath, item.Name, item.Price);

                if (!itemStats.ContainsKey(item.Id))
                {
                    itemStats[item.Id] = (0, item.Price, 0);
                }
            }
        }

        private void AddItemToPanel(FlowLayoutPanel panel, string itemId, string imagePath, string itemName, decimal itemPrice)
        {
            // Load image from file
            Image image;
            try
            {
                image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it, show a default image, etc.)
                MessageBox.Show($"Error loading image '{imagePath}': {ex.Message}");
                return;
            }

            // Create a new Panel
            Panel itemPanel = new Panel
            {
                Width = 135,
                Height = 155,
                Margin = new Padding(10)
            };

            // Create a PictureBox
            PictureBox pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Width = 100,
                Height = 100,
                Top = 10,
                Left = 35
            };

            // Add click event handler for the PictureBox
            pictureBox.Click += (sender, e) => PictureBox_Click(itemId, itemName, itemPrice);

            // Create a Label for the name
            Label nameLabel = new Label
            {
                Text = itemName,
                Top = 115,
                Left = 35,
                Width = 100,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Orange
            };

            // Create a Label for the price
            Label priceLabel = new Label
            {
                Text = $"₱{itemPrice}",
                Top = 137,
                Left = 35,
                Width = 100,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black
            };

            // Add controls to the item panel
            itemPanel.Controls.Add(pictureBox);
            itemPanel.Controls.Add(nameLabel);
            itemPanel.Controls.Add(priceLabel);

            // Add item panel to the FlowLayoutPanel
            panel.Controls.Add(itemPanel);
        }

        private void PictureBox_Click(string itemId, string itemName, decimal itemPrice)
        {
            if (itemStats == null)
            {
                MessageBox.Show("itemStats is null.");
                return;
            }

            if (!itemStats.ContainsKey(itemId))
            {
                MessageBox.Show($"Item ID '{itemId}' not found in itemStats.");
                return;
            }

            var currentStats = itemStats[itemId];
            itemStats[itemId] = (currentStats.quantity + 1, itemPrice, currentStats.total + itemPrice);

            // Check if the item already exists in the DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Column1"].Value?.ToString() == itemId)
                {
                    // Update existing row
                    row.Cells["Column3"].Value = itemStats[itemId].quantity;
                    row.Cells["Column5"].Value = itemStats[itemId].total;
                    return;
                }
            }

            // Add new row to the DataGridView
            dataGridView1.Rows.Add(itemId, itemName, itemStats[itemId].quantity, $"₱{itemPrice}", itemStats[itemId].total.ToString("₱0.00"));
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
        }
    }
}
