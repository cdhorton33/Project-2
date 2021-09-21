using System;
using System.Windows.Forms;
/// Chris Horton , CJ, Sprint 1
/// Date: 14/9/21
/// Version 1
/// Astronomical processing
/// add, display, delete, edit, bubblesort
/// comments:
///     - currently the code is working but not for the intended purposes, the methods have been tested without exception handling and work
///     - the methods at the moment sort by the data ascending
///     - the methods need to include the second array, allowing the user to sort by the hours of the array
///     - the hours array needs to be edited along with the data array

///version 2
///add, edit, display, delete, sort
///comments:
///     - in this version the second array has been implemented in multiple methods, not all of them though
///     

///version 3
///all methods
///comments:
///     - in this version all methods have been implemented to include both arrays
///     - the binary search method only works on data within the range of 1-23 because the array is only 24 units long

/// version 4
/// comments:
///     - the binary search method has been fixed to now search for all numbers

namespace Assessment2
{
    public partial class AstroProcessingForm : Form
    {

		
        public AstroProcessingForm()
        {
            InitializeComponent();
        }
        static int MAX = 24;
        int[] hoursArray = new int[MAX];
        int[] dataArray = new int[MAX];
        int nextEmptyHour = 0;


        #region Sort_Search
		//Method sortData_Click
		//  Comment: the sortData method sorts the array by the size of the inputted data in ascending order
		//	Client Requirement:  There are buttons that can sort and search the dataArray
		//  Functional requirement:   The sort method must be coded using the Bubble Sort algorithm
        private void sortData_Click(object sender, EventArgs e)
        {
            bubbleSort();
            displayHours();
            inputTextBox.Clear();
            this.inputTextBox.Focus();
        }
		//Method sortHours_Click
		//  Comment: the sortHours_Click method sorts the array by the hour it was inputted in order from 1 to 24
		//	Client Requirement:  There are buttons that can sort and search the Array
		//  Functional requirement:   The sort method must be coded using the Bubble Sort algorithm
        private void sortHours_Click(object sender, EventArgs e)
        {
            hoursSort();
            displayHours();
            this.inputTextBox.Focus();
        }
		//Method search_Click
		//  Comment: the search method searches the array for the number in the input text box
		//	Client Requirement:  -	The client can use a text box input to search the array.
		//						 -	There are buttons that can sort and search the data.
		//  Functional requirement:   -	The search method must be coded using the Binary Search algorithm.
		// 							  - A single text box is provided for the search input.
        private void search_Click(object sender, EventArgs e)
        {
            binarySearch();
            displayHours();
            inputTextBox.Clear();
            this.inputTextBox.Focus();
        }
        #endregion

