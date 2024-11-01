﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using MySql.Data.MySqlClient;

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadClinicalInstructors();
            LoadYearLevels();
            LoadDepartments();
            LoadGroups();
            LoadTimeShifts();
        }

        private void LoadClinicalInstructors()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT InstructorName FROM ClinicalInstructors";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string instructorName = reader["InstructorName"]?.ToString() ?? string.Empty;
                                lstClinicalInstructors.Items.Add(instructorName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading clinical instructors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadYearLevels()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT YearLevel FROM YearLevels";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string yearLevel = reader["YearLevel"]?.ToString() ?? string.Empty;
                                lstYearLevels.Items.Add(yearLevel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading year levels: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadDepartments()
        {

            string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;Uid=root;Pwd=;";
            string query = "SELECT DepartmentName FROM hospitaldepartments"; // Correctly targeting the DepartmentName column

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            listBox1.Items.Clear(); // Clear the existing items in the listbox to avoid duplicates

                            while (reader.Read())
                            {
                                // Fetch the DepartmentName value from the reader
                                string departmentName = reader["DepartmentName"]?.ToString() ?? string.Empty;

                                if (!string.IsNullOrEmpty(departmentName)) // Only add non-empty department names
                                {
                                    listBox1.Items.Add(departmentName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading departments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void LoadGroups()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT GroupNumber FROM Groups";  // Assuming 'Groups' is the table name

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Assuming that groupbox2, groupbox3, and groupbox4 are textboxes for displaying the number of groups
                            groupbox2.Clear();
                            groupbox3.Clear();
                            groupbox4.Clear();

                            int groupCount = 0;
                            while (reader.Read())
                            {
                                string groupNumber = reader["GroupNumber"]?.ToString() ?? string.Empty;

                                if (!string.IsNullOrEmpty(groupNumber))
                                {
                                    // Dynamically set the values in the textboxes based on the retrieved group numbers
                                    // This is a placeholder logic, depending on your exact requirements
                                    // Here, we assume you want to distribute groups evenly across textboxes
                                    if (groupCount % 3 == 0)
                                        groupbox2.AppendText(groupNumber + Environment.NewLine);
                                    else if (groupCount % 3 == 1)
                                        groupbox3.AppendText(groupNumber + Environment.NewLine);
                                    else
                                        groupbox4.AppendText(groupNumber + Environment.NewLine);

                                    groupCount++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading groups: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadTimeShifts()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT TimeShiftName FROM TimeShifts";  // Assuming 'TimeShifts' is the table name

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string timeShiftName = reader["TimeShiftName"]?.ToString() ?? string.Empty;
                                lstTimeShifts.Items.Add(timeShiftName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading time shifts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Path for saving the Excel file
            string filePath = Path.Combine(@"C:\excellsheet", "RotationSchedule.xlsx");

            using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook()) // Open existing workbook or create new one
            {
                // Check if the worksheet exists, otherwise add a new one
                var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Rotation Schedule")
                                ?? workbook.Worksheets.Add("Rotation Schedule");

                try
                {
                    // Retrieve number of rotations from TextBox (optional)
                    int numberOfRotations = 1; // Default to 1 if not provided
                    if (!string.IsNullOrWhiteSpace(txtNumberOfWeeks.Text) && int.TryParse(txtNumberOfWeeks.Text, out int parsedRotations))
                    {
                        numberOfRotations = parsedRotations;
                    }

                    // Retrieve selected shift times from TimeShiftList ListBox (optional)
                    var selectedShifts = lstTimeShifts.SelectedItems.Cast<string>().ToArray();
                    if (selectedShifts.Length == 0)
                    {
                        selectedShifts = new[] { "7am to 3pm", "3pm to 11pm", "11pm to 7am" }; // Add default shifts, including the new 11pm to 7am
                    }

                    // Retrieve year levels from ListBoxYearLevels (optional)
                    var yearLevels = lstYearLevels.SelectedItems.Cast<string>().Select(s => s.Trim()).ToArray();
                    if (yearLevels.Length == 0)
                    {
                        yearLevels = new[] { "2nd Year", "3rd Year", "4th Year" }; // Default year levels
                    }

                    // Retrieve groups from textboxes (use the entered values)
                    int groupCount2ndYear = int.TryParse(groupbox2.Text, out int groupBox2Groups) ? groupBox2Groups : 0;
                    int groupCount3rdYear = int.TryParse(groupbox3.Text, out int groupBox3Groups) ? groupBox3Groups : 0;
                    int groupCount4thYear = int.TryParse(groupbox4.Text, out int groupBox4Groups) ? groupBox4Groups : 0;

                    // Retrieve start and end dates from the DateTimePickers
                    DateTime startDate = dtpStartDate.Value;
                    DateTime endDate = dtpEndDate.Value;
                    if (startDate > endDate)
                    {
                        MessageBox.Show("Start date cannot be after the end date. Using default values.");
                        startDate = DateTime.Now;
                        endDate = startDate.AddDays(7 * numberOfRotations); // Default to one week per rotation
                    }

                    Dictionary<string, int> yearLevelStartRow = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "2nd Year", 2 },  // Start at row 2
            { "3rd Year", 18 },  // Skip 13 rows from 2nd Year
            { "4th Year", 34 }   // Skip 13 rows from 3rd Year
        };

                    // Map the group counts to the corresponding year levels
                    Dictionary<string, int> groupCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "2nd Year", groupCount2ndYear },
            { "3rd Year", groupCount3rdYear },
            { "4th Year", groupCount4thYear }
        };

                    // Adjust column widths for better readability
                    worksheet.Column(1).Width = 18;  // Year level and labels
                    worksheet.Column(2).Width = 25;  // Rotation info
                    worksheet.Column(3).Width = 25;  // Date range
                    worksheet.Column(4).Width = 25;  // Timeshift
                    worksheet.Column(5).Width = 25;  // Hours

                    // Find the last used column in the second year (to align all year levels)
                    int secondYearRow = yearLevelStartRow["2nd Year"];
                    int lastUsedColumn = worksheet.LastColumnUsed()?.ColumnNumber() ?? 1;

                    // Find the last rotation number from the last populated rotation in the second year
                    int lastRotationNumber = 0;
                    for (int col = 2; col <= lastUsedColumn; col++)
                    {
                        if (int.TryParse(worksheet.Cell(secondYearRow, col).GetString(), out int rotationNumber))
                        {
                            lastRotationNumber = Math.Max(lastRotationNumber, rotationNumber);
                        }
                    }

                    // Increment the rotation number for the new set of shifts
                    lastRotationNumber++;  // Start with the next rotation number after the last one

                    // Start appending new rotations for all year levels from the same column as the second year
                    int currentCol = lastUsedColumn + 1;

                    // Insert year labels and labels for all year levels before adding the rotations
                    foreach (var yearLevel in yearLevels)
                    {
                        int currentRow = yearLevelStartRow[yearLevel];

                        // Insert the year level label (e.g., "2nd Year", "3rd Year")
                        worksheet.Range(currentRow - 1, 1, currentRow - 1, 4).Merge();
                        worksheet.Cell(currentRow - 1, 1).Value = yearLevel;
                        worksheet.Cell(currentRow - 1, 1).Style.Font.SetBold(true).Font.SetFontSize(16);
                        worksheet.Cell(currentRow - 1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                        // Insert labels for Rotation, Date, Timeshift, Hours
                        worksheet.Cell(currentRow, 1).Value = "Rotation";
                        worksheet.Cell(currentRow + 1, 1).Value = "Date";
                        worksheet.Cell(currentRow + 2, 1).Value = "Timeshift";
                        worksheet.Cell(currentRow + 3, 1).Value = "Hours";

                        worksheet.Cell(currentRow, 1).Style.Font.SetBold(true);
                        worksheet.Cell(currentRow + 1, 1).Style.Font.SetBold(true);
                        worksheet.Cell(currentRow + 2, 1).Style.Font.SetBold(true);
                        worksheet.Cell(currentRow + 3, 1).Style.Font.SetBold(true);

                        // Center-align the text in the labels
                        worksheet.Range(currentRow, 1, currentRow + 3, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                                  .Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }

                    // Process rotations for all shifts and year levels
                    for (int rotation = 0; rotation < numberOfRotations; rotation++)
                    {
                        // Loop through each shift
                        for (int shiftIndex = 0; shiftIndex < selectedShifts.Length; shiftIndex++)
                        {
                            // Process each year level
                            foreach (var yearLevel in yearLevels)
                            {
                                int currentRow = yearLevelStartRow[yearLevel];
                                int groupCount = groupCounts[yearLevel];  // Get the number of groups for the current year level

                                // Process the second year and determine the rotation number
                                worksheet.Column(currentCol).Width = 20;

                                // Calculate the start date for the current week
                                DateTime currentWeekStartDate = startDate.AddDays(7 * rotation);
                                DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);

                                // Ensure the week stays within the bounds of the end date
                                if (currentWeekEndDate > endDate)
                                {
                                    currentWeekEndDate = endDate;
                                }

                                // Populate the rotation number, week range, shift times, and hours
                                worksheet.Cell(yearLevelStartRow[yearLevel], currentCol).Value = $"{lastRotationNumber}";
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 1, currentCol).Value = $"{currentWeekStartDate:MMM dd} - {currentWeekEndDate:MMM dd}";
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 2, currentCol).Value = selectedShifts[shiftIndex];
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 3, currentCol).Value = "48 HOURS";

                                // Apply center alignment dynamically
                                worksheet.Cell(currentRow, currentCol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                   .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                worksheet.Cell(currentRow + 1, currentCol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                   .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                worksheet.Cell(currentRow + 2, currentCol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                   .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                worksheet.Cell(currentRow + 3, currentCol).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                                                                   .Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                // Apply alternating background color for shifts
                                XLColor backgroundColor = (shiftIndex % 2 == 0) ? XLColor.White : XLColor.LightGreen;

                                worksheet.Cell(yearLevelStartRow[yearLevel], currentCol).Style.Fill.SetBackgroundColor(backgroundColor);
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 1, currentCol).Style.Fill.SetBackgroundColor(backgroundColor);
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 2, currentCol).Style.Fill.SetBackgroundColor(backgroundColor);
                                worksheet.Cell(yearLevelStartRow[yearLevel] + 3, currentCol).Style.Fill.SetBackgroundColor(backgroundColor);

                                // Insert the groups below the "Hours" row
                                for (int i = 0; i < groupCount; i++)
                                {
                                    // Calculate the group row, i.e., the row just below "Hours"
                                    int groupRow = yearLevelStartRow[yearLevel] + 4 + i;

                                    // Insert the group in the left-most column (column 1)
                                    worksheet.Cell(groupRow, 1).Value = $"Group {i + 1}";

                                    // Apply center alignment for groups
                                    worksheet.Cell(groupRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                                    // Set background color to light blue
                                    worksheet.Cell(groupRow, 1).Style.Fill.SetBackgroundColor(XLColor.LightBlue);
                                }
                            }

                            // Move to the next column
                            currentCol++;
                        }

                        // Increment the rotation number after all shifts in this rotation are done
                        lastRotationNumber++;
                    }

                    // Save the Excel file
                    workbook.SaveAs(filePath);
                    MessageBox.Show($"Excel file updated successfully at {filePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Path for the existing Excel file
            string filePath = Path.Combine(@"C:\excellsheet\RotationSchedule.xlsx");

            try
            {
                // Check if the file exists before attempting to delete it
                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                    MessageBox.Show("The Excel file has been successfully deleted.");
                }
                else
                {
                    MessageBox.Show("The file does not exist.");
                }
            }
            catch (Exception ex)
            {
                // Display any errors that occur during the file deletion process
                MessageBox.Show($"An error occurred while trying to delete the file: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Path for the existing Excel file
                string filePath = @"C:\excellsheet\RotationSchedule.xlsx";

                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Open the file using the default application (Excel)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = filePath,
                        UseShellExecute = true // This ensures it opens with the default program
                    });
                }
                else
                {
                    MessageBox.Show("The Excel file does not exist at the specified location.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to open the file: {ex.Message}");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Find all running Excel processes
                foreach (var process in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
                {
                    if (!process.HasExited)
                    {
                        process.Kill(); // Forcefully close the Excel application
                        process.WaitForExit(); // Ensure the process is closed before continuing
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to close Excel: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           // nothing here it is already changed 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // nothing to follow it is already changed 
        }

        private void lstYearLevels_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Path for saving the Excel file
            // keep button for the deployment of C.I 
            string filePath = Path.Combine(@"C:\excellsheet\", "RotationSchedule.xlsx");

            using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook()) // Open existing workbook or create new one
            {
                var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Rotation Schedule")
                                ?? workbook.Worksheets.Add("Rotation Schedule");

                try
                {
                    // Define a structure to track each Clinical Instructor's information
                    var clinicalInstructorsInfo = new Dictionary<string, (XLColor Color, int LastWeek)>();

                    // Retrieve the selected Clinical Instructor
                    string selectedCI = lstClinicalInstructors.SelectedItem?.ToString()?.Trim() ?? "Unknown";

                    if (string.IsNullOrEmpty(selectedCI))
                    {
                        MessageBox.Show("No Clinical Instructor selected.");
                        return;
                    }

                    // Retrieve the C.I.'s colors from the database (background and text color)
                    var (backgroundColor, textColor) = GetInstructorColorsFromDatabase(selectedCI);

                    // Check if we already have information for this C.I.
                    if (!clinicalInstructorsInfo.ContainsKey(selectedCI))
                    {
                        clinicalInstructorsInfo[selectedCI] = (backgroundColor, 0); // Initialize the C.I.'s info with week 0
                    }

                    // Retrieve the number of groups from the textboxes
                    int groupsIn2ndYear = int.TryParse(groupbox2.Text, out int g2) ? g2 : 0;
                    int groupsIn3rdYear = int.TryParse(groupbox3.Text, out int g3) ? g3 : 0;
                    int groupsIn4thYear = int.TryParse(groupbox4.Text, out int g4) ? g4 : 0;

                    // Combine all groups into a single list
                    List<int> allGroups = new List<int>();
                    for (int i = 1; i <= groupsIn2ndYear; i++) allGroups.Add(i); // Add groups from 2nd year
                    for (int i = 1; i <= groupsIn3rdYear; i++) allGroups.Add(i + 100); // Add groups from 3rd year (100 series)
                    for (int i = 1; i <= groupsIn4thYear; i++) allGroups.Add(i + 200); // Add groups from 4th year (200 series)

                    // Retrieve the selected areas from listbox1
                    var selectedAreas = listBox1.SelectedItems.Cast<string>().Select(s => s.Trim()).ToArray();
                    if (selectedAreas.Length == 0)
                    {
                        MessageBox.Show("No areas selected.");
                        return;
                    }

                    // Define starting rows for year levels
                    Dictionary<string, int> yearLevelStartRows = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
            { "2nd year", 6 },  // Start row for 2nd Year
            { "3rd year", 22 },  // Start row for 3rd Year
            { "4th year", 38 }   // Start row for 4th Year
        };

                    // Define the base columns for each timeshift
                    Dictionary<string, int> baseTimeshiftColumns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
            { "7am to 3pm", 2 },   // Base column for 7am to 3pm
            { "3pm to 11pm", 3 },  // Base column for 3pm to 11pm
            { "11pm to 7am", 4 }   // Base column for 11pm to 7am
        };

                    // Retrieve the selected year levels
                    var selectedYearLevels = lstYearLevels.SelectedItems.Cast<string>().Select(s => s.Trim().ToLowerInvariant()).ToArray();
                    if (selectedYearLevels.Length == 0)
                    {
                        MessageBox.Show("No year levels selected.");
                        return;
                    }

                    // Retrieve selected timeshifts
                    var selectedTimeshifts = lstTimeShifts.SelectedItems.Cast<string>().Select(s => s.Trim()).ToArray();
                    if (selectedTimeshifts.Length == 0)
                    {
                        MessageBox.Show("No timeshifts selected.");
                        return;
                    }

                    // Get the number of rotations dynamically
                    int numberOfRotations;
                    if (!int.TryParse(textBox1.Text, out numberOfRotations) || numberOfRotations <= 0)
                    {
                        MessageBox.Show("Invalid number of rotations.");
                        return;
                    }

                    // Retrieve weeks to exclude from the week excluder checklistbox
                    var excludedWeeks = checklistboxExclude.CheckedItems.Cast<string>().Select(int.Parse).ToHashSet();

                    // Initialize random number generator
                    Random random = new Random();

                    // Function to find the last week for the specific Clinical Instructor (based on color coding)
                    int GetLastWeekForCIBasedOnColor(XLColor ciColor)
                    {
                        int lastWeek = 0;

                        // Loop through each timeshift
                        foreach (var timeshift in baseTimeshiftColumns.Keys)
                        {
                            int timeshiftColumn = baseTimeshiftColumns[timeshift];

                            // Loop through each year level to find the last week where a C.I. rotation exists based on color
                            foreach (var yearLevel in yearLevelStartRows.Keys)
                            {
                                int startingRowForYearLevel = yearLevelStartRows[yearLevel];

                                // Loop through the weeks to find the last filled week for the C.I.
                                for (int week = 0; week < 50; week++) // Assuming 50 as the maximum number of weeks
                                {
                                    int weekOffset = week * 3; // Each week starts 3 columns later
                                    int targetColumn = timeshiftColumn + weekOffset;

                                    bool isWeekFilledForCI = false;

                                    // Check each group for that week and timeshift
                                    for (int groupNumber = 1; groupNumber <= 10; groupNumber++) // Assuming 10 groups
                                    {
                                        int targetRow = startingRowForYearLevel + groupNumber - 1;

                                        // Check if the current cell matches the C.I.'s color
                                        if (worksheet.Cell(targetRow, targetColumn).Style.Fill.BackgroundColor == ciColor)
                                        {
                                            isWeekFilledForCI = true;
                                            break;
                                        }
                                    }

                                    // If this week was filled for the CI, update the last filled week
                                    if (isWeekFilledForCI)
                                    {
                                        lastWeek = Math.Max(lastWeek, week + 1);
                                    }
                                }
                            }
                        }

                        return lastWeek;
                    }

                    // Get the last week for the selected C.I. using its color from the database
                    int lastWeekForCI = GetLastWeekForCIBasedOnColor(backgroundColor);
                    // Insert the rotations starting from the last known week for the current C.I.
                    foreach (var timeshift in selectedTimeshifts)
                    {
                        // Copy the list of all available groups
                        List<int> groupsForRotationCycle = new List<int>(allGroups);

                        // Ensure the number of groups does not exceed the number of rotations
                        if (groupsForRotationCycle.Count > numberOfRotations)
                        {
                            // Shuffle and take only the first 'numberOfRotations' groups
                            groupsForRotationCycle = groupsForRotationCycle.OrderBy(g => random.Next()).Take(numberOfRotations).ToList();
                        }

                        // Start from the next week after the last filled week
                        int startingWeek = lastWeekForCI + 1;

                        for (int rotation = 0; rotation < numberOfRotations; rotation++)
                        {
                            int week = startingWeek + rotation;

                            // Skip the weeks that are marked as excluded
                            if (excludedWeeks.Contains(week))
                            {
                                continue;
                            }

                            if (groupsForRotationCycle.Count == 0)
                            {
                                // If no groups are left in this cycle, reset the groups for the next rotation cycle
                                groupsForRotationCycle = new List<int>(allGroups);
                                if (groupsForRotationCycle.Count > numberOfRotations)
                                {
                                    // Shuffle and take only the first 'numberOfRotations' groups
                                    groupsForRotationCycle = groupsForRotationCycle.OrderBy(g => random.Next()).Take(numberOfRotations).ToList();
                                }
                            }

                            int weekOffset = (week - 1) * 3;
                            int timeshiftColumn = baseTimeshiftColumns[timeshift];
                            int targetColumn = timeshiftColumn + weekOffset;

                            foreach (var yearLevel in selectedYearLevels)
                            {
                                int startingRowForYearLevel = yearLevelStartRows[yearLevel];

                                // Randomly select one group to assign for the week
                                int randomGroupIndex = random.Next(groupsForRotationCycle.Count);
                                int groupToAssign = groupsForRotationCycle[randomGroupIndex];
                                groupsForRotationCycle.RemoveAt(randomGroupIndex); // Remove selected group from the cycle

                                int actualGroupNumber;
                                if (groupToAssign > 200) actualGroupNumber = groupToAssign - 200;
                                else if (groupToAssign > 100) actualGroupNumber = groupToAssign - 100;
                                else actualGroupNumber = groupToAssign;

                                int targetRow = startingRowForYearLevel + actualGroupNumber - 1;

                                if (string.IsNullOrWhiteSpace(worksheet.Cell(targetRow, targetColumn).GetString()))
                                {
                                    int randomAreaIndex = random.Next(selectedAreas.Length);
                                    string areaToAssign = selectedAreas[randomAreaIndex];

                                    worksheet.Cell(targetRow, targetColumn).Value = areaToAssign;
                                    worksheet.Cell(targetRow, targetColumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    worksheet.Cell(targetRow, targetColumn).Style.Fill.SetBackgroundColor(backgroundColor);
                                }
                                else
                                {
                                    MessageBox.Show($"Conflict detected in year level {yearLevel}, group {actualGroupNumber}, timeshift {timeshift}, week {week + 1}.");
                                }
                            }
                        }
                    }


                    // Save the workbook
                    workbook.SaveAs(filePath);
                    MessageBox.Show($"Excel file updated successfully at {filePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }



            // Function to retrieve background and text color from the database
            (XLColor backgroundColor, XLColor textColor) GetInstructorColorsFromDatabase(string instructorName)
            {
                string backgroundColorName = "";
                string textColorName = "";

                // SQL connection and query to retrieve the background and text colors
                string connectionString = "Server=127.0.0.1;Port=3306;Database=clinicalrotationplanner;Uid=root;"; // Replace with your actual connection string
                string query = "SELECT backgroundColor, textColor FROM ClinicalInstructors WHERE InstructorName = @InstructorName";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@instructorName", instructorName);

                        // Execute the query and retrieve the colors
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                backgroundColorName = reader["backgroundColor"]?.ToString() ?? "NoColor"; // Use default "NoColor"
                                textColorName = reader["textColor"]?.ToString() ?? "Black"; // Default to "Black"
                            }
                            else
                            {
                                MessageBox.Show($"No colors found for the Clinical Instructor: {instructorName}");
                                return (XLColor.NoColor, XLColor.Black); // Default to no color and black text if not found
                            }
                        }
                    }
                }
                // line 948 to line 1056 is to be fixed later 

                // Map the color names to XLColor for ClosedXML
                XLColor backgroundColor = MapColorNameToXLColor(backgroundColorName);
                XLColor textColor = MapColorNameToXLColor(textColorName);

                return (backgroundColor, textColor);
            }

            // Helper function to map color names from the database to XLColor
            XLColor MapColorNameToXLColor(string colorName)
            {
                switch (colorName?.ToLower()) // Ensure colorName is not null
                {
                    case "red":
                        return XLColor.Red;
                    case "blue":
                        return XLColor.Blue;
                    case "green":
                        return XLColor.Green;
                    case "yellow":
                        return XLColor.Yellow;
                    case "pink":
                        return XLColor.Pink;
                    case "brown":
                        return XLColor.Brown;
                    case "gray":
                        return XLColor.Gray;
                    case "orange":
                        return XLColor.Orange;
                    case "violet":
                        return XLColor.Violet;
                    // Add more colors as needed
                    default:
                        return XLColor.NoColor; // Default to no color if not recognized
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lstTimeShifts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstClinicalInstructors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Path for saving the Excel file
            // new button for the clearing of areas 
            string filePath = Path.Combine(@"C:\excellsheet", "RotationSchedule.xlsx");

            using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook()) // Open existing workbook or create new one
            {
                // Check if the worksheet exists
                var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Rotation Schedule");

                if (worksheet == null)
                {
                    MessageBox.Show("Rotation Schedule worksheet does not exist.");
                    return;
                }

                try
                {
                    // Get the number of weeks from user input (assuming it's input via a TextBox or another control)
                    if (!int.TryParse(txtNumberOfWeeks.Text, out int numberOfWeeks) || numberOfWeeks <= 0)
                    {
                        MessageBox.Show("Invalid number of weeks.");
                        return;
                    }

                    // Retrieve selected year levels from lstYearLevels (ListBox)
                    var selectedYearLevels = lstYearLevels.SelectedItems.Cast<string>().Select(s => s.Trim().ToLowerInvariant()).ToArray();
                    if (selectedYearLevels.Length == 0)
                    {
                        MessageBox.Show("No year levels selected.");
                        return;
                    }

                    // Retrieve selected timeshifts from lstTimeShifts (ListBox)
                    var selectedTimeshifts = lstTimeShifts.SelectedItems.Cast<string>().Select(s => s.Trim().ToLowerInvariant()).ToArray();
                    if (selectedTimeshifts.Length == 0)
                    {
                        MessageBox.Show("No timeshifts selected.");
                        return;
                    }

                    // Define starting rows for year levels with keys in lowercase for consistency
                    Dictionary<string, int> yearLevelStartRows = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "2nd year", 6 },  // Start row for 2nd Year
            { "3rd year", 22 }, // Start row for 3rd Year
            { "4th year", 38 }  // Start row for 4th Year
        };

                    // Define the base columns for each timeshift (adjust based on your layout)
                    Dictionary<string, int> baseTimeshiftColumns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "7am to 3pm", 2 },   // Base column for 7am to 3pm
            { "3pm to 11pm", 3 },  // Base column for 3pm to 11pm
            { "11pm to 7am", 4 }   // Base column for 11pm to 7am
        };

                    // Clear the areas for each selected year level, timeshift, and week
                    foreach (var yearLevel in selectedYearLevels)
                    {
                        if (!yearLevelStartRows.ContainsKey(yearLevel))
                        {
                            MessageBox.Show($"Year level '{yearLevel}' is not defined.");
                            continue;
                        }

                        int startingRowForYearLevel = yearLevelStartRows[yearLevel];

                        // **Read existing groups from Excel dynamically based on content**:
                        int availableGroups = 0;
                        int currentRow = startingRowForYearLevel;

                        // Dynamically count the number of non-empty rows for groups
                        while (!string.IsNullOrWhiteSpace(worksheet.Cell(currentRow, 1).GetString()))
                        {
                            availableGroups++;
                            currentRow++;
                        }

                        if (availableGroups == 0)
                        {
                            MessageBox.Show($"No available groups found for {yearLevel}. Skipping.");
                            continue; // Skip this year level if no groups are available
                        }

                        // Clear areas for the specified number of weeks
                        for (int week = 0; week < numberOfWeeks; week++)
                        {
                            int weekOffset = week * 3; // Each week starts 3 columns later (one column for each timeshift)

                            // Clear only the selected timeshifts
                            foreach (var selectedTimeshift in selectedTimeshifts)
                            {
                                if (!baseTimeshiftColumns.ContainsKey(selectedTimeshift))
                                {
                                    MessageBox.Show($"Timeshift '{selectedTimeshift}' is not defined.");
                                    continue;
                                }

                                int timeshiftColumn = baseTimeshiftColumns[selectedTimeshift] + weekOffset; // Calculate the dynamic column for this timeshift and week

                                // Loop through each row (group) for this year level and timeshift column
                                for (int j = 0; j < availableGroups; j++) // Limit clearing to the number of available groups
                                {
                                    int targetRow = startingRowForYearLevel + j; // Calculate the target row

                                    // Clear the cell in the timeshift column
                                    worksheet.Cell(targetRow, timeshiftColumn).Clear();
                                }
                            }
                        }
                    }

                    // Reset the C.I.'s total rotations to zero after clearing areas
                    foreach (var yearLevel in selectedYearLevels)
                    {
                        if (yearLevelStartRows.ContainsKey(yearLevel))
                        {
                            int startingRowForYearLevel = yearLevelStartRows[yearLevel];
                            int finalRowForCI = startingRowForYearLevel + 40; // Skip to the row where the C.I. name is

                            // Find and clear the "Total Rotations" cell (assuming it's right below the C.I. name)
                            worksheet.Cell(finalRowForCI + 1, baseTimeshiftColumns["7am to 3pm"]).Value = ""; // Reset to 0
                        }
                    }

                    // Save the updated Excel file
                    workbook.SaveAs(filePath);
                    MessageBox.Show("Areas cleared successfully, and rotations reset.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while clearing areas: {ex.Message}");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Path for saving the Excel file
            // new button for the area putting
            string filePath = Path.Combine(@"C:\excellsheet", "RotationSchedule.xlsx");

            using (var workbook = File.Exists(filePath) ? new XLWorkbook(filePath) : new XLWorkbook()) // Open existing workbook or create new one
            {
                // Check if the worksheet exists, otherwise add a new one
                var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Rotation Schedule")
                                ?? workbook.Worksheets.Add("Rotation Schedule");

                try
                {
                    // Retrieve selected areas from lstDepartments, trimming spaces
                    var selectedAreas = lstDepartments.SelectedItems.Cast<string>().Select(s => s.Trim()).ToArray();
                    if (selectedAreas.Length == 0)
                    {
                        MessageBox.Show("No areas selected.");
                        return;
                    }

                    // Retrieve selected year levels from lstYearLevels, trimming spaces and converting to lowercase
                    var selectedYearLevels = lstYearLevels.SelectedItems.Cast<string>().Select(s => s.Trim().ToLowerInvariant()).ToArray();
                    if (selectedYearLevels.Length == 0)
                    {
                        MessageBox.Show("No year levels selected.");
                        return;
                    }

                    // Retrieve selected timeshift from lstTimeShifts (assuming only one timeshift selected)
                    var selectedTimeshift = lstTimeShifts.SelectedItem?.ToString()?.Trim().ToLowerInvariant();
                    if (string.IsNullOrEmpty(selectedTimeshift))
                    {
                        MessageBox.Show("No timeshift selected.");
                        return;
                    }

                    // Get the number of weeks from user input (assuming it's input via a TextBox or another control)
                    if (!int.TryParse(txtNumberOfWeeks.Text, out int numberOfWeeks) || numberOfWeeks <= 0)
                    {
                        MessageBox.Show("Invalid number of weeks.");
                        return;
                    }

                    // Define starting rows for year levels
                    Dictionary<string, int> yearLevelStartRows = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "2nd year", 6 },  // Start row for 2nd Year
            { "3rd year", 22 },  // Start row for 3rd Year
            { "4th year", 38 }  // Start row for 4th Year
        };

                    // Define the base columns for each timeshift
                    Dictionary<string, int> baseTimeshiftColumns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "7am to 3pm", 2 },   // Base column for 7am to 3pm
            { "3pm to 11pm", 3 },  // Base column for 3pm to 11pm
            { "11pm to 7am", 4 }   // Base column for 11pm to 7am
        };

                    if (!baseTimeshiftColumns.ContainsKey(selectedTimeshift))
                    {
                        MessageBox.Show($"Timeshift '{selectedTimeshift}' is not defined.");
                        return;
                    }

                    // Read the number of groups from the textboxes in groupbox2, groupbox3, and groupbox4
                    int[] numberOfGroups = new int[3]; // Array to store the number of groups for each year level

                    bool validInput = int.TryParse(groupbox2.Text, out numberOfGroups[0]) &&
                                      int.TryParse(groupbox3.Text, out numberOfGroups[1]) &&
                                      int.TryParse(groupbox4.Text, out numberOfGroups[2]);

                    if (!validInput || numberOfGroups.Any(g => g <= 0))
                    {
                        MessageBox.Show("Invalid number of groups.");
                        return;
                    }

                    // Function to read the number of available groups from Excel
                    int GetAvailableGroups(int startRow, IXLWorksheet worksheet)
                    {
                        int availableGroups = 0;
                        int currentRow = startRow;

                        // Check column 1 (or a relevant column) for non-empty cells
                        while (!string.IsNullOrWhiteSpace(worksheet.Cell(currentRow, 1).GetString()))
                        {
                            availableGroups++;
                            currentRow++;
                        }

                        return availableGroups;
                    }

                    // Validate if selected groups exist for the selected year levels
                    for (int i = 0; i < selectedYearLevels.Length; i++)
                    {
                        var yearLevel = selectedYearLevels[i];
                        if (!yearLevelStartRows.ContainsKey(yearLevel))
                        {
                            MessageBox.Show($"Year level '{yearLevel}' is not defined.");
                            return;
                        }

                        int startingRow = yearLevelStartRows[yearLevel];

                        // Get available groups dynamically from Excel
                        int availableGroups = GetAvailableGroups(startingRow, worksheet);

                        if (numberOfGroups[i] > availableGroups)
                        {
                            MessageBox.Show($"The number of groups for '{yearLevel}' exceeds the available groups. Maximum groups: {availableGroups}.");
                            return;
                        }
                    }

                    // Find the next available week for the selected timeshift
                    int week = 0;
                    int timeshiftColumn = baseTimeshiftColumns[selectedTimeshift];

                    // Iterate through the weeks to find the first unfilled week
                    while (true)
                    {
                        int weekOffset = week * 3; // Each week starts 3 columns later (since there are 3 timeshifts per week)
                        int targetColumn = timeshiftColumn + weekOffset;

                        bool isWeekFilled = false;
                        foreach (var yearLevel in selectedYearLevels)
                        {
                            int startingRowForYearLevel = yearLevelStartRows[yearLevel];
                            bool isYearLevelFilled = false;

                            // Check if any group in the current week and timeshift is already filled
                            for (int g = 0; g < numberOfGroups[selectedYearLevels.ToList().IndexOf(yearLevel)]; g++)
                            {
                                int targetRow = startingRowForYearLevel + g;
                                if (!string.IsNullOrWhiteSpace(worksheet.Cell(targetRow, targetColumn).GetString()))
                                {
                                    isYearLevelFilled = true;
                                    break; // If any group is filled, the week is considered filled for this year level
                                }
                            }

                            if (isYearLevelFilled)
                            {
                                isWeekFilled = true;
                                break; // If the week is filled for any year level, move to the next week
                            }
                        }

                        if (!isWeekFilled)
                        {
                            break; // If the week is not filled for any year level, stop searching and start filling this week
                        }

                        week++; // Move to the next week
                    }

                    // Now insert the areas starting from the first available week
                    for (int w = 0; w < numberOfWeeks; w++)
                    {
                        int weekOffset = (week + w) * 3; // Calculate the offset for each week dynamically
                        int targetColumn = timeshiftColumn + weekOffset;

                        // Insert areas for each selected year level and groups
                        for (int i = 0; i < selectedYearLevels.Length; i++)
                        {
                            var yearLevel = selectedYearLevels[i];
                            int startingRowForYearLevel = yearLevelStartRows[yearLevel];
                            int areaIndex = 0; // Track the current area being placed

                            // Insert areas only for the number of groups specified for this year level
                            for (int g = 0; g < numberOfGroups[i]; g++)
                            {
                                if (areaIndex >= selectedAreas.Length) // If we run out of areas, reset the index to start over
                                {
                                    areaIndex = 0;
                                }

                                int targetRow = startingRowForYearLevel + g; // Adjust row based on group number

                                // Place the area in the timeshift column for the current group
                                worksheet.Cell(targetRow, targetColumn).Value = selectedAreas[areaIndex];
                                worksheet.Cell(targetRow, targetColumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                worksheet.Cell(targetRow, targetColumn).Style.Fill.SetBackgroundColor(XLColor.White); // Optional: Adjust color

                                areaIndex++; // Move to the next area
                            }
                        }
                    }

                    // Save the Excel file
                    workbook.SaveAs(filePath);
                    MessageBox.Show($"Excel file updated successfully at {filePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
