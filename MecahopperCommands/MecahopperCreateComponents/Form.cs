using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;

namespace MecahopperCreateComponents
{
    public class Form : Dialog<DialogResult>
    {
        TextBox linQty;
        TextBox rotQty;

        public int LinearQty
        {
            get
            {
                if (int.TryParse(linQty.Text, out int val))
                    return val;
                else
                    return 0;
            }
        }
        public int RotationQty
        {
            get
            {
                if (int.TryParse(rotQty.Text, out int val))
                    return val;
                else
                    return 0;
            }
        }

        public Form()
        {
            ClientSize = new Eto.Drawing.Size(300, 150);
            Padding = new Padding(5);
            Resizable = false;
            Result = DialogResult.Cancel;
            Title = "MECAHOPPER";
            WindowStyle = WindowStyle.Default;

            DefaultButton = new Button { Text = "OK" };
            DefaultButton.Click += (sender, e) => Close(DialogResult.Ok);

            AbortButton = new Button { Text = "Cancel" };
            AbortButton.Click += (sender, e) => Close(DialogResult.Cancel);

            var linearLabel = new Label { Text = "Linear Elements:" };
            

            var defaults_layout = new TableLayout
            {
                Padding = new Padding(5, 10, 5, 5),
                Spacing = new Size(5, 5),
                Rows = { new TableRow(null, DefaultButton, AbortButton) }
            };

            var layout = new TableLayout()
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5)
            };

            linQty = new TextBox() { PlaceholderText = "0" };
            rotQty = new TextBox() { PlaceholderText = "0" };

            layout.Rows.Add(new TableRow(
                new Label { Text = "Linear Nodes" },
                linQty
                ));
            layout.Rows.Add(new TableRow(
                new Label { Text = "Rotate Nodes" },
                rotQty
                ));
            layout.Rows.Add(defaults_layout);

            Content = layout;
        }
    }
}