        #region Add_Edit_Delete
		//Method fillArray_Click
		//  Comment: the fill array method fills the list box with numbers from 1 to 99
		//	Client Requirement:  
		//  Functional requirement:  -	The array is filled with random integers to simulate the data stream (numbers between 10 and 99) 
        private void fillArray_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for (int i = 0; i < 24; i++)
            {
                dataArray[nextEmptyHour] = rnd.Next(1, 99);
                hoursArray[nextEmptyHour] = nextEmptyHour + 1;
                nextEmptyHour++;
            }
            displayHours();
            inputTextBox.Clear();
            this.inputTextBox.Focus();
        }
		//Method delete_Click
		//  Comment: the delete method deletes the method selected in the data list box
		//	Client Requirement:  -	There is an input field (text box) so data can be deleted, added and edited
		//  Functional requirement:   -	The program must be able to add, edit and delete data values.
        private void delete_Click(object sender, EventArgs e)
        {
			
            if (listBoxData.SelectedIndex != -1)
            {
                int currentHour = int.Parse(listBoxData.SelectedItem.ToString());
                int hourIndex = listBoxData.SelectedIndex;
                DialogResult DeleteHour = MessageBox.Show("are you sure you want to delete hour " + (hoursArray[hourIndex]), "confirmation",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (DeleteHour == DialogResult.Yes)
                {
                    dataArray[hourIndex] = dataArray[nextEmptyHour - 1];
                    hoursArray[hourIndex] = hoursArray[nextEmptyHour - 1];
                    statusLabel1.Text = "task deleted";
                    nextEmptyHour--;
                    displayHours();
                    inputTextBox.Clear();
                }
                else
                {
                    statusLabel1.Text = "task not deleted";
                }
            }
            else
            {
                statusLabel1.Text = "cannot delete hour, text box is empty";
            }
            this.inputTextBox.Focus();
        }
        //Method add_Click
		//  Comment: the add method adds the text from the input text box into the array and the list box
		//	Client Requirement:  -	There is an input field (text box) so data can be deleted, added and edited
		//  Functional requirement:   -•	The program must be able to add, edit and delete data values
		private void add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inputTextBox.Text))
            {
                try
                {
                    int.TryParse(inputTextBox.Text, out dataArray[nextEmptyHour]);
                    if (dataArray[nextEmptyHour] == 0)
                    {
                        statusLabel1.Text = " the text in the text box cannot be converted into an integer";
                        nextEmptyHour--;
                    }
                    else
                    {
                        hoursArray[nextEmptyHour] = nextEmptyHour + 1;
                    }
                    nextEmptyHour++;
                }
                catch (IndexOutOfRangeException)
                {
                    statusLabel1.Text = " you have filled the data for today, please close and open the program to reset the array";
                }

            }
            else
            {
                statusLabel1.Text = "the text box is empty, please try again";
            }
            displayHours();
            inputTextBox.Clear();
            this.inputTextBox.Focus();
        }
        //Method edit_Click
		//  Comment: the edit method changes the selected index in the data list box to the integer in the input text box
		//	Client Requirement:  -	There is an input field (text box) so data can be deleted, added and edited
		//  Functional requirement:   -•	The program must be able to add, edit and delete data values
		private void edit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inputTextBox.Text))
            {
                if (!int.TryParse(listBoxData.SelectedItem.ToString(), out int selectedIndex))
                {
                    statusLabel1.Text = " cannot edit as the text box is empty or not an integer";
                }
                int hourIndex = listBoxData.SelectedIndex;
                dataArray[hourIndex] = int.Parse(inputTextBox.Text);

            }
            else
            {
                statusLabel1.Text = " cannot edit, text box empty";
            }
            displayHours();
            inputTextBox.Clear();
            this.inputTextBox.Focus();
        }
        #endregion

        #region methods
         //Method displayHours
		//  Comment: this method displays all the data in both list boxes
		//	Client Requirement:  
		//  Functional requirement:   
        private void displayHours()
        {
            listBoxData.Items.Clear();
            listBoxHours.Items.Clear();
            for (int x = 0; x < nextEmptyHour; x++)
            {
                listBoxData.Items.Add(dataArray[x]);
                listBoxHours.Items.Add(hoursArray[x]);
            }
        }
        //Method bubbleSort
		//  Comment: the bubbleSort method sorts the array by the size of the inputted data in ascending order
		//	Client Requirement:  There are buttons that can sort and search the dataArray
		//  Functional requirement:   The sort method must be coded using the Bubble Sort algorithm 	
		private void bubbleSort()
        {
            for (int i = 0; i < nextEmptyHour; i++)
            {
                for (int j = i + 1; j < nextEmptyHour; j++)
                {
                    if (dataArray[j] < dataArray[i])
                    {
                        int temp = dataArray[i];
                        dataArray[i] = dataArray[j];
                        dataArray[j] = temp;
                        int temp2 = hoursArray[i];
                        hoursArray[i] = hoursArray[j];
                        hoursArray[j] = temp2;
                    }
                }
            }
        }
        ///Method sortHours
		//  Comment: the sortHours method sorts the array by the hour it was inputted in order from 1 to 24
		//	Client Requirement:  There are buttons that can sort and search the Array
		//  Functional requirement:   The sort method must be coded using the Bubble Sort algorithm
		private void hoursSort()
        {
            for (int i = 0; i < nextEmptyHour; i++)
            {
                for (int j = i + 1; j < nextEmptyHour; j++)
                {
                    if (hoursArray[j] < hoursArray[i])
                    {
                        int temp = dataArray[i];
                        dataArray[i] = dataArray[j];
                        dataArray[j] = temp;
                        int temp2 = hoursArray[i];
                        hoursArray[i] = hoursArray[j];
                        hoursArray[j] = temp2;
                    }
                }
            }
        }
        //Method binarySearch
		//  Comment: the binarySearch method searches the array for the number in the input text box
		//	Client Requirement:  -	The client can use a text box input to search the array.
		//						 -	There are buttons that can sort and search the data.
		//  Functional requirement:   -	The search method must be coded using the Binary Search algorithm.
		// 							  - A single text box is provided for the search input.
		private void binarySearch()
        {
            bubbleSort();
            int min, max;
            bool found = false;
            min = 0;
            max = 23;
            int target;
            int.TryParse(inputTextBox.Text, out target);
            if (target != -1)
            {
                while ((min <= max) && (!found))
                {
                    int mid = (min + max) / 2;
                    if (target == dataArray[mid])
                    {
                        MessageBox.Show("number " + target + " found at hour " + hoursArray[mid]);
                        found = true;
                        break;
                    }
                    else if (dataArray[mid] >= target)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
            }
            if (!found)
            {
                MessageBox.Show("not found");
            }
        }


        #endregion
    }
}
