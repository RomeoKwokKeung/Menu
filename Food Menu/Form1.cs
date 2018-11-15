using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Food_Menu
{
    public partial class frmMenu : Form
    {
        private List<Product> ListOfFood = new List<Product>();
        private int FoodIndex = 0;
        private bool IsFoodTab = true;
        private List<Product> ListOfDrink = new List<Product>();
        private int DrinkIndex = 0;

        public frmMenu()
        {
            InitializeComponent();

            BuildProductList();

            SetEvent();

            DisplayFood();

            SetDgv();
        }

        private void BuildProductList()
        {
            Product product = new Product("Plzza", "Italian Pizza from Vietnam", 1500, Properties.Resources.Pizza, true);
            ListOfFood.Add(product);
            product = new Product("Sushi", "Japanese Sushi from Osaka", 200, Properties.Resources.Sushi, true);
            ListOfFood.Add(product);
            product = new Product("Ramen", "Japanese Ramen from Hongkong", 800, Properties.Resources.ramen, true);
            ListOfFood.Add(product);
            product = new Product("Coca Cola", "Famous softdrink from USA", 150, Properties.Resources.CocaCola, false);
            ListOfDrink.Add(product);
            product = new Product("Lemon Orange Juice", "Heathly drink from UK", 100, Properties.Resources.Juice, false);
            ListOfDrink.Add(product);
        }

        private void DisplayFood()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = ListOfFood[FoodIndex].TheImage;
            lblName.Text = ListOfFood[FoodIndex].Name;
            lblInfo.Text = ListOfFood[FoodIndex].Information;
            lblPrice.Text = ListOfFood[FoodIndex].Price.ToString("C");
        }

        private void DisplayDrink()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = ListOfDrink[DrinkIndex].TheImage;
            lblName.Text = ListOfDrink[DrinkIndex].Name;
            lblInfo.Text = ListOfDrink[DrinkIndex].Information;
            lblPrice.Text = ListOfDrink[DrinkIndex].Price.ToString("C");
        }

        private void SetEvent()
        {
            btnFood.Click += TanBtnTab_Click;
            btnDrink.Click += TanBtnTab_Click;
        }

        private void TanBtnTab_Click(object sender, EventArgs e)
        {
            Label btn = (Label)sender;
            if (btn.Name == (btnFood.Name) && !IsFoodTab)
            {
                btnFood.BackColor = Color.Aquamarine;
                btnFood.ForeColor = Color.Black;
                btnDrink.BackColor = Color.LightGreen;
                btnDrink.ForeColor = Color.White;
                IsFoodTab = true;
                DisplayFood();
            }
            else if (btn.Name == (btnDrink.Name) && IsFoodTab)
            {
                btnFood.BackColor = Color.LightGreen;
                btnFood.ForeColor = Color.White;
                btnDrink.BackColor = Color.Aquamarine;
                btnDrink.ForeColor = Color.Black;
                IsFoodTab = false;
                DisplayDrink();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (IsFoodTab)
            {
                FoodIndex++;
                //超過INDEX就返0
                if (FoodIndex >= ListOfFood.Count) FoodIndex = 0;
                DisplayFood();
            }

            else
            {
                DrinkIndex++;
                //超過INDEX就返0
                if (DrinkIndex >= ListOfDrink.Count) DrinkIndex = 0;
                DisplayDrink();
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (IsFoodTab)
            {
                FoodIndex--;
                if (FoodIndex <= -1) FoodIndex = ListOfFood.Count - 1;
                DisplayFood();
            }
            else
            {
                DrinkIndex--;
                if (DrinkIndex <= -1) DrinkIndex = ListOfDrink.Count - 1;
                DisplayDrink();
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            int rowIdx = 0;

            if (IsFoodTab)
            {
                // we are at food tab
                if (CheckExistOnDgvAndAddMore(ListOfFood[FoodIndex].Name) == false)
                {
                    dgv.Rows.Add();

                    rowIdx = dgv.Rows.Count - 1;
                    dgv.Rows[rowIdx].Height = 40;
                    dgv.Rows[rowIdx].Cells["ProductNm"].Value = ListOfFood[FoodIndex].Name;
                    dgv.Rows[rowIdx].Cells["Amount"].Value = 1;
                    dgv.Rows[rowIdx].Cells["Price"].Value = ListOfFood[FoodIndex].Price;
                    dgv.Rows[rowIdx].Cells[3].Value = "DEL";

                    dgv.ClearSelection();
                }
            }
            else
            {
                // we are at drink tab
                if (CheckExistOnDgvAndAddMore(ListOfDrink[DrinkIndex].Name) == false)
                {
                    dgv.Rows.Add();
                    rowIdx = dgv.Rows.Count - 1;
                    dgv.Rows[rowIdx].Height = 40;
                    dgv.Rows[rowIdx].Cells["ProductNm"].Value = ListOfDrink[DrinkIndex].Name;
                    dgv.Rows[rowIdx].Cells["Amount"].Value = 1;
                    dgv.Rows[rowIdx].Cells["Price"].Value = ListOfDrink[DrinkIndex].Price;
                    dgv.Rows[rowIdx].Cells[3].Value = "DEL";

                    dgv.ClearSelection();
                }
            }

            if (rowIdx % 2 == 0)
            {
                dgv.Rows[rowIdx].DefaultCellStyle.BackColor = Color.HotPink;
            }
            else
            {
                dgv.Rows[rowIdx].DefaultCellStyle.BackColor = Color.HotPink;
            }

            dgv.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.White;

            SumAllPrice();
        }

        private void SumAllPrice()
        {
            if (dgv.Rows.Count > 0)
            {
                int sum = 0;
                for (int index = 0; index < dgv.Rows.Count; index++)
                {
                    sum += Convert.ToInt32(dgv.Rows[index].Cells["Price"].Value);
                }

                lblTotalPrice.Text = sum.ToString();
            }
            else
            {
                lblTotalPrice.Text = "0";
            }
        }

        private bool CheckExistOnDgvAndAddMore(string productName)
        {
            bool ret = false;
            for (int index = 0; index < dgv.Rows.Count; index++)
            {
                if (dgv.Rows[index].Cells["ProductNm"].Value.ToString() == (productName))
                {
                    int amountOrder = int.Parse(dgv.Rows[index].Cells["Amount"].Value.ToString()) + 1;
                    dgv.Rows[index].Cells["Price"].Value = amountOrder * Convert.ToInt32(IsFoodTab ? ListOfFood[FoodIndex].Price
                        : ListOfDrink[DrinkIndex].Price);
                    dgv.Rows[index].Cells["Amount"].Value = amountOrder;
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // set e to find help the situation
            int colIdx = e.ColumnIndex;
            int rowIdx = e.RowIndex;

            if (colIdx == 3 && rowIdx >= 0)
            {
                int amount = int.Parse(dgv.Rows[rowIdx].Cells["Amount"].Value.ToString());
                int unitPrice = int.Parse(dgv.Rows[rowIdx].Cells["Price"].Value.ToString()) / amount;
                amount--;

                if (amount == 0)
                {
                    dgv.Rows.RemoveAt(rowIdx);
                }
                else
                {
                    dgv.Rows[rowIdx].Cells["Amount"].Value = amount;
                    dgv.Rows[rowIdx].Cells["Price"].Value = amount * unitPrice;
                }
            }
            SumAllPrice();
        }

        

        private void SetDgv()
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ReadOnly = true;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

            dgv.Columns.Add("ProductNm", "Name");
            dgv.Columns.Add("Amount", "No");
            dgv.Columns.Add("Price", "Amount");
            DataGridViewButtonColumn dbc = new DataGridViewButtonColumn();
            dgv.Columns.Add(dbc);
            dgv.Columns.Add("Remove", "Del");
            dgv.Columns["ProductNm"].Width = 70;
            dgv.Columns["ProductNm"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.Columns["ProductNm"].DefaultCellStyle.Font = new Font("Tahoma", 10.0f);
            dgv.Columns["ProductNm"].HeaderCell.Style.Font = new Font("Tahoma", 10.0f);
            dgv.Columns["Amount"].Width = 30;
            dgv.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Amount"].DefaultCellStyle.Font = new Font("Tahoma", 10.0f);
            dgv.Columns["Amount"].HeaderCell.Style.Font = new Font("Tahoma", 10.0f);
            dgv.Columns["Price"].Width = 70;
            dgv.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["Price"].DefaultCellStyle.Font = new Font("Tahoma", 10.0f);
            dgv.Columns["Price"].HeaderCell.Style.Font = new Font("Tahoma", 10.0f);
            dgv.Columns[3].Width = 25;
        }
    }


    public struct Product
    {
        public string Name;
        public string Information;
        public int Price;
        public Image TheImage;
        public bool IsFoodNotDrink;

        public Product(string pName, string pInformation, int pPrice, Image pImage, bool IsFood)
        {
            Name = pName;
            Information = pInformation;
            Price = pPrice;
            TheImage = pImage;
            IsFoodNotDrink = IsFood;
        }
    }


}
