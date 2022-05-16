using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.LayerManager;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Windows.ToolPalette;
using Autodesk.AutoCAD.Internal.Windows;
using Autodesk.AutoCAD.Internal.Forms;
using Autodesk.AutoCAD.BoundaryRepresentation;
using Autodesk.AutoCAD.Colors;
[assembly: CommandClass(typeof(threeDTools.DrawPlank))]
namespace threeDTools {
    public class plank {
        double len;
        double wid;
        double thk;
        string material;

        public plank()
        {
            double len = 2440;
            double wid = 1220;
            double thk = 18;
            string material = "plywood";
        }
        public void setSize(double length, double width) {
            len = length;
            wid = width;
        }

        static void Main(string[] args)
        {
            plank plank1 = new plank();
        }
    }

    class DrawPlank
    {
        [CommandMethod("createPlank")]
        public void createPlank()
        {
            Solid3d B = Box(100, 300, 100);
            Move(B, new Point3d(0, 0, 0));
            ToModelSpace(B);
        }
        /// <summary>
        /// 添加实体到模型空间
        /// </summary>
        /// <param name="ent">要添加的实体</param>
        /// <returns>实体ObjectId</returns>
        public static ObjectId ToModelSpace(Entity ent)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId entId;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord modelSpace = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = modelSpace.AppendEntity(ent);
                trans.AddNewlyCreatedDBObject(ent, true);
                trans.Commit();
            }
            return entId;
        }

        /// <summary>
        /// 创建长方体
        /// </summary>
        /// <param name="X">长</param>
        /// <param name="Y">宽</param>
        /// <param name="Z">高</param>
        /// <returns>长方体</returns>
        public static Solid3d Box(double X, double Y, double Z)
        {
            Solid3d Solid = new Solid3d();
            Solid.CreateBox(X, Y, Z);
            return Solid;
        }

        /// <summary>
        /// 以原点为基点，指定目标点移动实体
        /// </summary>
        /// <param name="ent">实体对象</param>
        /// <param name="pt">目标点</param>
        public static void Move(Entity ent, Point3d pt)
        {
            Matrix3d mt = Matrix3d.Displacement(pt - new Point3d());
            ent.TransformBy(mt);
        }
    }
}














