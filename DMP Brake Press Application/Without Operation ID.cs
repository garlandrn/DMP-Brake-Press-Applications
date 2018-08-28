            
            // #1
            // Item ID: Yes
            // Brake Press: Yes
			// DMP: Yes
			// Date: Yes
            if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }			
			
			// #2
            // Item ID: Yes
            // Brake Press: Yes
			// DMP: Yes
			// Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
			
						
			// #3
            // Item ID: Yes
            // Brake Press: Yes
			// DMP: No
			// Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

			// #4
            // Item ID: Yes
            // Brake Press: Yes
			// DMP: No
			// Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
			
			// #5
            // Item ID: Yes
            // Brake Press: No
			// DMP: Yes
			// Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
			
			// #6
            // Item ID: Yes
            // Brake Press: No
			// DMP: Yes
			// Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }
			
			
			// #7
            // Item ID: No
            // Brake Press: Yes
			// DMP: Yes
			// Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
			
			// #8
            // Item ID: No
            // Brake Press: Yes
			// DMP: Yes
			// Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }

			
            // #9
            // Item ID: No
            // Brake Press: Yes
			// DMP: No
			// Date: Yes
            if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

			// #10
            // Item ID: No
            // Brake Press: Yes
			// DMP: No
			// Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text != "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.BrakePress='" + BrakePress_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: " + BrakePress_TextBox.Text;
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }

            // #11
            // Item ID: Yes
            // Brake Press: No
			// DMP: No
			// Date: Yes
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }

			// #12
            // Item ID: Yes
            // Brake Press: No
			// DMP: No
			// Date: No
            else if (ItemID_TextBox.Text != "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                //SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[ItemOperationData] as D INNER JOIN [dbo].[OperationOEE] as O ON D.OperationID = O.OperationID WHERE D.ItemID='" + ItemID_TextBox.Text + "' AND D.DMPID='" + SearchDMPID_TextBox.Text + "'";
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.ItemID='" + ItemID_TextBox.Text + "'";
                ReportItemID = "Item ID: " + ItemID_TextBox.Text;
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: All";
                CreateReport();
            }
			
            // #13
            // Item ID: No
            // Brake Press: No
			// DMP: Yes
			// Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.DMPID='" + SearchDMPID_TextBox.Text + "' AND O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
			
            // #14
            // Item ID: No
            // Brake Press: No
			// DMP: Yes
			// Date: No
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text != "" && DateStartPicker.Checked == false)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O  INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.OperationID='" + OperationID_TextBox.Text + "' AND O.DMPID='" + SearchDMPID_TextBox.Text + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: " + SearchDMPID_TextBox.Text;
                ReportDate = "Date: All";
                CreateReport();
            }

			
            // #15
            // Item ID: No
            // Brake Press: No
			// DMP: No
			// Date: Yes
            else if (ItemID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == true)
            {
                SearchCommand = "SELECT D.ItemID as ItemID, D.OperationID as OperationID, O.RunDateTime as RunDateTime, O.OperationTime as OperationTime, O.PlannedTime as PlannedTime, O.Efficiency as Efficiency, O.OEE as OEE, D.BrakePress as BrakePress, D.PartsManufactured as PartsManufactured, D.PartsPerMinute as PartsPerMinute, D.EmployeeName as EmployeeName, D.DMPID as DMPID FROM [dbo].[OperationOEE] as O INNER JOIN [dbo].[ItemOperationData] as D ON O.OperationID = D.OperationID WHERE O.RunDateTime BETWEEN '" + DateStartPicker.Value.Date.ToShortDateString() + "' AND '" + DateEndPicker.Value.Date.ToShortDateString() + "'";
                ReportItemID = "Item ID: All";
                ReportBrakePress = "Brake Press: All";
                ReportEmployee = "DMP ID: All";
                ReportDate = "Date: " + DateStartPicker.Text + " - " + DateEndPicker.Text;
                CreateReport();
            }
			
            // Last
            // Excecutes When Item ID, Operation ID, DMP ID, and DateStartPicker are all Empty
            else if (ItemID_TextBox.Text == "" && OperationID_TextBox.Text == "" && BrakePress_TextBox.Text == "" && SearchDMPID_TextBox.Text == "" && DateStartPicker.Checked == false)
            {
                MessageBox.Show("Please Select a Date, DMP ID, or Item Number to Search Data");
            }