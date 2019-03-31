using System;
using Rhino;
using Rhino.Commands;
using Rhino.Input;

namespace MecahopperCreateComponents
{
    public class CreateConstraint : Command
    {
        static CreateConstraint _instance;
        public CreateConstraint()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CreateConstraint command.</summary>
        public static CreateConstraint Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "MECHA_Constraint"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Pick The First Block
            const Rhino.DocObjects.ObjectType filter = Rhino.DocObjects.ObjectType.InstanceReference;
            Rhino.DocObjects.ObjRef objRef;
            Rhino.DocObjects.InstanceObject primary = null;
            Result r = RhinoGet.GetOneObject("Select a Control Block", false, filter, out objRef);
            if (r == Result.Success)
                primary = objRef.Object() as Rhino.DocObjects.InstanceObject;
            if (primary != null)
                RhinoApp.WriteLine("Primary: " + primary.InstanceDefinition.Name);
            doc.Objects.UnselectAll();
            Rhino.DocObjects.ObjRef objRef2;
            Rhino.DocObjects.InstanceObject secondary = null;
            Result r2 = RhinoGet.GetOneObject("Select a Secondary Block", false, filter, out objRef2);
            if (r2 == Result.Success)
                secondary = objRef2.Object() as Rhino.DocObjects.InstanceObject;
            if (secondary != null)
                RhinoApp.WriteLine("Secondary: " + secondary.InstanceDefinition.Name);
            doc.Objects.UnselectAll();

            if (primary == null || secondary == null)
                return Result.Nothing;

            // Now assign the NodeID of the secondary block to the primary.
            string id = secondary.Attributes.GetUserString("NodeID");
            if (string.IsNullOrEmpty(id))
                return Result.Nothing;

            string constraints = primary.Attributes.GetUserString("Constraints");
            if (string.IsNullOrWhiteSpace(constraints))
                constraints = id;
            else
                constraints += "," + id;


            primary.Attributes.SetUserString("Constraints", constraints);

            return Result.Success;
        }
    }
}