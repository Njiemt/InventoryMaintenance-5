using System.Linq;

namespace InventoryMaintenance
{
    public partial class frmInventoryMaint : Form
    {
        public frmInventoryMaint()
        {
            InitializeComponent();
        }

        private List<InventoryItem> items = null!;
        
        //Mohamed Njie
        private void frmInventoryMaint_Load(object sender, EventArgs e)
        {
            items = InventoryDB.GetItems();
            LoadComboBox();
            FillItemListBox();
        }

        private void LoadComboBox()
        {
            cboFilterBy.DataSource = new string[] {
                "All", "Under $10", "$10 to $50", "Over $50"
            };
        }

        private void FillItemListBox()
        {
            lstItems.Items.Clear();

            string filter = cboFilterBy.SelectedValue?.ToString() ?? "";
            IEnumerable<InventoryItem> filteredItems = null!;

            // add items to the filteredItems collection based on FilterBy value
            if (filter == "Under $10")
            {
                filteredItems = items
                    .Where(it => it.Price < 10m)
                    .OrderBy(it => it.Description);
            }
            else if (filter == "$10 to $50")
            {
                filteredItems = items
                    .Where(it => it.Price >= 10m && it.Price <= 50m)
                    .OrderBy(it => it.Description);
            }
            else if (filter == "Over $50")
            {
                filteredItems = items
                    .Where(it => it.Price > 50m)
                    .OrderBy(it => it.Description);
            }
            else
            {
                // "All" or any other value
                filteredItems = items.OrderBy(it => it.Description);
            }

            // change code to loop the filteredItems collection
            foreach (InventoryItem item in filteredItems)
            {
                lstItems.Items.Add(item.GetDisplayText());
            }
        }

        //Mohamed Njie
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewItem newItemForm = new();
            InventoryItem item = newItemForm.GetNewItem();
            if (item != null)
            {
                items.Add(item);
                InventoryDB.SaveItems(items);
                FillItemListBox();
            }
        }
        
        //Mohamed Njie
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int i = lstItems.SelectedIndex;

            if (i == -1)
            {
                MessageBox.Show("Please select an item to delete.", "No item selected");
            }
            else
            {
                // get the display text from the listbox so we can find the matching InventoryItem
                string displayText = lstItems.Items[i].ToString() ?? "";

                InventoryItem? item = items.FirstOrDefault(it => it.GetDisplayText() == displayText);

                if (item == null)
                {
                    MessageBox.Show("Selected item could not be found.", "Error");
                    return;
                }

                string message = $"Are you sure you want to delete {item.Description}?";
                DialogResult result =
                    MessageBox.Show(message, "Confirm Delete",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    items.Remove(item);
                    InventoryDB.SaveItems(items);
                    FillItemListBox();
                }
            }
        }

        //Mohamed Njie
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Mohamed Njie
        private void cboFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillItemListBox();
        }
    }
}