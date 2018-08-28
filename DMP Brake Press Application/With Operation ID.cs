// #11
            // Excecutes When a Specific DMP ID has Run on Brake Press Selected 
            // Returns Specified DMP ID has Run on Selected  Brake Press
            if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #14
            // Excecutes When a Specific Brake Press and DMDID are Selected with DataStartPicker Selected 
            // Returns the Specific DMP ID Users Ran on Specified Brake Press Between the Start Date and The End Date
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
            // #12
            // Excecutes When a Specific Brake Press and DataStartPicker has been Selected 
            // Returns the Operations on Specified Brake Press Between the Start Date and The End Date
            if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
            // #10
            // Excecutes When a Specific Item ID has Run on Brake Press Selected 
            // Returns Specified Item ID Run on Selected  Brake Press
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
            // #13
            // Excecutes When a Specific Brake Press and Item ID are Selected as well as DataStartPicker Selected 
            // Returns the Specific Item ID Run on Specified Brake Press Between the Start Date and The End Date
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

            // #18
            // Excecutes When a Specific Brake Press and Item ID are Selected as well as DataStartPicker Selected 
            // Returns the Specific Item ID Run on Specified Brake Press Between the Start Date and The End Date
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }



            // #15
            // Excecutes When a Specific Brake Press and DMP ID are Selected
            // Returns the Specific DMP ID Ran on Specified Brake Press All
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
            // #3
            // Excecutes When Item ID, DMP ID are Entered and DateStartPicker has Selected a Date
            // Returns Every Operation Between the Start Date and The End Date Where the Item ID has been Run by DMP ID User
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

            // #7
            // Excecutes When Item ID and DateStart are Entered
            // Returns Every Operation of the Specified Item ID Between the Start Date and The End Date

            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }


            // #5
            // Excecutes When DMP ID has been Entered and DateStartPicker has Selected a Date
            // Returns Every Operation Run By Selected DMP ID Between the Start Date and The End Date
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

            // #2
            // Excecutes When Item ID and DMP ID Fields are Entered
            // Returns Entire History of the Item ID When Run By Specific Operator 
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text == "" && SearchDMPID_TextBox.Text != "")
            {
                //SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[ItemOperationData] as D INNER JOIN [dbo].[OperationOEE] as O ON D.OperationID = O.OperationID WHERE D.ItemID='" + ItemID_TextBox.Text + "' AND D.DMPID='" + SearchDMPID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }

            // #16
            // Excecutes When Item ID and Operation ID Fields are Entered
            // Returns One Operation Specified by Operation ID
            else if (ItemID_TextBox.Text != "" && OperationID_TextBox.Text != "")
            {
                //SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[ItemOperationData] as D INNER JOIN [dbo].[OperationOEE] as O ON D.OperationID = O.OperationID WHERE D.ItemID='" + ItemID_TextBox.Text + "' AND D.DMPID='" + SearchDMPID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.OperationID='" + OperationID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }

            // #1
            // Excecutes When Item ID Field is Entered
            // Returns Entire History of the Item ID Run By Every Operator 
            else if (ItemID_TextBox.Text != "")
            {
                //SearchCommand = "SELECT ItemID,OperationID,ItemRunCount,StartDateTime,EndDateTime,EmployeeName,DMPID,BrakePress,PartsManufactured,SetupTime,PartsPerMinute FROM [dbo].[ItemOperationData] WHERE ItemID='" + ItemID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }

            // #15
            // Excecutes When Operation ID is Entered
            // Returns One Operation Specified by Operation ID
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text != "" && SearchDMPID_TextBox.Text == "")
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.OperationID='" + OperationID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }

            // #17
            // Excecutes When Operation ID and DMP ID Fields are Entered
            // Returns One Operation Specified by Operation ID
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text != "" && SearchDMPID_TextBox.Text != "")
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.OperationID='" + OperationID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }

            // #4
            // Excecutes When DMP ID Field is Entered
            // Returns Entire History Operations Completed By Specified Operator
            else if (ItemID_TextBox.Text == "" && SearchDMPID_TextBox.Text != "")
            {
                //SearchCommand = "SELECT ItemID,OperationID,ItemRunCount,StartDateTime,EndDateTime,EmployeeName,DMPID,BrakePress,PartsManufactured,SetupTime,PartsPerMinute FROM [dbo].[ItemOperationData] WHERE DMPID='" + SearchDMPID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }

            // #6
            // Excecutes When DateStartPicker has Selected a Date
            // Returns Every Operation Between the Start Date and The End Date
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

            // #9
            // Excecutes When a Specific Brake Press has been Selected 
            // Returns Every Operation Performed on Selected Brake Press
            else if (BrakePress_TextBox.Text != "")
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }

            // Last
            // Excecutes When Item ID, Operation ID, DMP ID, and DateStartPicker are all Empty
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                MessageBox.Show("Please Select a Date, DMP ID, or Item Number to Search Data");
            }