using System;
using System.Collections.Generic;
using System.Linq;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace MecahopperCreateComponents
{
    public class ResetCommand : Command
    {
        static ResetCommand _instance;
        public ResetCommand()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ResetCommand command.</summary>
        public static ResetCommand Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "MECHA_Reset"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Reset the components
            // Find the block Pairs
            List<Node> nodes = new List<Node>();
            RhinoObject[] objects = doc.Objects.FindByObjectType(ObjectType.InstanceReference);
            foreach(var ro in objects)
            {
                InstanceObject inst = ro as InstanceObject;
                if (inst.InstanceDefinition.Name != "Linear_Base")
                    continue;

                string id = inst.Attributes.GetUserString("NodeID");
                if (string.IsNullOrEmpty(id))
                    continue;
                if (!double.TryParse(inst.Attributes.GetUserString("Travel"), out double travel))
                    continue;

                Node n = new Node();
                n.Id = id;
                n.Block = inst;
                n.Location = inst.InsertionPoint;
                n.XForm = inst.InstanceXform;
                n.Travel = travel;

                nodes.Add(n);
            }
            RhinoApp.WriteLine("Found Nodes: " + nodes.Count.ToString());
            foreach (var ro in objects)
            {
                InstanceObject inst = ro as InstanceObject;
                if (inst.InstanceDefinition.Name != "Linear_Glide")
                    continue;

                string id = inst.Attributes.GetUserString("NodeID");
                if (string.IsNullOrEmpty(id))
                    continue;


                int index = nodes.IndexOf(nodes.Where(nd => nd.Id == id).FirstOrDefault());
                if (index < 0)
                    continue;

                Transform xform = Transform.Unset;
                if (inst.InstanceXform.TryGetInverse(out Transform inverse))
                    xform = nodes[index].XForm * inverse;
                else
                    xform = nodes[index].XForm;
                doc.Objects.Transform(inst, xform, true);

                nodes[index].SubBlock = inst;
                nodes[index].SubLocation = inst.InsertionPoint;
                nodes[index].SubXForm = inst.InstanceXform;
                nodes[index].SubTraveled = 0.0;
            }
            doc.Views.Redraw();

            return Result.Success;
        }
    }
}