using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace MecahopperCreateComponents
{
    public class Node
    {
        public InstanceObject Block { get; set; }
        public Point3d Location { get; set; }
        public Transform XForm { get; set; }
        public GeometryBase Geometry { get; set; }
        public string Id { get; set; }
        public double Travel { get; set; }
        
        public InstanceObject SubBlock { get; set; }
        public Point3d SubLocation { get; set; }
        public Transform SubXForm { get; set; }
        public List<GeometryBase> SubGeo { get; set; }
        public double SubTraveled { get; set; }


        public Node()
        {

        }
    }
}
