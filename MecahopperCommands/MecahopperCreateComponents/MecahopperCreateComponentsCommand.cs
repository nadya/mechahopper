using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;
using System.IO;


namespace MecahopperCreateComponents
{
    [
        System.Runtime.InteropServices.Guid("c4c9338a-0e3e-47d2-84f5-221774a02c2e"),
        Rhino.Commands.CommandStyle(Rhino.Commands.Style.ScriptRunner)
    ]
    public class MecahopperCreateComponentsCommand : Command
    {
        string dirPath = Path.GetDirectoryName(typeof(MecahopperCreateComponentsCommand).Assembly.Location);
        string partChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public MecahopperCreateComponentsCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MecahopperCreateComponentsCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "MECHAHOPPER"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            // ---
            // Get units
            UnitSystem docUnits = doc.ModelUnitSystem;
            double scale = Rhino.RhinoMath.UnitScale(docUnits, UnitSystem.Millimeters);
            double inverse_scale = Rhino.RhinoMath.UnitScale(UnitSystem.Millimeters, docUnits);

            Form f = new Form();
            var dialog_rc = f.ShowModal(RhinoEtoApp.MainWindow);
            if (dialog_rc == Eto.Forms.DialogResult.Ok)
            {
                string createThese = "Creating These Elements:";
                createThese += "\n\tLinear Nodes: " + f.LinearQty.ToString();
                createThese += "\n\tRotation Nodes: " + f.RotationQty.ToString();
                RhinoApp.WriteLine(createThese);
                if(f.LinearQty > 0)
                {
                    string linBase = dirPath + "\\Linear_Base.3dm";
                    string linGlide = dirPath + "\\Linear_Glide.3dm";

                    for (int i = 0; i < f.LinearQty; i++)
                    {
                        if (i == 0)
                        {
                            RhinoApp.RunScript("-Insert \"" + dirPath + "\\Linear_Base.3dm\" _Block 0,0,0 _Enter _Enter", true);
                            Rhino.DocObjects.RhinoObject ro = doc.Objects.MostRecentObject();
                            Rhino.DocObjects.InstanceObject instObj = ro as Rhino.DocObjects.InstanceObject;
                            if (instObj != null)
                            {
                                instObj.Attributes.SetUserString("NodeID", partChars.Substring(i, 1));
                                instObj.Attributes.SetUserString("Travel", (300 * inverse_scale).ToString());
                            }
                            RhinoApp.RunScript("-Insert \"" + dirPath + "\\Linear_Glide.3dm\" _Block 0,0,0 _Enter _Enter", true);
                            ro = doc.Objects.MostRecentObject();
                            instObj = ro as Rhino.DocObjects.InstanceObject;
                            if (instObj != null)
                                instObj.Attributes.SetUserString("NodeID", partChars.Substring(i,1));
                        }
                        else
                        {
                            RhinoApp.RunScript("-Insert Linear_Base _Block 0," + (i * 150 * inverse_scale).ToString() + " _Enter _Enter", true);
                            Rhino.DocObjects.RhinoObject ro = doc.Objects.MostRecentObject();
                            Rhino.DocObjects.InstanceObject instObj = ro as Rhino.DocObjects.InstanceObject;
                            if (instObj != null)
                            {
                                instObj.Attributes.SetUserString("NodeID", partChars.Substring(i, 1));
                                instObj.Attributes.SetUserString("Travel", (300 * scale).ToString());
                            }
                            RhinoApp.RunScript("-Insert Linear_Glide _Block 0," + (i * 150 * scale).ToString() + " _Enter _Enter", true);
                            ro = doc.Objects.MostRecentObject();
                            instObj = ro as Rhino.DocObjects.InstanceObject;
                            if (instObj != null)
                                instObj.Attributes.SetUserString("NodeID", partChars.Substring(i, 1));
                        }
                    }
                }
            }
            else
                RhinoApp.WriteLine("did not click OK");
            // ---
            return Result.Success;
        }
    }
}
